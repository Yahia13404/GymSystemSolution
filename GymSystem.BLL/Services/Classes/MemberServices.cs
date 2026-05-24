using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystemG03.BLL.ViewModels.MembersViewModels;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IGenericRepository<Member> memberRepository;
        private readonly IGenericRepository<MemberShip> memberShipRepository;
        private readonly IGenericRepository<Plan> planRepository;
        private readonly IGenericRepository<HealthRecord> healthRecordRepository;
        private readonly IGenericRepository<Booking> bookingRepository;

        public MemberServices(IGenericRepository<Member> memberRepository , IGenericRepository<MemberShip> memberShipRepository , IGenericRepository<Plan> planRepository , IGenericRepository<HealthRecord> healthRecordRepository,
            IGenericRepository<Booking> bookingRepository)
        {
            this.memberRepository = memberRepository;
            this.memberShipRepository = memberShipRepository;
            this.planRepository = planRepository;
            this.healthRecordRepository = healthRecordRepository;
            this.bookingRepository = bookingRepository;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var Members = await memberRepository.GetAll(false, ct);
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
            var member =   await memberRepository.GetById(memberID, ct);
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

            var ActiveMemberShip = await memberShipRepository.FirstOrDefultAsync(mb => mb.MemberId ==  memberID && mb.EndDate > DateTime.Now,false , ct);
            if(ActiveMemberShip is not null)
            {
                var ActivePlan = await planRepository.GetById(ActiveMemberShip.PlanId, ct);
                memberViewModel.PlanName = ActivePlan?.Name;
                memberViewModel.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

            }
            return memberViewModel;


        }
        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberID, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(memberID, ct);
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
            var Record = await healthRecordRepository.FirstOrDefultAsync(hr => hr.MemberId == id,false ,ct);
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

      
        public async Task<bool> CreateMemberAsynce(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExist = await memberRepository.AnyAsync(m => m.Email == model.Email, ct);
            var phoneExist = await memberRepository.AnyAsync(m => m.Phone == model.Phone, ct);
            
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
            memberRepository.Add(member);
            var result = await memberRepository.ComoleteAsync();
            return result > 0;


        }  

        public async Task<bool> UpdateMemberDetailsAsynce(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {   
            var member = await memberRepository.GetById(id, ct);
            if (member == null) return false;
            var emailExist = await memberRepository.AnyAsync(m => m.Email == model.Email && m.Id != id, ct);
            var phoneExist = await memberRepository.AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct);
            if (emailExist || phoneExist) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Adress.BuildingNumber = model.BuildingNumber;
            member.Adress.Street = model.Street;
            member.Adress.City = model.City;
            member.UpdatedAt = DateTime.Now;
            memberRepository.Update(member);
            var result = await memberRepository.ComoleteAsync();
            return result > 0;
        }
        public async Task<bool> DeleteMemberAsync(int memberID, CancellationToken ct = default)
        {
            var HasFutureSessions =   await bookingRepository.AnyAsync(b => b.MemberId == memberID && b.Session.EndDate > DateTime.Now);
            if (HasFutureSessions) return false;
            memberRepository.Delete(memberID);
            var result = await memberRepository.ComoleteAsync();
            return result > 0;

        }
    }
}
