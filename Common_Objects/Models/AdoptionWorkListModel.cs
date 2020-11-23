using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class AdoptionWorkListModel
    {

        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

        //public List<AdoptionWorkListVM> GetUserWorkList(int useridlogged)
        //{
        //    List<AdoptionWorkListVM> avm = new List<AdoptionWorkListVM>();
        //    SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        //    var workList =
        //        (from list in db.ADOPT_Case_WorkList
        //         join status in db.apl_Adoption_Record_Status on list.Adopt_Record_Status_Id equals
        //         status.Adopt_Record_Status_Id
        //         join Assessment in db.Intake_Assessments on list.Intake_Assessment_Id equals Assessment.Intake_Assessment_Id
        //         join client in db.Clients on Assessment.Client_Id equals client.Client_Id
        //         join person in db.Persons on client.Person_Id equals person.Person_Id
        //         where (list.Allocate_To == useridlogged)
        //         select new
        //         {
        //             list.Intake_Assessment_Id,
        //             list.Adopt_CaseWoklist_Id,
        //             list.Allocated_By,
        //             list.Allocate_To,
        //             list.Date_Allocated,
        //             list.Reference_Number,
        //             list.Adopt_Record_Status_Id,
        //             list.Date_Acknowledged,
        //             person.First_Name,
        //             person.Last_Name,
        //             person.Identification_Number

        //         }).ToList();
        //    ;
        //    foreach (var item in workList)
        //    {
        //        AdoptionWorkListVM obj = new AdoptionWorkListVM();
                
        //        obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
        //        obj.Adopt_CaseWoklist_Id = item.Adopt_CaseWoklist_Id;
        //        obj.Allocated_By = item.Allocated_By;
        //        obj.Allocate_To = item.Allocate_To;
        //        obj.Allocated_By = item.Allocated_By;
        //        obj.Date_Allocated = item.Date_Allocated;
        //        obj.Reference_Number = item.Reference_Number;
        //        obj.Date_Allocated = item.Date_Allocated;
        //        obj.RecordStatusDescription = db.apl_Adoption_Record_Status.Find(item.Adopt_Record_Status_Id).Description;
        //        obj.FirstName = item.First_Name;
        //        obj.LastName = item.Last_Name;
        //        obj.IDNumber = item.Identification_Number;

        //        avm.Add(obj);
        //    }

        //    return avm;
        //}
        

        public List<AdoptionWorkListVM> GetUserWorkList(int useridlogged)
        {
            // initialising view model 
            List<AdoptionWorkListVM> avm = new List<AdoptionWorkListVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var workList =
                (from list in db.ADOPT_Case_WorkList
                 join status in db.apl_Adoption_Record_Status on list.Adopt_Record_Status_Id equals
                 status.Adopt_Record_Status_Id
                 join Assessment in db.Intake_Assessments on list.Intake_Assessment_Id equals Assessment.Intake_Assessment_Id
                 join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                 join person in db.Persons on client.Person_Id equals person.Person_Id
                 join sub in db.Problem_Sub_Categories on Assessment.Problem_Sub_Category_Id equals sub.Problem_Sub_Category_Id
                 where (list.Allocate_To == useridlogged && list.Adopt_Record_Status_Id != 3) || (list.Manager == useridlogged && list.Adopt_Record_Status_Id != 3)
                 select new
                 {
                     list.Intake_Assessment_Id,
                     list.Adopt_CaseWoklist_Id,
                     list.Allocated_By,
                     list.Manager,
                     list.Allocate_To,
                     list.Date_Allocated,
                     list.Reference_Number,
                     list.Adopt_Record_Status_Id,
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
                AdoptionWorkListVM obj = new AdoptionWorkListVM();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Adopt_CaseWoklist_Id = item.Adopt_CaseWoklist_Id;
                obj.Allocated_By = item.Allocated_By;
                obj.Allocate_To = item.Allocate_To;
                obj.Date_Allocated = item.Date_Allocated;
                obj.Reference_Number = item.Reference_Number;
                obj.RecordStatusDescription = db.apl_Adoption_Record_Status.Find(item.Adopt_Record_Status_Id).Description;
                obj.User_Surname = db.Users.Find(item.Allocate_To).Last_Name;
                obj.Date_Accepted = item.Date_Accepted;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                obj.ProblemSubCategoryDescription = db.Problem_Sub_Categories.Find(item.Problem_Sub_Category_Id).Description;

                avm.Add(obj);
            }

            return avm;
        }


        public List<AdoptionWorkListVM> GetUserNewWorkList(int useridlogged)
        {
            // initialising view model 
            List<AdoptionWorkListVM> avm = new List<AdoptionWorkListVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            // get problem category for adoption


            // get work list for user logged in
            var newworkList =

                (from list in db.ADOPT_Case_WorkList
                 join status in db.apl_Adoption_Record_Status on list.Adopt_Record_Status_Id equals
                 status.Adopt_Record_Status_Id
                 join Assessment in db.Intake_Assessments on list.Intake_Assessment_Id equals Assessment.Intake_Assessment_Id
                 join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                 join person in db.Persons on client.Person_Id equals person.Person_Id
                 join sub in db.Problem_Sub_Categories on Assessment.Problem_Sub_Category_Id equals sub.Problem_Sub_Category_Id

                 where (list.Allocate_To == useridlogged && status.Adopt_Record_Status_Id == 1) || (list.Manager == useridlogged && status.Adopt_Record_Status_Id == 1)
                 select new
                 {
                     list.Intake_Assessment_Id,
                     list.Adopt_CaseWoklist_Id,
                     list.Allocated_By,
                     list.Allocate_To,
                     list.Manager,
                     list.Date_Allocated,
                     list.Reference_Number,
                     list.Adopt_Record_Status_Id,
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
                AdoptionWorkListVM obj = new AdoptionWorkListVM();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Adopt_CaseWoklist_Id = item.Adopt_CaseWoklist_Id;
                obj.Allocated_By = item.Allocated_By;
                obj.Allocate_To = item.Allocate_To;
                obj.User_Surname = db.Users.Find(item.Allocate_To).Last_Name;
                obj.Date_Allocated = item.Date_Allocated;
                obj.Reference_Number = item.Reference_Number;
                obj.RecordStatusDescription = db.apl_Adoption_Record_Status.Find(item.Adopt_Record_Status_Id).Description;
                obj.Date_Accepted = item.Date_Accepted;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                obj.ProblemSubCategoryDescription = db.Problem_Sub_Categories.Find(item.Problem_Sub_Category_Id).Description;
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

        public int? GetAssessmentManager(int UserId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                // Get assement 

                return (from p in db.Users
                        join i in db.ADOPT_Case_WorkList on p.User_Id equals i.Manager
                        where i.Manager == UserId
                        select i.Manager).FirstOrDefault();
            }
        }


        public void UpdateWorkListCase(AdoptionWorkListVM list, int AssId)
        {
            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    // Accept New case on work list 
                    var editlist = (from i in db.ADOPT_Case_WorkList
                                    where i.Intake_Assessment_Id == (AssId)
                                    select i.Adopt_CaseWoklist_Id).FirstOrDefault();

                    ADOPT_Case_WorkList edit = db.ADOPT_Case_WorkList.Find(editlist);


                    edit.Intake_Assessment_Id = AssId;
                    edit.Adopt_Record_Status_Id = 2;
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



        public List<AdoptCaseGridMain> GetAdoptionWorkList()
        {
            // initializing view model
            List<AdoptCaseGridMain> caseViewModel = new List<AdoptCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                     join worklist in db.ADOPT_Case_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id

                                     where (subcat.Problem_Sub_Category_Id == 18 && worklist.Adopt_Record_Status_Id == 3)

                                     group p by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, p.Intake_Assessment_Id, worklist.Adopt_Record_Status_Id }
                                     into g
                                     select new
                                     {
                                         g.Key.Intake_Assessment_Id,
                                         g.Key.Client_Id,
                                         g.Key.First_Name,
                                         g.Key.Last_Name,
                                         g.Key.Identification_Number,
                                         g.Key.Adopt_Record_Status_Id,
                                         Assessments = g.ToList()

                                     }).ToList();
            ;
            foreach (var item in clientAssessments)
            {
                AdoptCaseGridMain obj = new AdoptCaseGridMain();

                obj.IntakeAssessmentId = item.Intake_Assessment_Id;
                obj.ClientId = item.Client_Id;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                obj.RecordStatusDescription = db.apl_Adoption_Record_Status.Find(item.Adopt_Record_Status_Id).Description;
                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }

        public AdoptionWorkListVM CheckAssementExistance()
        {
            // initializing view model

            AdoptionWorkListVM newCase = new AdoptionWorkListVM();

            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    //Check assessment from intake assessment not in Adoption work list
                    var List = (from List1 in db.Intake_Assessments
                                where !(from List2 in db.ADOPT_Case_WorkList
                                        select List2.Intake_Assessment_Id).Contains(List1.Intake_Assessment_Id)
                                where List1.Problem_Sub_Category_Id == 18 && (List1.Case_Manager_Id >= 1)
                                select List1).ToList();

                    //ADOPT_Case_WorkList act = db.ADOPT_Case_WorkList.Find(worlistid);
                    foreach (var item in List)
                    {
                        if (List != null)
                        {
                            //insert values not in Work list for List

                            ADOPT_Case_WorkList act = new ADOPT_Case_WorkList();
                            act.Intake_Assessment_Id = item.Intake_Assessment_Id;
                            act.Allocated_By = item.Assessed_By_Id;
                            act.Adopt_Record_Status_Id = 1;
                            act.Manager = item.Case_Manager_Supervisor_Id;
                            act.Allocate_To = item.Case_Manager_Id;
                            act.Date_Allocated = item.Date_Allocated;
                            db.ADOPT_Case_WorkList.Add(act);
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
    }
}
