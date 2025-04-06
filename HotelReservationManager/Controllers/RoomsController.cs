using HotelReservationManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelReservationManager.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly UserManager<User> _userManager;

        public RoomsController(HotelReservationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            try
            {
                var rooms = _context.Rooms.ToList();
                return View(rooms);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving rooms: " + ex.Message);
                return View(new List<Room>());
            }
        }

        public IActionResult Details(int Id)
        {
            try
            {
                var room = _context.Rooms.FirstOrDefault(r => r.Id == Id);
                if (room == null)
                {
                    return NotFound();
                }

                var user = _userManager.GetUserAsync(User).Result;
                var reservations = _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .Include(r => r.ReservationClients)
                    .ThenInclude(rc => rc.Client)
                    .ToList();

                // Check if the current user has a reservation for this room
                bool userHasReservation = false;
                if (user != null)
                {
                    userHasReservation = reservations.Any(r =>
                        r.UserId == user.Id &&
                        r.RoomId == Id);
                       
                ViewBag.isActive = user.IsActive;
                }

                ViewBag.UserHasReservation = userHasReservation;
                
                return View(room);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving room details: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
