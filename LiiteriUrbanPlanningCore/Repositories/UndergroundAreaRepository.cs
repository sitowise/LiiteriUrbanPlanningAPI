using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class UndergroundAreaRepository : SqlRepository<Models.UndergroundArea>
    {
        public UndergroundAreaRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.UndergroundArea> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Models.UndergroundArea> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.UndergroundAreaFactory());
        }

        public override Models.UndergroundArea Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.UndergroundAreaFactory()).Single();
        }

        public override Models.UndergroundArea First(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.UndergroundAreaFactory()).First();
        }
    }
}
