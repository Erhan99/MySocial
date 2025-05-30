using Microsoft.AspNetCore.Authorization;
using MySocial.Application.DTOs.Comment;
using MySocial.WebAPI.Requirements;
using System.Security.Claims;

namespace MySocial.WebAPI.Handlers
{
    public class IsCommentAuthorHandler : AuthorizationHandler<IsCommentAuthorRequirement, CommentDTO>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCommentAuthorRequirement requirement, CommentDTO resource)
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
