using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageVParrot.Data;
using GarageVParrot.Models;

namespace GarageVParrot.Controllers
{
    public class OpenHoursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private OpenHours _openHours = new OpenHours();

        public OpenHoursController(ApplicationDbContext context)
        {
            _context = context;
            _openHours = new OpenHours
            {
                Id = 1,
                MondayOpenHours = "08:45 - 12:00 / 14:00 - 18:00",
                TuesdayOpenHours = "08:45 - 12:00 / 14:00 - 18:00",
                WednesdayOpenHours = "08:45 - 12:00 / 14:00 - 18:00",
                ThursdayOpenHours = "08:45 - 12:00 / 14:00 - 18:00",
                FridayOpenHours = "08:45 - 12:00 / 14:00 - 18:00",
                SaturdayOpenHours = "08:45 - 12:00",
                SundayOpenHours = "fermé"
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var openhours = _openHours;
            return View(openhours);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        
        {
             return View(_openHours);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OpenHours openHours)
        {
            if(ModelState.IsValid) 
            {
                openHours.Id = _openHours.Id; 
                if (openHours == null || _context.OpenHours == null || openHours.Id != 1)
                {
                    return NotFound();
                }
                var openhoursExists = openHoursExists(1);
                if (!openhoursExists) 
                {
                    _context.Add(openHours);
                }
                else
                {
                    _context.OpenHours.Update(openHours);
                }
                await _context.SaveChangesAsync();
                return View(openHours);
            }
            return View(openHours);
        }

        private bool openHoursExists(int id)
        {
            return (_context.OpenHours?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}