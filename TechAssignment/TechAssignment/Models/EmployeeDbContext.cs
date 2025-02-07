using Microsoft.EntityFrameworkCore;

namespace EmployeeRegisterAPI.Models
{
    public class EmployeeDbContext : DbContext
    {
        // Constructor for dependency injection
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        // DbSet representing the Employee table in the database
        public DbSet<EmployeeModel> Employees { get; set; }

        // Model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeModel>(entity =>
            {
                entity.HasKey(e => e.EmployeeID); // Explicitly set primary key
                entity.Property(e => e.EmployeeName).IsRequired();
                entity.Property(e => e.Occupation).IsRequired();
            });
        }

    }
}
