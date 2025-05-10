using Microsoft.AspNetCore.Mvc;

namespace NutriFitApp.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Muestra la vista Views/Home/Index.cshtml
        }
    }
}
