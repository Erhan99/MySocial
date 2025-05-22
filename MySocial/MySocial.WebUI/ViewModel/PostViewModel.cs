using MySocial.Application.DTOs;
using MySocial.Application.DTOs.User;

namespace MySocial.WebUI.ViewModel
{
    public class PostViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string profilePictureUrl {  get; set; }

        public int postId { get; set; } = -1;
        public string ReceiverId { get; set; } = "";

        public IEnumerable<UserDTO> Users { get; set; } = [];
    }
}
