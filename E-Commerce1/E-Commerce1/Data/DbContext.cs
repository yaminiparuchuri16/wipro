using Microsoft.EntityFrameworkCore;
using E_Commerce1.Models;

namespace E_Commerce1.Data
{
    public class ECommerceDb : DbContext
    {
        public ECommerceDb(DbContextOptions<ECommerceDb> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<PaymentRequest> PaymentRequests => Set<PaymentRequest>();
        public DbSet<PaymentResponse> PaymentResponses => Set<PaymentResponse>();
        public DbSet<AdminLog> AdminLogs => Set<AdminLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<CartItem>().ToTable("CartItems");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<PaymentRequest>().ToTable("PaymentRequests");
            modelBuilder.Entity<PaymentResponse>().ToTable("PaymentResponses");
            modelBuilder.Entity<AdminLog>().ToTable("AdminLogs");

            // explicit identity PK config (safety)
            modelBuilder.Entity<PaymentRequest>().HasKey(p => p.Id);
            modelBuilder.Entity<PaymentRequest>().Property(p => p.Id).UseIdentityColumn();
            modelBuilder.Entity<PaymentResponse>().HasKey(p => p.Id);
            modelBuilder.Entity<PaymentResponse>().Property(p => p.Id).UseIdentityColumn();
            modelBuilder.Entity<AdminLog>().HasKey(a => a.Id);
            modelBuilder.Entity<AdminLog>().Property(a => a.Id).UseIdentityColumn();
        }
    }
}
