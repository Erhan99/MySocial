using MySocial.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.DTOs.Comment
{
    public class CreateCommentDTO
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
