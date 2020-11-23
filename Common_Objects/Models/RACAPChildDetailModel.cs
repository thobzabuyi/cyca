﻿using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class RACAPChildDetailModel
    {
        //GetRACAPChild
        public RACAPCaseMatchesViewModel GetRACAPChild(string receivedid)
        {
            if (receivedid == "")
            {
                receivedid = "0";
            }
            int id = int.Parse(receivedid);
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();


            var person = (from a in db.Persons
                          where a.Person_Id == id
                          select a).FirstOrDefault();
            
            int? SocWorkId = (from p in db.Persons
                              join q in db.Clients on p.Person_Id equals q.Person_Id
                              join o in db.Intake_Assessments on q.Client_Id equals o.Client_Id
                              join s in db.RACAP_Case_Details on o.Intake_Assessment_Id equals s.Intake_Assessment_Id
                              join t in db.RACAP_OrgansationResponsible on s.RACAP_Case_Id equals t.RACAP_Case_Id
                              where p.Person_Id == id
                              select t.Social_Worker_Id).FirstOrDefault();
            int? OrdRespPId = (from p in db.Persons
                               join q in db.Clients on p.Person_Id equals q.Person_Id
                               join o in db.Intake_Assessments on q.Client_Id equals o.Client_Id
                               join s in db.RACAP_Case_Details on o.Intake_Assessment_Id equals s.Intake_Assessment_Id
                               join t in db.RACAP_OrgansationResponsible on s.RACAP_Case_Id equals t.RACAP_Case_Id
                               where p.Person_Id == id
                               select t.Organization_Id_Parent).FirstOrDefault();
            var OrgRespons = db.Organizations.Find(OrdRespPId);

            var CFeatures = (from i in db.Clients
                             join j in db.Intake_Assessments on i.Client_Id equals j.Intake_Assessment_Id
                             join k in db.Persons on i.Person_Id equals k.Person_Id
                             join m in db.RACAP_Adoptive_Child on j.Intake_Assessment_Id equals m.Intake_Assessment_Id
                             where k.Person_Id == id
                             select m).FirstOrDefault();

            int RACAPCaseid = (from o in db.Persons
                               join p in db.Clients on o.Person_Id equals p.Person_Id
                               join q in db.Intake_Assessments on p.Client_Id equals q.Client_Id
                               join r in db.RACAP_Case_Details on q.Intake_Assessment_Id equals r.Intake_Assessment_Id
                               where p.Person_Id == id
                               select r.RACAP_Case_Id).FirstOrDefault();
            int CID = (from a in db.Persons
                       join b in db.Clients on a.Person_Id equals b.Person_Id
                       join c in db.Intake_Assessments on b.Client_Id equals c.Client_Id
                       where a.Person_Id == id
                       select c.Intake_Assessment_Id).FirstOrDefault();

            var SupDocuments = (from p in db.Persons
                                join k in db.Clients on p.Person_Id equals k.Person_Id
                                join l in db.Intake_Assessments on k.Client_Id equals l.Client_Id
                                join m in db.RACAP_Case_Details on l.Intake_Assessment_Id equals m.Intake_Assessment_Id
                                join n in db.RACAP_Supporting_Document on m.RACAP_Case_Id equals n.RACAP_Case_Id
                                where (p.Person_Id == id)
                                select new { n.RACAPDocumentTypeId,n.Document_Description }).ToList();

            RACAPCaseMatchesViewModel newObj = new RACAPCaseMatchesViewModel();
            newObj.intakeAssIdFCDV = CID;
            #region IdNotNull
            if (id != 0)
            {
                //newObj.First_Name_P1 = db.Persons.Find(id).First_Name;
                #region ChildDetails
                newObj.Last_Name_Child = db.Persons.Find(id).First_Name + " " + db.Persons.Find(id).Last_Name;
                if (db.Persons.Find(id).Known_As == null)
                {
                    newObj.Known_As_P1 = "Not captured";
                }
                else
                {
                    newObj.Known_As_Child = db.Persons.Find(id).Known_As;
                }

                if (db.Persons.Find(id).Identification_Number != null)
                {
                    //newObj.Identity_Type_P1 = Convert.ToString(db.Persons.Find(id).Identification_Type_Id);
                    newObj.Identity_Number_Child = db.Persons.Find(id).Identification_Number;
                }
                else
                {
                    newObj.Identity_Number_Child = "Not captured";

                }

                if (person.Language_Id != null)
                {
                    newObj.Language_S = db.Languages.Find(db.Persons.Find(id).Language_Id).Description;
                }
                else
                {
                    newObj.Language_S = "Not captured";

                }

                if (person.Religion_Id != null)
                {
                    newObj.Religion_S = db.Religions.Find(db.Persons.Find(id).Religion_Id).Description;
                }
                else
                {
                    newObj.Religion_S = "Not captured";

                }

                if (person.Population_Group_Id != null)
                {
                    newObj.Race_S = db.Races.Find(db.Persons.Find(id).Population_Group_Id).Description;
                }
                else
                {
                    newObj.Race_S = "Not captured";

                }

                if (db.Persons.Find(id).Age != null)
                {
                    newObj.Age_P1 = Convert.ToString(db.Persons.Find(id).Age);
                }
                else
                {
                    newObj.Age_P1 = "Not captured";
                }
                int RWC = (from c in db.RACAP_Case_WorkList
                           join d in db.Intake_Assessments on c.Intake_Assessment_Id equals d.Intake_Assessment_Id
                           join e in db.Clients on d.Client_Id equals e.Client_Id
                           join f in db.Persons on e.Person_Id equals f.Person_Id
                           where f.Person_Id==id
                           select c.RACAP_CaseWoklist_Id).FirstOrDefault();
                if (RWC != 0)
                {
                    newObj.RACAP_Ref_Number = db.RACAP_Case_WorkList.Find(RWC).Reference_Number;
                }
                else
                {
                    newObj.RACAP_Ref_Number = "Not captured";
                }
                if (RACAPCaseid != 0) {
                    newObj.RecordStatus = db.apl_RACAP_Record_Status.Find(db.RACAP_Case_Details.Find(RACAPCaseid).RACAP_Record_Status_Id).Description;
                        }
                else
                {
                    newObj.RecordStatus = "Not captured";
                }
                #endregion
                #region OrganisationalDetails
                #region OrgNotNull
                if (OrgRespons != null)
                {
                    if (OrgRespons.Description != null)
                    {
                        newObj.Organisation_Responsible_For_Child_S = OrgRespons.Description;
                    }
                    else
                    {
                        newObj.Organisation_Responsible_For_Child_S = "Not captured";
                    }
                    if (db.Users.Find(SocWorkId).Last_Name != null)
                    {
                        newObj.Social_Worker = db.Users.Find(SocWorkId).First_Name + "" + db.Users.Find(SocWorkId).Last_Name;
                    }
                    else
                    {
                        newObj.Social_Worker = "Not captured";
                    }
                    if ((from s in db.Social_Workers
                         where s.User_Id == SocWorkId
                         select s.SocialWorkerPracticeNumber).FirstOrDefault() != null)
                    {
                        newObj.Accreditation_Ref = (from s in db.Social_Workers
                                                    where s.User_Id == SocWorkId
                                                    select s.SocialWorkerPracticeNumber).FirstOrDefault();
                    }
                    else
                    {
                        newObj.Accreditation_Ref = "Not captured";
                    }

                    newObj.Accreditation_Date = (from s in db.Social_Workers
                                                 where s.User_Id == SocWorkId
                                                 select s.AccreditedDate).FirstOrDefault();
                    if (db.Users.Find(SocWorkId).Email_Address != null)
                    {
                        newObj.Email = db.Users.Find(SocWorkId).Email_Address;
                    }
                    else
                    {
                        newObj.Email = "Not captured";
                    }
                    if ((from s in db.Social_Workers
                         where s.User_Id == SocWorkId
                         select s.Mobile_Phone_Number).FirstOrDefault() != null)
                    {
                        newObj.Telephone = (from s in db.Social_Workers
                                            where s.User_Id == SocWorkId
                                            select s.Mobile_Phone_Number).FirstOrDefault();
                    }
                    else
                    {
                        newObj.Telephone = "Not captured";
                    }
                    if ((from p in db.Provinces
                         join o in db.Districts on p.Province_Id equals o.Province_Id
                         join q in db.Local_Municipalities on o.District_Id equals q.District_Municipality_Id
                         join n in db.Service_Offices on q.Local_Municipality_Id equals n.Local_Municipality_Id
                         join m in db.Social_Workers on n.Service_Office_Id equals m.Service_Office_Id
                         where m.User_Id == SocWorkId
                         select p.Description).FirstOrDefault() != null)
                    {
                        newObj.Province = (from p in db.Provinces
                                           join o in db.Districts on p.Province_Id equals o.Province_Id
                                           join q in db.Local_Municipalities on o.District_Id equals q.District_Municipality_Id
                                           join n in db.Service_Offices on q.Local_Municipality_Id equals n.Local_Municipality_Id
                                           join m in db.Social_Workers on n.Service_Office_Id equals m.Service_Office_Id
                                           where m.User_Id == SocWorkId
                                           select p.Description).FirstOrDefault();
                    }
                    else
                    {
                        newObj.Province = "Not captured";
                    }
                }
                #endregion
                #region OrgNull
                if (OrgRespons == null)
                    {
                        newObj.Organisation_Responsible_For_Child_S = "Not captured";
                        newObj.Social_Worker = "Not captured";
                        newObj.Accreditation_Ref = "Not captured";
                        newObj.Email = "Not captured";
                        newObj.Province = "Not captured";
                    }
                #endregion
                #endregion
                #region Features
                if (CFeatures != null)
                    {
                        newObj.Ethnic_CulturalGroup_S = db.apl_Ethnic_Cultural.Find(CFeatures.Ethnic_Cultural_Group_Id).Description;
                        newObj.Eye_Color_S = db.Eye_Colors.Find(CFeatures.Eye_Color_Id).Description;
                        newObj.Body_Structure_S = db.apl_Body_Structure.Find(CFeatures.Body_Structure_Id).Description;
                        newObj.Skin_Color_S = db.apl_Skin_Colour.Find(CFeatures.Skin_Colour_Id).Description;
                        newObj.Language_S = db.Languages.Find(CFeatures.Language_Id).Description;
                        newObj.GenderS = db.apl_Ethnic_Cultural.Find(CFeatures.Ethnic_Cultural_Group_Id).Description;
                        newObj.Race_S = db.Races.Find(CFeatures.Race_Id).Description;
                        newObj.Age_Child = Convert.ToString(db.Persons.Find(person.Person_Id).Age);
                    }
                #endregion
                #region SuportingDoc
                if (SupDocuments.Count != 0)
                {
                    List<RACAP_Supporting_Document> NewObj = new List<RACAP_Supporting_Document>();
                    foreach (var item in SupDocuments)
                    {
                        RACAP_Supporting_Document Obj = new RACAP_Supporting_Document();
                        Obj.RACAPDocumentTypeId = item.RACAPDocumentTypeId;
                        Obj.Document_Description = item.Document_Description;
                        NewObj.Add(Obj);
                    }
                    newObj.DocumentsList = NewObj;
                }
                if (SupDocuments.Count == 0)
                {
                    newObj.DocumentsList = null;
                }
                #endregion

            }
            #endregion

            return newObj;
        }
        //GetRACAPChildSocialWorker

    }
}
