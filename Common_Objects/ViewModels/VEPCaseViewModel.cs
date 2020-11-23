using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEPCaseViewModel
    {
        public int CaseID { get; set; }

        public int ClientId { get; set; }

        public string IncidentDetails { get; set; }

        public string PlaceOfIncident { get; set; }

        public int SettlementId { get; set; }

        public DateTime DateOfIncident { get; set; }

        public DateTime DateOfReporting { get; set; }

        public int KnowPerpetrator { get; set; }

        public int PerpFamilyMemeber { get; set; }

        public int PerpCommunityMember { get; set; }

        public int ReportedToPolice { get; set; }

        public string PoliceCaseNumber { get; set; }

        public string PoliceOBnumber { get; set; }

        public int VictimisationType { get; set; }

        public string VEPReference { get; set; }

        public DateTime DateCaptured { get; set; }

        public  int SexualOrientationId { get; set; }
    }
}
