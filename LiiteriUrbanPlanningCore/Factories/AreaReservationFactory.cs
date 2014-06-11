using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public class AreaReservationFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var ar = new Models.AreaReservation();

            ar.Description = (string) this.GetValueOrNull(rdr, "Description").ToString();
            ar.MainMarkId = (int?) this.GetValueOrNull(rdr, "MainMarkId");
            ar.AreaSize = (decimal?) this.GetValueOrNull(rdr, "AreaSize");
            ar.AreaPercent = (decimal?) this.GetValueOrNull(rdr, "AreaPercent");
            ar.FloorSpace = (int?) this.GetValueOrNull(rdr, "FloorSpace");
            ar.Efficiency = (decimal?) this.GetValueOrNull(rdr, "Efficiency");
            ar.AreaChange = (decimal?) this.GetValueOrNull(rdr, "AreaChange");
            ar.FloorSpaceChange = (int?) this.GetValueOrNull(rdr, "FloorSpaceChange");

            return ar;
        }
    }
}
