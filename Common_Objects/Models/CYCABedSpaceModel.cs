using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class CYCABedSpaceModel
    {
        //initialise private link to Entities
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

        public int GetPCMPersonIdByintAssId(int intAssessmentId)
        {
                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select p.Person_Id).SingleOrDefault();
        }

        public PCMPersonViewModel GetPCMPerson(int personId)
        {
            PCMPersonViewModel pcvm = new PCMPersonViewModel();

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
        public void UpdatePersonalDetails(PCMPersonViewModel person, string myStringuserId, int personId)
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
          return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id

                        where f.Client_Module_Id.Equals(ClientRefid) && i.Problem_Sub_Category_Id == 22
                        select f.Client_Module_Ref_No).FirstOrDefault();
        }

        //#region RECOMENDATIONS

        //public int GetPCMRecommendationByassId(int intAssessmentId)
        //{
        //    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        //    {

        //        return (from p in db.Persons
        //                join c in db.Clients on p.Person_Id equals c.Person_Id
        //                join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
        //                join Case in db.PCM_Recommendation on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
        //                where i.Intake_Assessment_Id.Equals(intAssessmentId)
        //                select Case.Recommendation_Id).FirstOrDefault();
        //    }
        //}

        //public PCMCaseDetailsViewModel GetRecomendationDetailsList(int RecommendationId)
        //{
        //    PCMCaseDetailsViewModel fvm = new PCMCaseDetailsViewModel();


        //    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        //    {
        //        try
        //        {
        //            PCM_Recommendation act = db.PCM_Recommendation.Find(RecommendationId);

        //            fvm.Recommendation_Id = act.Recommendation_Id;
        //            fvm.Recommendation_Type_Id = act.Recommendation_Type_Id;
        //            fvm.Placement_Type_Id = act.Placement_Type_Id;
        //            fvm.Comments_For_Recommendation = act.Comments_For_Recommendation;

        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //        {

        //            Exception raise = dbEx;
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string message = string.Format("{0}:{1}",
        //                        validationErrors.Entry.Entity.ToString(),
        //                        validationError.ErrorMessage);
        //                    // raise a new exception nesting
        //                    // the current instance as InnerException
        //                    raise = new InvalidOperationException(message, raise);
        //                }
        //            }
        //            throw raise;
        //        }

        //        return fvm;
        //    }
        //}

        ////public void CreatePCMRecomendationDeatils(PCMCaseDetailsViewModel cases, int intassid, int userId)
        ////{
        ////    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        ////    {
        ////        try
        ////        {

        ////            PCM_Recommendation newCase = new PCM_Recommendation();

        ////            newCase.Intake_Assessment_Id = intassid;
        ////            newCase.Recommendation_Type_Id = cases.Recommendation_Type_Id;
        ////            newCase.Placement_Type_Id = cases.Placement_Type_Id;
        ////            newCase.Comments_For_Recommendation = cases.Comments_For_Recommendation;
        ////            newCase.Created_By = userId;
        ////            newCase.Date_Created = DateTime.Now;
        ////            db.PCM_Recommendation.Add(newCase);

        ////            db.SaveChanges();
        ////        }
        ////        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        ////        {
        ////            Exception raise = dbEx;
        ////            foreach (var validationErrors in dbEx.EntityValidationErrors)
        ////            {
        ////                foreach (var validationError in validationErrors.ValidationErrors)
        ////                {
        ////                    string message = string.Format("{0}:{1}",
        ////                        validationErrors.Entry.Entity.ToString(),
        ////                        validationError.ErrorMessage);
        ////                    // raise a new exception nesting
        ////                    // the current instance as InnerException
        ////                    raise = new InvalidOperationException(message, raise);
        ////                }
        ////            }
        ////            throw raise;
        ////        }


        ////    }
        ////}

        //public void UpdatePCMRecomendationDetails(PCMCaseDetailsViewModel cases, int AssId, int userId)
        //{
        //    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        //    {
        //        try
        //        {
        //            PCM_Recommendation editCase = db.PCM_Recommendation.Find(AssId);

        //            if (AssId > 0)
        //            {


        //                editCase.Recommendation_Type_Id = cases.Recommendation_Type_Id;
        //                editCase.Placement_Type_Id = cases.Placement_Type_Id;
        //                editCase.Comments_For_Recommendation = cases.Comments_For_Recommendation;
        //                editCase.Date_Modified = DateTime.Now;
        //                editCase.Modified_By = userId;
        //            }

        //            db.SaveChanges();
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //        {

        //            Exception raise = dbEx;
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string message = string.Format("{0}:{1}",
        //                        validationErrors.Entry.Entity.ToString(),
        //                        validationError.ErrorMessage);
        //                    raise = new InvalidOperationException(message, raise);
        //                }
        //            }
        //            throw raise;
        //        }
        //    }
        //}
        //#endregion
        #region FACILITY BED SPACE REQUEST
      
        public List<CYCABedSpaceRequestViewModel> GetFacilityFormaloadList()
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();
            // get work list for user logged in
            var ListP = (
                  from p in db.Provinces
                  join d in db.apl_Cyca_Facility on p.Province_Id equals d.Province_Id

                  select new
                  {
                      p.Province_Id,
                      p.Description,
                      d.Facility_Id

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                obj.Facility_Id = item.Facility_Id;
                obj.ProvinceDescription = db.Provinces.Find(item.Province_Id).Description;
                obj.SelectFacility = db.apl_Cyca_Facility.Find(item.Facility_Id).FacilityName;
                obj.FacilityTell = db.apl_Cyca_Facility.Find(item.Facility_Id).FacilityTelNo;
                obj.Facilityemail = db.apl_Cyca_Facility.Find(item.Facility_Id).FacilityEmailAddress;
                obj.FacilityOfficialCapacity = db.apl_Cyca_Facility.Find(item.Facility_Id).OfficialCapacity;

                avm.Add(obj);
            }
            return avm;
        }

        public List<CYCABedSpaceRequestViewModel> GetFacilityList(int ProvinceID)
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();
            // get work list for user logged in
            var ListP = (
                  from p in db.Provinces
                  join d in db.apl_Cyca_Facility on p.Province_Id equals d.Province_Id
                  where p.Province_Id == (ProvinceID)
                  select new
                  {
                      p.Province_Id,
                      p.Description,
                      d.Facility_Id

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                obj.Facility_Id = item.Facility_Id;
                obj.ProvinceDescription = db.Provinces.Find(item.Province_Id).Description;
                obj.SelectFacility = db.apl_Cyca_Facility.Find(item.Facility_Id).FacilityName;
                obj.FacilityTell = db.apl_Cyca_Facility.Find(item.Facility_Id).FacilityTelNo;
                obj.Facilityemail = db.apl_Cyca_Facility.Find(item.Facility_Id).FacilityEmailAddress;
                obj.FacilityOfficialCapacity = db.apl_Cyca_Facility.Find(item.Facility_Id).OfficialCapacity;

                avm.Add(obj);
            }

            return avm;
        }

        public List<CYCABedSpaceRequestViewModel> GetMaleSpaceList(int fid)
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();
            // get work list for user logged in
            var ListMS = (
                  from a in db.Intake_Assessments
                  join i in db.CYCA_FacilityBedSpaceInbox_ReqOutBox on a.Intake_Assessment_Id equals i.Intake_Assessment_Id
                  join p in db.Provinces on i.ProvinceId equals p.Province_Id
                  join f in db.apl_Cyca_Facility on p.Province_Id equals f.Province_Id
                  join s in db.apl_Cyca_Male_BedSpace on f.Facility_Id equals s.Facility_Id               
                  where f.Facility_Id == (fid)

                  select new
                  {
                      s.Facility_Id,
                      s.Total_Space,
                      s.Available_Space,
                      s.Used_Space
                  }).ToList();


            ;
            foreach (var item in ListMS)
            {

                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                obj.Facility_Id = item.Facility_Id;
                obj.Male_Total_Space = item.Total_Space;
                obj.Male_Available_Space = item.Available_Space;
                obj.Male_Used_Space = item.Used_Space;

                avm.Add(obj);
            }

            return avm;
        }

        public List<CYCABedSpaceRequestViewModel> GetFemaleSpaceList(int fid)
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();
            // get work list for user logged in
            var ListMS = (
           from a in db.Intake_Assessments
           join i in db.CYCA_FacilityBedSpaceInbox_ReqOutBox on a.Intake_Assessment_Id equals i.Intake_Assessment_Id
           join p in db.Provinces on i.ProvinceId equals p.Province_Id
           join f in db.apl_Cyca_Facility on p.Province_Id equals f.Province_Id
           join s in db.apl_Cyca_Female_BedSpace on f.Facility_Id equals s.Facility_Id
           where f.Facility_Id == (fid)

           select new
           {
               s.Facility_Id,
               s.Total_Space,
               s.Available_Space,
               s.Used_Space
           }).ToList();

            ;
            foreach (var item in ListMS)
            {

                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                obj.Facility_Id = item.Facility_Id;
                obj.Female_Total_Space = item.Total_Space;
                obj.Female_Available_Space = item.Available_Space;
                obj.Female_Used_Space = item.Used_Space;


                avm.Add(obj);
            }

            return avm;
        }
        public List<apl_BedSpace_Request> GetBedStats()
        {
            return  db.apl_BedSpace_Request.ToList();
        }

        public List<apl_Cyca_BedSpaceRequest_Outcome> GetBedSpaceRequestOutcome()
        {
          return db.apl_Cyca_BedSpaceRequest_Outcome.ToList();
        }
        public List<CYCABedSpaceRequestViewModel> GetFacilityProgramList(int fid)
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();

            // get work list for user logged in
            var ListMS = (

                  from ppp in db.apl_Cyca_Facility_Programs
                  join ddd in db.apl_Cyca_Facility on ppp.Facility_Id equals ddd.Facility_Id
                  where ddd.Facility_Id == (fid)

                  select new
                  {

                      ppp.Program_Name,
                      ppp.Program_Description,
                      ppp.Program_StartDate,
                      ppp.Program_EndDate,
                      ppp.Program_Capacity
                  }).ToList();


            ;
            foreach (var item in ListMS)
            {

                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();


                obj.ProgramNames = item.Program_Name;
                obj.ProgramDescription = item.Program_Description;
                obj.ProgramStartDate = item.Program_StartDate;
                obj.ProgramEndDate = item.Program_EndDate;
                obj.ProgramCapacity = item.Program_Capacity;


                avm.Add(obj);
            }

            return avm;
        }

        public CYCABedSpaceRequestViewModel GetFacilityMailList(int falcilityid)
        {
            CYCABedSpaceRequestViewModel vm = new CYCABedSpaceRequestViewModel();

                try
                {

                    string email = (from i in db.apl_Cyca_Facility

                                    where i.Facility_Id == falcilityid
                                    select i.FacilityEmailAddress).FirstOrDefault();

                    int? AdmissionTypeId = (from i in db.PCM_Facility_Space_Inbox_ReqOutBox
                                            where i.Facility_Id == falcilityid
                                            select i.Admission_Type_Id).FirstOrDefault();

                    int? RequestStatusId = (from i in db.PCM_Facility_Space_Inbox_ReqOutBox
                                            where i.Facility_Id == falcilityid
                                            select i.Admission_Type_Id).FirstOrDefault();

                    apl_Cyca_Facility act = db.apl_Cyca_Facility.Find(falcilityid);

                    if (act != null)
                    {
                        vm.Facility_Id = act.Facility_Id;
                        vm.Facilityemail = act.FacilityEmailAddress;

                    }

                    if (AdmissionTypeId != null)
                    {
                        vm.Admission_Type_Id = AdmissionTypeId;
                        vm.selectAdmissionType = db.apl_Admission_Type.Find(AdmissionTypeId).Description;

                    }

                    if (RequestStatusId != null)
                    {
                        vm.Request_Status_Id = RequestStatusId;
                        vm.selectBedRequestStatus = db.apl_BedSpace_Request.Find(RequestStatusId).Description;

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

        
        #region GET FACILITY BED SPACE REQUEST LIST

        public int GetFacilityIdByUserID(int UserId)
        {
                //return (from p in db.Employees
                //        join c in db.Users on p.User_Id equals c.User_Id
                //        join i in db.Organizations on p.Organization_Id equals i.Organization_Id
                //        join f in db.apl_Cyca_Facility on i.Organization_Id equals f.Organization_Id
                //        where c.User_Id == (UserId)
                //        select i.Organization_Id).SingleOrDefault();
                return (from f in db.apl_Cyca_Facility
                        join e in db.Employees on f.Facility_Id equals e.Facility_Id
                        join u in db.Users on e.User_Id equals u.User_Id
                        where u.User_Id == UserId
                        select f.Facility_Id).SingleOrDefault();
        }
        public int GetFemaleBedspaceIdByFacilityId(int FacilityId)
        {
                return (from fb in db.apl_Cyca_Female_BedSpace
                        join f in db.apl_Cyca_Facility on fb.Facility_Id equals f.Facility_Id
                        where fb.Facility_Id == FacilityId
                        select fb.Female_Bed_Space_Id).SingleOrDefault();
        }
        public int GetFemaleAvailableSpaceByFacilityId(int fbId)
        {
                return (from fb in db.apl_Cyca_Female_BedSpace                   
                        where fb.Female_Bed_Space_Id == fbId
                        select fb.Available_Space).SingleOrDefault(); ;  
        }
        public int GetMaleAvailableSpaceByFacilityId(int fbId)
        {
                return (from fb in db.apl_Cyca_Male_BedSpace
                        where fb.Male_Bed_Space_Id == fbId
                        select fb.Available_Space).SingleOrDefault(); 
        }
        public List<CYCABedSpaceRequestViewModel> GetFacilitybedSpaceListByFacility(int FacilityId)
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();

            var ListP = (
                  from d in db.PCM_Facility_Space_Inbox_ReqOutBox
                  join f in db.apl_Cyca_Facility on d.Facility_Id equals f.Facility_Id
                  join u in db.Users on d.Sent_By equals u.User_Id
                  join p in db.Intake_Assessments on d.Intake_Assessment_Id equals p.Intake_Assessment_Id
                  join c in db.PCM_Case_Details on p.Intake_Assessment_Id equals c.Intake_Assessment_Id
                  join a in db.Courts on c.Court_id equals a.Court_Id
                  where d.Facility_Id == (FacilityId)
                  orderby d.Request_Id ascending                
                  select new 
                  {
                      p.Intake_Assessment_Id,
                      d.Date_Created,
                      d.Date_Recieved,
                      d.Time_Recieved,
                      d.Date_Closed,
                      d.Request_Open_Close,
                      d.Request_Id,
                      u.User_Id,
                      a.Court_Id,
                      d.Request_Status_Id,
                      d.Count_Declined,
                      d.Count_Accepted,
                      d.Date_Modified

                  }).ToList();
            ;
            foreach (var item in ListP)
            {
                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Request_Id = item.Request_Id;
                obj.selectPropationOfficer = db.Users.Find(item.User_Id).First_Name + " " + db.Users.Find(item.User_Id).First_Name;
                obj.courtName = db.Courts.Find(item.Court_Id).Description;
                obj.Date_Recieved = item.Date_Recieved;
                obj.Time_Recieved = item.Time_Recieved;
                obj.RequestOpenClose = item.Request_Open_Close;
                obj.Days_Lapsed = ((TimeSpan)(DateTime.Now - item.Date_Recieved)).Days;
                //obj.Hours_Lapsed = (DateTime.Now - item.Date_Created);
                obj.Request_Status_Id = item.Request_Status_Id;                
                obj.Date_Created = item.Date_Created;
                obj.Count_Declined = item.Count_Declined;
                obj.Count_Accepted = item.Count_Accepted;
                DateTime lastDate =Convert.ToDateTime(item.Date_Recieved);
                TimeSpan difference = DateTime.Now - lastDate;
                int days = difference.Days;
                int hours = difference.Hours;
                int minutes = difference.Minutes;
                obj.Hours_Lapsed = hours;

                obj.Date_Modified = item.Date_Modified;
                if (item.Date_Modified !=null)
                {
                    DateTime DateModified = Convert.ToDateTime(item.Date_Modified);
                    TimeSpan timemodified = DateTime.Now - DateModified;
                    int hoursModified = timemodified.Hours;
                    obj.Hours_Modified = hoursModified;
                }
                else
                {
                    obj.Hours_Modified = 0;
                }
               
               



                avm.Add(obj);
            }

            return avm;
        }
        public List<CYCABedSpaceRequestViewModel> GetFacilitybedSpaceList(int IntakeassId)
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();

            // get work list for user logged in

            var ListP = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Facility_Space_Inbox_ReqOutBox on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join z in db.apl_BedSpace_Request on d.Request_Status_Id equals z.Request_Status_Id
                  join f in db.apl_Cyca_Facility on d.Facility_Id equals f.Facility_Id
                  join u in db.Users on d.Sent_By equals u.User_Id
                  join c in db.PCM_Case_Details on p.Intake_Assessment_Id equals c.Intake_Assessment_Id
                  join a in db.Courts on c.Court_id equals a.Court_Id
                  where p.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,  
                      
                      d.Date_Created,
                      d.Date_Recieved,
                      d.Time_Recieved,
                      d.Date_Closed,
                      d.Request_Open_Close,
                      d.Request_Id,
                      u.User_Id,
                      a.Court_Id,
                      d.Request_Status_Id
                      
                      
                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Request_Id = item.Request_Id;
                obj.selectPropationOfficer = db.Users.Find(item.User_Id).First_Name + " " + db.Users.Find(item.User_Id).First_Name;
                obj.courtName = db.Courts.Find(item.Court_Id).Description;
                obj.Date_Recieved = item.Date_Recieved;
                obj.Time_Recieved = item.Time_Recieved;
                obj.RequestOpenClose = item.Request_Open_Close;
                obj.Days_Lapsed = ((TimeSpan)(DateTime.Now - item.Date_Recieved)).Days;
                obj.Request_Status_Id = item.Request_Status_Id;



                avm.Add(obj);
            }

            return avm;
        }
        public List<CYCABedSpaceRequestViewModel> GetBedSpaceRequestDetails(int RequestId)
        {
            List<CYCABedSpaceRequestViewModel> cvm = new List<CYCABedSpaceRequestViewModel>();

            var reqList = (
                from s in db.Intake_Assessments
                join d in db.PCM_Facility_Space_Inbox_ReqOutBox on s.Intake_Assessment_Id equals d.Intake_Assessment_Id
                join u in db.Users on d.Sent_By equals u.User_Id
                join e in db.Employees on u.User_Id equals e.Employee_Id
                join p in db.Provinces on d.Province_Id equals p.Province_Id
                join c in db.PCM_Case_Details on s.Intake_Assessment_Id equals c.Intake_Assessment_Id
                join a in db.Courts on c.Court_id equals a.Court_Id
                join x in db.Clients on s.Intake_Assessment_Id equals x.Client_Id
                join z in db.Persons on x.Client_Id equals z.Person_Id
                join b in db.apl_BedSpace_Request on d.Request_Status_Id equals b.Request_Status_Id
                //join g in db.Genders on z.Gender_Id equals g.Gender_Id
                where d.Request_Id == (RequestId)
                select new
                {
                    d.Request_Id,
                    u.User_Id,
                    e.Employee_Id,
                    p.Province_Id,
                    a.Court_Id,
                    z.Person_Id,
                    b.Request_Status_Id
                    //g.Gender_Id

                }).ToList();

            foreach (var item in reqList)
            {
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                //Probation Officer Details.....................................................
                obj.Request_Id = item.Request_Id;
                obj.selectProbationOfficerName = db.Users.Find(item.User_Id).First_Name;
                obj.selectProbationOfficerSurname = db.Users.Find(item.User_Id).Last_Name;
                obj.selectProbationOfficerTel = db.Employees.Find(item.Employee_Id).Phone_Number;
                obj.selectProbationOfficerEmail = db.Users.Find(item.User_Id).Email_Address;
                obj.selectProvince = db.Provinces.Find(item.Province_Id).Description;

                //.............................................................................

                // Client Details..........................................................................

                obj.courtName = db.Courts.Find(item.Court_Id).Description;
                obj.selectClientName = db.Persons.Find(item.Person_Id).First_Name + " " + db.Persons.Find(item.Person_Id).Last_Name;
                obj.selectClientGender = db.Persons.Find(item.Person_Id).Gender_Description;
                obj.Request_Status_Id = item.Request_Status_Id;
                obj.selectBedRequestStatus = db.apl_BedSpace_Request.Find(item.Request_Status_Id).Description;


                cvm.Add(obj);
            }
            return cvm;
        }

        //PUPULATE RESPOND TO BED SPACE REQUEST
        public CYCABedSpaceRequestViewModel GetRequestById(int RequestId, int userId)
        {
            CYCABedSpaceRequestViewModel vm = new CYCABedSpaceRequestViewModel();

                try
                {

                    PCM_Facility_Space_Inbox_ReqOutBox act = db.PCM_Facility_Space_Inbox_ReqOutBox.Find(RequestId);

                    if (act != null)
                    {
                        //Get Request Details
                        vm.Request_Status_Id = act.Request_Status_Id;
                        vm.Request_Id = RequestId;
                        vm.selectProbationOfficerName = db.Users.Find(act.Sent_By).First_Name;
                        vm.selectProbationOfficerSurname = db.Users.Find(act.Sent_By).Last_Name;
                        vm.selectProbationOfficerTel = (from s in db.Intake_Assessments
                                                        join d in db.PCM_Facility_Space_Inbox_ReqOutBox on s.Intake_Assessment_Id equals d.Intake_Assessment_Id
                                                        join u in db.Users on d.Sent_By equals u.User_Id
                                                        join e in db.Employees on u.User_Id equals e.Employee_Id
                                                        where d.Request_Id == RequestId
                                                        select e.Phone_Number).SingleOrDefault();
                        vm.selectProbationOfficerEmail = db.Users.Find(act.Sent_By).Email_Address;
                        vm.selectProvince = (from s in db.Intake_Assessments
                                             join d in db.PCM_Facility_Space_Inbox_ReqOutBox on s.Intake_Assessment_Id equals d.Intake_Assessment_Id
                                             join c in db.PCM_Case_Details on s.Intake_Assessment_Id equals c.Intake_Assessment_Id
                                             join a in db.Courts on c.Court_id equals a.Court_Id
                                             join Di in db.Districts on a.District_Id equals Di.District_Id
                                             join p in db.Provinces on Di.Province_Id equals p.Province_Id
                                             where d.Request_Id == RequestId
                                             select p.Description).SingleOrDefault();
                        vm.courtName = (from s in db.Intake_Assessments
                                             join d in db.PCM_Facility_Space_Inbox_ReqOutBox on s.Intake_Assessment_Id equals d.Intake_Assessment_Id
                                             join c in db.PCM_Case_Details on s.Intake_Assessment_Id equals c.Intake_Assessment_Id
                                             join a in db.Courts on c.Court_id equals a.Court_Id
                                            where d.Request_Id == RequestId
                                            select a.Description).SingleOrDefault();
                        vm.selectClientName=(from s in db.Intake_Assessments
                                             join d in db.PCM_Facility_Space_Inbox_ReqOutBox on s.Intake_Assessment_Id equals d.Intake_Assessment_Id
                                             join c in db.PCM_Case_Details on s.Intake_Assessment_Id equals c.Intake_Assessment_Id
                                             join x in db.Clients on s.Client_Id equals x.Client_Id
                                             join z in db.Persons on x.Person_Id equals z.Person_Id
                                             where d.Request_Id == RequestId
                                             select z.First_Name).SingleOrDefault();
                       vm.selectClientGender = (from s in db.Intake_Assessments
                                               join d in db.PCM_Facility_Space_Inbox_ReqOutBox on s.Intake_Assessment_Id equals d.Intake_Assessment_Id
                                               join x in db.Clients on s.Client_Id equals x.Client_Id
                                               join z in db.Persons on x.Person_Id equals z.Person_Id
                                               join g in db.Genders on z.Gender_Id equals g.Gender_Id
                                               where d.Request_Id == RequestId
                                               select g.Description).SingleOrDefault();
                        vm.selectOffenceDetails = (from s in db.Intake_Assessments
                                                  join d in db.PCM_Facility_Space_Inbox_ReqOutBox on s.Intake_Assessment_Id equals d.Intake_Assessment_Id
                                                  join c in db.PCM_Case_Details on s.Intake_Assessment_Id equals c.Intake_Assessment_Id
                                                  join n in db.PCM_Offence_Details on c.PCM_Case_Id equals n.PCM_Case_Id
                                                  join h in db.apl_Offense_Schedules on n.Offence_Schedule_Id equals h.Offence_Schedule_Id
                                                  join t in db.apl_Offence_Type on n.Offence_Type_Id equals t.Offence_Type_Id
                                                  where d.Request_Id == RequestId
                                                  select t.Description).SingleOrDefault();
                        vm.selectRequestStatus = db.PCM_Facility_Space_Inbox_ReqOutBox.Find(act.Request_Id).Request_Open_Close;
                        vm.selectAdmissionType = db.apl_Admission_Type.Find(act.Admission_Type_Id).Description;


                        //Get Facility Request Details
                        vm.selectFacilityName = db.apl_Cyca_Facility.Find(act.Facility_Id).FacilityName;
                        vm.selectFaciltyTel = db.apl_Cyca_Facility.Find(act.Facility_Id).FacilityTelNo;                     
                        vm.selectFacilityManagerEmail = (from f in db.apl_Cyca_Facility
                                                         join e in db.Employees on f.Facility_Id equals e.Facility_Id
                                                         join u in db.Users on e.User_Id equals u.User_Id
                                                         from r in u.Roles
                                                         where r.Role_Id == 33 && e.User_Id == userId
                                                         select u.Email_Address).SingleOrDefault();
                        vm.Date_Closed = DateTime.Today.ToString("yyyy/MM/dd");
                        vm.Time_Replied = DateTime.Now.ToString("HH:mm");
                        vm.AvailableFemaleSpace = (from f in db.apl_Cyca_Female_BedSpace 
                                                   where f.Facility_Id == act.Facility_Id
                                                   select f.Available_Space).SingleOrDefault();
                        vm.AvailableMaleSpace = (from m in db.apl_Cyca_Male_BedSpace
                                                 where m.Facility_Id == act.Facility_Id
                                                 select m.Available_Space).SingleOrDefault();
                        vm.Request_Comments = db.PCM_Facility_Space_Inbox_ReqOutBox.Find(act.Request_Id).Request_Comments;

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

        #endregion

        #region UPDATE BED SPACE ACCEPT/DECLINE
        public void UpdateFacilitybedSpaceAcceptDetails(CYCABedSpaceRequestViewModel vm, int Request_Id, int userId, int reid)
        {                              
                try
                {
                    PCM_Facility_Space_Inbox_ReqOutBox edit = db.PCM_Facility_Space_Inbox_ReqOutBox.Find(Request_Id);                  
                    edit.Request_Status_Id =Convert.ToInt32( reid);
                    edit.Reply_Date = DateTime.Today;
                    edit.Reply_Time = DateTime.Now;
                    edit.Modified_By = userId;
                    edit.Date_Modified = DateTime.Now;
                    if (edit.Count_Accepted == null || edit.Count_Declined == null)
                    {
                        edit.Count_Accepted = 0;
                        edit.Count_Declined = 0;
                    }
                    if (reid !=1)
                    {
                        edit.Request_Open_Close = "Closed";
                        if (reid == 2)
                        {
                            edit.Count_Accepted =+ 1;
                        }
                        else
                        {
                        if (edit.Count_Declined != 2)
                        {
                            edit.Count_Declined = edit.Count_Declined + 1;
                            edit.Request_Comments = vm.Request_Comments;
                        }
                           
                        }
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
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }  
        }


        public void UpdateFemaleMaleSpaceCapacity(CYCABedSpaceRequestViewModel vm, int FacilityId, int userId, int fbId, int reid)
        {
                vm.TotalFemaleBedSpace = (from f in db.apl_Cyca_Female_BedSpace
                                          where f.Facility_Id == FacilityId
                                          select f.Total_Space).SingleOrDefault();
                vm.TotalMalesBedSpace = (from m in db.apl_Cyca_Male_BedSpace
                                         where m.Facility_Id == FacilityId
                                         select m.Total_Space).SingleOrDefault();
                vm.AvailableFemaleSpace = (from f in db.apl_Cyca_Female_BedSpace
                                           where f.Facility_Id == FacilityId
                                           select f.Available_Space).SingleOrDefault();
                vm.AvailableMaleSpace = (from m in db.apl_Cyca_Male_BedSpace
                                         where m.Facility_Id == FacilityId
                                         select m.Available_Space).SingleOrDefault();

                try
                {                   
                    apl_Cyca_Male_BedSpace editMaleSpace = db.apl_Cyca_Male_BedSpace.Find(fbId);
                    apl_Cyca_Female_BedSpace editFemaleSpace = db.apl_Cyca_Female_BedSpace.Find(fbId);
              
                    string Gender = vm.selectClientGender;           
                    if (reid ==2)
                    {
                        if (Gender == "Male")
                        {
                            if (Convert.ToInt32(vm.AvailableMaleSpace) > 0)
                            {
                                int used_space = Convert.ToInt32(editMaleSpace.Used_Space) + 1;
                                editMaleSpace.Used_Space = used_space;

                                int aval_space = Convert.ToInt32(vm.TotalMalesBedSpace) - Convert.ToInt32(editMaleSpace.Used_Space);
                                editMaleSpace.Available_Space = aval_space;
                            }                                                      
                        }
                        else
                        {
                            if (Convert.ToInt32(vm.AvailableFemaleSpace) > 0)
                            {
                                int used_space = Convert.ToInt32(editFemaleSpace.Used_Space) + 1;
                                editFemaleSpace.Used_Space = used_space;

                                int aval_space = Convert.ToInt32(vm.TotalFemaleBedSpace) - Convert.ToInt32(editFemaleSpace.Used_Space);
                                editFemaleSpace.Available_Space = aval_space;
                            }
                           
                        }

                    }
                    else
                    {
                       
                        editFemaleSpace.Used_Space = editFemaleSpace.Used_Space;
                        editFemaleSpace.Available_Space = editFemaleSpace.Available_Space;

                        editMaleSpace.Used_Space = editMaleSpace.Used_Space;
                        editMaleSpace.Available_Space = editMaleSpace.Available_Space;
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
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
        }
        #endregion

        public CYCABedSpaceRequestViewModel GetFacilitybedSpaceOnEditDetails(int Request_Id)
        {
            CYCABedSpaceRequestViewModel vm = new CYCABedSpaceRequestViewModel();

                try
                {

                    PCM_Facility_Space_Inbox_ReqOutBox act = db.PCM_Facility_Space_Inbox_ReqOutBox.Find(Request_Id);

                    if (act != null)
                    {
                        vm.Request_Id = act.Request_Id;
                        vm.Facility_Id = act.Facility_Id;
                        vm.Request_Status_Id = act.Request_Status_Id;
                        vm.Admission_Type_Id = act.Admission_Type_Id;
                        vm.Request_Comments = act.Request_Comments;
                        vm.Date_Created = act.Date_Created;
                        vm.selectBedRequestStatus = db.apl_BedSpace_Request.Find(act.Request_Status_Id).Description;
                        vm.selectAdmissionType = db.apl_Admission_Type.Find(act.Admission_Type_Id).Description;
                        vm.SelectFacility = db.apl_Cyca_Facility.Find(act.Facility_Id).FacilityName;
                        //vm.Created_By = db.Clients.Find(act.)
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

        public void CreateFacilitybedSpaceDeatils(CYCABedSpaceRequestViewModel vm, int caseid, int userId)
        {
             try
                {
                    PCM_Facility_Space_Inbox_ReqOutBox newob = new PCM_Facility_Space_Inbox_ReqOutBox();
                    newob.Facility_Id = vm.Facility_Id;
                    newob.Request_Status_Id = vm.Request_Status_Id;
                    newob.Admission_Type_Id = vm.Admission_Type_Id;
                    newob.Request_Comments = vm.Request_Comments;
                    newob.Intake_Assessment_Id = caseid;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;
                    db.PCM_Facility_Space_Inbox_ReqOutBox.Add(newob);
                    db.SaveChanges();
                }
                catch
                {

                }
        }

        //public void UpdateFacilitybedSpaceAcceptDetails(CYCABedSpaceRequestViewModel vm, int Id, int userId, string acceptdecline)
        //{
        //    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        //    {
        //        try
        //        {
        //            PCM_Facility_Space_Inbox_ReqOutBox edit = db.PCM_Facility_Space_Inbox_ReqOutBox.Find(Id);
        //            edit.Request_Id = Id;

        //            if (acceptdecline == "Accept")
        //            {
        //                edit.Request_Status_Id = 3;
        //            }
        //            else if (acceptdecline == "Decline")
        //            {
        //                edit.Request_Status_Id = 5;
        //            }
        //            edit.Modified_By = userId;
        //            edit.Date_Modified = DateTime.Now;

        //            db.SaveChanges();
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //        {

        //            Exception raise = dbEx;
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string message = string.Format("{0}:{1}",
        //                        validationErrors.Entry.Entity.ToString(),
        //                        validationError.ErrorMessage);
        //                    raise a new exception nesting
        //                    the current instance as InnerException
        //                    raise = new InvalidOperationException(message, raise);
        //                }
        //            }
        //            throw raise;
        //        }
        //    }
        //} 

        #region GET ARCHIVES
        public List<CYCABedSpaceRequestViewModel>GetArchivesList(int IntakeassId)
        {
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();
            // get work list for user logged in
            var ListP = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Facility_Space_Inbox_ReqOutBox on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join z in db.apl_BedSpace_Request on d.Request_Status_Id equals z.Request_Status_Id
                  join f in db.apl_Cyca_Facility on d.Facility_Id equals f.Facility_Id
                  join u in db.Users on d.Sent_By equals u.User_Id
                  join c in db.PCM_Case_Details on p.Intake_Assessment_Id equals c.Intake_Assessment_Id
                  join a in db.Courts on c.Court_id equals a.Court_Id
                  where d.Request_Open_Close == "Closed"             
                  select new
                  {
                      p.Intake_Assessment_Id,

                      d.Date_Created,
                      d.Date_Recieved,
                      d.Time_Recieved,
                      d.Date_Closed,
                      d.Request_Open_Close,
                      d.Request_Id,
                      u.User_Id,
                      a.Court_Id,

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Request_Id = item.Request_Id;
                obj.selectPropationOfficer = db.Users.Find(item.User_Id).First_Name + " " + db.Users.Find(item.User_Id).First_Name;
                obj.courtName = db.Courts.Find(item.Court_Id).Description;
                obj.Date_Recieved = item.Date_Recieved;
                obj.Time_Recieved = item.Time_Recieved;
                obj.RequestOpenClose = item.Request_Open_Close;
                obj.Days_Lapsed = ((TimeSpan)(DateTime.Now - item.Date_Recieved)).Days;



                avm.Add(obj);
            }

            return avm;
        }
        #endregion


        #endregion


        #region PROVINCIAL COORDINATOR
        public List<CYCABedSpaceRequestViewModel> GetBedSpaceListDeclinedByCenterManager()
        {
            // initialising view model 
            List<CYCABedSpaceRequestViewModel> avm = new List<CYCABedSpaceRequestViewModel>();
            // initialise connection           
            var ListP = (
                  from d in db.PCM_Facility_Space_Inbox_ReqOutBox                      
                  join f in db.apl_Cyca_Facility on d.Facility_Id equals f.Facility_Id
                  join u in db.Users on d.Sent_By equals u.User_Id
                  join p in db.Intake_Assessments on d.Intake_Assessment_Id equals p.Intake_Assessment_Id
                  join c in db.PCM_Case_Details on p.Intake_Assessment_Id equals c.Intake_Assessment_Id
                  join a in db.Courts on c.Court_id equals a.Court_Id
                  where d.Request_Open_Close == "Open" || d.Request_Status_Id == 3

                  //Need to link users to facility
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.Date_Created,
                      d.Date_Recieved,
                      d.Time_Recieved,
                      d.Date_Closed,
                      d.Request_Open_Close,
                      d.Request_Id,
                      u.User_Id,
                      a.Court_Id,
                      d.Request_Status_Id

                  }).ToList();
            ;
            foreach (var item in ListP)
            {
                // initialising view model
                CYCABedSpaceRequestViewModel obj = new CYCABedSpaceRequestViewModel();
                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Request_Id = item.Request_Id;
                obj.selectPropationOfficer = db.Users.Find(item.User_Id).First_Name + " " + db.Users.Find(item.User_Id).First_Name;
                obj.courtName = db.Courts.Find(item.Court_Id).Description;
                obj.Date_Recieved = item.Date_Recieved;
                obj.Time_Recieved = item.Time_Recieved;
                obj.RequestOpenClose = item.Request_Open_Close;
                obj.Days_Lapsed = ((TimeSpan)(DateTime.Now - item.Date_Recieved)).Days;
                //obj.Hours_Lapsed = (DateTime.Now - item.Date_Created);
                obj.Request_Status_Id = item.Request_Status_Id;
                obj.Date_Created = item.Date_Created;

                DateTime lastDate = Convert.ToDateTime(item.Date_Recieved);
                TimeSpan difference = DateTime.Now - lastDate;
                int days = difference.Days;
                int hours = difference.Hours;
                int minutes = difference.Minutes;
                obj.Hours_Lapsed = hours;
                avm.Add(obj);
            }

            return avm;
        }
        #endregion
        #region DROPDOWNS


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
        public List<apl_Admission_Type> GetAdmissionTypeList()
        {
          return  db.apl_Admission_Type.ToList()
;        }

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
        public List<Province> GetAllProvincesFullInfo()
        {
            return db.Provinces.ToList();
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
