using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Employee> Employees { get; set; }=new List<Employee>();

    }
}
