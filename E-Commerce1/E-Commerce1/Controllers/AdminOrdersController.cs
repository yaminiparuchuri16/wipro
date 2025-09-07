using E_Commerce1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce1.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly ECommerceDb _db;
        public AdminOrdersController(ECommerceDb db) { _db = db; }

        // GET /api/admin/orders?status=Pending
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status = null)
        {
            var q = _db.Orders.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(o => o.Status == status);

            var orders = await q
                .OrderByDescending(o => o.Id)
                .Select(o => new
                {
                    id = o.Id,
                    email = o.Email,
                    total = o.Total,
                    status = o.Status
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET /api/admin/orders/123
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetDetails(int orderId)
        {
            var order = await _db.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            var items = await _db.OrderItems
                .AsNoTracking()
                .Where(i => i.OrderId == orderId)
                .OrderBy(i => i.Id)
                .Select(i => new
                {
                    productId = i.ProductId,
                    quantity = i.Quantity,
                    price = i.Price
                })
                .ToListAsync();

            return Ok(new
            {
                id = order.Id,
                email = order.Email,
                total = order.Total,
                status = order.Status,
                items
            });
        }

        public class OrderStatusUpdateDto
        {
            public string Status { get; set; } = "Pending";
        }

        // PUT /api/admin/orders/123/status
        [HttpPut("{orderId:int}/status")]
        public async Task<IActionResult> PutStatus(int orderId, [FromBody] OrderStatusUpdateDto dto)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.Status = dto.Status;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Order status updated", orderId, status = order.Status });
        }

        // PATCH /api/admin/orders/123/status  (alias for convenience)
        [HttpPatch("{orderId:int}/status")]
        public Task<IActionResult> PatchStatus(int orderId, [FromBody] OrderStatusUpdateDto dto)
            => PutStatus(orderId, dto);
    }
}
