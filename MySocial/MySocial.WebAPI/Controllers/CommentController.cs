using Microsoft.AspNetCore.Mvc;
using MySocial.Application.DTOs.Comment;
using MySocial.Application.DTOs.Post;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Repositories;
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
