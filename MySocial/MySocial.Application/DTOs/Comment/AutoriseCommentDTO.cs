using MySocial.Application.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.DTOs.Comment
{
    public class AutoriseCommentDTO
    {
        public CommentDTO Comment { get; set; }
        public bool canEdit { get; set; }
        public bool canDelete { get; set; }
    }
}
