using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Identity;
using MySocial.WebUI.Models;
using System.Security.Claims;

namespace MySocial.WebUI.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _postRepository = postRepository;
            _userManager = userManager;
        }

        public IActionResult GetPosts()
        {
            var posts = _postRepository.GetPosts(); 
            return Ok(new { data = posts });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddPost([FromBody] PostRequest request)
        {
            Post post = new Post
            {
                Content = request.Content,
                CreatedAt = DateTime.Now,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)

            };
            try
            {
                _postRepository.AddPost(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { success = true, data = _postRepository.GetPostById(post.Id) });
        }
    }
}
