using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Factories
{
    public interface IFactory
    {
        Models.IEntity Create(DbDataReader rdr);
    }
}