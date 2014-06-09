using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class RegionRepository : SqlRepository<Models.Region>
    {
        public RegionRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.Region> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Models.Region> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.RegionFactory());
        }

        public override Models.Region Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.RegionFactory()).Single();
        }

        public override Models.Region First(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.RegionFactory()).First();
        }
    }
}
