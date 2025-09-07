using System.ComponentModel.DataAnnotations.Schema;
namespace E_Commerce1.Models
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public int PaymentRequestId { get; set; }
        public bool Success { get; set; }
        public string TransactionId { get; set; } = "";
        
    }
}
