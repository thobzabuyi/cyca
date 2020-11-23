using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPInboxModel
    {

        public VEPInboxVM CheckAssementExistance()
        {
            // initializing view model

            VEPInboxVM newCase = new VEPInboxVM();

            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    //Check assessment from intake assessment not in Adoption work list
                    var List = (from List1 in db.Intake_Assessments
                                where !(from List2 in db.VEP_WorkList
                                        select List2.Intake_Assessment_Id).Contains(List1.Intake_Assessment_Id)
                                where List1.Problem_Sub_Category_Id == 19
                                select List1).ToList();

                    //ADOPT_Case_WorkList act = db.ADOPT_Case_WorkList.Find(worlistid);
                    foreach (var item in List)
                    {
                        if (List != null)
                        {
                            //insert values not in Work list for List

                            VEP_WorkList act = new VEP_WorkList();
                            act.Intake_Assessment_Id = item.Intake_Assessment_Id;
                            //act.Allocated_By =item.Assessed_By_Id;
                            //act.VEP_Record_Status_Id = 1;
                            //act.Allocated_To = item.Case_Manager_Id;
                            //act.Date_Allocated = item.Date_Allocated;
                            db.VEP_WorkList.Add(act);
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


        public List<VEPInboxVM> NewCasesList()
        {
            List<VEPInboxVM> kg = new List<VEPInboxVM>();

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var kList = (from p in db.Persons
                         join c in db.Clients
                         on p.Person_Id equals c.Person_Id
                         join i in db.Intake_Assessments
                         on c.Client_Id equals i.Client_Id
                         join g in db.Genders
                         on p.Gender_Id equals g.Gender_Id
                         //where (i.Problem_Sub_Category_Id == 19)
                         select new {
                             p.Person_Id,
                             p.First_Name,
                             p.Last_Name,
                             p.Identification_Number,
                             p.Gender,
                             p.Age,
                             i.Problem_Sub_Category_Id,
                             i.Assessment_Date,
                             i.Date_Allocated,
                             i.Created_By,
                             i.Intake_Assessment_Id,
                             c.Client_Id, g.Description }).ToList();

            foreach (var item in kList)
            {
                VEPInboxVM objK = new VEPInboxVM();
                objK.Person_Id = item.Person_Id;
                objK.First_Name = item.First_Name;
                objK.Last_Name = item.Last_Name;
                objK.Identification_Number = item.Identification_Number;
                objK.Description = item.Description;
                objK.Age = item.Age;
                objK.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;
                objK.Assessment_Date = item.Assessment_Date;
                objK.Date_Allocated = item.Date_Allocated;
                objK.Created_By = item.Created_By;
                objK.Client_Id = item.Client_Id;
                objK.Intake_Assessment_Id = item.Intake_Assessment_Id;
                kg.Add(objK);

            }
            return kg;
        }


        #region WorkList
        public List<VEPInboxVM> ExistingWorklist()
        {
            List<VEPInboxVM> kg = new List<VEPInboxVM>();

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var kList = (from i in db.Intake_Assessments
                             //join v in db.VEP_WorkList
                             //on i.Intake_Assessment_Id equals v.Intake_Assessment_Id
                         join c in db.Clients
                         on i.Client_Id equals c.Client_Id
                         join p in db.Persons
                         on c.Person_Id equals p.Person_Id
                         join g in db.Genders
                         on p.Gender_Id equals g.Gender_Id
                         //where (i.Problem_Sub_Category_Id == 19)
                         select new
                         {
                             p.Person_Id,
                             p.First_Name,
                             p.Last_Name,
                             p.Identification_Number,
                             p.Gender,
                             p.Age,
                             i.Problem_Sub_Category_Id,
                             i.Assessment_Date,
                             i.Date_Allocated,
                             i.Created_By,
                             c.Client_Id,
                             g.Description
                         }).ToList();

            foreach (var item in kList)
            {
                VEPInboxVM objK = new VEPInboxVM();
                objK.Person_Id = item.Person_Id;
                objK.First_Name = item.First_Name;
                objK.Last_Name = item.Last_Name;
                objK.Identification_Number = item.Identification_Number;
                objK.Description = item.Description;
                objK.Age = item.Age;
                objK.Problem_Sub_Category_Id = item.Problem_Sub_Category_Id;
                objK.Assessment_Date = item.Assessment_Date;
                objK.Date_Allocated = item.Date_Allocated;
                objK.Created_By = item.Created_By;

                //objK.Client_Id = item.Client_Id;
                kg.Add(objK);

            }
            return kg;
        }

        public List<VEPInboxVM> GetUserWorkList(int useridlogged)
        {
            // initialising view model 

            List<VEPInboxVM> avm = new List<VEPInboxVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            //var workList =
            //    (from list in db.VEP_WorkList
            //     join status in db.apl_VEP_Record_Status on list.VEP_Record_Status_Id equals
            //     status.VEP_Record_Status
            //     join Assessment in db.Intake_Assessments on list.Intake_Assessment_Id equals Assessment.Intake_Assessment_Id
            //     join client in db.Clients on Assessment.Client_Id equals client.Client_Id
            //     join person in db.Persons on client.Person_Id equals person.Person_Id
            //     where (list.Allocated_To == useridlogged)
            //     select new
            //     {
            //         list.Intake_Assessment_Id,
            //         list.Status_Id,
            //         list.Allocated_By,
            //         list.Allocated_To,
            //         list.Date_Allocated,
            //         list.Reference_Number,
            //         list.VEP_Record_Status_Id,
            //         list.Date_Accepted,
            //         person.First_Name,
            //         person.Last_Name,
            //         person.Identification_Number,
            //         Assessment.Assessment_Date,
            //         client.Client_Id

            //     }).ToList();
            //;
            //foreach (var item in workList)
            //{

            //    // initialising view model
            //    VEPInboxVM obj = new VEPInboxVM();

            //    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
            //    obj.Status_Id = item.Status_Id;
            //    obj.Allocated_By = item.Allocated_By;
            //    obj.Allocated_TO = item.Allocated_To;
            //    obj.Date_Allocated = item.Date_Allocated;
            //    obj.Reference_Number = item.Reference_Number;
            //    obj.VEpRecordStatusDescription = db.apl_VEP_Record_Status.Find(item.VEP_Record_Status_Id).Description;
            //    obj.Date_Accepted = item.Date_Accepted;
            //    obj.FirstName = item.First_Name;
            //    obj.LastName = item.Last_Name;
            //    obj.Assessment_Date = item.Assessment_Date;
            //    obj.IDNumber = item.Identification_Number;
            //    obj.Client_Id = item.Client_Id; 

            //    avm.Add(obj);
            //}

            return avm;
        }


        public List<VEPInboxVM> GetUserNewWorkList(int useridlogged)
        {
            // initialising view model 
            List<VEPInboxVM> avm = new List<VEPInboxVM>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            // get problem category for adoption


            // get work list for user logged in
            //var newworkList =

            //    (from list in db.VEP_WorkList
            //     join status in db.apl_VEP_Record_Status on list.VEP_Record_Status_Id equals
            //     status.VEP_Record_Status
            //     join Assessment in db.Intake_Assessments on list.Intake_Assessment_Id equals Assessment.Intake_Assessment_Id
            //     join client in db.Clients on Assessment.Client_Id equals client.Client_Id
            //     join person in db.Persons on client.Person_Id equals person.Person_Id
            //     join sub in db.Problem_Sub_Categories on Assessment.Problem_Sub_Category_Id equals sub.Problem_Sub_Category_Id

            //     where (list.Allocated_To == useridlogged && status.VEP_Record_Status == 1)
            //     select new
            //     {

            //         list.Intake_Assessment_Id,
            //         list.Status_Id,
            //         list.Allocated_By,
            //         list.Allocated_To,
            //         list.Date_Allocated,
            //         list.Reference_Number,
            //         list.VEP_Record_Status_Id,
            //         list.Date_Accepted,
            //         person.First_Name,
            //         person.Last_Name,
            //         person.Identification_Number,
            //         sub.Problem_Sub_Category_Id


            //     }).ToList();
            //;
            //foreach (var item in newworkList)
            //{

            //    // initialising view model
            //    VEPInboxVM obj = new VEPInboxVM();

            //    obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
            //    obj.Status_Id = item.Status_Id;
            //    obj.Allocated_By = item.Allocated_By;
            //    obj.Allocated_TO = item.Allocated_To;
            //    obj.Date_Allocated = item.Date_Allocated;
            //    obj.Reference_Number = item.Reference_Number;
            //    obj.VEpRecordStatusDescription = db.apl_VEP_Record_Status.Find(item.VEP_Record_Status_Id).Description;
            //    obj.Date_Accepted = item.Date_Accepted;
            //    obj.FirstName = item.First_Name;
            //    obj.LastName = item.Last_Name;
            //    obj.IDNumber = item.Identification_Number;
            //    obj.VEPProblemSubCategoryDescription = db.Problem_Sub_Categories.Find(item.Problem_Sub_Category_Id).Description;
            //    avm.Add(obj);
            //}

            return avm;
        }

        public List<AdoptCaseGridMain> GetAdoptionWorkList()
        {
            // initializing view model
            List<AdoptCaseGridMain> caseViewModel = new List<AdoptCaseGridMain>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get Adoption cases that have been accepted on adoption module

            //var clientAssessments = (from p in db.Intake_Assessments
            //                         join client in db.Clients on p.Client_Id equals client.Client_Id
            //                         join person in db.Persons on client.Person_Id equals person.Person_Id
            //                         join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

            //                         join worklist in db.VEP_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id

            //                         where (subcat.Problem_Sub_Category_Id == 18 && worklist.VEP_Record_Status_Id != 1)

            //                         group p by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, p.Intake_Assessment_Id, worklist.VEP_Record_Status_Id }
            //                         into g
            //                         select new
            //                         {
            //                             g.Key.Intake_Assessment_Id,
            //                             g.Key.Client_Id,
            //                             g.Key.First_Name,
            //                             g.Key.Last_Name,
            //                             g.Key.Identification_Number,
            //                             g.Key.VEP_Record_Status_Id,
            //                             Assessments = g.ToList()

            //                         }).ToList();
            //;
            //foreach (var item in clientAssessments)
            //{
            //    AdoptCaseGridMain obj = new AdoptCaseGridMain();

            //    obj.IntakeAssessmentId = item.Intake_Assessment_Id;
            //    obj.ClientId = item.Client_Id;
            //    obj.FirstName = item.First_Name;
            //    obj.LastName = item.Last_Name;
            //    obj.IDNumber = item.Identification_Number;
            //    obj.VEpRecordStatusDescription = db.apl_VEP_Record_Status.Find(item.VEP_Record_Status_Id).Description;
            //    caseViewModel.Add(obj);
            //}
            return caseViewModel;


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

        public void UpdateWorkVEPListCase(VEPInboxVM list, int AssId)
        {
            // initialise connection
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    // Accept New case on work list 
                    var editlist = (from i in db.VEP_WorkList
                                    where i.Intake_Assessment_Id == (AssId)
                                    select i.Status_Id).FirstOrDefault();

                    VEP_WorkList edit = db.VEP_WorkList.Find(editlist);


                    edit.Intake_Assessment_Id = AssId;
                    //edit.VEP_Record_Status_Id = 2;
                    //edit.Accepted_By = list.Case_Manager_Id;
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




        #endregion









    }
}
