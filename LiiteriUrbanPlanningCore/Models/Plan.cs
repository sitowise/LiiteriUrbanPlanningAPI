using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiiteriUrbanPlanningCore.Models
{
    public class Plan : PlanBrief, IEntity
    {
        public string MunicipalityName { get; set; }
        public string DecisionMaker { get; set; }
        public string DecisionNumber { get; set; }
        public decimal PlanArea { get; set; }
        public decimal? UndergroundArea { get; set; }
        public decimal? PlanAreaNew { get; set; }
        public decimal? PlanAreaChange { get; set; }
        public decimal? CoastlineLength { get; set; }
        public int? BuildingCountOwn { get; set; }
        public int? BuildingCountOther { get; set; }
        public int? BuildingCountOwnHoliday { get; set; }
        public int? BuildingCountOtherHoliday { get; set; }
    }
}