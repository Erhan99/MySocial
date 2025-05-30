using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.DTOs.Post
{
    public class AutorisedPostDTO
    {
        public PostDTO Post { get; set; }
        public bool canEdit { get; set; }
        public bool canDelete { get; set; }
    }
}
