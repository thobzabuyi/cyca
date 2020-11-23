using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Common_Objects.ViewModels.RACAPCaseViewModel;

namespace Common_Objects.ViewModels
{


   public class RACAPCaseViewModel

    {
        public int? GetRACAPIntAssId(int id)
        {
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            return db.RACAP_Case_Details.Find(id).Intake_Assessment_Id;
        }
        //Expiry Date.......................................................
        [Display(Name = "Expiry Date")]
        public Nullable<DateTime> Expiry_Date { get; set; }

        //Date Registered

        [Display(Name = "Date Registered")]
        public DateTime? Date_Registered { get; set; }
        [Display(Name = "Date Captured")]
        public DateTime? Date_Captured { get; set; }


        // Comments about case
        [Display(Name = "Adoption Comments")]
        public string Comments { get; set; }
        //Amended by Herman
        public int Intake_Assessment_Id { get; set; }
        //Amended by Herman
        public int Client_Module_Id { get; set; }

        //Reason for child to be adopted........................................................
        [Display(Name = "Reason for Adoption")]
        public int? Adoptions_Reason_Id { get; set; }
        public string selectReason { get; set; }

        public bool Verified { get; set; }
        [Display(Name ="Date Verified")]
        public DateTime? DateVerified { get; set; }

        // New Code Yanga
        public int RACAP_Adoptive_Child_Id { get; set; }
        public int RACAP_Prospective_Parent_Id { get; set; }

        [Display(Name = "Special Needs")]
        public int? Special_Needs_Id { get; set; }
        public string selectSpecialNeeds { get; set; }

        public List<int> SelectedSpecialNeedsIds { get; set; }

        [Display(Name = "Skin Color")]
        public int? Skin_Colour_Id { get; set; }
        public string selectSkinColor { get; set; }
        //Amended by Herman
        //Dropdown_EyeColor_3
        //End Amended by Herman
        [Display(Name = "Eye Color")]
        public int? Eye_Color_Id { get; set; }

        //Amended by Herman
        //Dropdown_EyeColor_4
        //End Amended by Herman
        public string selectEyerColor { get; set; }
        //Amended by Herman
        [Display(Name = "Language")]
        public int? Language_Id { get; set; }
        public string selectParentLanguages { get; set; }

        [Display(Name = "Gender")]
        public int? Gender_Id { get; set; }
        public string selectParentGenders { get; set; }

        [Display(Name = "PopulationGroup")]
        public int? Population_Group_Id { get; set; }
        public string selectParentPopulationGroups { get; set; }

        [Display(Name = "Race")]
        public int? Race_Id { get; set; }
        public int? Race_Id_Option2 { get; set; }
        public string selectParentRaces { get; set; }

        [Display(Name = "Religion")]
        public int? Religion_Id { get; set; }
        public string selectParentReligions { get; set; }

        //End Amended by Herman
        [Display(Name = "Disability")]
        public int? Disability_Id { get; set; }

        public string selectDisability { get; set; }

        [Display(Name = "Body Structure")]
        public int? Body_Structure_Id { get; set; }
        public string selectEyerBodyStructure { get; set; }

      
        [Display(Name = "Ethnic Cultural Group")]
        public int? Ethnic_Cultural_Group_Id { get; set; }

        
        public string selectEthnicCulturalGroup { get; set; }


        //CHILD ORGNANISATION DETAILS

        public int Responsible_Organisation { get; set; }


        [Display(Name = "Organisation Type")]
        public int? Organisation_Type_Id { get; set; }
        public string selectOrganisationType { get; set; }

        //Removal
        [Display(Name = "Comments on removal")]
        public string Removal_Comments { get; set; }
        public DateTime? Date_Removed { get; set; }
        [Display(Name = "Reason for Removal")]
        public int? Removal_Type_Id { get; set; }




        //Primary Key

        public int? RACAP_Case_Id { get; set; }


        //Legitimacy........................................................
        [Display(Name = "Legitimacy")]
        public int? Legitimacy_Id { get; set; }
        public string selectLegitimacy { get; set; }


        //Cross Cultural........................................................
        [Display(Name = "Cross Cultural")]
        public int? Cross_Cultural_Id { get; set; }
        public string selectCrossCultural { get; set; }


        //Record Status........................................................
        [Display(Name = "Record Status")]
        public int? RACAP_Record_Status_Id { get; set; }
        public string selectRacapStatus { get; set; }



        //RACAP Registration Status ........................................................
        [Display(Name = "Registration Status")]
        public int? RACAP_Registration_Status_Id { get; set; }
        public string selectRACAPRegistrationStatus { get; set; }

        //Province........................................................
        [Display(Name = "Province for court")]
        public int? Province_Id { get; set; }
        public string SelectProvinceDetails { get; set; }

        [Display(Name = "District for court")]
        public int? District_Id { get; set; }
        public string SelectDistrictDetails { get; set; }

        [Display(Name = "Organisation Responsible for Child")]
        public int? Organization_Id { get; set; }

        [Display(Name = "Organisation Responsible for Parent")]
        public int? OrganizationP_Id { get; set; }


        //Court........................................................

        [Display(Name = "Court")]
        public int? Court_id { get; set; }
        public virtual Court selectCourt { get; set; }

        [Display(Name = "Telephone Number")]
        public int? Telephone_Number { get; set; }

        //Organisation Social Worker
        //Herman

        public int Social_Worker_Organisation { get; set; }
        [Display(Name = "Organisational Type of Social Worker")]
        public int? Organization_Id_Type_Social_Worker { get; set; }

        public string SelectOrganisationSocialWorkerDetails { get; set; }


        public int RACAP_Responsible_Organisation { get; set; }
        [Display(Name = "Organisational Type Responsible for Child")]
        public int? Organization_Id_Type_Child { get; set; }
        [Display(Name = "Organisational Type Responsible for Parent")]
        public int? Organization_Id_Type_Parent { get; set; }
        //Herman End
        [Display(Name = "Organisation Responsible for Child")]
        public int Organization_Id_Child { get; set; }
        public string SelectOrganisationchildDetails { get; set; }

        [Display(Name = "Organisation Responsible for Parent")]
        public int? Organization_Id_Parent { get; set; }

        [Display(Name = "Organisation of Social Worker")]
        public int? Organization_Id_SocialWorker { get; set; }

        public string SelectOrganisationParentDetails { get; set; }

        //Herman
        public int RACAP_Record_StatusID { get; set; }
        [Display(Name = "RACAP Record Status")]
        public string rACAP_Record_Status { get; set; }

        //.........................................................................................................................


       
        public int SelectKindOfDocId { get; set; }
        public int DocumentsCheck { get; set; }
        public string FileDocumentDetails { get; set; }

        public int? Age_From { get; set; }
        public int? Age_To { get; set; }


        //Record Status........................................................
        [Display(Name = "Status")]
        public int? Record_Status_Id { get; set; }
        public string selectStatus { get; set; }
        
        //Reason For Adopting........................................................
        [Display(Name = "Parent Reason for Adopting")]
        public int? RACAP_Adopt_Reason_Id { get; set; }
        public string selectAdopting { get; set; }


        #region RACAP CHILD NEW CODE

        // New Code DropdownList
        //Amended by Herman
        //Dropdown_EyeColor_1
        //End Amended by Herman
        public virtual IEnumerable<EyeColorsLookupAdopt> EyeColors_Type_List { get; set; }
        //Amended by Herman
        public virtual ICollection<ParentDisabilityTypeLookupRACAP> ParentDisability_List { get; set; }
        public virtual ICollection<ParentLanguageTypeLookupRACAP> ParentLanguage_List { get; set; }


        public virtual ICollection<ParentGenderTypeLookupRACAP> ParentGender_List { get; set; }
        public virtual ICollection<ParentPopulationGroupTypeLookupRACAP> ParentPopulationGroup_List { get; set; }
        public virtual ICollection<ParentRaceTypeLookupRACAP> ParentRace_List { get; set; }
        public virtual ICollection<ParentReligionTypeLookupRACAP> ParentReligion_List { get; set; }
        //End Amended by Herman
        public virtual IEnumerable<BodyStructureLookupAdopt> BodyStructure_Type_List { get; set; }
        public virtual IEnumerable<SpecialNeedsLookupAdopt> SpecialNeeds_Type_List { get; set; }
        public virtual IEnumerable<SkinColourLookupAdopt> SkinColour_Type_List { get; set; }
        
        public virtual IEnumerable<EthnicCulturalLookupAdopt> EthnicCultural_Type_List { get; set; }

        public virtual IEnumerable<RACAPRegistrationStatusLookupAdopt> RACAPRegistrationStatus_Type_List { get; set; }
        public virtual IEnumerable<RACAPRecordStatusLookupAdopt> RACAPRecordStatus_Type_List { get; set; }
        public virtual ICollection<RaceTypeLookupRACAP> Race_Type { get; set; }
        public virtual IEnumerable<OrganisationTypeLookupRACAPChild> ChildOrganisation_Type_List { get; set; }
        public virtual IEnumerable<OrganisationTypeLookupSocialWorkerRACAP> Social_WorkerOrganisation_Type_List { get; set; }
        public IEnumerable<OrganisationLookupSocialWorkerRACAP> SocialWorkerOrganisation_List { get; set; }
        public IEnumerable<ProvinceLookupRACAP> Province_List { get; set; }
        public virtual ICollection<RemovalTypeLookupRACAP> Removal_Type_List { get; set; }
        public virtual ICollection<CorrespondenceTypeLookupRACAP> Correspondence_Type_List { get; set; }
        public virtual ICollection<ReasonForAdoptionLookupRACAP> Reason_Type_List { get; set; }
        public virtual IEnumerable<OrganisationChildLookupRACAP> OrganisationChild_List { get; set; }

        // End



        #endregion

        #region  RACAP CHILD FEATURES
        // Yanga New Code RACAP DropDowns
        //Amended by Herman
        //Dropdown_EyeColor_2
        //End Amended by Herman
        public class EyeColor
        {
            public int? selectEyeColors { get; set; }
            public IEnumerable<EyeColorsLookupAdopt> Eye_Colors_List { get; set; }
        }


        public class EyeColorsLookupAdopt
        {
            public int? Eye_Color_Id { get; set; }
            public string Description { get; set; }

        }
        //Amended by Herman
        //Language

        public class ParentLanguage
        {
            public int? selectParentLanguages { get; set; }
            public IEnumerable<ParentLanguagesLookupAdopt> ParentLanguages_List { get; set; }
        }


        public class ParentLanguagesLookupAdopt
        {
            public int? Language_Id { get; set; }
            public string Description { get; set; }

        }
        //Gender

        public class ParentGender
        {
            public int? selectParentGenders { get; set; }
            public IEnumerable<ParentGendersLookupAdopt> ParentGenders_List { get; set; }
        }


        public class ParentGendersLookupAdopt
        {
            public int? Gender_Id { get; set; }
            public string Description { get; set; }

        }

        //PopulationGroup
        public class ParentParentPopulationGroup
        {
            public int? selectParentPopulationGroups { get; set; }
            public IEnumerable<ParentPopulationGroupsLookupAdopt> ParentPopulationGroups_List { get; set; }
        }


        public class ParentPopulationGroupsLookupAdopt
        {
            public int? Population_Group_Id { get; set; }
            public string Description { get; set; }

        }

        //Race

        public class ParentRace
        {
            public int? selectParentRaces { get; set; }
            public IEnumerable<ParentRacesLookupAdopt> ParentRaces_List { get; set; }
        }


        public class ParentRacesLookupAdopt
        {
            public int? Race_Id { get; set; }
            public string Description { get; set; }

        }

        //Religion
        public class ParentReligion
        {
            public int? selectParentReligions { get; set; }
            public IEnumerable<ParentReligionsLookupAdopt> ParentReligions_List { get; set; }
        }


        public class ParentReligionsLookupAdopt
        {
            public int? Religion_Id { get; set; }
            public string Description { get; set; }

        }
        public class ParentDisabilityTypeRACAP
        {
            public int? selectedParentDisability { get; set; }
            public IEnumerable<ParentDisabilityTypeLookupRACAP> ParentDisability_Type_List { get; set; }
        }

        public class ParentDisabilityTypeLookupRACAP
        {
            public int Disability_Id { get; set; }
            public string Description { get; set; }

        }

        //Language

        public class ParentLanguageTypeRACAP
        {
            public int? selectedLanguageDisability { get; set; }
            public IEnumerable<ParentLanguageTypeLookupRACAP> ParentLanguage_Type_List { get; set; }
        }

        public class ParentLanguageTypeLookupRACAP
        {
            public int Language_Id { get; set; }
            public string Description { get; set; }

        }


        //Gender

        public class ParentGenderTypeRACAP
        {
            public int? selectedGenderDisability { get; set; }
            public IEnumerable<ParentGenderTypeLookupRACAP> ParentGender_Type_List { get; set; }
        }

        public class ParentGenderTypeLookupRACAP
        {
            public int Gender_Id { get; set; }
            public string Description { get; set; }

        }

        //PopulationGroup
        public class ParentPopulationGroupTypeRACAP
        {
            public int? selectedPopulationGroupDisability { get; set; }
            public IEnumerable<ParentPopulationGroupTypeLookupRACAP> ParentPopulationGroup_Type_List { get; set; }
        }

        public class ParentPopulationGroupTypeLookupRACAP
        {
            public int Population_Group_Id { get; set; }
            public string Description { get; set; }

        }

        //Race
        public class ParentRaceTypeRACAP
        {
            public int? selectedRaceDisability { get; set; }
            public IEnumerable<ParentRaceTypeLookupRACAP> ParentRace_Type_List { get; set; }
        }

        public class ParentRaceTypeLookupRACAP
        {
            public int Race_Id { get; set; }
            public string Description { get; set; }

        }

        //Religion
        public class ParentReligionTypeRACAP
        {
            public int? selectedReligionDisability { get; set; }
            public IEnumerable<ParentReligionTypeLookupRACAP> ParentReligion_Type_List { get; set; }
        }

        public class ParentReligionTypeLookupRACAP
        {
            public int Religion_Id { get; set; }
            public string Description { get; set; }

        }
        //End Amended by Herman
        public class BodyStructure
        {
            public int? selectBodyStructure { get; set; }
            public IEnumerable<BodyStructureLookupAdopt> BodyStructure_List { get; set; }
        }


        public class BodyStructureLookupAdopt
        {
            public int? Body_Structure_Id { get; set; }
            public string Description { get; set; }

        }

        public class SpecialNeeds
        {
            public int? selectSpecialNeeds { get; set; }
            public IEnumerable<SpecialNeedsLookupAdopt> SpecialNeeds_List { get; set; }
        }


        public class SpecialNeedsLookupAdopt
        {
            public int? Special_Needs_Id { get; set; }
            public string Description { get; set; }

        }

        public class SkinColour
        {
            public int? selectSkinColour { get; set; }
            public IEnumerable<SkinColourLookupAdopt> SkinColour_List { get; set; }
        }


        public class SkinColourLookupAdopt
        {
            public int? Skin_Colour_Id { get; set; }
            public string Description { get; set; }

        }
       
        public class EthnicCultural
        {
            public int? selectEthnicCultural { get; set; }
            public IEnumerable<EthnicCulturalLookupAdopt> EthnicCultural_List { get; set; }
        }

       
        public class EthnicCulturalLookupAdopt
        {
            public int? Ethnic_Cultural_Group_Id { get; set; }
            public string Description { get; set; }
        }



        public class RACAPRegistrationStatus
        {
            public int? selectRACAPRegistrationStatus { get; set; }
            public IEnumerable<RACAPRegistrationStatusLookupAdopt> RACAPRegistrationStatus_List { get; set; }
        }
        public class RACAPRegistrationStatusLookupAdopt
        {
            public int? RACAP_Registration_Status_Id { get; set; }
            public string Description { get; set; }

        }

        public class RACAPRecordStatus
        {
            public int? selectRACAPRecordStatus { get; set; }
            public IEnumerable<RACAPRecordStatusLookupAdopt> RACAPRecordStatus_List { get; set; }
        }
        public class RACAPRecordStatusLookupAdopt
        {
            public int? RACAP_Record_Status_Id { get; set; }
            public string Description { get; set; }

        }


        public class RemovalType
        {
            public int? selectRemovalType { get; set; }
            public IEnumerable<RemovalTypeLookupRACAP> RemovalType_List { get; set; }
        }


        public class RemovalTypeLookupRACAP
        {
            public int? Removal_Type_Id { get; set; }
            public string Description { get; set; }

        }


        public class CorrespondenceType
        {
            public int? selectCorrespondenceType { get; set; }
            public IEnumerable<CorrespondenceTypeLookupRACAP> CorrespondenceType_List { get; set; }
        }


        public class CorrespondenceTypeLookupRACAP
        {
            public int? RACAP_Correspondence_Type_Id { get; set; }
            public string Description { get; set; }

        }


        #endregion



        public virtual IEnumerable<RegistrationStatusLookupRACAP> GetAllRegistrationStatusType { get; set; }
        //public virtual IEnumerable<LegitimacyLookupRACAP> Legitimacy_Type_List { get; set; }
        public virtual IEnumerable<Cross_CulturalLookupRACAP> CrossCultural_Type_List { get; set; }
        public virtual IEnumerable<Record_StatusLookupRACAP> Status_Type_List { get; set; }
        public virtual IEnumerable<RegistrationStatusLookupRACAP> Registrations_Status_List { get; set; }

        public virtual IEnumerable<DateTime> Registrations_Date { get; set; }
        public virtual DateTime Expiring_Date { get; set; }
        public virtual IEnumerable<RegistrationStatusLookupRACAP> Expiry_Date_List { get; set; }

        public IEnumerable<OrganisationLookupRACAPParent> ParentOrganisation_List { get; set; }
        public IEnumerable<SocialWorkerLookupRACAPParent> SocialWorker_List { get; set; }
        public IEnumerable<DistrictLookupRACAP> District_List { get; set; }
    

        public IEnumerable<OrganisationTypeLookupRACAPParent> ParentdOrganisation_Type_List { get; set; }

        public int RACAP_Correspondence_Id { get; set; }

        public int? RACAP_Correspondence_Type_Id { get; set; }
        public string RACAP_Correspondenc_Type { get; set; }

        public DateTime? RACAP_Correspondence_Date_Created { get; set; }

        public int? RACAP_Correspondence_Created_By { get; set; }

        public DateTime? RACAP_Correspondence_Date_Modified { get; set; }

        public int? RACAP_Correspondence_Modified_By { get; set; }

        public string RACAP_Correspondence_Comments { get; set; }

        public string RACAP_Correspondence_FileName { get; set; }
        public string RACAP_Correspondence_FilePath { get; set; }

        public int SelectCorrespondenceId { get; set; }
        public SelectList CorrespondenceList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var Branches = (from a in _db.apl_RACAP_Correspondence_Type
                                select new { a.Description, a.RACAP_Correspondence_Type_Id, NewCheckbox = false }).ToList();
                var BranchList = (from b in Branches
                                  select new SelectListItem()
                                  {
                                      Text = b.Description,
                                      Value = Convert.ToString(b.RACAP_Correspondence_Type_Id),
                                      Disabled = b.NewCheckbox,
                                      Selected = b.RACAP_Correspondence_Type_Id.Equals(SelectCorrespondenceId)
                                  }).ToList();
                var selectList = new SelectList(BranchList, "Value", "Text", SelectCorrespondenceId);
                return selectList;
            }
        }
        //Hermand end

    }



    public class RegistrationStatusLookupRACAP
    {
        public int? RACAP_Registration_Status_Id { get; set; }
        public string Description { get; set; }

    }

    public class RecordStatusLookupRACAP
    {
        public int? RACAP_Record_Status_Id { get; set; }
        public string Description { get; set; }

    }
    public class Cross_CulturalLookupRACAP
    {
        public int Cross_Cultural_Id { get; set; }
        public string Description { get; set; }

    }
    
    public class Record_StatusLookupRACAP
    {
        public int Record_Status_Id { get; set; }
        public string Description { get; set; }

    }
    public class ProvinceLookupRACAP
    {
        public int? Province_Id { get; set; }
        public string Description { get; set; }

    }

    public class ProvinceRACAP
    {
        public int? SelectProvinceDetails { get; set; }
        public IEnumerable<ProvinceLookupRACAP> Province_List { get; set; }
    }

    public class DistrictLookupRACAP
    {
        public int? District_Id { get; set; }
        public string Description { get; set; }

    }

    public class DistrictRACAP
    {
        public int? SelectDistrictDetails { get; set; }
        public IEnumerable<DistrictLookupRACAP> District_List { get; set; }
    }

    public class CourtLookupRACAP
    {
        public int? Court_id { get; set; }
        public string Description { get; set; }

    }

    public class RACAPRecordStatusLookupRACAP
    {
        public int? RACAPRecordStatusId{ get; set; }
        public string Description { get; set; }

    }

    public class RACAPRegistrationStatusLookupRACAP
    {
        public int? RegistrationStatusId { get; set; }
        public string Description { get; set; }

    }

    public class ReasonForAdoptionLookupRACAP
    {
        public int? Adoptions_Reason_Id { get; set; }
        public string Description { get; set; }

    }

    public class OrganisationChildRACAP
    {
        public int? SelectOrganisationChildDetails { get; set; }
        public IEnumerable<OrganisationChildLookupRACAP> ChildOrganisation_List { get; set; }
    }
    public class OrganisationChildLookupRACAP
    {
        public int Organization_Id { get; set; }
        public string Description { get; set; }

        public string Telephone_Number { get; set; }
        public string Email_Address { get; set; }



    }
    
    public class OrganisationLookupSocialWorkerRACAP
    {
        public int Organization_Id { get; set; }
        public string Description { get; set; }


        public string Telephone_Number { get; set; }
        public string Email_Address { get; set; }

    }

    public class OrganisationParentRACAP
    {
        public int? SelectOrganisationParentDetails { get; set; }
        public IEnumerable<OrganisationLookupRACAPParent> ParentOrganisation_List { get; set; }
    }
    public class OrganisationLookupRACAPParent
    {
        public int Organization_Id { get; set; }
        public string Description { get; set; }

        public string Telephone_Number { get; set; }
        public string Email_Address { get; set; }
    }

    public class SocialWorkerParentRACAP
    {
        public int? SelectSocialWorkerParentDetails { get; set; }
        public IEnumerable<SocialWorkerLookupRACAPParent> ParentSocialWorker_List { get; set; }
    }
    public class SocialWorkerLookupRACAPParent
    {
        public int SocialWorkerId { get; set; }
        public string Description { get; set; }

        public string Telephone_Number { get; set; }
        public string Email_Address { get; set; }

        public string Social_Worker_Name { get; set; }

        public string Social_Worker_Surname { get; set; }

        public string Accreditation_Ref { get; set; }

        public DateTime? Accredited_Date { get; set; }
        public string SocWorkFax { get; set; }

        public string Province { get; set; }
    }


    //Merge Models


    public class MainPageModelRACAPVM
    {



        public RACAPCaseViewModel RACAPCaseViewModel { get; set; }
        public RACAPPersonViewModel RACAPPersonViewModel { get; set; }
        public List<RACAPCaseGridMain> Clients_Case_List { get; set; }

        public List<int> SelectedSpecialNeedsIds { get; set; }

        public RACAPOrgResponsibleVM RACAPOrgResponsibleVM { get; set; }

    }

    public class RACAPCaseMatchesViewModel
    {
        #region NewEntries_ToView_Parent_Details

            public int? intakeAssIdFPDV { get; set; }
            public int? intakeAssIdFCDV { get; set; }

        
            public List<RACAP_Supporting_Document> DocumentsList { get; set; }
            public int int_Person_Parent_1_Id { get; set; }
            public int int_Person_Child_Id { get; set; }
            public int int_Person_Parent_2_Id { get; set; }
            public int RACAP_CaseId { get; set; }
            public int int_Assesment_Id { get; set; }

            public int RACAP_WorkListId { get; set; }
            public int Record_Id { get; set; }
            public int Registration_Id { get; set; }
            public string RegistrationStatus { get; set; }
            [Display(Name ="Record Status")]
            public string RecordStatus { get; set; }
            [Display(Name ="Ref Number")]
            public string RACAP_Ref_Number { get; set; }
            //        Parent:
            //First Parent Captured on Intake
            [Display(Name = "Persons Name")]
            public string First_Name_P1 { get; set; }

        public string Last_Name_P1 { get; set; }
        public string Last_Name_Child { get; set; }
        [Display(Name = "Known As")]

        public string Known_As_P1 { get; set; }
        public string Known_As_Child { get; set; }
        public string Identity_Type_P1 { get; set; }
            [Display(Name = "Identity number")]

        public string Identity_Number_P1 { get; set; }
        [Display(Name = "Identity number")]

        public string Identity_Number_Child { get; set; }
        [Display(Name = "Date of Birth")]

            public DateTime? Date_Of_Birth_P1 { get; set; }
            [Display(Name = "Age")]

            public string Age_P1 { get; set; }
        [Display(Name = "Age")]

        public string Age_Child { get; set; }
        [Display(Name = "Estimated Age")]

            public int? Estimated_Age_P1 { get; set; }
            //Second Parent Captured on Intake
            [Display(Name = "Persons Name")]

            public string First_Name_P2 { get; set; }
            public string Last_Name_P2 { get; set; }
            [Display(Name = "Known As")]

            public string Known_As_P2 { get; set; }
            public string Identity_Type_P2 { get; set; }
            [Display(Name = "Identity number")]

            public string Identity_Number_P2 { get; set; }
            [Display(Name = "Date of Birth")]

            public DateTime? Date_Of_Birth_P2 { get; set; }
            [Display(Name = "Estimated Age")]

            public string Age_P2 { get; set; }
            public int? Estimated_Age_P2 { get; set; }


            //    Features
            [Display(Name ="Language")]
            public int? Language { get; set; }
            [Display(Name = "Language")]

            public string Language_S { get; set; }
            public int? Religion_Id { get; set; }
            [Display(Name ="Religion")]
            public string Religion_S { get; set; }
            [Display(Name = "Religion")]

            public string Religion_S_2 { get; set; }

            public int? MaritalStatus_Id { get; set; }
            [Display(Name = "MaritalStatus")]
            public string MaritalStatus_S { get; set; }
        public int? Nationality_Id { get; set; }
        [Display(Name = "Nationality")]
        public string Nationality_S { get; set; }
        [Display(Name = "Mobile_Number")]
        public string Mobile_Number_1 { get; set; }
        [Display(Name ="Age From")]
            public int? Age_From { get; set; }
            [Display(Name ="Age To")]
            public int? Age_To { get; set; }
            public int? Skin_Color { get; set; }
            [Display(Name = "Skin Color")]

            public string Skin_Color_S { get; set; }
            public List<int> Disability { get; set; }
            public int? Body_Structure { get; set; }
            [Display(Name = "Body Structure")]

            public string Body_Structure_S { get; set; }
            public int? Ethnic_CulturalGroup { get; set; }
            [Display(Name = "Ethnic CulturalGroup")]

            public string Ethnic_CulturalGroup_S { get; set; }
            public List<int> Special_Needs { get; set; }
            public int? Eye_Color { get; set; }
            [Display(Name ="Eye Color")]
            public string Eye_Color_S { get; set; }
            public int? Race { get; set; }
            [Display(Name = "Race")]

            public string Race_S { get; set; }
            [Display(Name = "Race")]

            public string Race_S_2 { get; set; }
            [Display(Name = "Gender")]

            public string GenderS { get; set; }
            public string GenderS_2 { get; set; }
        public string MaritalStatus_S_2 { get; set; }

        public string Nationality_S_2 { get; set; }
        public string Email_2 { get; set; }
        public string Mobile_Number_2 { get; set; }

        //    Org:
        public int? Organisation_Responsible_For_Child { get; set; }
            public string Organisation_Responsible_For_Child_S { get; set; }
            public int? Social_Worker_Id { get; set; }
            [Display(Name ="Social Worker")]
            public string Social_Worker { get; set; }
            public string Accreditation_Ref { get; set; }
            public DateTime? Accreditation_Date { get; set; }
            public string Email { get; set; }
            [Display(Name ="Tel")]
            public string Telephone { get; set; }
            [Display(Name ="In")]
            public string Province { get; set; }
            public int? OrganisationType { get; set; }
            public string OrganisationType_S { get; set; }



            //    Supporting Doc:
            public int? SupDocId { get; set; }
            public string SupDocDescription { get; set; }
        #endregion

        public string Search_Intake_Ref_No { get; set; }
        public string Search_PCM_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Identification_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Client> Client_List { get; set; }
        public int Selected_Client_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Client_Id { get; set; }
        public List<RACAPCaseGridMain> Clients_Case_List { get; set; }
    }

    #region UPLOAD RACAP SUPPORTING DOCUMENTS

    public class FileDetail
    {
        public Guid RACAP_File_Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        [Required(ErrorMessage = "Please Enter Document Name")]
        [Display(Name = "Name")]
        [MaxLength(200)]
        public int Document_Description { get; set; }
        public int RACAP_Case_Id { get; set; }

    }


    #endregion

    //Herman


    public class RACAPSupportingDocumentsVM
    {
        public int DocumentsId { get; set; }
        public int? DocumentsLabel { get; set; }

        public string DocLabel { get; set; }

        public string DocumentsType { get; set; }
        [Required]
        [Display(Name = "Documents file name:")]
        public string DocumentsFileName { get; set; }
        public string DocumentsPath { get; set; }
        [Required]
        [Display(Name = "Documents attachment description:")]
        public string DocumentsDescription { get; set; }

        public int? Intake_Assessment_Id { get; set; }

        public int? DocumentsCheck { get; set; }
        public string KindOfDocument { get; set; }
        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [DataType(DataType.ImageUrl)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public List<RACAPSupportingDocumentsVM> DocumentsVMs { get; set; }
        public virtual ICollection<RACAPSupportingDocumentsVM> FileDocumentDetails { get; set; }

        public int SelectKindOfDocId { get; set; }
        public SelectList DocumentsList
        {
            get
            {

                var _db = new SDIIS_DatabaseEntities();
                var AdoptDoc = (from a in _db.apl_ADOPTKindOfDocument
                                         select new { a.Description, a.DocumentsId }).ToList();
                var docList = (from b in AdoptDoc
                                  select new SelectListItem()
                                  {
                                      Text = b.Description,
                                      Value = Convert.ToString(b.DocumentsId),
                                      Selected = b.DocumentsId.Equals(SelectKindOfDocId)
                                  }).ToList();
                var selectDocList = new SelectList(docList, "Value", "Text", SelectKindOfDocId);
                return selectDocList;
            }
        }
    }

    public class OrganisationTypeChildRACAP
    {
        public int? SelectOrganisationTypechildDetails { get; set; }
        public IEnumerable<OrganisationTypeLookupRACAPChild> OrganisationType_List { get; set; }
    }
    public class OrganisationTypeLookupRACAPChild
    {
        public int? IdType { get; set; }
        public string Description { get; set; }



    }

    public class OrganisationTypeLookupSocialWorkerRACAP
    {
        public int Organization_Type_Id { get; set; }
        public string Description { get; set; }



    }
    
    public class OrganisationTypeLookupRACAPParent
    {
        public int Organization_Type_Id { get; set; }
        public string Description { get; set; }

    }

    //Herman End



}


