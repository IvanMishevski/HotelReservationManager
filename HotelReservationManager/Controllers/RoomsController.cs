using Microsoft.AspNetCore.Mvc;

namespace HotelReservationManager.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
