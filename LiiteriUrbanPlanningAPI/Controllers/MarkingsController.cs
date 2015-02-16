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
    public class MarkingsController :
        ApiController,
        Core.Controllers.IMarkingsController
    {
        private Core.Controllers.IMarkingsController GetController()
        {
            if (ConfigurationManager.AppSettings["UseWCF"] == "true") {
                ChannelFactory<Core.Controllers.IMarkingsController> factory =
                    new ChannelFactory<Core.Controllers.IMarkingsController>(
                        "UrbanPlanningServiceEndpoint");
                return factory.CreateChannel();
            } else {
                return new Core.Controllers.MarkingsController();
            }
        }

        [Route("markings/{markingType}/{queryType}/")]
        [Route("markings/{markingType}/{queryType}/{municipalityId}/")]
        [HttpGet]
        public IEnumerable<Core.Models.Marking> GetMarkings(
            string markingType,
            string queryType,
            int? municipalityId = null,
            string mainMarkName = null,
            string name = null)
        {
            return this.GetController().GetMarkings(
                markingType,
                queryType,
                municipalityId,
                mainMarkName,
                name);
        }
    }
}