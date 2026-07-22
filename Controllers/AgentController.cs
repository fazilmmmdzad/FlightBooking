using FlightBooking.AgentServices;
using FlightBooking.Dtos.AgentDtos;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    public class AgentController : Controller
    {
        private readonly ITravelAgentService _travelAgentService;
        public AgentController(ITravelAgentService travelAgentService)
        {
            _travelAgentService = travelAgentService;
        }
        [HttpGet]
        public IActionResult AskAgent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AskAgent([FromBody]AgentPromptRequestDto requestDto)
        {
            var result = await _travelAgentService.AskAgentAsync(requestDto.Prompt);
            return Json(result);
        }
    }
}
