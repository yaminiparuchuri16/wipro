using E_Commerce1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ECommerceDb _db;
        public UsersController(ECommerceDb db) => _db = db;

        // GET /api/users/by-email/{email}
        [HttpGet("by-email/{email}")]
        [Authorize] // or AllowAnonymous if you want to fetch before login
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Email required.");
            // email comes URL-encoded; EF compare is case-insensitive by default in SQL Server
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();
            return Ok(new { id = user.Id, email = user.Email, role = user.Role });
        }
    }
}
