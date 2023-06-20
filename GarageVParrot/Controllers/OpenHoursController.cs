﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageVParrot.Data;
using GarageVParrot.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GarageVParrot.Controllers
{
    public class OpenHoursController : Controller
    {
        private readonly ApplicationDbContext _context;
         public OpenHoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var openHours = await _context.OpenHours.FirstOrDefaultAsync();
            if(openHours == null)
            {
                return NotFound();
            }
            return View(openHours);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        
        {
            var openHours = await _context.OpenHours.FirstOrDefaultAsync(i => i.Id == 1);
            if (openHours == null)
            {
                return NotFound();
            }
            return View(openHours);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OpenHours openHours)
        {
            if (openHours == null || _context.OpenHours == null)
            {
                return NotFound();
            }
            //get the existing open Hours
            //BEWARE : only one isntance of open hours must be create WITH THE ID 1 // THIS IS MANDATORY SO THE CODE IS FUNCTIONNAL
            openHours.Id = 1;

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la modification des horaires, veuillez réessayer.";
                return View(openHours);
            }

            _context.OpenHours.Update(openHours);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Les horaires ont bien été modifiées.";
            return View(openHours);
            }
    }
}