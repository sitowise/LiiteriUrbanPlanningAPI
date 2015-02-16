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
    public class VersionController : ApiController
    {
        [Route("version/")]
        [Route("v1/version/")]
        [HttpGet]
        public IEnumerable<Core.Models.ApplicationVersion> GetVersion()
        {
            var versions = new List<Core.Models.ApplicationVersion>();
            Core.Controllers.IVersionController controller;

            controller = new Core.Controllers.VersionController();

            versions.AddRange(controller.GetVersion());

            if (ConfigurationManager.AppSettings["UseWCF"] == "true") {
                ChannelFactory<Core.Controllers.IVersionController> factory =
                    new ChannelFactory<Core.Controllers.IVersionController>(
                        "UrbanPlanningServiceEndpoint");
                controller = factory.CreateChannel();
                versions.AddRange(controller.GetVersion());
            }

            return versions;
        }
    }
}