using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.DTOs.User;
using MySocial.Infrastructure.Identity;
using MySocial.WebUI.ViewModel;

namespace MySocial.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public MessageController(UserManager<ApplicationUser> userManager) 
        { 
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new PostViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                profilePictureUrl = user.ProfilePictureUrl,
            };     
            return View(model);  
        }
        public async Task<IActionResult> Chat()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var model = new PostViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                profilePictureUrl = user.ProfilePictureUrl
            };
            return View(model);
        }
    }
}
