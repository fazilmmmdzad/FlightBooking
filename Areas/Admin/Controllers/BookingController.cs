using FlightBooking.Services.FlightServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly IFlightService _flightService;
        public BookingController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateBooking(string id)
        {
            var value = await _flightService.GetFlightByIdAsync(id);
            ViewBag.id = id;
            ViewBag.FlightNumber = value.FlightNumber;
            ViewBag.DepartureAirportCode = value.DepartureAirportCode;
            ViewBag.DepartureAirportName = value.DepartureAirportName;
            ViewBag.ArrivalAirportCode = value.ArrivalAirportCode;
            ViewBag.ArrivalAirportName = value.ArrivalAirportName;
            ViewBag.DepartureTime = value.DepartureTime;
            ViewBag.ArrivalTime = value.ArrivalTime;
            ViewBag.AirlineCode = value.AirlineCode;
            return View();
        }
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
