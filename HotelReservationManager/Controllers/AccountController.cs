using HotelReservationManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelReservationContext _context;              // Database context
        private readonly UserManager<User> _userManager;  // Handles user operations
        private readonly SignInManager<User> _signInManager; // Handles authentication

        // Constructor with dependency injection
        public AccountController(
            HotelReservationContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Displays the registration form
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Username is already taken.");
                    return View(model);
                }

                // Check if email already exists
                var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userWithSameEmail != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(model);
                }

                // Create new user from the registration data
                var user = new User
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };

                // Attempt to create the user in the database
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Automatically sign in the user after successful registration
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["SuccessMessage"] = "Registration successful! You are now logged in.";
                    return RedirectToAction("Index", "Home");
                }

                // If registration fails, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        // Displays the login form
        public IActionResult Login()
        {
            return View();
        }

        // Handles the login form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find user by email
                var users = await _userManager.Users.ToListAsync();
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null)
                {
                    // Attempt to sign in the user
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        model.Password,
                        isPersistent: true, // Enables "Remember me" functionality
                        lockoutOnFailure: true); // Enables account lockout for security

                    if (result.Succeeded)
                    {
                        // Successful login
                        TempData["SuccessMessage"] = "Successfully logged in!";
                        return RedirectToAction("Index", "Home");
                    }
                    if (result.IsLockedOut)
                    {
                        // Handle locked out accounts
                        ModelState.AddModelError("", "Account is locked out. Please try again later.");
                        return View(model);
                    }
                }

                // Generic error message for security
                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(model);
        }


        // Handles user logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
