using HumanResouresMangementSystem.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalId { get; set; }
        public WorkData? WorkData { get; set; }
        public int? DepartmentId;
        public Department? Department {  get; set; }
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();


        public ICollection<EmployeeDeduction> EmployeeDeductions { get; set; }
            = new List<EmployeeDeduction>();


        public ICollection<EmployeeOvertime> EmployeeOvertimes { get; set; }
            = new List<EmployeeOvertime>();
    }
}
