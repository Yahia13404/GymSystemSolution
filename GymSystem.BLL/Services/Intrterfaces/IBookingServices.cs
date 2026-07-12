using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.BookingsViewModels;
using GymSystem.BLL.ViewModels.MemberShipsViewModel;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Intrterfaces
{
    public interface IBookingServices
    {
        Task<result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<result<IEnumerable<MemberForSessionViewModel>>> GetMembersForSessionAsync(int sessionId, CancellationToken ct = default);
        Task<result<IEnumerable<MemberSelectListViewModel>>> GetMemberForDropdown(int sessionId, CancellationToken ct = default);
        Task <result> CreateBookingAsync(CreateBookingViewModel bookingViewModel, CancellationToken ct = default);
        Task<result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);
        Task<result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
    }
}
