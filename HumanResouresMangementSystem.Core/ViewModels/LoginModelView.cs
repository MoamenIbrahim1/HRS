using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Dto
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "من فضلك ادخل اسم مستخدم صالح")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل كلمة مرور صالحة")]
        public string Password { get; set; }
    }
}
