using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common_Objects.Models
{
    public class AdoptionPrintLetter
    {
        private SDIIS_DatabaseEntities _db = new SDIIS_DatabaseEntities();
        #region Child
        public int GetAdoptedChildId(int id)
        {
            return (from p in _db.ADOPT_Case_Details
                    where p.Intake_Assessment_Id == id
                    select p.Adopt_Case_Id).FirstOrDefault();
        }

        public Person GetPersonalInformationChild(int adoptCaseid)
        {
            return (from c in _db.Persons
                    join d in _db.Clients on c.Person_Id equals d.Person_Id
                    join e in _db.Intake_Assessments on d.Client_Id equals e.Client_Id
                    join a in _db.ADOPT_Case_Details on e.Intake_Assessment_Id equals a.Intake_Assessment_Id
                    where a.Adopt_Case_Id == adoptCaseid
                    select c).FirstOrDefault();
        }
        public int GetchildClientIdChild(int id)
        {
            return (from c in _db.Persons
                    join d in _db.Clients on c.Person_Id equals d.Person_Id
                    join e in _db.Intake_Assessments on d.Client_Id equals e.Client_Id
                    where e.Intake_Assessment_Id == id
                    select d.Client_Id).FirstOrDefault();
        }

        public Person GetBIOFemaleparentPersonalinformationChild(int? adoptCaseid)
        {
            return (from q in _db.Client_Biological_Parents
                    join o in _db.Persons on q.Person_Id equals o.Person_Id
                    join p in _db.Clients on q.Client_Id equals p.Client_Id
                    join r in _db.Intake_Assessments on p.Client_Id equals r.Client_Id
                    join a in _db.ADOPT_Case_Details on r.Intake_Assessment_Id equals a.Intake_Assessment_Id
                    join g in _db.Genders on o.Gender_Id equals g.Gender_Id
                    where (a.Adopt_Case_Id == adoptCaseid) &&  (g.Gender_Id ==2)
                    select o).FirstOrDefault();
        }

        public Person GetBIOMaleparentPersonalinformationChild(int? adoptCaseid)
        {
            return (from q in _db.Client_Biological_Parents
                    join o in _db.Persons on q.Person_Id equals o.Person_Id
                    join p in _db.Clients on q.Client_Id equals p.Client_Id
                    join r in _db.Intake_Assessments on p.Client_Id equals r.Client_Id
                    join a in _db.ADOPT_Case_Details on r.Intake_Assessment_Id equals a.Intake_Assessment_Id
                    join g in _db.Genders on o.Gender_Id equals g.Gender_Id
                    where (a.Adopt_Case_Id == adoptCaseid) && (g.Gender_Id == 1)
                    select o).FirstOrDefault();
        }

        public Person GetADOPTIVEparentClientIdFEMALEChild(int? adoptCaseid)
        {
            return (from q in _db.Client_Adoptive_Parents
                    join o in _db.Persons on q.Person_Id equals o.Person_Id
                    join p in _db.Clients on q.Client_Id equals p.Client_Id
                    join r in _db.Intake_Assessments on p.Client_Id equals r.Client_Id
                    join a in _db.ADOPT_Case_Details on r.Intake_Assessment_Id equals a.Intake_Assessment_Id
                    join g in _db.Genders on o.Gender_Id equals g.Gender_Id
                    where (a.Adopt_Case_Id == adoptCaseid) && (g.Gender_Id == 2)
                    select o).FirstOrDefault();
        }

        public Person GetADOPTIVEparentClientIdMALEChild(int? adoptCaseid)
        {
            return (from q in _db.Client_Adoptive_Parents
                    join o in _db.Persons on q.Person_Id equals o.Person_Id
                    join p in _db.Clients on q.Client_Id equals p.Client_Id
                    join r in _db.Intake_Assessments on p.Client_Id equals r.Client_Id
                    join a in _db.ADOPT_Case_Details on r.Intake_Assessment_Id equals a.Intake_Assessment_Id
                    join g in _db.Genders on o.Gender_Id equals g.Gender_Id
                    where (a.Adopt_Case_Id == adoptCaseid) && (g.Gender_Id == 1)
                    select o).FirstOrDefault();
        }


        public string GetreferenceNumber(int adoptCaseid)
        {
            return (from t in _db.ADOPT_Case_Details
                    join u in _db.Intake_Assessments on t.Intake_Assessment_Id equals u.Intake_Assessment_Id
                    join v in _db.Clients on u.Client_Id equals v.Client_Id
                    join re in _db.int_Client_Module_Registration on v.Client_Id equals re.Client_Id
                    join a in _db.ADOPT_Case_Details on u.Intake_Assessment_Id equals a.Intake_Assessment_Id
                    where t.Adopt_Case_Id == adoptCaseid
                    select re.Client_Module_Ref_No).FirstOrDefault();
        }

        public void AddAdoptionCorrespondence(int id, string commentCap, string corId, string filenameDB, int loggedInUser, int iD)
        {
            var currentHoursAndMinutes = DateTime.Now.Hour.ToString("0#") + DateTime.Now.Minute.ToString("0#") + DateTime.Now.Millisecond.ToString("0#");
            AdoptionPrintLetter Model = new AdoptionPrintLetter();

            var Table = new ADOPT_Letters();
            Table.Adopt_Correspondence_Comments = commentCap;
            Table.Adopt_Case_Id = id;
            Table.Intake_Assessment_Id = iD;
            Table.Correspondence_Type_Id = Convert.ToInt32(corId);
            Table.Adopt_Correspondence_FileName = filenameDB;
            Table.Adopt_Correspondence_Date_Created = DateTime.Now;
            var userModel = new UserModel();
            Table.Adopt_Correspondence_Created_By = loggedInUser;
            Table.Adopt_Correspondence_FilePath = "PathTemp";
            _db.ADOPT_Letters.Add(Table);
            _db.SaveChanges();
        }

        #endregion
    }
}
