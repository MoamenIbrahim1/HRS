using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}