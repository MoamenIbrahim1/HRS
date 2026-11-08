using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class WorkData
    {
        public int Id { get; set; }
        public DateTime ContractDate { get; set; }
        [Required(ErrorMessage = "يرجى تحديدالراتب ")]
        public decimal Salary { get; set; }
        [Required]
        public TimeSpan CheckInTime { get; set; }
        [Required]
        public TimeSpan CheckOutTime { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
