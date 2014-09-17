using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public class PlanSummaryFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var summary = new Models.PlanSummary();

            summary.PlanCount = (int) rdr["PlanCount"];
            summary.PlanArea = (decimal?) this.GetValueOrNull(rdr, "PlanArea");
            summary.PlanAreaNew = (decimal?) this.GetValueOrNull(rdr, "PlanAreaNew");
            summary.UndergroundArea = (decimal?) this.GetValueOrNull(rdr, "UndergroundArea");
            summary.PlanAreaChange = (decimal?) this.GetValueOrNull(rdr, "PlanAreaChange");
            summary.DurationAverage = (decimal?) this.GetValueOrNull(rdr, "DurationAverage");
            summary.DurationMedian = (decimal?) this.GetValueOrNull(rdr, "DurationMedian");
            summary.CoastlineLength = (decimal?) this.GetValueOrNull(rdr, "CoastlineLength");
            summary.BuildingCountOwn = (int?) this.GetValueOrNull(rdr, "BuildingCountOwn");
            summary.BuildingCountOther = (int?) this.GetValueOrNull(rdr, "BuildingCountOther");
            summary.BuildingCountOwnHoliday = (int?) this.GetValueOrNull(rdr, "BuildingCountOwnHoliday");
            summary.BuildingCountOtherHoliday = (int?) this.GetValueOrNull(rdr, "BuildingCountOtherHoliday");

            return summary;
        }
    }
}