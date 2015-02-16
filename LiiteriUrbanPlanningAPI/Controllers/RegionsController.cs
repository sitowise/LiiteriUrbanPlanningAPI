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
    public class RegionsController :
        ApiController,
        Core.Controllers.IRegionsController
    {
        private Core.Controllers.IRegionsController GetController()
        {
            if (ConfigurationManager.AppSettings["UseWCF"] == "true") {
                ChannelFactory<Core.Controllers.IRegionsController> factory =
                    new ChannelFactory<Core.Controllers.IRegionsController>(
                        "UrbanPlanningServiceEndpoint");
                return factory.CreateChannel();
            } else {
                return new Core.Controllers.RegionsController();
            }
        }

        [Route("regions/")]
        [HttpGet]
        public IEnumerable<Core.Models.RegionType> GetRegionTypes()
        {
            return this.GetController().GetRegionTypes();
        }

        [Route("regions/{regionType}/")]
        [HttpGet]
        public IEnumerable<Core.Models.Region> GetRegions(
            string regionType,
            int[] ely = null,
            int[] subRegion = null,
            int[] county = null,
            int[] greaterArea = null,
            int[] administrativeCourt = null)
        {
            return this.GetController().GetRegions(
                regionType,
                ely,
                subRegion,
                county,
                greaterArea,
                administrativeCourt);
        }
    }
}