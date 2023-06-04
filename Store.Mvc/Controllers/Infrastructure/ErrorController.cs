using Microsoft.AspNetCore.Mvc;

namespace Store.API.Controllers.Infrastructure
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        public IActionResult ErrorMessage(string message = null)
        {
            _logger.LogError(message);
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
