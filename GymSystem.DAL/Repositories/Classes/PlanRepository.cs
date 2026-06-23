using GymSystem.DAL.Data.Contexts;
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
    public class PlanRepository : GenericRepository<Plan> , IPlanRepository
    {
        private readonly GymDbContext dbContext;
        public PlanRepository(GymDbContext _dpContext):base(_dpContext)
        {
            dbContext = _dpContext;
        }
       
    }
}
