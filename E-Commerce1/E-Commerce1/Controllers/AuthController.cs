using E_Commerce1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _auth;
        public AuthController(AuthenticationService auth) { _auth = auth; }

        public class RegisterReq { public string Email { get; set; } = ""; public string Password { get; set; } = ""; public string? Role { get; set; } }
        public class LoginReq { public string Email { get; set; } = ""; public string Password { get; set; } = ""; }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterReq req)
        {
            var result = await _auth.RegisterUser(req.Email, req.Password, req.Role ?? "User");
            if (!result.Success) return BadRequest(result.Message);
            return Ok(new { result.User!.Id, result.User.Email, result.User.Role });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginReq req)
        {
            var user = await _auth.ValidateUser(req.Email, req.Password);
            if (user == null) return Unauthorized("Invalid credentials.");

            var jwt = _auth.GenerateJwt(user);
            return Ok(new { token = jwt, user = new { user.Id, user.Email, user.Role } });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            // Because NameClaimType = NameIdentifier, this is your email
            var email = User.Identity?.Name ?? "";
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            return Ok(new { email, role });
        }
    }
}
