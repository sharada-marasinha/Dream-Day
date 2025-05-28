using Microsoft.AspNetCore.Mvc;

namespace DreamDay.UI.Controllers
{
    public class WeddingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
