namespace E_Commerce1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";   // plain-text as per your request
        public string Role { get; set; } = "User";   // "Admin" or "User"
        public bool IsActive { get; set; } = true;
    }
}
