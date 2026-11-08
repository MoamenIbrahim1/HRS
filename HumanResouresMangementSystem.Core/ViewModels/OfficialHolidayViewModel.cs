using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.ViewModels
{
    public class OfficialHolidayViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الإجازة")]
        [Display(Name = "اسم الإجازة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى تحديد تاريخ الإجازة")]
        [DataType(DataType.Date)]
        [Display(Name = "التاريخ")]
        public DateTime HolidayDate { get; set; } = DateTime.Today;

        public List<OfficialHolidayViewModel> ExistingHolidays { get; set; } = new List<OfficialHolidayViewModel>();
    }
}
