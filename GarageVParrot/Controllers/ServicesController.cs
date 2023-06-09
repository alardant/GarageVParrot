using System;
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

        [HttpGet]
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
            var serviceViewModel = new ServiceViewModel { UserId = curUserId };
            return View(serviceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel serviceVM)
        {
            if (ModelState.IsValid)
            {
                try { 
                string imageFileName = null;
                if (serviceVM.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/ServicesImage");
                    string fileName = Guid.NewGuid().ToString() + " - " + serviceVM.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    await serviceVM.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    imageFileName = fileName;
                }
                var service = new Service
                {
                    UserId = serviceVM.UserId,
                    Title = serviceVM.Title,
                    Description = serviceVM.Description,
                    Image = imageFileName
                };

                await _context.AddAsync(service);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Le service a bien été crée.";
                return RedirectToAction("Index");

                } catch(Exception ex) {
                    TempData["Message"] = "Échec de la création du service, veuillez réessayer.";
                    return RedirectToAction("Index");
                }
            }
            TempData["Message"] = "Échec de la création du service, veuillez réessayer.";
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
                Image = service.Image,
            };
            return View(serviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditServiceViewModel serviceVM, IFormFile? file)
        {
            if (id != serviceVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                string imageFileName = null;

                if (file != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/ServicesImage");
                    string fileName = Guid.NewGuid().ToString() + " - " + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    imageFileName = fileName;

                    if (service.Image != null)
                    {
                        string dir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/ServicesImage");
                        string oldFilePath = Path.Combine(dir, service.Image);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }


                }
                else
                {
                    imageFileName = service.Image;
                }

                try
                {
                    var serviceToUpdate = new Service
                    {
                        Id = id,
                        UserId = serviceVM.UserId,
                        Title = serviceVM.Title,
                        Description = serviceVM.Description,
                        Image = imageFileName
                    };
                    _context.Update(serviceToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["Message"] = "Échec de la modification du service, veuillez réessayer.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                TempData["Message"] = "Le service a bien été modifié.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Message"] = "Échec de la modification du service, veuillez réessayer.";
            return RedirectToAction(nameof(Index));
        }

/*        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
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
            return View(service);
        }*/

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Reviews == null)
            {
                TempData["Message"] = "Échec de la suppression du service, veuillez réessayer.";
            }

            var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            try { 
            if (service.Image != null)
            {
                string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/ServicesImage");
                string oldFilePath = Path.Combine(uploadDir, service.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            if (service != null)
            {
                _context.Services.Remove(service);
            }

                await _context.SaveChangesAsync();
                TempData["Message"] = "Le service a bien été supprimé.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la service du véhicule, veuillez réessayer.";
                return RedirectToAction(nameof(Index));
            }
        }
        private bool ServiceExists(int id)
        {
            return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
