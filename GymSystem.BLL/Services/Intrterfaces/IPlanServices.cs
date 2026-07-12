using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.BLL.ViewModels.PlansViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Intrterfaces
{
    public interface IPlanServices
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsynce(CancellationToken ct = default ) ;

        Task<PlanViewModel?> GetPlanDetailsByIdAsync(int id, CancellationToken ct = default);

        Task<UpdatePlanViewModel> GetPlanToUpdate(int id, CancellationToken ct = default );
        Task<result> UpdatePlanViewModel(int id , UpdatePlanViewModel model,  CancellationToken ct = default );
        Task<result> ActiveAndDeactivePlan (  int PlanId ,CancellationToken ct = default);
    }
}
