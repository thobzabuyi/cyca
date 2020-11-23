using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Common_Objects.Models
{
    public class CYCAdmissionModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public PCM_Case_Details GetSpecificPCMCase(int caseId)
        {
            PCM_Case_Details pcmcase;

            try
            {
                var pcmcaseList = (from r in db.PCM_Case_Details
                                   where r.PCM_Case_Id.Equals(caseId)
                                   select r).ToList();

                pcmcase = (from r in pcmcaseList
                           select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }


            return pcmcase;
        }
        public List<apl_Admission_Type> GetAplAdmissionTypes()
        {
           return db.apl_Admission_Type.ToList();
        }
        public List<Intake_Assessment> GetListOfIntakeAssessments(int provinceId, int caseManagerId)
        {
            List<Intake_Assessment> intakeAssessments;

            try
            {
                var intakeAssessmentList = (from r in db.Intake_Assessments
                                            where ((r.Assessor.apl_Social_Worker.Any() && r.Assessor.apl_Social_Worker.FirstOrDefault().apl_Service_Office.apl_Local_Municipality.District.Province_Id == (provinceId))
                                                || (r.Assessor.Employees.Any() && r.Assessor.Employees.FirstOrDefault().apl_Service_Office.apl_Local_Municipality.District.Province_Id == (provinceId)))
                                                && (r.Case_Manager_Id == caseManagerId)
                                            select r).ToList();

                intakeAssessments = (from r in intakeAssessmentList
                                     select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return intakeAssessments;
        }
        public CYCA_Child_Allocation GetChildAllocation(int admissionId)
        {
            return db.CYCA_Child_Allocation.Where(ca => ca.Admission_Id == admissionId).SingleOrDefault();
        }
        public void SaveContext()
        {
            db.SaveChanges();
        }
        public List<apl_Cyca_Bodily_Search_Reasons> getBodySearchReasons()
        {
          return  db.apl_Cyca_Bodily_Search_Reasons.ToList();
        }
        public dynamic GetFilteredBodySearchReasons()
        {
            var reasons =  from s in db.apl_Cyca_Bodily_Search_Reasons.ToList()
                   select new
                   {
                       search_ReasonId = s.Search_Reason_Id,
                       description = s.Description
                   };
            return reasons;
        }
        public List<apl_Cyca_Admission_DocumentType> GetCycaAdmissionDocumentTypeList()
        {
            return db.apl_Cyca_Admission_DocumentType.ToList();
        }

        public void SaveCycaAdmissionDocument(CYCA_Admissions_Document document)
        {
            db.CYCA_Admissions_Document.Add(document);
            db.SaveChanges();
        }
        public dynamic getFilteredUserDetails()
        {
            var details = from u in db.Users.ToList()
                          select new
                          {
                              userId = u.User_Id,
                              Full_Name = u.First_Name + " " + u.Last_Name
                          };
            return details;
        }
        public int GetPCMPersonIdByintAssId(int intAssessmentId)
        {
            return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select p.Person_Id).SingleOrDefault();
        }
        public void AddBeog(CYCA_Admissions_BodilySearch eow)
            {
            db.CYCA_Admissions_BodilySearch.Add(eow);
            }
        public CYCAAdmissionViewModel GetPCMPerson(int personId)
        {
            CYCAAdmissionViewModel pcvm = new CYCAAdmissionViewModel();

            var act = db.Persons.Find(personId);
            pcvm.Person_Id = personId;
            pcvm.First_Name = act.First_Name;
            pcvm.Last_Name = act.Last_Name;
            pcvm.Known_As = act.Known_As;
            pcvm.Identification_Type_Id = act.Identification_Type_Id;
            pcvm.Identification_Number = act.Identification_Number;
            pcvm.Date_Of_Birth = act.Date_Of_Birth;
            pcvm.Age = act.Age;
            pcvm.Is_Estimated_Age = act.Is_Estimated_Age;
            pcvm.Language_Id = act.Language_Id;
            pcvm.Gender_Id = act.Gender_Id;
            pcvm.Marital_Status_Id = act.Marital_Status_Id;
            pcvm.Preferred_Contact_Type_Id = act.Preferred_Contact_Type_Id;
            pcvm.Religion_Id = act.Religion_Id;
            pcvm.Phone_Number = act.Phone_Number;
            pcvm.Mobile_Phone_Number = act.Mobile_Phone_Number;
            pcvm.Email_Address = act.Email_Address;
            pcvm.Population_Group_Id = act.Population_Group_Id;
            pcvm.Nationality_Id = act.Nationality_Id;
            pcvm.Disability_Type_Id = act.Disability_Type_Id;

            return pcvm;
        }


        public void UpdatePersonalDetails(CYCAAdmissionViewModel person, string myStringuserId, int personId)
        {
          
                try
                {
                    Person newPerson = db.Persons.Find(personId);

                    newPerson.First_Name = person.First_Name;
                    newPerson.Last_Name = person.Last_Name;
                    newPerson.Known_As = person.Known_As;
                    newPerson.Identification_Type_Id = person.Identification_Type_Id;
                    newPerson.Identification_Number = person.Identification_Number;
                    newPerson.Date_Of_Birth = person.Date_Of_Birth;
                    newPerson.Age = person.Age;
                    newPerson.Language_Id = person.Language_Id;
                    newPerson.Gender_Id = person.Gender_Id;
                    newPerson.Marital_Status_Id = person.Marital_Status_Id;
                    newPerson.Preferred_Contact_Type_Id = person.Preferred_Contact_Type_Id;
                    newPerson.Population_Group_Id = person.Population_Group_Id;
                    newPerson.Disability_Type_Id = person.Disability_Type_Id;
                    newPerson.Modified_By = myStringuserId;
                    newPerson.Date_Last_Modified = DateTime.Now;
                    db.SaveChanges();
                }
                catch
                {

                }

        }
        //..........................................................................ASSSESSMENT REGISTER.........................................

        public int GetClientRefIdssId(int intAssessmentId)
        {
            return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select f.Client_Module_Id).FirstOrDefault();
        }

        public string GetClientRef(int ClientRefid)
        {
            return (from p in db .Persons
                        join c in db .Clients on p.Person_Id equals c.Person_Id
                        join i in db .Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db .int_Client_Module_Registration on c.Client_Id equals f.Client_Id

                        where f.Client_Module_Id.Equals(ClientRefid) && i.Problem_Sub_Category_Id == 22
                        select f.Client_Module_Ref_No).FirstOrDefault();
        }


        #region GENERAL DETAILS

        public int GetPCMGeneralDetailsByassId(int intAssessmentId)
        {         
             return (from p in db .Persons
                        join c in db .Clients on p.Person_Id equals c.Person_Id
                        join i in db .Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db .PCM_General_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.General_Details_Id).FirstOrDefault();
        }
        public int GetActiveAdmissionID(int personId, int facilityId)
        {
            return (from p in db.Persons
                    join c in db.Clients on p.Person_Id equals c.Person_Id
                    join a in db.CYCA_Admissions_AdmissionDetails on new { client = c.Client_Id, facility = facilityId, active = true } equals new { client = a.Client_Id, facility = a.Facility_Id, active = a.Is_Active }
                    where p.Person_Id == personId
                    select a.Admission_Id).SingleOrDefault();
        }
        public CYCAAdmissionViewModel GetGeneralDetailsList(int GeneralId)
        {
            CYCAAdmissionViewModel fvm = new CYCAAdmissionViewModel();

                try
                {
                    PCM_General_Details act = db .PCM_General_Details.Find(GeneralId);

                    fvm.General_Details_Id = act.General_Details_Id;
                    fvm.Consulted_Sources = act.Consulted_Sources;
                    fvm.Trace_Efforts = act.Trace_Efforts;
                    fvm.Assessment_TimeEnd = act.Assessment_Time;
                    fvm.Assessment_DateEnd = act.Assessment_Date;
                    fvm.Additional_Info = act.Additional_Info;
                    fvm.Intake_Assessment_Id = act.Intake_Assessment_Id;

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

                return fvm;
        }

        public void InsertGeneralDetails(CYCAAdmissionViewModel cases, int intassid, int userId)
        {
                try
                {

                    PCM_General_Details newCase = new PCM_General_Details();

                    newCase.Intake_Assessment_Id = intassid;
                    newCase.Consulted_Sources = cases.Consulted_Sources;
                    newCase.Trace_Efforts = cases.Trace_Efforts;
                    newCase.Assessment_Date = cases.Assessment_DateEnd;
                    newCase.Assessment_Time = cases.Assessment_TimeEnd;
                    newCase.Additional_Info = cases.Additional_Info;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_General_Details.Add(newCase);

                    db.SaveChanges();
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

        public void UpdatePCM_General_Details(CYCAAdmissionViewModel cases, int userId, int AssId)
        {
                try
                {


                    PCM_General_Details editCase = db.PCM_General_Details.Find(AssId);

                    if (AssId > 0)
                    {

                        editCase.Consulted_Sources = cases.Consulted_Sources;
                        editCase.Trace_Efforts = cases.Trace_Efforts;
                        editCase.Assessment_Date = cases.Assessment_DateEnd;
                        editCase.Assessment_Time = cases.Assessment_TimeEnd;
                        editCase.Additional_Info = cases.Additional_Info;
                        editCase.Date_Modified = DateTime.Now;
                        editCase.Modified_By = userId;
                    }

                    db.SaveChanges();
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
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
        }
        #endregion

        #region CYCA ADMISSION
        //GET CLIENT MODULE REFERENCE ID            
        public int GetClientRefByAssId(int intAssessmentId)
         {
           return (from p in db.CYCA_Admissions_AdmissionDetails
                        join c in db.Clients on p.Client_Id equals c.Client_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select f.Client_Module_Id).FirstOrDefault();
        }

        //GET LIST OF ADMISSIONS PER ASSESSMENT
        public List<CYCAAdmissionViewModel> GetAdmissionListByPCMClientModuleId(int ClientRef)
        {
            // initialising view model 
            List<CYCAAdmissionViewModel> avm = new List<CYCAAdmissionViewModel>();

            // get admission list of a client per assessment
            var ListP = (
               (from a in db.CYCA_Admissions_AdmissionDetails
                join at in db.apl_Admission_Type on a.Admission_Type_Id equals at.Admission_Type_Id
                join r in db.int_Client_Module_Registration on a.Pcm_Ass_No equals r.Client_Module_Id
                where a.Pcm_Ass_No == ClientRef
                select new
                {
                    r.Client_Module_Id,
                    a.Admission_Id,
                    at.Admission_Type_Id
                }).ToList());
            
            foreach (var item in ListP)
            {
                // initialising view model
                CYCAAdmissionViewModel obj = new CYCAAdmissionViewModel();
                obj.Admission_Id = item.Admission_Id;
                obj.CaseStartDate = db.CYCA_Admissions_AdmissionDetails.Find(item.Admission_Id).Case_Start_Date;
                obj.CaseEndDate = db.CYCA_Admissions_AdmissionDetails.Find(item.Admission_Id).Case_End_Date.ToString();
                obj.selectedAdmissionType = db.apl_Admission_Type.Find(item.Admission_Type_Id).Description;
                obj.selectedClietRef = db.int_Client_Module_Registration.Find(item.Client_Module_Id).Client_Module_Ref_No;
                avm.Add(obj);
            }

            return avm;
        }
  
        //CREATE NEW ADMISSION
        public void AddAdmission(CYCAAdmissionViewModel vm, int clientref, int userId)
        {
                try
                {
                    CYCA_Admissions_AdmissionDetails newob = new CYCA_Admissions_AdmissionDetails();

                    
                    newob.Client_Id = (from a in db.CYCA_Admissions_AdmissionDetails
                                       join c in db.Clients on a.Client_Id equals c.Client_Id
                                       join cm in db.int_Client_Module_Registration on c.Client_Id equals cm.Client_Id
                                       where cm.Client_Module_Id == clientref
                                       select c.Client_Id).FirstOrDefault();
                    newob.Case_Start_Date = DateTime.Now;
                    newob.Case_Start_Time = DateTime.Now.ToString("HH:mm");
                    newob.Case_End_Date = Convert.ToDateTime(vm.CaseEndDate);
                    //newob.Case_End_Date = DateTime.ParseExact(vm.CaseEndDate, "yyyy/MM/d", CultureInfo.InvariantCulture);
                    newob.Case_End_Time = vm.CaseEndTime;
                    newob.Facility_Id = (from a in db.CYCA_Admissions_AdmissionDetails
                                         join f in db.apl_Cyca_Facility on a.Facility_Id equals f.Facility_Id
                                         where a.Pcm_Ass_No == clientref
                                         select f.Facility_Id).FirstOrDefault();
                    newob.Admission_Type_Id = vm.Admission_Type_Id;
                    newob.Comments_And_Observation = vm.CommentsAndObservation;
                    newob.Venue_Id = vm.VenueId;
                    newob.Admission_Date = DateTime.Now;
                    newob.Date_Captured = DateTime.Now;
                    newob.Captured_By = userId;
                    newob.Pcm_Ass_No = clientref;
                    newob.Date_Created = DateTime.Now;
                    newob.Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                        join u in db.Users on a.Captured_By equals u.User_Id
                                        select u.First_Name).FirstOrDefault();
                                       
                    newob.Date_Last_Modified = vm.Date_Last_Modified;
                    newob.Modified_By = vm.Modified_By;
                    newob.Is_Active = true;
                    newob.Is_Deleted = false;


                    db.CYCA_Admissions_AdmissionDetails.Add(newob);
                    db.SaveChanges();
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

        public CYCAAdmissionViewModel SelectAdmissionByAdmissionId(int AdmissionId)
        {
            CYCAAdmissionViewModel vm = new CYCAAdmissionViewModel();

                try
                {
                    CYCA_Admissions_AdmissionDetails act = db.CYCA_Admissions_AdmissionDetails.Find(AdmissionId);
                    if (act != null)
                    {
                        vm.Admission_Id = AdmissionId;
                        
                        //vm.Admission_Type_Id = act.Admission_Type_Id;
                        ////vm.selectedAdmissionType = db.apl_Admission_Type.Find(act.Admission_Type_Id).Description;                      
                        //vm.AdmissionDate = act.Admission_Date.ToString();
                        //vm.CaseStartTime = act.Case_Start_Time;
                        //vm.VenueId = act.Venue_Id;
                        ////vm.selectedVenue = db.apl_Cyca_Venue.Find(act.Venue_Id).VenueName;
                        //vm.CaseEndDate = act.Case_End_Date;
                        //vm.CommentsAndObservation = act.Comments_And_Observation;
                    }


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
            return vm;
        }

        //POPULATE MODAL FOR EDIT ADMISSION
        public CYCAAdmissionViewModel GetAdmissionByAdmissionId(int AdmissionId)
        {
            CYCAAdmissionViewModel vm = new CYCAAdmissionViewModel();

                try
                {
                    CYCA_Admissions_AdmissionDetails act = db.CYCA_Admissions_AdmissionDetails.Find(AdmissionId);
                    if (act!=null)
                    {
                        vm.Admission_Id = AdmissionId;
                        vm.Admission_Type_Id = act.Admission_Type_Id;
                        //vm.selectedAdmissionType = db.apl_Admission_Type.Find(act.Admission_Type_Id).Description;                      
                        vm.AdmissionDate = act.Admission_Date.ToString();
                        vm.CaseStartTime = act.Case_Start_Time;
                        vm.VenueId = act.Venue_Id;
                        //vm.selectedVenue = db.apl_Cyca_Venue.Find(act.Venue_Id).VenueName;
                        vm.CaseEndDate = act.Case_End_Date.ToString();
                        vm.CommentsAndObservation = act.Comments_And_Observation;
                    }

                   
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
            return vm;
        }

        //UPDATE/EDIT ADMISSION 
         public void UpdateAdmission(CYCAAdmissionViewModel vm, int Admission_Id, int userId, int adtypeId, int ventypeId)
        {
                try
                {
                    CYCA_Admissions_AdmissionDetails edit = db.CYCA_Admissions_AdmissionDetails.Find(Admission_Id);


                    //Admission_Id = vm.Admission_Id;
                    edit.Admission_Type_Id = Convert.ToInt32(adtypeId);
                    edit.Venue_Id = Convert.ToInt32(ventypeId);
                    edit.Case_End_Date =Convert.ToDateTime(vm.CaseEndDate);                  
                    edit.Comments_And_Observation = vm.CommentsAndObservation;
                    edit.Date_Last_Modified = DateTime.Now;
                    edit.Modified_By = db.Users.Find(edit.Admission_Id).First_Name;
                                  

                    db.SaveChanges();
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



        #endregion

        #region RE-ADMISSION
        public int GetAdmissionIdByClientrefId(int ClientRefid)
        {
                return (from a in db.CYCA_Admissions_AdmissionDetails                       
                        join c in db.Clients on a.Client_Id equals c.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id
                        where f.Client_Module_Id == ClientRefid
                        select a.Admission_Id).FirstOrDefault();
        }

 

        //CREATE NEW READMISSION
        public void ReAdmit(CYCAAdmissionViewModel vm, int admissionId, int userId)
        {
                try
                {
                    CYCA_Admissions_ReAdmissionDetails newob = new CYCA_Admissions_ReAdmissionDetails();


                    newob.Admission_Id = admissionId;
                    newob.Re_Admission_StartDate = DateTime.Now.ToString("yyyy/MM/dd");
                    newob.Re_Admission_CaseEndDate = vm.Re_Admission_EndDate;
                    newob.AdmissionType_Id = vm.AdmissionType_Id;                   
                    newob.Comments = vm.Comments;                
                    newob.Re_Admission_VenueId = vm.Venue_Id;
                    newob.Date_Captured = DateTime.Now;
                    newob.Re_Admission_CourtStartTime = DateTime.Now;                 
                    newob.Date_Created = DateTime.Now;
                    newob.Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                        join u in db.Users on a.Captured_By equals u.User_Id
                                        select u.First_Name).FirstOrDefault();

                    newob.Date_Last_Modified = vm.Date_Last_Modified;
                    newob.Modified_By = vm.Modified_By;
                    newob.Is_Active = true;
                    newob.Is_Deleted = false;


                    db.CYCA_Admissions_ReAdmissionDetails.Add(newob);
                    db.SaveChanges();
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
        #endregion

        #region BODILY SEARCH
        public int GetAdmissionIdByClientId(int ClientRefId)
        {
           return (from b in db.CYCA_Admissions_BodilySearch
                        join a in db.CYCA_Admissions_AdmissionDetails on b.Admission_Id equals a.Admission_Id
                        join c in db.Clients on a.Client_Id equals c.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id
                        where a.Pcm_Ass_No == ClientRefId
                        select a.Admission_Id).FirstOrDefault();
        }

        public List<CYCAAdmissionViewModel> GetBodilySearchById(int AdmissionId)
        {
            // initialising view model 
            List<CYCAAdmissionViewModel> avm = new List<CYCAAdmissionViewModel>();
            // get admission list of a client per assessment
            var ListP = (
               (from b in db.CYCA_Admissions_BodilySearch
                join a in db.CYCA_Admissions_AdmissionDetails on b.Admission_Id equals a.Admission_Id
                join r in db.apl_Cyca_Bodily_Search_Reasons on b.Search_Reason_Id equals r.Search_Reason_Id
                where b.Admission_Id == AdmissionId
                select new
                {
                   b.Bodily_Search_Id,
                   b.Bodily_Search_Time,
                   b.Bodily_Search_Date,
                   a.Admission_Id,

                   r.Search_Reason_Id
                }).ToList());
            ;
            foreach (var item in ListP)
            {
                // initialising view model
                CYCAAdmissionViewModel obj = new CYCAAdmissionViewModel();
                obj.Admission_Id = AdmissionId;
                obj.Bodily_Search_Id = item.Bodily_Search_Id;
                obj.Bodily_Search_Date = item.Bodily_Search_Date;
                obj.Bodily_Search_Time = item.Bodily_Search_Time;         
                obj.selectedSearchReason = db.apl_Cyca_Bodily_Search_Reasons.Find(item.Search_Reason_Id).Description;               
                avm.Add(obj);
            }

            return avm;
        }

        //ADD NEW BODILY SEARCH
        public void AddBodilySearch(CYCAAdmissionViewModel vm, int admissionId, int userId)
        {
                try
                {
                    CYCA_Admissions_BodilySearch newob = new CYCA_Admissions_BodilySearch();


                    newob.Admission_Id = admissionId;
                    //newob.Bodily_Search_Id = vm.Bodily_Search_Id;
                    newob.Bodily_Search_Date = DateTime.Now;
                    newob.Bodily_Search_Time = DateTime.Now;
                    newob.Description = vm.Description;
                    newob.Search_Reason_Id = vm.Search_Reason_Id;
                    newob.Conducted_By = vm.Conducted_By;
                    newob.Witnessed_By = vm.Witnessed_By;
                    newob.Date_Created = DateTime.Now;
                    newob.Created_By = (from a in db.CYCA_Admissions_AdmissionDetails
                                        join u in db.Users on a.Captured_By equals u.User_Id
                                        select u.First_Name + " " + u.Last_Name).FirstOrDefault();                    
                    newob.Is_Active = true;
                    newob.Is_Deleted = false;


                    db.CYCA_Admissions_BodilySearch.Add(newob);
                    db.SaveChanges();
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

        //POPULATE EDIT MODAL
        public CYCAAdmissionViewModel GetBodilySById(int BodilySearchId)
        {
            CYCAAdmissionViewModel vm = new CYCAAdmissionViewModel();

                try
                {

                    CYCA_Admissions_BodilySearch act = db.CYCA_Admissions_BodilySearch.Find(BodilySearchId);

                    if (act != null)
                    {
                        //Get Request Details
                        vm.Bodily_Search_Id = BodilySearchId;                                            
                        vm.Bodily_Search_Date = act.Bodily_Search_Date;
                        vm.Bodily_Search_Time = act.Bodily_Search_Time;
                        vm.physicalLocationDescription = act.Description;
                        vm.selectedSearchReason = db.apl_Cyca_Bodily_Search_Reasons.Find(act.Search_Reason_Id).Description;
                        vm.selectedConductor = db.Users.Find(act.Conducted_By).First_Name + " " + db.Users.Find(act.Conducted_By).Last_Name;
                        vm.selectedWitness = db.Users.Find(act.Witnessed_By).First_Name + " " + db.Users.Find(act.Witnessed_By).Last_Name;

                    }
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

                return vm;
        }

        //UPDATE/EDIT BODILY SEARCH
        public void UpdateBodilySearch(CYCAAdmissionViewModel vm, int Bodily_Search_Id, int userId, int searchreasonId, int witnessId, int conductorId)
        {
                try
                {
                    CYCA_Admissions_BodilySearch edit = db.CYCA_Admissions_BodilySearch.Find(Bodily_Search_Id);


                    //Admission_Id = vm.Admission_Id;
                    edit.Search_Reason_Id = Convert.ToInt32(searchreasonId);
                    edit.Witnessed_By = Convert.ToInt32(witnessId);
                    edit.Conducted_By = Convert.ToInt32(conductorId);
                    edit.Description = vm.physicalLocationDescription;
                    edit.Date_Last_Modified = DateTime.Now;
                    edit.Modified_By = db.Users.Find(edit.Bodily_Search_Id).First_Name;


                    db.SaveChanges();
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
        #endregion

        #region ILLEGAL ITEMS FOUND

        public List<CYCAAdmissionViewModel> GetIllegalItemById(int AdmissionId)
        {
            // initialising view model 
            List<CYCAAdmissionViewModel> avm = new List<CYCAAdmissionViewModel>();
            // get admission list of a client per assessment
            var ListP = (
               (from b in db.CYCA_Admissions_IllegalItemsFound
                join a in db.CYCA_Admissions_AdmissionDetails on b.Admission_Id equals a.Admission_Id  
                join u in db.Users on b.Handed_By equals u.User_Id
                where b.Admission_Id == AdmissionId
                select new
                {
                   b.Item_Found_Id,
                   b.Description,
                   b.Quantity,                 
                   a.Admission_Id,  
                   u.User_Id
                }).ToList());
            ;
            foreach (var item in ListP)
            {
                // initialising view model
                CYCAAdmissionViewModel obj = new CYCAAdmissionViewModel();
                obj.Admission_Id = AdmissionId;               
                obj.Item_Found_Id = item.Item_Found_Id;
                obj.Item_Description = item.Description;
                obj.Quantity = item.Quantity;
                obj.selectedHandedBy = db.Users.Find(item.User_Id).First_Name + " " + db.Users.Find(item.User_Id).Last_Name;                              
                avm.Add(obj);
            }

            return avm;
        }

        //POPULATE EDIT MODAL
        public CYCAAdmissionViewModel GetEachIllegalItemId(int IllegalItemId)
        {
            CYCAAdmissionViewModel vm = new CYCAAdmissionViewModel();
                try
                {

                    CYCA_Admissions_IllegalItemsFound act = db.CYCA_Admissions_IllegalItemsFound.Find(IllegalItemId);

                    if (act != null)
                    {
                       
                        vm.Item_Found_Id = IllegalItemId;
                        vm.Item_Description = act.Description;
                        vm.Quantity = act.Quantity;
                        vm.selectedHandedBy = db.Users.Find(act.Handed_By).First_Name;

                    }
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

                return vm;
        }

        //UPDATE/EDIT ILLEGAL ITEMS
        public void UpdateIllegalItems(CYCAAdmissionViewModel vm, int Item_Found_Id, int userId, int handedbyId)
        {
                try
                {
                    CYCA_Admissions_IllegalItemsFound edit = db.CYCA_Admissions_IllegalItemsFound.Find(Item_Found_Id);


                    //Admission_Id = vm.Admission_Id;                  
                    edit.Description = vm.Item_Description;
                    edit.Quantity = vm.Quantity;
                    edit.Handed_By = Convert.ToInt32(handedbyId);
                    edit.Date_Last_Modified = DateTime.Now;
                    edit.Modified_By = db.Users.Find(userId).First_Name;

                    db.SaveChanges();
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
        #endregion

        #region ADMISSION DOCUMENTS
        public void AddDocument(CYCAAdmissionViewModel vm,int admissionId, int userid)
        {
                try
                {
                    CYCA_Admissions_Document newobj = new CYCA_Admissions_Document();

                    newobj.Admission_Id = admissionId;
                    newobj.Document_Id = vm.Document_Id;
                    //SaveToPhysicalLocation(newobj.Document_Name) = vm.Document_Name;
                    newobj.Document_Ext = vm.Document_Ext;
                    newobj.DateSaved = DateTime.Now;
                    newobj.TimeSaved = DateTime.Now.ToString("HH:mm");
                    newobj.Created_By = userid.ToString();
                    newobj.Is_Active = true;
                    newobj.Is_Deleted = false;

                    db.CYCA_Admissions_Document.Add(newobj);
                    db.SaveChanges();

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

        //private string SaveToPhysicalLocation(HttpPostedFileBase file)
        //{
        //    if (file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);

        //    }

        //    return string.Empty;
        //}
        #endregion


    

        //..........................................................................DROPDOWNS.........................................

        #region DROPDOWNS


        public List<AdmissionVenueLookUpCYCA> GetAllCycaVenues()
        {
              return  db.apl_Cyca_Venue.Select(o => new AdmissionVenueLookUpCYCA
                {
                  Venue_Id = o.Venue_Id,
                  VenueName = o.VenueName
                }).ToList();
        }


        public List<PCMAdmissionTypeLookup> GetAllAdmissionType()
        {
            return db.apl_Admission_Type.Select(o => new PCMAdmissionTypeLookup
                {
                   Admission_Type_Id = o.Admission_Type_Id,
                   Description = o.Description
                }).ToList();

        }

        public List<HasLegalLookupPCM> GetHasLegal()
        {
            return db.apl_PCM_Has_Legal_Representive.Select(o => new HasLegalLookupPCM
                {
                    HasLegal_Id = o.HasLegal_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<PCMOffenseSchedulesLookup> GetOffenceSchedules()
        {
            return db.apl_Offense_Schedules.Select(o => new PCMOffenseSchedulesLookup
                {
                    Offence_Schedule_Id = o.Offence_Schedule_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<PCMWhenEscapedLookup> GetEscapePeriod()
        {
            return db.apl_Escape_Period.Select(o => new PCMWhenEscapedLookup
                {
                    When_Escaped_Id = o.When_Escaped_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<PCMGenderLookup> GetAllGender()
        {
            return db.Genders.Select(o => new PCMGenderLookup
                {
                    Gender_Id = o.Gender_Id,
                    Description = o.Description
                }).ToList();
        }


        public List<PCMRequestStatusLookup> GetBedSpaceRequeststatus()
        {
            return db.apl_BedSpace_Request.Select(o => new PCMRequestStatusLookup
                {
                    Request_Status_Id = o.Request_Status_Id,
                    Description = o.Description
                }).ToList();
        }
        public List<PCMAdmissionTypeLookup> GetAdmissionType()
        {
            return db.apl_Admission_Type.Select(o => new PCMAdmissionTypeLookup
                {
                    Admission_Type_Id = o.Admission_Type_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<PCMChargeLookup> GetAllCharges()
        {
                var Type = db.apl_PCM_Charges.Select(o => new PCMChargeLookup
                {
                    Charge_Id = o.Charge_Id,
                    Charge_Description = o.Charge_Description

                }).ToList();

                var Type2 = Type.OrderBy(b => b.Charge_Description).ToList();

                return Type2;
        }

        public List<DiversionProgrammesLookupPcm> GetAllDiversion_Programmes()
        {
            return db.apl_Diversion_Programmes.Select(o => new DiversionProgrammesLookupPcm
                {
                    Div_Program_Id = o.Div_Program_Id,
                    Programme_Name = o.Programme_Name
                }).ToList();
        }
        public List<OrganisationTypeLookupPCM> GetAllOrganisationType()
        {
            return db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupPCM
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();
        }
        public List<LocalMunicipalityLookupAdopt> GetAllLocalMunicipality()
        {
            return db.Local_Municipalities.Select(o => new LocalMunicipalityLookupAdopt
                {
                    Local_Municipality_Id = o.Local_Municipality_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<TownLookupPCM> GetAllTowns()
        {
            return db.Towns.Select(o => new TownLookupPCM
                {
                    Town_Id = o.Town_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<OrganizationLookupPcm> GetAllPCMOrganisation()
        {
            return db.Organizations.Select(o => new OrganizationLookupPcm
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description
                }).ToList();
        }
        public List<RecomendationOrderLookupPcm> GetOrder()
        {
            return db.apl_PCM_Orders.Select(o => new RecomendationOrderLookupPcm
                {
                    Recomendation_Order_Id = o.Recomendation_Order_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<PlacementTypeLookupPcm> GetPlacementType()
        {
            return db.apl_Placement_Type.Select(o => new PlacementTypeLookupPcm
                {
                    Placement_Type_Id = o.Placement_Type_Id,
                    Description = o.Description
                }).ToList();
        }
        public List<RecommendationTypeLookupPcm> GetRecommendationType()
        {
            return db.apl_Recommendation_Type.Select(o => new RecommendationTypeLookupPcm
                {
                    Recommendation_Type_Id = o.Recommendation_Type_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<RelationshipTypeLookupPcm> GetAllRelationType()
        {
            return db.Relationship_Types.Select(o => new RelationshipTypeLookupPcm
                {
                    Relationship_Type_Id = o.Relationship_Type_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<PCMHealthStatusLookup> GetHealth()
        {
            return db.apl_Health_Status.Select(o => new PCMHealthStatusLookup
                {
                    Health_Status_Id = o.Health_Status_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<ProvinceLookupPCM> GetAllProvinces()
        {
            return db.Provinces.Select(o => new ProvinceLookupPCM
                {
                    Province_Id = o.Province_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<PCMOffenceTypeLookup> GetOffenceType()
        {
            return db.apl_Offence_Type.Select(o => new PCMOffenceTypeLookup
                {
                    Offence_Type_Id = o.Offence_Type_Id,
                    Description = o.Description
                }).ToList();
        }
        public List<PCMOffenceCategoryLookup> GetOffenceCategory()
        {
            return db.Offence_Categories.Select(o => new PCMOffenceCategoryLookup
                {
                    Offence_Category_Id = o.Offence_Category_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<DistrictLookupPCM> GetAllDistrictByCourtID(int DistrictId)
        {
            return db.Districts.Where(x => x.District_Id == DistrictId).ToList().Select(o => new DistrictLookupPCM
                {
                    District_Id = o.District_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<Place_Of_DetentionLookupPCM> GetAllPlaceOfDetention()
        {
            return db.apl_Place_Of_Detention.Select(o => new Place_Of_DetentionLookupPCM
                {
                    Place_Of_Detention_Id = o.Place_Of_Detention_Id,
                    Description = o.Description
                }).ToList();
        }
        public List<DistrictLookupPCM> GetAllDistrict()
        {
            return db.Districts.Select(o => new DistrictLookupPCM
                {
                    District_Id = o.District_Id,
                    Description = o.Description
                }).ToList();
        }
        public List<IdentificationTypeLookup> GetAllIdentificationType()
        {
            return db.Identification_Types.Select(o => new IdentificationTypeLookup
                {
                    Identification_Type_Id = o.Identification_Type_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<LanguageTypeLookup> GetAllLanguageType()
        {
            return db.Languages.Select(o => new LanguageTypeLookup
                {
                    Language_Id = o.Language_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<GenderTypeLookup> GetAllGenderType()
        {
            return db.Genders.Select(o => new GenderTypeLookup
                {
                    Gender_Id = o.Gender_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<MaritalTypeLookup> GetAllMaritalType()
        {
            return db.Marital_Statusses.Select(o => new MaritalTypeLookup
                {
                    Marital_Status_Id = o.Marital_Status_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<ContactTypeLookup> GetAllContactType()
        {
            return db.Preferred_Contact_Types.Select(o => new ContactTypeLookup
                {
                    Preferred_Contact_Type_Id = o.Preferred_Contact_Type_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<ReligionTypeLookup> GetAllReligionType()
        {
            return db.Religions.Select(o => new ReligionTypeLookup
                {
                    Religion_Id = o.Religion_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<Population_GroupTypeLookup> GetAllPopulationType()
        {
            return db.Population_Groups.Select(o => new Population_GroupTypeLookup
                {
                    Population_Group_Id = o.Population_Group_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<NationalityTypeLookup> GetAllNationalityType()
        {
            return db.Nationalities.Select(o => new NationalityTypeLookup
                {
                    Nationality_Id = o.Nationality_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<DisabilityTypeLookup> GetAllDisabilityType()
        {
            return db.Disabilities.Select(o => new DisabilityTypeLookup
                {
                    Disability_Type_Id = o.Disability_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<SAPSLookup> GetAllSAPStation()
        {
            return db.SAPS_Stations.Select(o => new SAPSLookup
                {
                    SAPS_Station_Id = o.SAPS_Station_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<SAPS_OfficialLookup> GetAllSAPSOfficial()
        {
            return db.SAPS_Officials.Select(o => new SAPS_OfficialLookup
                {
                    SAPS_Info_Id = o.SAPS_Official_Id,
                    Description = o.First_Name
                }).ToList();
        }

        public List<CourtLookup> GetAllCourt()
        {
            return db.Courts.Select(o => new CourtLookup
                {
                    Court_id = o.Court_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<TownTypeLookup> GetAllTownType()
        {
            return db.Towns.Select(o => new TownTypeLookup
                {
                    Town_Id = o.Town_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<NotificationTypeLookup> GetAllNotificationType()
        {
            return db.apl_Form_Of_Notification.Select(o => new NotificationTypeLookup
                {
                    Form_Of_Notification_Id = o.Form_Of_Notification_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<AdmissionVenueLookUpCYCA> GetAllVenues()
        {
            return db.apl_Cyca_Venue.Select(o => new AdmissionVenueLookUpCYCA
                {
                    Venue_Id = o.Venue_Id,
                    VenueName = o.VenueName
                }).ToList();
        }

        public List<ChronicTypeLookup> GetAllChronicType()
        {
            return db.Chronic_Illnesses.Select(o => new ChronicTypeLookup
                {
                    Chronic_Illness_Id = o.Chronic_Illness_Id,
                    Description = o.Description
                }).ToList();
        }

        public List<AllergyTypeLookup> GetAllAllergyType()
        {
            return db.Allergies.Select(o => new AllergyTypeLookup
                {
                    Allergy_Id = o.Allergy_Id,
                    Description = o.Description
                }).ToList();
        }

        #endregion

    }
}
