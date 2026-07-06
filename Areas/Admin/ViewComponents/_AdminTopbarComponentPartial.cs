using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.ViewComponents
{
    public class _AdminTopbarComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
