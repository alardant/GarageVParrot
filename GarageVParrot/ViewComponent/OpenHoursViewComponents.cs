using GarageVParrot.Models;
using GarageVParrot.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GarageVParrot.ViewComponents
{
    public class OpenHoursViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public OpenHoursViewComponent(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var openHours = await _context.OpenHours.FirstOrDefaultAsync();

            return View("_Default", openHours);
        }
    }
}