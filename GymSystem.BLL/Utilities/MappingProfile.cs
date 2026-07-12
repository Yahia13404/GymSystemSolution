        using AutoMapper;
using GymSystem.BLL.ViewModels.MemberShipsViewModel;
using GymSystem.BLL.ViewModels.MembersViewModels;
        using GymSystem.BLL.ViewModels.PlansViewModels;
        using GymSystem.BLL.ViewModels.SessionsViewModels;
        using GymSystem.BLL.ViewModels.TrainersViewModels;
        using GymSystem.DAL.Entities;
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Net;
        using System.Text;
        using System.Threading.Tasks;

        namespace GymSystem.BLL.Utilities
        {
            public class MappingProfile : Profile
            {
                public MappingProfile()
                {
                    MapsSystem();
                    
        }
                private void MapsSystem()
                {
                    CreateMap<Session, SessionViewModel>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                        .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                    .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()).ReverseMap();
                    CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>(); 
                    CreateMap<Category, CategorySelectViewModel>();
                    CreateMap<Session, UpdateSessionViewModel>().ReverseMap();


            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Specialties,
                    opt => opt.MapFrom(src => src.Specialtize.ToString()))
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src =>
                        $"{src.Adress.BuildingNumber}, {src.Adress.Street}, {src.Adress.City}"));


            CreateMap<Trainer, CreateTrainerViewModel>()
                 
                .ForMember(dest => dest.BuildingNumber,
                    opt => opt.MapFrom(src => src.Adress.BuildingNumber))
                .ForMember(dest => dest.City,
                    opt => opt.MapFrom(src => src.Adress.City))
                .ForMember(dest => dest.Street,
                    opt => opt.MapFrom(src => src.Adress.Street))
                .ForMember(dest => dest.Specialties,
                    opt => opt.MapFrom(src => src.Specialtize)).ReverseMap();




            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber,
                    opt => opt.MapFrom(src => src.Adress.BuildingNumber))
                .ForMember(dest => dest.City,
                    opt => opt.MapFrom(src => src.Adress.City))
                .ForMember(dest => dest.Street,
                    opt => opt.MapFrom(src => src.Adress.Street))
                .ForMember(dest => dest.Specialties,
                    opt => opt.MapFrom(src => src.Specialtize)).ReverseMap();

         
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Adress,
                    opt => opt.MapFrom(src =>
                        $"{src.Adress.BuildingNumber}, {src.Adress.Street}, {src.Adress.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
     .ForMember(dest => dest.BuildingNumber,
         opt => opt.MapFrom(src => src.Adress.BuildingNumber))
     .ForMember(dest => dest.City,
         opt => opt.MapFrom(src => src.Adress.City))
     .ForMember(dest => dest.Street,
         opt => opt.MapFrom(src => src.Adress.Street))
     .ReverseMap()
     .ForMember(dest => dest.Adress,
         opt => opt.MapFrom(src => new Adress
         {
             BuildingNumber = src.BuildingNumber,
             City = src.City,
             Street = src.Street
         }))
     .ForMember(dest => dest.Photo,
         opt => opt.Ignore());




               

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.HealthRecord,
                    opt => opt.MapFrom(src => src.HealthRecordViewModel))
                .ForMember(dest => dest.Adress,
                    opt => opt.MapFrom(src => new Adress
                    {
                        BuildingNumber = src.BuildingNumber,
                        City = src.City,
                        Street = src.Street
                    }))
                .ReverseMap();

            CreateMap<Plan, PlanViewModel>()
                .ForMember(dest => dest.DurationDays  ,
                opt => opt.MapFrom(src => src.Duration));

                    CreateMap<Plan, UpdatePlanViewModel>()
            .ForMember(dest => dest.PlanName,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DurationDays,
                opt => opt.MapFrom(src => src.Duration))
            .ReverseMap()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.PlanName))
            .ForMember(dest => dest.Duration,
                opt => opt.MapFrom(src => src.DurationDays));




            CreateMap<MemberShip, MemberShipsViewModel>()
              .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
              .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
              .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Member, MemberSelectListViewModel>()
                .ForMember(d => d.MemberId, o => o.MapFrom(s => s.Id))
    .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Name));
            CreateMap<Plan, PlanSelectListViewModel>()
                 .ForMember(d => d.PlanId, o => o.MapFrom(s => s.Id))
    .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Name));



        }




      
    }

        }
