using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

using LiiteriUrbanPlanningCore.Models;
using LiiteriUrbanPlanningCore.Queries;
using LiiteriUrbanPlanningCore.Repositories;
using LiiteriUrbanPlanningCore.Util;

namespace LiiteriUrbanPlanningAPI.Controllers
{
    public class MarkingsController : ApiController
    {
        [Route("markings/{markingType}/{queryType}/")]
        [Route("markings/{markingType}/{queryType}/{municipalityId}/")]
        [HttpGet]
        public IEnumerable<Marking> GetMarkings(
            string markingType,
            string queryType,
            int? municipalityId = null,
            string mainMarkName = null,
            string name = null)
        {
            MarkingQuery query = new MarkingQuery();

            switch (markingType + "/" + queryType) {
                case "areaReservations/standard":
                    query.QueryType = (int)
                        MarkingQuery.QueryTypes.AreaReservationsStandard;
                    break;
                case "areaReservations/municipality":
                    query.QueryType = (int)
                        MarkingQuery.QueryTypes.AreaReservationsMunicipality;
                    break;
                case "undergroundAreas/standard":
                    query.QueryType = (int)
                        MarkingQuery.QueryTypes.UndergroundAreasStandard;
                    break;
                case "undergroundAreas/municipality":
                    query.QueryType = (int)
                        MarkingQuery.QueryTypes.UndergroundAreasMunicipality;
                    break;
                default:
                    throw new ArgumentException("Invalid QueryType specified!");
            }

            query.MunicipalityIdIs = municipalityId;
            query.MainMarkNameIs = mainMarkName;
            query.NameIs = name;

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new MarkingRepository(db);
                return (List<Marking>) repository.FindAll(query);
            }
        }
    }
}
