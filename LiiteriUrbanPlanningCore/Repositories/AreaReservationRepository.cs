using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class AreaReservationRepository : SqlRepository<Models.AreaReservation>
    {
        public AreaReservationRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.AreaReservation> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Models.AreaReservation> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query, new Factories.AreaReservationFactory());
        }

        public override Models.AreaReservation Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query).Single();
        }

        public override Models.AreaReservation First(Queries.ISqlQuery query)
        {
            return this.FindAll(query).First();
        }
    }
}