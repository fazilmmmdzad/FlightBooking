using FlightBooking.Services.MachineLearningServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ForecastController : Controller
    {
        private readonly MongoFlightDataService _mongoFlightDataService;
        private readonly FlightMlService _flightMlService;

        public ForecastController(MongoFlightDataService mongoFlightDataService, FlightMlService flightMlService)
        {
            _mongoFlightDataService = mongoFlightDataService;
            _flightMlService = flightMlService;
        }

        public async Task<IActionResult> TrainModel()
        {
            var mlData = await _mongoFlightDataService.ConvertToMlDataAsync();

            if (mlData.Count == 0)
                return Content("ML Data is empty.");

            _flightMlService.Train(mlData);
            
            return View();
        }
    }
}
