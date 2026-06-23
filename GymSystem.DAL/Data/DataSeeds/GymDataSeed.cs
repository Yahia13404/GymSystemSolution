using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeds
{
    public static class GymDataSeed
    {
        public static async Task SeedAsync(GymDbContext dbContext, string seedFilePath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await dbContext.Plans.AnyAsync(ct))
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json", seedFilePath);
                    if(Plans.Count > 0)
                    {
                        dbContext.Plans.AddRange(Plans);
                        logger.LogInformation($"seeded {Plans.Count} plans"); 
                    }
                }
                if(dbContext.ChangeTracker.HasChanges()) 
                    await dbContext.SaveChangesAsync(ct);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gym Data seeding failed");
            }
        }
        private static List<T> LoadDataFromJsonFile<T>(string fileName, string folderPath)
        {
            var FilePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException($"seed data file not found : {FilePath}");

            }
            var Data = File.ReadAllText(FilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options
             .Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(Data, options) ?? [] ;

        }
    }
}