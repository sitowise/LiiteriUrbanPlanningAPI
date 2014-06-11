using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiiteriUrbanPlanningCore.Models
{
    public class BuildingConservation : IEntity
    {
        public int? ConservationTypeId { get; set; }
        public string ConservationTypeName { get; set; }
        public int? BuildingCount { get; set; }
        public int? FloorSpace { get; set; }
        public int? ChangeCount { get; set; }
        public int? ChangeFloorSpace { get; set; }
    }
}
