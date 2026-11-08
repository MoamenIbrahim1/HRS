using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class EmployeeTransaction
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

    }
}
