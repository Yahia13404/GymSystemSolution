using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MemberShipsViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    public class MemberShipController : Controller
    {
        private readonly IMemberShipServices memberShipServices;

        public MemberShipController(IMemberShipServices memberShipServices  )
        {
            this.memberShipServices = memberShipServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
           var result = await memberShipServices.GetAllMemberShipsAsync(ct); 
            var MemberShips = result.Value;
            return View(MemberShips);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdowns(ct); 

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(ct); 
                return View(model);
            }
            var result = await memberShipServices.CreateMemberShipAsync(model, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "An error occurred while creating the membership.";
                await PopulateDropdowns(ct); 
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Cancel(int id ,CancellationToken ct)
        {
            var result = await memberShipServices.DeleteActiveMemberShipAsync(id, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Membership Canceled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "An error occurred while canceling the membership.";
            }
            return RedirectToAction(nameof(Index));

        }
        private async Task PopulateDropdowns(CancellationToken ct)
        {
            var plansResult = await memberShipServices.GetPlansForDropdownAsync(ct);
            ViewBag.Plans = new SelectList(
                plansResult.Value,
                "PlanId",
                "PlanName");

            var membersResult = await memberShipServices.GetMembersForDropdownAsync(ct);
            ViewBag.Members = new SelectList(
                membersResult.Value,
                "MemberId",
                "MemberName");
        }
    }
}
