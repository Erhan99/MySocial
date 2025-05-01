using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Infrastructure.Identity;
using MySocial.WebUI.Models;

namespace MySocial.WebUI.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikeInterface _likeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public LikeController(ILikeInterface likeRepository, UserManager<ApplicationUser> usermanager)
        {
            _likeRepository = likeRepository;
            _userManager = usermanager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken, HttpPost]
        public IActionResult LikePost([FromBody] LikeRequest request)
        {
            string? userId = _userManager.GetUserId(User);
            if (userId == null || request.PostId == null)
            {
                return BadRequest();
            }
            _likeRepository.AddLike(request.PostId, userId.Trim());
            return Ok(new { success = true });
        }
        [ValidateAntiForgeryToken, HttpPost]
        public IActionResult RemoveLike([FromBody] LikeRequest request)
        {
            string? userId = _userManager.GetUserId(User);
            if (userId == null || request.PostId == null)
            {
                return BadRequest();
            }
            _likeRepository.RemoveLike(request.PostId, userId.Trim());
            return Ok(new { success = true });
        }
    }
}
