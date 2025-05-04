using MySocial.Application.Interfaces.Repositories;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Data;

namespace MySocial.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly MSDbContext _context;

        public PostRepository(MSDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Object> GetPosts()
        {
            var posts = from post in _context.Posts
                        join user in _context.Users
                        on post.UserId equals user.Id
                        where post.IsDeleted == false
                        select new
                        {
                            post.Id,
                            post.Content,
                            post.CreatedAt,
                            User = new
                            {
                                user.Id,
                                user.UserName,
                                user.Email,
                                user.ProfilePictureUrl
                            },
                            Likes = _context.Likes.Count(l => l.PostId == post.Id && l.IsDeleted == false),
                            UsersLiked = _context.Likes.Where(l => l.PostId == post.Id && l.IsDeleted == false).Select(l => l.UserId).ToList(),
                            Comments = _context.Comments.Where(c => c.PostId == post.Id).ToList()
                        };
            return posts.ToList();
        }

        public Object GetPostById(int postId)
        {
            var post = (from p in _context.Posts
                        join user in _context.Users
                        on p.UserId equals user.Id
                        where p.Id == postId && p.IsDeleted == false
                        select new
                        {
                            p.Id,
                            p.Content,
                            p.CreatedAt,
                            User = new
                            {
                                user.Id,
                                user.UserName,
                                user.Email,
                                user.ProfilePictureUrl
                            },
                            Likes = _context.Likes.Count(l => l.PostId == p.Id && l.IsDeleted == false),
                            UsersLiked = _context.Likes.Where(l => l.PostId == p.Id).Select(l => l.UserId).ToList(),
                            Comments = _context.Comments.Where(c => c.PostId == p.Id).ToList()
                        }).FirstOrDefault();
            return post;
        }
        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(int postId)
        {
           Post post = _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
            if (post != null && post.IsDeleted == false)
            {
                post.IsDeleted = true;
                _context.SaveChanges();
            }
        }

        public void UpdatePost(int postId, string content)
        {
            Post post = _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
            if (post != null && post.IsDeleted == false)
            {
                post.Content = content;
                post.CreatedAt = DateTime.Now;
                _context.SaveChanges();
            }
        }
    }
}
