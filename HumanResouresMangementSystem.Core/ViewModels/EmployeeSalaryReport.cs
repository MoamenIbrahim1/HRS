using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.ViewModels
{
    public class EmployeeSalaryReport
    {
        public int? EmployeeId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public List<SalaryReportItemViewModel> Reports { get; set; } = new();


    }
}
