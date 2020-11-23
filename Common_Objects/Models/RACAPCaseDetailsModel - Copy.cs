using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using System.Web.Mvc;
using static Common_Objects.ViewModels.RACAPCaseViewModel;
using System.Data.Entity;

namespace Common_Objects.Models
{
    public class RACAPCaseDetailsModel
    {
        #region Child
        #region Documents

        public void CreateRACAPDocuments(RACAPSupportingDocumentsVM docAttached)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())

            {
                try
                {
                    RACAPSupportingDocument newdocAttached = new RACAPSupportingDocument();
                    newdocAttached.Intake_Assessment_Id = docAttached.Intake_Assessment_Id;
                    newdocAttached.DocumentsFileName = docAttached.DocumentsFileName;
                    newdocAttached.DocumentsDescription = docAttached.DocumentsDescription;
                    newdocAttached.DocumentsLabel = 1;
                    newdocAttached.DocumentsPath = "~/App_Data/RACAPSupportingDocuments/";
                    newdocAttached.DocumentsCheck = docAttached.DocumentsCheck;
                    db.RACAPSupportingDocuments.Add(newdocAttached);
                    db.SaveChanges();
                }
                catch
                {

                }

            }

        }

        public RACAPSupportingDocumentsVM GetRACAPSupDocuments(int Id)
        {
            RACAPSupportingDocumentsVM ASDvm = new RACAPSupportingDocumentsVM();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var adoptDocDB = db.AdoptionSupportingDocuments.Find(Id);
            ASDvm.DocumentsId = adoptDocDB.DocumentsId;
            ASDvm.DocumentsFileName = adoptDocDB.DocumentsFileName;
            ASDvm.DocumentsDescription = adoptDocDB.DocumentsDescription;
            ASDvm.DocumentsLabel = adoptDocDB.DocumentsLabel;
            ASDvm.DocumentsPath = adoptDocDB.DocumentsPath;
            ASDvm.Intake_Assessment_Id = Convert.ToInt32(adoptDocDB.Intake_Assessment_Id);
            return ASDvm;
        }

        public List<RACAPSupportingDocumentsVM> GetRACAPSupportDocumentsList(int intake_Assessment_Id)
        {
            List<RACAPSupportingDocumentsVM> avm = new List<RACAPSupportingDocumentsVM>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var documentsList =
                (from doc in db.RACAPSupportingDocuments
                 where (doc.Intake_Assessment_Id == intake_Assessment_Id)
                 select new
                 {
                     doc.Intake_Assessment_Id,
                     doc.SelectKindOfDocId,
                     doc.DocumentsFileName,
                     doc.DocumentsDescription,
                     doc.DocumentsLabel,
                     doc.DocumentsPath,
                     doc.DocumentsCheck,
                     doc.Document_Id

                 }).ToList();
            foreach (var item in documentsList)
            {
                RACAPSupportingDocumentsVM objadptDcmntsvm = new RACAPSupportingDocumentsVM();
                objadptDcmntsvm.DocumentsId = item.Document_Id;
                objadptDcmntsvm.Intake_Assessment_Id = item.Intake_Assessment_Id;
                objadptDcmntsvm.DocumentsFileName = item.DocumentsFileName;
                objadptDcmntsvm.DocumentsDescription = item.DocumentsDescription;
                objadptDcmntsvm.DocLabel = db.apl_ADOPTKindOfDocument.Find(item.DocumentsLabel).Description;
                objadptDcmntsvm.DocumentsPath = item.DocumentsPath;
                objadptDcmntsvm.KindOfDocument = db.apl_ADOPTKindOfDocument.Find(item.DocumentsCheck).Description;

                avm.Add(objadptDcmntsvm);
            }
            return avm;
        }

        #endregion Documents

        #region PERSON DETAILS
        // Get Adoption Client by int assessment ID
        public int GetRACAPPersonIdByintAssId(int intAssessmentId)
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

        // GET A PERSON BY PERSON ID
        public RACAPPersonViewModel GetRACAPPerson(int personId, int Id)
        {
            RACAPPersonViewModel Racapvm = new RACAPPersonViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var act = (from a in db.Persons
                       join b in db.Clients on a.Person_Id equals b.Person_Id
                       join c in db.Intake_Assessments on b.Client_Id equals c.Client_Id
                       where a.Person_Id == personId && c.Intake_Assessment_Id == Id
                       select a).FirstOrDefault();


            //db.Persons.Find(personId);
            Racapvm.Person_Id = personId;
            Racapvm.First_Name = act.First_Name;
            Racapvm.Last_Name = act.Last_Name;
            Racapvm.Known_As = act.Known_As;
            Racapvm.Identification_Type_Id = act.Identification_Type_Id;
            Racapvm.Identification_Number = act.Identification_Number;
            Racapvm.Date_Of_Birth = act.Date_Of_Birth;
            Racapvm.Age = act.Age;
            Racapvm.Is_Estimated_Age = act.Is_Estimated_Age;
            Racapvm.Language_Id = act.Language_Id;
            Racapvm.Gender_Id = act.Gender_Id;
            Racapvm.Marital_Status_Id = act.Marital_Status_Id;
            Racapvm.Preferred_Contact_Type_Id = act.Preferred_Contact_Type_Id;
            Racapvm.Religion_Id = act.Religion_Id;
            Racapvm.Phone_Number = act.Phone_Number;
            Racapvm.Mobile_Phone_Number = act.Mobile_Phone_Number;
            Racapvm.Email_Address = act.Email_Address;
            Racapvm.Population_Group_Id = act.Population_Group_Id;
            Racapvm.Nationality_Id = act.Nationality_Id;
            Racapvm.Disability_Type_Id = act.Disability_Type_Id;

            return Racapvm;
        }

        public RACAPPersonViewModel GetRACAPPersonChild(int personId)
        {
            RACAPPersonViewModel Adoptvm = new RACAPPersonViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var act = db.Persons.Find(personId);
            Adoptvm.Person_Id = personId;
            Adoptvm.First_Name = act.First_Name;
            Adoptvm.Last_Name = act.Last_Name;
            Adoptvm.Known_As = act.Known_As;
            Adoptvm.Identification_Type_Id = act.Identification_Type_Id;
            Adoptvm.Identification_Number = act.Identification_Number;
            Adoptvm.Date_Of_Birth = act.Date_Of_Birth;

            DateTime old = Convert.ToDateTime(Adoptvm.Date_Of_Birth);

            int age = DateTime.Now.AddYears(-old.Year).Year;

            Adoptvm.Age = age;

            Adoptvm.Is_Estimated_Age = act.Is_Estimated_Age;
            Adoptvm.Language_Id = act.Language_Id;
            Adoptvm.Gender_Id = act.Gender_Id;
            Adoptvm.Marital_Status_Id = act.Marital_Status_Id;
            Adoptvm.Preferred_Contact_Type_Id = act.Preferred_Contact_Type_Id;
            Adoptvm.Religion_Id = act.Religion_Id;
            Adoptvm.Phone_Number = act.Phone_Number;
            Adoptvm.Mobile_Phone_Number = act.Mobile_Phone_Number;
            Adoptvm.Email_Address = act.Email_Address;
            Adoptvm.Population_Group_Id = act.Population_Group_Id;
            Adoptvm.Nationality_Id = act.Nationality_Id;
            Adoptvm.Disability_Type_Id = act.Disability_Type_Id;

            return Adoptvm;
        }

        //UPDATE PERSON DETAILS 
        public void UpdateAdoptionPerson(RACAPPersonViewModel person, string userName)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    Person newPerson = db.Persons.Find(person.Person_Id);

                    newPerson.First_Name = person.First_Name;
                    newPerson.Last_Name = person.Last_Name;
                    newPerson.Known_As = person.Known_As;
                    newPerson.Identification_Type_Id = person.Identification_Type_Id;
                    newPerson.Identification_Number = person.Identification_Number;
                    newPerson.Date_Of_Birth = person.Date_Of_Birth;
                    newPerson.Age = person.Age;
                    newPerson.Is_Estimated_Age = person.Is_Estimated_Age;
                    newPerson.Language_Id = person.Language_Id;
                    newPerson.Gender_Id = person.Gender_Id;
                    newPerson.Marital_Status_Id = person.Marital_Status_Id;
                    newPerson.Preferred_Contact_Type_Id = person.Preferred_Contact_Type_Id;
                    newPerson.Religion_Id = person.Religion_Id;
                    newPerson.Phone_Number = person.Phone_Number;
                    newPerson.Mobile_Phone_Number = person.Mobile_Phone_Number;
                    newPerson.Email_Address = person.Email_Address;
                    newPerson.Population_Group_Id = person.Population_Group_Id;
                    newPerson.Nationality_Id = person.Nationality_Id;
                    newPerson.Disability_Type_Id = person.Disability_Type_Id;
                    newPerson.Modified_By = userName;
                    newPerson.Date_Last_Modified = DateTime.Now;
                    db.SaveChanges();
                }
                catch
                {

                }

            }

        }



        #endregion FOR PESRON DETAILS

        // RACAP CHILD CODE..................................................................................................
        #region  RACAP CHILD CODE  

        #region SEARCH ADD AND UPDATE RACAP CASE CHILD

        #region Other Function For Child

         public List<RACAPCaseGridMain> GetRACAPMATCHAss(DateTime dob, int race, int Gendeid)
        {
            // initializing view model
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            List<RACAPCaseGridMain> caseViewModel = new List<RACAPCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     join racapcase in db.RACAP_Case_Details on p.Intake_Assessment_Id equals racapcase.Intake_Assessment_Id
                                     //join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                     join Parent in db.RACAP_Prospective_Parent on racapcase.RACAP_Case_Id equals Parent.RACAP_Case_Id

                                     where (Parent.Gender_Id == Gendeid && Parent.Race_Id == race && racapcase.RACAP_Record_Status_Id == 1) // To be modified to check record Status

                                     group p by new
                                     {
                                         client.Client_Id,
                                         person.First_Name,
                                         person.Last_Name,
                                         person.Identification_Number,
                                         p.Intake_Assessment_Id,
                                         person.Population_Group_Id,
                                         person.Gender_Id,
                                         person.Age,
                                         racapcase.RACAP_Record_Status_Id,
                                         Parent.RACAP_Prospective_Parent_Id,
                                     }

                                     into g
                                     select new
                                     {
                                         g.Key.Intake_Assessment_Id,
                                         g.Key.Client_Id,
                                         g.Key.First_Name,
                                         g.Key.Last_Name,
                                         g.Key.Identification_Number,
                                         g.Key.RACAP_Record_Status_Id,
                                         g.Key.RACAP_Prospective_Parent_Id,
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
                obj.RACAPRecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                obj.RACAP_Prospective_Parent_Id = item.RACAP_Prospective_Parent_Id;
                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }


        // GET Match In Proccess
        public List<RACAPCaseGridMain> GetRACAPMATCHInProcess(int id)
        {
            // initializing view model
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            List<RACAPCaseGridMain> caseViewModel = new List<RACAPCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module


            var Matchid = (from matches in db.RACAP_Matches

                           join child in db.RACAP_Adoptive_Child on matches.RACAP_Adoptive_Child_Id equals child.RACAP_Adoptive_Child_Id
                           join caser in db.RACAP_Case_Details on child.RACAP_Case_Id equals caser.RACAP_Case_Id
                           join p in db.Intake_Assessments on caser.Intake_Assessment_Id equals p.Intake_Assessment_Id
                           join client in db.Clients on p.Client_Id equals client.Client_Id
                           join person in db.Persons on client.Person_Id equals person.Person_Id



                           where ((matches.RACAP_Adoptive_Child_Id == id) && ((caser.RACAP_Record_Status_Id == 2) || (caser.RACAP_Record_Status_Id == 3)))
                           select matches.RACAP_Match_Id).FirstOrDefault();

            if (Matchid != 0)
            {

                var Child = (from matches in db.RACAP_Matches

                             join Cchild in db.RACAP_Adoptive_Child on matches.RACAP_Adoptive_Child_Id equals Cchild.RACAP_Adoptive_Child_Id
                             join Ccaser in db.RACAP_Case_Details on Cchild.RACAP_Case_Id equals Ccaser.RACAP_Case_Id
                             join Cp in db.Intake_Assessments on Ccaser.Intake_Assessment_Id equals Cp.Intake_Assessment_Id
                             join Cclient in db.Clients on Cp.Client_Id equals Cclient.Client_Id
                             join Cperson in db.Persons on Cclient.Person_Id equals Cperson.Person_Id

                             where ((matches.RACAP_Match_Id == Matchid)) // To be modified to check record Status

                             group Cp by new
                             {

                                 matches.RACAP_Match_Id,
                                 matches.RACAP_Adoptive_Child_Id

                             }

                                         into g
                             select new
                             {
                                 g.Key.RACAP_Match_Id,
                                 g.Key.RACAP_Adoptive_Child_Id,
                                 Assessments = g.ToList()

                             }).ToList();

                var Parent = (from Pmatches in db.RACAP_Matches

                              join Pparent in db.RACAP_Prospective_Parent on Pmatches.RACAP_Prospective_Parent_Id equals Pparent.RACAP_Prospective_Parent_Id
                              join Pcaser in db.RACAP_Case_Details on Pparent.RACAP_Case_Id equals Pcaser.RACAP_Case_Id
                              join Pp in db.Intake_Assessments on Pcaser.Intake_Assessment_Id equals Pp.Intake_Assessment_Id
                              join Pclient in db.Clients on Pp.Client_Id equals Pclient.Client_Id
                              join Pperson in db.Persons on Pclient.Person_Id equals Pperson.Person_Id

                              where ((Pmatches.RACAP_Match_Id == Matchid)) // To be modified to check record Status

                              group Pp by new
                              {
                                  Pclient.Client_Id,
                                  Pperson.First_Name,
                                  Pperson.Last_Name,
                                  Pperson.Identification_Number,
                                  Pp.Intake_Assessment_Id,
                                  Pperson.Population_Group_Id,
                                  Pperson.Gender_Id,
                                  Pperson.Age,
                                  Pmatches.RACAP_Match_Id,
                                  Pmatches.RACAP_Prospective_Parent_Id,
                                  Pcaser.RACAP_Record_Status_Id
                              }

                                     into gP
                              select new
                              {
                                  gP.Key.Intake_Assessment_Id,
                                  gP.Key.Client_Id,
                                  gP.Key.First_Name,
                                  gP.Key.Last_Name,
                                  gP.Key.Identification_Number,
                                  gP.Key.RACAP_Record_Status_Id,
                                  gP.Key.RACAP_Match_Id,
                                  gP.Key.RACAP_Prospective_Parent_Id,
                                  Assessments = gP.ToList()

                              }).ToList();

                var Combined = (from q1 in Child
                                join q2 in Parent on q1.RACAP_Match_Id equals q2.RACAP_Match_Id

                                select new
                                {
                                    q2.Intake_Assessment_Id,
                                    q2.Client_Id,
                                    q2.First_Name,
                                    q2.Last_Name,
                                    q2.Identification_Number,
                                    q2.RACAP_Record_Status_Id,
                                    q2.RACAP_Prospective_Parent_Id,
                                    q1.RACAP_Adoptive_Child_Id

                                }).ToList();
                ;
                foreach (var item in Combined)
                {
                    RACAPCaseGridMain obj = new RACAPCaseGridMain();

                    obj.IntakeAssessmentId = item.Intake_Assessment_Id;
                    obj.ClientId = item.Client_Id;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.IDNumber = item.Identification_Number;
                    obj.RACAPRecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    obj.RACAP_Adoptive_Child_Id = item.RACAP_Adoptive_Child_Id;
                    obj.RACAP_Prospective_Parent_Id = item.RACAP_Prospective_Parent_Id;
                    caseViewModel.Add(obj);
                }
            }
            return caseViewModel;
        }

        // UPDATE RACAP RECORD STATUS

        public void UpdateRACAPRecordStatus(RACAPCaseViewModel cases, int Pid, int Cid, int option)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int ParentCaseid = (from i in db.RACAP_Prospective_Parent
                                        join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                        where i.RACAP_Prospective_Parent_Id == Pid
                                        select b.RACAP_Case_Id).FirstOrDefault();

                    int ChildCaseid = (from i in db.RACAP_Adoptive_Child
                                       join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                       where i.RACAP_Adoptive_Child_Id == Cid
                                       select b.RACAP_Case_Id).FirstOrDefault();

                    if (ParentCaseid != 0)
                    {
                        RACAP_Case_Details editp = db.RACAP_Case_Details.Find(ParentCaseid);
                        if (option == 1)
                        {
                            editp.RACAP_Record_Status_Id = 2;
                        }
                        if (option == 2)
                        {
                            editp.RACAP_Record_Status_Id = 3;
                        }
                        if (option == 3)
                        {
                            editp.RACAP_Record_Status_Id = 4;
                        }
                    }
                    if (ChildCaseid != 0)
                    {

                        RACAP_Case_Details edit = db.RACAP_Case_Details.Find(ChildCaseid);

                        if (option == 1)
                        {
                            edit.RACAP_Record_Status_Id = 2;
                        }
                        if (option == 2)
                        {
                            edit.RACAP_Record_Status_Id = 3;
                        }
                        if (option == 3)
                        {
                            edit.RACAP_Record_Status_Id = 4;
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
        }


        #endregion

        //Addition by Herman
        #region Other Function for Parent

        public List<RACAPCaseGridMain> GetRACAPMATCHAssParent(DateTime dob, int race, int Gendeid)
        {
            // initializing view model
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            List<RACAPCaseGridMain> caseViewModel = new List<RACAPCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     join racapcase in db.RACAP_Case_Details on p.Intake_Assessment_Id equals racapcase.Intake_Assessment_Id
                                     //join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                     join Child in db.RACAP_Adoptive_Child on racapcase.RACAP_Case_Id equals Child.RACAP_Case_Id
                                     join Parent in db.RACAP_Prospective_Parent on racapcase.RACAP_Case_Id equals Parent.RACAP_Case_Id
                                     where (
                                     //Child.Gender_Id==(Gendeid) && Child.Race_Id==(race)
                                     racapcase.RACAP_Record_Status_Id == 1
                                     ) // To be modified to check record Status

                                     group p by new 
                                     {
                                         client.Client_Id,
                                         person.First_Name,
                                         person.Last_Name,
                                         person.Identification_Number,
                                         p.Intake_Assessment_Id,
                                         person.Population_Group_Id,
                                         person.Gender_Id,
                                         person.Age,
                                         racapcase.RACAP_Record_Status_Id,
                                         Child.RACAP_Adoptive_Child_Id,
                                         Parent.RACAP_Prospective_Parent_Id,
                                     }

                                     into g
                                     select  g).ToList();

            ;
            foreach (var item in clientAssessments)
            {
                RACAPCaseGridMain obj = new RACAPCaseGridMain();

                obj.IntakeAssessmentId = item.Key.Intake_Assessment_Id;
                obj.ClientId = item.Key.Client_Id;
                obj.FirstName = item.Key.First_Name;
                obj.LastName = item.Key.Last_Name;
                obj.IDNumber = item.Key.Identification_Number;
                obj.RACAPRecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.Key.RACAP_Record_Status_Id).Description;
                obj.RACAP_Adoptive_Child_Id = item.Key.RACAP_Adoptive_Child_Id;
                obj.RACAP_Prospective_Parent_Id = item.Key.RACAP_Prospective_Parent_Id;
                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }
        //AdvancedSearch addition Herman
        
        //End AdvancedSearch addition Herman

        // GET Match In Proccess
        public List<RACAPCaseGridMain> GetRACAPMATCHInProcessParent(int id)
        {
            // initializing view model
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            List<RACAPCaseGridMain> caseViewModel = new List<RACAPCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module


            var MatchidParent = (from matchesParent in db.RACAP_Matches

                           join parent in db.RACAP_Prospective_Parent on matchesParent.RACAP_Prospective_Parent_Id equals parent.RACAP_Prospective_Parent_Id
                           join caser in db.RACAP_Case_Details on parent.RACAP_Case_Id equals caser.RACAP_Case_Id
                           join p in db.Intake_Assessments on caser.Intake_Assessment_Id equals p.Intake_Assessment_Id
                           join client in db.Clients on p.Client_Id equals client.Client_Id
                           join person in db.Persons on client.Person_Id equals person.Person_Id



                           where ((matchesParent.RACAP_Prospective_Parent_Id == id) && ((caser.RACAP_Record_Status_Id == 2) || (caser.RACAP_Record_Status_Id == 3)))
                           select matchesParent.RACAP_Match_Id).FirstOrDefault();

            if (MatchidParent != 0)
            {

                var Parent = (from matchesParent in db.RACAP_Matches

                             join Pparent in db.RACAP_Prospective_Parent on matchesParent.RACAP_Prospective_Parent_Id equals Pparent.RACAP_Prospective_Parent_Id
                              join Ccaser in db.RACAP_Case_Details on Pparent.RACAP_Case_Id equals Ccaser.RACAP_Case_Id
                             join Cp in db.Intake_Assessments on Ccaser.Intake_Assessment_Id equals Cp.Intake_Assessment_Id
                             join Cclient in db.Clients on Cp.Client_Id equals Cclient.Client_Id
                             join Cperson in db.Persons on Cclient.Person_Id equals Cperson.Person_Id

                             where ((matchesParent.RACAP_Match_Id == MatchidParent)) // To be modified to check record Status

                             group Cp by new
                             {

                                 matchesParent.RACAP_Match_Id,
                                 matchesParent.RACAP_Prospective_Parent_Id

                             }

                                         into g
                             select new
                             {
                                 g.Key.RACAP_Match_Id,
                                 g.Key.RACAP_Prospective_Parent_Id,
                                 Assessments = g.ToList()

                             }).ToList();

                var Child = (from Cmatches in db.RACAP_Matches

                              join CChild in db.RACAP_Adoptive_Child on Cmatches.RACAP_Adoptive_Child_Id equals CChild.RACAP_Adoptive_Child_Id
                             join Pcaser in db.RACAP_Case_Details on CChild.RACAP_Case_Id equals Pcaser.RACAP_Case_Id
                              join Pp in db.Intake_Assessments on Pcaser.Intake_Assessment_Id equals Pp.Intake_Assessment_Id
                              join Pclient in db.Clients on Pp.Client_Id equals Pclient.Client_Id
                              join Pperson in db.Persons on Pclient.Person_Id equals Pperson.Person_Id

                              where ((Cmatches.RACAP_Match_Id == MatchidParent)) // To be modified to check record Status

                              group Pp by new
                              {
                                  Pclient.Client_Id,
                                  Pperson.First_Name,
                                  Pperson.Last_Name,
                                  Pperson.Identification_Number,
                                  Pp.Intake_Assessment_Id,
                                  Pperson.Population_Group_Id,
                                  Pperson.Gender_Id,
                                  Pperson.Age,
                                  Cmatches.RACAP_Match_Id,
                                  Cmatches.RACAP_Adoptive_Child_Id,
                                  Pcaser.RACAP_Record_Status_Id
                              }

                                     into gC
                              select new
                              {
                                  gC.Key.Intake_Assessment_Id,
                                  gC.Key.Client_Id,
                                  gC.Key.First_Name,
                                  gC.Key.Last_Name,
                                  gC.Key.Identification_Number,
                                  gC.Key.RACAP_Record_Status_Id,
                                  gC.Key.RACAP_Match_Id,
                                  gC.Key.RACAP_Adoptive_Child_Id,
                                  Assessments = gC.ToList()

                              }).ToList();

                var Combined = (from q1 in Parent
                                join q2 in Child on q1.RACAP_Match_Id equals q2.RACAP_Match_Id

                                select new
                                {
                                    q2.Intake_Assessment_Id,
                                    q2.Client_Id,
                                    q2.First_Name,
                                    q2.Last_Name,
                                    q2.Identification_Number,
                                    q2.RACAP_Record_Status_Id,
                                    q2.RACAP_Adoptive_Child_Id,
                                    q1.RACAP_Prospective_Parent_Id

                                }).ToList();
                ;
                foreach (var item in Combined)
                {
                    RACAPCaseGridMain obj = new RACAPCaseGridMain();

                    obj.IntakeAssessmentId = item.Intake_Assessment_Id;
                    obj.ClientId = item.Client_Id;
                    obj.FirstName = item.First_Name;
                    obj.LastName = item.Last_Name;
                    obj.IDNumber = item.Identification_Number;
                    obj.RACAPRecordStatusDescription = db.apl_RACAP_Record_Status.Find(item.RACAP_Record_Status_Id).Description;
                    obj.RACAP_Adoptive_Child_Id = item.RACAP_Adoptive_Child_Id;
                    obj.RACAP_Prospective_Parent_Id = item.RACAP_Prospective_Parent_Id;
                    caseViewModel.Add(obj);
                }
            }
            return caseViewModel;
        }

        // UPDATE RACAP RECORD STATUS

        //public void UpdateRACAPRecordStatusParent(RACAPCaseViewModel cases, int Pid, int Cid, int option)
        public void UpdateRACAPRecordStatusParent(RACAPCaseViewModel cases, int Pid, int Cid, int option, int LoggedIn_User_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int ParentCaseid = (from i in db.RACAP_Prospective_Parent
                                        join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                        where i.RACAP_Prospective_Parent_Id == Pid
                                        select b.RACAP_Case_Id).FirstOrDefault();

                    int ChildCaseid = (from i in db.RACAP_Adoptive_Child
                                       join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                       where i.RACAP_Adoptive_Child_Id == Cid
                                       select b.RACAP_Case_Id).FirstOrDefault();

                    if (ParentCaseid != 0)
                    {
                        RACAP_Case_Details editp = db.RACAP_Case_Details.Find(ParentCaseid);
                        if (option == 1)
                        {
                            editp.RACAP_Record_Status_Id = 2;
                        }
                        if (option == 2)
                        {
                            editp.RACAP_Record_Status_Id = 3;
                        }
                        if (option == 3)
                        {
                            editp.RACAP_Record_Status_Id = 4;
                        }
                    }
                    if (ChildCaseid != 0)
                    {

                        RACAP_Case_Details edit = db.RACAP_Case_Details.Find(ChildCaseid);

                        if (option == 1)
                        {
                            edit.RACAP_Record_Status_Id = 2;
                        }
                        if (option == 2)
                        {
                            edit.RACAP_Record_Status_Id = 3;
                        }
                        if (option == 3)
                        {
                            edit.RACAP_Record_Status_Id = 4;
                        }
                    }
                    //Update RACAP_Matches tbl
                    int RACAP_Match_Id = (from x in db.RACAP_Matches
                                          where (x.RACAP_Prospective_Parent_Id == Pid && x.RACAP_Adoptive_Child_Id == Cid)
                                          select x.RACAP_Match_Id).FirstOrDefault();
                    if(RACAP_Match_Id==0)
                    { 
                    RACAP_Matches RMat = new RACAP_Matches();
                        RMat.RACAP_Prospective_Parent_Id = Pid;
                        RMat.RACAP_Adoptive_Child_Id = Cid;
                        RMat.Date_Created = DateTime.Now;
                        RMat.Created_By = LoggedIn_User_Id;
                        db.RACAP_Matches.Add(RMat);
                    db.SaveChanges();
                    }
                    if (RACAP_Match_Id != 0)
                    {
                        RACAP_Matches RMat = db.RACAP_Matches.Find(RACAP_Match_Id);
                        RMat.RACAP_Prospective_Parent_Id = Pid;
                        RMat.RACAP_Adoptive_Child_Id = Cid;
                        RMat.Date_Modified = DateTime.Now;
                        RMat.Modified_By = LoggedIn_User_Id;
                        db.SaveChanges();
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
            }
        }




        #endregion

        //End Addition by Herman

        //GET ADOPTION CASE BY ADOPTION CASE ID

        #region GET RACAP CASE DETAILS

        public int GetRACAPChildCaseDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.RACAP_Case_Id).FirstOrDefault();
            }
        }


        public RACAPCaseViewModel GetRACAPChildCaseDetails(int RacapcaseId)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int CaseId = (from i in db.RACAP_Case_Details
                                   where i.RACAP_Case_Id == RacapcaseId
                                   select i.RACAP_Case_Id).FirstOrDefault();

                    RACAP_Case_Details act = db.RACAP_Case_Details.Find(CaseId);

                    vm.RACAP_Case_Id = CaseId;

                    vm.Expiry_Date = act.Expiry_Date;
                    vm.Date_Registered = act.Date_Registered;
                    vm.Comments = act.Comments;
                    vm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
                    vm.RACAP_Record_Status_Id = act.RACAP_Record_Status_Id;
                    vm.RACAP_Registration_Status_Id = act.RACAP_Registration_Status_Id;
                    vm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
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

        public RACAPCaseViewModel GetRACAPCaseDetailsCreate(int Case_Id)
        {
            RACAPCaseViewModel AdoptCasevm = new RACAPCaseViewModel();
            //SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            //var act1 = db.ADOPT_Case_Details.Find(Adopt_Case_Id);


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? CaseId = (from i in db.RACAP_Case_Details
                                   where i.RACAP_Case_Id == Case_Id
                                   select i.RACAP_Case_Id).FirstOrDefault();

                    RACAP_Case_Details act = db.RACAP_Case_Details.Find(CaseId);




                    //AdoptCasevm.Intake_Assessment_Id = Intake_Assessment_Id;
                    //AdoptCasevm.Adopt_Case_Id = CaseId;
                    //AdoptCasevm.OdoptionOrder_Date = act.OdoptionOrder_Date;
                    AdoptCasevm.Date_Registered = act.Date_Registered;
                    AdoptCasevm.Comments = act.Comments;
                    AdoptCasevm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
                    //AdoptCasevm.Legitimacy_Id = act.Legitimacy_Id;
                    //AdoptCasevm.Cross_Cultural_Id = act.Cross_Cultural_Id;
                    AdoptCasevm.RACAP_Record_Status_Id = act.RACAP_Record_Status_Id;
                    //AdoptCasevm.Adopting_Reason_Id = act.Adopting_Reason_Id;
                    //AdoptCasevm.Court_id = act.Court_Id;
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

                return AdoptCasevm;
            }
        }

        public int GetRACAPChildClientRefIdssId(int intAssessmentId)
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

        public string GetRACAPChildClientRef(int ClientRefid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id
                        where f.Client_Module_Id.Equals(ClientRefid)
                        select f.Client_Module_Ref_No).FirstOrDefault();
            }
        }

        #endregion

        #region RACAP CHILD FEATURES

        public int GetRACAPChildFeaturedByssId(int IntAss)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                var RacapCaseId = (from p in db.Persons
                                   join c in db.Clients on p.Person_Id equals c.Person_Id
                                   join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                                   join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                                   where i.Intake_Assessment_Id.Equals(IntAss)
                                   select Case.RACAP_Case_Id).FirstOrDefault();

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        join F in db.RACAP_Adoptive_Child on Case.RACAP_Case_Id equals F.RACAP_Case_Id
                        where Case.RACAP_Case_Id.Equals(RacapCaseId)
                        select F.RACAP_Adoptive_Child_Id).FirstOrDefault();
            }
        }

        public RACAPCaseViewModel GetRACAPChildFeaturedDetails(int AdoptiveChildId)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? Adoptive_Child_Id = (from i in db.RACAP_Case_Details
                                              join b in db.RACAP_Adoptive_Child on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                              where b.RACAP_Adoptive_Child_Id == AdoptiveChildId
                                              select b.RACAP_Adoptive_Child_Id).FirstOrDefault();

                    RACAP_Adoptive_Child act = db.RACAP_Adoptive_Child.Find(Adoptive_Child_Id);
                    if(act!=null) { 
                    vm.Eye_Color_Id = act.Eye_Color_Id;
                    vm.Skin_Colour_Id = act.Skin_Colour_Id;
                    vm.Body_Structure_Id = act.Body_Structure_Id;
                    vm.Ethnic_Cultural_Group_Id = act.Ethnic_Cultural_Group_Id;
                    vm.Special_Needs_Id = act.Special_Needs_Id;
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

        public void CreateRACAPChildFeature(RACAPCaseViewModel cases, int id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    RACAP_Adoptive_Child newCase = new RACAP_Adoptive_Child();

                    newCase.RACAP_Case_Id = (from a in db.Intake_Assessments
                                             join b in db.RACAP_Case_Details on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             where a.Intake_Assessment_Id == id
                                             select b.RACAP_Case_Id).FirstOrDefault();
                    db.RACAP_Adoptive_Child.Add(newCase);
                    db.SaveChanges();
                    int idnew = newCase.RACAP_Adoptive_Child_Id;
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

        public RACAPCaseViewModel GetRACAPChildFeatureDetailsCreate(int AdoptiveChildid)
        {
            RACAPCaseViewModel AdoptCasevm = new RACAPCaseViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? childid = (from i in db.RACAP_Case_Details
                                    join b in db.RACAP_Adoptive_Child on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                    where b.RACAP_Adoptive_Child_Id == AdoptiveChildid
                                    select b.RACAP_Adoptive_Child_Id).FirstOrDefault();

                    RACAP_Adoptive_Child act = db.RACAP_Adoptive_Child.Find(childid);

                    if (AdoptCasevm.Eye_Color_Id != null)
                    {

                        AdoptCasevm.Eye_Color_Id = act.Eye_Color_Id;
                    }
                    if (AdoptCasevm.Special_Needs_Id != null)
                    {
                        AdoptCasevm.Special_Needs_Id = act.Special_Needs_Id;
                    }
                    if (AdoptCasevm.Body_Structure_Id != null)
                    {


                        AdoptCasevm.Body_Structure_Id = act.Body_Structure_Id;
                    }
                    if (AdoptCasevm.Ethnic_Cultural_Group_Id != null)
                    {


                        AdoptCasevm.Ethnic_Cultural_Group_Id = act.Ethnic_Cultural_Group_Id;
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

                return AdoptCasevm;
            }
        }

        public void UpdateRACAPChildFeatures(MainPageModelRACAPVM cases, int userId, int Assid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int AdoptiveChildId1 = (from i in db.RACAP_Adoptive_Child
                                            join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                            join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                            where a.Intake_Assessment_Id == Assid
                                            select i.RACAP_Adoptive_Child_Id).FirstOrDefault();

                    RACAP_Adoptive_Child editCase = db.RACAP_Adoptive_Child.Find(AdoptiveChildId1);

                    editCase.RACAP_Adoptive_Child_Id = AdoptiveChildId1;


                    if (cases.RACAPCaseViewModel.Eye_Color_Id != null)
                    {
                        editCase.Eye_Color_Id = cases.RACAPCaseViewModel.Eye_Color_Id;
                    }

                    if (cases.RACAPCaseViewModel.Skin_Colour_Id != null)
                    {
                        editCase.Skin_Colour_Id = cases.RACAPCaseViewModel.Skin_Colour_Id;
                    }

                    if (cases.RACAPCaseViewModel.Special_Needs_Id != null)
                    {
                        editCase.Special_Needs_Id = cases.RACAPCaseViewModel.Special_Needs_Id;
                    }

                    if (cases.RACAPCaseViewModel.Body_Structure_Id != null)
                    {
                        editCase.Body_Structure_Id = cases.RACAPCaseViewModel.Body_Structure_Id;
                    }

                    if (cases.RACAPCaseViewModel.Ethnic_Cultural_Group_Id != null)
                    {

                        editCase.Ethnic_Cultural_Group_Id = cases.RACAPCaseViewModel.Ethnic_Cultural_Group_Id;
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
        }



        #endregion

        #region RACAP CHILD REMOVAL
        public int GetRACAPChildRemovalByssId(int IntAss)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                var RacapCaseId = (from p in db.Persons
                                   join c in db.Clients on p.Person_Id equals c.Person_Id
                                   join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                                   join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                                   where i.Intake_Assessment_Id.Equals(IntAss)
                                   select Case.RACAP_Case_Id).FirstOrDefault();

                return (from Case in db.RACAP_Case_Details
                        where Case.RACAP_Case_Id.Equals(RacapCaseId)
                        select Case.RACAP_Case_Id).FirstOrDefault();
            }
        }

        public RACAPCaseViewModel GetRACAPChildRemovalDetails(int Caseid)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? removeid = (from i in db.RACAP_Case_Details
                                     join b in db.RACAP_Removal on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                     where i.RACAP_Case_Id == Caseid
                                     select b.Removal_Id).FirstOrDefault();
                    if (removeid > 0)
                    {

                        RACAP_Removal act = db.RACAP_Removal.Find(removeid);

                        vm.Removal_Type_Id = act.Removal_Type_Id;
                        vm.Removal_Comments = act.Removal_Comments;
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

        public void UpdateRACAPChildRemoval(MainPageModelRACAPVM cases, int userId, int Assid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int RACAP_Case_Id1 = (from i in db.RACAP_Adoptive_Child
                                          join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                          join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                          where a.Intake_Assessment_Id == Assid
                                          select b.RACAP_Case_Id).FirstOrDefault();

                    int RACAP_Case_Id1r = (from i in db.RACAP_Adoptive_Child
                                           join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                           join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                           join r in db.RACAP_Removal on b.RACAP_Case_Id equals r.RACAP_Case_Id
                                           where r.RACAP_Case_Id == RACAP_Case_Id1
                                           select b.RACAP_Case_Id).FirstOrDefault();
                    if (RACAP_Case_Id1r <= 0)
                    {
                        RACAP_Removal newremove = new RACAP_Removal();
                        newremove.RACAP_Case_Id = RACAP_Case_Id1;

                        newremove.Date_Removed = DateTime.Now;
                        db.RACAP_Removal.Add(newremove);
                        db.SaveChanges();
                    }


                    int RACAP_Case_Id1r2 = (from i in db.RACAP_Adoptive_Child
                                            join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                            join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                            join r in db.RACAP_Removal on b.RACAP_Case_Id equals r.RACAP_Case_Id
                                            where r.RACAP_Case_Id == RACAP_Case_Id1
                                            select b.RACAP_Case_Id).FirstOrDefault();

                    //Update.....................................................................
                    if (RACAP_Case_Id1r2 >= 0)
                    {
                        int Removalid = (from i in db.RACAP_Adoptive_Child
                                         join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                         join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                         join r in db.RACAP_Removal on b.RACAP_Case_Id equals r.RACAP_Case_Id
                                         where r.RACAP_Case_Id == RACAP_Case_Id1r2
                                         select r.Removal_Id).FirstOrDefault();

                        RACAP_Removal edit = db.RACAP_Removal.Find(Removalid);

                        edit.RACAP_Case_Id = RACAP_Case_Id1r2;

                        if (cases.RACAPCaseViewModel.Removal_Type_Id != null)
                        {
                            edit.Removal_Type_Id = cases.RACAPCaseViewModel.Removal_Type_Id;
                        }
                        if (cases.RACAPCaseViewModel.Removal_Comments != null)
                        {
                            edit.Removal_Comments = cases.RACAPCaseViewModel.Removal_Comments;
                            edit.Date_Removed = DateTime.Now;
                        }
                        db.SaveChanges();
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
            }
        }

        #endregion

        // GET ORGANISATION REPSONSIBLE FOR ADOPTION

        #region GET ORGANISATION REPSONSIBLE FOR RACAP


        public int GetRACAPChildOrganisationByssId(int IntAss)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                var RacapCaseId = (from p in db.Persons
                                   join c in db.Clients on p.Person_Id equals c.Person_Id
                                   join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                                   join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                                   where i.Intake_Assessment_Id.Equals(IntAss)
                                   select Case.RACAP_Case_Id).FirstOrDefault();

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        join F in db.RACAP_OrgansationResponsible on Case.RACAP_Case_Id equals F.RACAP_Case_Id
                        where Case.RACAP_Case_Id.Equals(RacapCaseId)
                        select F.RACAP_Responsible_Organisation).FirstOrDefault();
            }
        }

        public RACAPCaseViewModel GetRACAPChildOrganisationDetails(int OrganizationIdChild)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? OrganizationChild = (from i in db.RACAP_Case_Details
                                              join b in db.RACAP_OrgansationResponsible on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                              where b.RACAP_Responsible_Organisation == OrganizationIdChild
                                              select b.RACAP_Responsible_Organisation).FirstOrDefault();

                    RACAP_OrgansationResponsible act = db.RACAP_OrgansationResponsible.Find(OrganizationChild);
                    if (act.Organization_Id_Child != null)
                    {
                        vm.Organization_Id = act.Organization_Id_Child;
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


        public void CreateRACAPChildOrganisation(RACAPCaseViewModel cases, int id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    RACAP_OrgansationResponsible newCase = new RACAP_OrgansationResponsible();

                    newCase.RACAP_Case_Id = (from a in db.Intake_Assessments
                                             join b in db.RACAP_Case_Details on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             where a.Intake_Assessment_Id == id
                                             select b.RACAP_Case_Id).FirstOrDefault();
                    db.RACAP_OrgansationResponsible.Add(newCase);
                    db.SaveChanges();
                    int idnew = newCase.RACAP_Responsible_Organisation;
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

        public RACAPCaseViewModel GetRACAPChildOrganisationDetailsCreate(int ResponsibleOrganisationid)
        {
            RACAPCaseViewModel AdoptCasevm = new RACAPCaseViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? ResponsibleOrganisation = (from i in db.RACAP_Case_Details
                                                    join b in db.RACAP_OrgansationResponsible on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                                    where b.RACAP_Responsible_Organisation == ResponsibleOrganisationid
                                                    select b.RACAP_Responsible_Organisation).FirstOrDefault();

                    RACAP_OrgansationResponsible act = db.RACAP_OrgansationResponsible.Find(ResponsibleOrganisation);


                    if (act.Organization_Id_Child != null)
                    {
                        AdoptCasevm.Organization_Id = act.Organization_Id_Child;
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

                return AdoptCasevm;
            }
        }

        public void UpdateRACAPChildOrganisation(MainPageModelRACAPVM cases, int userId, int Assid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int ResponsibleOrganisation = (from i in db.RACAP_OrgansationResponsible
                                                   join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                                   join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                                   where a.Intake_Assessment_Id == Assid
                                                   select i.RACAP_Responsible_Organisation).FirstOrDefault();

                    RACAP_OrgansationResponsible editCase = db.RACAP_OrgansationResponsible.Find(ResponsibleOrganisation);

                    editCase.RACAP_Responsible_Organisation = ResponsibleOrganisation;


                    if (cases.RACAPCaseViewModel.Organization_Id != null)
                    {
                        editCase.Organization_Id_Child = cases.RACAPCaseViewModel.Organization_Id;
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
        }


        #endregion

        //ADD ADOPTION CASE DETAILS
        #region ADD RACAP CASE DETAILS 

        public void CreateRACAPChildCase(RACAPCaseViewModel cases, int id, int clientref, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    RACAP_Case_Details newCase = new RACAP_Case_Details();

                    newCase.Intake_Assessment_Id = id;
                    //newCase.Client_Module_Id = cases.Client_Module_Id;
                    newCase.Intake_Assessment_Id = (from a in db.Intake_Assessments
                                                    where a.Intake_Assessment_Id == id
                                                    select a.Intake_Assessment_Id).FirstOrDefault();
                    db.RACAP_Case_Details.Add(newCase);
                    db.SaveChanges();
                    int idnew = newCase.RACAP_Case_Id;
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

        //Herman
        public void CreateRACAPOrgnanisationResponsible(RACAPCaseViewModel cases, int id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    RACAP_OrgansationResponsible newCaseOrgRespon = new RACAP_OrgansationResponsible();

                    newCaseOrgRespon.Intake_Assessment_Id = id;
                    //newCase.Client_Module_Id = cases.Client_Module_Id;
                    newCaseOrgRespon.RACAP_Case_Id = (from a in db.RACAP_Case_Details
                                                      where a.Intake_Assessment_Id == id
                                                      select a.RACAP_Case_Id).FirstOrDefault();
                    //newCaseOrgRespon.Organization_Id_Child = cases.Organization_Id_Child;
                    //newCaseOrgRespon.Organization_Id_Parent = cases.Organization_Id_Parent;
                    db.RACAP_OrgansationResponsible.Add(newCaseOrgRespon);
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
        //End Herman

        #endregion

        //UPDATE CASE DETAILS
        #region UPDATE RACAP CASE DETAILS
        public void UpdateRACAPCase(MainPageModelRACAPVM cases, int userId, int AssId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? SubCategoryId = (from a in db.Intake_Assessments
                                         where a.Intake_Assessment_Id == AssId
                                         select a.Problem_Sub_Category_Id).FirstOrDefault();
                    int ? CaseId = (from i in db.RACAP_Case_Details
                                    where i.Intake_Assessment_Id == AssId
                                    select i.RACAP_Case_Id).FirstOrDefault();

                    RACAP_Case_Details editCase = db.RACAP_Case_Details.Find(CaseId);



                    editCase.Intake_Assessment_Id = AssId;


                    if (cases.RACAPCaseViewModel.Date_Registered != null)
                    {
                        editCase.Date_Registered = cases.RACAPCaseViewModel.Date_Registered;
                    }

                    if (cases.RACAPCaseViewModel.Date_Registered != null && SubCategoryId == 19)
                    {
  
                            DateTime startDate = Convert.ToDateTime(cases.RACAPCaseViewModel.Date_Registered);
                            DateTime expiryDate = startDate.AddDays(30);
                            editCase.Expiry_Date = expiryDate;
                    }

                
                    if (cases.RACAPCaseViewModel.Date_Registered != null && SubCategoryId == 20)
                    {
                        DateTime startDate = Convert.ToDateTime(cases.RACAPCaseViewModel.Date_Registered);
                        DateTime expiryDate = startDate.AddYears(3);
                        editCase.Expiry_Date = expiryDate;
                    }

                if (cases.RACAPCaseViewModel.Adoptions_Reason_Id != null)
                    {
                        editCase.Adoptions_Reason_Id = cases.RACAPCaseViewModel.Adoptions_Reason_Id;
                    }

                    if (cases.RACAPCaseViewModel.RACAP_Record_Status_Id != null)
                    {
                        editCase.RACAP_Record_Status_Id = cases.RACAPCaseViewModel.RACAP_Record_Status_Id;
                    }

                    if (cases.RACAPCaseViewModel.RACAP_Registration_Status_Id != null)
                    {

                        editCase.RACAP_Registration_Status_Id = cases.RACAPCaseViewModel.RACAP_Registration_Status_Id;
                    }
                    if (cases.RACAPCaseViewModel.RACAP_Record_Status_Id != null)
                    {

                        editCase.RACAP_Record_Status_Id = cases.RACAPCaseViewModel.RACAP_Record_Status_Id;
                    }

                    if (cases.RACAPCaseViewModel.Comments != null)
                    {

                        editCase.Comments = cases.RACAPCaseViewModel.Comments;
                    }

                    //db.RACAP_Case_Details.Add(editCase);
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

        public void UpdateRACAPOrganisation(MainPageModelRACAPVM cases, int userId, int Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? RACAPORGResponId = (from d in db.RACAP_OrgansationResponsible
                                             where d.Intake_Assessment_Id == Id
                                             select d.RACAP_Responsible_Organisation).FirstOrDefault();
                    RACAP_OrgansationResponsible editCase = db.RACAP_OrgansationResponsible.Find(RACAPORGResponId);
                    editCase.Intake_Assessment_Id = Id;
                    editCase.RACAP_Case_Id = editCase.RACAP_Case_Id;
                    editCase.Organization_Id_Child = cases.RACAPCaseViewModel.Organization_Id_Child;
                    editCase.Organization_Id_Parent = cases.RACAPCaseViewModel.Organization_Id_Parent;
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


        #endregion

        // RACAP 


        #region  LOAD DROP DOWNS MODEL

        #region RACAP MODULE DROP DOWNS

        public List<RemovalTypeLookupRACAP> GetAllRemovalReason()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Removal = db.apl_RACAP_Client_Removal.Select(o => new RemovalTypeLookupRACAP
                {
                    Removal_Type_Id = o.Removal_Type_Id,
                    Description = o.Description
                }).ToList();

                return Removal;
            }
        }

        public List<ReasonForAdoptionLookupRACAP> GetAllReasonForAdoptionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var AdoptionReasonType = db.apl_Adoption_Reason.Select(o => new ReasonForAdoptionLookupRACAP
                {
                    Adoptions_Reason_Id = o.Adoptions_Reason_Id,
                    Description = o.Description
                }).ToList();

                return AdoptionReasonType;
            }
        }

        public List<RACAPRegistrationStatusLookupAdopt> GetAllRegistration_StatusType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var registration_StatusType = db.apl_RACAP_Registration_Status.Select(o => new RACAPRegistrationStatusLookupAdopt
                {
                    RACAP_Registration_Status_Id = o.RACAP_Registration_Status_Id,
                    Description = o.Description
                }).ToList();

                return registration_StatusType;
            }
        }

        
        public List<EyeColorsLookupAdopt> GetAllEyeColors()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var EyeColors = db.Eye_Colors.Select(o => new EyeColorsLookupAdopt
                {
                    Eye_Color_Id = o.Eye_Color_Id,
                    Description = o.Description
                }).ToList();

                return EyeColors;
            }
        }

        public List<BodyStructureLookupAdopt> GetAllBodyStructure()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var BodyStructure = db.apl_Body_Structure.Select(o => new BodyStructureLookupAdopt
                {
                    Body_Structure_Id = o.Body_Structure_Id,
                    Description = o.Description
                }).ToList();

                return BodyStructure;
            }
        }


        public List<SpecialNeedsLookupAdopt> GetAllSpecialNeeds()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var SpecialNeeds = db.apl_Special_Need.Select(o => new SpecialNeedsLookupAdopt
                {
                    Special_Needs_Id = o.Special_Needs_Id,
                    Description = o.Description
                }).ToList();

                return SpecialNeeds;
            }
        }

        public List<SkinColourLookupAdopt> GetAllSkinColour()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var SkinColour = db.apl_Skin_Colour.Select(o => new SkinColourLookupAdopt
                {
                    Skin_Colour_Id = o.Skin_Colour_Id,
                    Description = o.Description
                }).ToList();

                return SkinColour;
            }
        }

        public List<EthnicCulturalLookupAdopt> GetAllEthnicCultural()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var EthnicCultural = db.apl_Ethnic_Cultural.Select(o => new EthnicCulturalLookupAdopt
                {
                    Ethnic_Cultural_Group_Id = o.Ethnic_Cultural_Id,
                    Description = o.Description
                }).ToList();

                return EthnicCultural;
            }
        }

        public List<RACAPRegistrationStatusLookupAdopt> GetAllRACAPRegistrationStatus()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var RegistrationStatus = db.apl_RACAP_Registration_Status.Select(o => new RACAPRegistrationStatusLookupAdopt
                {
                    RACAP_Registration_Status_Id = o.RACAP_Registration_Status_Id,
                    Description = o.Description
                }).ToList();

                return RegistrationStatus;
            }
        }

        public List<RACAPRecordStatusLookupAdopt> GetAllRACAPRecordStatus()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var RecordStatus = db.apl_RACAP_Record_Status.Select(o => new RACAPRecordStatusLookupAdopt
                {
                    RACAP_Record_Status_Id = o.RACAP_Record_Status_Id,
                    Description = o.Description
                }).ToList();

                return RecordStatus;
            }
        }

        public List<OrganisationTypeLookupRACAPChild> GetAllChildOrganisationType()
        {

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Child = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupRACAPChild
                {
                    IdType = o.IdType,
                    Description = o.Description
                }).ToList();

                return Child;
            }
        }

        #endregion


        public List<IdentificationTypeLookupRACAP> GetAllIdentificationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var idType = db.Identification_Types.Select(o => new IdentificationTypeLookupRACAP
                {
                    Identification_Type_Id = o.Identification_Type_Id,
                    Description = o.Description
                }).ToList();

                return idType;
            }
        }

        public List<LanguageTypeLookupRACAP> GetAllLanguageType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var languageType = db.Languages.Select(o => new LanguageTypeLookupRACAP
                {
                    Language_Id = o.Language_Id,
                    Description = o.Description
                }).ToList();

                return languageType;
            }
        }


        public List<GenderTypeLookupRACAP> GetAllGenderType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var genderType = db.Genders.Select(o => new GenderTypeLookupRACAP
                {
                    Gender_Id = o.Gender_Id,
                    Description = o.Description
                }).ToList();

                return genderType;
            }
        }

        public List<MaritalTypeLookupRACAP> GetAllMaritalType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var maritalType = db.Marital_Statusses.Select(o => new MaritalTypeLookupRACAP
                {
                    Marital_Status_Id = o.Marital_Status_Id,
                    Description = o.Description
                }).ToList();

                return maritalType;
            }
        }

        public List<ContactTypeLookupRACAP> GetAllContactType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var contactType = db.Preferred_Contact_Types.Select(o => new ContactTypeLookupRACAP
                {
                    Preferred_Contact_Type_Id = o.Preferred_Contact_Type_Id,
                    Description = o.Description
                }).ToList();

                return contactType;
            }
        }

        public List<ReligionTypeLookupRACAP> GetAllReligionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var religionType = db.Religions.Select(o => new ReligionTypeLookupRACAP
                {
                    Religion_Id = o.Religion_Id,
                    Description = o.Description
                }).ToList();

                return religionType;
            }
        }

        public List<Population_GroupTypeLookupRACAP> GetAllPopulationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var populationType = db.Population_Groups.Select(o => new Population_GroupTypeLookupRACAP
                {
                    Population_Group_Id = o.Population_Group_Id,
                    Description = o.Description
                }).ToList();

                return populationType;
            }
        }

        public List<Special_NeedTypeLookupRACAP> GetAllSpecialNeedType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var specialNeedType = db.apl_Special_Need.Select(o => new Special_NeedTypeLookupRACAP
                {
                    Special_Need_Id = o.Special_Needs_Id,
                    Description = o.Description
                }).ToList();

                return specialNeedType;
            }
        }

        public List<Ethnic_CulturalTypeLookupRACAP> GetAllEthnicCulturalType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var ethnicCulturalType = db.apl_Ethnic_Cultural.Select(o => new Ethnic_CulturalTypeLookupRACAP
                {
                    Ethnic_Cultural_Id = o.Ethnic_Cultural_Id,
                    Description = o.Description
                }).ToList();

                return ethnicCulturalType;
            }
        }


        public List<RaceTypeLookupRACAP> GetAllRaceType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var raceType = db.Population_Groups.Select(o => new RaceTypeLookupRACAP
                {
                    Race_Id = o.Population_Group_Id,
                    Description = o.Description
                }).ToList();

                return raceType;
            }
        }

        public DateTime GetDate_Registered(int Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                
                return DateTime.Now.AddYears(3);
            }
        }

        public List<Body_StructureTypeLookupRACAP> GetAllBody_StructureType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var bodyStructurerType = db.apl_Body_Structure.Select(o => new Body_StructureTypeLookupRACAP
                {
                    Body_Structure_Id = o.Body_Structure_Id,
                    Description = o.Description
                }).ToList();

                return bodyStructurerType;
            }
        }

        //Dropdown_EyeColor_5
        public List<Eye_ColourTypeLookupRACAP> GetAllEye_ColourType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var eyeColourType = db.Eye_Colors.Select(o => new Eye_ColourTypeLookupRACAP
                {
                    Eye_Colour_Id = o.Eye_Color_Id,
                    Description = o.Description
                }).ToList();

                return eyeColourType;
            }
        }

        //Language
        public List<ParentLanguageTypeLookupRACAP> GetAllParentLanguageType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parentLanguageType = db.Languages.Select(o => new ParentLanguageTypeLookupRACAP
                {
                    Language_Id = o.Language_Id,
                    Description = o.Description
                }).ToList();

                return parentLanguageType;
            }
        }

        //Gender

        public List<ParentGenderTypeLookupRACAP> GetAllParentGenderType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parentGenderType = db.Genders.Select(o => new ParentGenderTypeLookupRACAP
                {
                    Gender_Id = o.Gender_Id,
                    Description = o.Description
                }).ToList();

                return parentGenderType;
            }
        }

        //PopulationGroup
        public List<ParentPopulationGroupTypeLookupRACAP> GetAllParentPopulationGroupType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parentPopulationGroupType = db.Population_Groups.Select(o => new ParentPopulationGroupTypeLookupRACAP
                {
                    Population_Group_Id = o.Population_Group_Id,
                    Description = o.Description
                }).ToList();

                return parentPopulationGroupType;
            }
        }

        //Race
        public List<ParentRaceTypeLookupRACAP> GetAllParentRaceType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parentRaceType = db.Races.Select(o => new ParentRaceTypeLookupRACAP
                {
                    Race_Id = o.Race_Id,
                    Description = o.Description
                }).ToList();

                return parentRaceType;
            }
        }

        //Religion
        public List<ParentReligionTypeLookupRACAP> GetAllParentReligionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parentReligionType = db.Religions.Select(o => new ParentReligionTypeLookupRACAP
                {
                    Religion_Id = o.Religion_Id,
                    Description = o.Description
                }).ToList();

                return parentReligionType;
            }
        }
        //Age from
        public int? Agefrom { get; set; }
        //Age To

        public int? AgeTo { get; set; }

        public List<NationalityTypeLookupRACAP> GetAllNationalityType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var nationalityType = db.Nationalities.Select(o => new NationalityTypeLookupRACAP
                {
                    Nationality_Id = o.Nationality_Id,
                    Description = o.Description
                }).ToList();

                return nationalityType;
            }
        }
        public List<DisabilityTypeLookupRACAP> GetAllDisabilityType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var disabilityType = db.Disabilities.Select(o => new DisabilityTypeLookupRACAP
                {
                    Disability_Type_Id = o.Disability_Id,
                    Description = o.Description
                }).ToList();

                return disabilityType;
            }
        }


        public List<ParentDisabilityTypeLookupRACAP> GetAllParentDisabilityType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var disabilityType = db.Disabilities.Select(o => new ParentDisabilityTypeLookupRACAP
                {
                    Disability_Id = o.Disability_Id,
                    Description = o.Description
                }).ToList();

                return disabilityType;
            }
        }


        public List<Cross_CulturalLookupRACAP> GetAllCross_CulturalType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var CrossCulturalType = db.apl_Cross_Cultural.Select(o => new Cross_CulturalLookupRACAP
                {
                    Cross_Cultural_Id = o.Cross_Cultural_Id,
                    Description = o.Description
                }).ToList();

                return CrossCulturalType;
            }
        }


        public List<Record_StatusLookupRACAP> GetAllRecord_StatuslType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var StatusType = db.apl_Record_Status.Select(o => new Record_StatusLookupRACAP
                {
                    Record_Status_Id = o.Record_Status_Id,
                    Description = o.Description
                }).ToList();

                return StatusType;
            }
        }


        public List<ReasonForAdoptingLookupAdopt> GetAllReasonForAdoptinglType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var AdoptingType = db.apl_Adopting_Reason.Select(o => new ReasonForAdoptingLookupAdopt
                {
                    Adopting_Reason_Id = o.Adopting_Reason_Id,
                    Description = o.Description
                }).ToList();

                return AdoptingType;
            }
        }
        public List<ProvinceLookupRACAP> GetAllProvinces()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Province_id = db.Provinces.Select(o => new ProvinceLookupRACAP
                {
                    Province_Id = o.Province_Id,
                    Description = o.Description
                }).ToList();

                return Province_id;
            }
        }


        public List<DistrictLookupRACAP> GetAllDistrict()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var District_id = db.Districts.Select(o => new DistrictLookupRACAP
                {
                    District_Id = o.District_Id,
                    Description = o.Description
                }).ToList();

                return District_id;


            }
        }



        public List<CourtLookupAdopt> GetAllCourt()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Court_id = db.Courts.Select(o => new CourtLookupAdopt
                {
                    Court_id = o.Court_Id,
                    Description = o.Description
                }).ToList();

                return Court_id;


            }
        }


        public List<OrganisationTypeLookupRACAPParent> GetAllParentOrganisationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Parent = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupRACAPParent
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();

                return Parent;
            }
        }

        public List<OrganisationTypeLookupSocialWorkerRACAP> GetAllSocialWorkerOrganisationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var SocialWorker = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupSocialWorkerRACAP
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();

                return SocialWorker;
            }
        }
     
        public List<OrganisationChildLookupRACAP> GetAllChildOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Child = db.Organizations.Select(o => new OrganisationChildLookupRACAP
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description
                }).ToList();

                return Child;
            }
        }


        public List<OrganisationLookupRACAPParent> GetAllParentOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parent = db.Organizations.Select(o => new OrganisationLookupRACAPParent
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description,
                    Telephone_Number = o.Telephone_Number,
                    Email_Address = o.Email_Address
                }).ToList();

                return parent;
            }
        }

        public List<OrganisationLookupSocialWorkerRACAP> GetAllSocialWorkerOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parent = db.Organizations.Select(o => new OrganisationLookupSocialWorkerRACAP
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description,
                    Telephone_Number = o.Telephone_Number,
                    Email_Address = o.Email_Address
                }).ToList();

                return parent;
            }
        }


        #endregion


        #region RACAP CASE INFROMATION

        public int? GetRACAPCaseDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.RACAP_Case_Id).FirstOrDefault();
            }
        }

        public void CreateRACAPCase(RACAPCaseViewModel cases, int id, int ids, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    RACAP_Case_Details newCase = new RACAP_Case_Details();

                    newCase.Intake_Assessment_Id = id;
                    //newCase.Client_Module_Id = cases.Client_Module_Id;
                    newCase.Intake_Assessment_Id = (from a in db.Intake_Assessments
                                                    where a.Intake_Assessment_Id == id
                                                    select a.Intake_Assessment_Id).FirstOrDefault();
                    db.RACAP_Case_Details.Add(newCase);
                    db.SaveChanges();
                    int? idnew = newCase.RACAP_Case_Id;
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



        public RACAPCaseViewModel GetRACAPCaseDetails(int assid)
        {
            RACAPCaseViewModel AdoptCasevm = new RACAPCaseViewModel();
            //SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            //var act1 = db.ADOPT_Case_Details.Find(Adopt_Case_Id);


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? CaseId = (from i in db.RACAP_Case_Details
                                   where i.Intake_Assessment_Id == assid
                                   select i.RACAP_Case_Id).FirstOrDefault();

                    RACAP_Case_Details act = db.RACAP_Case_Details.Find(CaseId);




                    //AdoptCasevm.Intake_Assessment_Id = Intake_Assessment_Id;
                    //AdoptCasevm.RACAP_Case_Id = CaseId;
                    //AdoptCasevm.OdoptionOrder_Date = act.OdoptionOrder_Date;
                    //AdoptCasevm.Date_Registered = act.Date_Registered;
                    //AdoptCasevm.Adoption_Surname = act.Adoption_Surname;
                    //AdoptCasevm.Comments = act.Comments;
                    //AdoptCasevm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
                    //AdoptCasevm.Legitimacy_Id = act.Legitimacy_Id;
                    //AdoptCasevm.Cross_Cultural_Id = act.Cross_Cultural_Id;
                    //AdoptCasevm.Record_Status_Id = act.Record_Status_Id;
                    //AdoptCasevm.Adopting_Reason_Id = act.Adopting_Reason_Id;
                    //AdoptCasevm.Court_id = act.Court_Id;
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

                return AdoptCasevm;
            }
        }



        //  Get Specific Adoption Case
        public RACAP_Case_Details GetSpecificAdoptionCase(int RACAP_Case_Id)
        {
            RACAP_Case_Details Adoptcase;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var AdoptcaseList = (from r in dbContext.RACAP_Case_Details
                                     where r.RACAP_Case_Id.Equals(RACAP_Case_Id)
                                     select r).ToList();

                Adoptcase = (from r in AdoptcaseList
                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }


            return Adoptcase;
        }


        //  Get Specific Adoption Case Intake assement
        public RACAP_Case_Details GetSpecificAdoptionCaseByIntakeAssment(int Intake_Assessment_Id)
        {
            RACAP_Case_Details AdoptcaseIntAss;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var AdoptcaseIntAssList = (from r in dbContext.RACAP_Case_Details
                                           where r.Intake_Assessment_Id.Equals(Intake_Assessment_Id)
                                           select r).ToList();

                AdoptcaseIntAss = (from r in AdoptcaseIntAssList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }


            return AdoptcaseIntAss;
        }

        #endregion FOR RACAP CASE INFROMATION

        #endregion Child

        //Amended by Herman
        #region Parent

        #region PERSON DETAILS

        // GET A PERSON BY PERSON ID

        public RACAPPersonViewModel GetRACAPPersonParent(int personId)
        {
            RACAPPersonViewModel Adoptvm = new RACAPPersonViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var act = db.Persons.Find(personId);
            Adoptvm.Person_Id = personId;
            Adoptvm.First_Name = act.First_Name;
            Adoptvm.Last_Name = act.Last_Name;
            Adoptvm.Known_As = act.Known_As;
            Adoptvm.Identification_Type_Id = act.Identification_Type_Id;
            Adoptvm.Identification_Number = act.Identification_Number;
            Adoptvm.Date_Of_Birth = act.Date_Of_Birth;

            DateTime old = Convert.ToDateTime(Adoptvm.Date_Of_Birth);

            int age = DateTime.Now.AddYears(-old.Year).Year;

            Adoptvm.Age = age;

            Adoptvm.Is_Estimated_Age = act.Is_Estimated_Age;
            Adoptvm.Language_Id = act.Language_Id;
            Adoptvm.Gender_Id = act.Gender_Id;
            Adoptvm.Marital_Status_Id = act.Marital_Status_Id;
            Adoptvm.Preferred_Contact_Type_Id = act.Preferred_Contact_Type_Id;
            Adoptvm.Religion_Id = act.Religion_Id;
            Adoptvm.Phone_Number = act.Phone_Number;
            Adoptvm.Mobile_Phone_Number = act.Mobile_Phone_Number;
            Adoptvm.Email_Address = act.Email_Address;
            Adoptvm.Population_Group_Id = act.Population_Group_Id;
            Adoptvm.Nationality_Id = act.Nationality_Id;
            Adoptvm.Disability_Type_Id = act.Disability_Type_Id;

            return Adoptvm;
        }

        //UPDATE PERSON DETAILS 


        #endregion FOR PESRON DETAILS


        #region GET RACAP CASE DETAILS FOR PARENT

        //public int GetRACAPParentCaseDetailsByssId(int intAssessmentId)
        //{
        //    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        //    {

        //        return (from p in db.Persons
        //                join c in db.Clients on p.Person_Id equals c.Person_Id
        //                join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
        //                join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
        //                where i.Intake_Assessment_Id.Equals(intAssessmentId)
        //                select Case.RACAP_Case_Id).FirstOrDefault();
        //    }
        //}

        public int GetRACAPParentCaseDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Intake_Assessments
                        join q in db.RACAP_Case_Details on p.Intake_Assessment_Id equals q.Intake_Assessment_Id
                        where p.Intake_Assessment_Id.Equals(intAssessmentId)
                        select q.RACAP_Case_Id).FirstOrDefault();
            }
        }


        public RACAPCaseViewModel GetRACAPParentCaseDetails(int RacapcaseId)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int CaseId = (from i in db.RACAP_Case_Details
                                  where i.RACAP_Case_Id == RacapcaseId
                                  select i.RACAP_Case_Id).FirstOrDefault();

                    RACAP_Case_Details act = db.RACAP_Case_Details.Find(CaseId);

                    vm.RACAP_Case_Id = CaseId;

                    vm.Expiry_Date = act.Expiry_Date;
                    vm.Date_Registered = act.Date_Registered;
                    vm.Comments = act.Comments;
                    vm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
                    vm.RACAP_Record_Status_Id = act.RACAP_Record_Status_Id;
                    vm.RACAP_Registration_Status_Id = act.RACAP_Registration_Status_Id;
                    vm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
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

        public RACAPCaseViewModel GetRACAPCaseDetailsCreateParent(int Case_Id)
        {
            RACAPCaseViewModel AdoptCasevm = new RACAPCaseViewModel();
            //SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            //var act1 = db.ADOPT_Case_Details.Find(Adopt_Case_Id);


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? CaseId = (from i in db.RACAP_Case_Details
                                   where i.RACAP_Case_Id == Case_Id
                                   select i.RACAP_Case_Id).FirstOrDefault();

                    RACAP_Case_Details act = db.RACAP_Case_Details.Find(CaseId);




                    //AdoptCasevm.Intake_Assessment_Id = Intake_Assessment_Id;
                    //AdoptCasevm.Adopt_Case_Id = CaseId;
                    //AdoptCasevm.OdoptionOrder_Date = act.OdoptionOrder_Date;
                    AdoptCasevm.Date_Registered = act.Date_Registered;
                    AdoptCasevm.Comments = act.Comments;
                    AdoptCasevm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
                    //AdoptCasevm.Legitimacy_Id = act.Legitimacy_Id;
                    //AdoptCasevm.Cross_Cultural_Id = act.Cross_Cultural_Id;
                    AdoptCasevm.RACAP_Record_Status_Id = act.RACAP_Record_Status_Id;
                    //AdoptCasevm.Adopting_Reason_Id = act.Adopting_Reason_Id;
                    //AdoptCasevm.Court_id = act.Court_Id;
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

                return AdoptCasevm;
            }
        }

        public int GetRACAPParentClientRefIdssId(int intAssessmentId)
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

        public string GetRACAPParentClientRef(int ClientRefid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join f in db.int_Client_Module_Registration on c.Client_Id equals f.Client_Id
                        where f.Client_Module_Id.Equals(ClientRefid)
                        select f.Client_Module_Ref_No).FirstOrDefault();
            }
        }

        #endregion


        #region RACAP PARENT FEATURES

        public int GetRACAPParentFeaturedByssId(int IntAss)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                var RacapCaseId = (from p in db.Persons
                                   join c in db.Clients on p.Person_Id equals c.Person_Id
                                   join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                                   join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                                   where i.Intake_Assessment_Id.Equals(IntAss)
                                   select Case.RACAP_Case_Id).FirstOrDefault();

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        join F in db.RACAP_Prospective_Parent on Case.RACAP_Case_Id equals F.RACAP_Case_Id
                        where Case.RACAP_Case_Id.Equals(RacapCaseId)
                        select F.RACAP_Prospective_Parent_Id).FirstOrDefault();
            }
        }

        public RACAPCaseViewModel GetRACAPParentFeaturedDetails(int ProspectiveParentId)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? Prospective_Parent_Id = (from i in db.RACAP_Case_Details
                                              join b in db.RACAP_Prospective_Parent on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                              where b.RACAP_Prospective_Parent_Id == ProspectiveParentId
                                                  select b.RACAP_Prospective_Parent_Id).FirstOrDefault();

                    RACAP_Prospective_Parent act = db.RACAP_Prospective_Parent.Find(Prospective_Parent_Id);

                    vm.Eye_Color_Id = act.Eye_Color_Id;
                    vm.Skin_Colour_Id = act.Skin_Color_Id;
                    vm.Body_Structure_Id = act.Body_Structure_Id;
                    vm.Ethnic_Cultural_Group_Id = act.Ethnic_Cultural_Group_Id;
                    vm.Special_Needs_Id = act.Special_Needs_Id;
                    vm.Religion_Id = act.Religion_Id;
                    vm.Language_Id = act.Language_Id;
                    vm.Gender_Id = act.Gender_Id;
                    vm.Race_Id = act.Race_Id;
                    vm.Population_Group_Id = act.Population_Group_Id;
                    vm.Disability_Id = act.Disability_Id;
                    vm.Age_From = act.Age_From;
                    vm.Age_To = act.Age_To;

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

        public void CreateRACAPParentFeature(RACAPCaseViewModel cases, int id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    RACAP_Adoptive_Child newCase = new RACAP_Adoptive_Child();

                    newCase.RACAP_Case_Id = (from a in db.Intake_Assessments
                                             join b in db.RACAP_Case_Details on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             where a.Intake_Assessment_Id == id
                                             select b.RACAP_Case_Id).FirstOrDefault();
                    db.RACAP_Adoptive_Child.Add(newCase);
                    db.SaveChanges();
                    int idnew = newCase.RACAP_Adoptive_Child_Id;
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

        public RACAPCaseViewModel GetRACAPParentFeatureDetailsCreate(int ProspectiveParentid)
        {
            RACAPCaseViewModel AdoptCasevm = new RACAPCaseViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? parentid = (from i in db.RACAP_Case_Details
                                    join b in db.RACAP_Prospective_Parent on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                    where b.RACAP_Prospective_Parent_Id == ProspectiveParentid
                                     select b.RACAP_Prospective_Parent_Id).FirstOrDefault();

                    RACAP_Prospective_Parent act = db.RACAP_Prospective_Parent.Find(parentid);

                    if (AdoptCasevm.Eye_Color_Id != null)
                    {

                        AdoptCasevm.Eye_Color_Id = act.Eye_Color_Id;
                    }
                    if (AdoptCasevm.Special_Needs_Id != null)
                    {
                        AdoptCasevm.Special_Needs_Id = act.Special_Needs_Id;
                    }
                    if (AdoptCasevm.Body_Structure_Id != null)
                    {


                        AdoptCasevm.Body_Structure_Id = act.Body_Structure_Id;
                    }
                    if (AdoptCasevm.Ethnic_Cultural_Group_Id != null)
                    {


                        AdoptCasevm.Ethnic_Cultural_Group_Id = act.Ethnic_Cultural_Group_Id;
                    }
                    if (AdoptCasevm.Body_Structure_Id != null)
                    {


                        AdoptCasevm.Body_Structure_Id = act.Body_Structure_Id;
                    }
                    if (AdoptCasevm.Skin_Colour_Id != null)
                    {


                        AdoptCasevm.Skin_Colour_Id = act.Skin_Color_Id;
                    }
                    if (AdoptCasevm.Religion_Id != null)
                    {


                        AdoptCasevm.Religion_Id = act.Religion_Id;
                    }
                    if (AdoptCasevm.Language_Id != null)
                    {


                        AdoptCasevm.Language_Id = act.Language_Id;
                    }
                    if (AdoptCasevm.Gender_Id != null)
                    {


                        AdoptCasevm.Gender_Id = act.Gender_Id;
                    }
                    if (AdoptCasevm.Race_Id != null)
                    {


                        AdoptCasevm.Race_Id = act.Race_Id;
                    }
                    if (AdoptCasevm.Age_From != null)
                    {


                        AdoptCasevm.Age_From = act.Age_From;
                    }
                    if (AdoptCasevm.Age_To != null)
                    {


                        AdoptCasevm.Age_To = act.Age_To;
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

                return AdoptCasevm;
            }
        }

        public void UpdateRACAPParentFeatures(MainPageModelRACAPVM cases, int userId, int Assid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int ProspectiveParent1 = (from i in db.RACAP_Prospective_Parent
                                            join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                            join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                            where a.Intake_Assessment_Id == Assid
                                            select i.RACAP_Prospective_Parent_Id).FirstOrDefault();
                    int RACAP_Case_Id= (from j in db.RACAP_Case_Details
                                       where j.Intake_Assessment_Id== Assid
                                       select j.RACAP_Case_Id).FirstOrDefault();

                    //RACAP_Adoptive_Child editCase = db.RACAP_Adoptive_Child.Find(AdoptiveChildId1);
                    RACAP_Prospective_Parent editCase = db.RACAP_Prospective_Parent.Find(ProspectiveParent1);
                    if (ProspectiveParent1 == 0)
                    {
                        RACAP_Prospective_Parent newCase = new RACAP_Prospective_Parent();
                        newCase.Intake_Assessment_Id = Assid;
                        newCase.RACAP_Case_Id = RACAP_Case_Id;
                        newCase.Created_By = userId;
                        newCase.Date_Created = DateTime.Now;
                        if (cases.RACAPCaseViewModel.Eye_Color_Id != null)
                        {
                            newCase.Eye_Color_Id = cases.RACAPCaseViewModel.Eye_Color_Id;
                        }

                        if (cases.RACAPCaseViewModel.Skin_Colour_Id != null)
                        {
                            newCase.Skin_Color_Id = cases.RACAPCaseViewModel.Skin_Colour_Id;
                        }

                        if (cases.RACAPCaseViewModel.Special_Needs_Id != null)
                        {
                            newCase.Special_Needs_Id = cases.RACAPCaseViewModel.Special_Needs_Id;
                        }

                        if (cases.RACAPCaseViewModel.Religion_Id != null)
                        {
                            newCase.Religion_Id = cases.RACAPCaseViewModel.Religion_Id;
                        }

                        if (cases.RACAPCaseViewModel.Language_Id != null)
                        {
                            newCase.Language_Id = cases.RACAPCaseViewModel.Language_Id;
                        }

                        if (cases.RACAPCaseViewModel.Gender_Id != null)
                        {
                            newCase.Gender_Id = cases.RACAPCaseViewModel.Gender_Id;
                        }
                        if (cases.RACAPCaseViewModel.Race_Id != null)
                        {
                            newCase.Race_Id = cases.RACAPCaseViewModel.Race_Id;
                        }

                        if (cases.RACAPCaseViewModel.Body_Structure_Id != null)
                        {
                            newCase.Body_Structure_Id = cases.RACAPCaseViewModel.Body_Structure_Id;
                        }

                        if (cases.RACAPCaseViewModel.Ethnic_Cultural_Group_Id != null)
                        {

                            newCase.Ethnic_Cultural_Group_Id = cases.RACAPCaseViewModel.Ethnic_Cultural_Group_Id;
                        }
                        if (cases.RACAPCaseViewModel.Population_Group_Id != null)
                        {

                            newCase.Population_Group_Id = cases.RACAPCaseViewModel.Population_Group_Id;
                        }
                        if (cases.RACAPCaseViewModel.Disability_Id != null)
                        {

                            newCase.Disability_Id = cases.RACAPCaseViewModel.Disability_Id;
                        }
                        if (cases.RACAPCaseViewModel.Age_From != null)
                        {

                            newCase.Age_From = cases.RACAPCaseViewModel.Age_From;
                        }
                        if (cases.RACAPCaseViewModel.Age_To != null)
                        {

                            newCase.Age_To = cases.RACAPCaseViewModel.Age_To;
                        }
                        db.RACAP_Prospective_Parent.Add(newCase);
                        db.SaveChanges();
                    }
                    if (ProspectiveParent1!=0) {
                        editCase.RACAP_Prospective_Parent_Id = ProspectiveParent1;

                        editCase.Modified_By = userId;
                        editCase.Date_Modified = DateTime.Now;
                        if (cases.RACAPCaseViewModel.Eye_Color_Id != null)
                        {
                            editCase.Eye_Color_Id = cases.RACAPCaseViewModel.Eye_Color_Id;
                        }

                        if (cases.RACAPCaseViewModel.Skin_Colour_Id != null)
                        {
                            editCase.Skin_Color_Id = cases.RACAPCaseViewModel.Skin_Colour_Id;
                        }

                        if (cases.RACAPCaseViewModel.Special_Needs_Id != null)
                        {
                            editCase.Special_Needs_Id = cases.RACAPCaseViewModel.Special_Needs_Id;
                        }

                        if (cases.RACAPCaseViewModel.Religion_Id != null)
                        {
                            editCase.Religion_Id = cases.RACAPCaseViewModel.Religion_Id;
                        }

                        if (cases.RACAPCaseViewModel.Language_Id != null)
                        {
                            editCase.Language_Id = cases.RACAPCaseViewModel.Language_Id;
                        }

                        if (cases.RACAPCaseViewModel.Gender_Id != null)
                        {
                            editCase.Gender_Id = cases.RACAPCaseViewModel.Gender_Id;
                        }
                        if (cases.RACAPCaseViewModel.Race_Id != null)
                        {
                            editCase.Race_Id = cases.RACAPCaseViewModel.Race_Id;
                        }

                        if (cases.RACAPCaseViewModel.Body_Structure_Id != null)
                        {
                            editCase.Body_Structure_Id = cases.RACAPCaseViewModel.Body_Structure_Id;
                        }

                        if (cases.RACAPCaseViewModel.Ethnic_Cultural_Group_Id != null)
                        {

                            editCase.Ethnic_Cultural_Group_Id = cases.RACAPCaseViewModel.Ethnic_Cultural_Group_Id;
                        }
                        if (cases.RACAPCaseViewModel.Population_Group_Id != null)
                        {

                            editCase.Population_Group_Id = cases.RACAPCaseViewModel.Population_Group_Id;
                        }
                        if (cases.RACAPCaseViewModel.Disability_Id != null)
                        {

                            editCase.Disability_Id = cases.RACAPCaseViewModel.Disability_Id;
                        }
                        if (cases.RACAPCaseViewModel.Age_From != null)
                        {

                            editCase.Age_From = cases.RACAPCaseViewModel.Age_From;
                        }
                        if (cases.RACAPCaseViewModel.Age_To != null)
                        {

                            editCase.Age_To = cases.RACAPCaseViewModel.Age_To;
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
        }

        #endregion

        #region GET ORGANISATION REPSONSIBLE FOR RACAP


        public int GetRACAPParentOrganisationByssId(int IntAss)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                var RacapCaseId = (from p in db.Persons
                                   join c in db.Clients on p.Person_Id equals c.Person_Id
                                   join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                                   join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                                   where i.Intake_Assessment_Id.Equals(IntAss)
                                   select Case.RACAP_Case_Id).FirstOrDefault();

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        join F in db.RACAP_OrgansationResponsible on Case.RACAP_Case_Id equals F.RACAP_Case_Id
                        where Case.RACAP_Case_Id.Equals(RacapCaseId)
                        select F.RACAP_Responsible_Organisation).FirstOrDefault();
            }
        }

        public RACAPCaseViewModel GetRACAPParentOrganisationDetails(int OrganizationIdChild)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? OrganizationChild = (from i in db.RACAP_Case_Details
                                              join b in db.RACAP_OrgansationResponsible on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                              where b.RACAP_Responsible_Organisation == OrganizationIdChild
                                              select b.RACAP_Responsible_Organisation).FirstOrDefault();

                    RACAP_OrgansationResponsible act = db.RACAP_OrgansationResponsible.Find(OrganizationChild);
                    if (act.Organization_Id_Parent!= null)
                    {
                        vm.Organization_Id = act.Organization_Id_Parent;
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


        public void CreateRACAPParentOrganisation(RACAPCaseViewModel cases, int id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    RACAP_OrgansationResponsible newCase = new RACAP_OrgansationResponsible();

                    newCase.RACAP_Case_Id = (from a in db.Intake_Assessments
                                             join b in db.RACAP_Case_Details on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             where a.Intake_Assessment_Id == id
                                             select b.RACAP_Case_Id).FirstOrDefault();
                    db.RACAP_OrgansationResponsible.Add(newCase);
                    db.SaveChanges();
                    int idnew = newCase.RACAP_Responsible_Organisation;
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

        public RACAPCaseViewModel GetRACAPParentOrganisationDetailsCreate(int ResponsibleOrganisationid)
        {
            RACAPCaseViewModel AdoptCasevm = new RACAPCaseViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? ResponsibleOrganisation = (from i in db.RACAP_Case_Details
                                                    join b in db.RACAP_OrgansationResponsible on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                                    where b.RACAP_Responsible_Organisation == ResponsibleOrganisationid
                                                    select b.RACAP_Responsible_Organisation).FirstOrDefault();

                    RACAP_OrgansationResponsible act = db.RACAP_OrgansationResponsible.Find(ResponsibleOrganisation);


                    if (act.Organization_Id_Child != null)
                    {
                        AdoptCasevm.Organization_Id = act.Organization_Id_Child;
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

                return AdoptCasevm;
            }
        }

        public void UpdateRACAPParentOrganisation(MainPageModelRACAPVM cases, int userId, int Assid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int ResponsibleOrganisation = (from i in db.RACAP_OrgansationResponsible
                                                   join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                                   join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                                   where a.Intake_Assessment_Id == Assid
                                                   select i.RACAP_Responsible_Organisation).FirstOrDefault();
                    if (ResponsibleOrganisation == 0)
                    {
                        RACAP_OrgansationResponsible newCase = new RACAP_OrgansationResponsible();
                        newCase.Organization_Id_Parent = cases.RACAPCaseViewModel.Organization_Id;
                        newCase.Intake_Assessment_Id = Assid;
                        db.RACAP_OrgansationResponsible.Add(newCase);
                        db.SaveChanges();   

                    }
                    if (ResponsibleOrganisation != 0) { 

                    RACAP_OrgansationResponsible editCase = db.RACAP_OrgansationResponsible.Find(ResponsibleOrganisation);

                    editCase.RACAP_Responsible_Organisation = ResponsibleOrganisation;


                    if (cases.RACAPCaseViewModel.Organization_Id != null)
                    {
                        editCase.Organization_Id_Parent = cases.RACAPCaseViewModel.Organization_Id;
                            editCase.Intake_Assessment_Id = Assid;
                    }

                    db.SaveChanges();
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
            }
        }


        #endregion

        public void CreateRACAPParentCase(RACAPCaseViewModel cases, int id, int clientref, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    RACAP_Case_Details newCase = new RACAP_Case_Details();

                    newCase.Intake_Assessment_Id = id;
                    //newCase.Client_Module_Id = cases.Client_Module_Id;
                    newCase.Intake_Assessment_Id = (from a in db.Intake_Assessments
                                                    where a.Intake_Assessment_Id == id
                                                    select a.Intake_Assessment_Id).FirstOrDefault();
                    db.RACAP_Case_Details.Add(newCase);
                    db.SaveChanges();
                    int idnew = newCase.RACAP_Case_Id;
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


        #region RACAP PARENT REMOVAL
        public int GetRACAPParentRemovalByssId(int IntAss)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                var RacapCaseId = (from p in db.Persons
                                   join c in db.Clients on p.Person_Id equals c.Person_Id
                                   join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                                   join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                                   where i.Intake_Assessment_Id.Equals(IntAss)
                                   select Case.RACAP_Case_Id).FirstOrDefault();

                return (from Case in db.RACAP_Case_Details
                        where Case.RACAP_Case_Id.Equals(RacapCaseId)
                        select Case.RACAP_Case_Id).FirstOrDefault();
            }
        }

        public RACAPCaseViewModel GetRACAPParentRemovalDetails(int Caseid)
        {
            RACAPCaseViewModel vm = new RACAPCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? removeid = (from i in db.RACAP_Case_Details
                                     join b in db.RACAP_Removal on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                     where i.RACAP_Case_Id == Caseid
                                     select b.Removal_Id).FirstOrDefault();
                    if (removeid > 0)
                    {

                        RACAP_Removal act = db.RACAP_Removal.Find(removeid);

                        vm.Removal_Type_Id = act.Removal_Type_Id;
                        vm.Removal_Comments = act.Removal_Comments;
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

        public void UpdateRACAPParentRemoval(MainPageModelRACAPVM cases, int userId, int Assid)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int RACAP_Case_Id1 = (from i in db.RACAP_Prospective_Parent
                                          join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                          join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                          where a.Intake_Assessment_Id == Assid
                                          select b.RACAP_Case_Id).FirstOrDefault();

                    int RACAP_Case_Id1r = (from i in db.RACAP_Prospective_Parent
                                           join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                           join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                           join r in db.RACAP_Removal on b.RACAP_Case_Id equals r.RACAP_Case_Id
                                           where r.RACAP_Case_Id == RACAP_Case_Id1
                                           select b.RACAP_Case_Id).FirstOrDefault();
                    if (RACAP_Case_Id1r <= 0)
                    {
                        RACAP_Removal newremove = new RACAP_Removal();
                        newremove.RACAP_Case_Id = RACAP_Case_Id1;

                        newremove.Date_Removed = DateTime.Now;
                        db.RACAP_Removal.Add(newremove);
                        db.SaveChanges();
                    }


                    int RACAP_Case_Id1r2 = (from i in db.RACAP_Prospective_Parent
                                            join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                            join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                            join r in db.RACAP_Removal on b.RACAP_Case_Id equals r.RACAP_Case_Id
                                            where r.RACAP_Case_Id == RACAP_Case_Id1
                                            select b.RACAP_Case_Id).FirstOrDefault();

                    //Update.....................................................................
                    if (RACAP_Case_Id1r2 >= 0)
                    {
                        int Removalid = (from i in db.RACAP_Prospective_Parent
                                         join b in db.RACAP_Case_Details on i.RACAP_Case_Id equals b.RACAP_Case_Id
                                         join a in db.Intake_Assessments on b.Intake_Assessment_Id equals a.Intake_Assessment_Id
                                         join r in db.RACAP_Removal on b.RACAP_Case_Id equals r.RACAP_Case_Id
                                         where r.RACAP_Case_Id == RACAP_Case_Id1r2
                                         select r.Removal_Id).FirstOrDefault();

                        RACAP_Removal edit = db.RACAP_Removal.Find(Removalid);

                        edit.RACAP_Case_Id = RACAP_Case_Id1r2;

                        if (cases.RACAPCaseViewModel.Removal_Type_Id != null)
                        {
                            edit.Removal_Type_Id = cases.RACAPCaseViewModel.Removal_Type_Id;
                        }
                        if (cases.RACAPCaseViewModel.Removal_Comments != null)
                        {
                            edit.Removal_Comments = cases.RACAPCaseViewModel.Removal_Comments;
                            edit.Date_Removed = DateTime.Now;
                        }
                        db.SaveChanges();
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
            }
        }

        #endregion

        #region ParentPopup

        public List<RACAPPersonViewModel> GetListOfParents()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                List<Person> NewObj = new List<Person>();

                var ListAsFromDb = (from a in db.Persons
                                    join c in db.Clients on a.Person_Id equals c.Person_Id
                                    join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                                    join Case in db.RACAP_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id

                                    where i.Intake_Assessment_Id== 28387
                                    select new
                                    {
                                        a.Person_Id,
                                        a.First_Name,
                                        a.Last_Name,
                                        a.Identification_Number
                                    }).ToList();
                List<RACAPPersonViewModel> RPVM = new List<RACAPPersonViewModel>();

                foreach (var item in ListAsFromDb)
                {
                    RACAPPersonViewModel Obj = new RACAPPersonViewModel();
                    Obj.Person_Id = item.Person_Id;
                    Obj.First_Name = item.First_Name;
                    Obj.Last_Name = item.Last_Name;
                    Obj.Identification_Number = item.Identification_Number;
                    RPVM.Add(Obj);
                }
                return RPVM;
             }
        }

        #endregion


        #endregion Parent
        //End Amended by Herman
    }

}
