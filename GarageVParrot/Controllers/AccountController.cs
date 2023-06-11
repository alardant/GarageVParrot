using GarageVParrot.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GarageVParrot.Models;
using GarageVParrot.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Login()
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
                TempData["Error"] = "Échec de identification, veuillez réessayer.";
                return View(loginViewModel);
            }
            //User not found
            TempData["Error"] = "Échec de l'identification, veuillez réessayer.";
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) 
            { 
                try
                {
                    var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
                    if (user != null)
                    {
                        TempData["Message"] = "Échec de la création de l'employé, cette adresse email est déjà utilisée.";
                        return View(registerViewModel);
                    }

                    bool isAdmin = registerViewModel.Role;
                    string role = isAdmin ? UserRoles.Admin : UserRoles.User;

                    var newUser = new User()
                    {
                        Email = registerViewModel.EmailAddress,
                        UserName = registerViewModel.EmailAddress,
                        Role = role
                    };
                    var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                    if (newUserResponse.Succeeded)
                    {
                        string userRole = isAdmin ? UserRoles.Admin : UserRoles.User;
                        await _userManager.AddToRoleAsync(newUser, userRole);
                    }
                    TempData["Message"] = "L'employé a bien été crée.";

                    return RedirectToAction("Index");
                } catch (Exception ex)
                {
                    TempData["Message"] = "Échec de la création de l'employé, veuillez réessayer.";
                    return View(registerViewModel);
                }
            }
            TempData["Message"] = "Échec de la création de l'employé, veuillez réessayer.";
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.Reviews == null)
            {
                TempData["Message"] = "Échec de la suppression de l'employé, veuillez réessayer.";
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
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
            TempData["Message"] = "Échec de la suppression de l'employé, veuillez réessayer.";
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id, User user)
        {
            return View();
        }
    }
}