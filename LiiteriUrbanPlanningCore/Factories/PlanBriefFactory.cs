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

            p.Id = (int) rdr["Asemakaava_ID"];
            p.Name = rdr["Nimi"].ToString();
            p.MunicipalityId = (int) rdr["H_Kunta_Id"];
            p.MunicipalityPlanId = rdr["KuntaKaavaTunnus"].ToString();
            p.GeneratedPlanId = rdr["GenKaavaTunnus"].ToString();

            p.ApprovalDate = this.GetDateTimeValue(rdr, "HyvPvm");
            p.ProposalDate = this.GetDateTimeValue(rdr, "EhdotusPvm");
            p.InitialDate = this.GetDateTimeValue(rdr, "VireillePvm");
            p.FillDate = this.GetDateTimeValue(rdr, "TayttamisPvm");

            return p;
        }
    }
}