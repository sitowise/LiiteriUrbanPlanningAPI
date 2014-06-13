using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public class MarkingFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var marking = new Models.Marking();

            marking.MunicipalityId = (int?) this.GetValueOrNull(rdr, "MunicipalityId");
            marking.MunicipalityName = (string) this.GetValueOrNull(rdr, "MunicipalityName");
            marking.MainMarkId = (int?) this.GetValueOrNull(rdr, "MainMarkId");
            marking.MainMarkName = (string) this.GetValueOrNull(rdr, "MainMarkName");
            marking.Name = (string) this.GetValueOrNull(rdr, "Name").ToString();
            marking.Description = (string) this.GetValueOrNull(rdr, "Description");

            return marking;
        }
    }
}
