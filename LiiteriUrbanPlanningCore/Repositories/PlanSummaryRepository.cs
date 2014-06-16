using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class PlanSummaryRepository : SqlRepository<Models.PlanSummary>
    {
        public PlanSummaryRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.PlanSummary> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Models.PlanSummary> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanSummaryFactory());
        }

        public override Models.PlanSummary Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanSummaryFactory()).Single();
        }

        public override Models.PlanSummary First(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanSummaryFactory()).First();
        }
    }
}
