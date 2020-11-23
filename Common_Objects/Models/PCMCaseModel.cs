using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class PCMCaseModel
    { 

        public PCM_Case_Details GetSpecificPCMCase(int caseId) 
        {
            PCM_Case_Details pcmcase;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var pcmcaseList = (from r in dbContext.PCM_Case_Details
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

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var intakeAssessmentList = (from r in dbContext.Intake_Assessments
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
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select p.Person_Id).SingleOrDefault();
            }
        }

        public PCMPersonViewModel GetPCMPerson(int personId)
        {
            PCMPersonViewModel pcvm = new PCMPersonViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

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
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
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

        }
        //..........................................................................ASSSESSMENT REGISTER.........................................

        public int GetClientRefIdssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select f.Client_Module_Id).FirstOrDefault();
            }
        }
        public string GetClientRef(int ClientRefid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id

                        where f.Client_Module_Id.Equals(ClientRefid) && i.Problem_Sub_Category_Id ==22 
                        select f.Client_Module_Ref_No).FirstOrDefault();
            }
        }

        #region PCM CASE DETAILS

        public int GetPCMChildCaseIdDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.PCM_Case_Id).FirstOrDefault();
            }
        }

        public PCMCaseDetailsViewModel GetPCMCaseDetailsByPCMCaseId(int PcmCaseID1)
        {
            PCMCaseDetailsViewModel Casevm = new PCMCaseDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? CaseId = (from i in db.PCM_Case_Details
                                   where i.PCM_Case_Id == PcmCaseID1
                                   select i.PCM_Case_Id).FirstOrDefault();

                    PCM_Case_Details pcd = db.PCM_Case_Details.Find(CaseId);
                    Casevm.Date_Arrested = pcd.Date_Arrested;
                    Casevm.Time_Arrested = pcd.Time_Arrested;
                    Casevm.Arresting_Officer_Name = pcd.Arresting_Officer_Name;
                    Casevm.Investigate_Officer_Name = pcd.Investigate_Officer_Name;
                    Casevm.SDIISSAPS_Station_Id = pcd.SAPS_Station_Id;
                    Casevm.SAPS_Info_Id = pcd.SAPS_Info_Id;
                    Casevm.Detention_Place_Id = pcd.Detention_Place_Id;
                    Casevm.Court_Id = pcd.Court_id;
                    Casevm.CAS_No = pcd.CAS_No;
                    Casevm.First_Appear_Date = pcd.First_Appear_Date;
                    Casevm.Num_Of_Days_Custody = pcd.Num_Of_Days_Custody;
                    Casevm.HasLegal_Id = pcd.Has_Legal_Representitive;
                    Casevm.Name_Oflegal_Representitive = pcd.Name_Oflegal_Representitive;
                    Casevm.Is_Active = true;
                    //cases.Modified_By = userId;
                    Casevm.Date_Modified = DateTime.Now;
                    //cases.Modified_By = userId;
                    Casevm.Date_Created = DateTime.Now;
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
                return Casevm;
            }
        }

        public void InsertPCMCase(PCMCaseDetailsViewModel cases, int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Case_Details newCase = new PCM_Case_Details();

                    //PCMCaseDetailsViewModel cases = new PCMCaseDetailsViewModel();

                    //newCase.Intake_Assessment_Id = id;

                    newCase.Intake_Assessment_Id = id;
                    newCase.Client_Module_Id = cases.Client_Module_Id;
                    newCase.Date_Arrested = cases.Date_Arrested;
                    newCase.Time_Arrested = cases.Time_Arrested;
                    newCase.Arresting_Officer_Name = cases.Arresting_Officer_Name;
                    newCase.Investigate_Officer_Name = cases.Investigate_Officer_Name;
                    newCase.SAPS_Station_Id = cases.SDIISSAPS_Station_Id;
                    newCase.Detention_Place_Id = cases.Detention_Place_Id;
                    newCase.Court_id = cases.Court_Id;
                    newCase.CAS_No = cases.CAS_No;
                    newCase.First_Appear_Date = cases.First_Appear_Date;
                    newCase.Num_Of_Days_Custody = cases.Num_Of_Days_Custody;
                    newCase.Has_Legal_Representitive = cases.HasLegal_Id;
                    newCase.Name_Oflegal_Representitive = cases.Name_Oflegal_Representitive;
                    newCase.Is_Active = true;
                    newCase.Is_Deleted = false;
                    //newCase.Modified_By = userId;
                    newCase.Date_Modified = DateTime.Now;
                    //newCase.Modified_By = userId;
                    newCase.Date_Created = DateTime.Now;

                    db.PCM_Case_Details.Add(newCase);
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
            //return newCase;

        }

        public int GetCaseID(int caseid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var caseID = db.PCM_Case_Details.Where(c => c.PCM_Case_Id.Equals(caseid));
                if (caseID.Any())
                    return caseID.FirstOrDefault().PCM_Case_Id;
            }
            return 0;
        }

        public PCMCaseDetailsViewModel GetPCMCase(int cid)
        {
            PCMCaseDetailsViewModel cases = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int caseID = (from CD in db.PCM_Case_Details
                                  where CD.Intake_Assessment_Id == cid
                                  select CD.PCM_Case_Id).FirstOrDefault();
                    if (caseID != 0)
                    {

                        PCM_Case_Details pcd = db.PCM_Case_Details.Find(caseID);

                        cases.PCM_Case_Id = cid;
                        cases.Date_Arrested = pcd.Date_Arrested;
                        cases.Time_Arrested = pcd.Time_Arrested;
                        cases.Arresting_Officer_Name = pcd.Arresting_Officer_Name;
                        cases.Investigate_Officer_Name = pcd.Investigate_Officer_Name;
                        cases.SDIISSAPS_Station_Id = pcd.SAPS_Station_Id;
                        cases.SAPS_Info_Id = pcd.SAPS_Info_Id;
                        cases.Detention_Place_Id = pcd.Detention_Place_Id;
                        cases.Court_Id = pcd.Court_id;
                        cases.CAS_No = pcd.CAS_No;
                        cases.First_Appear_Date = pcd.First_Appear_Date;
                        cases.Num_Of_Days_Custody = pcd.Num_Of_Days_Custody;
                        cases.HasLegal_Id = pcd.Has_Legal_Representitive;
                        cases.Name_Oflegal_Representitive = pcd.Name_Oflegal_Representitive;
                        cases.Is_Active = true;
                        //cases.Modified_By = userId;
                        cases.Date_Modified = DateTime.Now;
                        //cases.Modified_By = userId;
                        cases.Date_Created = DateTime.Now;

                        //return cases;
                    }
                    else
                    {

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
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

            return cases;
        }

        public int GetUserID(string loginName)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from a in db.Users
                        where a.User_Name == loginName
                        select a.User_Id).FirstOrDefault();
            }

        }

        public void UpdatePCMCase(PCMCaseDetailsViewModel cases, int userId, int AssId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    int? CaseId = (from i in db.PCM_Case_Details
                                   where i.Intake_Assessment_Id == AssId
                                   select i.PCM_Case_Id).FirstOrDefault();
                    PCM_Case_Details pcd = db.PCM_Case_Details.Find(CaseId);

                    //cases.PCM_Case_Id = caseID;
                    pcd.Date_Arrested = cases.Date_Arrested;
                    pcd.Time_Arrested = cases.Time_Arrested;
                    pcd.Arresting_Officer_Name = cases.Arresting_Officer_Name;
                    pcd.Investigate_Officer_Name = cases.Investigate_Officer_Name;
                    pcd.SAPS_Station_Id = cases.SDIISSAPS_Station_Id;
                    pcd.Detention_Place_Id = cases.Detention_Place_Id;
                    pcd.Court_id = cases.Court_Id;
                    pcd.CAS_No = cases.CAS_No;
                    pcd.First_Appear_Date = cases.First_Appear_Date;
                    pcd.Num_Of_Days_Custody = cases.Num_Of_Days_Custody;
                    pcd.Has_Legal_Representitive = cases.HasLegal_Id;
                    pcd.Name_Oflegal_Representitive = cases.Name_Oflegal_Representitive;
                    pcd.Is_Active = true;
                    pcd.Modified_By = userId;
                    pcd.Date_Modified = DateTime.Now;
                    pcd.Modified_By = userId;
                    pcd.Date_Created = DateTime.Now;
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
        }



        #endregion

        #region ASSESSMENT REGISTER

        public int GePCMAssessmentByPcmCaeId(int IntAss)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(IntAss)
                        select Case.PCM_Case_Id).FirstOrDefault();
            }
        }
        public List<PCMCaseDetailsViewModel> GetAssmentRegisterList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in

            var ListV = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Assessment_Register on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join e in db.apl_Form_Of_Notification on d.Form_Of_Notification_Id equals e.Form_Of_Notification_Id
                  where d.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.Form_Of_Notification_Id,
                      d.Assessment_Date,
                      d.Assessment_Time,
                      d.Assesment_Register_Id,
                  }).ToList();
            ;
            foreach (var item in ListV)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Assessment_Time = item.Assessment_Time;
                obj.Assessment_Date = item.Assessment_Date;
                obj.Assesment_Register_Id = item.Assesment_Register_Id;
                obj.FormOfNotificationDescription = db.apl_Form_Of_Notification.Find(item.Form_Of_Notification_Id).Description;


                avm.Add(obj);
            }

            return avm;
        }


        public PCMCaseDetailsViewModel GetPCMAssessmentRegDetails(int PCMAssRegId)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Assessment_Register act = db.PCM_Assessment_Register.Find(PCMAssRegId);
                    if (act != null)
                    {
                        vm.Assesment_Register_Id = act.Assesment_Register_Id;
                        vm.Assessment_Date = act.Assessment_Date;
                        vm.Assessment_Time= act.Assessment_Time;
                        vm.Form_Of_Notification_Id = act.Form_Of_Notification_Id;
                        vm.FormOfNotificationDescription = db.apl_Form_Of_Notification.Find(act.Form_Of_Notification_Id).Description;
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
        }
        public void CreatePCMAssesmentRegister(PCMCaseDetailsViewModel pcmCDVMAdd, int id, int userID)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Assessment_Register newAssReg = new PCM_Assessment_Register();
                    //newAssReg.PCM_Case_Id = pcmCDVMAdd.PCM_Case_Id;
                    //newAssReg.Assessed_By = userID;
                    newAssReg.Intake_Assessment_Id = id;
                    newAssReg.Assessment_Date =pcmCDVMAdd.Assessment_Date;
                    newAssReg.Assessment_Time = pcmCDVMAdd.Assessment_Time;
                    newAssReg.Form_Of_Notification_Id = pcmCDVMAdd.Form_Of_Notification_Id;
                    newAssReg.CreatedBy = userID;
                    newAssReg.Date_Created = DateTime.Now;

                    db.PCM_Assessment_Register.Add(newAssReg);
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
        }

        public void UpdatePCMAssesmentRegister(PCMCaseDetailsViewModel cases, int Assid , int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    //int AssREGId = (from i in db.PCM_Assessment_Register
                    //                join b in db.PCM_Case_Details on i.PCM_Case_Id equals b.PCM_Case_Id
                    //                join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                    //                where a.Intake_Assessment_Id == Assid
                    //                select i.Assesment_Register_Id).FirstOrDefault();

                    PCM_Assessment_Register editCase = db.PCM_Assessment_Register.Find(Assid);

                    editCase.Assesment_Register_Id = Assid;



                    editCase.Assessment_Date = cases.Assessment_Date;



                    editCase.Assessment_Time = cases.Assessment_Time;



                    editCase.Form_Of_Notification_Id = cases.Form_Of_Notification_Id ;

                    editCase.Date_Modified = cases.Date_Modified;
                    editCase.ModifiedBy = userId;


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
        }


        public void DeleteRecFromTableAsessmentreg(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Assessment_Register Obj = (from j in db.PCM_Assessment_Register
                                               where j.Assesment_Register_Id == id
                                          select j).FirstOrDefault();
                db.PCM_Assessment_Register.Remove(Obj);
                db.SaveChanges();
            }
        }
        
        #endregion

        #region OFFENSE DETAILS

        public List<PCMCaseDetailsViewModel> GetOffenceList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in


            var ListP = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Offence_Details on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join x in db.Offence_Categories on d.Offence_Category_Id equals x.Offence_Category_Id
                  join o in db.apl_Offense_Schedules on d.Offence_Schedule_Id equals o.Offence_Schedule_Id
                  join t in db.apl_Offence_Type on d.Offence_Type_Id equals t.Offence_Type_Id
                  where d.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.PCM_Offence_Id,
                      x.Offence_Category_Id,
                      d.Offence_Circumstance,
                      d.Value_Of_Goods,
                      d.Value_Recovered,
                      d.IsChild_Responsible,
                      d.Responsibility_Details,
                      t.Offence_Type_Id


                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.PCM_Offence_Id = item.PCM_Offence_Id;
                obj.selectOffenceCategory = db.Offence_Categories.Find(item.Offence_Category_Id).Description;
                obj.selectOffeceType = db.apl_Offence_Type.Find(item.Offence_Type_Id).Description;
                obj.Offence_Circumstance = item.Offence_Circumstance;
                obj.Value_Of_Goods = item.Value_Of_Goods;
                obj.Offence_Circumstance = item.Offence_Circumstance;
                obj.Value_Recovered = item.Value_Recovered;
                obj.IsChild_Responsible = item.IsChild_Responsible;
                obj.Responsibility_Details = item.Responsibility_Details;

                avm.Add(obj);
            }

            return avm;
        }

        public PCMCaseDetailsViewModel GetPCMOffenceOnEditDetails(int offenseId)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Offence_Details act = db.PCM_Offence_Details.Find(offenseId);

                    if (act != null)
                    {
                        vm.PCM_Offence_Id = act.PCM_Offence_Id;
                        vm.Offence_Category_Id = act.Offence_Category_Id;
                        vm.selectOffenceCategory = db.Offence_Categories.Find(act.Offence_Category_Id).Description;
                        vm.Offence_Schedule_Id = act.Offence_Schedule_Id;
                        vm.selectOffenceSchedule = db.apl_Offense_Schedules.Find(act.Offence_Schedule_Id).Description;
                        vm.Offence_Type_Id = act.Offence_Type_Id;
                        vm.selectOffeceType= db.apl_Offence_Type.Find(act.Offence_Type_Id).Description;
                        vm.Offence_Circumstance = act.Offence_Circumstance;
                        vm.Value_Of_Goods = act.Value_Of_Goods;
                        vm.Value_Recovered = act.Value_Recovered;
                        vm.IsChild_Responsible = act.IsChild_Responsible;
                        vm.Responsibility_Details = act.Responsibility_Details;
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
        }

        public void CreatePCMOffenceDeatils(PCMCaseDetailsViewModel vm, int caseid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Offence_Details newob = new PCM_Offence_Details();
                    newob.Offence_Category_Id = vm.Offence_Category_Id;
                    newob.Offence_Schedule_Id = vm.Offence_Schedule_Id;
                    newob.Offence_Type_Id = vm.Offence_Type_Id;
                    newob.Offence_Circumstance = vm.Offence_Circumstance;
                    newob.Value_Recovered = vm.Value_Recovered;
                    newob.Value_Of_Goods = vm.Value_Of_Goods;
                    newob.IsChild_Responsible = vm.IsChild_Responsible;
                    newob.Responsibility_Details = vm.Responsibility_Details;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;
                    newob.Intake_Assessment_Id = caseid;

                    db.PCM_Offence_Details.Add(newob);
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
        }

        public void UpdatePCMOffenceDetails(PCMCaseDetailsViewModel vm, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Offence_Details edit = db.PCM_Offence_Details.Find(Id);

                    edit.PCM_Offence_Id = Id;
                    edit.Offence_Category_Id = vm.Offence_Category_Id;
                    edit.Offence_Schedule_Id = vm.Offence_Schedule_Id;
                    edit.Offence_Type_Id = vm.Offence_Type_Id;
                    edit.Offence_Circumstance = vm.Offence_Circumstance;
                    edit.Value_Recovered = vm.Value_Recovered;
                    edit.IsChild_Responsible = vm.IsChild_Responsible;
                    edit.Value_Of_Goods = vm.Value_Of_Goods;
                    edit.Responsibility_Details = vm.Responsibility_Details;
                    edit.Modified_By = userId;
                    edit.Date_Modified = DateTime.Now;

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
        }



        public void DeleteRecFromTableOff(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Offence_Details Obj = (from j in db.PCM_Offence_Details
                                           where j.PCM_Offence_Id == id
                                               select j).FirstOrDefault();
                db.PCM_Offence_Details.Remove(Obj);
                db.SaveChanges();
            }
        }


        #endregion

        #region CHARGE DETAILS

        public int GetPCMChargeByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select i.Intake_Assessment_Id).FirstOrDefault();
            }
        }

        public void CreatePCMChargeDeatils(PCMCaseDetailsViewModel cases, int RecId, int Charge_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Charge_Details newCase = new PCM_Charge_Details();

                    newCase.Intake_Assessment_Id = RecId;
                    newCase.Charge_Id = Charge_Id;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_Charge_Details.Add(newCase);

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
        }
        public List<PCMCaseDetailsViewModel> GetSelectedChargeFromDB(int RecId)
        {

            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var ChargeList =
                (from r in db.PCM_Charge_Details
                 join o in db.apl_PCM_Charges on r.Charge_Id equals
                 o.Charge_Id

                 where (r.Intake_Assessment_Id == RecId)
                 select new
                 {
                     r.Intake_Assessment_Id,
                     o.Charge_Id,
                     r.Charge_Detail_Id

                 }).ToList();
            ;
            foreach (var item in ChargeList)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Charge_Detail_Id = item.Charge_Detail_Id;
                obj.Charge_Code = db.apl_PCM_Charges.Find(item.Charge_Id).Charge_Code;
                obj.ChargeDescription = db.apl_PCM_Charges.Find(item.Charge_Id).Charge_Description;


                avm.Add(obj);
            }

            return avm;
        }


        public void DeleteChargeRecord(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Charge_Details Obj = (from j in db.PCM_Charge_Details
                                          where j.Charge_Detail_Id == id
                                          select j).FirstOrDefault();
                db.PCM_Charge_Details.Remove(Obj);
                db.SaveChanges();
            }
        }




        #endregion

        #region PREVIOUS ENVOLVEMENT

        public List<PCMCaseDetailsViewModel> GetPreviousEnvolvementList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in

            int? Clientid = (from p in db.Intake_Assessments
                             join c in db.Clients on p.Client_Id equals c.Client_Id
                             where (p.Intake_Assessment_Id == IntakeassId)
                             select c.Client_Id).FirstOrDefault();

            var ListP = (
                   from p in db.Intake_Assessments
                   join c in db.Clients on p.Client_Id equals c.Client_Id
                   join Pr in db.PCM_Previous_Involvement_Details on p.Intake_Assessment_Id equals Pr.Intake_Assessment_Id
                   join x in db.Offence_Categories on Pr.Offence_Category_Id equals x.Offence_Category_Id into list3
                   from l3 in list3.DefaultIfEmpty()




                   where p.Client_Id == (Clientid)
                   select new
                   {
                       p.Intake_Assessment_Id,
                       Pr.Offence_Category_Id,
                       Pr.Sentence_Outcomes,
                       Pr.IsConvicted,
                       Pr.Arrest_Date,
                       Pr.IsArrest,
                       Pr.Conviction_Date,
                       Pr.IsEscape,
                       Pr.Escapes_Date,
                       Pr.Previous_Involved,
                       Pr.Involvement_Id,

                   }).ToList();
            ;


            foreach (var item in ListP)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Involvement_Id = item.Involvement_Id;
                obj.Previous_Involved = item.Previous_Involved;

                if (item.IsArrest != null)
                {
                    obj.IsArrest = item.IsArrest;
                }
                else
                {
                    obj.IsArrest = "N/A";
                }

                if (item.Offence_Category_Id != null)
                {
                    obj.selectOffenceCategory = db.Offence_Categories.Find(item.Offence_Category_Id).Description;
                }
                else
                {
                    obj.selectOffenceCategory = "N/A";
                }
                obj.PreviousArrest_Date = item.Arrest_Date;

                if (item.IsConvicted != null)
                {
                    obj.IsConvicted = item.IsConvicted;
                }
                else
                {
                    obj.IsConvicted = "N/A";
                }
                obj.previousConviction_Date = item.Conviction_Date;

                if (item.IsEscape != null)
                {
                    obj.IsEscape = item.IsEscape;
                }
                else
                {
                    obj.IsEscape = "N/A";
                }
                obj.Escapes_Date = item.Escapes_Date;

                avm.Add(obj);
            }

            return avm;
        }

        public PCMCaseDetailsViewModel GetPreviousEnvolvementOnEditDetails(int Involvement_Id)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Previous_Involvement_Details act = db.PCM_Previous_Involvement_Details.Find(Involvement_Id);

                    if (act != null)
                    {
                        vm.Involvement_Id = act.Involvement_Id;
                        vm.Previous_Involved = act.Previous_Involved;
                        vm.IsArrest = act.IsArrest;
                        vm.PreviousArrest_Date = act.Arrest_Date;
                        vm.PreviousSentence_Outcomes = act.Sentence_Outcomes;
                        vm.IsConvicted = act.IsConvicted;
                        vm.previousConviction_Date = act.Conviction_Date;
                        vm.IsEscape = act.IsEscape;
                        vm.Escapes_Date = act.Escapes_Date;
                        vm.Escape_Time = act.Escape_Time;
                        vm.Place_Of_Escape = act.Place_Of_Escape;
                        vm.Offence_Category_Id = act.Offence_Category_Id;
                        vm.When_Escaped_Id = act.When_Escaped_Id;

                        if (act.Offence_Category_Id != null)
                        {
                            vm.selectOffenceCategory = db.Offence_Categories.Find(act.Offence_Category_Id).Description;
                        }
                        if (act.When_Escaped_Id != null)
                        {
                            vm.When_Escaped = db.apl_Escape_Period.Find(act.When_Escaped_Id).Description;
                        }

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
        }

        public void CreatePreviousEnvolvementDeatils(PCMCaseDetailsViewModel vm, int caseid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Previous_Involvement_Details newob = new PCM_Previous_Involvement_Details();
                    newob.Involvement_Id = vm.Involvement_Id;
                    newob.Previous_Involved = vm.Previous_Involved;
                    newob.IsArrest = vm.IsArrest;
                    newob.Arrest_Date = vm.PreviousArrest_Date;
                    newob.Sentence_Outcomes = vm.PreviousSentence_Outcomes;
                    newob.IsConvicted = vm.IsConvicted;
                    newob.Conviction_Date = vm.previousConviction_Date;
                    newob.Arrest_Date = vm.PreviousArrest_Date;
                    newob.IsEscape = vm.IsEscape;
                    newob.Escapes_Date = vm.Escapes_Date;
                    newob.Escape_Time = vm.Escape_Time;
                    newob.Place_Of_Escape = vm.Place_Of_Escape;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;
                    newob.Intake_Assessment_Id = caseid;
                    newob.When_Escaped_Id = vm.When_Escaped_Id;
                    newob.Offence_Category_Id = vm.Offence_Category_Id;

                    db.PCM_Previous_Involvement_Details.Add(newob);
                    db.SaveChanges();
                }
                catch (Exception dbEx)
                {
                    Exception raise = dbEx;
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        string message = string.Format("{0}:{1}",
                    //            validationErrors.Entry.Entity.ToString(),
                    //            validationError.ErrorMessage);
                    //        // raise a new exception nesting
                    //        // the current instance as InnerException
                    //        raise = new InvalidOperationException(message, raise);
                    //    }
                    //}
                    throw raise;
                }
            }
        }

        public void UpdatePreviousEnvolvementDetails(PCMCaseDetailsViewModel vm, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Previous_Involvement_Details edit = db.PCM_Previous_Involvement_Details.Find(Id);

                    edit.Involvement_Id = Id;
                    edit.Previous_Involved = vm.Previous_Involved;
                    edit.IsArrest = vm.IsArrest;
                    edit.Arrest_Date = vm.PreviousArrest_Date;
                    edit.Sentence_Outcomes = vm.PreviousSentence_Outcomes;
                    edit.IsConvicted = vm.IsConvicted;
                    edit.Conviction_Date = vm.previousConviction_Date;
                    edit.Arrest_Date = vm.PreviousArrest_Date;
                    edit.IsEscape = vm.IsEscape;
                    edit.Escapes_Date = vm.Escapes_Date;
                    edit.Escape_Time = vm.Escape_Time;
                    edit.Place_Of_Escape = vm.Place_Of_Escape;
                    edit.Modified_By = userId;
                    edit.Date_Modified = DateTime.Now;

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
        }

        public void DeleteRecFromTablePR(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Previous_Involvement_Details Obj = (from j in db.PCM_Previous_Involvement_Details
                                                        where j.Involvement_Id == id
                                           select j).FirstOrDefault();
                db.PCM_Previous_Involvement_Details.Remove(Obj);
                db.SaveChanges();
            }
        }


        #endregion

        #region HEALTH DEATILS DETAILS

        public List<PCMCaseDetailsViewModel> GetHealthstatusList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in



            var ListP = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Health_Details on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join x in db.apl_Health_Status on d.Health_Status_Id equals x.Health_Status_Id
                  where p.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.Health_Details_Id,
                      x.Health_Status_Id,
                      d.Injuries,
                      d.Allergies,
                      d.Medication,
                      d.Medical_Appointments

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Health_Details_Id = item.Health_Details_Id;
                obj.HealthStatusDescription = db.apl_Health_Status.Find(item.Health_Status_Id).Description;
                obj.Injuries = item.Injuries;
                obj.AllergyDescription = item.Allergies;
                obj.Medication = item.Medication;
                obj.Medical_Appointments = item.Medical_Appointments;

                avm.Add(obj);
            }

            return avm;
        }

        public PCMCaseDetailsViewModel GetPCMHealthOnEditDetails(int Health_Details_Id)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Health_Details act = db.PCM_Health_Details.Find(Health_Details_Id);

                    if (act != null)
                    {
                        vm.Health_Details_Id = act.Health_Details_Id;
                        vm.Health_Status_Id = act.Health_Status_Id;
                        vm.Injuries = act.Injuries;
                        vm.Medication = act.Medication;
                        vm.AllergyDescription = act.Allergies;
                        vm.HealthStatusDescription = db.apl_Health_Status.Find(act.Health_Status_Id).Description;
                        vm.Medical_Appointments = act.Medical_Appointments;
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
        }

        public void CreatePCMHealthDeatils(PCMCaseDetailsViewModel vm, int caseid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Health_Details newob = new PCM_Health_Details();
                    newob.Health_Status_Id = vm.Health_Status_Id;
                    newob.Injuries = vm.Injuries;
                    newob.Medication = vm.Medication;
                    newob.Allergies = vm.AllergyDescription;
                    newob.Medical_Appointments = vm.Medical_Appointments;
                    newob.Intake_Assessment_Id = caseid;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;

                    db.PCM_Health_Details.Add(newob);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

        public void UpdatePCMHealthDetails(PCMCaseDetailsViewModel vm, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Health_Details edit = db.PCM_Health_Details.Find(Id);

                    edit.Health_Details_Id = Id;
                    edit.Health_Status_Id = vm.Health_Status_Id;
                    edit.Injuries = vm.Injuries;
                    edit.Medication = vm.Medication;
                    edit.Allergies = vm.AllergyDescription;
                    edit.Medical_Appointments = vm.Medical_Appointments;
                    edit.Modified_By = userId;
                    edit.Date_Modified = DateTime.Now;

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
        }

        public void DeleteRecFromTableME(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Health_Details Obj = (from j in db.PCM_Health_Details
                                          where j.Health_Details_Id == id
                                                        select j).FirstOrDefault();
                db.PCM_Health_Details.Remove(Obj);
                db.SaveChanges();
            }
        }
        #endregion

        #region EDUCATION

        public List<PCMCaseDetailsViewModel> GetEducationList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var ListP = (
                  from p in db.Intake_Assessments
                  join a in db.Clients on p.Client_Id equals a.Client_Id
                  join d in db.Persons on a.Person_Id equals d.Person_Id
                  join e in db.Person_Education_Entities on d.Person_Id equals e.Person_Id
                  join f in db.Schools on e.School_Id equals f.School_Id
                  join g in db.Grades on e.Grade_Completed_Id equals g.Grade_Id
                  where p.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      e.Person_Education_Id,
                      f.School_Name,
                      f.Contact_Person,
                      f.Telephone_Number,
                      e.Grade_Completed_Id,
                      e.Date_Last_Attended,
                      e.Year_Completed,
                      f.School_Id


                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Person_Education_Id = item.Person_Education_Id;
                obj.School_Name = item.School_Name;
                obj.Contact_Person = item.Contact_Person;
                obj.Telephone_Number = item.Telephone_Number;
                obj.Grade_Completed_Id = item.Grade_Completed_Id;
                obj.Date_Last_Attended = item.Date_Last_Attended;
                obj.Year_Completed = item.Year_Completed;
                obj.School_Id = item.School_Id;
                obj.Grade_Completed = db.Grades.Find(item.Grade_Completed_Id).Description;

                avm.Add(obj);
            }

            return avm;
        }


        #endregion
         
        #region SOCIO ECONOMY

        public int GetPCMSocioEconomicDetailsByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Socio_Economy on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Socio_Economy_id).FirstOrDefault();
            }
        }

        public PCMCaseDetailsViewModel GetSocioEconomicsList(int SocioEconomyid)
        {
            PCMCaseDetailsViewModel fvm = new PCMCaseDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Socio_Economy act = db.PCM_Socio_Economy.Find(SocioEconomyid);

                    fvm.Socio_Economy_id = act.Socio_Economy_id;
                    fvm.Family_Background_Comment = act.Family_Background_Comment;
                    fvm.Finance_Work_Record = act.Finance_Work_Record;
                    fvm.Housing = act.Housing;
                    fvm.Social_Circumsances = act.Social_Circumsances;
                    fvm.Previous_Intervention = act.Previous_Intervention;
                    fvm.Inter_Personal_Relationship = act.Inter_Personal_Relationship;
                    fvm.Peer_Presure = act.Peer_Presure;
                    fvm.Substance_Abuse = act.Substance_Abuse;
                    fvm.Religious_Involve = act.Religious_Involve;
                    fvm.Child_Behavior = act.Child_Behavior;
                    fvm.Other = act.Other;

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
        }

        public void InsertSocioEconomy(PCMCaseDetailsViewModel cases, int intassid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Socio_Economy newCase = new PCM_Socio_Economy();

                    newCase.Intake_Assessment_Id = intassid;
                    newCase.Family_Background_Comment = cases.Family_Background_Comment;
                    newCase.Finance_Work_Record = cases.Finance_Work_Record;
                    newCase.Housing = cases.Housing;
                    newCase.Social_Circumsances = cases.Social_Circumsances;
                    newCase.Previous_Intervention = cases.Previous_Intervention;
                    newCase.Inter_Personal_Relationship = cases.Inter_Personal_Relationship;
                    newCase.Peer_Presure = cases.Peer_Presure;
                    newCase.Substance_Abuse = cases.Substance_Abuse;
                    newCase.Religious_Involve = cases.Religious_Involve;
                    newCase.Child_Behavior = cases.Child_Behavior;
                    newCase.Other = cases.Other;
                    newCase.Date_Created = cases.Date_Created;
                    newCase.Created_By = cases.Created_By;
                    db.PCM_Socio_Economy.Add(newCase);

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
        }

        public void UpdateSocioEconomy(PCMCaseDetailsViewModel cases, int userId, int SocioEconomyid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {


                    PCM_Socio_Economy editCase = db.PCM_Socio_Economy.Find(SocioEconomyid);

                    if (SocioEconomyid > 0)
                    {

                        editCase.Family_Background_Comment = cases.Family_Background_Comment;
                        editCase.Finance_Work_Record = cases.Finance_Work_Record;
                        editCase.Housing = cases.Housing;
                        editCase.Social_Circumsances = cases.Social_Circumsances;
                        editCase.Previous_Intervention = cases.Previous_Intervention;
                        editCase.Inter_Personal_Relationship = cases.Inter_Personal_Relationship;
                        editCase.Peer_Presure = cases.Peer_Presure;
                        editCase.Substance_Abuse = cases.Substance_Abuse;
                        editCase.Religious_Involve = cases.Religious_Involve;
                        editCase.Child_Behavior = cases.Child_Behavior;
                        editCase.Other = cases.Other;
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
        }
        #endregion

        #region DEVELOPMENTAL ASSESSMENT

        public int GetPCMDevelopmentAssessmentByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Development_Assessment on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Development_Id).FirstOrDefault();
            }
        }

        public PCMCaseDetailsViewModel GetDevelopmentAssessmentList(int DevelopmentId)
        {
            PCMCaseDetailsViewModel fvm = new PCMCaseDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Development_Assessment act = db.PCM_Development_Assessment.Find(DevelopmentId);

                    fvm.Development_Id = act.Development_Id;
                    fvm.Belonging = act.Belonging;
                    fvm.Mastery = act.Mastery;
                    fvm.Independence = act.Independence;
                    fvm.Generosity = act.Generosity;
                    fvm.Evaluation = act.Evaluation;


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
        }

        public void InsertDevelopmentAssessment(PCMCaseDetailsViewModel cases, int intassid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Development_Assessment newCase = new PCM_Development_Assessment();

                    newCase.Intake_Assessment_Id = intassid;
                    newCase.Belonging = cases.Belonging;
                    newCase.Mastery = cases.Mastery;
                    newCase.Independence = cases.Independence;
                    newCase.Generosity = cases.Generosity;
                    newCase.Evaluation = cases.Evaluation;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_Development_Assessment.Add(newCase);


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
        }

        public void UpdateDevelopmentAssessment(PCMCaseDetailsViewModel cases, int userId, int DevelopmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {


                    PCM_Development_Assessment editCase = db.PCM_Development_Assessment.Find(DevelopmentId);

                    if (DevelopmentId > 0)
                    {

                        editCase.Belonging = cases.Belonging;
                        editCase.Mastery = cases.Mastery;
                        editCase.Independence = cases.Independence;
                        editCase.Generosity = cases.Generosity;
                        editCase.Evaluation = cases.Evaluation;
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
        }
        #endregion

        #region GENERAL DETAILS

        public int GetPCMGeneralDetailsByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_General_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.General_Details_Id).FirstOrDefault();
            }
        }

        public PCMCaseDetailsViewModel GetGeneralDetailsList(int GeneralId)
        {
            PCMCaseDetailsViewModel fvm = new PCMCaseDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_General_Details act = db.PCM_General_Details.Find(GeneralId);

                    fvm.General_Details_Id = act.General_Details_Id;
                    fvm.Consulted_Sources = act.Consulted_Sources;
                    fvm.Trace_Efforts = act.Trace_Efforts;
                    fvm.Assessment_TimeEnd = act.Assessment_Time;
                    fvm.Assessment_DateEnd = act.Assessment_Date;
                    fvm.Additional_Info = act.Additional_Info;

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
        }

        public void InsertGeneralDetails(PCMCaseDetailsViewModel cases, int intassid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
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
        }

        public void UpdatePCM_General_Details(PCMCaseDetailsViewModel cases, int userId, int AssId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
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
        }
        #endregion

        #region RECOMENDATIONS

        public int GetPCMRecommendationByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Recommendation on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Recommendation_Id).FirstOrDefault();
            }
        }

        public PCMCaseDetailsViewModel GetRecomendationDetailsList(int RecommendationId)
        {
            PCMCaseDetailsViewModel fvm = new PCMCaseDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Recommendation act = db.PCM_Recommendation.Find(RecommendationId);

                    fvm.Recommendation_Id = act.Recommendation_Id;
                    fvm.Recommendation_Type_Id = act.Recommendation_Type_Id;
                    fvm.Placement_Type_Id = act.Placement_Type_Id;
                    fvm.Comments_For_Recommendation = act.Comments_For_Recommendation;

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
        }

        public void CreatePCMRecomendationDeatils(PCMCaseDetailsViewModel cases, int intassid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Recommendation newCase = new PCM_Recommendation();

                    newCase.Intake_Assessment_Id = intassid;
                    newCase.Recommendation_Type_Id = cases.Recommendation_Type_Id;
                    newCase.Placement_Type_Id = cases.Placement_Type_Id;
                    newCase.Comments_For_Recommendation = cases.Comments_For_Recommendation;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_Recommendation.Add(newCase);

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
        }

        public void UpdatePCMRecomendationDetails(PCMCaseDetailsViewModel cases, int AssId, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Recommendation editCase = db.PCM_Recommendation.Find(AssId);

                    if (AssId > 0)
                    {


                        editCase.Recommendation_Type_Id = cases.Recommendation_Type_Id;
                        editCase.Placement_Type_Id = cases.Placement_Type_Id;
                        editCase.Comments_For_Recommendation = cases.Comments_For_Recommendation;
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
        }
        #endregion

        #region RECOMMENDATION ORDERS

        public int GetPCMRecomendationByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Recommendation on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Recommendation_Id).FirstOrDefault();
            }
        }

        public List<PCMCaseDetailsViewModel> GetSelectedOrdersFromDB(int RecId)
        {

            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var orderList =
                (from r in db.PCM_Recommendation
                 join o in db.PCM_Orders on r.Recommendation_Id equals
                 o.Recommendation_Id
                 join ot in db.apl_PCM_Orders on o.Recomendation_Order_Id equals ot.Recomendation_Order_Id
                 //join rt in db.apl_Recommendation_Type on r.Recommendation_Type_Id equals rt.Recommendation_Type_Id

                 where (r.Recommendation_Id == RecId)
                 select new
                 {
                     r.Intake_Assessment_Id,
                     r.Recommendation_Id,
                     o.Order_Id,
                     ot.Recomendation_Order_Id,
                     //rt.Recommendation_Type_Id

                 }).ToList();
            ;
            foreach (var item in orderList)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Order_Id = item.Order_Id;
                obj.Description_Recomendation_Order = db.apl_PCM_Orders.Find(item.Recomendation_Order_Id).Description;


                avm.Add(obj);
            }

            return avm;
        }

        public void CreatePCMOrderDeatils(PCMCaseDetailsViewModel cases, int RecId, int Recomendation_Order_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Orders newCase = new PCM_Orders();

                    newCase.Recommendation_Id = RecId;
                    newCase.Recomendation_Order_Id = Recomendation_Order_Id;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_Orders.Add(newCase);

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
        }


        public void DeleteRecord(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Orders Obj = (from j in db.PCM_Orders
                                  where j.Order_Id == id
                                  select j).FirstOrDefault();
                db.PCM_Orders.Remove(Obj);
                db.SaveChanges();
            }
        }

        #endregion

        #region RECOMMENDED DIVESIONS

        public void CreatePCMDivesionsDeatils(PCMCaseDetailsViewModel cases, int RecId, int Div_Program_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Diversion_Recommendation newCase = new PCM_Diversion_Recommendation();

                    newCase.PCM_Recommendation_Id = RecId;
                    newCase.Recommendation_Programmes_Id = Div_Program_Id;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_Diversion_Recommendation.Add(newCase);

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
        }

        public List<PCMCaseDetailsViewModel> GetSelectedDivesionFromDB(int RecId)
        {

            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var orderList =
                (from r in db.PCM_Recommendation
                 join o in db.PCM_Diversion_Recommendation on r.Recommendation_Id equals
                 o.PCM_Recommendation_Id
                 join ot in db.apl_Diversion_Programmes on o.Recommendation_Programmes_Id equals ot.Div_Program_Id
                 //join rt in db.apl_Recommendation_Type on r.Recommendation_Type_Id equals rt.Recommendation_Type_Id

                 where (r.Recommendation_Id == RecId)
                 select new
                 {
                     r.Intake_Assessment_Id,
                     r.Recommendation_Id,
                     o.PCM_Diversion_Recomm_Id,
                     ot.Div_Program_Id,
                     //rt.Recommendation_Type_Id

                 }).ToList();
            ;
            foreach (var item in orderList) 
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.PCM_Diversion_Recomm_Id = item.PCM_Diversion_Recomm_Id;
                obj.DesrciptionDivesionPrograme = db.apl_Diversion_Programmes.Find(item.Div_Program_Id).Programme_Name;


                avm.Add(obj);
            }

            return avm;
        }

        public void DeleteProgrammeRecord(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Diversion_Recommendation Obj = (from j in db.PCM_Diversion_Recommendation
                                  where j.PCM_Diversion_Recomm_Id == id
                                  select j).FirstOrDefault();
                db.PCM_Diversion_Recommendation.Remove(Obj);
                db.SaveChanges();
            }
        }

        #endregion

        #region FACILITY BED SPACE REQUEST


        //FACILITY LIST AND PROGRAMS

        public List<PCMCaseDetailsViewModel> GetFacilityFormaloadList()
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
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
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

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

        public List<PCMCaseDetailsViewModel> GetFacilityList(int ProvinceID)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
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
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

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

        public List<PCMCaseDetailsViewModel> GetMaleSpaceList(int fid)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in



            var ListMS = (
                  from p in db.apl_Cyca_Male_BedSpace
                  join d in db.apl_Cyca_Facility on p.Facility_Id equals d.Facility_Id
                  where d.Facility_Id == (fid)

                  select new
                  {
                      p.Total_Space,
                      p.Available_Space,
                      p.Used_Space
                  }).ToList();

           
            ;
            foreach (var item in ListMS )
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Male_Total_Space = item.Total_Space.ToString();
                obj.Male_Available_Space = item.Available_Space.ToString();
                obj.Male_Used_Space = item.Used_Space.ToString();


                avm.Add(obj);
            }

            return avm;
        }

        public List<PCMCaseDetailsViewModel> GetFemaleSpaceList(int fid)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in



            var ListMS = (
                  

                  from pp in db.apl_Cyca_Female_BedSpace
                  join dd in db.apl_Cyca_Facility on pp.Facility_Id equals dd.Facility_Id
                  where dd.Facility_Id == (fid)

                  select new
                  {                      
                      FeTotal_Space = pp.Total_Space,
                      FeAvailable = pp.Available_Space,
                      FeUsed_Space = pp.Used_Space,
                      
                  }).ToList();
            
            ;
            foreach (var item in ListMS)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Female_Total_Space = item.FeTotal_Space.ToString(); ;
                obj.Female_Available_Space = item.FeAvailable.ToString();
                obj.Female_Used_Space = item.FeUsed_Space.ToString();


                avm.Add(obj);
            }

            return avm;
        }

        public List<PCMCaseDetailsViewModel> GetFacilityProgramList(int fid)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
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
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

               
                obj.ProgramNames = item.Program_Name;
                obj.ProgramDescription = item.Program_Description;
                obj.ProgramStartDate = item.Program_StartDate;
                obj.ProgramEndDate = item.Program_EndDate;
                obj.ProgramCapacity = item.Program_Capacity;


                avm.Add(obj);
            }

            return avm;
        }

        public PCMCaseDetailsViewModel GetFacilityMailList(int falcilityid)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
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
        }

       

        public List<PCMCaseDetailsViewModel> GetFacilitybedSpaceList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in

            var ListP = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Facility_Space_Inbox_ReqOutBox on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join x in db.apl_Admission_Type on d.Admission_Type_Id equals x.Admission_Type_Id
                  join z in db.apl_BedSpace_Request on d.Request_Status_Id equals z.Request_Status_Id
                  join f in db.apl_Cyca_Facility on d.Facility_Id equals f.Facility_Id
                  where p.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.Request_Status_Id,
                      x.Admission_Type_Id,
                      d.Facility_Id,
                      d.Province_Id,
                      d.Sent_By,
                      d.Request_Comments,
                      d.Date_Created,
                      d.Request_Id

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Request_Id = item.Request_Id;
                obj.selectBedRequestStatus = db.apl_BedSpace_Request.Find(item.Request_Status_Id).Description;
                obj.selectAdmissionType = db.apl_Admission_Type.Find(item.Admission_Type_Id).Description;
                obj.SelectFacility = db.apl_Cyca_Facility.Find(item.Facility_Id).FacilityName;
                obj.Request_Comments = item.Request_Comments;
                obj.Date_Created = item.Date_Created;

                avm.Add(obj);
            }

            return avm;
        }

        public PCMCaseDetailsViewModel GetFacilitybedSpaceOnEditDetails(int Request_Id)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
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
        }

        public void CreateFacilitybedSpaceDeatils(PCMCaseDetailsViewModel vm, int caseid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
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
                    newob.Sent_By = userId;
                    newob.Date_Created = DateTime.Now;
                    newob.Date_Recieved = DateTime.Now;
                    DateTime timeReceived = DateTime.Now;
                    string time = timeReceived.ToString("HH:mm");
                    newob.Time_Recieved = time;
                    newob.Request_Open_Close = "Open";                                 
                    db.PCM_Facility_Space_Inbox_ReqOutBox.Add(newob);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

        public void UpdateFacilitybedSpaceAcceptDetails(PCMCaseDetailsViewModel vm, int Id, int userId, string acceptdecline)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Facility_Space_Inbox_ReqOutBox edit = db.PCM_Facility_Space_Inbox_ReqOutBox.Find(Id);
                    edit.Request_Id = Id;

                    if (acceptdecline == "Accept")
                    {
                        edit.Request_Status_Id = 3;
                    }
                    else if (acceptdecline == "Decline")
                    {
                        edit.Request_Status_Id = 5;
                    }
                    edit.Modified_By = userId;
                    edit.Date_Modified = DateTime.Now;

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
        }

     

        #endregion

        #region VICTIM DETAILS

        #region VICTIM INDIVIDUAL

        public List<PCMCaseDetailsViewModel> GetPersons()
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            // get problem category for adoption


            // get work list for user logged in
            var pList =

                (from list in db.Persons
                 select new
                 {
                     list.Person_Id,
                     list.First_Name,
                     list.Last_Name,
                     list.Age,
                     list.Phone_Number
                 }).ToList().Take(50);
            ;
            foreach (var item in pList)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Person_Id = item.Person_Id;
                obj.Victim_First_Name = item.First_Name;
                obj.Victim_Last_Name = item.Last_Name;
                obj.Victim_Age = item.Age;
                obj.Victim_Phone_Number = item.Phone_Number;
                avm.Add(obj);
            }

            return avm;
        }
        
        public List<PCMCaseDetailsViewModel> GetVictimList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in

            var ListV = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Victim_Details on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join e in db.Persons on d.Person_Id equals e.Person_Id
                  where d.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.Victim_Id,
                      e.First_Name,
                      e.Last_Name,
                      e.Age,
                      e.Phone_Number,


                  }).ToList();
            ;
            foreach (var item in ListV)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Victim_Id = item.Victim_Id;
                obj.Victim_First_Name = item.First_Name;
                obj.Victim_Last_Name = item.Last_Name;
                //obj.Victim_Occupation = item.Victim_Occupation;
                obj.Victim_Age = item.Age;
                obj.Victim_Phone_Number = item.Phone_Number;
                //obj.Victim_Care_Giver_Names = item.Victim_Care_Giver_Names;
                //obj.Adressdecription = item.Address_Line_1+" "+ item.Address_Line_2 +" " + db.Towns.Find(item.Town_Id).Description+" "+item.Postal_Code;

                avm.Add(obj);
            }

            return avm;
        }


        public List<PCMCaseDetailsViewModel> GetPersonList(string LastName)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in

            var ListV = (
                  from
                  d in db.Persons
                  where d.Last_Name == (LastName)
                  select new
                  {
                      d.Person_Id,
                      d.First_Name,
                      d.Last_Name,
                      d.Age,
                      d.Phone_Number

                  }).ToList();
            ;
            foreach (var item in ListV)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Person_Id = item.Person_Id;
                obj.Victim_First_Name = item.First_Name;
                obj.Victim_Last_Name = item.Last_Name;
                obj.Victim_Age = item.Age;
                obj.Victim_Phone_Number = item.Phone_Number;

                avm.Add(obj);
            }

            return avm;
        }

        public PCMCaseDetailsViewModel GetVictimOnEditDetails(int Victim_Id)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Victim_Details act = db.PCM_Victim_Details.Find(Victim_Id);

                    if (act != null)
                    {
                        vm.Victim_Id = act.Victim_Id;
                        vm.IsVictimIndividual = act.IsVictimIndividual;
                        vm.Person_Id = act.Person_Id;
                        vm.Victim_Occupation = act.Victim_Occupation;

                        vm.Victim_Care_Giver_Names = act.Victim_Care_Giver_Names;
                        vm.Victim_Address_Line_1 = act.Address_Line_1;
                        vm.Victim_Address_Line_2 = act.Address_Line_2;
                        vm.Victim_Postal_Code = act.Postal_Code;
                        vm.Victim_Town_Id = act.Town_Id;

                        if (act.Town_Id != null)
                        {
                            vm.TownDescription = db.Towns.Find(act.Town_Id).Description;
                        }
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
        }

        public void CreateVictimDeatils(PCMCaseDetailsViewModel vm, int caseid, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Victim_Details newob = new PCM_Victim_Details();
                    newob.Victim_Id = vm.Victim_Id;
                    newob.Person_Id = Id;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;
                    newob.Intake_Assessment_Id = caseid;

                    db.PCM_Victim_Details.Add(newob);
                    db.SaveChanges();
                }
                catch (Exception dbEx)
                {
                    Exception raise = dbEx;
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        string message = string.Format("{0}:{1}",
                    //            validationErrors.Entry.Entity.ToString(),
                    //            validationError.ErrorMessage);
                    //        // raise a new exception nesting
                    //        // the current instance as InnerException
                    //        raise = new InvalidOperationException(message, raise);
                    //    }
                    //}
                    throw raise;
                }
            }
        }
            
        public void UpdateVictimDetails(PCMCaseDetailsViewModel vm, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Victim_Details edit = db.PCM_Victim_Details.Find(Id);

                    edit.Victim_Id = Id;
                    edit.IsVictimIndividual = vm.IsVictimIndividual;
                    edit.Victim_Occupation = vm.Victim_Occupation;
                    edit.Victim_Care_Giver_Names = vm.Victim_Care_Giver_Names;
                    edit.Address_Line_1 = vm.Victim_Address_Line_1;
                    edit.Address_Line_2 = vm.Victim_Address_Line_2;
                    edit.Town_Id = vm.Town_Id;
                    edit.Postal_Code = vm.Victim_Postal_Code;
                    edit.Modified_By = userId;
                    edit.Date_Modified = DateTime.Now;

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
        }

        public void DeleteRecordVictim(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Victim_Details Obj = (from j in db.PCM_Victim_Details
                                          where j.Victim_Id == id
                                          select j).FirstOrDefault();
                db.PCM_Victim_Details.Remove(Obj);
                db.SaveChanges();
            }
        }


        //Create a person and Victim

        public void PersonCreate_Victim(PCMCaseDetailsViewModel vm, int caseid, string userId,  bool isActive, bool isDeleted)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    Person newob = new Person();
                    newob.First_Name = vm.Victim_First_Name;
                    newob.Last_Name = vm.Victim_Last_Name;
                    newob.Age = vm.Victim_Age;
                    newob.Is_Piva_Validated = false;
                    newob.Is_Estimated_Age = true;
                    newob.Gender_Id = vm.Victim_Gender;
                    newob.Person_Id =Convert.ToInt32( vm.Person_Id > 0 ? vm.Person_Id : 1); 
                    newob.Created_By = userId;
                    newob.Is_Active = isActive;
                    newob.Is_Deleted = isDeleted;
                    newob.Date_Created = DateTime.Now;
                    db.Persons.Add(newob);
                    db.SaveChanges();


                    PCM_Victim_Details Vi = new PCM_Victim_Details();
                    PCMCaseDetailsViewModel vmvi = new PCMCaseDetailsViewModel();
                    Vi.Victim_Id = vmvi.Victim_Id;
                    Vi.Person_Id = newob.Person_Id;
                    Vi.Created_By = Convert.ToInt32(userId);
                    Vi.Date_Created = DateTime.Now;
                    Vi.Intake_Assessment_Id = caseid;
                    db.PCM_Victim_Details.Add(Vi);
                    db.SaveChanges();


                    Address ViA = new Address();
                    ViA.Town_Id = vmvi.Victim_Town_Id;
                    ViA.Address_Line_1 = vmvi.Victim_Address_Line_1;
                    ViA.Address_Line_2 = vmvi.Victim_Address_Line_2;
                    ViA.Postal_Code = vmvi.Victim_Postal_Code;
                    ViA.Address_Id = Convert.ToInt32(vmvi.Victim_Address_Id > 0 ? vmvi.Victim_Address_Id : 1); 
                    db.Addresses.Add(ViA);
                    db.SaveChanges();


            




                }
                catch (Exception dbEx)
                {
                    Exception raise = dbEx;
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        string message = string.Format("{0}:{1}",
                    //            validationErrors.Entry.Entity.ToString(),
                    //            validationError.ErrorMessage);
                    //        // raise a new exception nesting
                    //        // the current instance as InnerException
                    //        raise = new InvalidOperationException(message, raise);
                    //    }
                    //}
                    throw raise;
                }
            }
        }

        #endregion

        #region VICTIM ORGANISATION

        public List<PCMCaseDetailsViewModel> GetVictimOrganisationList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in

            var ListV = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Victim_Organisation_Details on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  where d.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.Victim_Organisation_Id,
                      d.Organisation_Name,
                      d.Contact_Person_First_Name,
                      d.Contact_Person_Last_Name,
                      d.Contact_Person_Occupation,
                      //d.Address_Line_1,
                      //d.Address_Line_2,
                      //d.Town_Id,
                      d.Telephone,


                  }).ToList();
            ;
            foreach (var item in ListV)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.OrganisationVictim_Id = item.Victim_Organisation_Id;
                obj.OrganisationName = item.Organisation_Name;
                obj.ContactPersonfirstname = item.Contact_Person_First_Name;
                obj.ContactPersonlastname = item.Contact_Person_Last_Name;
                obj.Victim_Occupation = item.Contact_Person_Occupation;
                obj.OrganisationTell = item.Telephone;
                //obj.TownDescription = db.Towns.Find(item.Town_Id).Description;
                //obj.Victim_Address_Line_1 = item.Address_Line_1;
                //obj.Victim_Address_Line_2 = item.Address_Line_2;

                avm.Add(obj);
            }

            return avm;
        }

        public PCMCaseDetailsViewModel GeVictimOrganisationOnEditDetails(int Victim_Organisation_Id)
        {
            PCMCaseDetailsViewModel vm = new PCMCaseDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Victim_Organisation_Details act = db.PCM_Victim_Organisation_Details.Find(Victim_Organisation_Id);

                    if (act != null)
                    {
                        vm.OrganisationVictim_Id = act.Victim_Organisation_Id;
                        vm.OrganisationName = act.Organisation_Name;
                        vm.ContactPersonfirstname = act.Contact_Person_First_Name;
                        vm.ContactPersonlastname = act.Contact_Person_Last_Name;
                        vm.OrganisationTell = act.Telephone;
                        vm.Victim_Occupation = act.Contact_Person_Occupation;
                        vm.Interventionservice_referrals = act.Interventionservice_referrals;
                        vm.Town_Id = act.Town_Id;
                        vm.Victim_Postal_Code = act.Postal_Code;
                        vm.Victim_Address_Line_1 = act.Address_Line_1;
                        vm.Victim_Address_Line_2 = act.Address_Line_2;
                        
                        vm.Towndesc = db.Towns.Find(act.Town_Id).Description;

                        int DistrictId = (from k in db.Districts
                                
                                         join k3 in db.Local_Municipalities on k.District_Id equals k3.District_Municipality_Id
                                         join k4 in db.Towns on k3.Local_Municipality_Id equals k4.Local_Municipality_Id
                                         where k4.Town_Id == act.Town_Id
                                         select k.District_Id).FirstOrDefault();
                        vm.District_Id = DistrictId;

                        vm.Ditrictdesc = db.Districts.Find(DistrictId).Description;

                        int ProvinceId = (from a in db.Districts
                                          join b in db.Provinces on a.Province_Id equals b.Province_Id
                                          where a.District_Id.Equals(DistrictId)
                                          select b.Province_Id).FirstOrDefault();

                        vm.Province_Id = ProvinceId;

                        vm.Provincedesc = db.Provinces.Find(ProvinceId).Description;

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
        }

        public void CreateVictimOrganisationDeatils(PCMCaseDetailsViewModel vm, int caseid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Victim_Organisation_Details newob = new PCM_Victim_Organisation_Details();
                    newob.Victim_Organisation_Id = vm.OrganisationVictim_Id;
                    newob.Organisation_Name = vm.OrganisationName;
                    newob.Contact_Person_First_Name = vm.ContactPersonfirstname;
                    newob.Contact_Person_Last_Name = vm.ContactPersonlastname;
                    newob.Telephone = vm.OrganisationTell;
                    newob.Interventionservice_referrals = vm.Interventionservice_referrals;
                    newob.Contact_Person_Occupation = vm.Victim_Occupation;
                    newob.Town_Id = vm.Town_Id;
                    newob.Postal_Code = vm.Victim_Postal_Code;
                    newob.Address_Line_1 = vm.Victim_Address_Line_1;
                    newob.Address_Line_2 = vm.Victim_Address_Line_2;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;
                    newob.Intake_Assessment_Id = caseid;
                    db.PCM_Victim_Organisation_Details.Add(newob);
                    db.SaveChanges();
                }
                catch (Exception dbEx)
                {
                    Exception raise = dbEx;
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        string message = string.Format("{0}:{1}",
                    //            validationErrors.Entry.Entity.ToString(),
                    //            validationError.ErrorMessage);
                    //        // raise a new exception nesting
                    //        // the current instance as InnerException
                    //        raise = new InvalidOperationException(message, raise);
                    //    }
                    //}
                    throw raise;
                }
            }
        }

        public void UpdateVictimOrganisationDetails(PCMCaseDetailsViewModel vm, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Victim_Organisation_Details edit = db.PCM_Victim_Organisation_Details.Find(Id);

                    edit.Victim_Organisation_Id = Id;
                    edit.Organisation_Name = vm.OrganisationName;
                    edit.Contact_Person_First_Name = vm.ContactPersonfirstname;
                    edit.Contact_Person_Last_Name = vm.ContactPersonlastname;
                    edit.Contact_Person_Occupation = vm.Victim_Occupation;
                    edit.Telephone = vm.OrganisationTell;
                    edit.Postal_Code = vm.Victim_Postal_Code;
                    edit.Town_Id = vm.Town_Id;
                    edit.Address_Line_1 = vm.Victim_Address_Line_1;
                    edit.Address_Line_2 = vm.Victim_Address_Line_2;
                    edit.Interventionservice_referrals = vm.Interventionservice_referrals;
                    edit.Modified_By = userId;
                    edit.Date_Modified = DateTime.Now;

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
        }

        #endregion


        #endregion

        #region CO-ACCUSED DETAILS

        public List<PCMCaseDetailsViewModel> GetCoAccusedList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var ListP = (
                  from p in db.Intake_Assessments
                  join c in db.PCM_Co_Accused_Details on p.Intake_Assessment_Id equals c.Intake_Assessment_Id
                  join b in db.Persons on c.Person_Id equals b.Person_Id
                  where p.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      c.Co_Accused_Id,
                      b.First_Name,
                      b.Last_Name,
                      c.Cubacc

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Co_Accused_Id = item.Co_Accused_Id;
                obj.Co_Accused_First_Name = item.First_Name;
                obj.Co_Accused_Last_Name = item.Last_Name;
                obj.Cubacc = item.Cubacc;

                avm.Add(obj);
            }

            return avm;
        }

        public void CreateCoAccusedDeatils(PCMCaseDetailsViewModel vm, int caseid, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Co_Accused_Details newob = new PCM_Co_Accused_Details();
                    
                    newob.Co_Accused_Id = vm.Co_Accused_Id;
                    newob.Person_Id = Id;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;
                    newob.Intake_Assessment_Id = caseid;
                    
                    db.PCM_Co_Accused_Details.Add(newob);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

        public void DeleteRecordCoAccused(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Co_Accused_Details Obj = (from j in db.PCM_Co_Accused_Details
                                          where j.Co_Accused_Id == id
                                          select j).FirstOrDefault();
                db.PCM_Co_Accused_Details.Remove(Obj);
                db.SaveChanges();
            }
        }

        public void PersonCreate_CoAccused(PCMCaseDetailsViewModel vm, int caseid, string userId, bool isActive, bool isDeleted)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    Person newob = new Person();
                    newob.First_Name = vm.Co_Accused_First_Name;
                    newob.Last_Name = vm.Co_Accused_Last_Name;
                    newob.Is_Piva_Validated = false;
                    newob.Is_Estimated_Age = true;
                    newob.Person_Id = Convert.ToInt32(vm.Person_Id > 0 ? vm.Person_Id : 1);
                    newob.Created_By = userId;
                    newob.Is_Active = isActive;
                    newob.Is_Deleted = isDeleted;
                    newob.Date_Created = DateTime.Now;
                    db.Persons.Add(newob);
                    db.SaveChanges();


                    PCM_Co_Accused_Details Vi = new PCM_Co_Accused_Details();
                    PCMCaseDetailsViewModel vmvi = new PCMCaseDetailsViewModel();
                    Vi.Co_Accused_Id = vmvi.Co_Accused_Id;
                    Vi.Person_Id = newob.Person_Id;
                    Vi.Cubacc = vmvi.Cubacc;
                    Vi.Created_By = Convert.ToInt32(userId);
                    Vi.Date_Created = DateTime.Now;
                    Vi.Intake_Assessment_Id = caseid;
                    db.PCM_Co_Accused_Details.Add(Vi);
                    db.SaveChanges();







                }
                catch (Exception dbEx)
                {
                    Exception raise = dbEx;
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        string message = string.Format("{0}:{1}",
                    //            validationErrors.Entry.Entity.ToString(),
                    //            validationError.ErrorMessage);
                    //        // raise a new exception nesting
                    //        // the current instance as InnerException
                    //        raise = new InvalidOperationException(message, raise);
                    //    }
                    //}
                    throw raise;
                }
            }
        }

        #endregion

        #region FAMILY BACKGRAOUND

        public int GetPCMFamillyBGDetailsByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Family_Information on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Family_information_Id).FirstOrDefault();
            }
        }

        public PCMCaseDetailsViewModel GetFamillyBGList(int FamilyinformationId)
        {
            PCMCaseDetailsViewModel fvm = new PCMCaseDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Family_Information act = db.PCM_Family_Information.Find(FamilyinformationId);

                    fvm.Family_information_Id = FamilyinformationId;
                    fvm.Family_Background = act.Family_Background;

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
        }

        public void InsertFamillyBG(PCMCaseDetailsViewModel cases, int intassid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Family_Information newCase = new PCM_Family_Information();

                    newCase.Intake_Assessment_Id = intassid;

                    newCase.Date_Created = cases.Date_Created;
                    newCase.Created_By = cases.Created_By;
                    db.PCM_Family_Information.Add(newCase);

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
        }

        public void UpdateFamillyBG(PCMCaseDetailsViewModel cases, int userId, int familyId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    PCM_Family_Information editCase = db.PCM_Family_Information.Find(familyId);




                    if (cases.Family_Background != null)
                    {

                        editCase.Family_Background = cases.Family_Background;
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
        }
        #endregion

        #region FAMILLY MEMEBERS

        public List<PCMCaseDetailsViewModel> GetFamilyMemberList(int IntakeassId)
        {
            // initialising view model 
            List<PCMCaseDetailsViewModel> avm = new List<PCMCaseDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in



            var ListP = (
                  from p in db.Intake_Assessments
                  join d in db.PCM_Family_Member on p.Intake_Assessment_Id equals d.Intake_Assessment_Id
                  join x in db.Relationship_Types on d.Relationship_Type_Id equals x.Relationship_Type_Id
                  join b in db.Persons on d.Person_Id equals b.Person_Id
                  where p.Intake_Assessment_Id == (IntakeassId)
                  select new
                  {
                      p.Intake_Assessment_Id,
                      d.Family_Member_Id,
                      x.Relationship_Type_Id,
                      b.First_Name,
                      b.Last_Name,
                      b.Age

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                PCMCaseDetailsViewModel obj = new PCMCaseDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Family_Member_Id = item.Family_Member_Id;
                obj.SelectRelationshipType = db.Relationship_Types.Find(item.Relationship_Type_Id).Description;
                obj.Family_Member_Name = item.First_Name;
                obj.Family_Member_Last_Name = item.Last_Name;
                obj.Family_Member_Age = item.Age;

                avm.Add(obj);
            }

            return avm;
        }

        public void CreateFamilyMemberDeatils(PCMCaseDetailsViewModel vm, int caseid, int Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Family_Member newob = new PCM_Family_Member();

                    newob.Family_Member_Id = vm.Family_Member_Id;
                    newob.Person_Id = Id;
                    newob.Relationship_Type_Id = vm.Relationship_Type_Id;
                    newob.Created_By = userId;
                    newob.Date_Created = DateTime.Now;
                    newob.Intake_Assessment_Id = caseid;
                    db.PCM_Family_Member.Add(newob);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

        public void DeleteRecordFamilyMember(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                PCM_Family_Member Obj = (from j in db.PCM_Family_Member
                                         where j.Family_Member_Id == id
                                              select j).FirstOrDefault();
                db.PCM_Family_Member.Remove(Obj);
                db.SaveChanges();
            }
        }

        public void PersonCreate_FamilyMember(PCMCaseDetailsViewModel vm, int caseid, string userId,int relation, bool isActive, bool isDeleted)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    Person newob = new Person();
                    newob.First_Name = vm.Family_Member_Name;
                    newob.Last_Name = vm.Family_Member_Last_Name;
                    newob.Age = vm.Family_Member_Age;
                    newob.Is_Piva_Validated = false;
                    newob.Is_Estimated_Age = true;
                    newob.Person_Id = Convert.ToInt32(vm.Person_Id > 0 ? vm.Person_Id : 1);
                    newob.Created_By = userId;
                    newob.Is_Active = isActive;
                    newob.Is_Deleted = isDeleted;
                    newob.Date_Created = DateTime.Now;
                    db.Persons.Add(newob);
                    db.SaveChanges();


                    PCM_Family_Member Vi = new PCM_Family_Member();
                    PCMCaseDetailsViewModel vmvi = new PCMCaseDetailsViewModel();
                    Vi.Family_Member_Id = vmvi.Family_Member_Id;
                    Vi.Person_Id = newob.Person_Id;
                    Vi.Relationship_Type_Id = relation;
                    Vi.Created_By = Convert.ToInt32(userId);
                    Vi.Date_Created = DateTime.Now;
                    Vi.Intake_Assessment_Id = caseid;
                    db.PCM_Family_Member.Add(Vi);
                    db.SaveChanges();







                }
                catch (Exception dbEx)
                {
                    Exception raise = dbEx;
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        string message = string.Format("{0}:{1}",
                    //            validationErrors.Entry.Entity.ToString(),
                    //            validationError.ErrorMessage);
                    //        // raise a new exception nesting
                    //        // the current instance as InnerException
                    //        raise = new InvalidOperationException(message, raise);
                    //    }
                    //}
                    throw raise;
                }
            }
        }

        #endregion



        //..........................................................................DROPDOWNS.........................................

        #region DROPDOWNS


        public List<HasLegalLookupPCM> GetHasLegal()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var period = db.apl_PCM_Has_Legal_Representive.Select(o => new HasLegalLookupPCM
                {
                    HasLegal_Id = o.HasLegal_Id,
                    Description = o.Description
                }).ToList();

                return period;
            }
        }

        public List<PCMOffenseSchedulesLookup> GetOffenceSchedules()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Offesche = db.apl_Offense_Schedules.Select(o => new PCMOffenseSchedulesLookup
                {
                    Offence_Schedule_Id = o.Offence_Schedule_Id,
                    Description = o.Description
                }).ToList();

                return Offesche;
            }
        }

        public List<PCMWhenEscapedLookup> GetEscapePeriod()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var period = db.apl_Escape_Period.Select(o => new PCMWhenEscapedLookup
                {
                    When_Escaped_Id = o.When_Escaped_Id,
                    Description = o.Description
                }).ToList();

                return period;
            }
        }

        public List<PCMGenderLookup> GetAllGender()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var g = db.Genders.Select(o => new PCMGenderLookup
                {
                    Gender_Id = o.Gender_Id,
                    Description = o.Description
                }).ToList();

                return g;
            }
        }


        public List<PCMRequestStatusLookup> GetBedSpaceRequeststatus()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_BedSpace_Request.Select(o => new PCMRequestStatusLookup
                {
                    Request_Status_Id = o.Request_Status_Id,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }
        public List<PCMAdmissionTypeLookup> GetAdmissionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_Admission_Type.Select(o => new PCMAdmissionTypeLookup
                {
                    Admission_Type_Id = o.Admission_Type_Id,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }

        public List<PCMChargeLookup> GetAllCharges()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_PCM_Charges.Select(o => new PCMChargeLookup
                {
                    Charge_Id = o.Charge_Id,
                    Charge_Description = o.Charge_Description

                }).ToList();

                var Type2 = Type.OrderBy(b => b.Charge_Description).ToList();

                return Type2;
            }
        }

        public List<DiversionProgrammesLookupPcm> GetAllDiversion_Programmes()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_Diversion_Programmes.Select(o => new DiversionProgrammesLookupPcm
                {
                    Div_Program_Id = o.Div_Program_Id,
                    Programme_Name = o.Programme_Name
                }).ToList();

                return Type;
            }
        }
        public List<OrganisationTypeLookupPCM> GetAllOrganisationType() 
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupPCM
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }
        public List<LocalMunicipalityLookupAdopt> GetAllLocalMunicipality()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Local_Mun_id = db.Local_Municipalities.Select(o => new LocalMunicipalityLookupAdopt
                {
                    Local_Municipality_Id = o.Local_Municipality_Id,
                    Description = o.Description
                }).ToList();

                return Local_Mun_id;


            }
        }

        public List<TownLookupPCM> GetAllTowns()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var town = db.Towns.Select(o => new TownLookupPCM
                {
                    Town_Id = o.Town_Id,
                    Description = o.Description
                }).ToList();

                return town;


            }
        }

        public List<OrganizationLookupPcm> GetAllPCMOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.Organizations.Select(o => new OrganizationLookupPcm
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }
        public List<RecomendationOrderLookupPcm> GetOrder()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_PCM_Orders.Select(o => new RecomendationOrderLookupPcm
                {
                    Recomendation_Order_Id = o.Recomendation_Order_Id,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }

        public List<PlacementTypeLookupPcm> GetPlacementType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_Placement_Type.Select(o => new PlacementTypeLookupPcm
                {
                    Placement_Type_Id = o.Placement_Type_Id,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }
        public List<RecommendationTypeLookupPcm> GetRecommendationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Type = db.apl_Recommendation_Type.Select(o => new RecommendationTypeLookupPcm
                {
                    Recommendation_Type_Id = o.Recommendation_Type_Id,
                    Description = o.Description
                }).ToList();

                return Type;
            }
        }

        public List<RelationshipTypeLookupPcm> GetAllRelationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var RelationType = db.Relationship_Types.Select(o => new RelationshipTypeLookupPcm
                {
                    Relationship_Type_Id = o.Relationship_Type_Id,
                    Description = o.Description
                }).ToList();

                return RelationType;
            }
        }

        public List<PCMHealthStatusLookup> GetHealth()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var id = db.apl_Health_Status.Select(o => new PCMHealthStatusLookup
                {
                    Health_Status_Id = o.Health_Status_Id,
                    Description = o.Description
                }).ToList();

                return id;
            }
        }

        public List<ProvinceLookupPCM> GetAllProvinces()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Province_id = db.Provinces.Select(o => new ProvinceLookupPCM
                {
                    Province_Id = o.Province_Id,
                    Description = o.Description
                }).ToList();

                return Province_id;
            }
        }

        public List<PCMOffenceTypeLookup> GetOffenceType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var id = db.apl_Offence_Type.Select(o => new PCMOffenceTypeLookup
                {
                    Offence_Type_Id = o.Offence_Type_Id,
                    Description = o.Description
                }).ToList();

                return id;
            }
        }
        public List<PCMOffenceCategoryLookup> GetOffenceCategory()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var id = db.Offence_Categories.Select(o => new PCMOffenceCategoryLookup
                {
                    Offence_Category_Id = o.Offence_Category_Id,
                    Description = o.Description
                }).ToList();

                return id;
            }
        }

        public List<DistrictLookupPCM> GetAllDistrictByCourtID(int DistrictId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                var districtlist = db.Districts.Where(x => x.District_Id == DistrictId).ToList().Select(o => new DistrictLookupPCM
                {
                    District_Id = o.District_Id,
                    Description = o.Description

                }).ToList();

                return districtlist;
            }
        }

        public List<Place_Of_DetentionLookupPCM> GetAllPlaceOfDetention()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Place_Of_Detention_id = db.apl_Place_Of_Detention.Select(o => new Place_Of_DetentionLookupPCM
                {
                    Place_Of_Detention_Id = o.Place_Of_Detention_Id,
                    Description = o.Description
                }).ToList();

                return Place_Of_Detention_id;


            }
        }
        public List<DistrictLookupPCM> GetAllDistrict()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var District_id = db.Districts.Select(o => new DistrictLookupPCM
                {
                    District_Id = o.District_Id,
                    Description = o.Description
                }).ToList();

                return District_id;


            }
        }
        public List<IdentificationTypeLookup> GetAllIdentificationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var idType = db.Identification_Types.Select(o => new IdentificationTypeLookup
                {
                    Identification_Type_Id = o.Identification_Type_Id,
                    Description = o.Description
                }).ToList();

                return idType;
            }
        }

        public List<LanguageTypeLookup> GetAllLanguageType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var languageType = db.Languages.Select(o => new LanguageTypeLookup
                {
                    Language_Id = o.Language_Id,
                    Description = o.Description
                }).ToList();

                return languageType;
            }
        }

        public List<GenderTypeLookup> GetAllGenderType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var genderType = db.Genders.Select(o => new GenderTypeLookup
                {
                    Gender_Id = o.Gender_Id,
                    Description = o.Description
                }).ToList();

                return genderType;
            }
        }

        public List<MaritalTypeLookup> GetAllMaritalType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var maritalType = db.Marital_Statusses.Select(o => new MaritalTypeLookup
                {
                    Marital_Status_Id = o.Marital_Status_Id,
                    Description = o.Description
                }).ToList();

                return maritalType;
            }
        }

        public List<ContactTypeLookup> GetAllContactType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var contactType = db.Preferred_Contact_Types.Select(o => new ContactTypeLookup
                {
                    Preferred_Contact_Type_Id = o.Preferred_Contact_Type_Id,
                    Description = o.Description
                }).ToList();

                return contactType;
            }
        }

        public List<ReligionTypeLookup> GetAllReligionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var religionType = db.Religions.Select(o => new ReligionTypeLookup
                {
                    Religion_Id = o.Religion_Id,
                    Description = o.Description
                }).ToList();

                return religionType;
            }
        }

        public List<Population_GroupTypeLookup> GetAllPopulationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var populationType = db.Population_Groups.Select(o => new Population_GroupTypeLookup
                {
                    Population_Group_Id = o.Population_Group_Id,
                    Description = o.Description
                }).ToList();

                return populationType;
            }
        }

        public List<NationalityTypeLookup> GetAllNationalityType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var nationalityType = db.Nationalities.Select(o => new NationalityTypeLookup
                {
                    Nationality_Id = o.Nationality_Id,
                    Description = o.Description
                }).ToList();

                return nationalityType;
            }
        }

        public List<DisabilityTypeLookup> GetAllDisabilityType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var disabilityType = db.Disabilities.Select(o => new DisabilityTypeLookup
                {
                    Disability_Type_Id = o.Disability_Id,
                    Description = o.Description
                }).ToList();

                return disabilityType;
            }
        }

        public List<SAPSLookup> GetAllSAPStation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var sapSt = db.SAPS_Stations.Select(o => new SAPSLookup
                {
                    SAPS_Station_Id = o.SAPS_Station_Id,
                    Description = o.Description
                }).ToList();

                return sapSt;
            }
        }

        public List<SAPS_OfficialLookup> GetAllSAPSOfficial()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var sapOff = db.SAPS_Officials.Select(o => new SAPS_OfficialLookup
                {
                    SAPS_Info_Id = o.SAPS_Official_Id,
                    Description = o.First_Name
                }).ToList();

                return sapOff;
            }
        }

        public List<CourtLookup> GetAllCourt()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var sapCourt = db.Courts.Select(o => new CourtLookup
                {
                    Court_id = o.Court_Id,
                    Description = o.Description
                }).ToList();

                return sapCourt;
            }
        }

        public List<TownTypeLookup> GetAllTownType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var TownType = db.Towns.Select(o => new TownTypeLookup
                {
                    Town_Id = o.Town_Id,
                    Description = o.Description
                }).ToList();

                return TownType;
            }
        }

        public List<NotificationTypeLookup> GetAllNotificationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var NotificationType = db.apl_Form_Of_Notification.Select(o => new NotificationTypeLookup
                {
                    Form_Of_Notification_Id = o.Form_Of_Notification_Id,
                    Description = o.Description
                }).ToList();

                return NotificationType;
            }
        }

        public List<ChronicTypeLookup> GetAllChronicType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var ChronicType = db.Chronic_Illnesses.Select(o => new ChronicTypeLookup
                {
                    Chronic_Illness_Id = o.Chronic_Illness_Id,
                    Description = o.Description
                }).ToList();

                return ChronicType;
            }
        }

        public List<AllergyTypeLookup> GetAllAllergyType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var AllergyType = db.Allergies.Select(o => new AllergyTypeLookup
                {
                    Allergy_Id = o.Allergy_Id,
                    Description = o.Description
                }).ToList();

                return AllergyType;
            }
        }

        #endregion



    }

}
