namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
