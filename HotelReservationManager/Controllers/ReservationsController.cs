using HotelReservationManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace HotelReservationManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ReservationsController> _logger;

        public ReservationsController(HotelReservationContext context, UserManager<User> userManager, ILogger<ReservationsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var reservations = _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Room)
                    .Include(r => r.ReservationClients)
                    .ThenInclude(rc => rc.Client)
                    .ToList();

                // Get the current user ID
                string currentUserId = _userManager.GetUserId(User);

                // Create a dictionary of room IDs that the current user has reservations for
                Dictionary<int, bool> userReservationsByRoom = new Dictionary<int, bool>();

                // Get all rooms
                var allRooms = _context.Rooms.Select(r => new { r.Id, r.IsAvailable }).ToList();


                // Initialize dictionary with all rooms set to false (no reservation)
                foreach (var room in allRooms)
                {
                    userReservationsByRoom[room.Id] = !room.IsAvailable;
                }

                // Mark rooms that the user has reservations for
                if (!string.IsNullOrEmpty(currentUserId))
                {
                    var userReservations = _context.Reservations
                        .Where(r => r.UserId == currentUserId && r.CheckOutDate >= DateTime.Now)
                        .Select(r => r.RoomId)
                        .ToList();

                    foreach (var roomId in userReservations)
                    {
                        userReservationsByRoom[roomId] = true;
                    }
                }

                ViewBag.UserReservationsByRoom = userReservationsByRoom;
                ViewBag.CurrentUserId = currentUserId;
                ViewBag.isOwner = reservations.Any(r => r.User.Id == currentUserId);

                return View(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving reservations");
                TempData["ErrorMessage"] = "An error occurred while retrieving reservations. Please try again later.";
                return View(new List<Reservation>());
            }
        }

        [HttpPost]
        public IActionResult Create(Reservation reservation, string RoomId)
        {

            try
            {
                if(reservation.CheckInDate > reservation.CheckOutDate)
                {
                    throw new Exception("Check-in date must be before check-out date.");
                }
                    string userId = _userManager.GetUserId(User);
                if (RoomId != null && userId != null)
                {
                    // Just set the foreign keys, not the navigation properties
                    reservation.RoomId = int.Parse(RoomId);
                    reservation.UserId = userId;

                    // Calculate the amount due properly
                    reservation.AmountDue = 0;
                    if (reservation.IsAllInclusive)
                        reservation.AmountDue += 100;
                    if (reservation.IsBreakfastIncluded)
                        reservation.AmountDue += 50;

                    // Check if a client with the same email already exists
                    var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                    var existingClient = _context.Clients.FirstOrDefault(c => c.Email == user.Email);
                    Client client;

                    if (existingClient == null)
                    {
                        // Create a new client based on the user information if not exists
                        client = new Client
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            IsAdult = true
                        };

                        // Add the new client to the context
                        _context.Clients.Add(client);
                        _context.SaveChanges(); // Save to get the client ID
                    }
                    else
                    {
                        // Use the existing client
                        client = existingClient;
                    }

                    // Add the reservation to the context
                    _context.Reservations.Add(reservation);
                    _context.SaveChanges(); // Save to get the reservation ID

                    // Create the relationship using the join entity
                    var reservationClient = new ReservationClient
                    {
                        ClientId = client.Id,
                        ReservationId = reservation.Id
                    };

                    // Add the join entity to the context
                    _context.ReservationClients.Add(reservationClient);

                    // Update room availability
                    var room = _context.Rooms.Find(int.Parse(RoomId));
                    if (room != null)
                    {
                        room.IsAvailable = false;
                        _context.Update(room);
                    }

                    // Save all changes
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Reservation created successfully!";
                    return RedirectToAction("Index", "Reservations");
                }
                else
                {
                    TempData["ErrorMessage"] = "Room ID or User ID is missing.";
                }

                return View(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating reservation");
                TempData["ErrorMessage"] = "An error occurred while creating the reservation. Please try again later.";
                ViewBag.RoomId = RoomId;
                return View(reservation);
            }
        }

        public IActionResult Create(string id)
        {
            try
            {
                ViewBag.RoomId = id; // Store the room ID in ViewBag           
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while preparing reservation creation page");
                TempData["ErrorMessage"] = "An error occurred while preparing the reservation form. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Details(string id)
        {
            var reservation = _context.Reservations.Where(r => r.Id == id)
            .Include(r => r.User)
            .Include(r => r.Room)
            .Include(r => r.ReservationClients)
            .ThenInclude(rc => rc.Client)
            .FirstOrDefault();

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
        public IActionResult Delete(string id)
        {
            var reservation = _context.Reservations.Where(r => r.Id == id)
               .Include(r => r.User)
               .Include(r => r.Room)
               .Include(r => r.ReservationClients)
               .ThenInclude(rc => rc.Client)
               .FirstOrDefault();

            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        [HttpPost]
        public IActionResult DeleteSubmit(string id)
        {
            try
            {
                var reservation = _context.Reservations
                    .Include(r => r.ReservationClients)
                    .Include(r => r.Room)
                    .FirstOrDefault(r => r.Id == id);

                if (reservation == null)
                {
                    TempData["ErrorMessage"] = "Reservation not found.";
                    return RedirectToAction("Index");
                }

                // First, remove all associated ReservationClient records
                if (reservation.ReservationClients != null)
                {
                    _context.ReservationClients.RemoveRange(reservation.ReservationClients);
                }

                // Update the room to be available again
                if (reservation.Room != null)
                {
                    reservation.Room.IsAvailable = true;
                    _context.Update(reservation.Room);
                }

                // Then remove the reservation itself
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Reservation deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting reservation");
                TempData["ErrorMessage"] = "An error occurred while deleting the reservation. Please try again later.";
                return RedirectToAction("Index");
            }
        }
    }
}