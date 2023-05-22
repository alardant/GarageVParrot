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

namespace GarageVParrot.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var reviewList = await _context.Reviews.ToListAsync();
            return View(reviewList);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
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
                var review = new Review
                {
                    UserId = reviewVM.UserId == null ? reviewVM.UserId : null,
                    Name = reviewVM.Name,
                    Description = reviewVM.Description,
                    Rating = reviewVM.Rating,
                    Accepted = reviewVM.Accepted,
                };

                await _context.AddAsync(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
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
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
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

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
            }
            var review = await _context.Reviews.FirstOrDefaultAsync(i => i.Id == id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
          return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
