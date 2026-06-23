
using GymSystem.DAL.Data.Configurations;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Contexts
{
    public class GymDbContext: IdentityDbContext<ApplicationUser>
    {
       

        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;database=GymDb;trusted_connection=True;trustServerCertificate=True");
        //}
        public GymDbContext(DbContextOptions<GymDbContext> options):base(options) 
        {
            
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Session>()
                .ToTable("Session");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            
        }



        public DbSet<Plan> Plans { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Booking> Booking { get; set; }
    }
}
