using HotelReservationManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly HotelReservationContext _context;

        public ReservationsController(HotelReservationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var reservations = _context.Reservations.ToList();
            return View(reservations);
        }
    }
}
