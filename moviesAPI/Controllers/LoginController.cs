using Microsoft.AspNetCore.Mvc;

namespace moviesAPI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
