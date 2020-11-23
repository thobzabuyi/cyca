using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class RACAPWorkListModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();     
        public List<RACAPWorkListVM> GetUserWorkList(int? useridlogged, string Search_First_Name, string Search_Last_Name, string Search_ID_Number, DateTime? Search_Date_Of_Birth, string Search_Intake_Ref_No, string Search_PCM_Ref_No)
        {
            // initialising view model 
            List<RACAPWorkListVM> avm = new List<RACAPWorkListVM>();

            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            int Search_Intake_Ref_No_1 = -1;
            if (Search_Intake_Ref_No != null && Search_Intake_Ref_No != "")
            {
                Search_Intake_Ref_No_1 = Convert.ToInt32(Search_Intake_Ref_No);
            }
            if (Search_Date_Of_Birth != null)
            {
               DateTime ttt = Convert.ToDateTime(Convert.ToString(Search_Date_Of_Birth).Replace("12:00:00 AM", "").Replace("{","").Replace("}",""));
            }
            if ((Search_Intake_Ref_No != null && Search_Intake_Ref_No != "")||(Search_Date_Of_Birth!=null))
            {
                var workList =
                    (
                    from Assessment in db.Intake_Assessments
                    join list_1 in db.RACAP_Case_Details on Assessment.Intake_Assessment_Id equals list_1.Intake_Assessment_Id into RCDs
                    from list_1 in RCDs.DefaultIfEmpty()
                    join list in db.RACAP_Case_WorkList on Assessment.Intake_Assessment_Id equals list.Intake_Assessment_Id into RCWs
                    from list in RCWs.DefaultIfEmpty()
                    join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                    join person in db.Persons on client.Person_Id equals person.Person_Id
                    where (
                                (list.Allocate_To == useridlogged || list.Allocate_To_2 == useridlogged)
                                &&
                                (Assessment.Problem_Sub_Category_Id == 19 || Assessment.Problem_Sub_Category_Id == 20)
                                &&
                                (list_1.RACAP_Record_Status_Id == 2)
                                &&
                                (person.First_Name.Contains(Search_First_Name) || Search_First_Name == null || Search_First_Name == "")
                                &&
                                (person.Last_Name.Contains(Search_Last_Name) || Search_Last_Name == null || Search_Last_Name == "")
                                &&
                                (person.Identification_Number.Contains(Search_ID_Number) || Search_ID_Number == null || Search_ID_Number == "")
                                &&
                                ( Assessment.Intake_Assessment_Id== Search_Intake_Ref_No_1 || Search_Intake_Ref_No == null || Search_Intake_Ref_No == "")
                                &&
                                (client.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")
                                && (list.Date_Allocated== Search_Date_Of_Birth|| Search_Date_Of_Birth==null )
                                && (list.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")

                            )


                    select new
                     {
                         list.Intake_Assessment_Id,
                         list.RACAP_CaseWoklist_Id,
                         list.Allocated_By,
                         list.Allocate_To,
                         list.Date_Allocated,
                         list.Allocate_To_2,
                         list.Reference_Number,
                         list.RACAP_Record_Status_Id,
                         list.Date_Accepted,
                         person.First_Name,
                         person.Last_Name,
                         person.Identification_Number,
                         Assessment.Problem_Sub_Category_Id,
                         person.Date_Of_Birth
                     }).Distinct().ToList();





                foreach (var item in workList)
                {

                    // initialising view model
                    RACAPWorkListVM obj = new RACAPWorkListVM();

                    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                    obj.RACAP_CaseWoklist_Id = item.RACAP_CaseWoklist_Id;
                    obj.Allocated_By = item.Allocated_By;
                    obj.Allocate_To = item.Allocate_To;
                    obj.Allocate_To_2 = item.Allocate_To_2;
                    obj.Date_Allocated = item.Date_Allocated;
                    obj.Reference_Number = item.Reference_Number;
                    obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    obj.Date_Accepted = item.Date_Accepted;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.IDNumber = item.Identification_Number;
                    obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;
                    obj.DateOfBirth = item.Date_Of_Birth;

                    avm.Add(obj);
                }

                return avm;
            }
            else
            {
                var workList =
                    (
                    from Assessment in db.Intake_Assessments
                    join list_1 in db.RACAP_Case_Details on Assessment.Intake_Assessment_Id equals list_1.Intake_Assessment_Id into RCDs
                    from list_1 in RCDs.DefaultIfEmpty()
                    join list in db.RACAP_Case_WorkList on Assessment.Intake_Assessment_Id equals list.Intake_Assessment_Id into RCWs
                    from list in RCWs.DefaultIfEmpty()
                    join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                    join person in db.Persons on client.Person_Id equals person.Person_Id
                    where (

                               (list.Allocate_To == useridlogged || list.Allocate_To_2 == useridlogged)
                                &&
                                (Assessment.Problem_Sub_Category_Id == 19 || Assessment.Problem_Sub_Category_Id == 20)
                                &&
                                ( list_1.RACAP_Record_Status_Id == 2)


                            )
                     where (person.First_Name.Contains(Search_First_Name) || Search_First_Name == null || Search_First_Name == "")
                     where (person.Last_Name.Contains(Search_Last_Name) || Search_Last_Name == null || Search_Last_Name == "")
                     where (person.Identification_Number.Contains(Search_ID_Number) || Search_ID_Number == null || Search_ID_Number == "")
                     where (list.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")

                     select new
                     {
                         Assessment.Intake_Assessment_Id,
                         list.RACAP_CaseWoklist_Id,
                         list.Allocated_By,
                         list.Allocate_To,
                         list.Allocate_To_2,
                         list.Date_Allocated,
                         list.Reference_Number,
                         list.RACAP_Record_Status_Id,
                         list.Date_Accepted,
                         person.First_Name,
                         person.Last_Name,
                         person.Identification_Number,
                         person.Identification_Type_Id,
                         Assessment.Problem_Sub_Category_Id,
                         person.Date_Of_Birth
                     }).ToList();



                foreach (var item in workList)
                {

                    // initialising view model
                    RACAPWorkListVM obj = new RACAPWorkListVM();

                    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                    obj.RACAP_CaseWoklist_Id = item.RACAP_CaseWoklist_Id;
                    obj.Allocated_By = item.Allocated_By;
                    obj.Allocate_To = item.Allocate_To;
                    obj.Allocate_To_2 = item.Allocate_To_2;
                    obj.Date_Allocated = item.Date_Allocated;
                    obj.Reference_Number = item.Reference_Number;
                    obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    obj.Date_Accepted = item.Date_Accepted;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.IDNumber = item.Identification_Number;
                    obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;
                    obj.DateOfBirth = item.Date_Of_Birth;
                    int TodaysYear = DateTime.Now.Year;
                    if (item.Problem_Sub_Category_Id == 20)
                    {
                        avm.Add(obj);
                    }
                    if (item.Problem_Sub_Category_Id == 19)
                    {
                        if (item.Date_Of_Birth != null)
                        {
                            int IdYear = Convert.ToInt32(Convert.ToString(item.Date_Of_Birth.Value.Year).Substring(2, 2));
                            int TYearLastTwoChar = Convert.ToInt32(Convert.ToString(TodaysYear).Substring(2, 2));
                            int Result = TYearLastTwoChar - IdYear;
                            if (Result <= 18 && Result > 0)
                            {
                                avm.Add(obj);
                            }

                        }
                        else
                        {
                            avm.Add(obj);
                        }
                    }



                }

                return avm;

            }
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

        public void UpdateWorkListCase(RACAPWorkListVM list, int AssId)
        {
            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    // Accept New case on work list 
                    var editlist = (from i in db.RACAP_Case_WorkList
                                    where i.Intake_Assessment_Id == (AssId)
                                    select i.RACAP_CaseWoklist_Id).FirstOrDefault();

                    RACAP_Case_WorkList edit = db.RACAP_Case_WorkList.Find(editlist);


                    edit.Intake_Assessment_Id = AssId;
                    edit.RACAP_Record_Status_Id = 2;
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
        public List<RACAPWorkListVM> GetRACAPWorkList(int? useridlogged, string Search_First_Name, string Search_Last_Name, string Search_ID_Number, DateTime? Search_Date_Of_Birth, string Search_Intake_Ref_No, string Search_PCM_Ref_No)
        {
            // initializing view model
            List<RACAPWorkListVM> caseViewModel = new List<RACAPWorkListVM>();

            // get Adoption cases that have been accepted on adoption module
            int Search_Intake_Ref_No_1 = -1;
            if (Search_Intake_Ref_No != null && Search_Intake_Ref_No != "")
            {
                string NewInRefNum = Search_Intake_Ref_No.Replace("t/", "");

                Search_Intake_Ref_No_1 = Convert.ToInt32(Search_Intake_Ref_No);
            }
            if (Search_Date_Of_Birth != null)
            {
                DateTime ttt = Convert.ToDateTime(Convert.ToString(Search_Date_Of_Birth).Replace("12:00:00 AM", "").Replace("{", "").Replace("}", ""));
            }
            if ((Search_Intake_Ref_No != null && Search_Intake_Ref_No != "")|| (Search_Date_Of_Birth!=null))
            {

                var clientAssessments = (from Assessment in db.Intake_Assessments
                                         join list_1 in db.RACAP_Case_Details on Assessment.Intake_Assessment_Id equals list_1.Intake_Assessment_Id into RCDs
                                         from list_1 in RCDs.DefaultIfEmpty()
                                         join list in db.RACAP_Case_WorkList on Assessment.Intake_Assessment_Id equals list.Intake_Assessment_Id into RCWs
                                         from list in RCWs.DefaultIfEmpty()
                                         join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                                         join person in db.Persons on client.Person_Id equals person.Person_Id


                                         where (
                                                    (list.Allocate_To == useridlogged || list.Allocate_To_2 == useridlogged)
                                                    &&
                                                    (Assessment.Problem_Sub_Category_Id == 19 || Assessment.Problem_Sub_Category_Id == 20)
                                                    && 
                                                    (list.RACAP_Record_Status_Id == 3)
                                                    &&
                                                    (person.First_Name.Contains(Search_First_Name) || Search_First_Name == null || Search_First_Name == "")
                                                    &&
                                                    (person.Last_Name.Contains(Search_Last_Name) || Search_Last_Name == null || Search_Last_Name == "")
                                                    &&
                                                    (person.Identification_Number.Contains(Search_ID_Number) || Search_ID_Number == null || Search_ID_Number == "")
                                                    &&
                                                    (Assessment.Intake_Assessment_Id == Search_Intake_Ref_No_1 || Search_Intake_Ref_No == null || Search_Intake_Ref_No == "")
                                                    &&
                                                    (client.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")
                                                    && (list.Date_Allocated == Search_Date_Of_Birth || Search_Date_Of_Birth == null)
                                                    && (list.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")

                                                )


                                         group Assessment by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, Assessment.Intake_Assessment_Id, Assessment.Date_Allocated, list.RACAP_Record_Status_Id, list.Allocate_To, list.Allocate_To_2, Assessment.Problem_Sub_Category_Id,person.Date_Of_Birth }
                                     into g
                                         select new
                                         {
                                             g.Key.Intake_Assessment_Id,
                                             g.Key.Client_Id,
                                             g.Key.First_Name,
                                             g.Key.Last_Name,
                                             g.Key.Identification_Number,
                                             g.Key.RACAP_Record_Status_Id,
                                             g.Key.Problem_Sub_Category_Id,
                                             g.Key.Date_Allocated,
                                             g.Key.Allocate_To,
                                             g.Key.Allocate_To_2,
                                             g.Key.Date_Of_Birth,
                                             Assessments = g.ToList()

                                         }).ToList();
                


                foreach (var item in clientAssessments)
                {
                    RACAPWorkListVM obj = new RACAPWorkListVM();

                    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                    obj.ClientId = item.Client_Id;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.Allocate_To = item.Allocate_To;
                    obj.Allocate_To_2 = item.Allocate_To_2;
                    obj.IDNumber = item.Identification_Number;
                    obj.Date_Allocated = item.Date_Allocated;
                    obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;

                        caseViewModel.Add(obj);


                }
                return caseViewModel;
            }
            else
            {
                var clientAssessments = (from p in db.Intake_Assessments
                                         join client in db.Clients on p.Client_Id equals client.Client_Id
                                         join person in db.Persons on client.Person_Id equals person.Person_Id
                                         //join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                         join worklist in db.RACAP_Case_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id

                                         where (
                                                    (worklist.Allocate_To == useridlogged || worklist.Allocate_To_2==useridlogged)
                                                    &&
                                                    (p.Problem_Sub_Category_Id == 19 || p.Problem_Sub_Category_Id == 20)
                                                    && 
                                                    (worklist.RACAP_Record_Status_Id == 3)
                                                    &&
                                                    (person.First_Name.Contains(Search_First_Name) || Search_First_Name == null || Search_First_Name == "")
                                                    &&
                                                    (person.Last_Name.Contains(Search_Last_Name) || Search_Last_Name == null || Search_Last_Name == "")
                                                    &&
                                                    (person.Identification_Number.Contains(Search_ID_Number) || Search_ID_Number == null || Search_ID_Number == "")
                    //&&
                    //(p.Intake_Assessment_Id == (Search_Intake_Ref_No_1))
                                                    &&
                                                    (worklist.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")
                                                )
                                         

                                         group p by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, p.Intake_Assessment_Id, p.Date_Allocated, worklist.RACAP_Record_Status_Id, worklist.Allocate_To, worklist.Allocate_To_2,p.Problem_Sub_Category_Id, person.Date_Of_Birth }
                                     into g
                                         select new
                                         {
                                             g.Key.Intake_Assessment_Id,
                                             g.Key.Client_Id,
                                             g.Key.First_Name,
                                             g.Key.Last_Name,
                                             g.Key.Identification_Number,
                                             g.Key.RACAP_Record_Status_Id,
                                             g.Key.Problem_Sub_Category_Id,
                                             g.Key.Date_Allocated,
                                             g.Key.Allocate_To,
                                             g.Key.Allocate_To_2,
                                             g.Key.Date_Of_Birth,
                                             Assessments = g.ToList()

                                         }).ToList();
               


                foreach (var item in clientAssessments)
                {
                    RACAPWorkListVM obj = new RACAPWorkListVM();

                    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                    obj.ClientId = item.Client_Id;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.IDNumber = item.Identification_Number;
                    obj.Date_Allocated = item.Date_Allocated;
                    obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;

                    if ( item.Problem_Sub_Category_Id == 19)
                    {
                        caseViewModel.Add(obj);
                    }
                    if (item.Problem_Sub_Category_Id == 20)
                    {
                        caseViewModel.Add(obj);

                    }
                }
                return caseViewModel;
            }
        }

        public List<RACAPWorkListVM> GetUserNewWorkList(int? useridlogged, string Search_First_Name, string Search_Last_Name, string Search_ID_Number, DateTime? Search_Date_Of_Birth, string Search_Intake_Ref_No, string Search_PCM_Ref_No)
        {

            // initialising view model 
            List<RACAPWorkListVM> rvm = new List<RACAPWorkListVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get problem category for RACAP
            //DateTime validYears = DateTime.Now.AddYears(-18);
            int Search_Intake_Ref_No_1 = -1;
            if (Search_Intake_Ref_No != null && Search_Intake_Ref_No != "")
            {
                Search_Intake_Ref_No_1 = Convert.ToInt32(Search_Intake_Ref_No);
            }
            // get work list for user logged in
            if ((Search_Intake_Ref_No != null && Search_Intake_Ref_No != "") ||(Search_Date_Of_Birth!=null) )
            {
                #region SearchHaveInTakeId

                var newworkList =
                (
                from Assessment in db.Intake_Assessments
                join list_1 in db.RACAP_Case_Details on Assessment.Intake_Assessment_Id equals list_1.Intake_Assessment_Id into RCDs
                from list_1 in RCDs.DefaultIfEmpty()
                join list in db.RACAP_Case_WorkList on Assessment.Intake_Assessment_Id equals list.Intake_Assessment_Id into RCWs
                from list in RCWs.DefaultIfEmpty()
                join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                join person in db.Persons on client.Person_Id equals person.Person_Id


                where (
                            (list.Allocate_To == useridlogged || list.Allocate_To_2 == useridlogged)
                            &&
                            (Assessment.Problem_Sub_Category_Id == 19 || Assessment.Problem_Sub_Category_Id == 20)

                            &&
                            (list.RACAP_Record_Status_Id == 1))
                //     &&
                where (person.First_Name.Contains(Search_First_Name) || Search_First_Name == null || Search_First_Name == "")
                //     &&
                where (person.Last_Name.Contains(Search_Last_Name) || Search_Last_Name == null || Search_Last_Name == "")
                //     &&
                where (person.Identification_Number.Contains(Search_ID_Number) || Search_ID_Number == null || Search_ID_Number == "")
                //                                     &&
                where (Assessment.Intake_Assessment_Id == Search_Intake_Ref_No_1 || Search_Intake_Ref_No == null || Search_Intake_Ref_No == "")
                //     &&
                where (client.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")
                where (Assessment.Assessment_Date.Year == Search_Date_Of_Birth.Value.Year
                            && Assessment.Assessment_Date.Month == Search_Date_Of_Birth.Value.Month
                            && Assessment.Assessment_Date.Day == Search_Date_Of_Birth.Value.Day || Search_Date_Of_Birth == null)

                where (list.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")

                       //)
                select new
                 {
                     list.Intake_Assessment_Id,
                     list.RACAP_CaseWoklist_Id,
                     list.Allocated_By,
                     list.Allocate_To,
                     list.Allocate_To_2,
                     Assessment.Assessment_Date,
                     list.Reference_Number,
                     list.RACAP_Record_Status_Id,
                     list.Date_Accepted,
                     person.First_Name,
                     person.Last_Name,
                     person.Age,
                     person.Gender,
                     person.Identification_Number,
                     Assessment.Problem_Sub_Category_Id,
                     person.Date_Of_Birth


                 }).OrderBy(x => x.Last_Name).Distinct().ToList();

                if (Search_Date_Of_Birth != null)
                {
                    newworkList = (from a in newworkList
                                   where (a.Assessment_Date.Year == Search_Date_Of_Birth.Value.Year
                            && a.Assessment_Date.Month == Search_Date_Of_Birth.Value.Month
                            && a.Assessment_Date.Day == Search_Date_Of_Birth.Value.Day)
                                   select a).ToList();
                }

                foreach (var item in newworkList)
                {

                    // initialising view model
                    RACAPWorkListVM obj = new RACAPWorkListVM();

                    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                    obj.RACAP_CaseWoklist_Id = item.RACAP_CaseWoklist_Id;
                    obj.Allocated_By = item.Allocated_By;
                    obj.Allocate_To = item.Allocate_To;
                    obj.Allocate_To_2 = item.Allocate_To_2;
                    obj.Date_Allocated = item.Assessment_Date;
                    obj.Reference_Number = item.Reference_Number;
                    obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    obj.Date_Accepted = item.Date_Accepted;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.Age = item.Age;
                    if (item.Gender != null)
                    {
                        obj.Gender = item.Gender.Description;
                    }
                    obj.IDNumber = item.Identification_Number;
                    obj.ProblemSubCategoryDescription = db.Problem_Sub_Categories.Find(item.Problem_Sub_Category_Id).Description;
                    obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;
                    rvm.Add(obj);
                } 
                return rvm;
                #endregion
            }
            else
            {
                
            var CheckIfExistsInRACAPCaseDetaiils = (from x in db.RACAP_Case_Details
                                                        select x.Intake_Assessment_Id).ToList();
                //*************************

                var newworkList =
                (from Assessment in db.Intake_Assessments
                 join list_1 in db.RACAP_Case_Details on Assessment.Intake_Assessment_Id equals list_1.Intake_Assessment_Id into RCDs
                 from list_1 in RCDs.DefaultIfEmpty()
                 join list in db.RACAP_Case_WorkList on Assessment.Intake_Assessment_Id equals list.Intake_Assessment_Id into RCWs
                 from list in RCWs.DefaultIfEmpty()
                 join client in db.Clients on Assessment.Client_Id equals client.Client_Id
                 join person in db.Persons on client.Person_Id equals person.Person_Id

                 where ((Assessment.Problem_Sub_Category_Id == 19 || Assessment.Problem_Sub_Category_Id == 20)
                 &&
                            (list.Allocate_To == useridlogged || list.Allocate_To_2 == useridlogged)
                            &&
                            (list.RACAP_Record_Status_Id == 1))
                 // &&
                 where (person.First_Name.Contains(Search_First_Name) || Search_First_Name == null || Search_First_Name == "")
                 //&&
                 where (person.Last_Name.Contains(Search_Last_Name) || Search_Last_Name == null || Search_Last_Name == "")
                 // &&
                 where (person.Identification_Number.Contains(Search_ID_Number) || Search_ID_Number == null || Search_ID_Number == "")
                 //&&
                 where(Assessment.Intake_Assessment_Id==(Search_Intake_Ref_No_1) || Search_Intake_Ref_No == null)
                 where (list.Reference_Number.Contains(Search_PCM_Ref_No) || Search_PCM_Ref_No == null || Search_PCM_Ref_No == "")
                 select new
                 {
                     Assessment.Intake_Assessment_Id,
                     //list.RACAP_CaseWoklist_Id,
                     Assessment.Assessed_By_Id,
                     Assessment.Case_Manager_Id,
                     //Assessment.Allocate_To_2,
                     Assessment.Assessment_Date,
                     Assessment.Date_Allocated,
                     //list.Reference_Number,
                     //list.RACAP_Record_Status_Id,
                     //list.Date_Accepted,
                     person.First_Name,
                     person.Last_Name,
                     person.Age,
                     person.Gender,
                     person.Identification_Number,
                     person.Identification_Type_Id,
                     Assessment.Problem_Sub_Category_Id,
                     person.Date_Of_Birth


                 }).OrderBy(x=>x.Last_Name).ToList();

                
                foreach (var item in newworkList)
                {
                      // initialising view model
                    RACAPWorkListVM obj = new RACAPWorkListVM();

                    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                    //obj.RACAP_CaseWoklist_Id = item.RACAP_CaseWoklist_Id;
                    obj.Allocated_By = item.Assessed_By_Id;
                    obj.Allocate_To = item.Case_Manager_Id;
                    //obj.Allocate_To_2 = item.Allocate_To_2;
                    obj.Date_Allocated = item.Assessment_Date;
                    //obj.Reference_Number = item.Reference_Number;
                    //obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    //obj.Date_Accepted = item.Date_Accepted;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.Age = item.Age;
                    if (item.Gender != null)
                    {
                        obj.Gender = item.Gender.Description;
                    }
                    obj.IDNumber = item.Identification_Number;
                    obj.ProblemSubCategoryDescription = db.Problem_Sub_Categories.Find(item.Problem_Sub_Category_Id).Description;
                    obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;
                    int TodaysYear = DateTime.Now.Year;
                    if (item.Problem_Sub_Category_Id == 20)
                    {
                        rvm.Add(obj);
                    }
                    if(item.Problem_Sub_Category_Id==19)
                    { 
                        if(item.Date_Of_Birth!=null)
                        {
                            int IdYear = Convert.ToInt32(Convert.ToString(item.Date_Of_Birth.Value.Year).Substring(2, 2));
                            int TYearLastTwoChar = Convert.ToInt32(Convert.ToString(TodaysYear).Substring(2, 2));
                            int Result = TYearLastTwoChar - IdYear;
                            if (Result <= 18 && Result >= 0)
                            {
                                rvm.Add(obj);
                            }

                        }
                        else
                        {
                                rvm.Add(obj);
                        }
                    }

                }
                //

                return rvm;
            }
        }

        public List<RACAPCaseGridMain> GetRACAPWorkList1(int IntAssId)
        {
            // initializing view model
            List<RACAPCaseGridMain> caseViewModel = new List<RACAPCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     //join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                     join worklist in db.RACAP_Case_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id

                                     where ((p.Problem_Sub_Category_Id == 19 || p.Problem_Sub_Category_Id == 20)) && p.Intake_Assessment_Id== IntAssId

                                     group p by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, p.Intake_Assessment_Id, worklist.RACAP_Record_Status_Id, p.Problem_Sub_Category_Id }
                                     into g
                                     select new
                                     {
                                         g.Key.Intake_Assessment_Id,
                                         g.Key.Client_Id,
                                         g.Key.First_Name,
                                         g.Key.Last_Name,
                                         g.Key.Identification_Number,
                                         g.Key.RACAP_Record_Status_Id,
                                         g.Key.Problem_Sub_Category_Id,
                                         Assessments = g.ToList()

                                     }).ToList();
            ;
            foreach (var item in clientAssessments)
            {
                RACAPCaseGridMain obj = new RACAPCaseGridMain();

                obj.IntakeAssessmentId = item.Intake_Assessment_Id;
                obj.ClientId = item.Client_Id;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;

                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }

        public RACAPSecondParent GetRACAPWorkListSecondParent(int IntAssId)
        {
            
            #region Check in Which table
            int newI=0;

            int ProspectiveAdoptivePersonId =

                (from a in db.Intake_Assessments
                 join b in db.Clients on a.Client_Id equals b.Client_Id
                 join c in db.int_Client_ProspectiveAdoptiveParents on b.Client_Id equals c.Client_Id
                 join d in db.Persons on c.Person_Id equals d.Person_Id
                 where a.Intake_Assessment_Id == IntAssId
                 select d.Person_Id).FirstOrDefault();

            if (ProspectiveAdoptivePersonId != 0)

                newI = ProspectiveAdoptivePersonId;

            #endregion


            // initializing view model
            RACAPSecondParent caseViewModel = new RACAPSecondParent();

            // get Adoption cases that have been accepted on adoption module
            #region ProspectiveAdoptiveParentTable
            if (ProspectiveAdoptivePersonId != 0)
            {
                var clientAssessmentsSecondParent = (from Adp in db.int_Client_ProspectiveAdoptiveParents
                                                     join person in db.Persons on Adp.Person_Id equals person.Person_Id
                                                     join client in db.Clients on Adp.Client_Id equals client.Client_Id
                                                     join p in db.Intake_Assessments on client.Client_Id equals p.Client_Id
                                                     //join worklist in db.RACAP_Case_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id

                                                     where (p.Intake_Assessment_Id == IntAssId
                                         && person.Person_Id == newI)

                                                     group p by new
                                                     {
                                                         client.Client_Id,
                                                         person.First_Name,
                                                         person.Last_Name,
                                                         person.Identification_Number,
                                                         p.Intake_Assessment_Id
                                                     // , worklist.RACAP_Record_Status_Id
                                                     ,
                                                         p.Problem_Sub_Category_Id,
                                                         person.Known_As,
                                                         person.Identification_Type_Id,
                                                         person.Age,
                                                         person.Is_Estimated_Age,
                                                         person.Date_Of_Birth,
                                                         person.Language_Id,
                                                         person.Gender_Id,
                                                         person.Population_Group_Id,
                                                         person.Religion_Id,
                                                         person.Marital_Status,
                                                         person.Nationality_Id,
                                                         person.Email_Address,
                                                         person.Mobile_Phone_Number,
                                                        
                                                        
                              
                                                     }
                                         into g
                                                     select new
                                                     {
                                                         g.Key.Intake_Assessment_Id,
                                                         g.Key.Client_Id,
                                                         g.Key.First_Name,
                                                         g.Key.Last_Name,
                                                         g.Key.Identification_Number,
                                                         //g.Key.RACAP_Record_Status_Id,
                                                         g.Key.Problem_Sub_Category_Id,
                                                         g.Key.Known_As,
                                                         g.Key.Identification_Type_Id,
                                                         g.Key.Is_Estimated_Age,
                                                         g.Key.Age,
                                                         g.Key.Date_Of_Birth,
                                                         g.Key.Language_Id,
                                                         g.Key.Gender_Id,
                                                         g.Key.Population_Group_Id,
                                                         g.Key.Religion_Id,
                                                         g.Key.Marital_Status,
                                                         g.Key.Nationality_Id,
                                                         g.Key.Email_Address,
                                                         g.Key.Mobile_Phone_Number,
                                                         Assessments = g.FirstOrDefault()

                                                     }).FirstOrDefault();

                caseViewModel.IntakeAssessmentId = clientAssessmentsSecondParent.Intake_Assessment_Id;
                caseViewModel.ClientId = clientAssessmentsSecondParent.Client_Id;
                caseViewModel.FirstName = clientAssessmentsSecondParent.First_Name;
                caseViewModel.LastName = clientAssessmentsSecondParent.Last_Name;
                caseViewModel.IDNumber = clientAssessmentsSecondParent.Identification_Number;
                // caseViewModel.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(clientAssessmentsSecondParent.RACAP_Record_Status_Id).Description;
                caseViewModel.Problem_Sub_Category_Id = clientAssessmentsSecondParent.Problem_Sub_Category_Id;
                caseViewModel.KnownAs = clientAssessmentsSecondParent.Known_As;
                if (clientAssessmentsSecondParent.Identification_Type_Id != null) { 
                    caseViewModel.Identification_Type_Id_ParentTwo = db.Identification_Types.Find(clientAssessmentsSecondParent.Identification_Type_Id).Description;
                }
                caseViewModel.Age = clientAssessmentsSecondParent.Age;
                caseViewModel.Is_Estimated_Age = clientAssessmentsSecondParent.Is_Estimated_Age;
                caseViewModel.Date_Of_Birth_ParentTwo = clientAssessmentsSecondParent.Date_Of_Birth;
                caseViewModel.Language_Id_ParentTwo = clientAssessmentsSecondParent.Language_Id;
                caseViewModel.Gender_Id = clientAssessmentsSecondParent.Gender_Id;
                caseViewModel.Email_Address2P = clientAssessmentsSecondParent.Email_Address;
                caseViewModel.Mobile_Phone_Number = clientAssessmentsSecondParent.Mobile_Phone_Number;

                var aDdresses = db.Persons.Find(ProspectiveAdoptivePersonId).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                var clientPostalAddress = (from a in aDdresses
                                           select a).LastOrDefault();





            }
            #endregion


            return caseViewModel;

        }

        public ResultsOfParentsInCPR GetRACAPCheckIfParentsExistsInCPR(int id)
        {
            ResultsOfParentsInCPR ResParent = new ResultsOfParentsInCPR();
            int P1 = (from z in db.Intake_Assessments
                      join d in db.Clients on z.Client_Id equals d.Client_Id
                      join a in db.Alleged_Offenders on d.Person_Id equals a.Person_Id
                      join p in db.Persons on a.Person_Id equals p.Person_Id
                      where z.Intake_Assessment_Id == id
                      select p.Person_Id).FirstOrDefault();

            int P2 = (from z in db.Intake_Assessments
                     join d in db.Clients on z.Client_Id equals d.Client_Id
                     join e in db.Client_Adoptive_Parents on d.Client_Id equals e.Client_Id
                     join a in db.Alleged_Offenders on e.Person_Id equals a.Person_Id
                      join p in db.Persons on a.Person_Id equals p.Person_Id
                      where z.Intake_Assessment_Id == id
                      select p.Person_Id).FirstOrDefault();

            if (P1 != 0)
            {
                ResParent.ParentOne= db.Persons.Find(P1).First_Name + " " 
                    + db.Persons.Find(P1).Last_Name + ", "
                    + db.Persons.Find(P1).Identification_Number + "Is an alleged offender in terms of the Child Protection Register: Please investigate!";
            }

            if (P1 == 0)
            {
                ResParent.ParentOne =  "Do not exist on Child Protection Register!";
            }
            if (P2 != 0)
            {
                ResParent.ParentTwo = db.Persons.Find(P2).First_Name + " "
                    + db.Persons.Find(P2).Last_Name + ", "
                    + db.Persons.Find(P2).Identification_Number + "Is an alleged offender in terms of the Child Protection Register: Please investigate!";
            }

            if (P2 == 0)
            {
                ResParent.ParentTwo =  "Do not exist on Child Protection Register!";
            }

            return ResParent;
        }
        //Amended by Herman
        public List<RACAPCaseGridMain> GetRACAPWorkListWithId(int id)
        {
            // initializing view model
            List<RACAPCaseGridMain> caseViewModel = new List<RACAPCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     //join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                     join worklist in db.RACAP_Case_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id

                                     where ((p.Problem_Sub_Category_Id == 19 || p.Problem_Sub_Category_Id == 20)
                                     && p.Intake_Assessment_Id == id

                                     )

                                     group p by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, p.Intake_Assessment_Id, worklist.RACAP_Record_Status_Id, p.Problem_Sub_Category_Id }
                                     into g
                                     select new
                                     {
                                         g.Key.Intake_Assessment_Id,
                                         g.Key.Client_Id,
                                         g.Key.First_Name,
                                         g.Key.Last_Name,
                                         g.Key.Identification_Number,
                                         g.Key.RACAP_Record_Status_Id,
                                         g.Key.Problem_Sub_Category_Id,
                                         Assessments = g.ToList()

                                     }).ToList();
            ;
            foreach (var item in clientAssessments)
            {
                RACAPCaseGridMain obj = new RACAPCaseGridMain();

                obj.IntakeAssessmentId = item.Intake_Assessment_Id;
                obj.ClientId = item.Client_Id;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                obj.RecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                obj.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;

                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }

        //End Amended by Herman
        public RACAPWorkListVM CheckAssementExistance()
        {
            // initializing view model

            RACAPWorkListVM newCase = new RACAPWorkListVM();

            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    //Check assessment from intake assessment not in Adoption work list
                    var List = (from List1 in db.Intake_Assessments
                                where !(from List2 in db.RACAP_Case_WorkList
                                        select List2.Intake_Assessment_Id).Contains(List1.Intake_Assessment_Id)
                                where List1.Problem_Sub_Category_Id == 19 || List1.Problem_Sub_Category_Id == 20
                                && List1.Case_Manager_Id>=1
                                select List1).ToList();

                    //ADOPT_Case_WorkList act = db.RACAP_Case_WorkList.Find(worlistid);
                    foreach (var item in List)
                    {
                        if (List != null)
                        {
                            //insert values not in Work list for List
                            int ClienTId = db.Intake_Assessments.Find(item.Intake_Assessment_Id).Client_Id;
                            RACAP_Case_WorkList act = new RACAP_Case_WorkList();
                            act.Intake_Assessment_Id = item.Intake_Assessment_Id;
                            act.Allocated_By = item.Assessed_By_Id;
                            act.RACAP_Record_Status_Id = 1;
                            act.Allocate_To = item.Case_Manager_Supervisor_Id;
                            act.Allocate_To_2 = item.Case_Manager_Id;
                            act.Date_Allocated = item.Date_Created;
                            //Amendment5_20180713                           
                            act.Reference_Number = "RACAP/" + Convert.ToString(ClienTId)+"/"+DateTime.Now.Year;
                            db.RACAP_Case_WorkList.Add(act);
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

        public User GetUser(string UserName)
        {

            return (from a in db.Users
                    where a.User_Name == UserName
                    select a).FirstOrDefault();

        }
        public List<Problem_Sub_Category> GetSubcategories()
        {
            return db.Problem_Sub_Categories.Where(x => x.Module_Id == 11).ToList();
        }

        public int? GetSupervisorId(int User_Id)
        {
            return (from d in db.Employees
                    where d.User_Id == User_Id
                    select d.Supervisor_Id).FirstOrDefault();
        }

        public int GetSupUserId(int supVersionId)
        {
            return db.Employees.Find(supVersionId).User_Id;
        }

        public List<Intake_Assessment> GetIntakeAssOfSupervisor(int SupId)
        {
            return db.Intake_Assessments.Where(x => x.Case_Manager_Supervisor_Id == SupId).ToList();
        }

        public List<Intake_Assessment> GetIntakeAssOfCaseMng(int userId)
        {
            return db.Intake_Assessments.Where(x => x.Case_Manager_Supervisor_Id == userId).ToList();
        }

        public int GetGenders()
        {
            return (from k in db.Genders
                    select k.Gender_Id).FirstOrDefault();
        }

        public SelectList GetGender_s(int GenderId)
        {
            return new SelectList(db.Genders, "Gender_Id", "Description", GenderId);
        }

        public int GetPopulation_Groups()
        {
            return (from k in db.Population_Groups
                    select k.Population_Group_Id).FirstOrDefault();
        }

        public SelectList GetPopulation_Group_s(int Population_GroupId)
        {
            return new SelectList(db.Population_Groups, "Population_Group_Id", "Description", Population_GroupId);
        }

        public int GetReligions()
        {
            return (from k in db.Religions
                    select k.Religion_Id).FirstOrDefault();
        }

        public SelectList GetReligion_s(int ReligionId)
        {
            return new SelectList(db.Religions, "Religion_Id", "Description", ReligionId);
        }

        public int GetNationalities()
        {
            return (from k in db.Nationalities
                    select k.Nationality_Id).FirstOrDefault();
        }

        public SelectList GetNationalitie_s(int NationalityId)
        {
            return new SelectList(db.Nationalities, "Nationality_Id", "Description", NationalityId);
        }

        public int GetProvinces()
        {
            return (from k in db.Provinces
                    select k.Province_Id).FirstOrDefault();
        }

        public SelectList GetProvince_s(int ProvId)
        {
            return new SelectList(db.Provinces, "Province_Id", "Description", ProvId);
        }

        public int GetDistricts()
        {
            return (from k in db.Districts
                    select k.District_Id).FirstOrDefault();
        }

        public SelectList GetDistrict_s(int DistrictId)
        {
            return new SelectList(db.Districts, "District_Id", "Description", DistrictId);
        }

        public int GetLocal_Municipalities()
        {
            return (from k in db.Local_Municipalities
                    select k.Local_Municipality_Id).FirstOrDefault();
        }

        public SelectList GetLocal_Municipalitie_s(int LocalMuniId)
        {
            return new SelectList(db.Local_Municipalities, "Local_Municipality_Id", "Description", LocalMuniId);
        }

        public int GetTowns()
        {
            return (from k in db.Towns
                    select k.Town_Id).FirstOrDefault();
        }

        public SelectList GetTowns_s(int TownId)
        {
            return new SelectList(db.Towns, "Town_Id", "Description", TownId);
        }

        public int GetMarital_Statusses()
        {
            return (from k in db.Marital_Statusses
                    select k.Marital_Status_Id).FirstOrDefault();
        }

        public SelectList GetMarital_Statusse_s(int MaritalTypeId)
        {
            return new SelectList(db.Marital_Statusses, "Marital_Status_Id", "Description", MaritalTypeId);
        }

        public int GetPersonId(int IntAssId)
        {
            return (from a in db.Intake_Assessments
                    join b in db.Clients on a.Client_Id equals b.Client_Id
                    join c in db.int_Client_ProspectiveAdoptiveParents on b.Client_Id equals c.Client_Id
                    join d in db.Persons on c.Person_Id equals d.Person_Id
                    where a.Intake_Assessment_Id == IntAssId
                    select d.Person_Id).FirstOrDefault();
        }
        public ICollection<Address> GetAddresses(int ProspectiveAdoptivePersonId)
        {
            return db.Persons.Find(ProspectiveAdoptivePersonId).Addresses;
        }

        public int GetProvinceId(int? TownId)
        {
            return (from k in db.Provinces
                    join l in db.Districts on k.Province_Id equals l.Province_Id
                    join m in db.Local_Municipalities on l.District_Id equals m.District_Municipality_Id
                    join n in db.Towns on m.Local_Municipality_Id equals n.Local_Municipality_Id
                    where n.Town_Id == TownId
                    select k.Province_Id).FirstOrDefault();
        }

        public int GetDistrictId(int? TownId)
        {
            return (from k in db.Provinces
                    join l in db.Districts on k.Province_Id equals l.Province_Id
                    join m in db.Local_Municipalities on l.District_Id equals m.District_Municipality_Id
                    join n in db.Towns on m.Local_Municipality_Id equals n.Local_Municipality_Id
                    where n.Town_Id == TownId
                    select l.District_Id).FirstOrDefault();
        }

        public int GetLocalMunicipalityId(int? TownId)
        {
            return (from k in db.Provinces
                    join l in db.Districts on k.Province_Id equals l.Province_Id
                    join m in db.Local_Municipalities on l.District_Id equals m.District_Municipality_Id
                    join n in db.Towns on m.Local_Municipality_Id equals n.Local_Municipality_Id
                    where n.Town_Id == TownId
                    select m.Local_Municipality_Id).FirstOrDefault();
        }
    }
}
