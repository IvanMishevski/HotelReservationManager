using Microsoft.AspNetCore.Mvc;

namespace HotelReservationManager.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult About()
        {
            return View("About");
        }
        public IActionResult Contacts()
        {
            return View("Contacts");
        }
    }
}
