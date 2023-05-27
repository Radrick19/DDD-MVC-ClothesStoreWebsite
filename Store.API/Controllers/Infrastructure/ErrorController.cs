using Microsoft.AspNetCore.Mvc;

namespace Store.API.Controllers.Infrastructure
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorMessage(string message = null)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
