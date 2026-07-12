using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip>, IMemberShipRepository 
    {
        private readonly GymDbContext _dbContext;

        public MemberShipRepository(GymDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
      
        public async Task<IEnumerable<MemberShip>> GetMemberShipsWihPlansAndMembersAsync(CancellationToken ct, Expression<Func<MemberShip, bool>>? filter = null)
        {
            var query = _dbContext.MemberShips.Include(m => m.Plan).Include(m => m.Member).AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync(ct);
        }
    }
}
