using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseAPIController
    {
        private readonly DataContext _dbContext;

        public UsersController(DataContext dbContext) => this._dbContext = dbContext;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await this._dbContext.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")] // /api/users/2
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            try
            {
                AppUser user = await this._dbContext.Users.FindAsync(id);
                return user;
            }
            catch
            {
                return NotFound($"User with ID {id} not found");
            }
        }

        [HttpPost("RemoveUser/{id}")]
        public async Task<IActionResult> RemoveUserByIdAsync(Guid id)
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
