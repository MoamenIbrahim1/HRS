using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.ViewModels
{
   public class SalaryReportItemViewModel
    {

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public decimal BasicSalary { get; set; }

        public int AttendanceDays { get; set; }

        public int AbsenceDays { get; set; }

        public double OvertimeHours { get; set; }

        public double DeductionHours { get; set; }

        public decimal TotalOvertime { get; set; }

        public decimal TotalDeduction { get; set; }

        public decimal NetSalary { get; set; }

    }
}
