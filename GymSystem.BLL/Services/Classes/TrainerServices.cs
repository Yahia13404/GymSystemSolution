using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.TrainersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymSystem.BLL.Services.Classes
{
    public class TrainerServices : ITrainerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public TrainerServices(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {

            var Trainer = mapper.Map<CreateTrainerViewModel, Trainer>(model);
            
            var TrainersRepo = _unitOfWork.GetRepository<Trainer>();
            var existingTrainer = await TrainersRepo.FirstOrDefultAsync(t => t.Email == model.Email, false, ct);
            if (existingTrainer != null)
            {
                return result.Fail("A trainer with the same email already exists.", ResultKind.Validationfailed); 
            }   

            TrainersRepo.Add(Trainer);
            var RowEffected = await _unitOfWork.CompleteAsync();
            
            return RowEffected > 0 ? result.Ok() : result.Fail("Failed to create Trainer");

        }

        public async Task<result> DeleteTrainerAsync(int TrainerId , CancellationToken ct = default)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var Trainer =  await TrainerRepo.GetById(TrainerId, ct);
            if (Trainer == null) 
            {
                return result.NotFound("Trainer is not found");

            }
            TrainerRepo.Delete(TrainerId); 
            var effectedRows = await _unitOfWork.CompleteAsync();
            return effectedRows > 0 ? result.Ok() : result.Fail("Failed to remove Trainer");

        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var TrainsersRepo = _unitOfWork.GetRepository<Trainer>();
            var Trainers = await TrainsersRepo.GetAll(false ,ct);
                if (Trainers == null)
                    return Enumerable.Empty<TrainerViewModel>();
            return mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);

        }

        public async Task<TrainerViewModel> GetTrainerByIdAsynce(int TrainerId, CancellationToken ct = default)
        {
            var Trainer = await  _unitOfWork.GetRepository<Trainer>().GetById(TrainerId, ct);

            if (Trainer == null)
            {
                return null;

            }
            return mapper.Map<Trainer, TrainerViewModel>(Trainer);
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsByIdAsync(int TrainerId, CancellationToken ct = default)
        {
           var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(TrainerId);

            if (Trainer == null)
            {
                return null;

            }
            var MappedTrainer =mapper.Map<Trainer, TrainerViewModel > (Trainer);
            return MappedTrainer; 
        }

        public async Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int TrainerId, CancellationToken ct = default)
        {
            var Trainer = await _unitOfWork.GetRepository<Trainer>().GetById(TrainerId,ct);
            
            if(Trainer == null)
            {
               return null ;
                
            }
            return mapper.Map<Trainer, TrainerToUpdateViewModel>(Trainer); 

        }

        public async Task<result> UpdateTrainerAsync(int TrainerId, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(TrainerId);
            if(Trainer == null)
            {
                return result.NotFound("Trainer is not found");
            }
            var emailExists = await TrainerRepo.AnyAsync(x =>
             x.Email == model.Email &&
             x.Id != TrainerId);

            if (emailExists)
            {
                return result.Fail("Email is already registered.");
            }

            var phoneExists = await TrainerRepo.AnyAsync(x =>
                x.Phone == model.Phone &&
                x.Id != TrainerId);

            if (phoneExists)
            {
                return result.Fail("phone is already used.");
            }
            var MapperTrainer = mapper.Map(model, Trainer);
            TrainerRepo.Update(MapperTrainer);
            var RowEffected = await _unitOfWork.CompleteAsync();
            return RowEffected > 0 ? result.Ok() : result.Fail("Failed to update Trainer");


        }

        


    }
}
