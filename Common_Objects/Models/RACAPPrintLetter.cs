using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class RACAPPrintLetter
    {
        private SDIIS_DatabaseEntities _db = new SDIIS_DatabaseEntities();
        #region Parent
            public int GetRacapProParentId(int id)
            {
                return (from p in _db.RACAP_Prospective_Parent
                        where p.Intake_Assessment_Id == id
                        select p.RACAP_Prospective_Parent_Id).FirstOrDefault();
            }

            public int? GetRacapAdpChildId(int RacapProParentId)
            {
                return (from p in _db.RACAP_Matches
                        where p.RACAP_Prospective_Parent_Id == RacapProParentId
                        select p.RACAP_Adoptive_Child_Id).FirstOrDefault();
            }

            public Person GetchildPersonalInformation(int? RacapAdpChildId)
            {
                return (from c in _db.Persons
                 join d in _db.Clients on c.Person_Id equals d.Person_Id
                 join e in _db.Intake_Assessments on d.Client_Id equals e.Client_Id
                 join f in _db.RACAP_Case_Details on e.Intake_Assessment_Id equals f.Intake_Assessment_Id
                 join g in _db.RACAP_Adoptive_Child on f.RACAP_Case_Id equals g.RACAP_Case_Id
                 where g.RACAP_Adoptive_Child_Id == RacapAdpChildId
                 select c).FirstOrDefault();
            }

            public int GetchildClientId(int Id)
            {
                return (from r in _db.Clients
                        where r.Person_Id == Id
                        select r.Client_Id).FirstOrDefault();
            }
            public int? GetmotherPersonId(int? id)
            {
                return (from c in _db.Client_Biological_Parents
                        where c.Client_Id == id
                        select c.Person_Id).FirstOrDefault();
            }

            public Person GetmotherPersonalInformation(int? motherPersonId)
            {
                return _db.Persons.Find(motherPersonId);
            }

            public int? GetfatherPersonId(int? id)
            {
                return (from c in _db.Client_Biological_Parents
                        where c.Client_Id == id && c.Relationship_Type_Id == 2
                        select c.Person_Id).FirstOrDefault();
            }

            public Person GetfatherPersonalInformation(int? fatherPersonId)
            {
                return _db.Persons.Find(fatherPersonId);
            }

            public Person GetparentPersonalinformation(int id)
            {
                return (from c in _db.Persons
                        join d in _db.Clients on c.Person_Id equals d.Person_Id
                        join e in _db.Intake_Assessments on d.Client_Id equals e.Client_Id
                        where e.Intake_Assessment_Id == id
                        select c).FirstOrDefault();
            }

            public string GetreferenceNumber(int id)
            {
               string NewStringifyRef =(from t in _db.RACAP_Case_WorkList
                     
                        where t.Intake_Assessment_Id == id
                        select t.Reference_Number).FirstOrDefault();
            return NewStringifyRef;
            }

            public int GetparentClientId(int id)
            {
                return (from q in _db.Clients
                        join r in _db.Persons on q.Person_Id equals r.Person_Id
                        where r.Person_Id == id
                        select q.Client_Id).FirstOrDefault();
            }

            public int GetRACAPCaseId(int id)
            {
                return (from r in _db.RACAP_Case_Details
                        where r.Intake_Assessment_Id == id
                        select r.RACAP_Case_Id).FirstOrDefault();
            }

            public void UpdateRACAPCorrespondence(int id, string commentCap, string corId, string outputFileName, int loggedInUser, int iD)
            {
                int CId = Convert.ToInt32(corId);
                var racapCorTable = (from r in _db.RACAP_Correspondence
                                     where r.RACAP_Case_Id == id && r.RACAP_Correspondence_FileName == (iD + "_" + CId + ".pdf")
                                     select r).FirstOrDefault();
                RACAPPrintLetter Model = new RACAPPrintLetter();

                var parentPersonalinformation = Model.GetparentPersonalinformation(id);

                if (racapCorTable != null)
                {
                    racapCorTable.RACAP_Correspondence_Comments = commentCap;
                    racapCorTable.RACAP_Case_Id = id;
                    racapCorTable.RACAP_Correspondence_Type_Id = Convert.ToInt32(corId);
                    racapCorTable.RACAP_Correspondence_FileName = iD + "_" + Convert.ToInt32(corId) + ".pdf";
                    racapCorTable.RACAP_Correspondence_Date_Modified = DateTime.Now;
                    racapCorTable.RACAP_Correspondence_Modified_By = loggedInUser;
                    racapCorTable.RACAP_Correspondence_FilePath = "RACAPDocumentPath";
                    _db.SaveChanges();
                }
            }

            public void AddRACAPCorrespondence(int id, string commentCap, string corId, string outputFileName, int loggedInUser, int iD)
            {
                var currentHoursAndMinutes = DateTime.Now.Hour.ToString("0#") + DateTime.Now.Minute.ToString("0#") + DateTime.Now.Millisecond.ToString("0#");
            RACAPPrintLetter Model = new RACAPPrintLetter();

            var parentPersonalinformation = Model.GetparentPersonalinformation(id);
            var racapCorTable = new RACAP_Correspondence();
                racapCorTable.RACAP_Correspondence_Comments = commentCap;
                racapCorTable.RACAP_Case_Id = id;
                racapCorTable.RACAP_Correspondence_Type_Id = Convert.ToInt32(corId);
                racapCorTable.RACAP_Correspondence_FileName = iD + "_" + Convert.ToInt32(corId)  + ".pdf";
                racapCorTable.RACAP_Correspondence_Date_Created = DateTime.Now;
                var userModel = new UserModel();
                racapCorTable.RACAP_Correspondence_Created_By = loggedInUser;
                racapCorTable.RACAP_Correspondence_FilePath = "RACAPDocumentPath";
                _db.RACAP_Correspondence.Add(racapCorTable);
                _db.SaveChanges();
            }
        #endregion


        #region Child
        public int GetRacapAdoptedChildIdChild(int id)
        {
            return (from p in _db.RACAP_Adoptive_Child
                    where p.Intake_Assessment_Id == id
                    select p.RACAP_Adoptive_Child_Id).FirstOrDefault();
        }

        public int? GetRacapProParentIdChild(int RacapAdoptedChildId)
        {
            return (from p in _db.RACAP_Matches
                    where p.RACAP_Adoptive_Child_Id == RacapAdoptedChildId
                    select p.RACAP_Prospective_Parent_Id).FirstOrDefault();
        }

        public Person GetchildPersonalInformationChild(int id)
        {
            return (from c in _db.Persons
                    join d in _db.Clients on c.Person_Id equals d.Person_Id
                    join e in _db.Intake_Assessments on d.Client_Id equals e.Client_Id
                    where e.Intake_Assessment_Id == id
                    select c).FirstOrDefault();
        }
        public int GetchildClientIdChild(int id)
        {
            return (from c in _db.Intake_Assessments
                    join d in _db.Clients on c.Client_Id equals d.Client_Id
                    where c.Intake_Assessment_Id == id
                    select d.Client_Id).FirstOrDefault();
        }

        public Person GetparentPersonalinformationChild(int? RacapProParentId)
        {
            return (from q in _db.RACAP_Prospective_Parent
                    join o in _db.Intake_Assessments on q.Intake_Assessment_Id equals o.Intake_Assessment_Id
                    join p in _db.Clients on o.Client_Id equals p.Client_Id
                    join r in _db.Persons on p.Person_Id equals r.Person_Id
                    where q.RACAP_Prospective_Parent_Id == RacapProParentId
                    select r).FirstOrDefault();
        }

        public int GetparentClientIdChild(int? RacapProParentId)
        {
            return (from q in _db.RACAP_Prospective_Parent
                    join o in _db.Intake_Assessments on q.Intake_Assessment_Id equals o.Intake_Assessment_Id
                    join p in _db.Clients on o.Client_Id equals p.Client_Id
                    join r in _db.Persons on p.Person_Id equals r.Person_Id
                    where q.RACAP_Prospective_Parent_Id == RacapProParentId
                    select p.Client_Id).FirstOrDefault();
        }
        #endregion

        public Person GetSecondPerson(int id)
        {
            return (from s in _db.int_Client_Prospective_Adoptive_Parent
                    join t in _db.Clients on s.Client_Id equals t.Client_Id
                    join u in _db.Intake_Assessments on t.Client_Id equals u.Client_Id
                    join p in _db.Persons on s.Person_Id equals p.Person_Id
                    where u.Intake_Assessment_Id == id
                    select p).FirstOrDefault();
        }

        public RACAP_Correspondence GetRACAPCorrespondence(int RACAPCaseId,int id, int Cid)
        {
            return (from r in _db.RACAP_Correspondence
                    where r.RACAP_Case_Id == RACAPCaseId && r.RACAP_Correspondence_FileName == (id + "_" + Cid + ".pdf")
                    select r).FirstOrDefault();
        }

        public string GetTownDescription(int? Id)
        {
            return _db.Towns.Find(Id).Description;
        }

        public ICollection<Address> GetAddresses(int Id)
        {
            return _db.Persons.Find(Id).Addresses;
        }
    }
}
