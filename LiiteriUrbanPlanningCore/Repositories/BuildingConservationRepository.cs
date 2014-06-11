using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class BuildingConservationRepository : SqlRepository<Models.BuildingConservation>
    {
        public BuildingConservationRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.BuildingConservation> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Models.BuildingConservation> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.BuildingConservationFactory());
        }

        public override Models.BuildingConservation Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.BuildingConservationFactory()).Single();
        }

        public override Models.BuildingConservation First(Queries.ISqlQuery query)
        {
            return this.FindAll(query,
                new Factories.BuildingConservationFactory()).First();
        }
    }
}
