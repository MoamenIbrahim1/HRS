using HumanResouresMangementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
   public class DeductionSetting
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DeductionType Type { get; set; }

        public decimal Value { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<EmployeeDeduction> employeeDeductions { get; set; }
    }
}
