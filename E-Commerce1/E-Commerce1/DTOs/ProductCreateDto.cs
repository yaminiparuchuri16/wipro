namespace E_Commerce1.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
    }
}