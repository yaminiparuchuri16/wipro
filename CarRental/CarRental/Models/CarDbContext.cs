using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarRental.Models
{
    public class CarDbContext : DbContext
    {
        public CarDbContext() { }

        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options)
        {
        }

        //OnConfiguring() method is used to select and configure the data source
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicles>().ToTable("Vehicles");
            modelBuilder.Entity<Customers>().ToTable("Customers");
            modelBuilder.Entity<User1>().ToTable("User1");
        }

        public DbSet<Vehicles> Vehicles { get; set; }
        public DbSet<Customers> Customers { get; set; }

        public DbSet<User1> User1 { get; set; }


    }
}
