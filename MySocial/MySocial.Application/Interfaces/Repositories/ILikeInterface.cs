using MySocial.Domain.Entities;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface ILikeInterface
    {
        void AddLike(int postId, string userId);
        void RemoveLike(int postId, string userId);
        IEnumerable<Like> GetLikes(int postId);
    }
}
