using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiiteriUrbanPlanningCore.Models
{
    public class Region : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegionType { get; set; }
        public int? OrderNumber { get; set; }
    }
}
