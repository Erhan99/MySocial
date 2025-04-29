using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Interfaces.Repositories;
using MySocial.WebUI.Models;

namespace MySocial.WebUI.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikeInterface _likeRepository;

        public LikeController(ILikeInterface likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LikePost([FromBody] LikeRequest request)
        {
            _likeRepository.AddLike(request.PostId, request.UserId);
            return RedirectToAction("Index", "Home");
        }
    }
}
