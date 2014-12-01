using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

using LiiteriUrbanPlanningCore.Models;
using LiiteriUrbanPlanningCore.Queries;
using LiiteriUrbanPlanningCore.Repositories;
using LiiteriUrbanPlanningCore.Util;

namespace LiiteriUrbanPlanningAPI.Controllers
{
    public class ContactsController : ApiController
    {
        [Route("people/")]
        [HttpGet]
        public IEnumerable<Person> GetContacts(
            string search = null,
            string personType = null,
            int? ely = null,
            int? subRegion = null,
            int? county = null,
            int? greaterArea = null,
            int? administrativeCourt = null,
            int? municipality = null)
        {
            PersonQuery query = new PersonQuery();

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

            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();

            using (DbConnection db = new SqlConnection(connStr)) {
                db.Open();
                var repository = new PersonRepository(db);
                return (List<Person>) repository.FindAll(query);
            }
        }
    }
}