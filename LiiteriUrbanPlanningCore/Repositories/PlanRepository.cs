using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class PlanRepository : SqlRepository<Models.Plan>
    {
        public PlanRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.Plan> GetAll()
        {
            return this.FindAll(
                new Queries.PlanQuery(),
                new Factories.PlanFactory());
        }

        public override IEnumerable<Models.Plan> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanFactory());
        }

        public override Models.Plan Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanFactory()).Single();
        }

        public override Models.Plan First(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanFactory()).First();
        }
    }
}