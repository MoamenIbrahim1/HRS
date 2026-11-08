using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class EmployeeOvertime
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public int OvertimeSettingId { get; set; }

        public OvertimeSetting OvertimeSetting { get; set; } = null!;

        public DateTime Date { get; set; }

        public double Hours { get; set; }

        public decimal HourRate { get; set; }

        public decimal Amount { get; set; }

        public bool IsApproved { get; set; }
    }
}
