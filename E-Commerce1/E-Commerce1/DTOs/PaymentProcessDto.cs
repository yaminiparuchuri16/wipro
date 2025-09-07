namespace E_Commerce1.DTOs
{
    public class PaymentProcessDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; } = "Mock";
    }
}