using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiiteriUrbanPlanningCore.Models
{
    public class PlanBrief : Models.IEntity
    {
        public int Id { get; set; }                     // Id
        public string Name { get; set; }                // Nimi
        public int MunicipalityId { get; set; }         // Kunta
        public string MunicipalityPlanId { get; set; }  // Kunnan kaavatunnus
        public string GeneratedPlanId { get; set; }     // Gen.kaavatunnus

        public DateTime? ApprovalDate { get; set; }     // Hyväksymispvm
        public DateTime? ProposalDate { get; set; }     // Ehdotuspvm
        public DateTime? InitialDate { get; set; }      // Vireilletulosta ilm. pvm
        public DateTime? FillDate { get; set; }         // Täyttämispvm
    }
}