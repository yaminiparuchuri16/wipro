namespace E_Commerce1.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }     // no FK constraint in DB: validate in code
        public int ProductId { get; set; }  // validate in code
        public int Quantity { get; set; }
    }
}
