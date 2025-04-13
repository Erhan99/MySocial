using MySocial.Domain.Entities;
using MySocial.WebUI.Models;

namespace MySocial.WebUI.ViewModels
{
    public class PostViewModel
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }

        public IEnumerable<PostUser>? Posts { get; set; } 
    }
}
