using System.ComponentModel.DataAnnotations;

namespace Role_Based_Product_Management.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(128)] public string Name { get; set; } = "";
        [Range(0.01, 1_000_000)] public decimal Price { get; set; }
    }
}
