using System.ComponentModel.DataAnnotations.Schema;
namespace E_Commerce1.Models
{
    public class PaymentRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; } = "Mock";
        
    }
}
