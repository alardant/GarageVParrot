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
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GarageVParrot.Controllers
{
    [Authorize(Roles = "admin")]
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
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la création du service, veuillez réessayer.";
                return View(serviceVM);
            }

            try 
            { 
                string imageFileName = null;

                // upload image if exists
                if (serviceVM.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/ServicesImage");
                    // create unique name
                    string fileName = Guid.NewGuid().ToString() + "-" + serviceVM.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    await serviceVM.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    imageFileName = fileName;
                }

                //create service
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

            } catch(Exception ex) 
            {
                TempData["Message"] = "Échec de la création du service, veuillez réessayer.";
                return View(serviceVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            //get the service with the id
            var service = await _context.Services.FirstOrDefaultAsync(i => i.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            // create a EditServiceViewModel and transfer it to the view
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

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la modification du service, veuillez réessayer.";
                return View(serviceVM);
            }

            try
            {
                var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                string imageFileName = null;

                // service's image management if new image exists
                if (file != null)
                {
                    // upload new image
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/ServicesImage");
                    string fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    imageFileName = fileName;

                    //remove old image
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
                // keep old image if no new image
                else
                {
                    imageFileName = service.Image;
                }

                //update service
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
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la modification du service, veuillez réessayer.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Message"] = "Le service a bien été modifié.";
            return View(serviceVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Reviews == null || !ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la suppression du service, veuillez réessayer.";
                return RedirectToAction(nameof(Index));
            }

            try
            {

            var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            // delete image if exists
            if (service.Image != null)
            {
                string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/ServicesImage");
                string oldFilePath = Path.Combine(uploadDir, service.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            //delete service
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            await _context.SaveChangesAsync();
            TempData["Message"] = "Le service a bien été supprimé.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la service du véhicule, veuillez réessayer.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
