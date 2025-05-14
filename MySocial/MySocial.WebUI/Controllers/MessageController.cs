using Microsoft.AspNetCore.Mvc;

namespace MySocial.WebUI.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
