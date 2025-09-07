using Microsoft.EntityFrameworkCore;

namespace EmployeeServiceCrud.Models
{
    public class EFCoreDbContext:DbContext
    {
        //Constructor calling the Base DbContext Class Constructor
        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options) : base(options)
        {
        }
        //OnConfiguring() method is used to select and configure the data source
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employe>().ToTable("Employ");
        }

        public DbSet<Employe> Employees { get; set; }
    }
}