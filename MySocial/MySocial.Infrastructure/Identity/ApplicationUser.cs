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
        public string? ProfilePictureUrl { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
