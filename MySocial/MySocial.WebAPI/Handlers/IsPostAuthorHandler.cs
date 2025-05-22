using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MySocial.Application.DTOs.Post;
using MySocial.Domain.Entities;
using MySocial.Infrastructure.Identity;
using System.Security.Claims;

namespace MySocial.WebUI.Requirements
{
    public class IsPostAuthorHandler : AuthorizationHandler<IsPostAuthorRequirement, PostDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IsPostAuthorHandler (UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostAuthorRequirement requirement, PostDTO resource)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"Handler: claim user ID = {userId}, post.UserId = {resource.User.Id}");

            if (userId == null)
            {
                return;
            }
            if (resource.User.Id == userId)
            {
                context.Succeed(requirement);
            }
        }

        }
    }
