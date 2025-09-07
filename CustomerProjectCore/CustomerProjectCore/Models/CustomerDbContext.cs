using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CustomerProjectCore.Models
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext() { }

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        //OnConfiguring() method is used to select and configure the data source
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Wallet>().ToTable("Wallet");

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

    }
}