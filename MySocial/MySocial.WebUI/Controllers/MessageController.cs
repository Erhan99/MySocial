using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocial.Application.DTOs.User;
using MySocial.Application.Interfaces.Repositories;
using MySocial.Infrastructure.Identity;
using MySocial.WebUI.ViewModel;

namespace MySocial.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        public MessageController(UserManager<ApplicationUser> userManager, IUserRepository userRepository) 
        { 
            _userManager = userManager;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new PostViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                profilePictureUrl = user.ProfilePictureUrl,
            };     
            return View(model);  
        }
        public async Task<IActionResult> Chat(string receiverId)
        {
            var user = await _userManager.GetUserAsync(User);
            var receiver = _userRepository.FindById(receiverId);
            if (user == null)
            {
                return NotFound();
            }
            var model = new PostViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                profilePictureUrl = user.ProfilePictureUrl,
                ReceiverId = receiverId,
                ReceiverName = receiver.UserName,
                receiverProfilePictureUrl =receiver.ProfilePictureUrl
            };
            return View(model);
        }
    }
}
