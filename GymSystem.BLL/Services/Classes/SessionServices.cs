using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
            {
                return result.Validation("End date must be after start date");
            }
            if (model.StartDate <= DateTime.Now)
            {
                return result.Validation("Start date must be in the future");
            }
            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(model.TrainerId, ct);
            if (Trainer == null)
            {
                return result.NotFound("Trainer not found");
            }
            var CategoryRepo = unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetById(model.CategoryId, ct);
            if (Category == null)
            {
                return result.NotFound("Category not found");
            }

            var Session = mapper.Map<CreateSessionViewModel, Session>(model);
            var sessionRepo = unitOfWork.GetRepository<Session>();
            sessionRepo.Add(Session);

            var RowEffected = await unitOfWork.CompleteAsync();

            return RowEffected > 0 ? result.Ok() : result.Fail("Failed to create session");


        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var Sessoins = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (!Sessoins.Any())
            {
                return null;
            }
            Sessoins = Sessoins.OrderByDescending(s => s.StartDate);

            var MappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessoins);
            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }
            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var CategoryRepo = await unitOfWork.GetRepository<Category>().GetAll(false, ct);
            var MappedCategories = mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(CategoryRepo);
            return MappedCategories;
        }

        public async Task<SessionViewModel> GetSessionByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(id, ct);
            
            if (session is null)
            {
                return null;

            }
            var MappedSession = mapper.Map<Session, SessionViewModel>(session);
            return MappedSession;

        }

        public async Task<SessionViewModel?> GetSessionDetailsByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryByIdAsync(sessionId, ct);
            if (session == null)
            {
                return null;
            }
            var MappedSession = mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlots = MappedSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return MappedSession;
        }

        public async Task<UpdateSessionViewModel> GetSessionForUpdateByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            if (session == null)
            {
                return null;
            }
            if(!await IsSessionValidForUpdateAsync(session, ct))
            {
                return null;
            }
            return mapper.Map<Session, UpdateSessionViewModel>(session);

             
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropdownAsync(CancellationToken ct = default)
        {
            var TrainerRepo = await unitOfWork.GetRepository<Trainer>().GetAll(false, ct);
            var MappedTrainers = mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(TrainerRepo);
            return MappedTrainers;
        }

        public async Task<result> RemoveSessionAsync(int id, CancellationToken ct = default)
        {
            var sessionRepo = unitOfWork.GetRepository<Session>();
            var session = await sessionRepo.GetById(id, ct); 
            if (session == null) return result.NotFound("Session not found");
            if(session.EndDate >= DateTime.Now) return result.Validation("Cannot delete a session that has no yet ended");
            var bookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (bookedCount > 0) return result.Fail("Session has booked slots and cannot be removed"); 
            sessionRepo.Delete(id);
            var effectedRows = await unitOfWork.CompleteAsync();
            return effectedRows > 0 ? result.Ok() : result.Fail("Failed to remove session");

        }

        public async Task<result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
           var sessionRepo =  unitOfWork.GetRepository<Session>();
           var session = await sessionRepo.GetById(id, ct);
           if (session == null) return result.NotFound("Session not found");
           if (session.StartDate <= DateTime.Now) return result.Validation("Session has already started and cannot be updated");
           var bookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
           if(bookedCount > 0) return result.Fail("Session has booked slots and cannot be updated");
           if (model.EndDate <= model.StartDate) return result.Validation("End date must be after start date");
           if (model.StartDate <= DateTime.Now) return result.Validation("Start date must be in the future");
            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(model.TrainerId, ct);
            if (Trainer == null)
            {
                return result.NotFound("Trainer not found");
            }
            session.UpdatedAt = DateTime.Now;
            mapper.Map(model, session);
            sessionRepo.Update(mapper.Map(model, session));

            var RowEffected = await unitOfWork.CompleteAsync();
            return RowEffected > 0 ? result.Ok() : result.Fail("Failed to update session");
        }

        private async Task<bool> IsSessionValidForUpdateAsync(Session session, CancellationToken ct)
        {
            if (session.StartDate <=  DateTime.Now)
            {
                return false;
            }
            var booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
           return booked == 0;

        }
    }
}