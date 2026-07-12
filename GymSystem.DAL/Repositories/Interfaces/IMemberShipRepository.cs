using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IMemberShipRepository  : IGenericRepository<MemberShip>
    {
        Task<IEnumerable<MemberShip>> GetMemberShipsWihPlansAndMembersAsync(CancellationToken ct , Expression<Func<MemberShip , bool >>? filter = null ); 
    }
}
