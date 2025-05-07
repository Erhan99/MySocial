using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.DTOs.Like
{
    public class CreateLikeDTO
    {
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
}
