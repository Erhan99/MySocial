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
    public class PostRepository : IPostRepository
    {
        private readonly MSDbContext _context;

        public PostRepository(MSDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> GetPosts()
        {
            return _context.Posts.ToList();
        }
        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(int postId)
        {
            throw new NotImplementedException();
        }

        public Post GetPostById(int postId)
        {
            throw new NotImplementedException();
        }

        public void UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
