using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Dto
{
    public class PermissionViewModel
    {
        public int PermissionId { get; set; }

        public string PageName { get; set; }

        public string ActionName { get; set; }

        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}
