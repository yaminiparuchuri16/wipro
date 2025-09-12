
using Ecommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Review> Reviews => Set<Review>();
    }

    public static class SeedData
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Users.Any())
            {
                db.Users.Add(new User { Email = "admin@shop.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Role = "Admin" });
                db.Users.Add(new User { Email = "user@shop.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"), Role = "User" });
            }
            if (!db.Products.Any())
            {
                db.Products.AddRange(new List<Product> {
                    new Product { Name = "Laptop", Description = "14 inch laptop", Price = 899.99m, Category="Electronics", ImageUrl="https://m.media-amazon.com/images/I/61SHFVKs+AL._SX679_.jpg" },
                    new Product { Name = "Handbag", Description = "Leather Bag", Price = 199.50m, Category="Electronics", ImageUrl="https://m.media-amazon.com/images/I/71W45geCFuL._SY695_.jpg" },
                    new Product { Name = "Sneakers", Description = "Running shoes", Price = 120.00m, Category="Apparel", ImageUrl="https://m.media-amazon.com/images/I/618Ylh6YkzL._SY695_.jpg" },
                    new Product { Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99m, Category="Electronics", ImageUrl="https://images.unsplash.com/photo-1511707171634-5f897ff02aa9" },
                    new Product { Name = "Backpack", Description = "Waterproof travel backpack", Price = 89.99m, Category="Accessories", ImageUrl="https://m.media-amazon.com/images/I/51c9UnffaFL._SX679_.jpg" },
                    new Product { Name = "Watch", Description = "Stylish wrist watch", Price = 249.99m, Category="Accessories", ImageUrl="https://m.media-amazon.com/images/I/81cuxixj2-L._SL1500_.jpg" },
                    new Product { Name = "Coffee Maker", Description = "Automatic coffee machine", Price = 149.99m, Category="Home Appliances", ImageUrl="https://m.media-amazon.com/images/I/61dPzhIhG-L._SL1500_.jpg" },
                    new Product { Name = "Desk Chair", Description = "Ergonomic office chair", Price = 179.99m, Category="Furniture", ImageUrl="https://images.unsplash.com/photo-1515378791036-0648a3ef77b2" },
                    new Product { Name = "Bluetooth Speaker", Description = "Portable wireless speaker", Price = 59.99m, Category="Electronics", ImageUrl="https://m.media-amazon.com/images/I/71o6CU8MqVL._SX679_.jpg" },
                    new Product { Name = "Fitness Tracker", Description = "Track your daily activity", Price = 99.99m, Category="Electronics", ImageUrl="https://m.media-amazon.com/images/I/51stTW4u0CL._SX679_.jpg" },
                    new Product { Name = "Sunglasses", Description = "UV protection sunglasses", Price = 49.99m, Category="Accessories", ImageUrl="https://m.media-amazon.com/images/I/41hB7sS0J4L._SX679_.jpg" },
                    new Product { Name = "Water Bottle", Description = "Insulated stainless steel bottle", Price = 29.99m, Category="Accessories", ImageUrl="https://m.media-amazon.com/images/I/514SxKVCptL._SL1500_.jpg" },
                    new Product { Name = "Gaming Mouse", Description = "High precision gaming mouse", Price = 39.99m, Category="Electronics", ImageUrl="https://m.media-amazon.com/images/I/71PfyTreJIL._SL1500_.jpg" },
                    new Product { Name = "Yoga Mat", Description = "Non-slip yoga mat", Price = 25.99m, Category="Fitness", ImageUrl="https://m.media-amazon.com/images/I/61DEict44KL._SL1500_.jpg" }
                });
            }
            db.SaveChanges();
        }
    }
}
