namespace E_Commerce1.DTOs
{
    public class ProductUpdateDto : ProductCreateDto
    {
        public bool? IsActive { get; set; }
    }
}