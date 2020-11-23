using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.ViewModels;

namespace Common_Objects.Models
{
   public class PCMPresentenceModel
    {
        #region  GET PERSON BY ASSESSMENT 

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
            pcvm.Identification_Number = act.Identification_Number;
            pcvm.Date_Of_Birth = act.Date_Of_Birth;
            return pcvm;
        }



        #endregion

        #region Presentence Sammary

        public int GetPCMPresentenseSummaryByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Presentence_Summaries on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Presentence_Summary_Id).FirstOrDefault();
            }
        }

        public PCMPresentenceDetailsViewModel GetPresentenseSummaryList(int Summary_Id)
        {
            PCMPresentenceDetailsViewModel fvm = new PCMPresentenceDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_Summaries act = db.PCM_Presentence_Summaries.Find(Summary_Id);

                    fvm.Presentence_Summary_Id = act.Presentence_Summary_Id;
                    fvm.Education_Sammary = act.Education_Sammary;
                    fvm.Offence_Sammary = act.Offence_Sammary;
                    fvm.Victim_Sammary = act.Victim_Sammary;
                    fvm.IsAssessed = act.IsAssessed;
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

        public void InsertPresentenceSummaries(PCMPresentenceDetailsViewModel cases, int intassid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_Summaries newCase = new PCM_Presentence_Summaries();

                    newCase.Intake_Assessment_Id = intassid;
                    newCase.Education_Sammary = cases.Education_Sammary;
                    newCase.Offence_Sammary = cases.Offence_Sammary;
                    newCase.Victim_Sammary = cases.Victim_Sammary;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_Presentence_Summaries.Add(newCase);


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

        public void UpdatePresentenceSummaries(PCMPresentenceDetailsViewModel cases, int userId, int Summary_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_Summaries editCase = db.PCM_Presentence_Summaries.Find(Summary_Id);

                    if (Summary_Id > 0)
                    {
                        /****** Script for SelectTopNRows command from SSMS  ******/

                        editCase.Education_Sammary = cases.Education_Sammary;
                        editCase.IsAssessed = cases.IsAssessed;
                        editCase.Offence_Sammary = cases.Offence_Sammary;
                        editCase.Victim_Sammary = cases.Victim_Sammary;
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

        #region Presentence Details

        public int GetPCMPresentenseByassId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.PCM_Presentence_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Presentence_Id).FirstOrDefault();
            }
        }

        public PCMPresentenceDetailsViewModel GetPresentenseList(int Presentence_Id)
        {
            PCMPresentenceDetailsViewModel fvm = new PCMPresentenceDetailsViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_Details act = db.PCM_Presentence_Details.Find(Presentence_Id);

                    fvm.Presentence_Id = act.Presentence_Id;
                    fvm.Court_id = act.Referral_Court_Id;
                    fvm.Date_Request_Received = act.Date_Request_Received;
                    fvm.Court_Appearance_Date = act.Court_Appearance_Date;
                    fvm.Date_Report_Submitted_To_Court = act.Date_Report_Submitted_To_Court;
                    fvm.Reasons_For_Non_Submission = act.Reasons_For_Non_Submission;
                    fvm.Sentencing_Options = act.Sentencing_Options;
                    fvm.Community_Based_Options_Id = act.Community_Based_Options_Id;
                    fvm.Restorective_Justice_Option_Id = act.Restorective_Justice_Option_Id;
                    fvm.Programme_Type_Id = act.Programme_Type_Id;
                    fvm.Programme_Id = act.Programme_Id;
                    fvm.Fine_or_Alternatives_To_Fine = act.Fine_or_Alternatives_To_Fine ?? false; 
                    fvm.Fine_Alternatives_Fine_Comments = act.Fine_Alternatives_Fine_Comments;
                    fvm.Suspended_Postponed_Sentence_Id = act.Suspended_Postponed_Sentence_Id;
                    fvm.Commital_Treatment_Centre = act.Commital_Treatment_Centre ?? false; 
                    fvm.Center_Name = act.Center_Name;
                    fvm.CYCCCenter_Name = act.Center_Name_CYCC;
                    fvm.Period_Commital_Treatment_Centre_From = act.Period_Commital_Treatment_Centre_From;
                    fvm.Period_Commital_Treatment_Centre_To = act.Period_Commital_Treatment_Centre_To;
                    fvm.Compulsory_esidence_CYCC = act.Compulsory_esidence_CYCC ?? false; 
                    fvm.Compulsory_esidence_CYCC_From = act.Compulsory_esidence_CYCC_From;
                    fvm.Compulsory_esidence_CYCC_To = act.Compulsory_esidence_CYCC_To;
                    fvm.Imprisoment = act.Imprisoment ?? false;
                    fvm.Imprisoment_Id = act.Imprisoment_Id;
                    fvm.Imprisomen_From = act.Imprisomen_From;
                    fvm.Imprisomen_To = act.Imprisomen_To;
                    fvm.Department_Id = act.Department_Id;
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

        public void InsertPresentence(PCMPresentenceDetailsViewModel cases, int intassid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_Details newCase = new PCM_Presentence_Details();

                    newCase.Intake_Assessment_Id = intassid;
                    newCase.Referral_Court_Id = cases.Court_id;
                    newCase.Date_Request_Received = cases.Date_Request_Received;
                    newCase.Court_Appearance_Date = cases.Court_Appearance_Date;
                    newCase.Date_Report_Submitted_To_Court = cases.Date_Report_Submitted_To_Court;
                    newCase.Reasons_For_Non_Submission = cases.Reasons_For_Non_Submission;
                    newCase.Sentencing_Options = cases.Sentencing_Options;
                    newCase.Community_Based_Options_Id = cases.Community_Based_Options_Id;
                    newCase.Restorective_Justice_Option_Id = cases.Restorective_Justice_Option_Id;
                    newCase.Programme_Type_Id = cases.Programme_Type_Id;
                    newCase.Programme_Id = cases.Programme_Id;
                    newCase.Fine_or_Alternatives_To_Fine =cases. Fine_or_Alternatives_To_Fine;
                    newCase.Fine_Alternatives_Fine_Comments = cases.Fine_Alternatives_Fine_Comments;
                    newCase.Suspended_Postponed_Sentence_Id = cases.Suspended_Postponed_Sentence_Id;
                    newCase.Commital_Treatment_Centre = cases.Commital_Treatment_Centre;
                    newCase.Center_Name = cases.Center_Name;
                    newCase.Center_Name_CYCC = cases.CYCCCenter_Name;
                    newCase.Period_Commital_Treatment_Centre_From = cases.Period_Commital_Treatment_Centre_From;
                    newCase.Period_Commital_Treatment_Centre_To = cases.Period_Commital_Treatment_Centre_To;
                    newCase.Compulsory_esidence_CYCC = cases.Compulsory_esidence_CYCC;
                    newCase.Compulsory_esidence_CYCC_From = cases.Compulsory_esidence_CYCC_From;
                    newCase.Compulsory_esidence_CYCC_To = cases.Compulsory_esidence_CYCC_To;
                    newCase.Imprisoment = cases.Imprisoment;
                    newCase.Imprisoment_Id = cases.Imprisoment_Id;
                    newCase.Imprisomen_From = cases.Imprisomen_From;
                    newCase.Imprisomen_To = cases.Imprisomen_To;
                    newCase.Department_Id = cases.Department_Id;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    db.PCM_Presentence_Details.Add(newCase);


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

        public void UpdatePresentence(PCMPresentenceDetailsViewModel cases, int userId, int Summary_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_Details editCase = db.PCM_Presentence_Details.Find(Summary_Id);

                    if (Summary_Id > 0)
                    {
                        
                        editCase.Referral_Court_Id = cases.Court_id;
                        editCase.Date_Request_Received = cases.Date_Request_Received;
                        editCase.Court_Appearance_Date = cases.Court_Appearance_Date;
                        editCase.Date_Report_Submitted_To_Court = cases.Date_Report_Submitted_To_Court;
                        editCase.Reasons_For_Non_Submission = cases.Reasons_For_Non_Submission;
                        editCase.Sentencing_Options = cases.Sentencing_Options;
                        editCase.Community_Based_Options_Id = cases.Community_Based_Options_Id;
                        editCase.Restorective_Justice_Option_Id = cases.Restorective_Justice_Option_Id;
                        editCase.Programme_Type_Id = cases.Programme_Type_Id;
                        editCase.Programme_Id = cases.Programme_Id;
                        editCase.Fine_or_Alternatives_To_Fine = cases.Fine_or_Alternatives_To_Fine;
                        editCase.Fine_Alternatives_Fine_Comments = cases.Fine_Alternatives_Fine_Comments;
                        editCase.Suspended_Postponed_Sentence_Id = cases.Suspended_Postponed_Sentence_Id;
                        editCase.Commital_Treatment_Centre = cases.Commital_Treatment_Centre;
                        editCase.Center_Name = cases.Center_Name;
                        editCase.Center_Name_CYCC = cases.CYCCCenter_Name;
                        editCase.Period_Commital_Treatment_Centre_From = cases.Period_Commital_Treatment_Centre_From;
                        editCase.Period_Commital_Treatment_Centre_To = cases.Period_Commital_Treatment_Centre_To;
                        editCase.Compulsory_esidence_CYCC = cases.Compulsory_esidence_CYCC;
                        editCase.Compulsory_esidence_CYCC_From = cases.Compulsory_esidence_CYCC_From;
                        editCase.Compulsory_esidence_CYCC_To = cases.Compulsory_esidence_CYCC_To;
                        editCase.Imprisoment = cases.Imprisoment;
                        editCase.Imprisoment_Id = cases.Imprisoment_Id;
                        editCase.Imprisomen_From = cases.Imprisomen_From;
                        editCase.Imprisomen_To = cases.Imprisomen_To;
                        editCase.Department_Id = cases.Department_Id;
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

        #region Presentence Outcome
        public List<PCMPresentenceDetailsViewModel> GetSentenseList(int IntakeassId)
        {
            // initialising view model 
            List<PCMPresentenceDetailsViewModel> avm = new List<PCMPresentenceDetailsViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in


            var ListP = (
                 from p in db.Persons
                 join c in db.Clients on p.Person_Id equals c.Person_Id
                 join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                 join Case in db.PCM_Presentence_OutCome on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                 join status in db.apl_PCM_Case_Status on Case.PCM_Case_Status_Id equals status.PCM_Case_Status_Id
                 
                 where i.Intake_Assessment_Id.Equals(IntakeassId)
                  select new
                  {
                      i.Intake_Assessment_Id,
                      Case.Sentence_Id,
                      Case.Court_Date,
                      Case.NextCourtDate,
                      Case.PCM_Case_Status_Id,
                      Case.Court_Outcome

                  }).ToList();
            
            foreach (var item in ListP)
            {

                // initialising view model
                PCMPresentenceDetailsViewModel obj = new PCMPresentenceDetailsViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Sentence_Id = item.Sentence_Id;
                obj.Court_Date = item.Court_Date;
                obj.NextCourtDate = item.NextCourtDate;
                obj.Court_Outcome = item.Court_Outcome;
                obj.SelectCaseStatusDetails = db.apl_PCM_Case_Status.Find(item.PCM_Case_Status_Id).Description;


                avm.Add(obj);
            }

            return avm;
        }

        public PCMPresentenceDetailsViewModel GetPCMSentenseOnEditDetails(int Sentence_Id)
        {
            PCMPresentenceDetailsViewModel vm = new PCMPresentenceDetailsViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_OutCome act = db.PCM_Presentence_OutCome.Find(Sentence_Id);

                    if (act != null)
                    {
                        vm.Sentence_Id = act.Sentence_Id;
                        vm.Court_Date = act.Court_Date;
                        vm.Reason_for_Remand = act.Reason_for_Remand;
                        vm.NextCourtDate = act.NextCourtDate;
                        vm.Court_Outcome = act.Court_Outcome;
                        vm.Community_Based_Options_Id = act.Community_Based_Options_Id;
                        if (act.Community_Based_Options_Id != null)
                        {
                            vm.SelectCommunityBasedOptionsDetails = db.apl_PCM_Community_Based_Options.Find(act.Community_Based_Options_Id).Description;
                        }
                        vm.Restorective_Justice_Option_Id=act.Restorective_Justice_Option_Id;
                        if (act.Restorective_Justice_Option_Id != null)
                        {
                            vm.SelectRestorectiveJusticeDetails = db.apl_Restorective_Justice_Types.Find(act.Restorective_Justice_Option_Id).Description;
                        }
                       
                        vm.Programme_Type_Id = act.Programme_Type_Id;
                        if (act.Programme_Type_Id != null)
                        {
                            vm.SelectProgrammeTypeDetails = db.apl_Programme_Type.Find(act.Programme_Type_Id).Description;
                        }
                        
                        vm.Programme_Id = act.Programme_Id;
                        if (act.Programme_Id != null)
                        {
                            vm.SelectProgrammeDetails = db.apl_Programmes.Find(act.Programme_Id).Programme_Name;
                        }
                       
                        vm.Fine_or_Alternatives_To_Fine = act.Fine_or_Alternatives_To_Fine ?? false; 
                        vm.Fine_Alternatives_Fine_Comments = act.Fine_Alternatives_Fine_Comments;
                        vm.Suspended_Postponed_Sentence_Id = act.Suspended_Postponed_Sentence_Id;
                        if (act.Suspended_Postponed_Sentence_Id != null)
                        {
                            vm.SelectSupendedPostponedSentenceDetails = db.apl_PCM_Supended_Postponed_Sentence.Find(act.Suspended_Postponed_Sentence_Id).Description;
                        }
                        
                        vm.Commital_Treatment_Centre = act.Commital_Treatment_Centre ?? false; ;
                        vm.Center_Name = act.Center_Name;
                        vm.CYCCCenter_Name = act.Center_Name_CYCC;
                        vm.Period_Commital_Treatment_Centre_From = act.Period_Commital_Treatment_Centre_From;
                        vm.Period_Commital_Treatment_Centre_To = act.Period_Commital_Treatment_Centre_To;
                        vm.Compulsory_esidence_CYCC = act.Compulsory_esidence_CYCC ?? false; 
                        vm.Compulsory_esidence_CYCC_From = act.Compulsory_esidence_CYCC_From;
                        vm.Compulsory_esidence_CYCC_To = act.Compulsory_esidence_CYCC_To;
                        vm.Imprisoment = act.Imprisoment ?? false;
                        vm.Imprisoment_Id = act.Imprisoment_Id;
                        if (act.Imprisoment_Id != null)
                        {
                            vm.SelectImprisomentDetails = db.apl_PCM_Imprisoment.Find(act.Imprisoment_Id).Description;
                        }
                       
                        vm.Imprisomen_From = act.Imprisomen_From;
                        vm.Imprisomen_To = act.Imprisomen_To;
                        vm.Department_Id = act.Department_Id;
                        if (act.Department_Id != null)
                        {
                            vm.SelectDepartmentDetails = db.apl_Department.Find(act.Department_Id).Description;
                        }
                        
                        vm.PCM_Case_Status_Id = act.PCM_Case_Status_Id;
                        if (act.PCM_Case_Status_Id != null)
                        {
                            vm.SelectCaseStatusDetails = db.apl_PCM_Case_Status.Find(act.PCM_Case_Status_Id).Description;
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

        public void CreateSentenseDeatils(PCMPresentenceDetailsViewModel vm, int caseid, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_OutCome newCase = new PCM_Presentence_OutCome();
                    newCase.Court_Date = vm.Court_Date;
                    newCase.Reason_for_Remand = vm.Reason_for_Remand;
                    newCase.NextCourtDate = vm.NextCourtDate;
                    newCase.Court_Outcome = vm.Court_Outcome;
                    newCase.Community_Based_Options_Id = vm.Community_Based_Options_Id;
                    newCase.Restorective_Justice_Option_Id = vm.Restorective_Justice_Option_Id;
                    newCase.Programme_Type_Id = vm.Programme_Type_Id;
                    newCase.Programme_Id = vm.Programme_Id;
                    newCase.Fine_or_Alternatives_To_Fine = vm.Fine_or_Alternatives_To_Fine;
                    newCase.Fine_Alternatives_Fine_Comments = vm.Fine_Alternatives_Fine_Comments;
                    newCase.Suspended_Postponed_Sentence_Id = vm.Suspended_Postponed_Sentence_Id;
                    newCase.Commital_Treatment_Centre = vm.Commital_Treatment_Centre;
                    newCase.Center_Name = vm.Center_Name;
                    newCase.Center_Name_CYCC = vm.CYCCCenter_Name;
                    newCase.Period_Commital_Treatment_Centre_From = vm.Period_Commital_Treatment_Centre_From;
                    newCase.Period_Commital_Treatment_Centre_To = vm.Period_Commital_Treatment_Centre_To;
                    newCase.Compulsory_esidence_CYCC = vm.Compulsory_esidence_CYCC;
                    newCase.Compulsory_esidence_CYCC_From = vm.Compulsory_esidence_CYCC_From;
                    newCase.Compulsory_esidence_CYCC_To = vm.Compulsory_esidence_CYCC_To;
                    newCase.Imprisoment = vm.Imprisoment;
                    newCase.Imprisoment_Id = vm.Imprisoment_Id;
                    newCase.Imprisomen_From = vm.Imprisomen_From;
                    newCase.Imprisomen_To = vm.Imprisomen_To;
                    newCase.Department_Id = vm.Department_Id;
                    newCase.PCM_Case_Status_Id = 1;
                    newCase.Created_By = userId;
                    newCase.Date_Created = DateTime.Now;
                    newCase.Intake_Assessment_Id = caseid;

                    db.PCM_Presentence_OutCome.Add(newCase);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }

        public void UpdateSentenseDetails(PCMPresentenceDetailsViewModel vm, int Sentence_Id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    PCM_Presentence_OutCome newCase = db.PCM_Presentence_OutCome.Find(Sentence_Id);

                    newCase.Sentence_Id = Sentence_Id;
                    newCase.Court_Date = vm.Court_Date;
                    newCase.Reason_for_Remand = vm.Reason_for_Remand;
                    newCase.NextCourtDate = vm.NextCourtDate;
                    newCase.Court_Outcome = vm.Court_Outcome;
                    newCase.Community_Based_Options_Id = vm.Community_Based_Options_Id;
                    newCase.Restorective_Justice_Option_Id = vm.Restorective_Justice_Option_Id;
                    newCase.Programme_Type_Id = vm.Programme_Type_Id;
                    newCase.Programme_Id = vm.Programme_Id;
                    newCase.Fine_or_Alternatives_To_Fine = vm.Fine_or_Alternatives_To_Fine;
                    newCase.Fine_Alternatives_Fine_Comments = vm.Fine_Alternatives_Fine_Comments;
                    newCase.Suspended_Postponed_Sentence_Id = vm.Suspended_Postponed_Sentence_Id;
                    newCase.Commital_Treatment_Centre = vm.Commital_Treatment_Centre;
                    newCase.Center_Name = vm.Center_Name;
                    newCase.Center_Name_CYCC = vm.CYCCCenter_Name;
                    newCase.Period_Commital_Treatment_Centre_From = vm.Period_Commital_Treatment_Centre_From;
                    newCase.Period_Commital_Treatment_Centre_To = vm.Period_Commital_Treatment_Centre_To;
                    newCase.Compulsory_esidence_CYCC = vm.Compulsory_esidence_CYCC;
                    newCase.Compulsory_esidence_CYCC_From = vm.Compulsory_esidence_CYCC_From;
                    newCase.Compulsory_esidence_CYCC_To = vm.Compulsory_esidence_CYCC_To;
                    newCase.Imprisoment = vm.Imprisoment;
                    newCase.Imprisoment_Id = vm.Imprisoment_Id;
                    newCase.Imprisomen_From = vm.Imprisomen_From;
                    newCase.Imprisomen_To = vm.Imprisomen_To;
                    newCase.Department_Id = vm.Department_Id;
                    newCase.PCM_Case_Status_Id = vm.PCM_Case_Status_Id;
                    newCase.Modified_By = userId;
                    newCase.Date_Modified = DateTime.Now;

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

        #region Dropdowns

        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMCommunityBasedOptionsLookup> GetCommunityBasedOption()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_PCM_Community_Based_Options.Select(o => new PCMPresentenceDetailsViewModel.PCMCommunityBasedOptionsLookup
                {
                    Community_Based_Options_Id = o.Community_Based_Options_Id,
                    Description = o.Description
                }).ToList();

                return obj;
            }
        }


        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMSupendedPostponedSentenceLookup> GetSupendedPostponedSentence()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_PCM_Supended_Postponed_Sentence.Select(o => new PCMPresentenceDetailsViewModel.PCMSupendedPostponedSentenceLookup
                {
                    Supended_Postponed_Sentence_Id = o.Suspended_Postponed_Sentence_Id,
                    Description = o.Description
                }).ToList();

                return obj;
            }
        }

        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMCaseStatusLookup> GetCaseStatus()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_PCM_Case_Status.Select(o => new PCMPresentenceDetailsViewModel.PCMCaseStatusLookup
                {
                    PCM_Case_Status_Id = o.PCM_Case_Status_Id,
                    Description = o.Description
                }).ToList();

                return obj;
            }
        }


        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMRestorectiveJusticeLookup> GetRestorectiveJustice()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_Restorective_Justice_Types.Select(o => new PCMPresentenceDetailsViewModel.PCMRestorectiveJusticeLookup
                {
                    Restorective_Justice_Option_Id = o.Restorective_Justice_Option_Id,
                    Description = o.Description
                }).ToList();

                return obj;
            }
        }

        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMProgrammeTypeLookup> GetProgrammeType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_Programme_Type.Select(o => new PCMPresentenceDetailsViewModel.PCMProgrammeTypeLookup
                {
                    Programme_Type_Id = o.Programme_Type_Id,
                    Description = o.Description
                }).ToList();

                return obj;
            }
        }

        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMProgrammeLookup> GetProgramme()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_Programmes.Select(o => new PCMPresentenceDetailsViewModel.PCMProgrammeLookup
                {
                    Programme_Id = o.Programme_Id,
                    Description = o.Programme_Name
                }).ToList();

                return obj;
            }
        }

        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMImprisomentLookup> GetImprisoment()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_PCM_Imprisoment.Select(o => new PCMPresentenceDetailsViewModel.PCMImprisomentLookup
                {
                    Imprisoment_Id = o.Imprisoment_Id,
                    Description = o.Description
                }).ToList();

                return obj;
            }
        }

        public List<Common_Objects.ViewModels.PCMPresentenceDetailsViewModel.PCMDepartmentLookup> GetDepartment()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var obj = db.apl_Department.Select(o => new PCMPresentenceDetailsViewModel.PCMDepartmentLookup
                {
                    Department_Id = o.Department_Id,
                    Description = o.Description
                }).ToList();

                return obj;
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

        public List<DistrictLookupPCM> GetAllDistrictByCourtID( int court)
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

        #endregion

    }
}
