namespace E_Commerce1.DTOs
{
    public class PaymentResponseDto
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; } = "";
    }
}