using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
      Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync( CancellationToken ct );
      Task<Session?> GetSessionWithTrainerAndCategoryByIdAsync(int sessionId, CancellationToken ct);
      Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct);

    }
}
