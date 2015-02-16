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
    public interface IPlansController
    {
        [OperationContract]
        IEnumerable<Models.PlanBrief> GetPlans(
            string keyword = null,
            string planName = null,
            int[] tyviId = null,
            string[] generatedPlanId = null,
            string[] municipalityPlanId = null,
            string[] approver = null,
            string planType = null,
            Util.DateRange approvalDateWithin = null,
            Util.DateRange proposalDateWithin = null,
            Util.DateRange initialDateWithin = null,
            Util.DateRange fillDateWithin = null,
            int[] ely = null,
            int[] subRegion = null,
            int[] county = null,
            int[] greaterArea = null,
            int[] administrativeCourt = null,
            int[] municipality = null);

        [OperationContract]
        Models.Plan GetPlan(int id);

        [OperationContract]
        IEnumerable<Models.AreaReservation> GetAreaReservations(
            int id, string type);

        [OperationContract]
        IEnumerable<Models.UndergroundArea> GetUndergroundAreas(
            int id, string type);

        [OperationContract]
        IEnumerable<Models.BuildingConservation> GetBuildingConservations(
            int id, string type);
    }

    public class PlansController : IPlansController
    {
        public IEnumerable<Models.PlanBrief> GetPlans(
            string keyword = null,
            string planName = null,
            int[] tyviId = null,
            string[] generatedPlanId = null,
            string[] municipalityPlanId = null,
            string[] approver = null,
            string planType = null,
            Util.DateRange approvalDateWithin = null,
            Util.DateRange proposalDateWithin = null,
            Util.DateRange initialDateWithin = null,
            Util.DateRange fillDateWithin = null,
            int[] ely = null,
            int[] subRegion = null,
            int[] county = null,
            int[] greaterArea = null,
            int[] administrativeCourt = null,
            int[] municipality = null)
        {
            Queries.PlanQuery query = new Queries.PlanQuery();

            if (planName != null) {
                query.NameLike = "%" + planName + "%";
            }

            query.KeywordSearch = keyword;
            query.GeneratedPlanIdIn = generatedPlanId;
            query.MunicipalityPlanIdIn = municipalityPlanId;
            query.ApproverIn = approver;
            query.TyviIdIn = tyviId;

            if (planType != null) {
                query.PlanTypeIn = planType.Split(',').ToList().ConvertAll(
                    x => (new Dictionary<string, int>(){
                        {"T", (int) Queries.PlanQuery.PlanTypes.Normal},
                        {"R", (int) Queries.PlanQuery.PlanTypes.BeachPlan},
                        {"M", (int) Queries.PlanQuery.PlanTypes.WithUndergroundAreas},
                        {"tavallinen", (int) Queries.PlanQuery.PlanTypes.Normal},
                        {"rantaasemakaava", (int) Queries.PlanQuery.PlanTypes.BeachPlan},
                        {"maanalaistasisaltava", (int) Queries.PlanQuery.PlanTypes.WithUndergroundAreas},
                    })[x]).ToArray();
            }

            query.ApprovalDateWithin = approvalDateWithin;
            query.ProposalDateWithin = proposalDateWithin;
            query.InitialDateWithin = initialDateWithin;
            query.FillDateWithin = fillDateWithin;

            query.ElyIn = ely;
            query.SubRegionIn = subRegion;
            query.CountyIn = county;
            query.GreaterAreaIn = greaterArea;
            query.AdministrativeCourtIn = administrativeCourt;
            query.MunicipalityIn = municipality;

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new Repositories.PlanBriefRepository(db);
                return (List<Models.PlanBrief>) repository.FindAll(query);
            }
        }

        public Models.Plan GetPlan(int id)
        {
            Queries.PlanQuery query = new Queries.PlanQuery();

            query.IdIs = id;

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new Repositories.PlanRepository(db);
                return (Models.Plan) repository.Single(query);
            }
        }

        public IEnumerable<Models.AreaReservation> GetAreaReservations(
            int id, string type)
        {
            var query = new Queries.AreaReservationQuery(id);

            switch (type) {
                case "main":
                    query.QueryType =
                        (int) Queries.AreaReservationQuery.QueryTypes.Main;
                    break;
                case "sub":
                    query.QueryType =
                        (int) Queries.AreaReservationQuery.QueryTypes.Sub;
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository =
                    new Repositories.AreaReservationRepository(db);
                return (List<Models.AreaReservation>)
                    repository.FindAll(query);
            }
        }

        public IEnumerable<Models.UndergroundArea> GetUndergroundAreas(
            int id, string type)
        {
            var query = new Queries.UndergroundAreaQuery(id);

            switch (type) {
                case "main":
                    query.QueryType =
                        (int) Queries.UndergroundAreaQuery.QueryTypes.Main;
                    break;
                case "sub":
                    query.QueryType =
                        (int) Queries.UndergroundAreaQuery.QueryTypes.Sub;
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository =
                    new Repositories.UndergroundAreaRepository(db);
                return (List<Models.UndergroundArea>)
                    repository.FindAll(query);
            }
        }

        public IEnumerable<Models.BuildingConservation> GetBuildingConservations(
            int id, string type)
        {
            var query = new Queries.BuildingConservationQuery(id);

            switch (type) {
                case "main":
                    query.QueryType =
                        (int) Queries.BuildingConservationQuery.QueryTypes.Main;
                    break;
                case "sub":
                    query.QueryType =
                        (int) Queries.BuildingConservationQuery.QueryTypes.Sub;
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository =
                    new Repositories.BuildingConservationRepository(db);
                return (List<Models.BuildingConservation>)
                    repository.FindAll(query);
            }
        }
    }
}