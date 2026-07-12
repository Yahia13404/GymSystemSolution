using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.BookingsViewModels;
using GymSystem.BLL.ViewModels.MemberShipsViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingServices bookingServices;

        public BookingController(IBookingServices bookingServices)
        {
            this.bookingServices = bookingServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {

            var sessions = await bookingServices.GetAllSessionsAsync(ct);
            
            return View(sessions.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForUpcomingSession(int Id, CancellationToken ct)
        {
            var members = await bookingServices.GetMembersForSessionAsync (Id, ct);
            return View(members.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSessions(int Id, CancellationToken ct)
        {
            var members = await bookingServices.GetMembersForSessionAsync(Id, ct);
            return View(members.Value);
        }


        [HttpGet]
        public async Task<IActionResult> GetMemberForCompletedSession(int Id, CancellationToken ct)
        {
            var members = await bookingServices.GetMembersForSessionAsync(Id, ct);
            return View(members.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int Id , CancellationToken ct)
        {
            var members =await  GetMemberForDropdown(Id, ct);
            ViewBag.Members =  new SelectList (members , "MemberId", "MemberName") ;
            
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create (CreateBookingViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var Result = await bookingServices.CreateBookingAsync(model, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Booking created successfully!";
            
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
        
            }

           return RedirectToAction(nameof(GetMembersForUpcomingSession) , new {id = model.SessionId});
        }



        


        [HttpPost]
        public async Task<IActionResult> Attended(int MemberId , int SessionId   , CancellationToken ct)
        {
            var Result = await bookingServices.MarkAttendedAsync(MemberId, SessionId ,  ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member Marked As Attended Successfuly";

            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
            }
            return RedirectToAction(nameof(GetMembersForOngoingSessions)  ,  new { id = SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {
            var Result = await bookingServices.CancelBookingAsync( memberId, sessionId, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Booking canceled successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession)   , new { id = sessionId });
        }
        private async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropdown(int sessionId, CancellationToken ct)
        {
            var members = await bookingServices.GetMemberForDropdown(sessionId, ct);
            return members.Value;
        }
    }
}
