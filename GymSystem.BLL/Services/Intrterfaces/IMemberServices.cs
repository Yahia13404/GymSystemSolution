using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using GymSystem.BLL.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Intrterfaces
{
    public interface IMemberServices

    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<MemberViewModel?> GetMembersDetailsAsync(int id,CancellationToken ct = default);
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int id,CancellationToken ct = default);

        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id,CancellationToken ct = default);
        Task<result> CreateMemberAsync(CreateMemberViewModel model , CancellationToken ct =default);
        Task<result> UpdateMemberDetailsAsynce(int id , MemberToUpdateViewModel model , CancellationToken ct = default);
        Task<MemberViewModel> GetMemberByIdAsync(int id, CancellationToken ct = default);

        Task<result> DeleteMemberAsync(int id,CancellationToken ct = default);
     }
}
