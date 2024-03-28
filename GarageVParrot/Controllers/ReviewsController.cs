using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageVParrot.Data;
using GarageVParrot.Models;
using GarageVParrot.ViewModels;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GarageVParrot.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reviewList = await _context.Reviews.Where(i => i.Accepted == true).ToListAsync();
            return View(reviewList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Validate(string searchString)
        {
            var listReviews = _context.Reviews.AsQueryable();

            //display only the requested item form the search bar input
            if (!String.IsNullOrEmpty(searchString))
            {
                listReviews = listReviews.Where(i => i.Name.Contains(searchString) || i.Description.Contains(searchString));

            }
            var result = await listReviews.ToListAsync();

            return View(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Validate(int id, ReviewViewModel reviewVM)
        {
            // get all the reviews
            var reviewList = await _context.Reviews.ToListAsync();

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la modification du témoignage, veuillez réessayer.";
                return View(reviewList);
            }

            if (!reviewVM.Accepted)
            {
                TempData["Message"] = "Échec de la modification du témoignagne, veuillez cocher la case Valider le témoignage.";
                return View(reviewList);
            }

            try
            {
                var reviewToUpdate = await _context.Reviews.FirstOrDefaultAsync(i => i.Id == id);
                if (reviewToUpdate == null)
                {
                    return NotFound();
                }
                reviewToUpdate.Accepted = reviewVM.Accepted;
                reviewToUpdate.UserId = reviewVM.UserId;

                _context.Update(reviewToUpdate);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Le témoignage a bien été validé.";

                //get a list updated of all the reviews
                var reviews = await _context.Reviews.AsNoTracking().ToListAsync();
                return View(reviews);
            }
            catch (Exception)
            {
                TempData["Message"] = "Échec de la modification du témoignage, veuillez réessayer.";
                return View(reviewList);
            }
        }

        [HttpGet]
        public IActionResult Create()

        {
            var reviewViewModel = new ReviewViewModel();

            //If user is connected, add his user id to the review
            reviewViewModel.UserId = User.Identity.IsAuthenticated ? HttpContext.User.GetUserId() : null;
            reviewViewModel.datePublished = DateTime.Now;
            return View(reviewViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel reviewVM)
        {

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la création de votre témoignage, veuillez réessayer.";
                return View(reviewVM);
            }

            try
            //create review
            {
                var review = new Review
                {
                    UserId = reviewVM.UserId,
                    Name = reviewVM.Name,
                    Description = reviewVM.Description,
                    Rating = reviewVM.Rating,
                    Accepted = reviewVM.Accepted,
                };

                await _context.AddAsync(review);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Votre témoignage a été soumis avec succès.";
                return RedirectToAction("Validate");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la création de votre témoignage, veuillez réessayer.";
                return View(reviewVM);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Reviews == null || !ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la suppression du témoignage, veuillez réessayer.";
                return RedirectToAction("Validate");
            }

            try
            // delete review
            {
                var review = await _context.Reviews.FirstOrDefaultAsync(i => i.Id == id);
                if (review != null)
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Le témoignage a bien été supprimé.";
                }
                else
                {
                    TempData["Message"] = "Échec de la suppression du témoignage, veuillez réessayer.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la suppression du témoignage, veuillez réessayer.";
            }
            return RedirectToAction("Validate");
        }

        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
