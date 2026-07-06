using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.ViewComponents.DefaultViewComponents
{
    public class _DefaultSeviceComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}