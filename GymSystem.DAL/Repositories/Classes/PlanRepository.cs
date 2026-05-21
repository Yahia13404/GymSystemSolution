using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;
        public PlanRepository(GymDbContext _dpContext)
        {
            dbContext = _dpContext;
        }
        public async Task<IEnumerable<Plan>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var Plans = isTracked ? dbContext.Plans : dbContext.Plans.AsNoTracking();
            return await Plans.ToListAsync();
        }
        public async Task<Plan?> GetById(int id, CancellationToken ct = default)
        {
            var Plan = await dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id, ct);
            return Plan;
        }
        public void Add(Plan plan)
        {
            dbContext.Add(plan);
        }
        public void Update(Plan plan)
        {
            dbContext.Update(plan);
        }
        public void Delete(int id)
        {
            var Plan = dbContext.Plans.FirstOrDefault(p => p.Id == id);
            if (Plan is not null)
            {
                dbContext.Remove(Plan);
            }
        }
        public async Task<int> ComoleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
