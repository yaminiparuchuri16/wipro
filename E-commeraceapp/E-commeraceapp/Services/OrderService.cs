using Common.DTOs;
using Common.Interfaces;
using Database.Context;
using Database.Models;
using Microsoft.EntityFrameworkCore;

public class OrderService : IOrderService
{
    private readonly EcommerceDbContext _db;
    public OrderService(EcommerceDbContext db) { _db = db; }

    public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto dto)
    {
        // You need to fetch the price for each product from the database
        var productIds = dto.Items.Select(i => i.ProductId).ToList();
        var products = await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.Price);

        var order = new Order
        {
            UserId = userId,
            // Calculate TotalAmount using the fetched product prices
            TotalAmount = dto.Items.Sum(i => products[i.ProductId] * i.Quantity),
            Status = "Pending",
            ShippingAddress = dto.ShippingAddress,
            CreatedAt = DateTime.UtcNow,
            OrderItems = dto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = products[i.ProductId]
            }).ToList()
        };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
    {
        var orders = await _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return orders.Select(order => new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        });
    }

    public async Task<OrderDto> GetOrderByIdAsync(int userId, int orderId)
    {
        var order = await _db.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId);

        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _db.Orders.Include(o => o.OrderItems).ToListAsync();
        return orders.Select(order => new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        });
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null) return false;
        order.Status = status;
        await _db.SaveChangesAsync();
        return true;
    }
}

// Add this property to your OrderDto class definition
public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } // <-- Add this property
}
