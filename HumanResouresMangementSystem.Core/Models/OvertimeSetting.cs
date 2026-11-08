using HumanResouresMangementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class OvertimeSetting
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public OvertimeRateType RateType { get; set; }

        public decimal Rate { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<EmployeeOvertime> EmployeeOvertimes { get; set; }
        = new List<EmployeeOvertime>();
    }
}
