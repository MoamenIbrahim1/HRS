using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Authorization
{
   public class PermissionRequirement: IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

    }
}
