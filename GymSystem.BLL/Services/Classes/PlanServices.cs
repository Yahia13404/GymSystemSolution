using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.PlansViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanServices(IUnitOfWork unitOfWork , IMapper mapper )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsynce(CancellationToken ct = default)
        {
            var  Plans = await unitOfWork.GetRepository<Plan>().GetAll(false,ct);

            if (Plans == null) 
            {
                return null; 
            }
            var MapperPlans = mapper.Map<IEnumerable<PlanViewModel>>(Plans);
            return MapperPlans;
        }

        public async Task<PlanViewModel?> GetPlanDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            var PlansRepo =  unitOfWork.GetRepository<Plan>();
            var Plan = await PlansRepo.GetById(id);
            if (Plan == null)
            {
                return null;
            }
            var MappedPlan = mapper.Map<Plan, PlanViewModel>(Plan);
            return MappedPlan;
        }

        public async Task<UpdatePlanViewModel> GetPlanToUpdate(int id, CancellationToken ct = default)
        {
            var PlanRepo = unitOfWork.GetRepository<Plan>();   
            var Plan = await PlanRepo.GetById(id);
            if (Plan == null) return null; 
            var Result = mapper.Map<Plan, UpdatePlanViewModel>(Plan);
            return Result;
        }

        public async Task<result> UpdatePlanViewModel(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var PlanRepo = unitOfWork.GetRepository<Plan>();
            var Plan =  await PlanRepo.GetById(id, ct);
            if (Plan == null)
            {
                return result.NotFound();   

            }
            Plan.UpdatedAt = DateTime.Now;
            var MappedPlan  =  mapper.Map(model, Plan);
            PlanRepo.Update(MappedPlan);
            var Result = await unitOfWork.CompleteAsync(); 
            return Result > 0 ? result.Ok() : result.Fail("Failed to update session");


        }
    }
}
