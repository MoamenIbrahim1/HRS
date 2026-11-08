using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Services
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(
            int userId,
            string permissionName);
    }
}
