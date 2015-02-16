using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Configuration;
using System.ServiceModel; // WCF

using Core = LiiteriUrbanPlanningCore;

namespace LiiteriUrbanPlanningAPI.Controllers
{
    public class ContactsController :
        ApiController,
        Core.Controllers.IContactsController
    {
        private Core.Controllers.IContactsController GetController()
        {
            if (ConfigurationManager.AppSettings["UseWCF"] == "true") {
                ChannelFactory<Core.Controllers.IContactsController> factory =
                    new ChannelFactory<Core.Controllers.IContactsController>(
                        "UrbanPlanningServiceEndpoint");
                return factory.CreateChannel();
            } else {
                return new Core.Controllers.ContactsController();
            }
        }

        [Route("people/")]
        [HttpGet]
        public IEnumerable<Core.Models.Person> GetContacts(
            string search = null,
            string personType = null,
            int? ely = null,
            int? subRegion = null,
            int? county = null,
            int? greaterArea = null,
            int? administrativeCourt = null,
            int? municipality = null)
        {
            return this.GetController().GetContacts(
                search,
                personType,
                ely,
                subRegion,
                county,
                greaterArea,
                administrativeCourt,
                municipality);
        }
    }
}