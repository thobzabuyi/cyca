using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common_Objects.ViewModels;

namespace CYCA_Module_V2.Common_Objects
{
    public class CYCAAdmissionsModel
    {
        public CYCA_ClientBiometricViewModel GetBiometricViewModel(int PersonId)
        {
            CYCA_ClientBiometricViewModel returnModel = new CYCA_ClientBiometricViewModel();
            using (SDIIS_DatabaseEntities _context = new SDIIS_DatabaseEntities())
            {
                var ad = (from a in _context.CYCA_Admissions_AdmissionDetails
                          where a.Client_Id.Equals(PersonId)
                          select a).ToList();

                var afis = _context.int_DSD_Afis.Where(af => af.Person_Id.Equals(PersonId)).SingleOrDefault();
                var person = _context.Persons.Where(p => p.Person_Id.Equals(PersonId)).Single();
                if (afis != null)
                {
                    returnModel.HasBiometric = true;
                    returnModel.IsPivaVerified = person.Is_Piva_Validated;
                    returnModel.IsVerified = afis.Is_Verified;
                }
                else
                {
                    returnModel.HasBiometric = false;
                    returnModel.IsPivaVerified = false;
                    returnModel.IsVerified = false;
                }
            }
            return returnModel;
        }

        public List<CYCAAdmissionBodilySearchViewModel> GetBodilySearchList(int Id)
        {
            List<CYCAAdmissionBodilySearchViewModel> avm = new List<CYCAAdmissionBodilySearchViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var docL = db.CYCA_BodilySearch_Document.ToList();
            var ListP = (
                 (from bs in db.CYCA_BodilySearch
                  //join bsd in db.CYCA_BodilySearch_Document on bs.Bodily_Search_Id equals bsd.Bodily_Search_Id
                  join a in db.CYCA_Admissions_AdmissionDetails on bs.Admission_Id equals a.Admission_Id
                  join bsr in db.apl_Cyca_Bodily_Search_Reasons on bs.Search_Reason_Id equals bsr.Search_Reason_Id
                  join p in db.Persons on bs.Person_Id equals p.Person_Id                  
                  where p.Person_Id == Id
                  select new
                  {
                      bs.Bodily_Search_Id,
                      //bsd.Document_Id,
                      bsr.Search_Reason_Id,
                      bs.Conducted_By,
                      bs.Witnessed_By,
                      a.Admission_Id

                  }).ToList());
            ;
            foreach (var item in ListP)
            {
                CYCAAdmissionBodilySearchViewModel obj = new CYCAAdmissionBodilySearchViewModel();
                obj.Bodily_Search_Id = item.Bodily_Search_Id;
                //obj.DocumentId = item.Document_Id;
                obj.Admission_Id = item.Admission_Id;
                obj.Bodily_Search_Date = db.CYCA_BodilySearch.Find(item.Bodily_Search_Id).Bodily_Search_Date.ToString();
                obj.ReasonForSearch = db.apl_Cyca_Bodily_Search_Reasons.Find(item.Search_Reason_Id).Description;
                obj.ConductedBy = db.Users.Find(item.Conducted_By).First_Name + " " + db.Users.Find(item.Conducted_By).Last_Name;
                obj.WitnessedBy = db.Users.Find(item.Witnessed_By).First_Name + " " + db.Users.Find(item.Witnessed_By).Last_Name;
                obj.Files = docL.Where(x => x.Bodily_Search_Id == item.Bodily_Search_Id).ToList();
                avm.Add(obj);
            }

            return avm;
        }

        public CYCAAdmissionBodilySearchPartiallViewModel GetBodySearchByBodySearchId(int BodySearchId)
        {
            CYCAAdmissionBodilySearchPartiallViewModel vm = new CYCAAdmissionBodilySearchPartiallViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_BodilySearch act = db.CYCA_BodilySearch.Find(BodySearchId);
                    if (act != null)
                    {
                        vm.BodySearchId = BodySearchId;
                        vm.Admission_Id = Convert.ToInt16(act.Admission_Id);
                        vm.BodySearchDate = act.Bodily_Search_Date.ToString();
                        vm.Search_Reason_Id = act.Search_Reason_Id;
                        vm.ReasonForSearch = db.apl_Cyca_Bodily_Search_Reasons.Find(act.Search_Reason_Id).Description;
                        vm.Conducted_By = act.Conducted_By;
                        vm.ConductedBy = db.Users.Find(act.Conducted_By).First_Name + " " + db.Users.Find(act.Conducted_By).Last_Name;
                        vm.Witnessed_By = act.Witnessed_By;
                        vm.WitnessedBy = db.Users.Find(act.Witnessed_By).First_Name + " " + db.Users.Find(act.Witnessed_By).Last_Name;
                        vm.Document_Type_Id = act.Document_Type_Id;                            
                        vm.Description = act.Description;
                        vm.saveBodySearchId = BodySearchId;
                    }

                    CYCA_BodilySearch_Document bsd = db.CYCA_BodilySearch_Document.Find(BodySearchId);
                    var bs = db.CYCA_BodilySearch_Document.Find(BodySearchId);
                    
                    var docL = db.CYCA_BodilySearch_Document.ToList();

                    //if (bsd != null)
                    //{
                        foreach (var item in docL)
                        {
                        
                        if (item.Bodily_Search_Id == BodySearchId)
                            {
                            vm.saveBodySearchId = BodySearchId;
                            vm.BodySearchId = BodySearchId;
                                vm.DocumentId = item.Document_Id;
                                vm.DocumentName = item.Document_Name;                                
                            }
                           
                        }

                    //}
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            return vm;
        }

        public CYCAAdmissionBodilySearchPartiallViewModel GetIllegalItemByIllegalItemId(int Item_Found_Id)
        {
            CYCAAdmissionBodilySearchPartiallViewModel vm = new CYCAAdmissionBodilySearchPartiallViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    CYCA_Admissions_IllegalItemsFound act = db.CYCA_Admissions_IllegalItemsFound.Find(Item_Found_Id);
                    if (act != null)
                    {
                        vm.Item_Found_Id = Item_Found_Id;
                        vm.Admission_Id = Convert.ToInt16(act.Admission_Id);
                        vm.Quantity = act.Quantity;
                        vm.Handed_By = act.Handed_By;                       
                        vm.Document_Type_Id = act.Document_Type_Id;
                        vm.Description = act.Description;                       
                        
                    }

                    //CYCA_BodilySearch_Document bsd = db.CYCA_BodilySearch_Document.Find(BodySearchId);

                    //var docL = db.CYCA_BodilySearch_Document.ToList();

                    //if (bsd != null)
                    //{
                    //    foreach (var item in docL)
                    //    {
                    //        vm.BodySearchId = BodySearchId;
                    //        vm.DocumentId = bsd.Document_Id;
                    //        vm.DocumentName = bsd.Document_Name;
                    //    }

                    //}
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            return vm;
        }


    }
    public class CYCAAdmissionsViewModel
    {
        public int AddmissionId { get; set; }
        public int ReAddmissionId { get; set; }
        public string FacilityName { get; set; }
    }
    public class CYCAAdmissionBodySearchViewModel
    {
        public string ChildFullName { get; set; }
        public int? Person_Id { get; set; }
        public int? CLientID { get; set; }
        public int BodySearchId { get; set; }
        public string Facility { get; set; }
        public int? AdmissionId { get; set; }
        public string BodySearchDate { get; set; }
        [Display(Name = "Reason For Bodily Search")]
        public string ReasonForSearch { get; set; }
        [Display(Name = "Describe other Search Reason")]
        public string OtherReasonForSearch { get; set; }
        public string Description { get; set; }
        public int? WitnessedById { get; set; }
        [Display(Name = "Witnessed By")]
        public string WitnessedBy { get; set; }
        public string CreatedBy { get; set; }
        public string Admission { get; set; }
        public int? ConductedById { get; set; }
        [Display(Name = "Conducted By")]
        public string ConductedBy { get; set; }
        public int? SearchReasonId { get; set; }
        public string DocumentName { get; set; }
        public int? DocumentId { get; set; }
        public int? Document_Type_Id { get; set; }
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }
        public int? Admission_Id { get; set; }
        public int? saveBodySearchId { get; set; }
        public string RequestType { get; set; }
        public string Additional_Info { get; set; }
        public string OtherDocTypeDescription { get; set; }
        public string OtherSeacrhReasonDescription { get; set; }
        public List<CYCA_BodilySearch_Document> Files { get; set; }
        public List<LiteFiles> liteFiles { get; set; }

    }
    public class CycaAdmissionIllegalItemsViewModel
    {
        public int Item_Found_Id { get; set; }
        public int? Person_Id { get; set; }
        public int? CLientID { get; set; }        
        //[Display(Name = "Quantity:")]
        public int? Quantity { get; set; }
        public int? Handed_By { get; set; }
        public int? Handed_To { get; set; }
        public int? Admission_Id { get; set; }
        public int? DocType_Id { get; set; }
        public string Additional_Info { get; set; }
        public string RequestType { get; set; }
        //[Display(Name = "Item Handed To:")]
        public string selectedHandedBy { get; set; }
        //[Display(Name = "Description of Illegal Item(s) Found:")]
        public string Description { get; set; }
        public DateTime Date_Captured { get; set; }
        public string IllegalItemDate { get; set; }
        public string OtherDocTypeDescription { get; set; }
        public List<CYCA_IllegalItems_Document> Files { get; set; }
        public List<LiteFiles> liteFiles { get; set; }

    }
    public class CYCAAdmissionBodilySearchPartiallViewModel
    {
        public string ChildFullName { get; set; }        
        public int? CLientID { get; set; }
        public int? Person_Id { get; set; }
        public int BodySearchId { get; set; }
        public string Facility { get; set; }
        public int? AdmissionId { get; set; }
        public string BodySearchDate { get; set; }
        public string ReasonForSearch { get; set; }
        public string Description { get; set; }
        public int? Witnessed_By { get; set; }
        public string WitnessedBy { get; set; }
        public string CreatedBy { get; set; }
        public string Admission { get; set; }
        public int? Conducted_By { get; set; }
        public string ConductedBy { get; set; }
        public int? Search_Reason_Id { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentTypeName { get; set; }
        public int? Document_Type_Id { get; set; }
        public int? Admission_Id { get; set; }
        public int? saveBodySearchId { get; set; }


        public int Item_Found_Id { get; set; }
        public int? Quantity { get; set; }
        public int? Handed_By { get; set; }
        //[Display(Name = "Item Handed To:")]
        public string selectedHandedBy { get; set; }
        public int? DocType_Id { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Additional_Info { get; set; }
     
        public List<CYCAAdmissionBodySearchViewModel> CYCAAdmissionBodySearchViewModels { get; set; }
        public List<CycaAdmissionIllegalItemsViewModel> CycaAdmissionIllegalItemsViewModels { get; set; }
        public int PersonId { get; set; }
    }

   

    public class CYCAAdmissionExtraMuralActivityViewModel
    {
        public int Extra_Mural_Activity_Id { get; set; }
        public int Person_Id { get; set; }
        public int Admission_Id { get; set; }

        [Display(Name = "Weight:")]
        [Required(ErrorMessage ="This field is required")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string[] Hobby_Id { get; set; }
        [Display(Name = "Child Hobbies/Interest:")]       
        public string selectedHobby { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string[] Activity_Id { get; set; }
        [Display(Name = "Sport Activities:")]      
        public string selectedSportActivity { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int? Eye_Color_Id { get; set; }
        [Display(Name = "Eye Color:")]
        public string selectedEyeColor { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int? Hair_Color_Id { get; set; }
        [Display(Name = "Hair Color:")]     
        public string selectedHairColor { get; set; }


        [Required(ErrorMessage = "This field is required")]
        public int? Physical_Build_Id { get; set; }
        [Display(Name = "Physical Build:")]     
        public string selectedPhysicalBuild { get; set; }

       
        [Display(Name = "Additional Description:")]
        public string Additional_Description { get; set; }

        public string DateCreated { get; set; }

        
       


        public IEnumerable<apl_Cyca_Child_Hobbies> HobbiesCollection { get; set; }
       
    }

    public class CYCAAdmissionExtraMuralActivityPartialViewModel
    {
        public List<CYCAAdmissionExtraMuralActivityViewModel> CYCAAdmissionExtraMuralActivityViewModels { get; set; }
        public int PersonId { get; set; }
        public int AdmissionId { get; set; }
        string Additiona_Description { get; set; }
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
        public int Admission_Id { get; set; }
        public string DateCreated { get; set; }
    }

    public class CYCADischargeViewModel
    {
        public string ChildFullName { get; set; }
        public int PersonId { get; set; }

        public int DischargeId { get; set; }
        public int AdmissionId { get; set; }
        public int? UserHandedOverId { get; set; }
        [Display(Name = "Staff member who handed the child over:")]
        public string selectedUserHandedOver { get; set; }
        public int? UserHandedOverDesignationId { get; set; }
        [Display(Name = "Designation of staff member:")]
        public string selectedUserHandedOverDesignation { get; set; }
        public int? UserReceivedDesignationId { get; set; }
        [Display(Name = "Designation of person receiving the child:")]
        public string selectedUserReceivedDesignation { get; set; }
        [Display(Name = "Name of the person receiving the child:")]

        public string UserReceivedName { get; set; }
        public int? UserReceivedOrganisationId { get; set; }
        [Display(Name = "Organisation of person receiving the child:")]
        public string selectedUserReceivedOrganisation { get; set; }
        [Display(Name = "Details of other organisation:")]
        public string OtherOrgComment { get; set; }
        public int? DischargeReasonId { get; set; }
        [Display(Name = "Reason for discharge:")]
        public string selectedDischargeReason { get; set; }
        [Display(Name = "Discharge date:")]

        public string DischargeDate { get; set; }
        public string Comments { get; set; }
        [Display(Name = "Keep bed space:")]

        public bool KeepBedSpace { get; set; }
        [Display(Name = "Expected date of return:")]

        public string ExpectedReturnDate { get; set; }
        public int? DocType_Id { get; set; }
        public string RequestType { get; set; }
        public List<CYCA_Admissions_Document> Files { get; set; }
    }
    public class ClientDischargeHistoryModel 
    {
        public int CLientID { get; set; }
        public List<CYCADischargeViewModel> ClientDischargeHistory { get; set; }
    }
    public class StaffDisplay
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public List<string> roles { get; set; }
    }

}
