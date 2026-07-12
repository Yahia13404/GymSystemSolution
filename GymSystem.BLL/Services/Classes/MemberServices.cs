using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using GymSystem.BLL.ViewModels.TrainersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAttachmentServices attachmentServices;

        public MemberServices(IUnitOfWork unitOfWork, IMapper mapper ,IAttachmentServices attachmentServices)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.attachmentServices = attachmentServices;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var Members = await unitOfWork.GetRepository<Member>().GetAll(false, ct);
            if (Members == null) return [];
            var MembersViewModel = mapper.Map<IEnumerable<Member> , IEnumerable<MemberViewModel>> (Members);
            return MembersViewModel;

        }
        public async Task<MemberViewModel?> GetMembersDetailsAsync(int memberID, CancellationToken ct = default)
        {
            var member =   await unitOfWork.GetRepository<Member>().GetById(memberID, ct);
            if (member == null) return null;
            var memberViewModel = mapper.Map<Member , MemberViewModel> (member);
            var ActiveMemberShip = await unitOfWork.GetRepository<MemberShip>().FirstOrDefultAsync(mb => mb.MemberId ==  memberID && mb.EndDate > DateTime.Now,false , ct);
            if(ActiveMemberShip is not null)
            {
                var ActivePlan = await unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId, ct);
                memberViewModel.PlanName = ActivePlan?.Name;
                memberViewModel.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

            }
            return memberViewModel;
        }  
        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberID, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(memberID, ct);
            if (member is null) return null;
            var memberToUpdateViewModel = mapper.Map<Member ,MemberToUpdateViewModel> (member);
            return memberToUpdateViewModel;

        }


        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id, CancellationToken ct = default)
        {
            var Record = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefultAsync(hr => hr.MemberId == id,false ,ct);
            if (Record is null) return null;
            var HealthRecordViewModel = mapper.Map<HealthRecord , HealthRecordViewModel> (Record);
            return HealthRecordViewModel;

        }

      
        public async Task<result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {

            var emailExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (emailExist || phoneExist) return result.Validation("Email Or phone already exist ");
            var member = unitOfWork.GetRepository<Member>();
            var Mappedmember = mapper.Map<CreateMemberViewModel, Member>(model);
            var NewPhotoName = await attachmentServices.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, "MembersPictures");
            if (string.IsNullOrEmpty(NewPhotoName))
            {
                return result.Validation("photo is not valid "); 
            }    
            Mappedmember.Photo = NewPhotoName;
            member.Add(Mappedmember);
            var Result = await unitOfWork.CompleteAsync();
            return Result > 0 ? result.Ok() : result.Fail("Failed to create Member");
        }  

        public async Task<result> UpdateMemberDetailsAsynce(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(id, ct);
            if (member == null) return result.NotFound();

            var emailExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id, ct);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct);
            if (emailExist || phoneExist) return result.Fail("Email Or phone already exist");

            var oldName = member.Name;


            mapper.Map(model, member);

         

            member.Name = oldName;

            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().Update(member); 
            var Result = await unitOfWork.CompleteAsync();

            return Result > 0 ? result.Ok() : result.Fail("Failed to Update Member");
        }
        public async Task<result> DeleteMemberAsync(int memberID, CancellationToken ct = default)
        {
            var HasFutureSessions = await unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberID && b.Session.EndDate > DateTime.Now);
            if (HasFutureSessions) return result.Fail("Member Has Future Session");
            var memberRepo = unitOfWork.GetRepository<Member>();
            var member = await memberRepo.GetById(memberID,ct);
            if(member == null)
            {
                return result.NotFound("Member is not found");
            }
           
            memberRepo.Delete(memberID);
            if (member.Photo is not null)
            {
               attachmentServices.Delete(member.Photo, "MembersPictures");

            }
            var Result = await unitOfWork.CompleteAsync();
            return Result > 0 ? result.Ok() : result.Fail("Failed to remove member");

        }

        public async Task<MemberViewModel> GetMemberByIdAsync(int id, CancellationToken ct = default)
        {
            
            var Member = await unitOfWork.GetRepository<Member>().GetById(id);
            if(Member is null)
            {
                return null; 
            }
            var mappedMember = mapper.Map<Member , MemberViewModel>(Member);
            return mappedMember;
       
        }
    }
}
