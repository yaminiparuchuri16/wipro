
using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPaymentService _payment;
        public OrdersController(AppDbContext db, IPaymentService payment) { _db = db; _payment = payment; }

        private int? TryGetUserId()
        {
            var val = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(val) ? null : int.Parse(val);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMyOrders()
        {
            var uid = TryGetUserId();
            if (uid == null)
            {
                // If not authenticated, return all orders (or you can return an empty list or a default response)
                var allOrders = await _db.Orders.Include(o => o.Items).ToListAsync();
                return Ok(allOrders);
            }
            var orders = await _db.Orders.Include(o => o.Items).Where(o => o.UserId == uid.Value).ToListAsync();
            return Ok(orders);
        }

        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var uid = TryGetUserId()!.Value;
            var cart = await _db.CartItems.Include(c=>c.Product).Where(c => c.UserId == uid).ToListAsync();
            if (!cart.Any()) return BadRequest("Cart is empty.");

            var order = new Order { UserId = uid, Items = new List<OrderItem>() };
            decimal total = 0;
            foreach (var c in cart)
            {
                order.Items.Add(new OrderItem { ProductId = c.ProductId, Quantity = c.Quantity, UnitPrice = c.Product!.Price });
                total += c.Product!.Price * c.Quantity;
            }
            order.Total = total;
            order.Status = "Pending";
            _db.Orders.Add(order);
            _db.CartItems.RemoveRange(cart);
            await _db.SaveChangesAsync();

            var paymentId = _payment.ProcessPayment(total);
            order.Status = "Paid";
            await _db.SaveChangesAsync();
            return Ok(new { orderId = order.Id, paymentId, total });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _db.Orders.Include(o => o.Items).ToListAsync();
            return Ok(orders);
        }

        [HttpPost("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            order.Status = status;
            await _db.SaveChangesAsync();
            return Ok(order);
        }
    }
}
