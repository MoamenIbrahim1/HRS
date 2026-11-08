using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Dto
{
    public class GeneralSettingsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "معامل الإضافي مطلوب")]
        [Range(1, 24, ErrorMessage = "قيمة الإضافي يجب أن تكون بين 1 و 24")]
        [Display(Name = "الإضافي (بالساعات)")]
        public double ExtraHoursRate { get; set; }

        [Required(ErrorMessage = "معامل الخصم مطلوب")]
        [Range(1, 24, ErrorMessage = "قيمة الخصم يجب أن تكون بين 1 و 24")]
        [Display(Name = "الخصم (بالساعات)")]
        public double DeductionHoursRate { get; set; }

        [Required(ErrorMessage = "يرجى اختيار يوم الإجازة الأول")]
        [Display(Name = "يوم الإجازة الرسمي 1")]
        public DayOfWeek FirstWeekendDay { get; set; }

        [Display(Name = "يوم الإجازة الرسمي 2")]
        public DayOfWeek? SecondWeekendDay { get; set; }
    }
}
