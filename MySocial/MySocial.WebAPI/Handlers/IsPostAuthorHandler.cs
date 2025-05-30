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
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostAuthorRequirement requirement, PostDTO resource)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
