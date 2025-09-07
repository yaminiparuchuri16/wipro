using System.ComponentModel.DataAnnotations;

namespace Role_Based_Product_Management.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        public bool RememberMe { get; set; }
    }
}
