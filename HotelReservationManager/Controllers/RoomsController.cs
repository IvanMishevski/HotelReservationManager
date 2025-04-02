using HotelReservationManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationManager.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelReservationContext _context;

        public RoomsController(HotelReservationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }
    }
}
