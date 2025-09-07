using System.Security.Claims;
using E_Commerce1.Data;
using E_Commerce1.Models;

namespace E_Commerce1.Middleware
{
    public class AdminActionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public AdminActionLoggingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            if (!path.StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var role = context.User.FindFirstValue(ClaimTypes.Role);
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            await _next(context); // execute controller first

            var email = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var db = context.RequestServices.GetRequiredService<ECommerceDb>();
            var admin = db.Users.FirstOrDefault(u => u.Email == email);
            if (admin == null) return;

            string? target = null;
            if (context.Request.RouteValues.TryGetValue("id", out var idVal) && idVal != null)
                target = $"id:{idVal}";
            else if (context.Request.RouteValues.TryGetValue("orderId", out var orderId) && orderId != null)
                target = $"orderId:{orderId}";

            db.AdminLogs.Add(new AdminLog
            {
                AdminId = admin.Id,
                Action = $"{context.Request.Method} {path}",
                Target = target
            });
            await db.SaveChangesAsync();
        }
    }
}
