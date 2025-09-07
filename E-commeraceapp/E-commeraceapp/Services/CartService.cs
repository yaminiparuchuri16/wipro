using Common.DTOs;
using Common.Interfaces;
using Database.Context;
using Database.Models;
using Microsoft.EntityFrameworkCore;

public class CartService : ICartService
{
    private readonly EcommerceDbContext _db;
    public CartService(EcommerceDbContext db) { _db = db; }

    public async Task<CartDto> GetCartByUserIdAsync(int userId)
    {
        var cart = await _db.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return null;

        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.CartItems.Select(i => new CartItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    public async Task<bool> AddToCartAsync(int userId, AddToCartDto dto)
    {
        // Minimal implementation
        var cart = await _db.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return false;
        cart.CartItems.Add(new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCartItemAsync(int userId, int productId, int quantity)
    {
        var cart = await _db.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return false;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return false;
        item.Quantity = quantity;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromCartAsync(int userId, int productId)
    {
        var cart = await _db.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return false;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return false;
        cart.CartItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync(int userId)
    {
        var cart = await _db.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return false;
        cart.CartItems.Clear();
        await _db.SaveChangesAsync();
        return true;
    }
}