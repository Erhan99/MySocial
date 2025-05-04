using Microsoft.AspNetCore.Mvc;

namespace MySocial.WebUI.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
