using HumanResouresMangementSystem.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class HrGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
            = new List<User>();

        public ICollection<GroupPermission> GroupPermissions { get; set; }
            = new List<GroupPermission>();
    }
}
