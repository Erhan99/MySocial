using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.DTOs.Post;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Identity;
using System.Security.Claims;

namespace MySocial.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostController(IPostRepository postRepository, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            _postRepository = postRepository;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var posts = _postRepository.GetPosts();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var post = _postRepository.GetPostById(id);

            if (post == null) return NotFound();

            return Ok(post);
        }

        [HttpGet("authorization/CanEdit/{postId}")]
        public async Task<IActionResult> CanEditPost(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            var result = await _authorizationService.AuthorizeAsync(User, post, "CanEditPost");
            return Ok(new
            {
                authorized = result.Succeeded,
                isAuthenticated = User.Identity.IsAuthenticated,
                nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                name = User.Identity.Name,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpPost]
        public IActionResult Create(CreatePostDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Post content can't be null or empty");
            }
            if (request.UserId == null)
            {
                return BadRequest("User id is null");
            }
            Post post = new Post
            {
                Content = request.Content,
                CreatedAt = DateTime.Now,
                UserId = request.UserId
            };
            _postRepository.AddPost(post);

            return CreatedAtAction(nameof(GetById), new { id = post.Id }, _postRepository.GetPostById(post.Id));
        }

        [HttpPut("remove/{id}")]
        public IActionResult Remove(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _postRepository.DeletePost(id);

            return CreatedAtAction(nameof(GetById), new { id = id }, _postRepository.GetPostById(id));
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, UpdatePostDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Post content can't be null or empty");
            }
            if (id == null)
            {
                return NotFound();
            }
            _postRepository.UpdatePost(id, request.Content);
            return CreatedAtAction(nameof(GetById), new { id = id }, _postRepository.GetPostById(id));
        }
    }
}
