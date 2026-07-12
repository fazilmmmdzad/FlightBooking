using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        public IActionResult CreateBooking()
        {
            return View();
        }

        public IActionResult BookingList()
        {
            return View();
        }
    }
}
