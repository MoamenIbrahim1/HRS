using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Dto
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "اسم الموظف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "من فضلك ادخل رقم تليفون صحيح (11 رقم)")]
        [Display(Name = "رقم التليفون")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "النوع")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "الجنسية")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الميلاد")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "الرقم القومي")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "من فضلك أدخل تاريخ التعاقد")]
        [DataType(DataType.Date)]
    
        public DateTime ContractDate { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Range(0, double.MaxValue, ErrorMessage = "من فضلك ادخل قيمة راتب صحيحة")]
        [Display(Name = "الراتب")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Time)]
        [Display(Name = "موعد الحضور")]
        public TimeSpan CheckInTime { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Time)]
        [Display(Name = "موعد الانصراف")]
        public TimeSpan CheckOutTime { get; set; }
    }
}
