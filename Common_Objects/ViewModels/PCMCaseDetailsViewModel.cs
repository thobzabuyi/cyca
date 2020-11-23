
using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc; 

namespace Common_Objects.ViewModels
{
    public class PCMCaseDetailsViewModel   
    {
        #region PCM CASE

        public int? Person_Id { get; set; }
        public int PCM_Case_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Client_Module_Id { get; set; }
        [Required]
        [Display(Name = "Date Arrested")]
        public DateTime? Date_Arrested { get; set; }
        [Required]
        [Display(Name = "Time Arrested")]
        public DateTime? Time_Arrested { get; set; }
        [Display(Name = "Arresting Officer")]
        public string Arresting_Officer_Name { get; set; }
        [Display(Name = "Investigate Officer")]
        public string Investigate_Officer_Name { get; set; }
        [Display(Name = "Police Station")]
        public int? SAPS_Info_Id { get; set; }
        public string SelectSAPOfficialInfo { get; set; }
        [Display(Name = "Detention Place")]
        public int? Detention_Place_Id { get; set; }
        [Display(Name = "Cas Number")]
        public string CAS_No { get; set; }
        [Display(Name = "First Appear Date")]
        public DateTime? First_Appear_Date { get; set; }
        [Display(Name = "Days Custody")]
        public string Num_Of_Days_Custody { get; set; }
        [Display(Name = "Has Legal Representitive")]
  
        public string Name_Oflegal_Representitive { get; set; }

        public string FormOfNotificationDescription { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Date_Created { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Date_Modified { get; set; }
        public string SelectDetentionCourt { get; set; }
        [Display(Name = "Court Name")]
        public int? Court_Id { get; set; }
        public string SelectedSAPStation { get; set; }
        [Display(Name = "Police Station")]
        public int? SDIISSAPS_Station_Id { get; set; }

        public string SelectProvinceDetails { get; set; }
        [Display(Name = "Province")]
        public int Province_Id { get; set; }
        [Display(Name = "Province")]
        public int Province_IdPolice { get; set; }
        public string SelectDistrictDetails { get; set; }
        [Display(Name = "District")]
        public int District_Id { get; set; }
        [Display(Name = "District")]
        public int District_IdPolice { get; set; }
        public virtual ICollection<SAPSLookup> SAPS_List { get; set; }
        public virtual ICollection<SAPS_OfficialLookup> SAPSOfficial_List { get; set; }
        public virtual ICollection<SAPSLookup> DetentionPlace_List { get; set; }
        public virtual ICollection<CourtLookup> Court_List { get; set; }

        //PCM_Assessment_Register
        public int Assesment_Register_Id { get; set; }
        [Display(Name = "Assessed By")]
        public int Assessed_By { get; set; }
        

        [Display(Name = "Assessment Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Assessment_Date { get; set; }
       
        [Display(Name = "Assessment Time")]
        public DateTime? Assessment_Time { get; set; }
        
        [Display(Name = "Form of Notification")]
        public int? Form_Of_Notification_Id { get; set; }
        
        public int Place_Of_Detention_Id { get; set; }
        [Display(Name = "Town")]
        public int? Town_Id { get; set; }
        public virtual ICollection<Place_Of_DetentionLookupPCM> Place_Of_Detention_List { get; set; }
        
        public virtual ICollection<TownTypeLookup> Town_Type { get; set; }
        public virtual ICollection<ProvinceLookupPCM> Province_List { get; set; }
        public virtual ICollection<DistrictLookupPCM> District_List{ get; set; }

        public virtual ICollection<NotificationTypeLookup> Form_Of_Notification_List { get; set; }

        public virtual ICollection<AdmissionVenueLookUpCYCA> VenueType_List { get; set; }

        #endregion

        #region OFFENSE
        public int PCM_Offence_Id { get; set; }
        [Display(Name = "Nature Of Offense")]
        public int? Offence_Category_Id { get; set; }

        [Display(Name = "Offence Type")]
        public int? Offence_Type_Id { get; set; }

        [Display(Name = "Offence Circumstance")]
        public string Offence_Circumstance { get; set; }
        [Display(Name = "Value Of Goods")]
        public string Value_Of_Goods { get; set; }
        [Display(Name = "Value Recovered ")]
        public string Value_Recovered { get; set;}
        [Display(Name = "IsChild Responsible")]
        public string IsChild_Responsible { get; set; }
        [Display(Name = "Responsibility Details")]
        public string Responsibility_Details { get; set; }

        public string selectOffenceCategory { get; set; } 
        public string selectOffeceType { get; set; }

        public virtual ICollection<PCMOffenceCategoryLookup> Offence_List { get; set; }
        public virtual ICollection<PCMOffenceTypeLookup> OffenceType_List { get; set; }

        [Display(Name ="Schedule")]

        public int? Offence_Schedule_Id { get; set; }

        public string selectOffenceSchedule { get; set; }

        public int Charge_Detail_Id { get; set; }

        [Display(Name = "Charge(s)")]
        public int?[] Charge_Id { get; set; }
        public string ChargeDescription { get; set; }
        public string Charge_Code { get; set; }
        public virtual ICollection<PCMChargeLookup> Charge_List { get; set; }

        public string WhenEscapedDescription { get; set; }
        public virtual ICollection<PCMWhenEscapedLookup> WhenEscaped_List { get; set; }

        public virtual ICollection<PCMOffenseSchedulesLookup> OffenseSchedules_List { get; set; }


        //public MultiSelectList Charges { get; set; }

        public List<SelectListItem> Charges { get; set; }
        public int Involvement_Id { get; set; }
        [Display(Name = "Previous Involved")]
        public string Previous_Involved { get; set; }
        [Display(Name = "IsArrest")]

        public string IsArrest { get; set; }
        [Display(Name = "Arrest Date")]
        public DateTime? PreviousArrest_Date { get; set; }
        [Display(Name = "Sentence Outcomes")]
        public string PreviousSentence_Outcomes { get; set; }
        [Display(Name = "IsConvicted")]
        public string IsConvicted { get; set; }
        [Display(Name = "Date")]
        public DateTime? previousConviction_Date { get; set; }
        [Display(Name = "IsEscape")]
        public string IsEscape { get; set; }
        [Display(Name = "Escapes Date")]
        public DateTime? Escapes_Date { get; set; }
        [Display(Name = "Time")]
        public DateTime? Escape_Time { get; set; }
        [Display(Name = "Place Of Escape")]
        public string Place_Of_Escape { get; set; }

        [Display(Name = "When Escaped")]
        public int? When_Escaped_Id { get; set; }

        public string When_Escaped { get; set; }





        #endregion

        #region VICTIM DETAILS

        public int Victim_Id { get; set; }

        [Display(Name = "Is Victim Individual")]
        public string IsVictimIndividual { get; set; }
        [Display(Name = "First Name")]
        public string Victim_First_Name { get; set; }
        [Display(Name = "Last Name")]
        public string Victim_Last_Name { get; set; }

        [Display(Name = "Gender")]
        public int Victim_Gender { get; set; }
        [Display(Name = "Occupation")]
        public string Victim_Occupation { get; set; }
        [Display(Name = "Age")]
        public int? Victim_Age { get; set; }
        [Display(Name = "Phone Number")]
        public string Victim_Phone_Number { get; set; }
        [Display(Name = "Care Giver Names")]
        public string Victim_Care_Giver_Names { get; set; }
        [Display(Name = "Address Line 1")]
        public string Victim_Address_Line_1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string Victim_Address_Line_2 { get; set; }
        [Display(Name = "Town")]
        public int? Victim_Town_Id { get; set; }
        [Display(Name = "Postal Code")]
        public string Victim_Postal_Code { get; set; }

        [Display(Name = "Town Description")]
        public string TownDescription { get; set; }

        public string Adressdecription { get; set; }

        public List<PCMCaseGridMain> Person_search_List { get; set; }
        public List<Client> Client_List { get; set; }

        public int Victim_Address_Id { get; set; }

        [Display(Name = "Organisation Name")]
        public string OrganisationName { get; set; }
        [Display(Name = "Contact Person First Name")]
        public string ContactPersonfirstname { get; set; }
        [Display(Name = "Contact Person Last Name")]
        public string ContactPersonlastname { get; set; }
        [Display(Name = "Organisation Tell")]
        public string OrganisationTell { get; set; }

        [Display(Name = "Intervention service/referrals")]
        public string Interventionservice_referrals { get; set; }

       
        [Display(Name = "Organisation_Id")]
        public int OrganisationVictim_Id { get; set; }

        public string Ditrictdesc { get; set; }
        public string Towndesc { get; set; }
        public string Provincedesc { get; set; }


        //SEARCH CONTROLL

        [Display(Name = "Enter Last Name")]
        public string Vi_Last_Name { get; set; }

        #endregion

        
        #region CO ACCUSED
        public int Co_Accused_Id { get; set; }
        
        public string HasAccussed { get; set; }
        [Display(Name = "Co Accused First Name")]
        public string Co_Accused_First_Name { get; set; }
        [Display(Name = "Co Accused Last Name")]
        public string Co_Accused_Last_Name { get; set; }
        [Display(Name = "Cubacc")]
        public string Cubacc { get; set; }
        [Display(Name = "Age")]

        public int Age { get; set; }

        #endregion

        #region DEVELOPMENT ASSESSMENT

        [Display(Name = "Development")]
        public int Development_Id { get; set; }

        [Display(Name = "Belonging")]
        public string Belonging { get; set; }

        [Display(Name = "Mastery")]
        public string Mastery { get; set; }

        [Display(Name = "Independence")]
        public string Independence { get; set; }

        [Display(Name = "Generosity")]
        public string Generosity { get; set; }

        [Display(Name = "Evaluation")]
        public string Evaluation { get; set; }
        #endregion

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

        #region RECOMMENDATION ORDERS
        [Display(Name = "Order(s)")]
        public int?[] Recomendation_Order_Id { get; set; }
        public int Order_Id { get; set; }

        [Display(Name = "Order(s)")]
        public string Description_Recomendation_Order { get; set; }

        public string RecomendationDescription { get; set; }

        [Display(Name = "Local Municipality")]
        public int? Local_Municipality_Id { get; set; }
        public int? selectLocalMunicipality { get; set; }

        #endregion

        #region  FACILITY BED SPACE

        public long Request_Id { get; set; }
        public int Sent_By { get; set; }

        [Display(Name = "Subject")]
        public string Request_Subject { get; set; }
        public DateTime Date_Recieved { get; set; }
        public DateTime Time_Recieved { get; set; }

        [Display(Name = "Comments")]
        public string Request_Comments { get; set; }

        [Display(Name = "Facility")]
        public int? Facility_Id { get; set; }
        [Display(Name = "Facility")]
        public string SelectFacility { get; set; }

        [Display(Name = "Status")]
        public int? Request_Status_Id { get; set; }

        public string Request_Status_Comments { get; set; }
        [Display(Name = "Admission Type")]
        public string selectAdmissionType { get; set; }
        [Display(Name = "Bed Request Status")]
        public string selectBedRequestStatus { get; set; }

        public virtual ICollection<PCMAdmissionTypeLookup> AdmissionType_List { get; set; }
        public virtual ICollection<PCMRequestStatusLookup> RequestStatus_List { get; set; }
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

        public string Male_Total_Space { get; set; }
        public string Male_Available_Space { get; set; }
        public string Male_Used_Space { get; set; }
        public string Female_Total_Space { get; set; }
        public string Female_Available_Space { get; set; }
        public string Female_Used_Space { get; set; }

        #endregion


        #region SOCIO ECONNOMY
        //PCM_Socia-Econonic
        public int Socio_Economy_id { get; set; }

        [Display(Name = "Family Background Comment")]
        public string Family_Background_Comment { get; set; }
        [Display(Name = "Finance Work_Record")]
        public string Finance_Work_Record { get; set; }
        [Display(Name = "Housing")]
        public string Housing { get; set; }
        [Display(Name = "Social Circumsances")]
        public string Social_Circumsances { get; set; }
        [Display(Name = "Previous Intervention")]
        public string Previous_Intervention { get; set; }
        [Display(Name = "Inter Personal Relationship")]
        public string Inter_Personal_Relationship { get; set; }
        [Display(Name = "Peer Presure")]
        public string Peer_Presure { get; set; }
        [Display(Name = "Substance Abuse")]
        public string Substance_Abuse { get; set; }
        [Display(Name = "Religious Involve")]
        public string Religious_Involve { get; set; }
        [Display(Name = "Child Behavior")]
        public string Child_Behavior { get; set; }
        [Display(Name = "Other Comments")]
        public string Other { get; set; }
        

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
        [Display(Name = "Assessment End Date")]
        public DateTime? Assessment_DateEnd { get; set; }
        [Display(Name = "Assessment End Time")]

        public DateTime? Assessment_TimeEnd { get; set; }

        #endregion

        public List<AdoptCaseGridMain> AssessmentRegisterGridList { get; set; }


        #region MEDICAL
        // PCM_Health_Chronic
        [Display(Name = "Injuries")]
        public string Injuries { get; set; }
        [Display(Name = "Health Status")]
        public int Health_Status_Id { get; set; }
        [Display(Name = "Medication")]
        public string Medication { get; set; }
        [Display(Name = "Allergy Description")]
        public string AllergyDescription { get; set; }
        public int Health_Details_Id { get; set; }
        [Display(Name = "Medical Appointments")]
        public DateTime? Medical_Appointments { get; set; }
        public virtual ICollection<PCMHealthStatusLookup> Health_Status_List { get; set; }
        public string HealthStatusDescription { get; set; }

        #endregion

        #region EDUCATION DETAILS
        //PCM_Educational_Infomation
        public int Person_Education_Id { get; set; }
        public string School_Name { get; set; }
        public string Contact_Person { get; set; }
        public string Telephone_Number { get; set; }
        public string Year_Completed { get; set; }
        public int? Grade_Completed_Id { get; set; }
        public DateTime? Date_Last_Attended { get; set; }
        public int School_Id { get; set; }
        public string Grade_Completed { get; set; }
        #endregion

        #region FAMAILY BACKGROUND 

        public int Family_information_Id { get; set; }
        [Display(Name = "Family Background")]
        public string Family_Background { get; set; }

        //Relationship Type........................................................
        [Display(Name = "Relationship Type")]
        public int? Relationship_Type_Id { get; set; }
        public virtual ICollection<RelationshipTypeLookupPcm> RelationshipType_List { get; set; }

        public virtual ICollection<PCMGenderLookup> Gender_List { get; set; }

        public virtual ICollection<HasLegalLookupPCM> HasLegal_List { get; set; }

        [Display(Name = "Has Legal Representitive")]

        public int? HasLegal_Id { get; set; }
        
        public string HasLegalDetails { get; set; }

   

        public int Family_Member_Id { get; set; }
    
        public string SelectRelationshipType { get; set; }
        [Display(Name = "Family Member Name")]

        public string Family_Member_Name { get; set; }
        [Display(Name = "Family Member Last Name")]
        public string Family_Member_Last_Name { get; set; }
        [Display(Name = "Family Member Age")]
        public int? Family_Member_Age { get; set; }
        #endregion

        public virtual ICollection<SourceReferralLookupPCM> SourceReferral_List { get; set; }
        public int Source_Referral_Id { get; set; }
    }


    #region  DROP DOWNS ASSESSMENT



    public class AdmissionVenueCYCA
    {
        public int? VenueDetails { get; set; }
        public IEnumerable<AdmissionVenueLookUpCYCA> Venue_List { get; set; }
    }

    public class AdmissionVenueLookUpCYCA
    {
        public int Venue_Id { get; set; }
        public string VenueName { get; set; }

    }

    public class HasLegalPCM
    {
        public int? HasLegalDetails { get; set; }
        public IEnumerable<HasLegalLookupPCM> HasLegal_List { get; set; }
    }
    public class HasLegalLookupPCM
    {
        public int HasLegal_Id { get; set; }
        public string Description { get; set; }



    } 



    public class TownPCM
    {
        public int? TownDetails { get; set; }
        public IEnumerable<TownLookupPCM> Town_List { get; set; }
    }
    public class TownLookupPCM
    {
        public int Town_Id { get; set; }
        public string Description { get; set; }



    }


    public class OrganisationTypePCM
    {
        public int? OrganisationTypeDetails { get; set; }
        public IEnumerable<OrganisationTypeLookupPCM> OrganisationType_List { get; set; }
    }
    public class OrganisationTypeLookupPCM
    {
        public int Organization_Type_Id { get; set; }
        public string Description { get; set; }



    }

    public class OffenseSchedulesPCM
    {
        public int? SelectOffenseSchedulesDetails { get; set; }
        public IEnumerable<PCMOffenseSchedulesLookup> OffenseSchedules_List { get; set; }
    }

    public class PCMOffenseSchedulesLookup
    {
        public int? Offence_Schedule_Id { get; set; }
        public string Description { get; set; }

    }

    public class GenderPCM
    {
        public int? SelectGenderDetails { get; set; }
        public IEnumerable<PCMGenderLookup> Gender_List { get; set; }
    }

    public class PCMGenderLookup
    {
        public int? Gender_Id { get; set; }
        public string Description { get; set; }

    }
    
    public class WhenEscapedPCM
    {
        public int? SelectWhenEscapedDetails { get; set; }
        public IEnumerable<PCMWhenEscapedLookup> WhenEscaped_List { get; set; }
    }

    public class PCMWhenEscapedLookup
    {
        public int? When_Escaped_Id { get; set; }
        public string Description { get; set; }

    }

    public class RequestStatusPCM
    {
        public int? SelectRequestStatusDetails { get; set; }
        public IEnumerable<PCMRequestStatusLookup> RequestStatus_List { get; set; }
    }

    public class PCMRequestStatusLookup
    {
        public int? Request_Status_Id { get; set; }
        public string Description { get; set; }

    }
    public class AdmissionTypePCM
    {
        public int? SelectAdmissionTypeDetails { get; set; }
        public IEnumerable<PCMAdmissionTypeLookup> AdmissionType_List { get; set; }
    }

   

    public class PCMAdmissionTypeLookup
    {
        public int? Admission_Type_Id { get; set; }
        public string Description { get; set; }

    }


    public class Place_Of_DetentionLookupPCM
    {
        public int? Place_Of_Detention_Id { get; set; }
        public string Description { get; set; }

    }

    public class Place_Of_DetentionPCM
    {
        public int? SelectPlace_Of_DetentionDetails { get; set; }
        public IEnumerable<Place_Of_DetentionLookupPCM> Place_Of_Detention_List { get; set; }
    }

    public class OffenceCategoryPCM
    {
        public int? SelectOffenceCategoryDetails { get; set; }
        public IEnumerable<PCMOffenceCategoryLookup> Offence_List { get; set; }
    }

    public class PCMOffenceCategoryLookup
    {
        public int? Offence_Category_Id { get; set; }
        public string Description { get; set; }

    }

    public class OffenceTypePCM
    {
        public int? SelectOffenceTypeDetails { get; set; }
        public IEnumerable<PCMOffenceTypeLookup> OffenceType_List { get; set; }
    }

    public class PCMOffenceTypeLookup
    {
        public int? Offence_Type_Id { get; set; }
        public string Description { get; set; }

    }
    
    public class HealthStatusPCM
    {
        public int? SelectHealthStatusDetails { get; set; }
        public IEnumerable<PCMHealthStatusLookup> Health_Status_List { get; set; }
    }

    public class PCMHealthStatusLookup
    {
        public int? Health_Status_Id { get; set; }
        public string Description { get; set; }

    }
    
    public class RelationshipTypePcm
    {
        public int? selectRelationshipType { get; set; }
        public IEnumerable<RelationshipTypeLookupPcm> RelationshipType_List { get; set; }
    }
    
    public class RelationshipTypeLookupPcm
    {
        public int Relationship_Type_Id { get; set; }
        public string Description { get; set; }

    }

    public class RecommendationTypePcm
    {
        public int? selectRecommendationType { get; set; }
        public IEnumerable<RecommendationTypeLookupPcm> RecommendationType_List { get; set; }
    }

    public class RecommendationTypeLookupPcm
    {
        public int Recommendation_Type_Id { get; set; }
        public string Description { get; set; }

    }

    public class PlacementTypePcm
    {
        public int? selectPlacementType{ get; set; }
        public IEnumerable<PlacementTypeLookupPcm> PlacementType_List { get; set; }
    }

    public class PlacementTypeLookupPcm
    {
        public int Placement_Type_Id { get; set; }
        public string Description { get; set; }

    }

    public class RecomendationOrderPcm
    {
        public int? selectRecomendationOrder { get; set; }
        public IEnumerable<RecomendationOrderLookupPcm> RecomendationOrder_List { get; set; }
    }

    public class RecomendationOrderLookupPcm
    {
        public int Recomendation_Order_Id { get; set; }
        public string Description { get; set; }

    }

    public class IdTypePcm
    {
        public int? selectIdType { get; set; }
        public IEnumerable<IdTypeLookupPcm> Organisation_Type_List { get; set; }
    }
    
    public class IdTypeLookupPcm
    {
        public int IdType { get; set; }
        public string Description { get; set; }

    }

    public class OrganizationPcm
    {
        public int? selectOrganization { get; set; }
        public IEnumerable<OrganizationLookupPcm> PCMOrganisation_List { get; set; }
    }

    public class OrganizationLookupPcm
    {
        public int Organization_Id { get; set; }
        public string Description { get; set; }

    }


    public class DiversionProgrammesPcm
    {
        public int? selectDiversionProgrammes { get; set; }
        public IEnumerable<DiversionProgrammesLookupPcm> DiversionProgrammes_List { get; set; }
    }

    public class DiversionProgrammesLookupPcm
    {
        public int Div_Program_Id { get; set; }
        public string Programme_Name { get; set; }

    }



    public class ChargePCM
    {
        public int? SelectChargeDetails { get; set; }
        public IEnumerable<PCMChargeLookup> Charge_List { get; set; }
    }

    public class PCMChargeLookup
    {
        public int? Charge_Id { get; set; }
        public string Charge_Description { get; set; }

    }


    #endregion

    #region DROP DOWNS PERSON

    public class SAPS
    {
        public int? SelectedSAPStation { get; set; }
        public IEnumerable<SAPSLookup> Saps_List { get; set; }
    }
    public class SAPSLookup
    {
        public int SAPS_Station_Id { get; set; }
        public string Description { get; set; }

    }
    public class SAPS_Official
    {
        public int? SelectSAPOfficialInfo { get; set; }
        public IEnumerable<SAPS_OfficialLookup> SapsOfficial_List { get; set; }
    }
    public class SAPS_OfficialLookup
    {
        public int SAPS_Info_Id { get; set; }
        public string Description { get; set; }

    }
    public class Court
    {
        public int? SelectCourtDetails { get; set; }
        public IEnumerable<CourtLookup> Court_List { get; set; }
    }
    public class CourtLookup
    {
        public int Court_id { get; set; }
        public string Description { get; set; }

    }



    /// <summary>
    /// Town dropdown list
    /// </summary>
    public class TownType
    {
        public string Description { get; internal set; }
        public int? selectedTown { get; set; }
        public IEnumerable<TownTypeLookup> Town_List { get; set; }
    }

    public class TownTypeLookup
    {
        public int Town_Id { get; set; }
        public string Description { get; set; }

    }

    public class NotificationType
    {
        public string Description { get; internal set; }
        public int? selectedNotification { get; set; }
        public IEnumerable<NotificationTypeLookup> Notification_List { get; set; }
    }

    public class NotificationTypeLookup
    {
        public int? Form_Of_Notification_Id { get; set; }
        public string Description { get; set; }
    }

    public class ChronicType
    {
        public string Description { get; internal set; }
        public int? selectedChronic { get; set; }
        public IEnumerable<ChronicTypeLookup> Chronic_List { get; set; }
    }

    public class ChronicTypeLookup
    {
        public int Chronic_Illness_Id { get; set; }
        public string Description { get; set; }

    }

  public class AllergyType
    {
        public string Description { get; internal set; }
        public int? selectedChronic { get; set; }
        public IEnumerable<AllergyTypeLookup> Allergy_List { get; set; }
    }

    public class AllergyTypeLookup
    {
        public int Allergy_Id { get; set; }
        public string Description { get; set; }
    }

    public class ProvinceLookupPCM
    {
        public int? Province_Id { get; set; }
        public string Description { get; set; }

    }

    public class ProvincePCM
    {
        public int? SelectProvinceDetails { get; set; }
        public IEnumerable<ProvinceLookupPCM> Province_List { get; set; }
    }

    public class DistrictLookupPCM
    {
        public int? District_Id { get; set; }
        public string Description { get; set; }

    }

    public class DistrictPCM
    {
        public int? SelectDistrictDetails { get; set; }
        public IEnumerable<DistrictLookupPCM> District_List { get; set; }
    }


    #endregion


   






}


