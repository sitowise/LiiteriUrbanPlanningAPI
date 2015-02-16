using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

using System.ServiceModel; // WCF

using Core = LiiteriUrbanPlanningCore;

namespace LiiteriUrbanPlanningAPI.Controllers
{
    public class PlansController :
        ApiController,
        Core.Controllers.IPlansController
    {
        private Core.Controllers.IPlansController GetController()
        {
            if (ConfigurationManager.AppSettings["UseWCF"] == "true") {
                ChannelFactory<Core.Controllers.IPlansController> factory =
                    new ChannelFactory<Core.Controllers.IPlansController>(
                        "UrbanPlanningServiceEndpoint");
                return factory.CreateChannel();
            } else {
                return new Core.Controllers.PlansController();
            }
        }

        [Route("plans/")]
        [HttpGet]
        public IEnumerable<Core.Models.PlanBrief> GetPlans(
            string keyword = null,
            string planName = null,
            int[] tyviId = null,
            string[] generatedPlanId = null,
            string[] municipalityPlanId = null,
            string[] approver = null,
            string planType = null,
            Core.Util.DateRange approvalDateWithin = null,
            Core.Util.DateRange proposalDateWithin = null,
            Core.Util.DateRange initialDateWithin = null,
            Core.Util.DateRange fillDateWithin = null,
            int[] ely = null,
            int[] subRegion = null,
            int[] county = null,
            int[] greaterArea = null,
            int[] administrativeCourt = null,
            int[] municipality = null)
        {
            return this.GetController().GetPlans(
                keyword,
                planName,
                tyviId,
                generatedPlanId,
                municipalityPlanId,
                approver,
                planType,
                approvalDateWithin,
                proposalDateWithin,
                initialDateWithin,
                fillDateWithin,
                ely,
                subRegion,
                county,
                greaterArea,
                administrativeCourt,
                municipality);
        }

        [Route("plans/{id}")]
        [HttpGet]
        public Core.Models.Plan GetPlan(int id)
        {
            return this.GetController().GetPlan(id);
        }

        [Route("plans/{id}/areaReservations/{type}/")]
        [HttpGet]
        public IEnumerable<Core.Models.AreaReservation> GetAreaReservations(
            int id, string type)
        {
            return this.GetController().GetAreaReservations(id, type);
        }

        [Route("plans/{id}/undergroundAreas/{type}/")]
        [HttpGet]
        public IEnumerable<Core.Models.UndergroundArea> GetUndergroundAreas(
            int id, string type)
        {
            return this.GetController().GetUndergroundAreas(id, type);
        }

        [Route("plans/{id}/buildingConservations/{type}/")]
        [HttpGet]
        public IEnumerable<Core.Models.BuildingConservation> GetBuildingConservations(
            int id, string type)
        {
            return this.GetController().GetBuildingConservations(id, type);
        }
    }
}