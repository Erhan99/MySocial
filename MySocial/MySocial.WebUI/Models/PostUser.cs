using MySocial.Domain.Entities;
using MySocial.Infrastructure.Identity;

namespace MySocial.WebUI.Models
{
    public class PostUser
    {
        public Post post { get; set; }
        public ApplicationUser user { get; set; }
        public int likes { get; set; }
        public bool isLiked { get; set; }
    }
}
