using MySocial.Application.DTOs;
using MySocial.Domain.Entities;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public IEnumerable<PostDto> GetPosts();
        public void AddPost(Post post);
        public void UpdatePost(int postId, string content);
        public void DeletePost(int postId);
        public Object GetPostById(int postId);
    }
}
