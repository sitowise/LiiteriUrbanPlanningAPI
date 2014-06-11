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
    public class PlansController : ApiController
    {
        [Route("plans/")]
        [HttpGet]
        public IEnumerable<PlanBrief> GetPlans(
            string planName = null,
            string generatedPlanId = null,
            string municipalityPlanId = null,
            string approver = null,
            string planType = null,
            DateRange approvalDateWithin = null,
            DateRange proposalDateWithin = null,
            DateRange initialDateWithin = null,
            DateRange fillDateWithin = null,
            int? ely = null,
            int? subRegion = null,
            int? county = null,
            int? greaterArea = null,
            int? administrativeCourt = null,
            int? municipality = null)
        {
            PlanQuery query = new PlanQuery();

            if (planName != null) {
                query.NameLike = "%" + planName + "%";
            }

            query.GeneratedPlanIdIs = generatedPlanId;
            query.MunicipalityPlanIdIs = municipalityPlanId;
            query.ApproverIs = approver;

            if (planType != null) {
                query.PlanTypeIn = planType.Split(',').ToList().ConvertAll(
                    x => (new Dictionary<string, int>(){
                        {"T", (int) PlanQuery.PlanTypes.Normal},
                        {"R", (int) PlanQuery.PlanTypes.BeachPlan},
                        {"M", (int) PlanQuery.PlanTypes.WithUndergroundAreas},
                        {"tavallinen", (int) PlanQuery.PlanTypes.Normal},
                        {"rantaasemakaava", (int) PlanQuery.PlanTypes.BeachPlan},
                        {"maanalaistasisaltava", (int) PlanQuery.PlanTypes.WithUndergroundAreas},
                    })[x]).ToArray();
            }

            query.ApprovalDateWithin = approvalDateWithin;
            query.ProposalDateWithin = proposalDateWithin;
            query.InitialDateWithin = initialDateWithin;
            query.FillDateWithin = fillDateWithin;

            query.ElyIs = ely;
            query.SubRegionIs = subRegion;
            query.CountyIs = county;
            query.GreaterAreaIs = greaterArea;
            query.AdministrativeCourtIs = administrativeCourt;
            query.MunicipalityIs = municipality;

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new PlanBriefRepository(db);
                return (List<PlanBrief>) repository.FindAll(query);
            }
        }

        [Route("plans/{id}")]
        [HttpGet]
        public Plan GetPlan(int id)
        {
            PlanQuery query = new PlanQuery();

            query.IdIs = id;

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new PlanRepository(db);
                return (Plan) repository.Single(query);
            }
        }

        [Route("plans/{id}/areaReservations/{type}")]
        [HttpGet]
        public IEnumerable<AreaReservation> GetAreaReservations(
            int id, string type)
        {
            AreaReservationQuery query = new AreaReservationQuery(id);

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

        [Route("plans/{id}/undergroundAreas/{type}")]
        [HttpGet]
        public IEnumerable<UndergroundArea> GetUndergroundAreas(
            int id, string type)
        {
            UndergroundAreaQuery query = new UndergroundAreaQuery(id);

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

        [Route("plans/{id}/buildingConservations/{type}")]
        [HttpGet]
        public IEnumerable<BuildingConservation> GetBuildingConservations(
            int id, string type)
        {
            BuildingConservationQuery query = new BuildingConservationQuery(id);

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