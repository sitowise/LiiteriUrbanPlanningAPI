using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    class RegionFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var r = new Models.Region();

            //r.Id = rdr.GetInt32(rdr.GetBytes(rdr.GetOrdinal("regionId")));
            r.Id = Convert.ToInt32(rdr["regionId"]); // source could be byte, int, or something else
            r.Name = rdr["name"].ToString();
            r.RegionType = rdr["regionType"].ToString();

            return r;
        }
    }
}
