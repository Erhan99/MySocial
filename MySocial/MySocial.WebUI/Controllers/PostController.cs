using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MySocial.Application.DTOs.Post;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Identity;
using MySocial.WebUI.Models;
using MySocial.WebUI.ViewModel;
using System.Security.Claims;

namespace MySocial.WebUI.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Detail(int postId)
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
                profilePictureUrl = user.ProfilePictureUrl,
                postId = postId
            };
            return View(model);
        }
    }
}
