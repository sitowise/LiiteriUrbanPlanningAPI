using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Diagnostics;

namespace LiiteriUrbanPlanningCore.Factories
{
    public class PersonFactory : BaseFactory, IFactory
    {
        public override Models.IEntity Create(DbDataReader rdr)
        {
            var person = new Models.Person();

            person.Firstname = rdr["FirstName"].ToString();
            person.Lastname = rdr["LastName"].ToString();
            person.StreetName = rdr["StreetName"].ToString();
            person.ZipCode = rdr["ZipCode"].ToString();
            person.City = rdr["City"].ToString();
            person.Phone = rdr["Phone"].ToString();
            person.Fax = rdr["Fax"].ToString();
            person.Email = rdr["Email"].ToString();
            person.OrganizationName = rdr["OrganizationName"].ToString();
            person.Office = rdr["Office"].ToString();
            person.VatNumber = rdr["VatNumber"].ToString();

            person.PersonType = rdr["PersonType"].ToString();

            return person;
        }
    }
}