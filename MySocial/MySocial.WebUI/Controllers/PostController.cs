using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
            if (request.Content == null || request.Content == "")
            {
                return BadRequest("Post content can't be null or empty");
            }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePost([FromBody] PostRequest request)
        {
            try
            {
                if (request.PostId != -1)
                {
                    _postRepository.DeletePost(request.PostId);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { success = true, data = _postRepository.GetPostById(request.PostId) });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost([FromBody] PostRequest request)
        {
            try
            {
                if(request.Content == null || request.Content == "")
                {
                    return BadRequest("Post content can't be null or empty");
                }
                if (request.PostId != -1)
                {
                    _postRepository.UpdatePost(request.PostId, request.Content);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { success = true, data = _postRepository.GetPostById(request.PostId) });
        }

        public IActionResult Detail(int postId)
        {
            return View(postId);
        }
    }
}
