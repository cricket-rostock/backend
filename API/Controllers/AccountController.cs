using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _dbContext;

        public AccountController(DataContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await CheckIfUserExits(registerDto.Username))
            {
                return BadRequest("User already exists");
            }
            else
            {
                using var hmac = new HMACSHA512();
                var user = new AppUser
                {
                    Id = registerDto.Id,
                    Username = registerDto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
        }

        private async Task<bool> CheckIfUserExits(string username)
        {
            return await _dbContext.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
        }

        [HttpPost("RemoveUser/{id}")]
        public async Task<IActionResult> RemoveUserByIdAsync(int id)
        {
            var userToRemove = await this._dbContext.Users.FindAsync(id); // Find the user with the given ID asynchronously
            if (userToRemove != null)
            {
                this._dbContext.Users.Remove(userToRemove); // Remove the user
                await this._dbContext.SaveChangesAsync(); // Save changes to the database asynchronously
                return Ok($"User {userToRemove.Username} with ID {id} has been removed.");
            }
            else
            {
                return NotFound($"User with ID {id} not found.");
            }
        }
    }
}
