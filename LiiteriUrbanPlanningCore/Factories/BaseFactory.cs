using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public abstract class BaseFactory : Factories.IFactory
    {
        public abstract Models.IEntity Create(DbDataReader rdr);

        public DateTime? GetDateTimeValue(DbDataReader rdr, string key)
        {
            if (Convert.IsDBNull(rdr[key])) return null;
            return (DateTime) rdr.GetDateTime(rdr.GetOrdinal(key));
        }
    }
}
