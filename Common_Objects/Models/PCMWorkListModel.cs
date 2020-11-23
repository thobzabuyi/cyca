using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class PCMWorkListModel
    {

        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();


        public List<PCMSocialWorkerWorkListVM> PCMGetUserWorkList(int useridlogged)
        {
            // initialising view model 
            List<PCMSocialWorkerWorkListVM> avm = new List<PCMSocialWorkerWorkListVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var workList =
                (from list in db.PCM_WorkList
                 join status in db.apl_PCM_Record_Status on list.PCM_Record_Status_Id equals
                 status.PCM_Record_Status_Id
                 join Assessment in db.Intake_Assessments on list.Intake_Assessment_Id equals Assessment.Intake_Assessment_Id
                 join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                 join person in db.Persons on client.Person_Id equals person.Person_Id
                 join sub in db.Problem_Sub_Categories on Assessment.Problem_Sub_Category_Id equals sub.Problem_Sub_Category_Id
                 where (list.Manager == useridlogged)
                 select new
                 {
                     list.Intake_Assessment_Id,
                     list.PCMCaseWoklist_Id,
                     list.Allocated_By,
                     list.Manager,
                     list.Date_Allocated,
                     list.Reference_Number,
                     list.PCM_Record_Status_Id,
                     list.Date_Accepted,
                     person.First_Name,
                     person.Last_Name,
                     person.Identification_Number,
                     sub.Problem_Sub_Category_Id

                 }).ToList();
            ;
            foreach (var item in workList)
            {

                // initialising view model
                PCMSocialWorkerWorkListVM obj = new PCMSocialWorkerWorkListVM();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.PCMCaseWoklist_Id = item.PCMCaseWoklist_Id;
                obj.Allocated_By = item.Allocated_By;
                obj.Allocate_To = item.Manager;
                obj.Date_Allocated = item.Date_Allocated;
                obj.Reference_Number = item.Reference_Number;
                obj.RecordStatusDescription = db.apl_PCM_Record_Status.Find(item.PCM_Record_Status_Id).Description;
                obj.Date_Accepted = item.Date_Accepted;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                obj.ProblemSubCategoryDescription = db.Problem_Sub_Categories.Find(item.Problem_Sub_Category_Id).Description;

                avm.Add(obj);
            }

            return avm;
        }


        public List<PCMSocialWorkerWorkListVM> GetPCMUserNewWorkList(int useridlogged)
        {
            // initialising view model 
            List<PCMSocialWorkerWorkListVM> avm = new List<PCMSocialWorkerWorkListVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            // get problem category for adoption


            // get work list for user logged in
            var newworkList =

                (from list in db.PCM_WorkList
                 join status in db.apl_PCM_Record_Status on list.PCM_Record_Status_Id equals
                 status.PCM_Record_Status_Id
                 join Assessment in db.Intake_Assessments on list.Intake_Assessment_Id equals Assessment.Intake_Assessment_Id
                 join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                 join person in db.Persons on client.Person_Id equals person.Person_Id
                 join sub in db.Problem_Sub_Categories on Assessment.Problem_Sub_Category_Id equals sub.Problem_Sub_Category_Id

                 where (list.Manager == useridlogged && status.PCM_Record_Status_Id == 1)
                 select new
                 {
                     list.Intake_Assessment_Id,
                     list.PCMCaseWoklist_Id,
                     list.Allocated_By,
                     list.Manager,
                     list.Date_Allocated,
                     list.Reference_Number,
                     list.PCM_Record_Status_Id,
                     list.Date_Accepted,
                     person.First_Name,
                     person.Last_Name,
                     person.Identification_Number,
                     sub.Problem_Sub_Category_Id


                 }).ToList();
            ;
            foreach (var item in newworkList)
            {

                // initialising view model
                PCMSocialWorkerWorkListVM obj = new PCMSocialWorkerWorkListVM();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.PCMCaseWoklist_Id = item.PCMCaseWoklist_Id;
                obj.Allocated_By = item.Allocated_By;
                obj.Allocate_To = item.Manager;
                obj.Date_Allocated = item.Date_Allocated;
                obj.Reference_Number = item.Reference_Number;
                obj.RecordStatusDescription = db.apl_PCM_Record_Status.Find(item.PCM_Record_Status_Id).Description;
                obj.Date_Accepted = item.Date_Accepted;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                obj.ProblemSubCategoryDescription = db.Problem_Sub_Categories.Find(item.Problem_Sub_Category_Id).Description;
                avm.Add(obj);
            }

            return avm;
        }

        public List<PCMSocialWorkerEndpointCasesVM> GetNewEndpointAlloctedCasesList(int useridlogged)
        {
            // initialising view model 
            List<PCMSocialWorkerEndpointCasesVM> avm = new List<PCMSocialWorkerEndpointCasesVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            // get problem category for adoption


            // get work list for user logged in
            var newworkList =

                (from list in db.PCM_EndPoint_PO_Inbox
                 join status in db.apl_PCM_Endpoint_Record_Status on list.Endpoint_Record_Status_Id equals
                 status.Endpoint_Record_Status_Id
                 join person in db.Persons on list.Person_Id equals person.Person_Id


                 where (list.Case_Allocated_To == useridlogged && status.Endpoint_Record_Status_Id == 1)
                 select new
                 {
                   
                     list.End_Point_POD_Id,
                     list.Case_Allocated_To,
                     list.Date_Recieved,
                     list.Endpoint_Record_Status_Id,
                     list.Allocated_ByS,
                     person.First_Name,
                     person.Last_Name,
                     person.Identification_Number


                 }).ToList();
            ;
            foreach (var item in newworkList)
            {

                // initialising view model
                PCMSocialWorkerEndpointCasesVM obj = new PCMSocialWorkerEndpointCasesVM();

      
                obj.End_Point_POD_Id = item.End_Point_POD_Id;
                obj.Allocated_By = item.Allocated_ByS;
                obj.POAllocate_To = item.Case_Allocated_To;
                obj.Date_Recieved = item.Date_Recieved;
                obj.EndpointRecordStatusDescription = db.apl_PCM_Endpoint_Record_Status.Find(item.Endpoint_Record_Status_Id).Description;
                obj.FirstNameE = item.First_Name;
                obj.LastNameE = item.Last_Name;
                obj.IDNumberE = item.Identification_Number;
                obj.PONmae_To = db.Users.Find(item.Case_Allocated_To).Last_Name;
                obj.Allocated_ByName = db.Users.Find(item.Allocated_ByS).Last_Name;

                avm.Add(obj);
            }

            return avm;
        }

        public int GetAssessment(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                // Get assement 

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select i.Intake_Assessment_Id).FirstOrDefault();
            }
        }

        public void UpdatePCMWorkListCase(PCMSocialWorkerWorkListVM list, int AssId)
        {
            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    // Accept New case on work list 
                    var editlist = (from i in db.PCM_WorkList
                                    where i.Intake_Assessment_Id == (AssId)
                                    select i.PCMCaseWoklist_Id).FirstOrDefault();

                    PCM_WorkList edit = db.PCM_WorkList.Find(editlist);


                    edit.Intake_Assessment_Id = AssId;
                    edit.PCM_Record_Status_Id = 2;
                    edit.Accepted_By = list.Case_Manager_Id;
                    edit.Date_Accepted = DateTime.Now;

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

        public void UpdatePCMEndpointWorkListCase(PCMSocialWorkerEndpointCasesVM list, int End_Point_POD_Id,int userId)
        {
            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    // Accept New case on work list 
                    var editlist = (from i in db.PCM_EndPoint_PO_Inbox
                                    where i.End_Point_POD_Id == (End_Point_POD_Id)
                                    select i.End_Point_POD_Id).FirstOrDefault();

                    PCM_EndPoint_PO_Inbox edit = db.PCM_EndPoint_PO_Inbox.Find(editlist);


                    edit.End_Point_POD_Id = End_Point_POD_Id;
                    edit.Endpoint_Record_Status_Id = 4;
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
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }


            }
        }


        public List<Person> GetListOfPersonsPCM()
        {
            List<Person> listOfPersons;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var persons = (from ass in db.Intake_Assessments
                               join client in db.Clients on ass.Client_Id equals client.Client_Id
                               join p in db.Persons on client.Person_Id equals p.Person_Id
                               join subcat in db.Problem_Sub_Categories on ass.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id
                               join worklist in db.PCM_WorkList on ass.Intake_Assessment_Id equals worklist.Intake_Assessment_Id
                               where (subcat.Problem_Sub_Category_Id == 22 && worklist.PCM_Record_Status_Id != 1)
                               select p).ToList().Distinct();

             

                listOfPersons = (from p in persons
                                 select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfPersons;
        }

        public List<PCMCaseGridMain> GetPCMWorkList()
        {
            // initializing view model
            List<PCMCaseGridMain> caseViewModel = new List<PCMCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                     join worklist in db.PCM_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id//work list

                                     where (subcat.Problem_Sub_Category_Id == 22 && worklist.PCM_Record_Status_Id !=1)

                                     group p by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, person.Person_Id}
                                     into g
                                     select new
                                     {
                                         g.Key.Person_Id,
                                         g.Key.Client_Id,
                                         g.Key.First_Name,
                                         g.Key.Last_Name,
                                         g.Key.Identification_Number,
                                         Assessments = g.ToList()

                                     }).ToList();
            ;
            foreach (var item in clientAssessments)
            {
                PCMCaseGridMain obj = new PCMCaseGridMain();

                obj.Person_Id = item.Person_Id;
                obj.ClientId = item.Client_Id;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                //obj.PCMallassessment = db.Intake_Assessments.Where(o => o.Client_Id == item.Client_Id).Take(5).ToList();
                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }

        public PCMSocialWorkerWorkListVM CheckPCMAssementExistance()
        {
            // initializing view model
            PCMSocialWorkerWorkListVM newCase = new PCMSocialWorkerWorkListVM();
            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    //Check assessment from intake assessment not in Adoption work list
                    var List = (from List1 in db.Intake_Assessments
                                where !(from List2 in db.PCM_WorkList
                                        select List2.Intake_Assessment_Id).Contains(List1.Intake_Assessment_Id)
                                where List1.Problem_Sub_Category_Id == 22
                                select List1).ToList();

                    //ADOPT_Case_WorkList act = db.ADOPT_Case_WorkList.Find(worlistid);
                    foreach (var item in List)
                    {
                        if (List != null)
                        {
                            //insert values not in Work list for List

                            PCM_WorkList act = new PCM_WorkList();
                            act.Intake_Assessment_Id = item.Intake_Assessment_Id;
                            act.Allocated_By = item.Assessed_By_Id;
                            act.PCM_Record_Status_Id = 1;
                            act.Manager = item.Case_Manager_Id;
                            act.Date_Allocated = item.Date_Allocated;
                            db.PCM_WorkList.Add(act);
                            db.SaveChanges();

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

                return newCase;

            }

        }




        public List<PCMCaseGridNested> PCMassessmentByclient()
        {
            // initialising view model 
            List<PCMCaseGridNested> avm = new List<PCMCaseGridNested>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var workList =
                (from Assessment in db.Intake_Assessments
                 join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                 join person in db.Persons on client.Person_Id equals person.Person_Id
                 join pcmWl in db.PCM_WorkList on Assessment.Intake_Assessment_Id equals pcmWl.Intake_Assessment_Id

                 join subcat in db.Problem_Sub_Categories on Assessment.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                 where (client.Client_Id == Assessment.Client_Id && subcat.Problem_Sub_Category_Id == 22 && pcmWl.PCM_Record_Status_Id != 1)

                 
                 select new
                 {
                     Assessment.Intake_Assessment_Id,
                     Assessment.Assessment_Date,
                     Assessment.Assessed_By_Id,
                     pcmWl.PCM_Record_Status_Id

                 }).ToList();
            
            foreach (var item in workList)
            {

                // initialising view model
                PCMCaseGridNested obj = new PCMCaseGridNested();

                obj.AssessmentId = item.Intake_Assessment_Id;
                obj.AssessmentDate = item.Assessment_Date;
                obj.POName = db.Users.Find(item.Assessed_By_Id).Last_Name;
                obj.RecordStatusDescription = db.apl_PCM_Record_Status.Find(item.PCM_Record_Status_Id).Description;

                avm.Add(obj);
            }

            return avm;
        }

    }
}
