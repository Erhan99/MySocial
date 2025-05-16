using Microsoft.AspNetCore.Identity;
using MySocial.Application.DTOs.Message;
using MySocial.Application.DTOs.User;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Data;
using MySocial.Application.Interfaces.Repositories;

using MySocial.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;

namespace MySocial.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MSDbContext _context;
        public MessageRepository(MSDbContext context) {
            _context = context;
        }
        public List<MessageDTO> getMessagesBySenderAndReceiver(string user1, string user2)
        {
            var users = _context.Users;
            var messages = _context.Messages
        .Where(m =>
            (m.SenderId == user1 && m.ReceiverId == user2) ||
            (m.SenderId == user2 && m.ReceiverId == user1))
        .OrderBy(m => m.CreatedAt)
        .Select(m => new MessageDTO
        {
            Id = m.Id,
            Content = m.Content,
            Sender = new UserDTO
            {
                Id = m.SenderId,
                UserName = users.Where(u => u.Id == m.SenderId).Select(u => u.UserName).FirstOrDefault(),
                ProfilePictureUrl = users.Where(u => u.Id == m.SenderId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
            },
            Receiver = new UserDTO
            {
                Id = m.ReceiverId,
                UserName = users.Where(u => u.Id == m.ReceiverId).Select(u => u.UserName).FirstOrDefault(),
                ProfilePictureUrl = users.Where(u => u.Id == m.ReceiverId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
            },
            CreatedAt = m.CreatedAt
        })
        .ToList();
            return messages;
        }

        public MessageDTO getMessageById(int id)
        {
            var users = _context.Users;
            var message = _context.Messages
            .Where(m => m.Id == id)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new MessageDTO
            {
                Id = m.Id,
                Content = m.Content,
                Sender = new UserDTO
                {
                    Id = m.SenderId,
                    UserName = users.Where(u => u.Id == m.SenderId).Select(u => u.UserName).FirstOrDefault(),
                    ProfilePictureUrl = users.Where(u => u.Id == m.SenderId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                },
                Receiver = new UserDTO
                {
                    Id = m.ReceiverId,
                    UserName = users.Where(u => u.Id == m.ReceiverId).Select(u => u.UserName).FirstOrDefault(),
                    ProfilePictureUrl = users.Where(u => u.Id == m.ReceiverId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                },
                CreatedAt = m.CreatedAt
            }).FirstOrDefault();
            return message;
        }

        public IEnumerable<GetLastMessageUserDTO> GetMessagedUsers(string currentUserId)
        {
            var users = _context.Users.ToList();

            var messages = _context.Messages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .AsEnumerable() 
                .Select(m => new
                {
                    Message = m,
                    PartnerId = m.SenderId == currentUserId ? m.ReceiverId : m.SenderId
                })
                .GroupBy(x => x.PartnerId)
                .Select(g => g.OrderByDescending(x => x.Message.CreatedAt).First())
                .ToList();

            var result = messages.Select(x =>
            {
                var user = users.FirstOrDefault(u => u.Id == x.PartnerId);
                return new GetLastMessageUserDTO
                {
                    Receiver = new UserDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        ProfilePictureUrl = user.ProfilePictureUrl
                    },
                    Content = x.Message.Content,
                    CreatedAt = x.Message.CreatedAt
                };
            }).OrderByDescending(m => m.CreatedAt).ToList();

            return result;
        }

        public void CreateMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }
    }
}
