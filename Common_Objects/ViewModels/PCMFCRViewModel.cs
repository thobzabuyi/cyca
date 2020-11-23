using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMFCRViewModel
    {
        public int FormalCourt_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public DateTime? Appearance_Date { get; set; }

        public int FormalCourtOutcome_Id { get; set; }
        public DateTime? CourtDate { get; set; }
        public string Remand { get; set; }
        public string RemandReason { get; set; }
        public DateTime? NextCourtDate { get; set; }
        public string CourtOutcome { get; set; }

        public int? Court_Id { get; set; }
        public string Court_Name { get; set; }

        public int? FormalCaseStatus_Id { get; set; }
        //FormalCaseStatus_Id
        public string FormalCaseStatus { get; set; }

        public virtual ICollection<CourtsTypeLookup> Courts_Type { get; set; }
        public virtual ICollection<CaseTypeLookup> Case_Type { get; set; }

        public virtual ICollection<PlacementRecomendationTypeLookupPcm> PlacementRecomendation_List { get; set; }
        public virtual ICollection<RecommendationTypeLookup> Recommendation_Type_List { get; set; }
        public int? Placement_Type_Id { get; set; }
        public int? PCM_Recommendation_Id { get; set; }
        public int? Recommendation_Type_Id { get; set; }

        public string descrRecommendation { get; set; }

        public string descrPlacement { get; set; }
        public string descrStatusCourt { get; set; }
        
    }

    public class CourtsType
    {
        public int? selectedCourts { get; set; }
        public IEnumerable<CourtsTypeLookup> Courts_Type_List { get; set; }
    }

    public class CourtsTypeLookup
    {
        public int Court_Id { get; set; }
        public string Court_Name { get; set; }
    }



    public class CaseTypes
    {
        public int? selectedCase { get; set; }
        public IEnumerable<CaseTypeLookup> Case_Type_List { get; set; }
    }

    public class CaseTypeLookup
    {
        public int? FormalCaseStatus_Id { get; set; }
        public string FormalCaseStatus { get; set; }
    }
}
