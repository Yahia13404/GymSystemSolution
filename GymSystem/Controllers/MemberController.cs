using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberServices memberServices;
        private readonly IAttachmentServices attachmentServices;

        public MemberController(IMemberServices memberServices , IAttachmentServices attachmentServices)
        {
            this.memberServices = memberServices;
            this.attachmentServices = attachmentServices;
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
            if (Result.IsSuccess)
                TempData["Success"] = "Member Created Successfully";
            else
                TempData["Error"] =Result.Error;

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

        [HttpGet]
        public async Task<IActionResult> EditMember(int id , CancellationToken ct)
        {
            var Member = await memberServices.GetMemberToUpdateAsync(id, ct);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("Index");
            }
            return View(Member);

        }


        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model ,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var Result = await memberServices.UpdateMemberDetailsAsynce(id,model ,ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;

                return RedirectToAction("Index"); 
            }
        }
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Member = await memberServices.GetMemberByIdAsync(id, ct);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("Index");
            }
            return View(Member);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct)
        {
            var Result = await memberServices.DeleteMemberAsync(id, ct);

            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess ? "Member deleted successfully!" : Result.Error;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Pictures(int id, CancellationToken ct)
        {
            var member = await memberServices.GetMemberByIdAsync(id, ct);

            if (member == null || string.IsNullOrEmpty(member.Photo))
            {
                return NotFound();
            }

            var (stream, contentType) =
                attachmentServices.GetFile(member.Photo, "MembersPictures");

            return File(stream, contentType);
        }

    }
}
