using HotelReservationManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationManager.Controllers
{
    public class UsersController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(HotelReservationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchString, int pageSize = 10, int pageNumber = 1)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString) ||
                                        u.Email.Contains(searchString) ||
                                        u.FirstName.Contains(searchString) ||
                                        u.MiddleName.Contains(searchString) ||
                                        u.LastName.Contains(searchString));
            }

            int totalUsers = await users.CountAsync();
            var usersList = await users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(usersList);
        }

        public IActionResult Details(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }
        public IActionResult Edit(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(string id, User user)
        {
            if (ModelState.IsValid){

                var existingUser = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (existingUser == null)
                {
                    return NotFound();
                }
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.MiddleName = user.MiddleName;
                existingUser.EGN = user.EGN;
                existingUser.LastName = user.LastName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.IsActive = user.IsActive;

                _context.Update(existingUser);
                _context.SaveChanges();
                return RedirectToAction("Index", "Users");
            }
            return View(user);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult CreateAsync([Bind("UserName,Email,FirstName,MiddleName,LastName,PhoneNumber,EGN")] User user)
        {
            if (ModelState.IsValid)
            {
                // Save movie and initialize statistics
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Users");

            }
            return View();
        }
        public IActionResult Delete(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }
        [HttpPost]
        public IActionResult DeleteSubmit(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Users");
            }
            return NotFound();
        }
    }
}
