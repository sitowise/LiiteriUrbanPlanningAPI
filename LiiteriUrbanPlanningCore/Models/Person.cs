using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiiteriUrbanPlanningCore.Models
{
    public class Person : Models.IEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string StreetName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string OrganizationName { get; set; }
        public string Office { get; set; }
        public string PersonType { get; set; }
        public string VatNumber { get; set; }
    }
}