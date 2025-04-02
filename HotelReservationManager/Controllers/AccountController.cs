using HotelReservationManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        // Handles the registration form submission
        

        // Displays the login form
        public IActionResult Login()
        {
            return View();
        }

        // Handles the login form submission
       

        // Handles user logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
