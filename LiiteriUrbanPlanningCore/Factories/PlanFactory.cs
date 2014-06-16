using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public class PlanFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var p = new Models.Plan();

            p.Id = (int) rdr["Id"];
            p.Name = rdr["Name"].ToString();
            p.MunicipalityId = (int) rdr["MunicipalityId"];
            p.MunicipalityPlanId = rdr["MunicipalityPlanId"].ToString();
            p.GeneratedPlanId = rdr["GeneratedPlanId"].ToString();

            p.ApprovalDate = (DateTime?) this.GetValueOrNull(rdr, "ApprovalDate");
            p.ProposalDate = (DateTime?) this.GetValueOrNull(rdr, "ProposalDate");
            p.InitialDate = (DateTime?) this.GetValueOrNull(rdr, "InitialDate");
            p.FillDate = (DateTime?) this.GetValueOrNull(rdr, "FillDate");

            p.MunicipalityName = rdr["MunicipalityName"].ToString();
            p.DecisionMaker = rdr["DecisionMaker"].ToString();
            p.DecisionNumber = rdr["DecisionNumber"].ToString();
            p.PlanArea = (decimal?) this.GetValueOrNull(rdr, "PlanArea");
            p.UndergroundArea = (decimal?) this.GetValueOrNull(rdr, "UndergroundArea");
            p.PlanAreaNew = (decimal?) this.GetValueOrNull(rdr, "PlanAreaNew");
            p.PlanAreaChange = (decimal?) this.GetValueOrNull(rdr, "PlanAreaChange");
            p.CoastlineLength = (decimal?) this.GetValueOrNull(rdr, "CoastlineLength");
            p.BuildingCountOwn = (int?) this.GetValueOrNull(rdr, "BuildingCountOwn");
            p.BuildingCountOther = (int?) this.GetValueOrNull(rdr, "BuildingCountOther");
            p.BuildingCountOwnHoliday = (int?) this.GetValueOrNull(rdr, "BuildingCountOwnHoliday");
            p.BuildingCountOtherHoliday = (int?) this.GetValueOrNull(rdr, "BuildingCountOtherHoliday");

            return p;
        }
    }
}