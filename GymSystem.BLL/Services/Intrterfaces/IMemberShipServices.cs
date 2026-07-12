using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MemberShipsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Intrterfaces
{
    public interface IMemberShipServices
    {
        Task<result<IEnumerable<MemberShipsViewModel>>> GetAllMemberShipsAsync(CancellationToken ct = default);
        
        Task<result> CreateMemberShipAsync(CreateMemberShipViewModel model, CancellationToken ct = default);

        Task<result<IEnumerable<PlanSelectListViewModel>>> GetPlansForDropdownAsync(CancellationToken ct = default);
        Task<result<IEnumerable<MemberSelectListViewModel>>> GetMembersForDropdownAsync(CancellationToken ct = default);

        Task<result> DeleteActiveMemberShipAsync(int MemberId, CancellationToken ct = default);

    }
}
