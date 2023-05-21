﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageVParrot.Data;
using GarageVParrot.Models;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity;
using GarageVParrot.ViewModels;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace GarageVParrot.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public ServicesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var listService = await _context.Services.ToListAsync();
            return View(listService);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            var ServiceViewModel = new ServiceViewModel { UserId = curUserId };
            return View(ServiceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel serviceVM)
        {
            if (ModelState.IsValid)
            {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ServicesImage");
                    string fileName = Guid.NewGuid().ToString() + " - " + serviceVM.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    serviceVM.Image.CopyTo(new FileStream(filePath, FileMode.Create));

                var service = new Service
                {
                    UserId = serviceVM.UserId,
                    Title = serviceVM.Title,
                    Description = serviceVM.Description, 
                    Image = fileName
                };

                await _context.AddAsync(service);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FirstOrDefaultAsync(i => i.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            var serviceVM = new EditServiceViewModel
            {
                UserId = service.UserId,
                Title = service.Title,
                Description = service.Description,
                URL = service.Image,
            };
            return View(serviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditServiceViewModel serviceVM)
        {
            if (id != serviceVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var serviceImage = await _context.Services.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                if (serviceImage.Image != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ServicesImage");
                    string oldFilePath = Path.Combine(uploadDir, serviceImage.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                try { 
                string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ServicesImage");
                string fileName = Guid.NewGuid().ToString() + "-" + serviceVM.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                    serviceVM.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    var service = new Service
                    {
                    Id = id,
                    UserId = serviceVM.UserId,
                    Title = serviceVM.Title,
                    Description = serviceVM.Description,
                    Image = fileName
                };
                    _context.Update(service);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_context.Services?.Any(e => e.Id == id)).GetValueOrDefault())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceVM);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
