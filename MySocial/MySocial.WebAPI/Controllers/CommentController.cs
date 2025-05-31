using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MySocial.Application.DTOs.Comment;
using MySocial.Application.DTOs.Post;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;

namespace MySocial.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthorizationService _authorizationService;
        public CommentController(ICommentRepository commentRepository, IAuthorizationService authorizationService)
        {
            _commentRepository = commentRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var comment = _commentRepository.GetCommentById(id);

            if (comment == null) return NotFound();

            return Ok(comment);
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetByPost(int postId)
        {
            var isAdmin = User.HasClaim("IsAdmin", "");
            var comments = _commentRepository.GetCommentByPost(postId);

            if (comments == null) return NotFound();

            List<AutoriseCommentDTO> authComments = new List<AutoriseCommentDTO>();
            foreach (var comment in comments)
            {
                var result = await _authorizationService.AuthorizeAsync(User, comment, "CanEditComment");
                if (!isAdmin)
                {
                    var dto = new AutoriseCommentDTO
                    {
                        Comment = comment,
                        canEdit = result.Succeeded,
                        canDelete = result.Succeeded
                    };
                    authComments.Add(dto);
                }
                else {
                    var dto = new AutoriseCommentDTO
                    {
                        Comment = comment,
                        canEdit = result.Succeeded,
                        canDelete = true
                    };
                    authComments.Add(dto);
                }
            }
            return Ok(authComments);
        }

        [HttpPost]
        public IActionResult Create(CreateCommentDTO request)
        {
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Comment content can't be null or empty");
            }
            Comment comment = new Comment
            {
                Content = request.Content,
                CreatedAt = DateTime.Now,
                UserId = request.UserId,
                PostId = request.PostId,
            };
            _commentRepository.AddComment(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, _commentRepository.GetCommentById(comment.Id));
        }

        [HttpPut("remove/{id}")]
        public IActionResult Remove(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _commentRepository.DeleteComment(id);

            return CreatedAtAction(nameof(GetById), new { id = id }, _commentRepository.GetCommentById(id));
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, UpdateCommentDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Post content can't be null or empty");
            }
            if (id == null)
            {
                return NotFound();
            }
            _commentRepository.UpdateComment(id, request.Content);
            return CreatedAtAction(nameof(GetById), new { id = id }, _commentRepository.GetCommentById(id));
        }
    }
}
