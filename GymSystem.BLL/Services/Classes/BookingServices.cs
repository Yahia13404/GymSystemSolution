using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.BookingsViewModels;
using GymSystem.BLL.ViewModels.MemberShipsViewModel;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookingServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default)
        {
           var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync( ct);
            var sessionViewModels = mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in sessionViewModels)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);

            }
            return result<IEnumerable<SessionViewModel>>.Ok(sessionViewModels);
        }


        public async Task<result<IEnumerable<MemberForSessionViewModel>>> GetMembersForSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var memberBooking = await unitOfWork.BookingRepository.GetBookingsBySessionIdAsync(sessionId, ct);
            var session = await unitOfWork.SessionRepository.GetById(sessionId, ct);
            var memberBookingViewModel = memberBooking.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                MemberName = b.Member.Name,
                BookingDate = b.CreatedAt,
                SessionId = b.SessionId,
                IsAttended = session?.StartDate > DateTime.Now ? false : b.IsAttended

            }).ToList();

            return result<IEnumerable<MemberForSessionViewModel>>.Ok(memberBookingViewModel);
        }


        public async Task<result<IEnumerable<MemberSelectListViewModel>>> GetMemberForDropdown(int sessionId, CancellationToken ct = default)
        {
            var booking = await unitOfWork.BookingRepository.GetAll(false , ct);
            var BookedMemberIds = booking.Select(b => b.MemberId).ToList();

            var membersAvailable= await unitOfWork.GetRepository<Member>().GetAll(false ,ct );
            var mappedMembersAvailable = mapper.Map<IEnumerable<MemberSelectListViewModel>>(membersAvailable);
            return result<IEnumerable<MemberSelectListViewModel>>.Ok(mappedMembersAvailable.Where(m => !BookedMemberIds.Contains(m.MemberId)));

        }

        public async Task<result> CreateBookingAsync(CreateBookingViewModel bookingViewModel, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetById(bookingViewModel.SessionId, ct); 
            if (session is null)
            {
                return result.NotFound("Session Not Found");
            }

            if(session.StartDate <= DateTime.Now)
            {
                return result.Validation("Cannot book a session that has already started");
            }

            var HasActiveMembership = await unitOfWork.MemberShipRepository.AnyAsync(m => m.MemberId  == bookingViewModel.MemberId && m.EndDate > DateTime.Now, ct);
            if (!HasActiveMembership)
            {
                return result.Validation("Member does not have an active membership");
            }

            var alreadyBooked = await unitOfWork.BookingRepository.AnyAsync(b => b.MemberId == bookingViewModel.MemberId && b.SessionId == bookingViewModel.SessionId, ct);

            if (alreadyBooked)
            {
                return result.Validation("Member has already booked this session");
            }

            var bookedSlotsCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(bookingViewModel.SessionId, ct);

            if (bookedSlotsCount >= session.Capacity)
            {
                return result.Validation("Session is fully booked");
            }


            var booking = new Booking
            {
                MemberId = bookingViewModel.MemberId,
                SessionId = bookingViewModel.SessionId,
                CreatedAt = DateTime.Now,
                IsAttended = false
            };

            unitOfWork.BookingRepository.Add(booking);


            var Result = await unitOfWork.CompleteAsync();
            return Result > 0 ? result.Ok() : result.Fail("Failed to create booking", ResultKind.NotFound); 


        }


        public async Task<result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await unitOfWork.BookingRepository.FirstOrDefultAsync(b => b.MemberId == memberId && b.SessionId == sessionId,true ,ct);
            if (booking is null)
            {
                return result.NotFound("Booking not found");
            }

            booking.IsAttended = true;

            unitOfWork.BookingRepository.Update(booking);
            var Result = await unitOfWork.CompleteAsync();
            return Result > 0 ? result.Ok() : result.Fail("Failed to mark attendance");
        }

        public async Task<result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            if (session is null)
            {
                return result.NotFound("Session not found");
            }
            
            if (session.StartDate <= DateTime.Now)
            {
                return result.Validation("Cannot cancel a booking for a session that has already started");
            }

            var booking = await unitOfWork.BookingRepository.FirstOrDefultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, true, ct);
            if (booking is null)
            {
                return result.NotFound("Booking not found");
            }
            unitOfWork.BookingRepository.Delete(booking);

            return await unitOfWork.CompleteAsync() > 0 ? result.Ok() : result.Fail("Failed to cancel booking");

        }

        
    }
}
