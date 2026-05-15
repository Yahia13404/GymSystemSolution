using GymSystem.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly GymDbContext dbContext = new GymDbContext();
        public async Task<IActionResult> Index()
        {
            var Plans = await dbContext.Plans.ToListAsync();
            return View(Plans);
        }
        public async Task<IActionResult> Details(int id)
        {
            var Plan = await dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
            if(Plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
    }
}
