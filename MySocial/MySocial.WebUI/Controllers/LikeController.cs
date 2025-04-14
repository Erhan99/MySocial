using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Interfaces.Repositories;

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

        public IActionResult LikePost(int postId, string userId)
        {
            _likeRepository.AddLike(postId, userId);
            return RedirectToAction("Index", "Home");
        }
    }
}
