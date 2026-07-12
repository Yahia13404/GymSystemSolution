using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionServices sessionServices;

        public SessionController(ISessionServices sessionServices)
        {
            this.sessionServices = sessionServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await sessionServices.GetAllSessionsAsync(ct);

            return View(sessions);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulationDropDowns(ct);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulationDropDowns(ct);
                return View(model);

            }
            var Result = await sessionServices.CreateSessionAsync(model, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
                await PopulationDropDowns(ct);
                return View(model);
            }


        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await sessionServices.GetSessionDetailsByIdAsync(id, ct);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            return View(session);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await sessionServices.GetSessionForUpdateByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            await PopulationDropDowns(ct);
            return View(session);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulationDropDowns(ct);
                return View(model);
            }
            var Result = await sessionServices.UpdateSessionAsync(id, model, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
                await PopulationDropDowns(ct);
                return View(model);
            }
        }


        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Session = await sessionServices.GetSessionByIdAsync(id, ct);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            return View(Session);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        { 
            var Result = await sessionServices.RemoveSessionAsync(id, ct);
         
            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess ? "Session deleted successfully!" : Result.Error;
            return RedirectToAction("Index");
        }


        private async Task PopulationDropDowns(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await sessionServices.GetTrainersForDropdownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionServices.GetCategoriesForDropDownAsync(ct), "Id", "CategoryName");

        }
    }
}
