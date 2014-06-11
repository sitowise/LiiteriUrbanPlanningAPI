using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiiteriUrbanPlanningCore.Models
{
    public class AreaReservation : IEntity
    {
        public string Description { get; set; }
        public int? MainMarkId { get; set; }
        public decimal? AreaSize { get; set; }
        public decimal? AreaPercent { get; set; }
        public int? FloorSpace { get; set; }
        public decimal? Efficiency { get; set; }
        public decimal? AreaChange { get; set; }
        public int? FloorSpaceChange { get; set; }
    }
}
