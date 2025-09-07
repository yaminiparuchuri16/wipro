using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce1.Data;
using E_Commerce1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce1.Services
{
    public class AuthenticationService
    {
        private readonly ECommerceDb _db;
        private readonly IConfiguration _cfg;

        public AuthenticationService(ECommerceDb db, IConfiguration cfg)
        {
            _db = db; _cfg = cfg;
        }

        public async Task<(bool Success, string Message, User? User)> RegisterUser(string email, string password, string role = "User")
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, "Email and password are required.", null);

            email = email.Trim();
            if (await _db.Users.AnyAsync(u => u.Email == email))
                return (false, "Email already registered.", null);

            var user = new User { Email = email, Password = password, Role = string.IsNullOrWhiteSpace(role) ? "User" : role.Trim() };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return (true, "Registered", user);
        }

        public async Task<User?> ValidateUser(string email, string password) =>
            await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

        public string GenerateJwt(User user)
        {
            // MUST match Program.cs + appsettings.json
            var issuer = _cfg["Jwt:Issuer"]!;
            var audience = _cfg["Jwt:Audience"]!;
            var keyStr = _cfg["Jwt:Key"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // (Optional convenience) so controllers can call a single method
        public async Task<string?> LoginUser(string email, string password)
        {
            var user = await ValidateUser(email, password);
            if (user == null) return null;
            return GenerateJwt(user);
        }
    }
}
