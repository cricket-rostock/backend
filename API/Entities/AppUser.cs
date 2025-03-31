using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    [Key]
    public Guid Id { get; set; }
    public required string Username { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string GivenName { get; set; } = "";
    public string Role { get; set; } = "";
    public bool PromptPasswordReset { get; set; } = false;
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}
