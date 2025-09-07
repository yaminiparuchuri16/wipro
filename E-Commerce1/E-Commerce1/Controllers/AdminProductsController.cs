using E_Commerce1.Data;
using E_Commerce1.DTOs;
using E_Commerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce1.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminProductsController : ControllerBase
    {
        private readonly ECommerceDb _db;
        public AdminProductsController(ECommerceDb db) { _db = db; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var p = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                IsActive = true
            };
            _db.Products.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _db.Products.FindAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();

            p.Name = dto.Name;
            p.Description = dto.Description;
            p.Price = dto.Price;
            p.Category = dto.Category;
            p.ImageUrl = dto.ImageUrl;
            if (dto.IsActive.HasValue) p.IsActive = dto.IsActive.Value;

            await _db.SaveChangesAsync();
            return Ok(p);
        }

        [HttpPatch("{id:int}/category")]
        public async Task<IActionResult> SetCategory(int id, [FromBody] SetCategoryDto dto)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            p.Category = dto.Category;
            await _db.SaveChangesAsync();
            return Ok(new { message = "Category updated" });
        }

        [HttpDelete("{id:int}")]
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
