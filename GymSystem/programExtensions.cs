using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Data.DataSeeds;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystem
{
    public static class programExtensions
    {
        public static async Task MigarateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var configurations = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();




            var Pending = await dbcontext.Database.GetPendingMigrationsAsync();
            if (Pending.Any())
            {
                logger.LogInformation($"Apply {Pending.Count()} pending migrations");
                await dbcontext.Database.MigrateAsync(); 

            }
            var SeedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeed.SeedAsync(dbcontext, SeedPath, logger);
            await IdentityDataSeeding.SeedAsync(RoleManager , UserManager, logger); 
        }

    }
}
