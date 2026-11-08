using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HumanResouresMangementSystem.DAL.Services
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionAuthorizationHandler(
            IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return;

            if (!int.TryParse(userId, out int id))
                return;

            var hasPermission =
                await _permissionService.HasPermissionAsync(
                    id,
                    requirement.PermissionName);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}