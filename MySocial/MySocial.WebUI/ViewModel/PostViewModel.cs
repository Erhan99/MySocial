using MySocial.Application.DTOs;

namespace MySocial.WebUI.ViewModel
{
    public class PostViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string profilePictureUrl {  get; set; }
        public int postId { get; set; } = -1;
        public string ReceiverId { get; set; } = "";
    }
}
