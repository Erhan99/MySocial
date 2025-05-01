using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Infrastructure.Identity;
using MySocial.WebUI.Models;
using System.Diagnostics;

namespace MySocial.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILikeInterface _likeRepository;

        public HomeController(ILogger<HomeController> logger, IPostRepository postRepository, UserManager<ApplicationUser> userManager, ILikeInterface likeRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
            _userManager = userManager;
            _likeRepository = likeRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
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
