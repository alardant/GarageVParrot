﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageVParrot.Data;
using GarageVParrot.Models;
using GarageVParrot.ViewModels;
using Microsoft.Extensions.Hosting;

namespace GarageVParrot.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var listCar = await _context.Cars.ToListAsync();
            return View(listCar);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpGet]
        public IActionResult Create()
        {

            var curUserId = HttpContext.User.GetUserId();
            var carVM = new CarViewModel { UserId = curUserId };
            return View(carVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                string coverImageFileName = null;
                List<ImageListCar> listImageFileName = new List<ImageListCar>();

                if (carViewModel.CoverImage != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageCover");
                    string fileName = Guid.NewGuid().ToString() + " - " + carViewModel.CoverImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    await carViewModel.CoverImage.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    coverImageFileName = fileName;
                }

                if (carViewModel.ImageListCar != null && carViewModel.ImageListCar.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                    foreach (IFormFile imageFile in carViewModel.ImageListCar)
                    {
                        string fileName = Guid.NewGuid().ToString() + " - " + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        await imageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

                        ImageListCar image = new ImageListCar
                        {
                            ImageName = imageFile.FileName,
                            ImagePath = fileName,
                            CarId = carViewModel.Id
                        };

                        listImageFileName.Add(image);
                    }
                }

                var car = new Car
                {
                    Price = carViewModel.Price,
                    CoverImage = coverImageFileName,
                    ImageListCar = listImageFileName,
                    Year = carViewModel.Year,
                    Kilometers = carViewModel.Kilometers,
                    Brand = carViewModel.Brand,
                    Model = carViewModel.Model,
                    NumberOfDoors = carViewModel.NumberOfDoors,
                    NumberOfSeats = carViewModel.NumberOfSeats,
                    AirConditionner = carViewModel.AirConditionner,
                    Power = carViewModel.Power,
                    Motor = carViewModel.Motor,
                    Bluetooth = carViewModel.Bluetooth,
                    Gps = carViewModel.Gps,
                    SpeedRegulator = carViewModel.SpeedRegulator,
                    Airbags = carViewModel.Airbags,
                    ReversingRadar = carViewModel.ReversingRadar,
                    CritAir = carViewModel.CritAir,
                    Warranty = carViewModel.Warranty,
                    Abs = carViewModel.Abs,
                    Energy = carViewModel.Energy,
                    Category = carViewModel.Category,
                    GearType = carViewModel.GearType,
                    UserId = carViewModel.UserId,
                };

                await _context.AddAsync(car);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", car.UserId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,CoverImage,Year,Kilometers,Brand,Model,NumberOfDoors,NumberOfSeats,AirConditionner,Power,Motor,Bluetooth,Gps,SpeedRegulator,Airbags,ReversingRadar,CritAir,Warranty,Abs,Energy,Category,GearType,UserId")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", car.UserId);
            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(i => i.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, ImageListCar imageListCar)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cars'  is null.");
            }

            var car = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (car.CoverImage != null)
            {
                string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageCover");
                string oldFilePath = Path.Combine(uploadDir, car.CoverImage);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            if (ImagesListCarExists(id))
            {
                List<ImageListCar> imageList = _context.ImagesListCar.Where(image => image.CarId == id).ToList();
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                foreach (var imageFile in imageList)
                {
                    string oldFilePath = Path.Combine(uploadsFolder, imageFile.ImagePath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }

            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool CarExists(int id)
        {
            return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool ImagesListCarExists(int id)
        {
            return (_context.ImagesListCar?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
} 
