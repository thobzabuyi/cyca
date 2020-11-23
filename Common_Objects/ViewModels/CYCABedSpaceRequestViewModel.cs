using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CYCABedSpaceRequestViewModel
    {
        
        #region RECOMENDATION

        public int Recommendation_Id { get; set; }
        [Display(Name = "Comments For Recommendation")]
        public string Comments_For_Recommendation { get; set; }
        [Display(Name = "Placement Type")]
        public int? Placement_Type_Id { get; set; }

        [Display(Name = "Recommendation Type")]
        public int? Recommendation_Type_Id { get; set; }

        public string selectRecommendationType { get; set; }
        public virtual ICollection<RecommendationTypeLookupPcm> RecommendationTyp_List { get; set; }

        public string selectPlacementType { get; set; }
        public virtual ICollection<PlacementTypeLookupPcm> PlacementType_List { get; set; }

        public string selectRecomendationOrder { get; set; }

        public virtual ICollection<RecomendationOrderLookupPcm> RecomendationOrder_List { get; set; }
        public string selectOrganization { get; set; }
        [Display(Name = "Organization")]
        public int Organization_Id { get; set; }
        public virtual ICollection<OrganizationLookupPcm> PCMOrganisation_List { get; set; }
        public string selectIdType { get; set; }
        [Display(Name = "Organisation Type")]
        public int IdType { get; set; }
        //public virtual ICollection<IdTypeLookupPcm> Organisation_Type_List { get; set; }


        [Display(Name = "Diversion Programmes")]
        public int[] Div_Program_Id { get; set; }


        public virtual ICollection<DiversionProgrammesLookupPcm> DiversionProgrammes_List { get; set; }

        [Display(Name = "Organisational Type")]
        public int? Organization_Id_Type { get; set; }

        public virtual ICollection<OrganisationTypeLookupPCM> OrganisationType_List { get; set; }
        public IEnumerable<LocalMunicipalityLookupAdopt> LocalMunicipality_List { get; set; }
        public IEnumerable<TownLookupPCM> Town_List { get; set; }

        public int PCM_Diversion_Recomm_Id { get; set; }
        [Display(Name = "Diversion Programmes")]
        public string DesrciptionDivesionPrograme { get; set; }

        #endregion

        #region  FACILITY BED SPACE

        public long Request_Id { get; set; }
        public string Sent_By { get; set; }
        public string Created_By { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        [Display(Name = "Subject")]
        public string Request_Subject { get; set; }
        public DateTime? Date_Recieved { get; set; }
        public string Time_Recieved { get; set; }
        public DateTime? Date_Created { get; set; }
        public DateTime? Date_Modified { get; set; }
        public int? Days_Lapsed { get; set; }
        public int Hours_Lapsed { get; set; }
        public int Hours_Modified { get; set; }
        public int? Hours { get; set; }
        public int? Outcome_Id { get; set; }
        [DisplayName("Respond")]
        public string outcomeDescription { get; set; }

        

        public int? TotalFemaleBedSpace { get; set; }
        public int? TotalMalesBedSpace { get; set; }
        public int? AvailableMaleSpace { get; set; }
        public int? AvailableFemaleSpace { get; set; }


        [Display(Name = "Comments")]
        [Required(ErrorMessage ="This field is required")]
        public string Request_Comments { get; set; }

        [Display(Name = "Facility")]
        public int? Facility_Id { get; set; }
        [Display(Name = "Facility")]
        public string SelectFacility { get; set; }       
        public string RequestOpenClose { get; set; }
        [Display(Name = "Status")]
        public int? Request_Status_Id { get; set; }
        public string Request_Status_Comments { get; set; }
        
        [Display(Name = "Bed Request Status")]
        public string selectBedRequestStatus { get; set; }
        public string selectPropationOfficer { get; set; }
        [Display(Name = "Probation Officer Name")]
        public string selectProbationOfficerName { get; set; }
        [Display(Name = "Probation Officer Surname")]
        public string selectProbationOfficerSurname { get; set; }
        [Display(Name = "Probation Officer Tel")]
        public string selectProbationOfficerTel { get; set; }
        [Display(Name = "Probation Officer Email")]
        public string selectProbationOfficerEmail { get; set; }
        [Display(Name = "Court")]
        public string courtName { get; set; }
        [Display(Name = "Province")]
        public string selectProvince { get; set; }
        [Display(Name = "Child Name")]
        public string selectClientName { get; set; }
        [Display(Name = "Child Gender")]
        public string selectClientGender { get; set; }
        [Display(Name = "Offence Details")]
        public string selectOffenceDetails { get; set; }
        [Display(Name = "Request Status")]
        public string selectRequestStatus { get; set; }
        [Display(Name = "Admission Type")]
        public string selectAdmissionType { get; set; }
        [Display(Name = "Facility Name")]
        public string selectFacilityName { get; set; }
        [Display(Name = "Facility contact No")]
        public string selectFaciltyTel { get; set; }
        [Display(Name = "Facility Manager Email")]
        public string selectFacilityManagerEmail { get; set; }
        [Display(Name = "Reply Date")]
        public string Date_Closed { get; set; }
        [Display(Name = "Reply Time")]
        public string Time_Replied { get; set; }
        public int? Count_Declined { get; set; }
        public int? Count_Accepted { get; set; }


        public int Province_Id { get; set; }


        public List<CYCACaseGridMain> CycaClients_Case_List { get; set; }


        public List<CYCACaseGridMain> PCMallassessment { get; set; }

        public List<Client> Client_List { get; set; }

        public virtual ICollection<PCMAdmissionTypeLookup> AdmissionType_List { get; set; }
        public virtual ICollection<PCMRequestStatusLookup> RequestStatus_List { get; set; }

        public virtual ICollection<TownTypeLookup> Town_Type { get; set; }
        public virtual ICollection<ProvinceLookupPCM> Province_List { get; set; }
        public virtual ICollection<DistrictLookupPCM> District_List { get; set; }

        [Display(Name = "Admission Type")]
        public int? Admission_Type_Id { get; set; }
        public string ProvinceDescription { get; set; }
        public string FacilityTell { get; set; }
        public string Facilityemail { get; set; }
        public string FacilityOfficialCapacity { get; set; }
        public int Program_Id { get; set; }
        public string ProgramNames { get; set; }
        public string ProgramDescription { get; set; }
        public string ProgramCapacity { get; set; }
        public string ProgramDuration { get; set; }
        public DateTime? ProgramStartDate { get; set; }
        public DateTime? ProgramEndDate { get; set; }

        public int? Male_Total_Space { get; set; }
        public int? Male_Available_Space { get; set; }
        public int? Male_Used_Space { get; set; }
        public int? Female_Total_Space { get; set; }
        public int? Female_Available_Space { get; set; }
        public int? Female_Used_Space { get; set; }

        #endregion

    }
}
