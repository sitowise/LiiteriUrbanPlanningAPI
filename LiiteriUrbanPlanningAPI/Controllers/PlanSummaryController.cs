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
    [RoutePrefix("plansummary")]
    public class PlanSummaryController :
        ApiController,
        Core.Controllers.IPlanSummaryController
    {
        public class PlanSummaryRequest
        {
            public int[] PlanIds { get; set; }
        }

        private Core.Controllers.IPlanSummaryController GetController()
        {
            if (ConfigurationManager.AppSettings["UseWCF"] == "true") {
                ChannelFactory<Core.Controllers.IPlanSummaryController> factory =
                    new ChannelFactory<Core.Controllers.IPlanSummaryController>(
                        "UrbanPlanningServiceEndpoint");
                return factory.CreateChannel();
            } else {
                return new Core.Controllers.PlanSummaryController();
            }
        }

        [Route("", Order = 2)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public Core.Models.PlanSummary GetPlanSummary(
            [FromBody] PlanSummaryRequest reqobj)
        {
            return this.GetPlanSummary(reqobj.PlanIds);
        }

        [Route("", Order = 2)]
        [Route("{planIds}/", Order = 2)]
        [HttpGet]
        public Core.Models.PlanSummary GetPlanSummary(
            int[] planIds) // int[] conversion done by IntegerArrayModelBinder
        {
            return this.GetController().GetPlanSummary(planIds);
        }

        [Route("areaReservations/{type}/", Order = 1)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public IEnumerable<Core.Models.AreaReservation> GetPlanSummaryAreaReservations(
            [FromBody] PlanSummaryRequest reqobj,
            string type)
        {
            return this.GetPlanSummaryAreaReservations(reqobj.PlanIds, type);
        }

        [Route("areaReservations/", Order = 1)]
        [Route("areaReservations/{type}/", Order = 1)]
        [Route("{planIds}/areaReservations/{type}/", Order = 1)]
        [Route("areaReservations/{type}/{planIds}/", Order = 1)]
        [HttpGet]
        public IEnumerable<Core.Models.AreaReservation> GetPlanSummaryAreaReservations(
            int[] planIds,
            string type)
        {
            return this.GetPlanSummaryAreaReservations(planIds, type);
        }

        [Route("undergroundAreas/{type}/", Order = 1)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public IEnumerable<Core.Models.UndergroundArea> GetUndergroundAreas(
            [FromBody] PlanSummaryRequest reqobj,
            string type)
        {
            return this.GetUndergroundAreas(reqobj.PlanIds, type);
        }

        [Route("undergroundAreas/", Order = 1)]
        [Route("undergroundAreas/{type}/", Order = 1)]
        [Route("{planIds}/undergroundAreas/{type}/", Order = 1)]
        [Route("undergroundAreas/{type}/{planIds}/", Order = 1)]
        [HttpGet]
        public IEnumerable<Core.Models.UndergroundArea> GetUndergroundAreas(
            int[] planIds,
            string type)
        {
            return this.GetController().GetUndergroundAreas(planIds, type);
        }

        [Route("buildingConservations/{type}/", Order = 1)]
        [HttpPost]
        /* Support POST here since we can receive a large number of Ids */
        public IEnumerable<Core.Models.BuildingConservation> GetBuildingConservations(
            [FromBody] PlanSummaryRequest reqobj,
            string type)
        {
            return this.GetBuildingConservations(reqobj.PlanIds, type);
        }

        [Route("buildingConservations/", Order = 1)]
        [Route("buildingConservations/{type}/", Order = 1)]
        [Route("{planIds}/buildingConservations/{type}/", Order = 1)]
        [Route("buildingConservations/{type}/{planIds}/", Order = 1)]
        [HttpGet]
        public IEnumerable<Core.Models.BuildingConservation> GetBuildingConservations(
            int[] planIds,
            string type)
        {
            return this.GetController().GetBuildingConservations(planIds, type);
        }
    }
}