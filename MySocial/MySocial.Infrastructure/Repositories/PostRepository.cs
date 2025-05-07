using MySocial.Application.DTOs.User;
using MySocial.Application.DTOs.Post;
using MySocial.Application.DTOs.Comment;
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

        public IEnumerable<PostDTO> GetPosts()
        {
            var users = _context.Users;
            return _context.Posts
                .Where(post => !post.IsDeleted)
                .OrderByDescending(post => post.CreatedAt)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    User = new UserDTO
                    {
                        Id = p.UserId,
                        UserName = users.Where(u => u.Id == p.UserId).Select(u => u.UserName).FirstOrDefault(),
                        ProfilePictureUrl = users.Where(u => u.Id == p.UserId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                    },
                    UsersLiked = _context.Likes.Where(l => l.PostId == p.Id && !l.IsDeleted).Select(l => l.UserId).ToList(),
                    Comments = _context.Comments.Where(c => c.PostId == p.Id && !c.IsDeleted).Select(c => new CommentDTO
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        User = new UserDTO
                        {
                            Id = c.UserId,
                            UserName = users.Where(u => u.Id == c.UserId).Select(u => u.UserName).FirstOrDefault(),
                            ProfilePictureUrl = users.Where(u => u.Id == c.UserId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                        },
                    }).ToList()
                });
        }

        public PostDTO GetPostById(int postId)
        {
            var users = _context.Users;
            return _context.Posts
                .Where(post => !post.IsDeleted && post.Id == postId)
                .OrderByDescending(post => post.CreatedAt)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    User = new UserDTO
                    {
                        Id = p.UserId,
                        UserName = users.Where(u => u.Id == p.UserId).Select(u => u.UserName).FirstOrDefault(),
                        ProfilePictureUrl = users.Where(u => u.Id == p.UserId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                    },
                    UsersLiked = _context.Likes.Where(l => l.PostId == p.Id && !l.IsDeleted).Select(l => l.UserId).ToList(),
                    Comments = _context.Comments.Where(c => c.PostId == p.Id && !c.IsDeleted).Select(c => new CommentDTO
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        User = new UserDTO
                        {
                            Id = c.UserId,
                            UserName = users.Where(u => u.Id == c.UserId).Select(u => u.UserName).FirstOrDefault(),
                            ProfilePictureUrl = users.Where(u => u.Id == c.UserId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                        },
                    }).ToList()
                }).FirstOrDefault();
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
