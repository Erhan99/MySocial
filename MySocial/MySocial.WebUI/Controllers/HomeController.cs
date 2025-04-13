using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Infrastructure.Identity;
using MySocial.WebUI.Models;
using MySocial.WebUI.ViewModels;
using System.Diagnostics;

namespace MySocial.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _postRepository = postRepository;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index(PostViewModel post)
        {
            var posts = _postRepository.GetPosts();
            var postsWithUser = posts.Select(p => new PostUser
            {
                post = p,
                user = _userManager.Users.FirstOrDefault(u => u.Id == p.UserId)
            }).ToList();
            post.Posts = postsWithUser;
            return View(post);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
