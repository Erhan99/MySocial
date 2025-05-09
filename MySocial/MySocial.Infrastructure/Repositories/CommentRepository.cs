using MySocial.Application.DTOs.Comment;
using MySocial.Application.DTOs.User;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Data;
using System.ComponentModel.Design;

namespace MySocial.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MSDbContext _context;

        public CommentRepository(MSDbContext context)
        {
            _context = context;
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
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

        public void UpdateComment(int commentId, string content)
        {
            Comment comment = _context.Comments.Where(c => c.Id == commentId).FirstOrDefault();
            comment.Content = content;
            comment.CreatedAt = DateTime.Now;
            _context.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            Comment comment = _context.Comments.Where(c => c.Id == commentId).FirstOrDefault();
            comment.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
