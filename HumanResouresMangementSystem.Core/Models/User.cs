using HumanResouresMangementSystem.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace HumanResouresMangementSystem.Core.Entites
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PasswordHash { get; set; }

        public int? GroupId { get; set; }

        public HrGroup Group { get; set; }
       
    }
}
