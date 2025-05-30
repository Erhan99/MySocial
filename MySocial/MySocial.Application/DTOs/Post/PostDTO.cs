using MySocial.Application.DTOs.Comment;
using MySocial.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.DTOs.Post
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsModified { get; set; }
        public UserDTO User { get; set; }
        public List<string> UsersLiked { get; set; }
        public List<CommentDTO> Comments { get; set; }
    }
}
