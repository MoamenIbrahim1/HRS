using HumanResouresMangementSystem.Core.Entites;
using HumanResouresMangementSystem.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            // 1. الأقسام (Departments Seeding)
            var defaultDepartments = new[]
            {
                "قسم التطوير",
                "قسم المبيعات",
                "قسم التسويق"
            };

            foreach (var deptName in defaultDepartments)
            {
                var deptExists = await context.Departments.AnyAsync(d => d.Name == deptName);
                if (!deptExists)
                {
                    context.Departments.Add(new Department { Name = deptName });
                }
            }
            await context.SaveChangesAsync();

            // 2. Group (المجموعة الرئيسية)
            var adminGroup = await context.Groups
                .FirstOrDefaultAsync(x => x.Name == "HR");

            if (adminGroup == null)
            {
                adminGroup = new HrGroup
                {
                    Name = "HR"
                };

                context.Groups.Add(adminGroup);
                await context.SaveChangesAsync();
            }

            // 3. Permissions
            var permissionsData = new[]
            {
                // الموظفين
                new { Name = "employee.view", ControllerName = "Employee", ActionName = "Index" },
                new { Name = "employee.create", ControllerName = "Employee", ActionName = "Create" },
                new { Name = "employee.edit", ControllerName = "Employee", ActionName = "Edit" },
                new { Name = "employee.delete", ControllerName = "Employee", ActionName = "Delete" },

                // الإعدادات العامة
                new { Name = "generalsettings.view", ControllerName = "GeneralSettings", ActionName = "Index" },
                new { Name = "generalsettings.create", ControllerName = "GeneralSettings", ActionName = "Create" },
                new { Name = "generalsettings.edit", ControllerName = "GeneralSettings", ActionName = "Edit" },
                new { Name = "generalsettings.delete", ControllerName = "GeneralSettings", ActionName = "Delete" },

                // الإجازات الرسمية
                new { Name = "officialholidays.view", ControllerName = "OfficialHolidays", ActionName = "Index" },
                new { Name = "officialholidays.create", ControllerName = "OfficialHolidays", ActionName = "Create" },
                new { Name = "officialholidays.edit", ControllerName = "OfficialHolidays", ActionName = "Edit" },
                new { Name = "officialholidays.delete", ControllerName = "OfficialHolidays", ActionName = "Delete" },

                // الحضور والانصراف
                new { Name = "attendance.view", ControllerName = "Attendance", ActionName = "Index" },
                new { Name = "attendance.create", ControllerName = "Attendance", ActionName = "Create" },
                new { Name = "attendance.edit", ControllerName = "Attendance", ActionName = "Edit" },
                new { Name = "attendance.delete", ControllerName = "Attendance", ActionName = "Delete" },

                // المستخدمين
                new { Name = "Users.View", ControllerName = "Users", ActionName = "Index" },
                new { Name = "Users.Create", ControllerName = "Users", ActionName = "Create" },
                new { Name = "Users.Edit", ControllerName = "Users", ActionName = "Edit" },
                new { Name = "Users.Delete", ControllerName = "Users", ActionName = "Delete" },
                    // إدارة المجموعات والصلاحيات (الصلاحية الناقصة)
    new { Name = "groups.view", ControllerName = "Admin", ActionName = "Permissions" },
    new { Name = "groups.edit", ControllerName = "Admin", ActionName = "Permissions" },
            };

            foreach (var permissionData in permissionsData)
            {
                var permission = await context.Permissions
                    .FirstOrDefaultAsync(x => x.Name == permissionData.Name);

                if (permission == null)
                {
                    permission = new Permission
                    {
                        Name = permissionData.Name,
                        ControllerName = permissionData.ControllerName,
                        ActionName = permissionData.ActionName
                    };

                    context.Permissions.Add(permission);
                    await context.SaveChangesAsync();
                }

                var exists = await context.GroupPermissions
                    .AnyAsync(x => x.GroupId == adminGroup.Id && x.PermissionId == permission.Id);

                if (!exists)
                {
                    context.GroupPermissions.Add(new GroupPermission
                    {
                        GroupId = adminGroup.Id,
                        PermissionId = permission.Id
                    });
                }
            }

            await context.SaveChangesAsync();

            // 4. HR Admin User
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.UserName == "admin");

            if (user == null)
            {
                var passwordHasher = new PasswordHasher<User>();

                user = new User
                {
                    FullName = "HR Admin",
                    UserName = "admin",
                    Email = "admin@hrsystem.com",
                    GroupId = adminGroup.Id
                };

                user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            // 5. الموظفين (Employees Seeding)
            var devDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "قسم التطوير");
            var salesDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "قسم المبيعات");

            if (!await context.Employees.AnyAsync())
            {
                var employees = new List<Employee>
                {
                    new Employee { Name = "موظف 1", Address = "القاهرة", PhoneNumber = "01000000001", Gender = "ذكر", Nationality = "مصري", BirthDate = new DateTime(1995, 1, 1), NationalId = "12345678901234", DepartmentId = devDept?.Id },
                    new Employee { Name = "موظف 2", Address = "الجيزة", PhoneNumber = "01000000002", Gender = "ذكر", Nationality = "مصري", BirthDate = new DateTime(1996, 2, 2), NationalId = "12345678901235", DepartmentId = devDept?.Id },
                    new Employee { Name = "موظف 3", Address = "الإسكندرية", PhoneNumber = "01000000003", Gender = "أنثى", Nationality = "مصرية", BirthDate = new DateTime(1997, 3, 3), NationalId = "12345678901236", DepartmentId = salesDept?.Id }
                };

                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();
            }

            // 6. بيانات العمل والرواتب (WorkData Seeding)
            if (!await context.WorkData.AnyAsync())
            {
                var empList = await context.Employees.ToListAsync();
                var workDatas = new List<WorkData>();

                decimal baseSalary = 5000;
                foreach (var emp in empList)
                {
                    workDatas.Add(new WorkData
                    {
                        EmployeeId = emp.Id,
                        Salary = baseSalary,
                       ContractDate = new DateTime(2023, 1, 1)
                    });
                    baseSalary += 1500; // رواتب مختلفة: 5000، 6500، 8000
                }

                await context.WorkData.AddRangeAsync(workDatas);
                await context.SaveChangesAsync();
            }

            // 7. الحضور والانصراف لشهر 9 لسنة 2026 (Attendance Seeding)
            if (!await context.Attendances.AnyAsync(a => a.Date.Month == 9 && a.Date.Year == 2026))
            {
                var empList = await context.Employees.ToListAsync();
                var attendances = new List<Attendance>();

                foreach (var emp in empList)
                {
                    // إدخال 22 يوم حضور لشهر سبتمبر 2026
                    for (int day = 1; day <= 22; day++)
                    {
                        attendances.Add(new Attendance
                        {
                            EmployeeId = emp.Id,
                            Date = new DateTime(2026, 9, day),
                            CheckInTime = new TimeSpan(9, 0, 0),
                            CheckOutTime = new TimeSpan(17, 0, 0)
                        });
                    }
                }

                await context.Attendances.AddRangeAsync(attendances);
                await context.SaveChangesAsync();
            }
            // 8. إعدادات الإضافي والخصومات (Settings Seeding)
            var overtimeSetting = await context.OvertimeSettings.FirstOrDefaultAsync();
            if (overtimeSetting == null)
            {
                overtimeSetting = new OvertimeSetting { Rate = 1.5m }; // أو حسب خصائص الجدول لديك
                context.OvertimeSettings.Add(overtimeSetting);
                await context.SaveChangesAsync();
            }
            var deductionSetting = await context.DeductionSettings.FirstOrDefaultAsync();
            if (deductionSetting == null)
            {
                deductionSetting = new DeductionSetting
                {
                    Name = "خصم تأخير",
                    Value = 50,
                    IsActive = true
                };
                context.DeductionSettings.Add(deductionSetting);
                await context.SaveChangesAsync();
            }

            // 9. الإضافي (Overtime Seeding)
            if (!await context.EmployeeOvertimes.AnyAsync(o => o.Date.Month == 9 && o.Date.Year == 2026))
            {
                var firstEmp = await context.Employees.FirstOrDefaultAsync();
                if (firstEmp != null)
                {
                    context.EmployeeOvertimes.Add(new EmployeeOvertime
                    {
                        EmployeeId = firstEmp.Id,
                        OvertimeSettingId = overtimeSetting.Id, // <--- إسناد الـ Foreign Key المطلوب
                        Date = new DateTime(2026, 9, 10),
                        Hours = 6,
                        Amount = 300,
                        IsApproved = true
                    });
                    await context.SaveChangesAsync();
                }
            }

            // 10. الخصومات (Deduction Seeding)
            if (!await context.EmployeeDeductions.AnyAsync(d => d.Date.Month == 9 && d.Date.Year == 2026))
            {
                var firstEmp = await context.Employees.FirstOrDefaultAsync();
                if (firstEmp != null)
                {
                    context.EmployeeDeductions.Add(new EmployeeDeduction
                    {
                        EmployeeId = firstEmp.Id,
                        DeductionSettingId = deductionSetting.Id, // <--- إسناد الـ Foreign Key المطلوب إن وجد
                        Date = new DateTime(2026, 9, 15),
                        Hours = 2,
                        Amount = 150,
                        IsApproved = true
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}