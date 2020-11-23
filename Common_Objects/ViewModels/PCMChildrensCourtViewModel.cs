using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMChildrensCourtViewModel
    {
        public int Children_Court_Id { get; set; }
        public Nullable<int> PCM_Recommendation_Id { get; set; }
        public Nullable<System.DateTime> Court_Expiry_Date { get; set; }
        public Nullable<System.DateTime> Prelim_Enquiry_Date { get; set; }
        public string Child_Need_Care { get; set; }
        public string Substance_Abuse_Treat { get; set; }
        //public bool? Substance_Abuse_Treat { get; set; }
        public Nullable<System.DateTime> Referral_Date { get; set; }
        public string Alternative_Placement { get; set; }
        public string Case_Manager { get; set; }
        public string Service_Provider { get; set; }

        //PCM_Formal_Court_Recommendation
        public int PCM_Formal_Court_Recomm_Id { get; set; }
        public Nullable<int> Type_Of_Center_Id { get; set; }
        public Nullable<int> Type_Of_Placement_Id { get; set; }
        public int? Recommendation_Type_Id { get; set; }
        public Nullable<int> Personal_Details_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
        public Nullable<bool> Withdrawal_Status { get; set; }

        // PCM_Childrens_Court_Outcome

        public int Outcome_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<System.DateTime> Court_Date { get; set; }
        public string Remand { get; set; }
        public string Reason_Remand { get; set; }
        public Nullable<System.DateTime> Next_Court_Date { get; set; }
        public string Court_Outcome { get; set; }
        public string Case_Status { get; set; }

        public virtual ICollection<CourtOutcomeCaseStatusTypeLookup> CourtOutcomeCaseStatusation_Type { get; set; }
        public string CourtOutcome_CaseStatus { get; set; }

        // PCM_Childrens_Court_Doc

        public int? Doc_ID { get; set; }
        public string Moudule_Name { get; set; }
        public string Doc_Name { get; set; }

        public virtual ICollection<PlacementRecomendationTypeLookupPcm> PlacementRecomendation_List { get; set; }
        public virtual ICollection<RecommendationTypeLookup> Recommendation_Type_List { get; set; }
        public int? Placement_Type_Id { get; set; }

        public int? Court_Case_Status_Id { get; set; }

        public string descrRecommendation { get; set; }

        public string descrPlacement { get; set; }
        public string descrStatusCourt { get; set; } 
    }


    public class CourtOutcomeCaseStatusType
    {
        public int? selectedCourtOutcomeCaseStatus { get; set; }
        public IEnumerable<CourtOutcomeCaseStatusTypeLookup> CourtOutcomeCaseStatus_Type_List { get; set; }
    }

    public class CourtOutcomeCaseStatusTypeLookup
    {
        public int CourtOutcome_CaseStatus_ID { get; set; }
        public string CourtOutcome_CaseStatus { get; set; }
    }
}
