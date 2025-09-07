using E_Commerce1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce1.Controllers
{
    [ApiController]
    [Route("api/admin/reports")]
    [Authorize(Policy = "AdminOnly")]
    public class ReportsController : ControllerBase
    {
        private readonly ECommerceDb _db;
        public ReportsController(ECommerceDb db) { _db = db; }

        // KPIs: total revenue (Paid), total orders, total items sold, AOV (Paid only)
        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {
            var paidOrders = _db.Orders.Where(o => o.Status == "Paid");

            var totalRevenue = await paidOrders.SumAsync(o => (int?)o.Total) ?? 0;
            var totalOrders = await _db.Orders.CountAsync();
            var totalItems = await _db.OrderItems.SumAsync(i => (int?)i.Quantity) ?? 0;

            var paidCount = await paidOrders.CountAsync();
            var averageOrderValue = paidCount > 0 ? totalRevenue / paidCount : 0;

            return Ok(new { totalRevenue, totalOrders, totalItems, averageOrderValue });
        }

        // ✅ Fixed: JOIN first, then GROUP BY (EF Core can translate this)
        [HttpGet("top-products")]
        public async Task<IActionResult> TopProducts([FromQuery] int take = 5)
        {
            var q =
                from oi in _db.OrderItems
                join p in _db.Products on oi.ProductId equals p.Id
                group new { oi, p } by new { oi.ProductId, p.Name } into g
                orderby g.Sum(x => x.oi.Quantity) descending
                select new
                {
                    productId = g.Key.ProductId,
                    name = g.Key.Name,
                    qty = g.Sum(x => x.oi.Quantity),
                    revenue = g.Sum(x => x.oi.Price * x.oi.Quantity)
                };

            var result = await q.Take(take).ToListAsync();
            return Ok(result);
        }

        // Orders grouped by status (counts + paid revenue)
        [HttpGet("orders-by-status")]
        public async Task<IActionResult> OrdersByStatus()
        {
            var result = await _db.Orders
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    status = g.Key,
                    count = g.Count(),
                    revenue = g.Where(o => o.Status == "Paid").Sum(o => o.Total)
                })
                .OrderByDescending(x => x.count)
                .ToListAsync();

            return Ok(result);
        }

        // Sales by product category
        [HttpGet("sales-by-category")]
        public async Task<IActionResult> SalesByCategory()
        {
            var q =
                from oi in _db.OrderItems
                join p in _db.Products on oi.ProductId equals p.Id
                group new { oi, p } by p.Category into g
                select new
                {
                    category = g.Key ?? "Uncategorized",
                    qty = g.Sum(x => x.oi.Quantity),
                    revenue = g.Sum(x => x.oi.Price * x.oi.Quantity)
                };

            var result = await q.OrderByDescending(x => x.revenue).ToListAsync();
            return Ok(result);
        }

        // Top N customers by paid revenue
        [HttpGet("top-customers")]
        public async Task<IActionResult> TopCustomers([FromQuery] int take = 5)
        {
            var q =
                from o in _db.Orders
                where o.Status == "Paid"
                group o by new { o.UserId, o.Email } into g
                orderby g.Sum(x => x.Total) descending
                select new
                {
                    userId = g.Key.UserId,
                    email = g.Key.Email,
                    orders = g.Count(),
                    revenue = g.Sum(x => x.Total)
                };

            var result = await q.Take(take).ToListAsync();
            return Ok(result);
        }
    }
}
