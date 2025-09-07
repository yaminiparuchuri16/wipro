using System.ComponentModel.DataAnnotations;

namespace Role_Based_Product_Management.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;


        [EmailAddress]
        public string? Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [Required]
        public string Role { get; set; } = "Manager"; // Admin or Manager


        public string? FullName { get; set; }
    }
}
