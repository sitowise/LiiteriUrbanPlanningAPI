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
    public interface IMarkingsController
    {
        [OperationContract]
        IEnumerable<Models.Marking> GetMarkings(
            string markingType,
            string queryType,
            int? municipalityId = null,
            string mainMarkName = null,
            string name = null);
    }

    public class MarkingsController : IMarkingsController
    {
        public IEnumerable<Models.Marking> GetMarkings(
            string markingType,
            string queryType,
            int? municipalityId = null,
            string mainMarkName = null,
            string name = null)
        {
            Queries.MarkingQuery query = new Queries.MarkingQuery();

            switch (markingType + "/" + queryType) {
                case "areaReservations/standard":
                    query.QueryType = (int)
                        Queries.MarkingQuery.QueryTypes.AreaReservationsStandard;
                    break;
                case "areaReservations/municipality":
                    query.QueryType = (int)
                        Queries.MarkingQuery.QueryTypes.AreaReservationsMunicipality;
                    break;
                case "undergroundAreas/standard":
                    query.QueryType = (int)
                        Queries.MarkingQuery.QueryTypes.UndergroundAreasStandard;
                    break;
                case "undergroundAreas/municipality":
                    query.QueryType = (int)
                        Queries.MarkingQuery.QueryTypes.UndergroundAreasMunicipality;
                    break;
                default:
                    throw new ArgumentException("Invalid QueryType specified!");
            }

            query.MunicipalityIdIs = municipalityId;
            query.MainMarkNameIs = mainMarkName;
            query.NameIs = name;

            string connStr = ConfigurationManager
                .ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new Repositories.MarkingRepository(db);
                return (List<Models.Marking>) repository.FindAll(query);
            }
        }
    }
}