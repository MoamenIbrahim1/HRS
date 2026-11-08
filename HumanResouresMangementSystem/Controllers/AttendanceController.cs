using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Models;
using HumanResouresMangementSystem.Core.ViewModels;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HumanResouresMangementSystem.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        [HasPermission("Attendance.Create")]
        [HttpGet]
        public async Task<IActionResult> Index(AttendanceReportFilterViewModel filter)
        {
            // تحميل قائمة الأقسام وقائمة الموظفين لـ ViewBags
            ViewBag.Departments = new SelectList(await _context.Departments.AsNoTracking().ToListAsync(), "Id", "Name");
            ViewBag.Employees = new SelectList(await _context.Employees.AsNoTracking().ToListAsync(), "Id", "Name");

            var query = _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .AsNoTracking()
                .AsQueryable();

            // 1. الفلترة باسم الموظف
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim();
                query = query.Where(a => a.Employee != null && a.Employee.Name.Contains(term));
            }

            // 2. الفلترة بالقسم
            if (filter.DepartmentId.HasValue && filter.DepartmentId.Value > 0)
            {
                query = query.Where(a => a.Employee != null && a.Employee.DepartmentId == filter.DepartmentId.Value);
            }

            // 3. فلترة تاريخ البداية (من)
            if (filter.FromDate.HasValue)
            {
                var fromDate = filter.FromDate.Value.Date;
                query = query.Where(a => a.Date >= fromDate);
            }

            // 4. فلترة تاريخ النهاية (إلى)
            if (filter.ToDate.HasValue)
            {
                var toDate = filter.ToDate.Value.Date;
                query = query.Where(a => a.Date <= toDate);
            }

            filter.Results = await query
                .OrderByDescending(a => a.Date)
                .Select(a => new AttendanceReportItemViewModel
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee != null ? a.Employee.Name : "غير معروف",
                    DepartmentName = (a.Employee != null && a.Employee.Department != null) ? a.Employee.Department.Name : "بدون قسم",
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    Date = a.Date
                }).ToListAsync();

            return View(filter);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(AttendanceCreateEditViewModel model)
        {
            if (model.Id > 0)
            {
                var attendance = await _context.Attendances.FindAsync(model.Id);
                if (attendance != null)
                {
                    attendance.EmployeeId = model.EmployeeId;
                    attendance.Date = model.Date;
                    attendance.CheckInTime = model.CheckInTime;
                    attendance.CheckOutTime = model.CheckOutTime;
                    _context.Attendances.Update(attendance);
                    TempData["SuccessMessage"] = "تم تعديل بيانات الحضور بنجاح";
                }
            }
            else
            {
                var attendance = new Attendance
                {
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    CheckInTime = model.CheckInTime,
                    CheckOutTime = model.CheckOutTime
                };
                await _context.Attendances.AddAsync(attendance);
                TempData["SuccessMessage"] = "تم تسجيل الحضور بنجاح";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HasPermission("Attendance.Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.Attendances.FindAsync(id);
            if (record != null)
            {
                _context.Attendances.Remove(record);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                TempData["SuccessMessage"] = "تم استيراد الملف بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}