using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiiteriUrbanPlanningCore.Models
{
    public class PlanSummary : IEntity
    {
        public int PlanCount { get; set; }
        public decimal PlanArea { get; set; }
        public decimal? PlanAreaNew { get; set; }
        public decimal? UndergroundArea { get; set; }
        public decimal? PlanAreaChange { get; set; }
        public decimal? DurationAverage { get; set; }
        public decimal? CoastlineLength { get; set; }
        public int? BuildingCountOwn { get; set; }
        public int? BuildingCountOther { get; set; }
        public int? BuildingCountOwnHoliday { get; set; }
        public int? BuildingCountOtherHoliday { get; set; }
    }
}
