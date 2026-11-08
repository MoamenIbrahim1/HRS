using HumanResouresMangementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Services
{
    public class PermissionService:IPermissionService
    {
        private readonly AppDbContext _context;

        public PermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasPermissionAsync(
            int userId,
            string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
                return false;

            // normalize once (client-side)
            var lowerPermission = permissionName.ToLower();

            // 1) Direct name match (permission.Name) — use ToLower() comparison translatable by EF Core
            var directMatch = await _context.GroupPermissions
                .AnyAsync(x =>
                    x.Group.Users.Any(u => u.Id == userId) &&
                    x.Permission.Name != null &&
                    x.Permission.Name.ToLower() == lowerPermission);
            if (directMatch) return true;

            // 2) Try to parse "Controller.Action" style (e.g. Employee.Index or Employees.Index)
            var parts = permissionName.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                var controllerPart = parts[0];
                var actionPart = parts[1];

                // Normalize controller: tolerate trailing 's' (Employees vs Employee)
                string NormalizeController(string c) =>
                    string.IsNullOrWhiteSpace(c) ? c : (c.EndsWith("s", StringComparison.OrdinalIgnoreCase) ? c[..^1] : c);

                var ctrlNormalized = NormalizeController(controllerPart);

                // Map Index -> view to match seed naming like "employee.view"
                var mappedAction = actionPart.Equals("Index", StringComparison.OrdinalIgnoreCase)
                    ? "view"
                    : actionPart.ToLower();

                // precompute lower-case variants (client-side)
                var ctrlALower = controllerPart.ToLower();
                var ctrlBLower = ctrlNormalized.ToLower();
                var actionLower = actionPart.ToLower();
                var mappedActionLower = mappedAction.ToLower();

                var ctrlActionMatch = await _context.GroupPermissions
                    .AnyAsync(x =>
                        x.Group.Users.Any(u => u.Id == userId) &&
                        x.Permission.ControllerName != null &&
                        x.Permission.ActionName != null &&
                        (
                            x.Permission.ControllerName.ToLower() == ctrlALower ||
                            x.Permission.ControllerName.ToLower() == ctrlBLower
                        ) &&
                        (
                            x.Permission.ActionName.ToLower() == actionLower ||
                            x.Permission.ActionName.ToLower() == mappedActionLower
                        )
                    );

                if (ctrlActionMatch) return true;

                // 3) Try name pattern "controller.mappedAction", e.g. "employee.view"
                var patternName = $"{ctrlNormalized.ToLower()}.{mappedActionLower}";
                var patternMatch = await _context.GroupPermissions
                    .AnyAsync(x =>
                        x.Group.Users.Any(u => u.Id == userId) &&
                        x.Permission.Name != null &&
                        x.Permission.Name.ToLower() == patternName);

                if (patternMatch) return true;
            }

            return false;
        }
    }
}