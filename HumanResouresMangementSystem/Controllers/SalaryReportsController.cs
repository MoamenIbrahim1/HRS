using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.ViewModels;
using HumanResouresMangementSystem.DAL.Interfaces;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResouresMangementSystem.Controllers
{
    [Authorize]
    public class SalaryReportsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int CompanyStartYear = 2008; // تحديد سنة بداية تأسيس الشركة

        public SalaryReportsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HasPermission("PayrollReport.Index")]
        [HttpGet]
        public async Task<IActionResult> Index(int? employeeId, int? month, int? year)
        {
            month ??= DateTime.Now.Month;
            year ??= DateTime.Now.Year;

            var model = new EmployeeSalaryReport
            {
                EmployeeId = employeeId,
                Month = month.Value,
                Year = year.Value,
                Reports = new List<SalaryReportItemViewModel>()
            };

            // تحميل قائمة الموظفين للعرض في الـ Dropdown
            ViewBag.Employees = await _unitOfWork.Employees.GetQueryable().AsNoTracking().ToListAsync();

            // Rule 2: التحقق من صحة السنة (بدأت الشركة سنة 2008)
            if (year < CompanyStartYear)
            {
                ModelState.AddModelError("", "من فضلك اختر سنة صحيحه");
                return View(model);
            }

            // 1. استعلام الموظفين
            var employeesQuery = _unitOfWork.Employees.GetQueryable()
                .Include(e => e.Department)
                .Include(e => e.WorkData)
                .AsNoTracking();

            if (employeeId.HasValue)
            {
                // Rule 1: التحقق من وجود الموظف عند البحث
                bool employeeExists = await _unitOfWork.Employees.GetQueryable().AnyAsync(x => x.Id == employeeId.Value);
                if (!employeeExists)
                {
                    ModelState.AddModelError("", "من فضلك ادخل اسم موظف صالح");
                    return View(model);
                }

                employeesQuery = employeesQuery.Where(x => x.Id == employeeId.Value);
            }

            var employees = await employeesQuery.ToListAsync();

            // 2. جلب سجلات الحضور والإضافي والخصومات
            var attendances = await _unitOfWork.Attendances.GetQueryable()
                .AsNoTracking()
                .Where(x => x.Date.Month == month.Value && x.Date.Year == year.Value)
                .ToListAsync();

            var overtimes = await _unitOfWork.EmployeeOvertimes.GetQueryable()
                .AsNoTracking()
                .Where(x => x.Date.Month == month.Value && x.Date.Year == year.Value && x.IsApproved)
                .ToListAsync();

            var deductions = await _unitOfWork.EmployeeDeductions.GetQueryable()
                .AsNoTracking()
                .Where(x => x.Date.Month == month.Value && x.Date.Year == year.Value && x.IsApproved)
                .ToListAsync();

            int totalDaysInMonth = DateTime.DaysInMonth(year.Value, month.Value);

            // 3. تجميع البيانات لكل موظف
            foreach (var employee in employees)
            {
                var empAttendances = attendances.Where(a => a.EmployeeId == employee.Id).ToList();
                var empOvertimes = overtimes.Where(o => o.EmployeeId == employee.Id).ToList();
                var empDeductions = deductions.Where(d => d.EmployeeId == employee.Id).ToList();

                decimal basicSalary = employee.WorkData?.Salary ?? 0;
                int attendanceDays = empAttendances.Count(x => x.CheckInTime.HasValue);
                int absenceDays = totalDaysInMonth - attendanceDays;

                double overtimeHours = empOvertimes.Sum(x => x.Hours);
                decimal totalOvertime = empOvertimes.Sum(x => x.Amount);

                double deductionHours = empDeductions.Sum(x => x.Hours);
                decimal totalDeduction = empDeductions.Sum(x => x.Amount);

                var report = new SalaryReportItemViewModel
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.Name,
                    DepartmentName = employee.Department?.Name ?? "بدون قسم",
                    BasicSalary = basicSalary,
                    AttendanceDays = attendanceDays,
                    AbsenceDays = absenceDays,
                    OvertimeHours = overtimeHours,
                    TotalOvertime = totalOvertime,
                    DeductionHours = deductionHours,
                    TotalDeduction = totalDeduction,
                    NetSalary = basicSalary + totalOvertime - totalDeduction
                };

                model.Reports.Add(report);
            }

            return View(model);
        }

        [HasPermission("PayrollReport.Create")]
        [HttpGet]
        public async Task<IActionResult> Print(int employeeId, int month, int year)
        {
            // التحقق من السنة في صفحة الطباعة أيضاً
            if (year < CompanyStartYear) return BadRequest("من فضلك اختر سنة صحيحه");

            var employeeList = await _unitOfWork.Employees.FindAsync(
                e => e.Id == employeeId,
                e => e.Department,
                e => e.WorkData
            );

            var employee = employeeList.FirstOrDefault();

            if (employee == null) return NotFound("من فضلك ادخل اسم موظف صالح");

            var attendances = await _unitOfWork.Attendances.FindAsync(
                x => x.EmployeeId == employeeId && x.Date.Month == month && x.Date.Year == year
            );

            var overtimes = await _unitOfWork.EmployeeOvertimes.FindAsync(
                x => x.EmployeeId == employeeId && x.Date.Month == month && x.Date.Year == year && x.IsApproved
            );

            var deductions = await _unitOfWork.EmployeeDeductions.FindAsync(
                x => x.EmployeeId == employeeId && x.Date.Month == month && x.Date.Year == year && x.IsApproved
            );

            int totalDaysInMonth = DateTime.DaysInMonth(year, month);
            decimal basicSalary = employee.WorkData?.Salary ?? 0;
            int attendanceDays = attendances.Count(x => x.CheckInTime.HasValue);

            var item = new SalaryReportItemViewModel
            {
                EmployeeId = employee.Id,
                EmployeeName = employee.Name,
                DepartmentName = employee.Department?.Name ?? "بدون قسم",
                BasicSalary = basicSalary,
                AttendanceDays = attendanceDays,
                AbsenceDays = totalDaysInMonth - attendanceDays,
                OvertimeHours = (double)overtimes.Sum(x => x.Hours),
                TotalOvertime = overtimes.Sum(x => x.Amount),
                DeductionHours = (double)deductions.Sum(x => x.Hours),
                TotalDeduction = deductions.Sum(x => x.Amount),
                NetSalary = basicSalary + overtimes.Sum(x => x.Amount) - deductions.Sum(x => x.Amount)
            };

            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(item);
        }
    }
}