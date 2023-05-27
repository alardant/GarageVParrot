using GarageVParrot.Data;
using GarageVParrot.Models;
using GarageVParrot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GarageVParrot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _context = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var openhours = await _context.OpenHours.FirstOrDefaultAsync();
            var services = await _context.Services.ToListAsync();
            var reviews = await _context.Reviews.Where(i=> i.Rating >= 4).ToListAsync();
            var HomeVM = new HomeViewModel
            {
                Reviews = reviews,
                Services = services,
            };
            return View(HomeVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}