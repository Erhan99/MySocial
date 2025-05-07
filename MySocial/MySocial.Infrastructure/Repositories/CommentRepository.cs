using Microsoft.EntityFrameworkCore;
using MySocial.Application.DTOs;
using MySocial.Application.DTOs.Comment;
using MySocial.Application.DTOs.User;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MSDbContext _context;

        public CommentRepository (MSDbContext context)
        {
            _context = context;
        }

        public void AddComment(Comment comment)
        {
            try
            {
                _context.Comments.Add(comment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveChanges failed: {ex}");
            }
        }

        public CommentDTO GetCommentById(int commentId)
        {
            var users = _context.Users;
            return _context.Comments
                .Where(c => c.Id == commentId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDTO
                {
                    Id = c.Id,
                    CreatedAt = c.CreatedAt,
                    Content = c.Content,
                    User = new UserDTO
                    {
                        Id = c.UserId,
                        UserName = users.Where(u => u.Id == c.UserId).Select(u => u.UserName).FirstOrDefault(),
                        ProfilePictureUrl = users.Where(u => u.Id == c.UserId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                    }
                }).FirstOrDefault();
        }
    }
}
