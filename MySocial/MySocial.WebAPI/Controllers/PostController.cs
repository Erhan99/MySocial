using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.DTOs.Post;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;

namespace MySocial.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IAuthorizationService _authorizationService;
        public PostController(IPostRepository postRepository, IAuthorizationService authorizationService)
        {
            _postRepository = postRepository;
            _authorizationService = authorizationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var isAdmin = User.HasClaim("IsAdmin", ""); 

            var posts = _postRepository.GetPosts();
            List<AutorisedPostDTO> authPosts = new List<AutorisedPostDTO>();
            foreach (var post in posts)
            {
                var result = await _authorizationService.AuthorizeAsync(User, post, "CanEditPost");
                if (!isAdmin)
                {
                    var dto = new AutorisedPostDTO
                    {
                        Post = post,
                        canEdit = result.Succeeded,
                        canDelete = result.Succeeded
                    };
                    authPosts.Add(dto);
                }
                else
                {
                    var dto = new AutorisedPostDTO
                    {
                        Post = post,
                        canEdit = result.Succeeded,
                        canDelete = true
                    };
                    authPosts.Add(dto);
                }

            }
            return Ok(authPosts);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var post = _postRepository.GetPostById(id);

            if (post == null) return NotFound();

            return Ok(post);
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
