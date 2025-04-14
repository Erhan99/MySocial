using Microsoft.EntityFrameworkCore;
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
    public class LikeRepository : ILikeInterface
    {
        private readonly MSDbContext _context;
        public LikeRepository(MSDbContext context)
        {
            _context = context;
        }
        public bool AddLike(int postId, string userId)
        {
            if(_context.Likes.Any(l => l.PostId == postId && l.UserId == userId))
            {
                _context.Remove(_context.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId));
                _context.SaveChanges();
                return false;
            }
            else
            {
                _context.Likes.Add(new Domain.Entities.Like
                {
                    PostId = postId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });
                _context.SaveChanges();
                return true;
            }
        }

        public IEnumerable<Like> GetLikes(int postId)
        {
            return _context.Likes.Where(l => l.PostId == postId);
                
        }

        public void RemoveLike(int postId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
