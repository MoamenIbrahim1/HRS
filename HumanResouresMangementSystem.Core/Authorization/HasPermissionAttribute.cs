using Microsoft.AspNetCore.Authorization;

namespace HumanResouresMangementSystem.Core.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permissionName)
        {
            Policy = $"Permission:{permissionName}";
        }
    }
}