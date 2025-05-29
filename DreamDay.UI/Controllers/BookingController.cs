using Microsoft.AspNetCore.Mvc;

namespace DreamDay.UI.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
