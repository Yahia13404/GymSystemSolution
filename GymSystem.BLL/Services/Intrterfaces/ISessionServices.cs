using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Intrterfaces
{
    public interface ISessionServices
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropdownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default);
        Task<SessionViewModel?> GetSessionDetailsByIdAsync(int sessionId, CancellationToken ct = default);  
        Task<UpdateSessionViewModel> GetSessionForUpdateByIdAsync(int sessionId, CancellationToken ct = default);
        Task<result> UpdateSessionAsync(int id,UpdateSessionViewModel model ,CancellationToken ct = default);
        Task<SessionViewModel> GetSessionByIdAsync(int id, CancellationToken ct = default);
        Task<result> RemoveSessionAsync(int id, CancellationToken ct = default);

    }
}
