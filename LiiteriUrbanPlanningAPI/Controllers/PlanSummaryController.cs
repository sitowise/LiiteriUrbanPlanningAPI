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
    public class PlanSummaryController : ApiController
    {
        [Route("plansummary/{planIds}")]
        [HttpGet]
        public PlanSummary GetPlans(
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
    }
}
