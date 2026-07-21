using FlightBooking.MachineLearningModels;
using FlightBooking.Services.MachineLearningServices;
using FlightBooking.Services.NoShowServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ForecastController : Controller
    {
        private readonly MongoFlightDataService _mongoFlightDataService;
        private readonly FlightMlService _flightMlService;
        private readonly NoShowService _noShowService;

        public ForecastController(MongoFlightDataService mongoFlightDataService, FlightMlService flightMlService, NoShowService noShowService)
        {
            _mongoFlightDataService = mongoFlightDataService;
            _flightMlService = flightMlService;
            _noShowService = noShowService;
        }

        [HttpGet]
        public async Task<IActionResult> NoShowAnalysis()
        {
            var values = await _noShowService.GetSlotBasedNoShowRatesAsync();
            return View(values);
        }

        public async Task<IActionResult> TrainModel()
        {
            var mlData = await _mongoFlightDataService.ConvertToMlDataAsync();

            if (mlData.Count == 0)
                return Content("ML Data is empty.");

            _flightMlService.Train(mlData);
            
            return View();
        }

        public IActionResult Predict()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Predict(DateTime flightDate, string flightType)
        {
            var input = new FlightData
            {
                Month = flightDate.Month,

                DayOfWeek = (float)flightDate.DayOfWeek,

                FlightType = flightType == "Morning" ? 0 : 1
            };

            var prediction = _flightMlService.Predict(input);

            ViewBag.Result = prediction.PredictedLabel
                ? "Bu uçuş böyük ehtimal dolacaqdır."
                : "Bu uçuşda yoğunluq az görünür.";

            ViewBag.Probability = prediction.Probability;

            return View();
        }
    }
}
