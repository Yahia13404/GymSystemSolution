using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices memberServices;

        public MemberController(IMemberServices memberServices)
        {
            this.memberServices = memberServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Members = await memberServices.GetAllMembersAsync(ct);
            return View(Members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);
            var Result = await memberServices.CreateMemberAsync(model, ct);
            if (Result)
                TempData["Success"] = "Member Created Successfully";
            else
                TempData["Error"] = "Failed to Create Member";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var MemberDetails = await memberServices.GetMembersDetailsAsync(id, ct);
            if (MemberDetails is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(MemberDetails);
        }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var HealthRecordDetails  = await memberServices.GetMemberHealthRecordAsync(id, ct);
            if (HealthRecordDetails is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecordDetails);
        }
        
    }
}
