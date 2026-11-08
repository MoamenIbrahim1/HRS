using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.ViewModels
{
    public class AttendanceReportFilterViewModel
    {
        public string? SearchTerm { get; set; } // بحث باسم الموظف أو القسم
        public int? DepartmentId { get; set; }  // فلترة بالقسم
        public DateTime? FromDate { get; set; } // من تاريخ
        public DateTime? ToDate { get; set; }   // إلى تاريخ

        public List<AttendanceReportItemViewModel> Results { get; set; } = new();
    }
}
