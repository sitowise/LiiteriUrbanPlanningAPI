using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiiteriUrbanPlanningCore.Models
{
    public class Marking : IEntity
    {
        public int? MunicipalityId { get; set; }
        public string MunicipalityName { get; set; }
        public int? MainMarkId { get; set; }
        public string MainMarkName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
    }
}