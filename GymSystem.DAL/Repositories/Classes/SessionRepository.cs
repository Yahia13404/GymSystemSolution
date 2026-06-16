using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;
        public SessionRepository(GymDbContext dbContext) :base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            var sessions = dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);
            return await sessions.ToListAsync(ct);
        }

        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct)
        {
            return dbContext.Booking.AsNoTracking().CountAsync(b=>b.SessionId == sessionId );
        }

        public async  Task<Session?> GetSessionWithTrainerAndCategoryByIdAsync(int sessionId, CancellationToken ct)
        {
            var session = dbContext.Sessions.Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s=>s.Id == sessionId);

            return await session ;
        }
    }
}
