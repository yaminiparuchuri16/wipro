
using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CartController(AppDbContext db) { _db = db; }

        private int GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out var id) && id > 0)
                return id;
            // No auth: use first user in DB
            var user = _db.Users.FirstOrDefault();
            return user?.Id ?? 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var uid = GetUserId();
            var items = await _db.CartItems.Include(c=>c.Product).Where(c => c.UserId == uid).ToListAsync();
            return Ok(items);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] Ecommerce.Api.Dtos.AddCartItemDto dto)
        {
            var userId = GetUserId();
            var existing = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == dto.ProductId);
            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
            }
            else
            {
                var item = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                _db.CartItems.Add(item);
            }
            await _db.SaveChangesAsync();
            return Ok(new { message = "Added" });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveItem([FromBody] CartItem item)
        {
            var uid = GetUserId();
            var existing = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == uid && c.ProductId == item.ProductId);
            if (existing == null) return NotFound();
            _db.CartItems.Remove(existing);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Removed" });
        }
    }
}
