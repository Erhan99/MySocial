using MySocial.Application.DTOs.Message;
using MySocial.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        public List<MessageDTO> getMessagesBySenderAndReceiver(string user1, string user2);
        public void CreateMessage(Message message);
        public MessageDTO getMessageById(int id);
    }
}
