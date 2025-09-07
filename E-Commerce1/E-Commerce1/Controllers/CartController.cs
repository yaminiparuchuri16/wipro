using E_Commerce1.Data;
using E_Commerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ECommerceDb _db;
        public CartController(ECommerceDb db) { _db = db; }

        public class AddReq { public int UserId { get; set; } public int ProductId { get; set; } public int Quantity { get; set; } = 1; }
        public class CheckoutReq { public int UserId { get; set; } public string? Email { get; set; } }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var items = await _db.CartItems
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Id)
                .ToListAsync();
            return Ok(items);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddReq req)
        {
            if (req.Quantity <= 0) req.Quantity = 1;

            if (!await _db.Users.AnyAsync(u => u.Id == req.UserId))
                return BadRequest("User not found.");
            if (!await _db.Products.AnyAsync(p => p.Id == req.ProductId))
                return BadRequest("Product not found.");

            var existing = await _db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == req.UserId && c.ProductId == req.ProductId);

            if (existing is null)
            {
                _db.CartItems.Add(new CartItem { UserId = req.UserId, ProductId = req.ProductId, Quantity = req.Quantity });
            }
            else
            {
                existing.Quantity += req.Quantity;
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Added to cart." });
        }

        [HttpDelete("{cartItemId:int}")]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item == null) return NotFound();
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutReq req)
        {
            var user = await _db.Users.FindAsync(req.UserId);
            if (user == null) return BadRequest("User not found.");

            var cart = await _db.CartItems.Where(c => c.UserId == req.UserId).ToListAsync();
            if (cart.Count == 0) return BadRequest("Cart is empty.");

            var productIds = cart.Select(c => c.ProductId).Distinct().ToList();
            var products = await _db.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
            var priceById = products.ToDictionary(p => p.Id, p => p.Price);

            int total = 0;
            foreach (var c in cart)
            {
                if (!priceById.ContainsKey(c.ProductId))
                    return BadRequest($"Product {c.ProductId} no longer available.");
                total += priceById[c.ProductId] * c.Quantity;
            }

            var order = new Order
            {
                UserId = user.Id,
                Email = string.IsNullOrWhiteSpace(req.Email) ? user.Email : req.Email!,
                Total = total,
                Status = "Pending"
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            var items = cart.Select(c => new OrderItem
            {
                OrderId = order.Id,
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = priceById[c.ProductId]
            }).ToList();

            _db.OrderItems.AddRange(items);
            _db.CartItems.RemoveRange(cart);
            await _db.SaveChangesAsync();

            return Ok(new { orderId = order.Id, total = order.Total, status = order.Status });
        }
    }
}
