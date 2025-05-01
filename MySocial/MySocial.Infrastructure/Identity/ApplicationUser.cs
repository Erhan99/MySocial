using Microsoft.AspNetCore.Identity;

namespace MySocial.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePictureUrl { get; set; } = "icons/default_profile_icon.svg";
        public bool IsAdmin { get; set; } = false;

    }
}
