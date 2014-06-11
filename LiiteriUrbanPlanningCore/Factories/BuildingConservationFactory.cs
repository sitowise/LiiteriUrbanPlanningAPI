using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public class BuildingConservationFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var bc = new Models.BuildingConservation();

            bc.ConservationTypeId = (int?) this.GetValueOrNull(rdr, "ConservationTypeId");
            bc.ConservationTypeName = (string) this.GetValueOrNull(rdr, "ConservationTypeName").ToString();
            bc.BuildingCount = (int?) this.GetValueOrNull(rdr, "BuildingCount");
            bc.FloorSpace = (int?) this.GetValueOrNull(rdr, "FloorSpace");
            bc.ChangeCount = (int?) this.GetValueOrNull(rdr, "ChangeCount");
            bc.ChangeFloorSpace = (int?) this.GetValueOrNull(rdr, "ChangeFloorSpace");

            return bc;
        }
    }
}
