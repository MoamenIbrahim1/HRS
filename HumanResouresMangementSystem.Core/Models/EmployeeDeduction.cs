using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class EmployeeDeduction
    {
        public int Id { get; set; }
        //Relation With Employee

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;
        //RELATION WITh Detuction setting

        public int DeductionSettingId { get; set; }

        public DeductionSetting DeductionSetting { get; set; } = null!;
        public int Hours {  get; set; }
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string? Reason { get; set; }

        public bool IsApproved { get; set; }
    }
}
