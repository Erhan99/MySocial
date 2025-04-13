using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Infrastructure.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string? ProfilePictureUrl { get; set; } = "icons/default_profile_icon.svg";
        public bool IsAdmin { get; set; } = false;
        
    }
}
