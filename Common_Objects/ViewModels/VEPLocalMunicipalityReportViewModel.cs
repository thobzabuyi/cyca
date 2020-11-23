using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEPLocalMunicipalityReportViewModel
    {
        public string RefNo { get; set; }

        public string VictimisationType { get; set; }

        public string SiteCode { get; set; }

        public string Race { get; set; }

        public string Gender { get; set; }

        public string SexualOrientation { get; set; }

        public string MaritalStatus { get; set; }

        public int? Age { get; set; }

        public string Disability { get; set; }

        public string Citizenship { get; set; }

        public string Settlement { get; set; }

        public string KnownPerpetrator { get; set; }

        public string PerpFamilyMember { get; set; }

        public string PerpCommunityMember { get; set; }

        public string ReportedToPolice { get; set; }

        public DateTime? DateOfIncident { get; set; }

        public DateTime? DateOfReporting { get; set; }
    }
}
