namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string Username { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Role { get; set; } = "";
    public bool PromptPasswordReset { get; set; } = false;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
