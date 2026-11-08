using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HumanResouresMangementSystem.Core.Authorization
{
    public class PermissionPolicyProvider
        : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(
            IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(
            string policyName)
        {
            if (!policyName.StartsWith("Permission:"))
            {
                return await base.GetPolicyAsync(policyName);
            }

            var permissionName =
                policyName.Substring("Permission:".Length);

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(
                    new PermissionRequirement(permissionName))
                .Build();

            return policy;
        }
    }
}