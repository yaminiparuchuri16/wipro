namespace E_Commerce1.DTOs
{
    public class OrderStatusUpdateDto
    {
        public string Status { get; set; } = "Pending"; // Pending/Paid/Shipped/Cancelled
    }
}
