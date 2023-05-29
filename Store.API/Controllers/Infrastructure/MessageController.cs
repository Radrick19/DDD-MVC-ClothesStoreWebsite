using Microsoft.AspNetCore.Mvc;

namespace Store.Mvc.Controllers.Infrastructure
{
    public class MessageController : Controller
    {
        public IActionResult Index(string text)
        {
            return View("Index", text);
        }
    }
}
