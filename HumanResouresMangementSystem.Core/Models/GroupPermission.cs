using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class GroupPermission
    {
        public int GroupId { get; set; }

        public HrGroup Group { get; set; }

        public int PermissionId { get; set; }

        public Permission Permission { get; set; }

        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}
