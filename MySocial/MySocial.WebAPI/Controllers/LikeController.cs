using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.DTOs.Like;
using MySocial.Application.Interfaces.Repositories;

namespace MySocial.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeInterface _likeRepository;

        public LikeController(ILikeInterface likeRepository)
        {
            _likeRepository = likeRepository;
        }

        [HttpPost]
        public IActionResult LikePost(CreateLikeDTO request)
        {
            if (request.UserId == null)
            {
                return NotFound("User not found");
            }
            if (request.PostId == null)
            {
                return NotFound("Post not found");
            }
            _likeRepository.AddLike(request.PostId, request.UserId);
            return Ok();
        }

        [HttpPut]
        public IActionResult RemoveLike(CreateLikeDTO request)
        {
            if (request.UserId == null)
            {
                return NotFound("User not found");
            }
            if (request.PostId == null)
            {
                return NotFound("Post not found");
            }
            _likeRepository.RemoveLike(request.PostId, request.UserId);
            return Ok();
        }
    }
}
