using FlightBooking.Dtos.CheckInDtos;
using FlightBooking.Services.BookingServices;
using FlightBooking.Services.CheckInServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CheckInController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ICheckInService _checkInService;
        public CheckInController(IBookingService bookingService, ICheckInService checkInService)
        {
            _bookingService = bookingService;
            _checkInService = checkInService;
        }
        //public async Task<IActionResult> Index(string id)
        //{
        //    ViewBag.FlightNumber = TempData.Peek("FlightNumber");
        //    ViewBag.DepartureTime = TempData.Peek("DepartureTime");
        //    ViewBag.ArrivalTime = TempData.Peek("ArrivalTime");

        //    var passenger = await _bookingService.GetPassengerNameByIdAsync(id);
        //    var pnrNumber = await _bookingService.GetPnrByPassengerIdAsync(id);
        //    var gate = await _bookingService.GetGateByPassengerIdAsync(id);

        //    ViewBag.Name = passenger.Name;
        //    ViewBag.Surname = passenger.Surname;
        //    ViewBag.PassengerName = passenger.Name + " " + passenger.Surname;
        //    ViewBag.PnrNumber = pnrNumber;
        //    ViewBag.Gate = gate;

        //    return View();
        //}

        public async Task<IActionResult> Index(string id)
        {
            ViewBag.FlightNumber = TempData["FlightNumber"];
            ViewBag.DepartureTime = TempData["DepartureTime"];
            ViewBag.ArrivalTime = TempData["ArrivalTime"];
            ViewBag.AirlineCode = TempData["AirlineCode"];          
            ViewBag.DepartureAirportCode = TempData["DepartureAirportCode"];
            ViewBag.DepartureAirportName = TempData["DepartureAirportName"];
            ViewBag.ArrivalAirportCode = TempData["ArrivalAirportCode"];
            ViewBag.ArrivalAirportName = TempData["ArrivalAirportName"];
            ViewBag.BasePrice = TempData["BasePrice"];
            ViewBag.Currency = TempData["Currency"];

            var passenger = await _bookingService.GetPassengerNameByIdAsync(id);
            var pnrNumber = await _bookingService.GetPnrByPassengerIdAsync(id);
            var gate = await _bookingService.GetGateByPassengerIdAsync(id);
            //var flightId = await _bookingService.GetFlightIdByPassengerIdAsync(id); 

            ViewBag.Name = passenger.Name;
            ViewBag.Surname = passenger.Surname;
            ViewBag.PassengerName = passenger.Name + " " + passenger.Surname;
            ViewBag.PnrNumber = pnrNumber;
            ViewBag.Pnr = pnrNumber;   
            ViewBag.Gate = gate;

            ViewBag.PassengerId = id;
            ViewBag.FlightId = "6a4cfb50067fe1c02895c69c";

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(CompleteCheckInDto completeCheckInDto)
        {
            await _checkInService.CompleteCheckInAsync(completeCheckInDto);
            return RedirectToAction("Test");
        }
    }
}
