using System.ComponentModel.DataAnnotations;

namespace Role_Based_Product_Management.Models
{
    public class Product
    {
        public int Id { get; set; }


        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;


        // We will store encrypted price string and expose a transient decimal for UI binding
        [Required]
        public string ProtectedPrice { get; set; } = string.Empty;
    }
}
