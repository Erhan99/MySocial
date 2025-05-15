using MySocial.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.DTOs.Message
{
    public class CreateMessageDTO
    {
        public string Content { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
    }
}
