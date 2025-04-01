using Microsoft.AspNetCore.Mvc;

namespace HotelReservationManager.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
