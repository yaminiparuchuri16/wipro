using Microsoft.AspNetCore.Identity;

namespace Role_Based_Product_Management.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
