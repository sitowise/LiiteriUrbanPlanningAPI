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
    }
}