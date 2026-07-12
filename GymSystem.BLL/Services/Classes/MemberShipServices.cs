using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MemberShipsViewModel;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberShipServices : IMemberShipServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberShipServices(IUnitOfWork unitOfWork , IMapper mapper )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<result<IEnumerable<MemberShipsViewModel>>> GetAllMemberShipsAsync(CancellationToken ct = default)
        {
            var memberShips = await unitOfWork.MemberShipRepository.GetMemberShipsWihPlansAndMembersAsync(ct , m => m.EndDate > DateTime.Now );
            var memberShipsViewModel = mapper.Map<IEnumerable<MemberShipsViewModel>>(memberShips);

            return result<IEnumerable<MemberShipsViewModel>>.Ok(memberShipsViewModel);
        }
        public async Task<result> CreateMemberShipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {
            var memberExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId, ct);
            if(!memberExist)
            {
                return  result.NotFound("Member Not Found");
            }

            var planExist = await unitOfWork.GetRepository<Plan>().AnyAsync(p => p.Id == model.PlanId, ct);
            if(!planExist)
            {
                return result.NotFound("Plan Not Found");
            }
            
            var HasActivePlan = await unitOfWork.GetRepository<MemberShip>().AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            
            if(HasActivePlan)
            {
                return result.Fail("Member Already Has Active Plan OR Member is not exist", ResultKind.Conflict);
            }
            
            var plan = await unitOfWork.GetRepository<Plan>().GetById(model.PlanId, ct);
            if(!plan.IsActive)
                return result.Fail("Plan is not Active", ResultKind.Conflict);

            var memberShip = new MemberShip
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                CreatedAt = model.StartDate ?? DateTime.Now,
                EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.Duration)

            }; 

           unitOfWork.GetRepository<MemberShip>().Add(memberShip);
            var Result = await unitOfWork.CompleteAsync();
            return Result > 0 ? result.Ok() : result.Fail("Failed to Create MemberShip", ResultKind.Validationfailed);





        }

        
        public async Task<result<IEnumerable<MemberSelectListViewModel>>> GetMembersForDropdownAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAll( ct: ct);
            var memberSelectViewModel = mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);

            return result<IEnumerable<MemberSelectListViewModel>>.Ok(memberSelectViewModel);

        }

        public async Task<result<IEnumerable<PlanSelectListViewModel>>> GetPlansForDropdownAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepository<Plan>().GetAll(ct: ct);
            var planSelectViewModel = mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);

            return result<IEnumerable<PlanSelectListViewModel>>.Ok(planSelectViewModel);

        }

        public async Task<result> DeleteActiveMemberShipAsync(int MemberId, CancellationToken ct = default)
        {
            var activeMemberShip = await unitOfWork.GetRepository<MemberShip>().FirstOrDefultAsync(m => m.MemberId == MemberId && m.EndDate > DateTime.Now , true , ct );
            if(activeMemberShip is null)
            {
                return result.NotFound("Active MemberShip Not Found");
            }
            unitOfWork.GetRepository<MemberShip>().Delete(activeMemberShip.Id);
            var Result = await unitOfWork.CompleteAsync();

            return Result > 0 ?  result.Ok() : result.Fail("Member cannot be deleted") ;

        }



    }
}
