using Microsoft.AspNetCore.Mvc;

namespace HotelReservationManager.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
