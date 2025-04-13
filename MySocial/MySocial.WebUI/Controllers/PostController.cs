using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Identity;
using MySocial.Infrastructure.Repositories;
using MySocial.WebUI.ViewModels;
using System.Security.Claims;

namespace MySocial.WebUI.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddPost(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post
                {
                    Content = model.Content,
                    CreatedAt = DateTime.Now,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)

                };
                _postRepository.AddPost(post);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
