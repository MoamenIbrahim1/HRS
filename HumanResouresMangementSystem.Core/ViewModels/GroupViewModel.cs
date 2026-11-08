using System.ComponentModel.DataAnnotations;

namespace HumanResouresMangementSystem.Core.Dto
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم المجموعة")]
        public string GroupName { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
        = new();
    }

}