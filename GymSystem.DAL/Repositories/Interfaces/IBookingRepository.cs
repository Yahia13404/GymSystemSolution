using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IBookingRepository  : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(int sessionId ,CancellationToken ct = default);
       


    }
}
