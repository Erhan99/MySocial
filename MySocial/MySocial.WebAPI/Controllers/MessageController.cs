using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MySocial.Application.DTOs.Message;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Repositories;

namespace MySocial.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public IActionResult GetBySenderAndReceiver([FromQuery] GetMessageDTO dto)
        {
            var messages = _messageRepository.getMessagesBySenderAndReceiver(dto.userId1, dto.userId2).ToList();
            if (messages == null)
            {
                return NotFound();
            }
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var message = _messageRepository.getMessageById(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpGet("MessagedUser/{CurrentUserId}")]
        public IActionResult GetLastMessageUser(string CurrentUserId)
        {
            var messages = _messageRepository.GetMessagedUsers(CurrentUserId);
            return Ok(messages);
        }

        [HttpPost]
        public IActionResult Create(CreateMessageDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content)) return BadRequest();

            Message message = new Message()
            {
                Content = dto.Content,
                ReceiverId = dto.ReceiverId,
                SenderId = dto.SenderId,
                Status = "Sent"
            };
            _messageRepository.CreateMessage(message);
            return CreatedAtAction(nameof(GetById), new { id=message.Id }, _messageRepository.getMessageById(message.Id));
        }
    }
}
