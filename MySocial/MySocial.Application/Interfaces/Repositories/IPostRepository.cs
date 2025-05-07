using MySocial.Application.DTOs.Post;
using MySocial.Domain.Entities;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public IEnumerable<PostDTO> GetPosts();
        public void AddPost(Post post);
        public void UpdatePost(int postId, string content);
        public void DeletePost(int postId);
        public PostDTO GetPostById(int postId);
    }
}
