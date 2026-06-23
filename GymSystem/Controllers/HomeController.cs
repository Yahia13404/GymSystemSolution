using System.Diagnostics;
using System.Threading.Tasks;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller 
    {
        private readonly ILogger<HomeController> logger;
        private readonly IAnalyticsServices analyticsServices;

        public HomeController(ILogger<HomeController> logger , IAnalyticsServices analyticsServices )
        {
            this.logger = logger;
            this.analyticsServices = analyticsServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Data =await analyticsServices.GetAnalyticsDataAsync(ct);
            return View(Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
