using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public ICollection<GroupPermission> GroupPermissions { get; set; }
            = new List<GroupPermission>();
    }
}
