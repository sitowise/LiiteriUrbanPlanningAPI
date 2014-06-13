using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class MarkingRepository : SqlRepository<Models.Marking>
    {
        public MarkingRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.Marking> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Models.Marking> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.MarkingFactory());
        }

        public override Models.Marking Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.MarkingFactory()).Single();
        }

        public override Models.Marking First(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.MarkingFactory()).First();
        }
    }
}
