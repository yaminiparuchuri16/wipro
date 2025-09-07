namespace E_Commerce1.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }    // validate in code
        public int ProductId { get; set; }  // validate in code
        public int Quantity { get; set; }
        public int Price { get; set; }      // copy product price at time of order
    }
}
