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
    public interface IPlanSummaryController
    {
        [OperationContract]
        Models.PlanSummary GetPlanSummary(int[] planIds);

        [OperationContract]
        IEnumerable<Models.AreaReservation> GetPlanSummaryAreaReservations(
            int[] planIds,
            string type);

        [OperationContract]
        IEnumerable<Models.UndergroundArea> GetUndergroundAreas(
            int[] planIds,
            string type);

        [OperationContract]
        IEnumerable<Models.BuildingConservation> GetBuildingConservations(
            int[] planIds,
            string type);
    }

    public class PlanSummaryController : IPlanSummaryController
    {
        public Models.PlanSummary GetPlanSummary(
            int[] planIds) // int[] conversion done by IntegerArrayModelBinder
        {
            Queries.PlanSummaryQuery query = new Queries.PlanSummaryQuery();
            query.PlanIdIn = planIds;

            string connStr = ConfigurationManager
                .ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new Repositories.PlanSummaryRepository(db);
                return (Models.PlanSummary) repository.Single(query);
            }
        }

        public IEnumerable<Models.AreaReservation> GetPlanSummaryAreaReservations(
            int[] planIds,
            string type)
        {
            var query = new Queries.AreaReservationQuery();
            query.PlanIdIn = planIds;

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

            string connStr = ConfigurationManager
                .ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository =
                    new Repositories.AreaReservationRepository(db);
                return (List<Models.AreaReservation>)
                    repository.FindAll(query);
            }
        }

        public IEnumerable<Models.UndergroundArea> GetUndergroundAreas(
            int[] planIds,
            string type)
        {
            var query = new Queries.UndergroundAreaQuery();
            query.PlanIdIn = planIds;

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

            string connStr = ConfigurationManager
                .ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository =
                    new Repositories.UndergroundAreaRepository(db);
                return (List<Models.UndergroundArea>)
                    repository.FindAll(query);
            }
        }

        public IEnumerable<Models.BuildingConservation> GetBuildingConservations(
            int[] planIds,
            string type)
        {
            var query = new Queries.BuildingConservationQuery();
            query.PlanIdIn = planIds;

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

            string connStr = ConfigurationManager
                .ConnectionStrings["urbanPlanningDB"].ToString();

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