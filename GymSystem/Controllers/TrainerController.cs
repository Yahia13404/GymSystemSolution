using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.ViewModels.TrainersViewModels;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private readonly ITrainerServices trainerServices;

        public TrainerController(ITrainerServices TrainerServices)
        {
            trainerServices = TrainerServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Trainers = await trainerServices.GetAllTrainersAsync(ct);
            return View(Trainers);
        }



        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var Result = await trainerServices.CreateTrainerAsync(model, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Trainer created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
                return View(model);
            }
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)

        {
            var Trainer = await trainerServices.GetTrainerDetailsByIdAsync(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction("Index");
            }
            return View(Trainer);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var Trainer = await trainerServices.GetTrainerToUpdateAsync(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction("Index");
            }
            return View(Trainer);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var Result = await trainerServices.UpdateTrainerAsync(id, model, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Trainer Updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Trainer = await trainerServices.GetTrainerByIdAsynce(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction("Index");
            }
            return View(Trainer);
        }


        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct)
        {
            var Result = await trainerServices.DeleteTrainerAsync(id, ct);

            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess ? "Trainer deleted successfully!" : Result.Error;
            return RedirectToAction("Index", "Trainer");
        }
    }


}
