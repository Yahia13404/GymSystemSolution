using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Contexts
{
    public class GymDbContext : DbContext
    {
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;database=GymDb;trusted_connection=True;trustServerCertificate=True");
        }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new configuration.PlanConfiguration());
        }
        public DbSet<Plan> Plans { get; set; }
    }
}
