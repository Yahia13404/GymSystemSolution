using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Intrterfaces
{
    public interface ITrainerServices
    {
         Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
         Task<result>CreateTrainerAsync( CreateTrainerViewModel model, CancellationToken ct = default);
        Task<TrainerViewModel?> GetTrainerDetailsByIdAsync(int TrainerId, CancellationToken ct = default);
        Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int TrainerId, CancellationToken ct = default);
        Task<result> UpdateTrainerAsync(int TrainerId , TrainerToUpdateViewModel model, CancellationToken ct); 
        Task <TrainerViewModel> GetTrainerByIdAsynce(int TrainerId, CancellationToken ct = default);
        Task<result> DeleteTrainerAsync(int Trainer ,CancellationToken ct = default);
    }
}
