using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

using System.ServiceModel; // WCF

namespace LiiteriUrbanPlanningCore.Controllers
{
    [ServiceContract]
    public interface IContactsController
    {
        [OperationContract]
        IEnumerable<Models.Person> GetContacts(
            string search = null,
            string personType = null,
            int? ely = null,
            int? subRegion = null,
            int? county = null,
            int? greaterArea = null,
            int? administrativeCourt = null,
            int? municipality = null);
    }

    public class ContactsController : IContactsController
    {
        public IEnumerable<Models.Person> GetContacts(
            string search = null,
            string personType = null,
            int? ely = null,
            int? subRegion = null,
            int? county = null,
            int? greaterArea = null,
            int? administrativeCourt = null,
            int? municipality = null)
        {
            Queries.PersonQuery query = new Queries.PersonQuery();

            query.SearchKeyword = search;

            query.ElyIs = ely;
            query.SubRegionIs = subRegion;
            query.CountyIs = county;
            query.GreaterAreaIs = greaterArea;
            query.AdministrativeCourtIs = administrativeCourt;
            query.MunicipalityIs = municipality;

            if (personType == null) {
                personType = "any";
            }

            string[] personTypes = (
                from p in personType.Split(',')
                select p.Trim().ToLower()).ToArray();

            query.PersonTypes = personTypes;

            string connStr = ConfigurationManager
                .ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new Repositories.PersonRepository(db);
                return (List<Models.Person>) repository.FindAll(query);
            }
        }
    }
}