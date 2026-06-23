using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeds
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager ,
           ILogger logger )
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return;
                if (!HasRoles) 
                {
                    var Roles = new List<IdentityRole>
                    {
                        new IdentityRole() {Name = "SuperAdmin"},
                        new IdentityRole() {Name = "Admin"}
                    };
                    foreach (var roleName in Roles.Select(R=>R.Name)) 
                    {
                        if (!await roleManager.RoleExistsAsync(roleName)) 
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                            if (!roleResult.Succeeded)
                                logger.LogError("Failed To Create Role.."); 

                        }
                    }
                }
                if (!HasUsers) 
                {
                    var MainUser = new ApplicationUser()
                    {
                        FirstName = "Yahia",
                        LastName = "Mohamed",
                        UserName = "Yahia",
                        Email = "yehia@gmail.com",
                        PhoneNumber = "01234567890",
                    };
                    var UserResult =  await userManager.CreateAsync(MainUser , "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainUser, "SuperAdmin");
                    if (!UserResult.Succeeded)
                    {
                        logger.LogError("Failed To Seed Users");

                    }



                }
                return;
            }
            catch(Exception ex ) { logger.LogError("Failed To seed Identity Data");
                throw;
            }

        }
    }
}
