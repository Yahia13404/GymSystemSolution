        using AutoMapper;
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
                    MapsSession();
                }
                private void MapsSession()
                {
                    CreateMap<Session, SessionViewModel>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                        .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                    .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()).ReverseMap();
                    CreateMap<CreateSessionViewModel, Session>();
                    CreateMap<Trainer, TrainerSelectViewModel>();
                    CreateMap<Category, CategorySelectViewModel>();
                    CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
                    CreateMap<Trainer, TrainerViewModel>();
                    CreateMap<CreateTrainerViewModel, Trainer>().ForMember(
                dest => dest.Adress,
                opt => opt.MapFrom(src => new Adress
                {
                    Street = src.Street,
                    City = src.City
                }));

                    CreateMap<TrainerToUpdateViewModel, Trainer>().ReverseMap();

                    CreateMap<Member, MemberViewModel>();
                    CreateMap<Member, MemberToUpdateViewModel>().ReverseMap().ForMember(dest => dest.Name, opt => opt.Ignore());
                    CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
                    CreateMap<CreateMemberViewModel, Member>()
                        .ForMember(dest => dest.HealthRecord,opt => opt.MapFrom(src => src.HealthRecordViewModel))
                        .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => new Adress
                    {
                        BuildingNumber = src.BuildingNumber,
                        City = src.City,
                        Street = src.Street
                    }));

                    CreateMap<Plan, PlanViewModel>();

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
                }
            }
        }
