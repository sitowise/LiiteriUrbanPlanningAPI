using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

using System.ServiceModel; // WCF

namespace LiiteriUrbanPlanningCore.Controllers
{
    [ServiceContract]
    public interface IRegionsController
    {
        [OperationContract]
        IEnumerable<Models.RegionType> GetRegionTypes();

        [OperationContract]
        IEnumerable<Models.Region> GetRegions(
            string regionType,
            int[] ely = null,
            int[] subRegion = null,
            int[] county = null,
            int[] greaterArea = null,
            int[] administrativeCourt = null);
    }

    public class RegionsController : IRegionsController
    {
        public IEnumerable<Models.RegionType> GetRegionTypes()
        {
            return new List<Models.RegionType> {
                new Models.RegionType() { Name = "Suuralue", TypeName = "greaterArea"  },
                new Models.RegionType() { Name = "Hallinto-oikeus", TypeName = "administrativeCourt"  },
                new Models.RegionType() { Name = "Ympäristö-ELY", TypeName = "ely"  },
                new Models.RegionType() { Name = "Maakunta", TypeName = "county"  },
                new Models.RegionType() { Name = "Seutukunta", TypeName = "subRegion"  },
                new Models.RegionType() { Name = "Kunta", TypeName = "municipality"  },
            };
        }

        public IEnumerable<Models.Region> GetRegions(
            string regionType,
            int[] ely = null,
            int[] subRegion = null,
            int[] county = null,
            int[] greaterArea = null,
            int[] administrativeCourt = null)
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["urbanPlanningDB"].ToString();

            Queries.RegionQuery query = new Queries.RegionQuery(regionType);

            query.ElyIn = ely;
            query.SubRegionIn = subRegion;
            query.CountyIn = county;
            query.GreaterAreaIn = greaterArea;
            query.AdministrativeCourtIn = administrativeCourt;

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new Repositories.RegionRepository(db);
                return (List<Models.Region>) repository.FindAll(query);
            }
        }
    }
}