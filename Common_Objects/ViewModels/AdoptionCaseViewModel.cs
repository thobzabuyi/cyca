using Common_Objects.Models;
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
    public class AdoptionCaseViewModel
    {

        //Court Order Date.......................................................
        [Required]
        [Display(Name = "Adoption Order Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> OdoptionOrder_Date { get; set; }

        //Date Registered


        [Display(Name = "Date Finalised in DSD")]
        public DateTime? Date_Registered { get; set; }

        // Adoption Surname 
        [Display(Name = "Adoption Surname")]
        public string Adoption_Surname { get; set; }

        // Comments about case
        [Display(Name = "Adoption Comments")]
        public string Comments { get; set; }

        public int? Intake_Assessment_Id { get; set; }
        [Required]
        [Display(Name = "Court Reference")]
        public string CourtReffence { get; set; }

        //Reason for child to be adopted........................................................
        [Display(Name = "Reason for Adoption")]
        public int? Adoptions_Reason_Id { get; set; }
        [Required]
        public string selectReason { get; set; }


        //Primary Key

        public int? Adopt_Case_Id { get; set; }
        [Display(Name = "Correspondence Type")]
        public int? Correspondence_Type_Id { get; set; }
        [Display(Name = "Correspondence Comments")]
        public string Adopt_Correspondence_Comments { get; set; }


        //Legitimacy........................................................
        [Display(Name = "Legitimacy")]
        public int? Legitimacy_Id { get; set; }
        public string selectLegitimacy { get; set; }


        //Cross Cultural........................................................
        [Display(Name = "Adoption Sub-type")]
        public int? Cross_Cultural_Id { get; set; }
        [Required]

        public string selectCrossCultural { get; set; }

        public int Age { get; set; }
        //Record Status........................................................
        [Display(Name = "Status")]
        public int? Record_Status_Id { get; set; }
        [Required]

        public string selectStatus { get; set; }

        public int? Client_Module_Id { get; set; }
        public string Client_Module_Ref_No { get; set; }
        //Reason For Adopting........................................................
        [Display(Name = "Parent Reason for Adopting")]
        public int? Adopting_Reason_Id { get; set; }
        [Required]

        public string selectAdopting { get; set; }

        //Relationship Type........................................................
        [Display(Name = "Relationship")]
        public int? Relationship_Type_Id { get; set; }
        [Required]

        public string selectRelationshipType { get; set; }

        //Adoption Type........................................................
        [Display(Name = "Adoption Type")]
        public int? Adopt_Type_Id { get; set; }
        public string selectAdoptionType { get; set; }

        //Adoption Type........................................................
        [Display(Name = "Is child related?")]
        public int? NonRelation_Realtion_Id { get; set; }
        public string selectRelationStatus { get; set; }

        //Adoption Category..................................................................
        [Display(Name = "Adoption Category")]
        public int? Adoption_Category_Id { get; set; }

        public string selectAdoptionCategory { get; set; }

        //Province........................................................
        [Display(Name = "Province for court")]
        public int? Province_Id { get; set; }
        [Required]

        [Display(Name = "Province for organisation")]
        public int? Province_IdChildOrg { get; set; }
        [Required]

        [Display(Name = "Province for organisation")]
        public int? Province_IdParentOrg { get; set; }
        [Required]

        public string SelectProvinceDetails { get; set; }

        [Display(Name = "District for court")]
        public int? District_Id { get; set; }
        [Required]

        [Display(Name = "District")]
        public int? District_IdChilOrg { get; set; }
        [Required]

        [Display(Name = "District")]
        public int? District_IdParentOrg { get; set; }
        [Required]

        public string SelectDistrictDetails { get; set; }

        [Display(Name = "Date Registration Finalised")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date_Finalised { get; set; }

        //Local Municipality............................................
        //Province........................................................
        [Display(Name = "Local Municipality")]
        public int? Local_Municipality_Id { get; set; }
        [Required]

        [Display(Name = "Local Municipality")]
        public int? Local_Municipality_IdParent { get; set; }
        //Court........................................................

        [Display(Name = "Court")]
        public int? Court_id { get; set; }
        [Required]

        public virtual Court selectCourt { get; set; }

        [Display(Name = "Telephone Number")]
        public int? Telephone_Number { get; set; }

        [Display(Name = "Correspondence Type")]

        //Organisation Social Worker
        //Herman
        [Required]

        public int Social_Worker_Organisation { get; set; }
        [Display(Name = "Organisational Type of Social Worker")]
        public int? Organization_Id_Type_Social_Worker { get; set; }
        [Required]

        public string SelectOrganisationSocialWorkerDetails { get; set; }

        [Display(Name = "Social Worker")]
        public int? Child_Social_Worker_Id { get; set; }
        public IEnumerable<ChildSocialWorkerLookupAdopt> ChildSocialWorker_List { get; set; }
        public string Child_Social_Worker { get; set; }


        [Display(Name = "Social Worker")]
        public int? Parent_Social_Worker_Id { get; set; }
        public string Parent_Social_Worker { get; set; }
        public IEnumerable<ParentSocialWorkerLookupAdopt> ParentSocialWorker_List { get; set; }



        //Herman End

        //Organisation Childs
        //Herman

        public int Adopt_Responsible_Organisation { get; set; }
        [Display(Name = "Organisational Type Responsible")]
        public int? Organization_Id_Type_Child { get; set; }
        [Display(Name = "Organisational Type Responsible")]
        public int? Organization_Id_Type_Parent { get; set; }
        //Herman End
        [Display(Name = "Organisation Responsible")]
        public int? Organization_Id_Child { get; set; }
        [Required]

        public string SelectOrganisationchildDetails { get; set; }

        [Display(Name = "Organisation Responsible")]
        public int? Organization_Id_Parent { get; set; }

        [Display(Name = "Organisation of Social Worker")]
        public int? Organization_Id_SocialWorker { get; set; }
        [Required]

        public string SelectOrganisationParentDetails { get; set; }

        public int Adopt_Correspondence_Id { get; set; }
        public string CorrespondenceDescription { get; set; }
        public int? Adopt_Correspondence_Created_By { get; set; }
        public DateTime? Adopt_Correspondence_Date_Created { get; set; }
        public string PersonCreated { get; set; }

        public string Adopt_Correspondence_FileName { get; set; }
        public string Adopt_Correspondence_FilePath { get; set; }

        public List<AdoptionCaseViewModel> Correspondencelist { get; set; }
        public virtual ICollection<CorrespondenceTypeLookupAdopt> CorrespondenceType_List { get; set; }
        public virtual ICollection<ReasonForAdoptionLookupAdopt> Reason_Type_List { get; set; }
        public virtual ICollection<RelationshipTypeLookupAdopt> RelationshipType_List { get; set; }
        public virtual ICollection<AdoptionTypeLookupAdopt> AdoptionType_List { get; set; }
        public virtual ICollection<AdoptionRelationNonRelationLookupAdopt> RelationStatus_List { get; set; }
        public virtual IEnumerable<LegitimacyLookupAdopt> Legitimacy_Type_List { get; set; }
        public virtual IEnumerable<Cross_CulturalLookupAdopt> CrossCultural_Type_List { get; set; }
        public virtual IEnumerable<Record_StatusLookupAdopt> Status_Type_List { get; set; }
        public virtual IEnumerable<ReasonForAdoptingLookupAdopt> Adopting_Type_List { get; set; }
        public virtual IEnumerable<CourtLookupAdopt> Court_List { get; set; }
        public virtual IEnumerable<OrganisationLookupAdoptChild> ChildOrganisation_List { get; set; }

        public IEnumerable<OrganisationLookupAdoptParent> ParentOrganisation_List { get; set; }
        public IEnumerable<OrganisationLookupAdoptSocialWorker> SocialWorkerOrganisation_List { get; set; }

        public IEnumerable<ProvinceLookupAdopt> Province_List { get; set; }
        public IEnumerable<DistrictLookupAdopt> District_List { get; set; }

        public IEnumerable<LocalMunicipalityLookupAdopt> LocalMunicipality_List { get; set; }

        public virtual IEnumerable<AdoptionCategoryLookupAdopt> AdoptionCategory_List { get; set; }
        //Herman



        public IEnumerable<OrganisationTypeLookupAdoptChild> ChildOrganisation_Type_List { get; set; }
        //public virtual IEnumerable<OrganisationTypeLookupSocialWorker> Social_WorkerOrganisation_Type_List { get; set; }

        public IEnumerable<OrganisationTypeLookupAdoptParent> ParentdOrganisation_Type_List { get; set; }
        public IEnumerable<OrganisationTypeLookupAdoptSocialWorker> SocialWorkerOrganisation_Type_List { get; set; }
        //Hermand end

    }

    public class AdoptionCategory
    {
        public int? selectAdoptionCategory { get; set; }
        public IEnumerable<AdoptionCategoryLookupAdopt> AdoptionCategory_List { get; set; }
    }

    public class AdoptionCategoryLookupAdopt
    {
        public int? Adoption_Category_Id { get; set; }
        public string Description { get; set; }

    }
    public class LocalMunicipality
    {
        public int? selectLocalMunicipality { get; set; }
        public IEnumerable<LocalMunicipalityLookupAdopt> LocalMunicipality_List { get; set; }
    }

    public class LocalMunicipalityLookupAdopt
    {
        public int? Local_Municipality_Id { get; set; }
        public int? Local_Municipality_IdParent { get; set; }
        public string Description { get; set; }

    }

    public class ChildSocialWorker
    {
        public int? selectChildSocialWorker { get; set; }
        public IEnumerable<ChildSocialWorkerLookupAdopt> ChildSocialWorker_List { get; set; }
    }

    public class ChildSocialWorkerLookupAdopt
    {
        public int? Child_Social_Worker_Id { get; set; }
        public string Names { get; set; }

    }

    public class ParentSocialWorker
    {
        public int? selectParentSocialWorker { get; set; }
        public IEnumerable<ParentSocialWorkerLookupAdopt> ParentSocialWorker_List { get; set; }
    }

    public class ParentSocialWorkerLookupAdopt
    {
        public int? Parent_Social_Worker_Id { get; set; }
        public string SocPNames { get; set; }

    }

    public class ReasonForAdoption
    {
        public int? selectReason { get; set; }
        public IEnumerable<ReasonForAdoptionLookupAdopt> Reason_Type_List { get; set; }
    }

    public class ReasonForAdoptionLookupAdopt
    {
        public int? Adoptions_Reason_Id { get; set; }
        public string Description { get; set; }

    }


    public class Legitimacy
    {
        public int? selectLegitimacy { get; set; }
        public IEnumerable<LegitimacyLookupAdopt> Legitimacy_Type_List { get; set; }
    }


    public class LegitimacyLookupAdopt
    {
        public int? Legitimacy_Id { get; set; }
        public string Description { get; set; }

    }

    public class Cross_Cultural
    {
        public int? selectCrossCultural { get; set; }
        public IEnumerable<Cross_CulturalLookupAdopt> CrossCultural_Type_List { get; set; }
    }

    public class Cross_CulturalLookupAdopt
    {
        public int Cross_Cultural_Id { get; set; }
        public string Description { get; set; }

    }

    public class Record_Status
    {
        public int? selectStatus { get; set; }
        public IEnumerable<Record_StatusLookupAdopt> Status_Type_List { get; set; }
    }

    public class Record_StatusLookupAdopt
    {
        public int Record_Status_Id { get; set; }
        public string Description { get; set; }

    }


    public class AdoptionRelationNonRelation
    {
        public int? selectRelationStatus { get; set; }
        public IEnumerable<AdoptionRelationNonRelationLookupAdopt> RelationStatus_List { get; set; }
    }
    public class AdoptionRelationNonRelationLookupAdopt
    {
        public int NonRelation_Realtion_Id { get; set; }
        public string Description { get; set; }

    }

    public class AdoptionType
    {
        public int? selectAdoptionType { get; set; }
        public IEnumerable<AdoptionTypeLookupAdopt> AdoptionType_List { get; set; }
    }
    public class AdoptionTypeLookupAdopt
    {
        public int Adopt_Type_Id { get; set; }
        public string Description { get; set; }

    }

    public class RelationshipType
    {
        public int? selectRelationshipType { get; set; }
        public IEnumerable<RelationshipTypeLookupAdopt> RelationshipType_List { get; set; }
    }


    public class RelationshipTypeLookupAdopt
    {
        public int Relationship_Type_Id { get; set; }
        public string Description { get; set; }

    }
    public class CorrespondenceTypeLookupAdopt
    {
        public int Correspondence_Type_Id { get; set; }
        public string Description { get; set; }

    }

    public class CorrespondenceType
    {
        public int? selectCorrespondenceType { get; set; }
        public IEnumerable<CorrespondenceTypeLookupAdopt> CorrespondenceType_List { get; set; }
    }



    public class AdoptingReason
    {
        public int? selectAdopting { get; set; }
        public IEnumerable<ReasonForAdoptingLookupAdopt> Adopting_Type_List { get; set; }
    }

    public class ReasonForAdoptingLookupAdopt
    {
        public int? Adopting_Reason_Id { get; set; }
        public string Description { get; set; }

    }

    public class CourtAdopt
    {
        public int? SelectCourtDetails { get; set; }
        public IEnumerable<CourtLookupAdopt> Court_List { get; set; }
    }

    public class ProvinceLookupAdopt
    {
        public int? Province_Id { get; set; }
        public int? Province_IdChildOrg { get; set; }
        public int? Province_IdParentOrg { get; set; }
        public string Description { get; set; }

    }

    public class ProvinceAdopt
    {
        public int? SelectProvinceDetails { get; set; }
        public IEnumerable<ProvinceLookupAdopt> Province_List { get; set; }
    }

    public class DistrictLookupAdopt
    {
        public int? District_Id { get; set; }
        public int? District_IdChilOrg { get; set; }
        public int? District_IdParentOrg { get; set; }
        public string Description { get; set; }

    }

    public class DistrictAdopt
    {
        public int? SelectDistrictDetails { get; set; }
        public IEnumerable<DistrictLookupAdopt> District_List { get; set; }
    }

    public class CourtLookupAdopt
    {
        public int? Court_id { get; set; }
        public string Description { get; set; }

    }

    public class OrganisationChild
    {
        public int? SelectOrganisationchildDetails { get; set; }
        public IEnumerable<OrganisationLookupAdoptChild> ChildOrganisation_List { get; set; }
    }
    public class OrganisationLookupAdoptChild
    {
        public int? Organization_Id { get; set; }
        public string Description { get; set; }



    }

    public class OrganisationParent
    {
        public int? SelectOrganisationParentDetails { get; set; }
        public IEnumerable<OrganisationLookupAdoptParent> ParentOrganisation_List { get; set; }
    }
    public class OrganisationLookupAdoptParent
    {
        public int Organization_Id { get; set; }
        public string Description { get; set; }

        public string Telephone_Number { get; set; }
        public string Email_Address { get; set; }

    }

    public class OrganisationSocialWorker
    {
        public int? SelectOrganisationParentDetails { get; set; }
        public IEnumerable<OrganisationLookupAdoptSocialWorker> SocialWorkerOrganisation_List { get; set; }
    }
    public class OrganisationLookupAdoptSocialWorker
    {
        public int Organization_Id { get; set; }
        public string Description { get; set; }

        public string Telephone_Number { get; set; }
        public string Email_Address { get; set; }
    }
    //Merge Models
    public class MainPageModelVM
    {
        public AdoptionCaseViewModel AdoptionCaseViewModel { get; set; }
        public AdoptionPersonViewModel AdoptionPersonViewModel { get; set; }
    }
    //Herman

    public class AdoptionSupportingDocumentsVM
    {
        public int DocumentsId { get; set; }
        public int? DocumentsLabel { get; set; }

        public string DocLabel { get; set; }

        public string DocumentsType { get; set; }
        [Required]
        [Display(Name = "File name:")]
        public string DocumentsFileName { get; set; }
        public string DocumentsPath { get; set; }
        [Required]
        [Display(Name = "Attachment description")]
        public string DocumentsDescription { get; set; }
        [Display(Name = "Intake Assessment ID")]

        public int? Intake_Assessment_Id { get; set; }

        public int DocumentsCheck { get; set; }
        [Display(Name = "Kind of Document")]

        public string KindOfDocument { get; set; }
        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [DataType(DataType.ImageUrl)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public List<AdoptionSupportingDocumentsVM> DocumentsVMs { get; set; }
        public virtual ICollection<AdoptionSupportingDocumentsVM> FileDocumentDetails { get; set; }

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

    public class OrganisationTypeLookupSocialWorker
    {
        public int Organization_Type_Id { get; set; }
        public string Description { get; set; }



    }
    public class OrganisationTypeLookupAdoptChild
    {
        public int Organization_Type_Id { get; set; }
        public string Description { get; set; }



    }


    //Herman End



}


