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
    /* The serialization of class will be used as the POST data when
     * providing a list of planIds for the various methods below */
    public class PlanSummaryRequest
    {
        public int[] PlanIds { get; set; }
    }

    [RoutePrefix("plansummary")]
    public class PlanSummaryController : ApiController
    {
        [Route("", Order = 2)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public PlanSummary GetPlanSummary(
            [FromBody] PlanSummaryRequest reqobj)
        {
            return this.GetPlanSummary(reqobj.PlanIds);
        }

        [Route("", Order = 2)]
        [Route("{planIds}/", Order = 2)]
        [HttpGet]
        public PlanSummary GetPlanSummary(
            int[] planIds) // int[] conversion done by IntegerArrayModelBinder
        {
            PlanSummaryQuery query = new PlanSummaryQuery();
            query.PlanIdIn = planIds;

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new PlanSummaryRepository(db);
                return (PlanSummary) repository.Single(query);
            }
        }

        [Route("areaReservations/{type}/", Order = 1)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public IEnumerable<AreaReservation> GetPlanSummaryAreaReservations(
            [FromBody] PlanSummaryRequest reqobj,
            string type)
        {
            return this.GetPlanSummaryAreaReservations(reqobj.PlanIds, type);
        }

        [Route("areaReservations/", Order = 1)]
        [Route("areaReservations/{type}/", Order = 1)]
        [Route("{planIds}/areaReservations/{type}/", Order = 1)]
        [Route("areaReservations/{type}/{planIds}/", Order = 1)]
        [HttpGet]
        public IEnumerable<AreaReservation> GetPlanSummaryAreaReservations(
            int[] planIds,
            string type)
        {
            AreaReservationQuery query = new AreaReservationQuery();
            query.PlanIdIn = planIds;

            switch (type) {
                case "main":
                    query.QueryType = (int) AreaReservationQuery.QueryTypes.Main;
                    break;
                case "sub":
                    query.QueryType = (int) AreaReservationQuery.QueryTypes.Sub;
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new AreaReservationRepository(db);
                return (List<AreaReservation>) repository.FindAll(query);
            }
        }

        [Route("undergroundAreas/{type}/", Order = 1)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public IEnumerable<UndergroundArea> GetUndergroundAreas(
            [FromBody] PlanSummaryRequest reqobj,
            string type)
        {
            return this.GetUndergroundAreas(reqobj.PlanIds, type);
        }

        [Route("undergroundAreas/", Order = 1)]
        [Route("undergroundAreas/{type}/", Order = 1)]
        [Route("{planIds}/undergroundAreas/{type}/", Order = 1)]
        [Route("undergroundAreas/{type}/{planIds}/", Order = 1)]
        [HttpGet]
        public IEnumerable<UndergroundArea> GetUndergroundAreas(
            int[] planIds,
            string type)
        {
            UndergroundAreaQuery query = new UndergroundAreaQuery();
            query.PlanIdIn = planIds;

            switch (type) {
                case "main":
                    query.QueryType = (int) UndergroundAreaQuery.QueryTypes.Main;
                    break;
                case "sub":
                    query.QueryType = (int) UndergroundAreaQuery.QueryTypes.Sub;
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new UndergroundAreaRepository(db);
                return (List<UndergroundArea>) repository.FindAll(query);
            }
        }

        [Route("buildingConservations/{type}/", Order = 1)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public IEnumerable<BuildingConservation> GetBuildingConservations(
            [FromBody] PlanSummaryRequest reqobj,
            string type)
        {
            return this.GetBuildingConservations(reqobj.PlanIds, type);
        }

        [Route("buildingConservations/", Order = 1)]
        [Route("buildingConservations/{type}/", Order = 1)]
        [Route("{planIds}/buildingConservations/{type}/", Order = 1)]
        [Route("buildingConservations/{type}/{planIds}/", Order = 1)]
        [HttpGet]
        public IEnumerable<BuildingConservation> GetBuildingConservations(
            int[] planIds,
            string type)
        {
            BuildingConservationQuery query = new BuildingConservationQuery();
            query.PlanIdIn = planIds;

            switch (type) {
                case "main":
                    query.QueryType = (int) BuildingConservationQuery.QueryTypes.Main;
                    break;
                case "sub":
                    query.QueryType = (int) BuildingConservationQuery.QueryTypes.Sub;
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new BuildingConservationRepository(db);
                return (List<BuildingConservation>) repository.FindAll(query);
            }
        }
    }
}