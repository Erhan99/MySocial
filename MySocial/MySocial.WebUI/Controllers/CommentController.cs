using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySocial.Application.DTOs.Post;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.WebUI.Models;
using System.Security.Claims;

namespace MySocial.WebUI.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddComment([FromBody] PostDTO request)
        {
            if (request.Content.IsNullOrEmpty())
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
            try
            {
               _commentRepository.AddComment(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { success = true, data = _commentRepository.GetCommentById(comment.Id) });
        }
    }
}
