using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystemG03.BLL.ViewModels.MembersViewModels;
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
        Task<bool> CreateMemberAsynce(CreateMemberViewModel model , CancellationToken ct =default);
        Task<bool> UpdateMemberDetailsAsynce(int id , MemberToUpdateViewModel model , CancellationToken ct = default);
    
        Task<bool> DeleteMemberAsync(int id,CancellationToken ct = default);
     }
}
