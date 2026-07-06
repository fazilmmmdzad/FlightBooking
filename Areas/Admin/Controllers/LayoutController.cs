using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LayoutController : Controller
    {
        public IActionResult AdminLayout()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateFlight()
        {
            return View();
        }
    }
}
