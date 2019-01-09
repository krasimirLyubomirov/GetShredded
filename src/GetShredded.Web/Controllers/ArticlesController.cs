using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        public IActionResult Training()
        {
            return View();
        }

        public IActionResult Nutrition()
        {
            return View();
        }

        public IActionResult Supplement()
        {
            return View();
        }
    }
}
