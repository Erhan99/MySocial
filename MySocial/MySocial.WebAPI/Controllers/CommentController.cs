using Microsoft.AspNetCore.Mvc;
using MySocial.Application.DTOs.Comment;
using MySocial.Application.DTOs.Post;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using System.Security.Claims;

namespace MySocial.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var comment = _commentRepository.GetCommentById(id);

            if (comment == null) return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public IActionResult AddComment(CommentDTO request)
        {
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Comment content can't be null or empty");
            }
            Comment comment = new Comment
            {
                Content = request.Content,
                CreatedAt = DateTime.Now,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                PostId = request.Id,
            };
            _commentRepository.AddComment(comment);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, _commentRepository.GetCommentById(request.Id));
        }
    }
}
