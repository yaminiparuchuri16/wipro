
using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] string? category, [FromQuery] string? sort)
        {
            var query = _db.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(p => p.Name.Contains(q));
            if (!string.IsNullOrWhiteSpace(category)) query = query.Where(p => p.Category == category);
            if (sort == "price_asc") query = query.OrderBy(p => p.Price);
            if (sort == "price_desc") query = query.OrderByDescending(p => p.Price);
            var list = await query.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _db.Products.Include(p => p.Reviews).FirstOrDefaultAsync(p => p.Id == id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Product updated)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            p.Name = updated.Name; p.Description = updated.Description; p.Price = updated.Price;
            p.Category = updated.Category; p.ImageUrl = updated.ImageUrl;
            await _db.SaveChangesAsync();
            return Ok(p);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
