namespace E_Commerce1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; } = "";
        public int Total { get; set; }
        public string Status { get; set; } = "Pending";  // Pending/Paid/Shipped/Cancelled
        
    }
}
