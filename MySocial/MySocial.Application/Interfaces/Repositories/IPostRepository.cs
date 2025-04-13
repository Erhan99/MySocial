using MySocial.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public IEnumerable<Post> GetPosts();
        public void AddPost(Post post);
        public void UpdatePost(Post post);
        public void DeletePost(int postId);
        public Post GetPostById(int postId);
    }
}
