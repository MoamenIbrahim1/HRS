using HumanResouresMangementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Dto
{
    public class WorkDataViewModel
    {

        public int Id { get; set; }
    [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime ContractDate { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime CheckInTime { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public DateTime CheckOutTime { get; set; }

      
    }
}
