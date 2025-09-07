using Common.DTOs;
using Common.Interfaces;
using Database.Context;
using Database.Models;
using Microsoft.EntityFrameworkCore; // Needed for EF Core extension methods

public class ProductService : IProductService
{
    private readonly EcommerceDbContext _db;
    public ProductService(EcommerceDbContext db) { _db = db; }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _db.Products.AsNoTracking().ToListAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            Category = p.Category,
            ImageUrl = p.ImageUrl
        });
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return null;
        return new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            Category = p.Category,
            ImageUrl = p.ImageUrl
        };
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto dto
    )
    {
        var p = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            Category = dto.Category,
            ImageUrl = dto.ImageUrl
        };
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        dto.Id = p.Id;
        return dto;
    }

    public async Task<bool> UpdateProductAsync(int id, ProductDto dto)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return false;
        p.Name = dto.Name;
        p.Description = dto.Description;
        p.Price = dto.Price;
        p.Stock = dto.Stock;
        p.Category = dto.Category;
        p.ImageUrl = dto.ImageUrl;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return false;
        _db.Products.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
    
    public required string ProductName { get; set; }

    public static PaymentService CreatePaymentService(string stripeSecretKey)
    {
        return new PaymentService(stripeSecretKey);
    }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}