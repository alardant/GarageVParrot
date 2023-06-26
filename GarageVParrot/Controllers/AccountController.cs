using GarageVParrot.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GarageVParrot.Models;
using GarageVParrot.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace GarageVParrot.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            TempData["ErrorMessage"] = "Vous n'avez pas les autorisations nécessaires pour accéder à cette page.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string? message)
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                //Password is incorrect
                TempData["Message"] = "Échec de identification, veuillez réessayer.";
                return View(loginViewModel);
            }
            //User not found
            TempData["Message"] = "Échec de l'identification, veuillez réessayer.";
            return View(loginViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Register()
        {
            var response = new AccountViewModel();
            return View(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Register(AccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid) 
            {
                TempData["Message"] = "Échec de la création de l'employé, veuillez réessayer.";
                return View(accountViewModel);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(accountViewModel.EmailAddress);
                if (user != null)
                {
                    TempData["Message"] = "Échec de la création de l'employé, cette adresse email est déjà utilisée.";
                    return View(accountViewModel);
                }

                // set user's role
                bool isAdmin = accountViewModel.Role;
                string role = isAdmin ? UserRoles.Admin : UserRoles.User;

                //create the new user
                var newUser = new User()
                {
                    Email = accountViewModel.EmailAddress,
                    UserName = accountViewModel.EmailAddress,
                    Role = role
                };
                var newUserResponse = await _userManager.CreateAsync(newUser, accountViewModel.Password);

                //add user's role
                if (newUserResponse.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, role);
                }

                TempData["Message"] = "L'employé a bien été crée.";
                return RedirectToAction("Index");

            } catch (Exception ex)
            {
                TempData["Message"] = "Échec de la création de l'employé, veuillez réessayer.";
                return View(accountViewModel);
            }
    }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Role = roles.FirstOrDefault();
            }

            return View(users);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.Reviews == null || !ModelState.IsValid)
            {
                TempData["Message"] = "Échec de la suppression de l'employé, veuillez réessayer.";
                return RedirectToAction("Index");
            }

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
                //delete user
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "L'employé a bien été supprimé.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Échec de la suppression de l'employé, veuillez réessayer.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Échec de la suppression de l'employé, veuillez réessayer.";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            var account = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (id == null || account == null)
            {
                return NotFound();
            }

            var accountVM = new EditAccountViewModel
            {
                EmailAddress = account.Email,
                CurrentPassword = account.PasswordHash,
                Role = account.Role == "admin" ? true : false
            };
            return View(accountVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(string? id, EditAccountViewModel editaccountVM)
        {
            var account = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);

            if (id != account.Id ||account == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
            TempData["Message"] = "Échec de la modification du compte, veuillez réessayer.";
            return View(editaccountVM);
            }

            try
            {
                //check if the current password is true
                var passwordCheck = await _userManager.CheckPasswordAsync(account, editaccountVM.CurrentPassword);
                if (!passwordCheck)
                {
                    TempData["Message"] = "Échec de la modification, veuillez réessayer.";
                    return View(editaccountVM);
                }

                //set the new role and get the old role
                var oldRole = account.Role;
                bool isAdmin = editaccountVM.Role;
                string role = isAdmin ? UserRoles.Admin : UserRoles.User;

                // if a new password is set, update with the new information
                if (editaccountVM.NewPassword != null) 
                { 
                    //update the new password new password
                    var newUserResponse = await _userManager.ChangePasswordAsync(account, editaccountVM.CurrentPassword, editaccountVM.NewPassword);

                    if (newUserResponse.Succeeded)
                    {
                        account.Email = editaccountVM.EmailAddress;
                        account.UserName = editaccountVM.EmailAddress;
                        account.Role = role;

                        await _userManager.UpdateAsync(account);
                        await _userManager.RemoveFromRoleAsync(account, oldRole);
                        await _userManager.AddToRoleAsync(account, role);
                    }
                } else
                //if no new password is set, update with the new information
                {
                    account.Email = editaccountVM.EmailAddress;
                    account.UserName = editaccountVM.EmailAddress;
                    account.Role = role;

                    await _userManager.UpdateAsync(account);
                    await _userManager.RemoveFromRoleAsync(account, oldRole);
                    await _userManager.AddToRoleAsync(account, role);
                }
                    
                TempData["Message"] = "L'employé a bien été modifié.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Message"] = "Échec de la modification du compte, veuillez réessayer.";
                return View(editaccountVM);
            }
        }
    }
}