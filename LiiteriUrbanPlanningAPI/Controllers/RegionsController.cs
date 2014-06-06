using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Diagnostics;

using System.Data.Common;
using System.Data.SqlClient;

using System.Configuration;

using LiiteriUrbanPlanningCore.Models;
using LiiteriUrbanPlanningCore.Queries;
using LiiteriUrbanPlanningCore.Repositories;
using LiiteriUrbanPlanningCore.Util;

namespace LiiteriUrbanPlanningAPI.Controllers
{
    public class RegionsController : ApiController
    {
        [Route("regions/")]
        [HttpGet]
        public IEnumerable<RegionType> GetRegionTypes()
        {
            return new List<RegionType> {
                new RegionType() { Name = "Suuralue", TypeName = "greaterArea"  },
                new RegionType() { Name = "Hallinto-oikeus", TypeName = "administrativeCourt"  },
                new RegionType() { Name = "Ympäristö-ELY", TypeName = "ely"  },
                new RegionType() { Name = "Maakunta", TypeName = "county"  },
                new RegionType() { Name = "Seutukunta", TypeName = "subRegion"  },
                new RegionType() { Name = "Kunta", TypeName = "municipality"  },
            };
        }

        [Route("regions/{regionType}")]
        [HttpGet]
        public IEnumerable<Region> GetRegions(string regionType)
        {
            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            RegionQuery query = new RegionQuery(regionType);

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new RegionRepository(db);
                return (List<Region>) repository.FindAll(query);
            }
        }
    }
}
