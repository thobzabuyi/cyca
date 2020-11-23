using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMPreliminaryViewModel
    {
        public int? Intake_Assessment_Id { get; set; }
        public int? Client_Id { get; set; }
        public string Preliminary_Assessment { get; set; }
        public string Presenting_Problem { get; set; }
        public string PreInquiryConducted { get; set; }
        public string ReasonPreInquiryConducted { get; set; }
        public DateTime? Assessment_Date { get; set; }
        public int PCM_Preliminary_Id { get; set; }
        public int? PCM_Case_Id { get; set; }

        [Required]
        [Display(Name = "Preliminary Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public Nullable<System.DateTime> PCM_Preliminary_Date { get; set; }
        public int? PCM_Preliminary_Status_Id { get; set; }
        public string PCM_Outcome_Reason { get; set; }
        public int? PCM_Offence_Id { get; set; }
        public int? PCM_Recommendation_Id { get; set; }
        public int Created_By { get; set; }
        public DateTime? Date_Created { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Date_Modified { get; set; }
        public virtual ICollection<StatusTypeLookup> Status_Type { get; set; }
        public string Preliminary_Status { get; set; }
        public int  Placement_Pre_Status_Id { get; set; }
        public int Placement_Pre_Recommended_Id { get; set;  }


       public virtual ICollection<RecommendationTypeLookup> Recommendation_Type { get; set; }
       public string Recommendation { get; set; }




        public string ProblemSubCategoryDescription { get; set; }
        public int Problem_Sub_Category_Id { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public int gender { get; set; }

        //public int PCM_Case_Id { get; set; }
        //public int? PCM_Recommendation_Id { get; set; }
        public int? Recommendation_Id { get; set; }
        public int? Recommendation_Type_Id { get; set; }

        
        public string Comments_For_Recommendation { get; set; }
        


        public int? Order_Id { get; set; }
        public int? Recomendation_Order_Id { get; set; }

        public int? PCM_Placement_Id { get; set; }
        public int?  Type_Of_Center_Id { get; set; }
        public int? Facility_Id { get; set; }

        public int? Person_Id { get; set; }



        public int? Offence_Category_Id { get; set; }
        public int? Charge_Id { get; set; }
        public string Offence_Circumstance { get; set; }
        public string Value_Of_Goods { get; set; }
        public string Value_Recovered { get; set; }
        public string IsChild_Responsible { get; set; }
        public string Responsibility_Details { get; set; }


        public DateTime? Date_Arrested { get; set; }
        public string Investigate_Officer_Name { get; set; }

        public string Description { get; set; }


        public virtual ICollection<PlacementRecomendationTypeLookupPcm> PlacementRecomendation_List { get; set; }

        [Display(Name = "Final Placement Options")]

        public int? Placement_Type_Id { get; set; }

        public virtual ICollection<PreliminaryStatusTypeLookupPcm> PreliminaryStatus_List { get; set; }

        [Display(Name = "Assessment Recommendations Type")]

        public int? Preliminary_Status_Id { get; set; }

        //[Display(Name = "Assessment Placement Type")]

        //public int? Preliminary_Status_Id { get; set; }

    }

    public class StatusType
    {
        public int? selectedStatus { get; set; }
        public IEnumerable<StatusTypeLookup> Status_Type_List { get; set; }
    }

    public class StatusTypeLookup
    {
        public int Preliminary_Status_Id { get; set; }
        public string Preliminary_Status { get; set; }
    }


    public class RecommendationType
    {
        public int? selectedRecommendation { get; set; }
        public IEnumerable<RecommendationTypeLookup> Recommendation_Type_List { get; set; }
    }

    public class RecommendationTypeLookup
    {
        public int PCM_Recommendation_Id { get; set; }
        public string Recommendation { get; set; }
    }


    public class PlacementRecomendation
    {
        public int? selectedPlacementRecomendation { get; set; }
        public IEnumerable<PlacementRecomendationTypeLookupPcm> PlacementRecomendation_List { get; set; }
    }

    public class PlacementRecomendationTypeLookupPcm
    {
        public int Placement_Type_Id { get; set; }
        public string Description { get; set; }
    }

    public class PreliminaryStatus
    {
        public int? selectedPreliminaryStatus { get; set; }
        public IEnumerable<PreliminaryStatusTypeLookupPcm> PreliminaryStatus_List { get; set; }
    }

    public class PreliminaryStatusTypeLookupPcm
    {
        public int Preliminary_Status_Id { get; set; }
        public string Description { get; set; }
    }


}
