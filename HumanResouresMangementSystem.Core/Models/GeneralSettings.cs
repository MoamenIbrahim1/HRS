using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.Core.Models
{
    public class GeneralSettings
    {
        public int Id { get; set; }
        public double ExtraHoursRate { get; set; }
        public double DeductionHoursRate { get; set; }
        public DayOfWeek FirstWeekendDay { get; set; }
        public DayOfWeek? SecondWeekendDay { get; set; }
    }
}
