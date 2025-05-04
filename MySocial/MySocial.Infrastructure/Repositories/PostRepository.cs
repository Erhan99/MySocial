using MySocial.Application.DTOs;
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

        public IEnumerable<PostDto> GetPosts()
        {
            var users = _context.Users;
            return _context.Posts
                .Where(post => !post.IsDeleted)
                .OrderByDescending(post => post.CreatedAt)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    User = new UserDto
                    {
                        Id = p.UserId,
                        UserName = users.Where(u => u.Id == p.UserId).Select(u => u.UserName).FirstOrDefault(),
                        ProfilePictureUrl = users.Where(u => u.Id == p.UserId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                    },
                    UsersLiked = _context.Likes.Where(l => l.PostId == p.Id && !l.IsDeleted).Select(l => l.UserId).ToList(),
                    Comments = _context.Comments.Where(c => c.PostId == p.Id && !c.IsDeleted).Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Text = c.Content,
                        CreatedAt = c.CreatedAt,
                        User = new UserDto
                        {
                            Id = c.UserId,
                            UserName = users.Where(u => u.Id == c.UserId).Select(u => u.UserName).FirstOrDefault(),
                            ProfilePictureUrl = users.Where(u => u.Id == c.UserId).Select(u => u.ProfilePictureUrl).FirstOrDefault()
                        },
                    }).ToList()
                });
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
