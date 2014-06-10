using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    class PlanBriefFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var p = new Models.PlanBrief();

            p.Id = (int) rdr["Id"];
            p.Name = rdr["Name"].ToString();
            p.MunicipalityId = (int) rdr["MunicipalityId"];
            p.MunicipalityPlanId = rdr["MunicipalityPlanId"].ToString();
            p.GeneratedPlanId = rdr["GeneratedPlanId"].ToString();

            p.ApprovalDate = (DateTime?) this.GetValueOrNull(rdr, "ApprovalDate");
            p.ProposalDate = (DateTime?) this.GetValueOrNull(rdr, "ProposalDate");
            p.InitialDate = (DateTime?) this.GetValueOrNull(rdr, "InitialDate");
            p.FillDate = (DateTime?) this.GetValueOrNull(rdr, "FillDate");

            return p;
        }
    }
}