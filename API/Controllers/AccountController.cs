using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseAPIController
{
    private readonly DataContext _dbContext;

    public AccountController(DataContext dbContext) => this._dbContext = dbContext;

    [HttpPost("Login")]
    public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync<AppUser>(
            x => x.Username == loginDto.Username.ToLower()
        );
        if (user == null)
            return Unauthorized("Invalid username");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
        }
        return user;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        if (await UserExits(registerDto.Username))
        {
            return BadRequest("User already exists");
        }
        else
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username.ToLower(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                GivenName = $"{registerDto.FirstName} {registerDto.LastName}",
                PromptPasswordReset = true,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }

    private async Task<bool> UserExits(string username)
    {
        return await _dbContext.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
    }
}
