using MySocial.Domain.Entities;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public IEnumerable<Object> GetPosts();
        public void AddPost(Post post);
        public void UpdatePost(Post post);
        public void DeletePost(int postId);
        public Object GetPostById(int postId);
    }
}
