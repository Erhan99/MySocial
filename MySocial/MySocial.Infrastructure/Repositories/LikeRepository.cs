using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Data;

namespace MySocial.Infrastructure.Repositories
{
    public class LikeRepository : ILikeInterface
    {
        private readonly MSDbContext _context;
        public LikeRepository(MSDbContext context)
        {
            _context = context;
        }
        public void AddLike(int postId, string userId)
        {
            _context.Likes.Add(new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
            _context.SaveChanges();
        }

        public IEnumerable<Like> GetLikes(int postId)
        {
            return _context.Likes.Where(l => l.PostId == postId && l.IsDeleted == false);

        }
        
        public void RemoveLike(int postId, string userId)
        {
            Like like = _context.Likes.Where(l => l.UserId == userId && l.PostId == postId && l.IsDeleted == false).FirstOrDefault();
            if (like != null)
            {
                like.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}
