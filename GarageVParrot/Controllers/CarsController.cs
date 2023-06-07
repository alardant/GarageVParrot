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
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var openhours = await _context.OpenHours.FirstOrDefaultAsync();
            var listCar = await _context.Cars.ToListAsync();
            return View(listCar);
        }

        [HttpGet]
        public async Task<IActionResult> CarManagement()
        {
            var openhours = await _context.OpenHours.FirstOrDefaultAsync();
            var listCar = await _context.Cars.ToListAsync();
            return View(listCar);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
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
            var editCarVM = new EditCarViewModel
            {
                UserId = car.UserId,

            };

            return View(editCarVM);
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
                TempData["Message"] = "Le véhicule a bien été crée.";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Échec de la création du véhicule, veuillez réessayer.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(i => i.Id == id);
            List<ImageListCar> imageList = _context.ImagesListCar.Where(image => image.CarId == id).ToList();

            if (car == null)
            {
                return NotFound();
            }
            var editCarViewModel = new EditCarViewModel
            {
                UserId = car.UserId,
                Price = car.Price,
                CoverImage = car.CoverImage,
                ImageListCar = imageList,
                Year = car.Year,
                Kilometers = car.Kilometers,
                Brand = car.Brand,
                Model = car.Model,
                NumberOfDoors = car.NumberOfDoors,
                NumberOfSeats = car.NumberOfSeats,
                AirConditionner = car.AirConditionner,
                Power = car.Power,
                Motor = car.Motor,
                Gps = car.Gps,
                Bluetooth = car.Bluetooth,
                SpeedRegulator = car.SpeedRegulator,
                Airbags = car.Airbags,
                ReversingRadar = car.ReversingRadar,
                CritAir = car.CritAir,
                Warranty = car.Warranty,
                Abs = car.Abs,
                Energy = car.Energy,
                Category = car.Category,
                GearType = car.GearType
            };
            return View(editCarViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarViewModel carVM, IFormFile? file, IFormFileCollection files)
        {
            if (id != carVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var car = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                string coverImageFileName = null;
                List<ImageListCar> listImageFileName = new List<ImageListCar>();
                
                if (file != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageCover");
                    string fileName = Guid.NewGuid().ToString() + " - " + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    coverImageFileName = fileName;

                    if (car.CoverImage != null)
                    {
                        string dir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageCover");
                        string oldFilePath = Path.Combine(dir, car.CoverImage);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
                else
                {
                    coverImageFileName = car.CoverImage;
                }
                if (files != null && files.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                    foreach (IFormFile imageFile in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + " - " + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        await imageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

                        ImageListCar image = new ImageListCar
                        {
                            ImageName = imageFile.FileName,
                            ImagePath = fileName,
                            CarId = carVM.Id
                        };

                        listImageFileName.Add(image);
                    }
                    if (ImagesListCarExists(id))
                    {
                        var imageListCar = await _context.ImagesListCar.AsNoTracking().Where(image => image.CarId == id).ToListAsync();
                        string uploadsPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                        foreach (var imageFile in imageListCar)
                        {
                            string oldFilePath = Path.Combine(uploadsPath, imageFile.ImagePath);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                          
                            }
                                _context.Remove(imageFile);
                        }
                    }
                } 
/*                else
                {
                    // ne récupère pas la liste
                    listImageFileName = car.ImageListCar;
                }
*/
                try
                {
                    var CarToUpdate = new Car
                    {
                        Id = id,
                        Price = carVM.Price,
                        CoverImage = coverImageFileName,
                        ImageListCar = listImageFileName,
                        Year = carVM.Year,
                        Kilometers = carVM.Kilometers,
                        Brand = carVM.Brand,
                        Model = carVM.Model,
                        NumberOfDoors = carVM.NumberOfDoors,
                        NumberOfSeats = carVM.NumberOfSeats,
                        AirConditionner = carVM.AirConditionner,
                        Power = carVM.Power,
                        Motor = carVM.Motor,
                        Bluetooth = carVM.Bluetooth,
                        Gps = carVM.Gps,
                        SpeedRegulator = carVM.SpeedRegulator,
                        Airbags = carVM.Airbags,
                        ReversingRadar = carVM.ReversingRadar,
                        CritAir = carVM.CritAir,
                        Warranty = carVM.Warranty,
                        Abs = carVM.Abs,
                        Energy = carVM.Energy,
                        Category = carVM.Category,
                        GearType = carVM.GearType,
                        UserId = carVM.UserId,
                    };
                    _context.Update(CarToUpdate);
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
                TempData["Message"] = "Le véhicule a bien été modifié.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Message"] = "Échec de la modification du véhicule, veuillez réessayer.";
            return View(carVM);
        }

/*        [HttpGet]
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
        }*/

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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
                var imageList = await _context.ImagesListCar.Where(image => image.CarId == id).ToListAsync();
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                foreach (var imageFile in imageList)
                {
                    string oldFilePath = Path.Combine(uploadsFolder, imageFile.ImagePath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    _context.Remove(imageFile);
                }
            }

            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Le véhicule a bien été supprimé.";
            return RedirectToAction(nameof(Index));

        }

        private bool CarExists(int id)
        {
            return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool ImagesListCarExists(int id)
        {

            return _context.ImagesListCar.Any(image => image.CarId == id);
        }

    }
}
