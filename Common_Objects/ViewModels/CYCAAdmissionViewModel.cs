
using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class EnrollModel
    {
        public string Id { get; set; }
        public int PersonId { get; set; }
    }

    public class GangViewModel
    {
        public int Gang_Membership_Id { get; set; }
        [Display(Name = "Select Gang Membership")]
        public Nullable<int> Gang_Membership_Type_Id { get; set; }
        public int clientID { get; set; }
        public Nullable<int> Admission_Id { get; set; }
        [Display(Name = "Does the child belong to any gang group?")]
        public bool Is_Member { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Date_Captured { get; set; }
        public Nullable<int> ReAdmission_Id { get; set; }
        [Display(Name = "Gang Membership Rank")]
        public string Membership_Rank { get; set; }
        public string Membership_Type { get; set; }
        [Display(Name = "Document Type")]
        public string selectedDocType { get; set; }
        public int Document_Type_Id { get; set; }
        public string Additional_Info { get; set; }
        public string Gang_Membership_Additional_Info { get; set; }

        public List<CYCA_GangAndTatooDocument> documents { get; set; }
        public string RequestType { get; set; }
    }

    public class TatooVewModel {
        public int Tatoo_Id { get; set; }
        public Nullable<int> Admission_Id { get; set; }
        [Display(Name = " Tattoo Visible")]
        public bool Tatoo_Visible { get; set; }
        public int clientID { get; set; }
        [Display(Name = "Tattoo Description")]
        public string Tatoo_Description { get; set; }
        [Display(Name = "Tattoo Size")]
        public string Tatoo_Size { get; set; }
        [Display(Name = "Tattoo Position")]
        public string Tatoo_Position { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Date_Captured { get; set; }
        public Nullable<int> ReAdmission_Id { get; set; }
        [Display(Name = "Document Type")]
        public string selectedDocType { get; set; }
        public int Document_Type_Id { get; set; }
        public string Additional_Info { get; set; }
        public string RequestType { get; set; }
        public List<CYCA_GangAndTatooDocument> documents { get; set; }
    }

    public class GangAndTatoosViewModel {
        public int clientID;
        public int admissionID;
        //public List<GangViewModel> gangs;
        //public List<TatooVewModel> tatoos;
        public List<GangViewModel> gangs { get; set; }
        public List<TatooVewModel> tatoos { get; set; }
       
    }
    public class CYCAAdmissionViewModel
    {
        #region CHILD DETAILS
        public int Person_Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        [Display(Name = "Know As")]
        public string Known_As { get; set; }
        [Display(Name = "Identity Type")]
        public int? Identification_Type_Id { get; set; }
        public string selectedIdType { get; set; }
        [Display(Name = "Identity Number")]
        public string Identification_Number { get; set; }
        public bool Is_Piva_Validated { get; set; }
        public string Piva_Transaction_Id { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime? Date_Of_Birth { get; set; }
        public int? Age { get; set; }
        [Display(Name = "Estimated Age")]
        public bool Is_Estimated_Age { get; set; }
        [Display(Name = "Language")]
        public int? Language_Id { get; set; }
        public string selectedLanguage { get; set; }
        [Display(Name = "Gender")]
        public int? Gender_Id { get; set; }
        public string selectedGender { get; set; }
        [Display(Name = "Marital Status")]
        public int? Marital_Status_Id { get; set; }
        public string selectedMaritalStatus { get; set; }
        [Display(Name = "Preferred Contact Type")]
        public int? Preferred_Contact_Type_Id { get; set; }
        public string selectedContact { get; set; }
        [Display(Name = "Religion")]
        public int? Religion_Id { get; set; }
        public string selectedReligion { get; set; }
        [Display(Name = "Phone Number")]
        public string Phone_Number { get; set; }
        [Display(Name = "Mobile Number")]
        public string Mobile_Phone_Number { get; set; }
        [Display(Name = ("Email Address"))]
        public string Email_Address { get; set; }
        [Display(Name = ("Population Group"))]
        public int? Population_Group_Id { get; set; }
        public string selectedPopulation { get; set; }
        [Display(Name = "Nationality")]
        public int? Nationality_Id { get; set; }
        public string selectNationality { get; set; }
        [Display(Name = "Disability Type")]
        public int? Disability_Type_Id { get; set; }
        public string selectedDisability { get; set; }
        [Display(Name = "Enrollment Verified")]
        public bool IsVerified { get; set; }
        [Display(Name = "Enrolled")]
        public bool HasBiometric { get; set; }
        [Display(Name = "PIVA Verified")]
        public bool IsPivaVerified { get; set; }

        public virtual ICollection<IdentificationTypeLookup> Identification_Type { get; set; }
        public virtual ICollection<LanguageTypeLookup> Language_Type { get; set; }
        public virtual ICollection<GenderTypeLookup> Gender_Type { get; set; }
        public virtual ICollection<MaritalTypeLookup> Marital_Type { get; set; }
        public virtual ICollection<ContactTypeLookup> Contact_Type { get; set; }
        public virtual ICollection<ReligionTypeLookup> Religion_Type { get; set; }
        public virtual ICollection<Population_GroupTypeLookup> Population_Group { get; set; }
        public virtual ICollection<NationalityTypeLookup> Nationality_Group { get; set; }
        public virtual ICollection<DisabilityTypeLookup> Disability_Group { get; set; }

        public int IntakeAssPar { get; set; }

        //Child Details
        [Display(Name =" Child Name:")]
        public string childFullNames { get; set; }
        [Display(Name ="Probation & Officer:")]
        public string probationOfficer { get; set; }
        [Display(Name ="Office Address:")]
        public string officeAddress { get; set; }
        [Display(Name ="Office Telephone Number:")]
        public string officeTelNumber { get; set; }
        [Display(Name ="Additional Contact Information")]
        public string AdditionalContactNo { get; set; }
        [Display(Name ="Magisterial District")]
        public string magisterialDistrict { get; set; }
        [Display(Name ="Assessment Date:")]
        public string assessmentDate { get; set; }
        [Display(Name ="Assessment Time:")]
        public string assessmentTime { get; set; }
        [Display(Name ="Court:")]
        public string court { get; set; }
        [Display(Name ="CASE Number:")]
        public string CASNumber { get; set; }
        [Display(Name ="Supervisor")]
        public string supervisor { get; set; }
        #endregion

        #region ADMISSION DETAILS
        //Admission Details
        public int Admission_Id { get; set; }
        public int ClientId { get; set; }
        public DateTime? CaseStartDate { get; set; }
        [Display(Name = "Admission Time:")]
        public string CaseStartTime { get; set; }
        [Display(Name = "Discharge date and time:")]
        public string CaseEndDate { get; set; }
        public DateTime? CaseEndTime { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int ReAdmissionCount { get; set; }
        public bool ActiveAdmission { get; set; }
        public bool ActiveReAdmission { get; set; }
        public int LatestAdmission { get; set; }
        public string Case_End_Date { get; set; }

        [Display(Name = "Comments and Observation:")]
        public string CommentsAndObservation { get; set; }
        [Display(Name = "Admission Reason:")]
        public int? VenueId { get; set; }
        public int? Admission_Type_Id { get; set; }
        [Display(Name = "Admission Reason")]
        public string selectedAdmissionType { get; set; }

        public string AdmissionDate { get; set; }
        public DateTime DateCaptured { get; set; }
        public int CapturedBy { get; set; }
        public int PcmAssNo { get; set; }
        public string selectedClietRef { get; set; }
        [Display(Name = "Venue:")]
        public string selectedVenue { get; set; }
        public string RequestType { get; set; }
        public string ChildFullName { get; set; }
        

        public virtual ICollection<PCMAdmissionTypeLookup> AdmissionType_List { get; set; }
        public virtual ICollection<AdmissionVenueLookUpCYCA> VenueType_List { get; set; }

        #endregion

        #region RE ADMISSION

        public int Re_Admission_Id { get; set; }  
        [Display(Name ="Re-Admission Date:")]
        public string Re_Admission_StartDate { get; set; }
        [Display(Name = "Discharge date and time:")]
        public DateTime? Re_Admission_EndDate { get; set; }
        [Display(Name = "Re-Admission Time:")]
        public DateTime? Re_Admission_CourtStartTime { get; set; }
        [Display(Name = "Comments:")]
        public string Comments { get; set; }
        public int? AdmissionType_Id { get; set; }
        [Display(Name = "Re-Admission Reason:")]
        public string selectedReAdmissionType { get; set; }
        public int? Venue_Id { get; set; }
        [Display(Name = "Venue:")]
        public string selectedReAdmissionVenue { get; set; }

        #endregion

      

        #region BODILY SEARCH
        public int Bodily_Search_Id { get; set; }
        [Display(Name ="Bodily Search Date:")]
        public DateTime? Bodily_Search_Date { get; set; }
        [Display(Name = "Bodily Search Time:")]
        public DateTime? Bodily_Search_Time { get; set; }       
        public string Description { get; set; }            
        public int? Conducted_By { get; set; }
        public int? Witnessed_By { get; set; }  
        public int? User_Id { get; set; }
        public DateTime? Date_Created { get; set; }
        public string Created_By { get; set; }
        public DateTime? Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public int? Search_Reason_Id { get; set; }
        [Display(Name = "Reason for Search:")]
        public string selectedSearchReason { get; set; }
        public IEnumerable<SelectListItem> selectedSearchReasons { get; set; }
        [Display(Name ="Conducted By:")]
        public string selectedConductor { get; set; }
        [Display(Name ="Witnessed By:")]
        public string selectedWitness { get; set; }
        [Display(Name ="Description of the physical location:")]
        public string physicalLocationDescription { get; set; }
        #endregion

        #region ILLEGAL ITEM FOUND
        public int Item_Found_Id { get; set; } 
        [Display(Name ="Quantity:")]
        public int? Quantity { get; set; }        
        public int? Handed_By { get; set; }
        [Display(Name = "Item Handed To:")]
        public string selectedHandedBy { get; set; }
        //[Display(Name ="Description of Illegal Item(s) Found:")]
        public string Item_Description { get; set; }
        public int? DocType_Id { get; set; } 
        public DateTime Date_Captured { get; set; }
        #endregion

        #region ADMISSION DOCUMENTS
        public int Document_Id { get; set; }
        public string Document_Ext { get; set; }
        public int? Document_Type_Id { get; set; } 
        [Display(Name ="Document Type:")]
        public string selectedDocType { get; set; }
        [Display(Name = "Date Uploaded::")]
        public DateTime DateSaved { get; set; }
        [Display(Name = "Time Uploaded:")]
        public string TimeSaved { get; set; }
        [Display(Name = "Document:")]
        public HttpPostedFileBase Document_Name { get; set; }  
        public DateTime Created_By_Id { get; set; }
        #endregion

        #region EXTRA MURAL ACTIVITIES
       
            public int Extra_Mural_Activity_Id { get; set; }
            [Display(Name = "Weight:")]
            public string Weight { get; set; }
            public int? Hobby_Id { get; set; }
            [Display(Name = "Child Hobbies/Interest:")]
            public string selectedHobby { get; set; }
            public int? Activity_Id { get; set; }
            [Display(Name = "Sport Activities:")]
            public string selectedSportActivity { get; set; }
            public int? Eye_Color_Id { get; set; }
            [Display(Name = "Eye Color:")]
            public string selectedEyeColor { get; set; }
            public int? Hair_Color_Id { get; set; }
            [Display(Name = "hair Color:")]
            public string selectedHairColor { get; set; }
            public int? Physical_Build_Id { get; set; }
            [Display(Name = "Physical Build:")]
            public string selectedPhysicalBuild { get; set; }
            //public int Admission_Id { get; set; }
            public string DateCreated { get; set; }
            [Display(Name = "Additional Description:")]
            public string additionalDescription { get; set; }
        
        #endregion


        #region GENERAL DEAILS
        //  PCM_General_Details
        public int General_Details_Id { get; set; }

        [Display(Name = "Consulted Sources")]
        public string Consulted_Sources { get; set; }
        [Display(Name = "Trace Effortst")]
        public string Trace_Efforts { get; set; }
        [Display(Name = "Additional Info")]
        public string Additional_Info { get; set; }
        public string GangMembership_Additional_Info { get; set; }
        public string SearchReason_Additional_Info { get; set; }
        [Display(Name = "Assessment End Date")]
        public DateTime? Assessment_DateEnd { get; set; }
        [Display(Name = "Assessment End Time")]

        public DateTime? Assessment_TimeEnd { get; set; }

        public int? Intake_Assessment_Id { get; set; }

        #endregion

        #region DROPDOWN ADMISSION

        public class AdmissionVenueCYCA
        {
            public int? VenueDetails { get; set; }
            public int? selectedVenue { get; set; }
            public IEnumerable<AdmissionVenueLookUpCYCA> Venue_List { get; set; }
        }

        public class AdmissionVenueLookUpCYCA
        {
            public int Venue_Id { get; set; }
            public string VenueName { get; set; }

        }

        public class Venue
        {
            public string VenueName { get; internal set; }
            public int? selectedVenue { get; set; }
            public IEnumerable<VenueLookUp> CYCAVenue_List { get; set; }
        }

        public class VenueLookUp
        {
            public int? Venue_Id { get; set; }
            public string VenueName { get; set; }
        }

        //public class AdmissionVenue
        //{
        //    public int? VenueDetails { get; set; }
        //    public IEnumerable<AdmissionVenueLookUpCYCA> Venue_List { get; set; }
        //}

        //public class AdmissionVenueLookUpCYCA
        //{
        //    public int? Venue_Id { get; set; }
        //    public string Description { get; set; }

        //}

        public class AdmissionType
        {
            public int? SelectAdmissionTypeDetails { get; set; }
            public IEnumerable<AdmissionTypeLookupCYCA> AdmissionType_List { get; set; }
        }

        public class AdmissionTypeLookupCYCA
        {
            public int? Admission_Type_Id { get; set; }
            public string Description { get; set; }

        }
        #endregion
        [Display(Name = "Does the child belong to a gang?")]
        public bool IsGangMember { get; set; }
        [Display(Name = "Gang Membership")]
        public string SelectedGangMemberType { get; set; }
        public int? Gang_Member_Type_Id { get; set; }
        public List<CYCA_Admissions_Document> files { get; set; }
        public List<LiteFiles> liteFiles { get; set; }
        
        public List<CYCA_ReAdmissions_Document> Rfiles { get; set; }
    }
    public class LiteFiles
    {
        public string Document_Name { get; set; }
        public int Document_Id { get; set; }
        public int? Admission_Id { get; set; }
        public int? ReAdmission_Id { get; set; }
        public int? DischargeId { get; set; }
        public int? Bodily_Search_Id { get; set; }
        public int? Item_Found_Id { get; set; }

    }

    public class CYCAAdmissionPartialViewModel
    {
        public string LoggedInUserFacility { get; set; }

        public List<CYCAAdmissionViewModel> CYCAAdmissionViewModels { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int Admission_Id { get; set; }
        public int ReAdmission_Id { get; set; }        
        public int ClientId { get; set; }
        public DateTime? CaseStartDate { get; set; }
        [Display(Name = "Admission Time:")]
        public string CaseStartTime { get; set; }
        [Display(Name = "Discharge date and time:")]
        public DateTime? CaseEndDate { get; set; }
        public DateTime? CaseEndTime { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int ReAdmissionCount { get; set; }

        [Display(Name = "Comments and Observation:")]
        public string CommentsAndObservation { get; set; }
        [Display(Name = "Admission Reason:")]
        public int? VenueId { get; set; }
        public int? Admission_Type_Id { get; set; }
        [Display(Name = "Admission Reason")]
        public string selectedAdmissionType { get; set; }

        public string AdmissionDate { get; set; }
        public DateTime DateCaptured { get; set; }
        public int CapturedBy { get; set; }
        public int PcmAssNo { get; set; }
        public string selectedClietRef { get; set; }
        [Display(Name = "Venue:")]
        public string selectedVenue { get; set; }

        public int Document_Id { get; set; }
        public string Document_Ext { get; set; }
        public int? Document_Type_Id { get; set; }
        [Display(Name = "Document Type:")]
        public string selectedDocType { get; set; }
        [Display(Name = "Date Uploaded::")]
        public DateTime DateSaved { get; set; }
        [Display(Name = "Time Uploaded:")]
        public string TimeSaved { get; set; }
        [Display(Name = "Document:")]
        public HttpPostedFileBase Document_Name { get; set; }
        public DateTime Created_By_Id { get; set; }

        [Display(Name = "Additional Info")]
        public string Additional_Info { get; set; }
        [Display(Name = "Enrolled")]
        public bool HasBiometric { get; set; }
        public int? Gang_Member_Type_Id { get; set; }


    }

    public class CYCAAdmissionBodilySearchViewModel
    {
        public string ChildFullName { get; set; }
        public int? Person_Id { get; set; }
        public int Bodily_Search_Id { get; set; }
        public string Facility { get; set; }       
        public string Bodily_Search_Date { get; set; }
        public string ReasonForSearch { get; set; }
        public string Description { get; set; }
        public int? Witnessed_By { get; set; }
        public string WitnessedBy { get; set; }
        public string CreatedBy { get; set; }
        public string Admission { get; set; }
        public int? Conducted_By { get; set; }
        public string ConductedBy { get; set; }
        public int? Search_Reason_Id { get; set; }
        public string DocumentName { get; set; }
        public int? DocumentId { get; set; }
        public int? DocType_Id { get; set; }
        public int? Admission_Id { get; set; }

        public List<CYCA_BodilySearch_Document> Files { get; set; }

    }

    //public class CYCAAdmissionBodilySearchPartiallViewModel
    //{
    //    public List<CYCAAdmissionBodilySearchViewModel> CYCAAdmissionBodySearchViewModels { get; set; }
    //    public int PersonId { get; set; }
    //}
}

