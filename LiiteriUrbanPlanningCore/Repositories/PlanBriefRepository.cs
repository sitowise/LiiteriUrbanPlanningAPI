using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class PlanBriefRepository : SqlRepository<Models.PlanBrief>
    {
        public PlanBriefRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.PlanBrief> GetAll()
        {
            return this.FindAll(
                new Queries.PlanQuery(),
                new Factories.PlanBriefFactory());
        }

        public override IEnumerable<Models.PlanBrief> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanBriefFactory());
        }

        public override Models.PlanBrief Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanBriefFactory()).Single();
        }

        public override Models.PlanBrief First(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.PlanBriefFactory()).First();
        }
    }
}