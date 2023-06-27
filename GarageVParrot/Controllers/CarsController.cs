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
            var listCar = await _context.Cars.ToListAsync();
            return View(listCar);
        }

        [HttpPost]
        public async Task<IActionResult> GetFilteredCars(List<string>? brandList, List<string>? modelList, List<Category>? categoryList, List<Energy>? energyList, List<GearType>? selectedGears, List<int>? critairList, bool hasAirConditionner, bool hasBluetooth, bool hasReversingRadar, bool hasGPS, bool hasSpeedRegulator, bool hasABS, int? MinYear, int? MaxYear, int? MinPrice, int? MaxPrice, int? MinKm, int? MaxKm)
        {

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Les filtres n'ont pas pu être appliqués, veuillez réessayer.";
                var carsList = await _context.Cars.ToListAsync();
                return PartialView("_CarList", carsList);
            }

            var cars = await _context.Cars.Where(i =>
            (brandList.Count() != 0 ? brandList.Contains(i.Brand) : true) && 
            (modelList.Count() != 0 ? modelList.Contains(i.Model) : true) &&
            (categoryList.Count() != 0 ? categoryList.Contains(i.Category) : true) &&
            (energyList.Count() != 0 ? energyList.Contains(i.Energy) : true) &&
            (selectedGears.Count() != 0 ? selectedGears.Contains(i.GearType) : true) &&
            (critairList.Count() != 0 ? critairList.Contains(i.CritAir) : true) && 
            (hasAirConditionner ? i.AirConditionner : true ) &&
            (hasBluetooth ? i.Bluetooth : true) &&
            (hasReversingRadar ? i.ReversingRadar : true) &&
            (hasGPS ? i.Gps : true) &&
            (hasSpeedRegulator ? i.SpeedRegulator : true) &&
            (hasABS ? i.Abs : true) &&
            (MinYear != null ? i.Year >= MinYear : true) &&
            (MaxYear != null ? i.Year <= MaxYear : true) &&
            (MinPrice != null ? i.Price <= MinPrice : true) &&
            (MaxPrice != null ? i.Price <= MaxPrice : true) &&
            (MinKm != null ? i.Kilometers <= MinKm : true) &&
            (MaxKm != null ? i.Kilometers <= MaxKm : true)
            ).ToListAsync();

            if(cars.Count() == 0)
            {
                TempData["Message"] = "Aucun véhicule ne correspond à votre recherche.";
            }
            return PartialView("_CarList", cars);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CarManagement(string searchString)
        {
            var listCar = _context.Cars.AsQueryable();

            //display only the requested item form the search bar input
            if (!String.IsNullOrEmpty(searchString))
            {
                //search if the input is an int - convert the input in int if possible
                if (int.TryParse(searchString, out int searchNumber))
                {
                    listCar = listCar.Where(i => i.Brand.Contains(searchString) || i.Model.Contains(searchString) || i.Year == searchNumber || (int)i.Kilometers == searchNumber);
                }
                //search if the input is a string or if conversion fails
                else
                {
                    listCar = listCar.Where(i => i.Brand.Contains(searchString) || i.Model.Contains(searchString));
                }
            }

            var result = await listCar.ToListAsync();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(i => i.Id == id);
            var imageListCar = await _context.ImagesListCar.Where(i => i.CarId == id).ToListAsync();

            var carVM = new DetailCarViewModel
            {
                Car = car,
				ImageListCar = imageListCar
			};

            if (car == null)
            {
                return NotFound();
            }
            return View(carVM);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {

            var curUserId = HttpContext.User.GetUserId();
            var carVM = new CarViewModel { UserId = curUserId };
            return View(carVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la création du véhicule, veuillez réessayer.";
                return View(carViewModel);
            }

            try { 
                string coverImageFileName = null;
                List<ImageListCar> listImageFileName = new List<ImageListCar>();

                //upload car's cover image
                if (carViewModel.CoverImage != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageCover");
                    //create a unique name
                    string fileName = Guid.NewGuid().ToString() + "-" + carViewModel.CoverImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    await carViewModel.CoverImage.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    coverImageFileName = fileName;
                }

                //upload car's other images
                if (carViewModel.ImageListCar != null && carViewModel.ImageListCar.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                    foreach (IFormFile imageFile in carViewModel.ImageListCar)
                    {
                        string fileName = Guid.NewGuid().ToString() + "-" + imageFile.FileName;
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

                //set a new car
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
                return RedirectToAction("CarManagement");
            } catch (Exception ex) {
                TempData["Message"] = "Échec de la création du véhicule, veuillez réessayer.";
                return View(carViewModel);
            }
        }


        [Authorize]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarViewModel carVM, IFormFile? file, IFormFileCollection files)
        {
            if (id != carVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la modification du véhicule, veuillez réessayer.";
                return RedirectToAction(nameof(CarManagement));
            }

            //set car to update with its images
            var car = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            string coverImageFileName = null;
            List<ImageListCar> listImageFileName = new List<ImageListCar>();
            
            //cover image management if a new is set
            if (file != null)
            {
                //upload new cover image
                string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageCover");
                string fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

                coverImageFileName = fileName;

                //delete old cover image if exists
                if (car.CoverImage != null)
                {
                    string oldFilePath = Path.Combine(uploadDir, car.CoverImage);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            else
            //if no new cover image keep the old cover image
            {
                coverImageFileName = car.CoverImage;
            }

            //other images management if a new list is set
            if (files != null && files.Count > 0)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                foreach (IFormFile imageFile in files)
                {
                    //upload new images
                    string fileName = Guid.NewGuid().ToString() + "-" + imageFile.FileName;
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
                // delete old other images
                if (ImagesListCarExists(id))
                {
                    string uploadsPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageList");
                    _context.ImagesListCar.RemoveRange(car.ImageListCar);
                    foreach (var imageFile in car.ImageListCar)
                    {
                        string oldFilePath = Path.Combine(uploadsPath, imageFile.ImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
            }

            //update the car
            try
            {
                car.Price = carVM.Price;
                car.CoverImage = coverImageFileName;
                car.ImageListCar = listImageFileName;
                car.Year = carVM.Year;
                car.Kilometers = carVM.Kilometers;
                car.Brand = carVM.Brand;
                car.Model = carVM.Model;
                car.NumberOfDoors = carVM.NumberOfDoors;
                car.NumberOfSeats = carVM.NumberOfSeats;
                car.AirConditionner = carVM.AirConditionner;
                car.Power = carVM.Power;
                car.Motor = carVM.Motor;
                car.Bluetooth = carVM.Bluetooth;
                car.Gps = carVM.Gps;
                car.SpeedRegulator = carVM.SpeedRegulator;
                car.Airbags = carVM.Airbags;
                car.ReversingRadar = carVM.ReversingRadar;
                car.CritAir = carVM.CritAir;
                car.Warranty = carVM.Warranty;
                car.Abs = carVM.Abs;
                car.Energy = carVM.Energy;
                car.Category = carVM.Category;
                car.GearType = carVM.GearType;
                car.UserId = carVM.UserId;

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
                    TempData["Message"] = "Échec de la modification du véhicule, veuillez réessayer.";
                    return RedirectToAction(nameof(CarManagement));
                }
            }
            TempData["Message"] = "Le véhicule a bien été modifié.";
            return RedirectToAction(nameof(CarManagement));
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Cars == null || !ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la suppression du véhicule, veuillez réessayer.";
                return RedirectToAction(nameof(CarManagement));
            }

            try
            {
            var car = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

                // delete cover image from upload and database if exists
                if (car.CoverImage != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads/CarImageCover");
                    string oldFilePath = Path.Combine(uploadDir, car.CoverImage);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                // delete ohter images from upload and database if exists
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

                //delete car
                if (car != null)
                {
                    _context.Cars.Remove(car);
                }

                await _context.SaveChangesAsync();
                TempData["Message"] = "Le véhicule a bien été supprimé.";
                return RedirectToAction(nameof(CarManagement));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la suppression du véhicule, veuillez réessayer.";
                return RedirectToAction(nameof(CarManagement));
            }
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
