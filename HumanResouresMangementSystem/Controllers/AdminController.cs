using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Dto;
using HumanResouresMangementSystem.Core.Entites;
using HumanResouresMangementSystem.Core.Models;
using HumanResouresMangementSystem.DAL.Interfaces;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HumanResouresMangementSystem.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HasPermission("Groups.View")]
        public IActionResult Index()
        {
            return View();
        }

        [HasPermission("Groups.View")]
        [HttpGet]
        public async Task<IActionResult> Permissions(int id = 0)
        {
            var model = new GroupViewModel();

            List<GroupPermission> existingGroupPermissions = new List<GroupPermission>();

            if (id > 0)
            {
                var group = await _unitOfWork.Groups.GetByIdAsync(id);

                if (group == null)
                    return NotFound();

                model.GroupId = group.Id;
                model.GroupName = group.Name;

                var existingPerms = await _unitOfWork.GroupPermissions.FindAsync(gp => gp.GroupId == id);
                existingGroupPermissions = existingPerms.ToList();
            }

            var allPermissions = await _unitOfWork.Permissions.GetAllAsync();

            model.Permissions = allPermissions
              .Where(p => p.ControllerName != "Users" && p.ControllerName != "Groups")
              .GroupBy(p => p.ControllerName)
              .Select(g =>
              {
                  var groupPermsForPage = existingGroupPermissions
              .Where(gp => allPermissions.Any(p => p.Id == gp.PermissionId && p.ControllerName == g.Key))
              .ToList();

                  return new PermissionViewModel
                  {
                      PageName = GetArabicPageName(g.Key),
                      CanView = groupPermsForPage.Any(gp => gp.CanView),
                      CanAdd = groupPermsForPage.Any(gp => gp.CanAdd),
                      CanEdit = groupPermsForPage.Any(gp => gp.CanEdit),
                      CanDelete = groupPermsForPage.Any(gp => gp.CanDelete)
                  };
              }).ToList();

            return View(model);
        }
        [HasPermission("Groups.edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Permissions(GroupViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.GroupName))
            {
                ModelState.AddModelError("GroupName", "من فضلك ادخل اسم المجموعة");
                return View(model);
            }

            // 🔍 التحقق من تكرار اسم المجموعة (مع تجاهل المسافات وحالة الأحرف)
            var isNameExist = await _unitOfWork.Groups.GetQueryable()
        .AnyAsync(g => g.Id != model.GroupId &&
               g.Name.Trim().ToLower() == model.GroupName.Trim().ToLower());

            if (isNameExist)
            {
                ModelState.AddModelError("GroupName", "المجموعة موجودة بالفعل");
                return View(model);
            }

            bool hasAnyPermission = model.Permissions != null && model.Permissions.Any(p =>
              p.CanView || p.CanAdd || p.CanEdit || p.CanDelete
            );

            if (!hasAnyPermission)
            {
                ModelState.AddModelError(string.Empty, "من فضلك قم بتحديد صلاحية واحدة على الأقل");
                return View(model);
            }

            HrGroup group;

            if (model.GroupId == 0)
            {
                group = new HrGroup { Name = model.GroupName };
                await _unitOfWork.Groups.AddAsync(group);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                group = await _unitOfWork.Groups.GetByIdAsync(model.GroupId);
                if (group == null) return NotFound();

                group.Name = model.GroupName;

                var oldPermissions = await _unitOfWork.GroupPermissions.FindAsync(gp => gp.GroupId == model.GroupId);
                foreach (var permission in oldPermissions)
                {
                    _unitOfWork.GroupPermissions.Delete(permission);
                }
            }

            var allPermissions = await _unitOfWork.Permissions.GetAllAsync();

            foreach (var item in model.Permissions)
            {
                string controllerName = GetEnglishControllerName(item.PageName);

                var pagePermissions = allPermissions
                  .Where(p => p.ControllerName == controllerName)
                  .ToList();

                foreach (var perm in pagePermissions)
                {
                    await _unitOfWork.GroupPermissions.AddAsync(new GroupPermission
                    {
                        GroupId = group.Id,
                        PermissionId = perm.Id,
                        CanView = item.CanView,
                        CanAdd = item.CanAdd,
                        CanEdit = item.CanEdit,
                        CanDelete = item.CanDelete
                    });
                }
            }

            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index", "Home");
        }
        #region Helper Methods
        private static string GetArabicPageName(string controllerName) => controllerName switch
        {
            "Employees" => "المستخدمين",
            "GeneralSettings" => "الإعدادات العامة",
            "Attendance" => "الحضور والانصراف",
            "PayrollReport" => "تقرير الرواتب",
            "Employee" => "اداراة الموظفين",
            "OfficialHolidays" => "الأجازات الرسمية",
            _ => controllerName
        };

        private static string GetEnglishControllerName(string arabicName) => arabicName switch
        {
            "المستخدمين" => "Employees",
            "الإعدادات العامة" => "GeneralSettings",
            "الحضور والانصراف" => "Attendance",
            "تقرير الرواتب" => "PayrollReport",
            "اداراة الموظفين" => "Employee",
            "الأجازات الرسمية" => "OfficialHolidays",

            _ => arabicName
        };
        #endregion
    }
}