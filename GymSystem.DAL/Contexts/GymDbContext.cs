using GymSystem.DAL.Configurations;
using GymSystem.DAL.Entities;
using GymSystemG03.DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Contexts
{
    public class GymDbContext: DbContext
    {
        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;database=GymDb;trusted_connection=True;trustServerCertificate=True");
        //}
        public GymDbContext(DbContextOptions<GymDbContext> options):base(options) 
        {
            
        }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfigurations());
            modelBuilder.ApplyConfiguration<Member>(new MemberConfigurations());
        }
        
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Member> Members { get; set; }


    }
}
