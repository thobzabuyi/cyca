using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Word;


namespace Common_Objects.Models
{
    public class AdoptionCaseDetailsModel
    {
        #region Documents

        public void CreateAdoptDocuments(AdoptionSupportingDocumentsVM docAttached)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())

            {
                try
                {
                    AdoptionSupportingDocument newdocAttached = new AdoptionSupportingDocument();
                    newdocAttached.Intake_Assessment_Id = docAttached.Intake_Assessment_Id;
                    newdocAttached.DocumentsFileName = docAttached.DocumentsFileName;
                    newdocAttached.DocumentsDescription = docAttached.DocumentsDescription;
                    newdocAttached.DocumentsLabel = 1;
                    newdocAttached.DocumentsPath = "~/App_Data/AdoptionSupportingDocuments/";
                    newdocAttached.DocumentsCheck = docAttached.DocumentsCheck;
                    db.AdoptionSupportingDocuments.Add(newdocAttached);
                    db.SaveChanges();
                }
                catch
                {

                }

            }

        }

        public AdoptionSupportingDocumentsVM GetAdoptSupDocuments(int Id)
        {
            AdoptionSupportingDocumentsVM ASDvm = new AdoptionSupportingDocumentsVM();
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

        public List<AdoptionSupportingDocumentsVM> GetAdoptionSupportDocumentsList(int intake_Assessment_Id)
        {
            List<AdoptionSupportingDocumentsVM> avm = new List<AdoptionSupportingDocumentsVM>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var documentsList =
                (from doc in db.AdoptionSupportingDocuments
                 where (doc.Intake_Assessment_Id == intake_Assessment_Id)
                 select new
                 {
                     doc.Intake_Assessment_Id,
                     doc.DocumentsId,
                     doc.DocumentsFileName,
                     doc.DocumentsDescription,
                     doc.DocumentsLabel,
                     doc.DocumentsPath,
                     doc.DocumentsCheck


                 }).ToList();
            foreach (var item in documentsList)
            {
                AdoptionSupportingDocumentsVM objadptDcmntsvm = new AdoptionSupportingDocumentsVM();
                objadptDcmntsvm.DocumentsId = item.DocumentsId;
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
        public int GetAdoptPersonIdByintAssId(int intAssessmentId)
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
        public AdoptionPersonViewModel GetAdoptionPerson(int personId)
        {
            AdoptionPersonViewModel Adoptvm = new AdoptionPersonViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var act = db.Persons.Find(personId);
            Adoptvm.Person_Id = personId;
            Adoptvm.First_Name = act.First_Name;
            Adoptvm.Last_Name = act.Last_Name;
            Adoptvm.Known_As = act.Known_As;
            Adoptvm.Identification_Type_Id = act.Identification_Type_Id;
            Adoptvm.Identification_Number = act.Identification_Number;
            Adoptvm.Date_Of_Birth = act.Date_Of_Birth;
            Adoptvm.Age = act.Age;
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

        public void UpdatePersonalDetails(MainPageModelVM person, string myStringuserId, int personId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    Person newPerson = db.Persons.Find(personId);

                    newPerson.First_Name = person.AdoptionPersonViewModel.First_Name;
                    newPerson.Last_Name = person.AdoptionPersonViewModel.Last_Name;
                    newPerson.Known_As = person.AdoptionPersonViewModel.Known_As;
                    newPerson.Identification_Type_Id = person.AdoptionPersonViewModel.Identification_Type_Id;
                    newPerson.Identification_Number = person.AdoptionPersonViewModel.Identification_Number;
                    newPerson.Date_Of_Birth = person.AdoptionPersonViewModel.Date_Of_Birth;
                    newPerson.Age = person.AdoptionPersonViewModel.Age;
                    newPerson.Language_Id = person.AdoptionPersonViewModel.Language_Id;
                    newPerson.Gender_Id = person.AdoptionPersonViewModel.Gender_Id;
                    newPerson.Marital_Status_Id = person.AdoptionPersonViewModel.Marital_Status_Id;
                    newPerson.Nationality_Id = person.AdoptionPersonViewModel.Nationality_Id;
                    newPerson.Population_Group_Id = person.AdoptionPersonViewModel.Population_Group_Id;
                    newPerson.Disability_Type_Id = person.AdoptionPersonViewModel.Disability_Type_Id;
                    newPerson.Modified_By = myStringuserId;
                    newPerson.Date_Last_Modified = DateTime.Now;
                    db.SaveChanges();
                }
                catch
                {

                }

            }

        }
        public void UpdateAdoptionPerson(AdoptionPersonViewModel person, string userName)
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

        #region SEARCH ADD AND UPDATE ADOPTION CASE

        //GET ADOPTION CASE BY ADOPTION CASE ID

        //ADD ADOPTION CASE DETAILS
        #region GET ADOPTION CASE DETAILS

        public int GetAdoptCaseDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.ADOPT_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        where i.Intake_Assessment_Id.Equals(intAssessmentId)
                        select Case.Adopt_Case_Id).FirstOrDefault();
            }
        }



        public AdoptionCaseViewModel GetAdoptionCaseDetails(int assid)
        {
            AdoptionCaseViewModel AdoptCasevm = new AdoptionCaseViewModel();


            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? CaseId = (from i in db.ADOPT_Case_Details
                                   where i.Intake_Assessment_Id == assid
                                   select i.Adopt_Case_Id).FirstOrDefault();

                    ADOPT_Case_Details act = db.ADOPT_Case_Details.Find(CaseId);

                    AdoptCasevm.Adopt_Case_Id = CaseId;
                    AdoptCasevm.OdoptionOrder_Date = act.OdoptionOrder_Date;
                    AdoptCasevm.Date_Registered = act.Date_Registered;
                    AdoptCasevm.Adoption_Surname = act.Adoption_Surname;
                    AdoptCasevm.Comments = act.Comments;
                    AdoptCasevm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
                    AdoptCasevm.Legitimacy_Id = act.Legitimacy_Id;
                    AdoptCasevm.Cross_Cultural_Id = act.Cross_Cultural_Id;
                    //...New Code ........................
                    AdoptCasevm.NonRelation_Realtion_Id = act.NonRelation_Realtion_Id;
                    AdoptCasevm.Adopt_Type_Id = act.Adopt_Type_Id;
                    AdoptCasevm.Relationship_Type_Id = act.Relationship_Type_Id;
                    AdoptCasevm.CourtReffence = act.Court_Reffence;
                    AdoptCasevm.Record_Status_Id = act.Record_Status_Id;
                    AdoptCasevm.Adopting_Reason_Id = act.Adopting_Reason_Id;
                    AdoptCasevm.Court_id = act.Court_Id;
                    AdoptCasevm.Date_Finalised = act.Date_Finalised;
                    AdoptCasevm.Adoption_Category_Id = act.Adoption_Category_Id;
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

        public AdoptionCaseViewModel GetAdoptionCaseDetailsCreate(int Adopt_Case_Id)
        {
            AdoptionCaseViewModel AdoptCasevm = new AdoptionCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? CaseId = (from i in db.ADOPT_Case_Details
                                   where i.Adopt_Case_Id == Adopt_Case_Id
                                   select i.Adopt_Case_Id).FirstOrDefault();

                    ADOPT_Case_Details act = db.ADOPT_Case_Details.Find(CaseId);

                    AdoptCasevm.Adopt_Case_Id = CaseId;
                    AdoptCasevm.OdoptionOrder_Date = act.OdoptionOrder_Date;
                    AdoptCasevm.Date_Registered = act.Date_Registered;
                    AdoptCasevm.Adoption_Surname = act.Adoption_Surname;
                    AdoptCasevm.Comments = act.Comments;
                    AdoptCasevm.Adoptions_Reason_Id = act.Adoptions_Reason_Id;
                    AdoptCasevm.Legitimacy_Id = act.Legitimacy_Id;
                    AdoptCasevm.Cross_Cultural_Id = act.Cross_Cultural_Id;
                    AdoptCasevm.Record_Status_Id = act.Record_Status_Id;
                    AdoptCasevm.Adopting_Reason_Id = act.Adopting_Reason_Id;
                    AdoptCasevm.Court_id = act.Court_Id;
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



        public void CreateADOPTIONCase(AdoptionCaseViewModel cases, int id, int clientref, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    ADOPT_Case_Details newCase = new ADOPT_Case_Details();

                    newCase.Intake_Assessment_Id = id;
                    //newCase.Client_Module_Id = cases.Client_Module_Id;
                    newCase.Intake_Assessment_Id = (from a in db.Intake_Assessments
                                                    where a.Intake_Assessment_Id == id
                                                    select a.Intake_Assessment_Id).FirstOrDefault();
                    newCase.Client_Module_Id = clientref;
                    db.ADOPT_Case_Details.Add(newCase);

                    db.SaveChanges();
                    int idnew = newCase.Adopt_Case_Id;
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

        //UPDATE CASE DETAILS

        public void UpdateADOPTIONCase(MainPageModelVM cases, int userId, int AssId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var editCase1 = (from i in db.ADOPT_Case_Details
                                     where i.Intake_Assessment_Id == (AssId)
                                     select i.Adopt_Case_Id).FirstOrDefault();

                    ADOPT_Case_Details editCase = db.ADOPT_Case_Details.Find(editCase1);


                    editCase.Intake_Assessment_Id = AssId;
                    //editCase.Client_Module_Id = cases.AdoptionCaseViewModel.Client_Module_Id;

                    if (cases.AdoptionCaseViewModel.Court_id != null)
                    {

                        editCase.Court_Id = cases.AdoptionCaseViewModel.Court_id;
                    }
                    if (cases.AdoptionCaseViewModel.OdoptionOrder_Date != null)
                    {
                        editCase.OdoptionOrder_Date = cases.AdoptionCaseViewModel.OdoptionOrder_Date;
                        editCase.Created_By = userId;
                        editCase.Date_Modified = DateTime.Now;
                        editCase.Modified_By = userId;
                        editCase.Date_Created = DateTime.Now;
                    }

                    if (cases.AdoptionCaseViewModel.Date_Registered != null)
                    {
                        editCase.Date_Registered = cases.AdoptionCaseViewModel.Date_Registered;
                    }

                    if (cases.AdoptionCaseViewModel.Adoptions_Reason_Id != null)
                    {
                        editCase.Adoptions_Reason_Id = cases.AdoptionCaseViewModel.Adoptions_Reason_Id;
                    }

                    if (cases.AdoptionCaseViewModel.Adopting_Reason_Id != null)
                    {
                        editCase.Adopting_Reason_Id = cases.AdoptionCaseViewModel.Adopting_Reason_Id;
                    }
                    if (cases.AdoptionCaseViewModel.Adoption_Surname != null)
                    {

                        editCase.Adoption_Surname = cases.AdoptionCaseViewModel.Adoption_Surname;
                    }
                    if (cases.AdoptionCaseViewModel.Cross_Cultural_Id != null)
                    {

                        editCase.Cross_Cultural_Id = cases.AdoptionCaseViewModel.Cross_Cultural_Id;
                    }
                    if (cases.AdoptionCaseViewModel.Record_Status_Id != null)
                    {

                        editCase.Record_Status_Id = cases.AdoptionCaseViewModel.Record_Status_Id;
                    }

                    if (cases.AdoptionCaseViewModel.Comments != null)
                    {

                        editCase.Comments = cases.AdoptionCaseViewModel.Comments;
                    }
                    //............New Code.................................................................

                    if (cases.AdoptionCaseViewModel.CourtReffence != null)
                    {

                        editCase.Court_Reffence = cases.AdoptionCaseViewModel.CourtReffence;
                    }
                    if (cases.AdoptionCaseViewModel.Relationship_Type_Id != null)
                    {

                        editCase.Relationship_Type_Id = cases.AdoptionCaseViewModel.Relationship_Type_Id;
                    }
                    if (cases.AdoptionCaseViewModel.NonRelation_Realtion_Id != null)
                    {

                        editCase.NonRelation_Realtion_Id = cases.AdoptionCaseViewModel.NonRelation_Realtion_Id;
                    }
                    if (cases.AdoptionCaseViewModel.Adopt_Type_Id != null)
                    {

                        editCase.Adopt_Type_Id = cases.AdoptionCaseViewModel.Adopt_Type_Id;
                    }
                    if (cases.AdoptionCaseViewModel.Adoption_Category_Id != null)
                    {

                        editCase.Adoption_Category_Id = cases.AdoptionCaseViewModel.Adoption_Category_Id;
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


        public void CreateADOPTIONClientId(AdoptionCaseViewModel cases, int id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {

                    //ADOPT_Case_Details newCase = new ADOPT_Case_Details();
                    int_Client_Module_Registration newref = new int_Client_Module_Registration();


                    int AssessmentId = (from a in db.Intake_Assessments
                                        where a.Intake_Assessment_Id == id
                                        select a.Intake_Assessment_Id).FirstOrDefault();

                    newref.Client_Module_Id = (from a in db.Clients
                                               join b in db.Intake_Assessments on
                                               a.Client_Id equals b.Client_Id
                                               where b.Intake_Assessment_Id == AssessmentId
                                               select a.Client_Id).FirstOrDefault();

                    db.int_Client_Module_Registration.Add(newref);

                    db.SaveChanges();
                    int idRefidnew = newref.Client_Module_Id;
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

        public int GetClientIdByAssmentId(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {


                return (from k in db.Intake_Assessments
                        join k1 in db.Clients on k.Client_Id equals k1.Client_Id
                        where k.Intake_Assessment_Id == id
                        select k1.Client_Id).FirstOrDefault();


            }
        }


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
                        where f.Client_Module_Id.Equals(ClientRefid)
                        select f.Client_Module_Ref_No).FirstOrDefault();
            }
        }

        #endregion

        // GET ORGANISATION REPSONSIBLE FOR ADOPTION

        #region GET ORGANISATION REPSONSIBLE FOR ADOPTION

        public int GetOrgnanisationtCaseDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.ADOPT_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        join Org in db.ADOPT_OrgansationResponsible on Case.Adopt_Case_Id equals Org.Adopt_Case_Id
                        orderby Org.Adopt_Responsible_Organisation ascending
                        where Org.Intake_Assessment_Id == intAssessmentId
                        select Org.Adopt_Responsible_Organisation).FirstOrDefault();
            }
        }

        public AdoptionCaseViewModel GetOrgnanisationtCaseDetails(int Adopt_Responsible_Organisation)
        {
            AdoptionCaseViewModel AdoptCasevm = new AdoptionCaseViewModel();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var act = db.ADOPT_OrgansationResponsible.Find(Adopt_Responsible_Organisation);
            var typeChild = (from a in db.apl_Organisation_Type
                             join a1 in db.Organizations on a.IdType equals a1.Organisation_Type_Id
                             join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Child
                             where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                             select a.IdType).FirstOrDefault();
            var typeParent = (from a in db.apl_Organisation_Type
                              join a1 in db.Organizations on a.IdType equals a1.Organisation_Type_Id
                              join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Parent
                              where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                              select a.IdType).FirstOrDefault();
            var typeSocialWorker = (from a in db.apl_Organisation_Type
                                    join a1 in db.Organizations on a.IdType equals a1.Organisation_Type_Id
                                    join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_SocialWorker
                                    where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                                    select a.IdType).FirstOrDefault();
            //Child Organisation........................................................................................................
            var ChildLocalMunId = (from a in db.Local_Municipalities
                                   join a1 in db.Organizations on a.Local_Municipality_Id equals a1.Local_Municipality_Id
                                   join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Child
                                   where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                                   select a.Local_Municipality_Id).FirstOrDefault();
            var ChildDistrictId = (from a in db.Local_Municipalities
                                   join a1 in db.Organizations on a.Local_Municipality_Id equals a1.Local_Municipality_Id
                                   join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Child
                                   join d in db.Districts on a.District_Municipality_Id equals d.District_Id
                                   join p in db.Provinces on d.Province_Id equals p.Province_Id
                                   where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                                   select d.District_Id).FirstOrDefault();
            var ChildProvcId = (from a in db.Local_Municipalities
                                join a1 in db.Organizations on a.Local_Municipality_Id equals a1.Local_Municipality_Id
                                join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Child
                                join d in db.Districts on a.District_Municipality_Id equals d.District_Id
                                join p in db.Provinces on d.Province_Id equals p.Province_Id
                                where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                                select p.Province_Id).FirstOrDefault();
            //Parent Organisation........................................................................................................
            var ParentLocalMunId = (from a in db.Local_Municipalities
                                    join a1 in db.Organizations on a.Local_Municipality_Id equals a1.Local_Municipality_Id
                                    join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Parent
                                    where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                                    select a.Local_Municipality_Id).FirstOrDefault();
            var ParentDistrictId = (from a in db.Local_Municipalities
                                    join a1 in db.Organizations on a.Local_Municipality_Id equals a1.Local_Municipality_Id
                                    join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Parent
                                    join d in db.Districts on a.District_Municipality_Id equals d.District_Id
                                    join p in db.Provinces on d.Province_Id equals p.Province_Id
                                    where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                                    select d.District_Id).FirstOrDefault();
            var ParentProvcId = (from a in db.Local_Municipalities
                                 join a1 in db.Organizations on a.Local_Municipality_Id equals a1.Local_Municipality_Id
                                 join a11 in db.ADOPT_OrgansationResponsible on a1.Organization_Id equals a11.Organization_Id_Parent
                                 join d in db.Districts on a.District_Municipality_Id equals d.District_Id
                                 join p in db.Provinces on d.Province_Id equals p.Province_Id
                                 where a11.Adopt_Responsible_Organisation == Adopt_Responsible_Organisation
                                 select p.Province_Id).FirstOrDefault();

            //AdoptCasevm.Intake_Assessment_Id = Intake_Assessment_Id;
            AdoptCasevm.Adopt_Responsible_Organisation = Adopt_Responsible_Organisation;
            if (act != null)
            {
                AdoptCasevm.Organization_Id_Child = act.Organization_Id_Child;

                AdoptCasevm.Organization_Id_Type_Child = typeChild;

                AdoptCasevm.Organization_Id_Parent = act.Organization_Id_Parent;
                AdoptCasevm.Organization_Id_Type_Parent = typeParent;
                AdoptCasevm.Organization_Id_SocialWorker = act.Organization_Id_SocialWorker;
                AdoptCasevm.Organization_Id_Type_Social_Worker = typeSocialWorker;
                AdoptCasevm.Child_Social_Worker_Id = act.Child_Social_Worker_Id;
                AdoptCasevm.Parent_Social_Worker_Id = act.Parent_Social_Worker_Id;
                AdoptCasevm.Local_Municipality_Id = ChildLocalMunId;
                AdoptCasevm.District_IdChilOrg = ChildDistrictId;
                AdoptCasevm.Province_IdChildOrg = ChildProvcId;

                AdoptCasevm.Local_Municipality_IdParent = ParentLocalMunId;
                AdoptCasevm.District_IdParentOrg = ParentDistrictId;
                AdoptCasevm.Province_IdParentOrg = ParentProvcId;


            }
            return AdoptCasevm;
        }




        #endregion

        //ADD ADOPTION CASE DETAILS
        #region ADD ADOPTION CASE DETAILS


        //Herman
        public void CreateAdoptionOrgnanisationResponsible(AdoptionCaseViewModel cases, int id, int userId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    ADOPT_OrgansationResponsible newCaseOrgRespon = new ADOPT_OrgansationResponsible();

                    newCaseOrgRespon.Intake_Assessment_Id = id;
                    //newCase.Client_Module_Id = cases.Client_Module_Id;
                    newCaseOrgRespon.Adopt_Case_Id = (from a in db.ADOPT_Case_Details
                                                      where a.Intake_Assessment_Id == id
                                                      select a.Adopt_Case_Id).FirstOrDefault();
                    //newCaseOrgRespon.Organization_Id_Child = cases.Organization_Id_Child;
                    //newCaseOrgRespon.Organization_Id_Parent = cases.Organization_Id_Parent;
                    db.ADOPT_OrgansationResponsible.Add(newCaseOrgRespon);
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
        #region UPDATE CASE DETAILS


        public void UpdateADOPTIONOrganisation(MainPageModelVM cases, int userId, int Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int AdoptORGResponId = (from d in db.ADOPT_OrgansationResponsible
                                            where d.Intake_Assessment_Id == Id
                                            select d.Adopt_Responsible_Organisation).FirstOrDefault();
                    ADOPT_OrgansationResponsible editCase = db.ADOPT_OrgansationResponsible.Find(AdoptORGResponId);
                    //if()
                    editCase.Intake_Assessment_Id = Id;
                    editCase.Adopt_Case_Id = editCase.Adopt_Case_Id;
                    editCase.Organization_Id_Child = cases.AdoptionCaseViewModel.Organization_Id_Child;

                    editCase.Organization_Id_Parent = cases.AdoptionCaseViewModel.Organization_Id_Parent;
                    editCase.Organization_Id_SocialWorker = cases.AdoptionCaseViewModel.Organization_Id_SocialWorker;
                    editCase.Child_Social_Worker_Id = cases.AdoptionCaseViewModel.Child_Social_Worker_Id;
                    editCase.Parent_Social_Worker_Id = cases.AdoptionCaseViewModel.Parent_Social_Worker_Id;
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

        // GET ADOPTIVE PARENT DETAILS

        public int GetAdoptiveParentIdByintAssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (


                    from p in db.Client_Adoptive_Parents
                    join c in db.Clients on p.Client_Id equals c.Client_Id
                    join i in db.Persons on p.Person_Id equals i.Person_Id
                    join a in db.Intake_Assessments on c.Client_Id equals a.Client_Id
                    where a.Intake_Assessment_Id.Equals(intAssessmentId)
                    select p.Person_Id).SingleOrDefault();
            }
        }

        public int GetAdoptionPersonIdByintAssId(int intAssessmentId)
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

        public List<AdoptionPersonViewModel> GetAdoptivePerson()
        {
            // initialising view model 
            List<AdoptionPersonViewModel> avm = new List<AdoptionPersonViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var ListP = (
                  from p in db.Client_Adoptive_Parents
                  join c in db.Clients on p.Client_Id equals c.Client_Id
                  join i in db.Persons on p.Person_Id equals i.Person_Id
                  join a in db.Intake_Assessments on c.Client_Id equals a.Client_Id
                  //where a.Intake_Assessment_Id.Equals()
                  select new
                  {
                      p.Client_Adoptive_Parent_Id,
                      i.First_Name,
                      i.Last_Name,
                      i.Date_Of_Birth,
                      i.Identification_Number

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                AdoptionPersonViewModel obj = new AdoptionPersonViewModel();

                obj.Client_Adoptive_Parent_Id = item.Client_Adoptive_Parent_Id;
                obj.First_Name = item.First_Name;
                obj.Last_Name = item.Last_Name;
                obj.Date_Of_Birth = item.Date_Of_Birth;
                obj.Identification_Number = item.Identification_Number;

                avm.Add(obj);

            }
            return avm;
        }

        // GET ADOPTIVE PARENT

        public List<AdoptionPersonViewModel> GetAdoptiveParentPerson(int AdoptiveParent)
        {
            // initialising view model 
            List<AdoptionPersonViewModel> avm = new List<AdoptionPersonViewModel>();
            // initialise connection
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            // get work list for user logged in
            var ListP = (
                  from p in db.Client_Adoptive_Parents
                  join c in db.Clients on p.Client_Id equals c.Client_Id
                  join i in db.Persons on p.Person_Id equals i.Person_Id
                  join a in db.Intake_Assessments on c.Client_Id equals a.Client_Id
                  where a.Intake_Assessment_Id.Equals(AdoptiveParent)
                  select new
                  {
                      p.Client_Adoptive_Parent_Id,
                      i.First_Name,
                      i.Last_Name,
                      i.Date_Of_Birth,
                      i.Identification_Number

                  }).ToList();
            ;
            foreach (var item in ListP)
            {

                // initialising view model
                AdoptionPersonViewModel obj = new AdoptionPersonViewModel();

                obj.Client_Adoptive_Parent_Id = item.Client_Adoptive_Parent_Id;
                obj.First_Name = item.First_Name;
                obj.Last_Name = item.Last_Name;
                obj.Date_Of_Birth = item.Date_Of_Birth;
                obj.Identification_Number = item.Identification_Number;

                avm.Add(obj);
            }

            return avm;
        }

        #endregion

        #region CORRESPONDENCE
        public int GetCorresponenceIdDetailsByssId(int intAssessmentId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                return (from p in db.Persons
                        join c in db.Clients on p.Person_Id equals c.Person_Id
                        join i in db.Intake_Assessments on c.Client_Id equals i.Client_Id
                        join Case in db.ADOPT_Case_Details on i.Intake_Assessment_Id equals Case.Intake_Assessment_Id
                        join Org in db.ADOPT_Letters on Case.Adopt_Case_Id equals Org.Adopt_Case_Id
                        orderby Org.Adopt_Correspondence_Id ascending
                        where i.Intake_Assessment_Id == intAssessmentId
                        select Org.Adopt_Correspondence_Id).FirstOrDefault();
            }
        }

        public AdoptionCaseViewModel GetCorrespondences(int Caseid)
        {
            AdoptionCaseViewModel vm = new AdoptionCaseViewModel();

            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                try
                {
                    int? correspondenceid = (from i in db.ADOPT_Letters
                                             where i.Adopt_Case_Id == Caseid
                                             select i.Adopt_Correspondence_Id).FirstOrDefault();
                    if (correspondenceid > 0)
                    {

                        ADOPT_Letters act = db.ADOPT_Letters.Find(correspondenceid);

                        vm.Correspondence_Type_Id = act.Correspondence_Type_Id;
                        vm.Adopt_Correspondence_Comments = act.Adopt_Correspondence_Comments;
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


        public List<AdoptionCaseViewModel> PrintedLetterList(int assessment)
        {
            // initializing view model
            List<AdoptionCaseViewModel> caseViewModel = new List<AdoptionCaseViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            // get Adoption cases that have been accepted on adoption module

            var ListLetter = (from p in db.Intake_Assessments
                              join b in db.ADOPT_Case_Details on p.Intake_Assessment_Id equals b.Intake_Assessment_Id
                              join c in db.ADOPT_Letters on p.Intake_Assessment_Id equals c.Intake_Assessment_Id
                              join d in db.apl_Adoption_Correspondence_Type on c.Correspondence_Type_Id equals d.Correspondence_Type_Id
                              join f in db.Users on c.Adopt_Correspondence_Created_By equals f.User_Id

                              where (p.Intake_Assessment_Id == assessment)

                              group p by new
                              {
                                  p.Intake_Assessment_Id,
                                  c.Adopt_Correspondence_Id,
                                  c.Adopt_Correspondence_Date_Created,
                                  c.Adopt_Correspondence_Comments,
                                  d.Correspondence_Type_Id,
                                  c.Adopt_Correspondence_Created_By,
                              }
                                     into g
                              select new
                              {
                                  g.Key.Intake_Assessment_Id,
                                  g.Key.Adopt_Correspondence_Id,
                                  g.Key.Adopt_Correspondence_Date_Created,
                                  g.Key.Adopt_Correspondence_Comments,
                                  g.Key.Correspondence_Type_Id,
                                  g.Key.Adopt_Correspondence_Created_By,
                                  Health = g.ToList()

                              }).ToList();
            ;
            foreach (var item in ListLetter)
            {
                AdoptionCaseViewModel obj = new AdoptionCaseViewModel();

                obj.Intake_Assessment_Id = item.Intake_Assessment_Id;
                obj.Adopt_Correspondence_Id = item.Adopt_Correspondence_Id;
                obj.CorrespondenceDescription = db.apl_Adoption_Correspondence_Type.Find(item.Correspondence_Type_Id).Description;
                obj.Adopt_Correspondence_Date_Created = item.Adopt_Correspondence_Date_Created;
                obj.PersonCreated = db.Users.Find(item.Adopt_Correspondence_Created_By).Last_Name;
                obj.Adopt_Correspondence_Comments = item.Adopt_Correspondence_Comments;
                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }

        public void DeleteRecord(int id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {

                ADOPT_Letters Obj = (from j in db.ADOPT_Letters
                                     where j.Adopt_Correspondence_Id == id
                                     select j).FirstOrDefault();
                db.ADOPT_Letters.Remove(Obj);
                db.SaveChanges();
            }
        }


        public void CreateOpenLetter(AdoptionCaseViewModel cases, int Assid, int userId, string savePath, string templatePath)
        {
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            string FileName = "";



            if (cases.Correspondence_Type_Id == 1)
            {

                FileName = "General_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";

            }
            if (cases.Correspondence_Type_Id == 2)
            {
                FileName = "Reminder_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";
            }
            if (cases.Correspondence_Type_Id == 3)
            {
                FileName = "Fault_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";
            }
            if (cases.Correspondence_Type_Id == 4)
            {
                FileName = "Acknowledgment_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";
            }
            if (cases.Correspondence_Type_Id == 5)
            {
                FileName = "Cover_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";
            }
            if (cases.Correspondence_Type_Id == 6)
            {
                FileName = "Home_Affairs_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";
            }
            if (cases.Correspondence_Type_Id == 7)
            {
                FileName = "Enquiries_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";
            }
            if (cases.Correspondence_Type_Id == 8)
            {
                FileName = "Regret_letter_English";
                savePath = savePath + Assid + FileName + ".doc";
                templatePath = templatePath + FileName + ".dot";
            }






            Application app = new Application();
            Document doc = new Document();
            doc = app.Documents.Open(templatePath);
            doc.Activate();

            int? PersonIdChild = (from c in db.ADOPT_Case_Details
                                  join g in db.Intake_Assessments on c.Intake_Assessment_Id equals g.Intake_Assessment_Id
                                  join h in db.Clients on g.Client_Id equals h.Client_Id
                                  join f in db.Persons on h.Person_Id equals f.Person_Id
                                  where c.Intake_Assessment_Id == Assid
                                  select h.Person_Id).FirstOrDefault();



            if (doc.Bookmarks.Exists("ClientName") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["ClientName"].Range.Text = db.Persons.Find(PersonIdChild).First_Name + " " + db.Persons.Find(PersonIdChild).Last_Name;
            }
            if (doc.Bookmarks.Exists("CHildName") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["CHildName"].Range.Text = db.Persons.Find(PersonIdChild).First_Name + " " + db.Persons.Find(PersonIdChild).Last_Name;
            }
            if (doc.Bookmarks.Exists("CHildID") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["CHildID"].Range.Text = db.Persons.Find(PersonIdChild).Identification_Number;
            }

            AddressModel GetAdd = new AddressModel();
            PersonModel GetPer = new PersonModel();

            var Add = db.Persons.Find(PersonIdChild).Addresses.FirstOrDefault();
            int AddressId = Add.Address_Id;
            if (doc.Bookmarks.Exists("StrAdr") && PersonIdChild != 0 && PersonIdChild != null)
            {

                doc.Bookmarks["StrAdr"].Range.Text = GetAdd.GetSpecificAddress(AddressId).Address_Line_1;

            }
            if (doc.Bookmarks.Exists("TownAdr") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["TownAdr"].Range.Text = db.Towns.Find(GetAdd.GetSpecificAddress(AddressId).Town_Id).Description;
            }
            if (doc.Bookmarks.Exists("PCodeAdr") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["PCodeAdr"].Range.Text = GetAdd.GetSpecificAddress(AddressId).Postal_Code;
            }
            if (doc.Bookmarks.Exists("ContactPerson") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["ContactPerson"].Range.Text = db.Persons.Find(PersonIdChild).First_Name + " " + db.Persons.Find(PersonIdChild).Last_Name;
            }
            if (doc.Bookmarks.Exists("Date"))
            {
                doc.Bookmarks["Date"].Range.Text = Convert.ToString(DateTime.Now.ToLongDateString());
            }
            if (doc.Bookmarks.Exists("ClientLetterDate"))
            {
                doc.Bookmarks["ClientLetterDate"].Range.Text = Convert.ToString(DateTime.Now.ToLongDateString());
            }
            if (doc.Bookmarks.Exists("DateOfReg"))
            {
                doc.Bookmarks["DateOfReg"].Range.Text = Convert.ToDateTime((from d in db.ADOPT_Case_Details
                                                                            where d.Intake_Assessment_Id == Assid
                                                                            select d.Date_Registered).FirstOrDefault()).ToString("dd MMMM yyyy");
            }
            if (doc.Bookmarks.Exists("ChildDateOfBirth") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["ChildDateOfBirth"].Range.Text = Convert.ToString(db.Persons.Find(PersonIdChild).Date_Of_Birth);
            }
            if (doc.Bookmarks.Exists("BiologicalMother") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["BiologicalMother"].Range.Text = db.Persons.Find(PersonIdChild).First_Name + " " + db.Persons.Find(PersonIdChild).Last_Name;
            }
            int MotherClientId = (from m in db.Client_Biological_Parents
                                  where m.Person_Id == PersonIdChild
                                  select m.Client_Biological_Parent_Id).FirstOrDefault();
            if (doc.Bookmarks.Exists("BiologicalMotherCountry") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["BiologicalMotherCountry"].Range.Text = Convert.ToString(db.Persons.Find(PersonIdChild).Population_Group);
            }
            if (doc.Bookmarks.Exists("BiologicalMotherState") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["BiologicalMotherState"].Range.Text = Convert.ToString(db.Persons.Find(PersonIdChild).Population_Group);
            }
            if (doc.Bookmarks.Exists("BiologicalMotherLanguage") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["BiologicalMotherLanguage"].Range.Text = Convert.ToString(db.Persons.Find(PersonIdChild).Language);
            }
            if (doc.Bookmarks.Exists("AdoptionCity"))
            {
                doc.Bookmarks["AdoptionCity"].Range.Text = db.Towns.Find(GetAdd.GetSpecificAddress(AddressId).Town_Id).Description;
            }
            int? RelId = (from r in db.RACAP_Prospective_Parent
                          where r.RACAP_Case_Id == Assid
                          select r.Religion_Id).FirstOrDefault();
            if (doc.Bookmarks.Exists("BiologicalMotherChurch"))
            {
                if (RelId != null)
                    doc.Bookmarks["BiologicalMotherChurch"].Range.Text = db.Religions.Find(RelId).Description;
            }
            if (doc.Bookmarks.Exists("BiologicalFather") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["BiologicalFather"].Range.Text = db.Persons.Find(PersonIdChild).First_Name + " " + db.Persons.Find(PersonIdChild).Last_Name;
            }
            if (doc.Bookmarks.Exists("ChildBirthPlace"))
            {
                doc.Bookmarks["ChildBirthPlace"].Range.Text = db.Towns.Find(GetAdd.GetSpecificAddress(AddressId).Town_Id).Description;
            }
            if (doc.Bookmarks.Exists("ChildNameAtBirth") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["ChildNameAtBirth"].Range.Text = db.Persons.Find(PersonIdChild).First_Name + " " + db.Persons.Find(PersonIdChild).Last_Name;
            }
            if (doc.Bookmarks.Exists("SubmissionDate"))
            {
                doc.Bookmarks["SubmissionDate"].Range.Text = Convert.ToDateTime((from d in db.ADOPT_Case_Details
                                                                                 where d.Intake_Assessment_Id == Assid
                                                                                 select d.Date_Registered).FirstOrDefault()).ToString("dd MMMM yyyy");
            }
            if (doc.Bookmarks.Exists("PlaceOfExec"))
            {
                doc.Bookmarks["PlaceOfExec"].Range.Text = db.Towns.Find(GetAdd.GetSpecificAddress(AddressId).Town_Id).Description;
            }
            if (doc.Bookmarks.Exists("Email") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["Email"].Range.Text = db.Persons.Find(PersonIdChild).Email_Address;
            }
            if (doc.Bookmarks.Exists("Enquiries"))
            {
                doc.Bookmarks["Enquiries"].Range.Text = "testc";
            }
            if (doc.Bookmarks.Exists("RefNumber") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["RefNumber"].Range.Text = (from r in db.Clients
                                                         where r.Person_Id == PersonIdChild
                                                         select r.Reference_Number).FirstOrDefault();
            }
            if (doc.Bookmarks.Exists("Telephone") && PersonIdChild != 0 && PersonIdChild != null)
            {
                doc.Bookmarks["Telephone"].Range.Text = db.Persons.Find(PersonIdChild).Phone_Number;
            }
            doc.SaveAs2(savePath);
            app.Application.Quit();

            int Case_Id = (from s in db.ADOPT_Case_Details
                           where s.Intake_Assessment_Id == Assid
                           select s.Adopt_Case_Id).FirstOrDefault();

            ADOPT_Letters newLetter = new ADOPT_Letters();
            newLetter.Intake_Assessment_Id = Assid;
            newLetter.Adopt_Case_Id = Case_Id;
            newLetter.Adopt_Correspondence_Created_By = userId;
            newLetter.Correspondence_Type_Id = cases.Correspondence_Type_Id;
            newLetter.Adopt_Correspondence_Comments = cases.Adopt_Correspondence_Comments;
            newLetter.Adopt_Correspondence_Date_Created = DateTime.Now;
            newLetter.Adopt_Correspondence_FileName = Assid + FileName;
            newLetter.Adopt_Correspondence_FilePath = "~/App_Data/";

            db.ADOPT_Letters.Add(newLetter);
            db.SaveChanges();


        }

        public string PreviewWordLetter(int Adopt_Correspondence_Id)
        {
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            var data = (from a in db.ADOPT_Letters
                        join b in db.ADOPT_Case_Details on a.Adopt_Case_Id equals b.Adopt_Case_Id
                        where a.Adopt_Correspondence_Id == Adopt_Correspondence_Id
                        select new { a.Adopt_Correspondence_FileName }).ToList();

            var TypeId = (from a in db.ADOPT_Letters
                          where a.Adopt_Correspondence_Id == Adopt_Correspondence_Id
                          select a.Correspondence_Type_Id).FirstOrDefault();

            string FileExtention = "";
            if (TypeId == 1)
            {
                FileExtention = "General_letter_English";
            }
            if (TypeId == 2)
            {
                FileExtention = "Reminder_letter_English";
            }
            if (TypeId == 3)
            {
                FileExtention = "Fault_letter_English";
            }
            if (TypeId == 4)
            {
                FileExtention = "Acknowledgment_letter_English";
            }
            if (TypeId == 5)
            {
                FileExtention = "Cover_letter_English";
            }
            if (TypeId == 6)
            {
                FileExtention = "Home_Affairs_letter_English";
            }
            if (TypeId == 7)
            {
                FileExtention = "Enquiries_letter_English";
            }
            if (TypeId == 8)
            {
                FileExtention = "Regret_letter_English";
            }
            string File__ = (from a in data
                             select a.Adopt_Correspondence_FileName).FirstOrDefault();
            return File__;
        }

        #endregion


        #region  LOAD DROP DOWNS MODEL

        public List<AdoptionCategoryLookupAdopt> GetAllAdoptionCategory()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var AdC = db.apl_Adoption_Category.Select(o => new AdoptionCategoryLookupAdopt
                {
                    Adoption_Category_Id = o.Adoption_Category_Id,
                    Description = o.Description
                }).ToList();

                return AdC;


            }
        }
        public List<ChildSocialWorkerLookupAdopt> GetChildSocialWorker()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var x = from c in db.Social_Workers
                        join b in db.Users on c.User_Id equals b.User_Id
                        join o in db.Organizations on c.Organization_Id equals o.Organization_Id
                        select new ChildSocialWorkerLookupAdopt
                        {
                            Child_Social_Worker_Id = b.User_Id,
                            Names = b.Last_Name + " " + b.First_Name
                        };

                return x.ToList();
            }
        }
        public List<ParentSocialWorkerLookupAdopt> GetParentSocialWorker()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var x = from c in db.Social_Workers
                        join b in db.Users on c.User_Id equals b.User_Id
                        join o in db.Organizations on c.Organization_Id equals o.Organization_Id
                        select new ParentSocialWorkerLookupAdopt
                        {
                            Parent_Social_Worker_Id = b.User_Id,
                            SocPNames = b.Last_Name + " " + b.First_Name
                        };

                return x.ToList();
            }
        }

        public List<CorrespondenceTypeLookupAdopt> GetAllCorrespondenceTypes()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Correspondence_Type = db.apl_Adoption_Correspondence_Type.Select(o => new CorrespondenceTypeLookupAdopt
                {
                    Correspondence_Type_Id = o.Correspondence_Type_Id,
                    Description = o.Description
                }).ToList();

                return Correspondence_Type;
            }
        }


        public List<AdoptionTypeLookupAdopt> GetAllAdoptionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var AdoptionType = db.apl_Adoption_Type.Select(o => new AdoptionTypeLookupAdopt
                {
                    Adopt_Type_Id = o.Adopt_Type_Id,
                    Description = o.Description
                }).ToList();

                return AdoptionType;
            }
        }

        public List<AdoptionRelationNonRelationLookupAdopt> GetAllRelationStatus()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var RelationStatus = db.apl_Adoption_Relation_NonRelation.Select(o => new AdoptionRelationNonRelationLookupAdopt
                {
                    NonRelation_Realtion_Id = o.NonRelation_Realtion_Id,
                    Description = o.Description
                }).ToList();

                return RelationStatus;
            }
        }
        public List<RelationshipTypeLookupAdopt> GetAllRelationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var RelationType = db.Relationship_Types.Select(o => new RelationshipTypeLookupAdopt
                {
                    Relationship_Type_Id = o.Relationship_Type_Id,
                    Description = o.Description
                }).ToList();

                return RelationType;
            }
        }
        public List<IdentificationTypeLookupAdopt> GetAllIdentificationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var idType = db.Identification_Types.Select(o => new IdentificationTypeLookupAdopt
                {
                    Identification_Type_Id = o.Identification_Type_Id,
                    Description = o.Description
                }).ToList();

                return idType;
            }
        }

        public List<LanguageTypeLookupAdopt> GetAllLanguageType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var languageType = db.Languages.Select(o => new LanguageTypeLookupAdopt
                {
                    Language_Id = o.Language_Id,
                    Description = o.Description
                }).ToList();

                return languageType;
            }
        }


        public List<GenderTypeLookupAdopt> GetAllGenderType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var genderType = db.Genders.Select(o => new GenderTypeLookupAdopt
                {
                    Gender_Id = o.Gender_Id,
                    Description = o.Description
                }).ToList();

                return genderType;
            }
        }

        public List<MaritalTypeLookupAdopt> GetAllMaritalType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var maritalType = db.Marital_Statusses.Select(o => new MaritalTypeLookupAdopt
                {
                    Marital_Status_Id = o.Marital_Status_Id,
                    Description = o.Description
                }).ToList();

                return maritalType;
            }
        }

        public List<ContactTypeLookupAdopt> GetAllContactType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var contactType = db.Preferred_Contact_Types.Select(o => new ContactTypeLookupAdopt
                {
                    Preferred_Contact_Type_Id = o.Preferred_Contact_Type_Id,
                    Description = o.Description
                }).ToList();

                return contactType;
            }
        }

        public List<ReligionTypeLookupAdopt> GetAllReligionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var religionType = db.Religions.Select(o => new ReligionTypeLookupAdopt
                {
                    Religion_Id = o.Religion_Id,
                    Description = o.Description
                }).ToList();

                return religionType;
            }
        }

        public List<Population_GroupTypeLookupAdopt> GetAllPopulationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var populationType = db.Population_Groups.Select(o => new Population_GroupTypeLookupAdopt
                {
                    Population_Group_Id = o.Population_Group_Id,
                    Description = o.Description
                }).ToList();

                return populationType;
            }
        }

        public List<NationalityTypeLookupAdopt> GetAllNationalityType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var nationalityType = db.Nationalities.Select(o => new NationalityTypeLookupAdopt
                {
                    Nationality_Id = o.Nationality_Id,
                    Description = o.Description
                }).ToList();

                return nationalityType;
            }
        }

        public List<DisabilityTypeLookupAdopt> GetAllDisabilityType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var disabilityType = db.Disabilities.Select(o => new DisabilityTypeLookupAdopt
                {
                    Disability_Type_Id = o.Disability_Id,
                    Description = o.Description
                }).ToList();

                return disabilityType;
            }
        }


        public List<ReasonForAdoptionLookupAdopt> GetAllReasonForAdoptionType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var AdoptionReasonType = db.apl_Adoption_Reason.Select(o => new ReasonForAdoptionLookupAdopt
                {
                    Adoptions_Reason_Id = o.Adoptions_Reason_Id,
                    Description = o.Description
                }).ToList();

                return AdoptionReasonType;
            }
        }


        public List<LegitimacyLookupAdopt> GetAllLegitimacyType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var LegitimacyType = db.apl_Legitimacy.Select(o => new LegitimacyLookupAdopt
                {
                    Legitimacy_Id = o.Legitimacy_Id,
                    Description = o.Description
                }).ToList();

                return LegitimacyType;
            }
        }

        public List<Cross_CulturalLookupAdopt> GetAllCross_CulturalType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var CrossCulturalType = db.apl_Cross_Cultural.Select(o => new Cross_CulturalLookupAdopt
                {
                    Cross_Cultural_Id = o.Cross_Cultural_Id,
                    Description = o.Description
                }).ToList();

                return CrossCulturalType;
            }
        }


        public List<Record_StatusLookupAdopt> GetAllRecord_StatuslType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var StatusType = db.apl_Adoption_Record_Status.Select(o => new Record_StatusLookupAdopt
                {
                    Record_Status_Id = o.Adopt_Record_Status_Id,
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



        public List<ProvinceLookupAdopt> GetAllProvinces()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Province_id = db.Provinces.Select(o => new ProvinceLookupAdopt
                {
                    Province_Id = o.Province_Id,
                    Province_IdChildOrg = o.Province_Id,
                    Province_IdParentOrg = o.Province_Id,
                    Description = o.Description
                }).ToList();

                return Province_id;
            }
        }


        public List<DistrictLookupAdopt> GetAllDistrict()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var District_id = db.Districts.Select(o => new DistrictLookupAdopt
                {
                    District_Id = o.District_Id,
                    District_IdChilOrg = o.District_Id,
                    District_IdParentOrg = o.District_Id,
                    Description = o.Description
                }).ToList();

                return District_id;


            }
        }

        public List<LocalMunicipalityLookupAdopt> GetAllLocalMunicipality()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Local_Mun_id = db.Local_Municipalities.Select(o => new LocalMunicipalityLookupAdopt
                {
                    Local_Municipality_Id = o.Local_Municipality_Id,
                    Local_Municipality_IdParent = o.Local_Municipality_Id,
                    Description = o.Description
                }).ToList();

                return Local_Mun_id;


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


        public List<OrganisationTypeLookupAdoptParent> GetAllParentOrganisationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Parent = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupAdoptParent
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();

                return Parent;
            }
        }

        public List<OrganisationTypeLookupAdoptSocialWorker> GetAllSocialWorkerOrganisationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var SocialWorker = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupAdoptSocialWorker
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();

                return SocialWorker;
            }
        }
        public List<OrganisationTypeLookupAdoptChild> GetAllChildOrganisationType()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Child = db.apl_Organisation_Type.Select(o => new OrganisationTypeLookupAdoptChild
                {
                    Organization_Type_Id = o.IdType,
                    Description = o.Description
                }).ToList();

                return Child;
            }
        }

        public List<OrganisationLookupAdoptChild> GetAllChildOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var Child = db.Organizations.Select(o => new OrganisationLookupAdoptChild
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description
                }).ToList();

                return Child;
            }
        }


        public List<OrganisationLookupAdoptParent> GetAllParentOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parent = db.Organizations.Select(o => new OrganisationLookupAdoptParent
                {
                    Organization_Id = o.Organization_Id,
                    Description = o.Description,
                    Telephone_Number = o.Telephone_Number,
                    Email_Address = o.Email_Address
                }).ToList();

                return parent;
            }
        }

        public List<OrganisationLookupAdoptSocialWorker> GetAllSocialWorkerOrganisation()
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                var parent = db.Organizations.Select(o => new OrganisationLookupAdoptSocialWorker
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

        #region ADOPTION CASE INFROMATION



        //  Get Specific Adoption Case
        public ADOPT_Case_Details GetSpecificAdoptionCase(int Adopt_Case_Id)
        {
            ADOPT_Case_Details Adoptcase;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var AdoptcaseList = (from r in dbContext.ADOPT_Case_Details
                                     where r.Adopt_Case_Id.Equals(Adopt_Case_Id)
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
        public ADOPT_Case_Details GetSpecificAdoptionCaseByIntakeAssment(int Intake_Assessment_Id)
        {
            ADOPT_Case_Details AdoptcaseIntAss;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var AdoptcaseIntAssList = (from r in dbContext.ADOPT_Case_Details
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

        #endregion FOR ADOPTION CASE INFROMATION

        #region REPORTS


        public List<AdoptCaseGridMain> GetAdoptionCases(DateTime searchDateFrom, DateTime searchDateTo)
        {
            // initializing view model
            List<AdoptCaseGridMain> caseViewModel = new List<AdoptCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module
            DateTime today = DateTime.Today;

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     join cas in db.ADOPT_Case_Details on p.Intake_Assessment_Id equals cas.Intake_Assessment_Id
                                     join rec in db.apl_Adoption_Record_Status on cas.Record_Status_Id equals rec.Adopt_Record_Status_Id
                                     join race in db.Population_Groups on person.Population_Group_Id equals race.Population_Group_Id
                                     join gender in db.Genders on person.Gender_Id equals gender.Gender_Id
                                     join court in db.Courts on cas.Court_Id equals court.Court_Id
                                     join district in db.Districts on court.District_Id equals district.District_Id
                                     join prov in db.Provinces on district.Province_Id equals prov.Province_Id
                                     join reason in db.apl_Adoption_Reason on cas.Adoptions_Reason_Id equals reason.Adoptions_Reason_Id
                                     join adtype in db.apl_Adoption_Type on cas.Adopt_Type_Id equals adtype.Adopt_Type_Id

                                     where (cas.Date_Registered >= searchDateFrom && cas.Date_Registered <= searchDateTo)

                                     group p by new
                                     {
                                         client.Client_Id,
                                         person.First_Name,
                                         person.Last_Name,
                                         person.Identification_Number,
                                         person.Gender_Id,
                                         person.Population_Group_Id,
                                         p.Intake_Assessment_Id,
                                         cas.Date_Registered,
                                         rec.Adopt_Record_Status_Id
                                     ,
                                         cas.Court_Id,
                                         prov.Province_Id,
                                         district.District_Id,
                                         reason.Adoptions_Reason_Id,
                                         adtype.Adopt_Type_Id,
                                         person.Date_Of_Birth
                                     }
                                     into g
                                     select new
                                     {
                                         g.Key.Intake_Assessment_Id,
                                         g.Key.Client_Id,
                                         g.Key.First_Name,
                                         g.Key.Last_Name,
                                         g.Key.Identification_Number,
                                         g.Key.Adopt_Record_Status_Id,
                                         g.Key.Population_Group_Id,
                                         g.Key.Gender_Id,
                                         g.Key.Province_Id,
                                         g.Key.Court_Id,
                                         g.Key.District_Id,
                                         g.Key.Adoptions_Reason_Id,
                                         g.Key.Date_Registered,
                                         g.Key.Adopt_Type_Id,
                                         g.Key.Date_Of_Birth,

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
                obj.DescriptionPopulation_Group = db.Population_Groups.Find(item.Population_Group_Id).Description;
                obj.CourtDescription = db.Courts.Find(item.Court_Id).Description;
                obj.DistrictDescription = db.Districts.Find(item.District_Id).Description;
                obj.Description = db.Provinces.Find(item.Province_Id).Description;
                obj.DescriptionGender = db.Genders.Find(item.Gender_Id).Description;
                obj.AdoptionReasonDescription = db.apl_Adoption_Reason.Find(item.Adoptions_Reason_Id).Description;
                obj.DescriptionAdoption_Type = db.apl_Adoption_Type.Find(item.Adopt_Type_Id).Description;
                obj.Province_Id = item.Province_Id;
                obj.Date_Registered = item.Date_Registered;
                obj.Adopt_Record_Status_Id = item.Adopt_Record_Status_Id;
                obj.Population_Group_Id = item.Population_Group_Id;
                obj.Gender_Id = item.Gender_Id;
                obj.Adopt_Type_Id = item.Adopt_Type_Id;

                obj.Date_Of_Birth = today - item.Date_Of_Birth;

                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }


        #endregion
    }


}

