using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;


        public MemberServices(IUnitOfWork unitOfWork)

        {
            this.unitOfWork = unitOfWork;

        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var Members = await unitOfWork.GetRepository<Member>().GetAll(false, ct);
            if (Members == null) return [];
            var MembersViewModel = Members.Select(m => new MemberViewModel()
            {
                Id =   m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString(),
            });
            return MembersViewModel;

        }
        public async Task<MemberViewModel?> GetMembersDetailsAsync(int memberID, CancellationToken ct = default)
        {
            var member =   await unitOfWork.GetRepository<Member>().GetById(memberID, ct);
            if (member == null) return null;
            var memberViewModel = new MemberViewModel()
            {
                
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address =  $"{member.Adress.BuildingNumber} - {member.Adress.Street} - {member.Adress.City}",



            };

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
            var memberToUpdateViewModel = new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = member.Adress.BuildingNumber,
                Street = member.Adress.Street,
                City = member.Adress.City
            };
            return memberToUpdateViewModel;

        }


        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id, CancellationToken ct = default)
        {
            var Record = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefultAsync(hr => hr.MemberId == id,false ,ct);
            if (Record is null) return null;
            var HealthRecordViewModel = new HealthRecordViewModel()
            {
                Height = Record.Height,
                Weight = Record.Weight,
                BloodType = Record.BloodType,
                Note = Record.Note
            };
            return HealthRecordViewModel;

        }

      
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            
            if (emailExist || phoneExist) return false;
            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Adress = new Adress()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                },
                HealthRecord = new HealthRecord()
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }


            };
            unitOfWork.GetRepository<Member>().Add(member);
            var result = await unitOfWork.CompleteAsync();
            return result > 0;


        }  

        public async Task<bool> UpdateMemberDetailsAsynce(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {   
            var member = await unitOfWork.GetRepository<Member>().GetById(id, ct);
            if (member == null) return false;
            var emailExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id, ct);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct);
            if (emailExist || phoneExist) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Adress.BuildingNumber = model.BuildingNumber;
            member.Adress.Street = model.Street;
            member.Adress.City = model.City;
            member.UpdatedAt = DateTime.Now;
            unitOfWork.GetRepository<Member>().Update(member);
            var result = await unitOfWork.GetRepository<Member>().ComoleteAsync();
            return result > 0;
        }
        public async Task<bool> DeleteMemberAsync(int memberID, CancellationToken ct = default)
        {
            var HasFutureSessions =   await unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberID && b.Session.EndDate > DateTime.Now);
            if (HasFutureSessions) return false;
            unitOfWork.GetRepository<Member>().Delete(memberID);
            var result = await unitOfWork.CompleteAsync();
            return result > 0;

        }

        
    }
}
