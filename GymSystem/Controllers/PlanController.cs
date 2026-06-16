using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.BLL.ViewModels.PlansViewModels;
using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanServices planServise;

        public PlanController(IPlanServices planServise)
        {
            this.planServise = planServise;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Plans = await planServise.GetAllPlansAsynce(ct);
            return View(Plans);
        }
        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var Plan = await planServise.GetPlanDetailsByIdAsync(id, token);
            if (Plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
        [HttpGet]
        public async Task<IActionResult> EditPlans ( int id, CancellationToken token)
        {
            var Plan = await planServise.GetPlanToUpdate(id);
            if (Plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction("Index");
            }
            return View(Plan);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlans(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var Result = await planServise.UpdatePlanViewModel(id, model, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Plan updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;

                return RedirectToAction("Index");
            }
        }


    }
}
