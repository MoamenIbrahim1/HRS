using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class OfficialHoliday
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public bool IsRecurring { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
