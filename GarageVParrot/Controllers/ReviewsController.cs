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

        [HttpGet]
        public async Task<IActionResult> Validate()
        {
            var reviewList = await _context.Reviews.AsNoTracking().Where(i => i.Accepted == false).ToListAsync();
            return View(reviewList);
        }

        [HttpPost]
        public async Task<IActionResult> Validate(int id, ReviewViewModel reviewVM)
        {
            var reviewList = await _context.Reviews.Where(i => i.Accepted == false).ToListAsync();

            if (ModelState.IsValid)
            {
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
                    TempData["Message"] = "Le témoignage a bien été modifié.";
                    var reviewListUpadted = await _context.Reviews.AsNoTracking().Where(i => i.Accepted == false).ToListAsync();
                    return View(reviewListUpadted);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_context.Reviews?.AsNoTracking().Any(e => e.Id == id)).GetValueOrDefault())
                    {
                        TempData["Message"] = "Échec de la modification du témoignage, veuillez réessayer.";
                        return View(reviewList);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            TempData["Message"] = "Échec de la modification du témoignage, veuillez réessayer.";
            return View(reviewList);
        }

        [HttpGet]
        public IActionResult Create()

        {
            var reviewViewModel = new ReviewViewModel();
            reviewViewModel.UserId = User.Identity.IsAuthenticated ? HttpContext.User.GetUserId() : null;
            reviewViewModel.datePublished = DateTime.Now;
            return View(reviewViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel reviewVM)
        {

            if (ModelState.IsValid)
            {
                try
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
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Échec de la création de votre témoignage, veuillez réessayer.";
                }
                ModelState.Clear();
                return RedirectToAction("Index");
                
            }
            TempData["Message"] = "Échec de la création de votre témoignage, veuillez réessayer.";
            return RedirectToAction("Index");
        }

/*        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FirstOrDefaultAsync(i => i.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            var reviewVM = new ReviewViewModel
            {
                UserId = review.UserId,
                Name = review.Name,
                Description = review.Description,
                Rating = review.Rating,
                Accepted = review.Accepted,
            };
            return View(reviewVM);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReviewViewModel reviewViewModel)
        {
            if (id != reviewViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var reviewToUpdate = new Review
                    {
                        Id = id,
                        UserId = reviewViewModel.UserId,
                        Name = reviewViewModel.Name,
                        Description = reviewViewModel.Description,
                        Rating = reviewViewModel.Rating,
                        Accepted = reviewViewModel.Accepted,
                    };
                    _context.Update(reviewToUpdate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault())
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
            return View(reviewViewModel);
        }*/

        // POST: Reviews/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Reviews == null)
            {
                TempData["Message"] = "Échec de la suppression du témoignage, veuillez réessayer.";
            }
            try
            {
                var review = await _context.Reviews.FirstOrDefaultAsync(i => i.Id == id);
                if (review != null)
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Le témoignage a bien été supprimé.";
                } else
                {
                    TempData["Message"] = "Échec de la suppression du témoignage, veuillez réessayer.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la suppression du témoignage, veuillez réessayer.";
            }
            string referrerUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referrerUrl))
            {
                return Redirect(referrerUrl);
            }
            else
            {
                return RedirectToAction("Index"); // ou une autre action par défaut si le referer est manquant
            }
        }

        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
