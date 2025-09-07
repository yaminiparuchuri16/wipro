namespace E_Commerce1.DTOs
{
    public class CartAddDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}