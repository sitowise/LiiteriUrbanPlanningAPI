using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public class UndergroundAreaFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var ua = new Models.UndergroundArea();

            ua.Description = (string) this.GetValueOrNull(rdr, "Description").ToString();
            ua.AreaSize = (decimal?) this.GetValueOrNull(rdr, "AreaSize");
            ua.AreaPercent = (decimal?) this.GetValueOrNull(rdr, "AreaPercent");
            ua.FloorSpace = (int?) this.GetValueOrNull(rdr, "FloorSpace");
            ua.AreaChange = (decimal?) this.GetValueOrNull(rdr, "AreaChange");
            ua.FloorSpaceChange = (int?) this.GetValueOrNull(rdr, "FloorSpaceChange");

            return ua;
        }
    }
}