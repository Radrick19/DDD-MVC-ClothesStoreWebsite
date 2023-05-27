using Microsoft.AspNetCore.Mvc;

namespace Store.API.Controllers.Infrastructure
{
    public class AccessDeniedController : Controller
    {
        public async Task Index()
        {
            Response.StatusCode = 403;
            await Response.WriteAsync("Access Denied");
        }
    }
}
