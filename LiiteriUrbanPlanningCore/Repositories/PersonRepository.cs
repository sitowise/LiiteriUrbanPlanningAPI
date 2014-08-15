using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public class PersonRepository: SqlRepository<Models.Person>
    {
        public PersonRepository(DbConnection dbConnection) : base(dbConnection)
        {
        }

        public override IEnumerable<Models.Person> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Models.Person> FindAll(Queries.ISqlQuery query)
        {
            return this.FindAll(query, new Factories.PersonFactory());
        }

        public override Models.Person Single(Queries.ISqlQuery query)
        {
            return this.FindAll(query).Single();
        }

        public override Models.Person First(Queries.ISqlQuery query)
        {
            return this.FindAll(query).First();
        }
    }
}