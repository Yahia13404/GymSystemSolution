using GymSystem.DAL.Contexts;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanRepository planRepository;
        public PlanController( IPlanRepository _planRepository) 
        {
            planRepository = _planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken token) 
        {
            var Plans = await planRepository.GetAll(false ,token);
            return View(Plans);
        }
        public async Task<IActionResult> Details(int id , CancellationToken token)
        {
            var Plan = await planRepository.GetById(id , token ) ;
            if (Plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
    }
}
