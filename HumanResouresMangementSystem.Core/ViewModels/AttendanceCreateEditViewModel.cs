using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.ViewModels
{
    public class AttendanceCreateEditViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
    }
}
