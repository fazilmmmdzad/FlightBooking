using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.ViewComponents.BookingViewComponents
{
    public class _BookingTagsComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}