using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Common_Objects.Models
{
    public class UnsuitabilityModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public List<CPR_Unsuitability> GetListOfUnsuitablePersons(bool showWithoutRuiling)
        {
            try
            {
                List<CPR_Unsuitability> listOfUnsuitables = new List<CPR_Unsuitability>();

                var dbContext = new SDIIS_DatabaseEntities();
                try
                {

                    var unsuitables = (from c in dbContext.CPR_Unsuitability
                                       where c.IsRemoved != true
                                       select c).ToList();



                    listOfUnsuitables = (from c in unsuitables
                                         where c.IsRemoved != true
                                         orderby c.Person_Id
                                         select c).ToList();

                }
                catch (Exception ex)
                {
                    return null;
                }

                return listOfUnsuitables;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void AddUnsuitablePerson(int person_Id, int incident_Id, int createdBy)
        {
            try {
                var newCPR_Unsuitability = new CPR_Unsuitability
                {
                    Person_Id = person_Id,
                    Incident_Id = incident_Id,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy,
                    RegistrationType = 1
                };
                db.CPR_Unsuitability.Add(newCPR_Unsuitability);
                db.SaveChanges();
                newCPR_Unsuitability.CPRB_ReferenceNumber = "CPRB/NAT/" + newCPR_Unsuitability.Unsuitablity_Id.ToString("0000000#") + "/" + DateTime.Now.Year.ToString();
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


        }

        public void EditUnsuitability(int person_Id, int incident_Id, int createdBy)
        {
            try {
                var oldCPR_Unsuitability = (from a in db.CPR_Unsuitability
                                            where a.Person_Id == person_Id
                                            select a).FirstOrDefault();

                if (oldCPR_Unsuitability == null)
                {
                    oldCPR_Unsuitability = new CPR_Unsuitability();
                    oldCPR_Unsuitability.Person_Id = person_Id;
                    oldCPR_Unsuitability.Incident_Id = incident_Id;
                    oldCPR_Unsuitability.CreatedDate = DateTime.Now;
                    oldCPR_Unsuitability.CreatedBy = createdBy;
                    oldCPR_Unsuitability.RegistrationType = 1;
                    db.CPR_Unsuitability.Add(oldCPR_Unsuitability);
                    db.SaveChanges();
                    oldCPR_Unsuitability.CPRB_ReferenceNumber = "CPRB/NAT/" + oldCPR_Unsuitability.Unsuitablity_Id.ToString("0000000#") + "/" + DateTime.Now.Year.ToString();
                    db.SaveChanges();
                }
                else
                {
                    oldCPR_Unsuitability.Incident_Id = incident_Id;
                    oldCPR_Unsuitability.CreatedDate = DateTime.Now;
                    oldCPR_Unsuitability.CreatedBy = createdBy;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CPR_Unsuitability GetUnsuitabilityDetails(int id)
        {
            try {
                return db.CPR_Unsuitability.Where(x => x.Unsuitablity_Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CPR_Unsuitability_Conviction GetUnsuitabilityConviction(int id)
        {
            try {
                return db.CPR_Unsuitability_Conviction.Where(x => x.Unsuitability_Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CPR_Unsuitability_Ruiling GetUnsuitabilityRuling(int id)
        {
            try {
                return db.CPR_Unsuitability_Ruiling.Where(x => x.Unsuitability_Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

        }


        public string GetTownDescription(int? id)
        {
            try
            {
                if (id != null)
                {
                    return db.Towns.Find(id).Description;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public string GetDistrictDescription(int? id)
        {
            try
            {
                return db.Districts.Find(id).Description;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public string GetDistrictDescriptionViaTown(string id)
        {
            if (id != null && id != "")
            {
                int newId = Convert.ToInt32(id);
                int LocalMunId = db.Towns.Find(newId).Local_Municipality_Id;
                int DistrictId =db.Local_Municipalities.Find(LocalMunId).District_Municipality_Id;
                return db.Districts.Find(DistrictId).Description;
            }
            else return null;
        }


        public CPR_Unsuitability_Forum GetUnsuitabilityForum(int id)
        {
            try
            {
                return db.CPR_Unsuitability_Forum.Find(id);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CPR_Unsuitability_Findings GetUnsuitabilityFindings(int id)
        {
            try
            {
                return db.CPR_Unsuitability_Findings.Where(x => x.Unsuitability_Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }

        }


        public List<CPR_Unsuitability> filteredList(string SearchSurname, string SearchName, string SearchRecNumber)
        {
            return (from a in db.CPR_Unsuitability
                    join b in db.Persons on a.Person_Id equals b.Person_Id
                    where
                    (
                        (b.Last_Name.Contains(SearchSurname) || SearchSurname == "" || SearchSurname == null)
                        &&
                        (b.First_Name.Contains(SearchName) || SearchName == "" || SearchName == null)
                        &&
                        (a.CPRB_ReferenceNumber.Contains(SearchRecNumber) || SearchRecNumber == "" || SearchRecNumber == null)
                    //&&
                    //(a.RegistrationTypeText.Contains(searchFindingBy) ||searchFindingBy==""||searchFindingBy==null)
                    )
                    orderby a.NotificationDate
                    select a
                                ).OrderByDescending(a => a.NotificationDate).ToList();
        }

        public List<CPR_Unsuitability> GetListOfUnsuitablePersons()
        {
            List<CPR_Unsuitability> listOfUnsuitablePersons;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var unsuitablePersons = (from p in dbContext.CPR_Unsuitability
                                         select p).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfUnsuitablePersons = (from p in unsuitablePersons
                                           select p).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfUnsuitablePersons;
        }


        public Client_CareGiver GetUnsuitabilityCareGiver(int id)
        {
            return db.Client_CareGivers.FirstOrDefault(p => p.Client_Caregiver_Id == id);
        }


        public List<CPR_Unsuitability_Forum> GetAllForums()
        {
            return (from f in db.CPR_Unsuitability_Forum
                    select f).ToList();
        }

        public CPR_Unsuitability_Findings GetCPR_Unsuitability_Findings(int id)
        {
            return db.CPR_Unsuitability_Findings.FirstOrDefault(p => p.Unsuitability_Id.Equals(id));
        }

        public Client GetChildDetails(int? ChildId)
        {
            return db.Clients.FirstOrDefault(p => p.Client_Id == ChildId);
        }
        public CPR_Unsuitability_Incedent GetIncedent(int? IncidentId)
        {
            return db.CPR_Unsuitability_Incedent.FirstOrDefault(p => p.Incedent_Id == IncidentId);
        }
        public string GetCourtName(int? Court_Id)
        {
            return db.Courts.Where(x => x.Court_Id == Court_Id).Select(p => p.Description).FirstOrDefault();
        }

        public CPR_Unsuitability_Forum GetCPR_Unsuitability_Forum(string Forum_Name)
        {
            return db.CPR_Unsuitability_Forum.Where(x => x.Forum_Name == Forum_Name).FirstOrDefault();
        }

        public CPR_Unsuitability_Forum GetCPR_Unsuitability_ForumByForum_Id(int? Forum_Id)
        {
            return db.CPR_Unsuitability_Forum.FirstOrDefault(p => p.Forum_Id == Forum_Id);
        }

        public void AddCPR_Unsuitability_Forum(CPR_Unsuitability_Forum forumCreate)
        {
            db.CPR_Unsuitability_Forum.Add(forumCreate);
        }

        public void SaveInfo()
        {
            db.SaveChanges();
        }

        public CPR_Unsuitability GetCPR_Unsuitability(int Unsuitability_Id)
        {
            return (from un in db.CPR_Unsuitability
                    where un.Unsuitablity_Id.Equals(Unsuitability_Id)
                    select un).FirstOrDefault();
        }

        public void AddCPR_Unsuitability_Findings(CPR_Unsuitability_Findings findings)
        {
            db.CPR_Unsuitability_Findings.Add(findings);
        }

        public CPR_Unsuitability_Conviction GetCPR_Unsuitability_Conviction(int Unsuitability_Id)
        {
            return db.CPR_Unsuitability_Conviction.Where(p => p.Unsuitability_Id == Unsuitability_Id).FirstOrDefault();
        }

        public void AddCPR_Unsuitability_Conviction(CPR_Unsuitability_Conviction conviction)
        {
            db.CPR_Unsuitability_Conviction.Add(conviction);
        }

        public CPR_Unsuitability_Ruiling GetCPR_Unsuitability_Ruiling(int? Unsuitability_Id)
        {
            return db.CPR_Unsuitability_Ruiling.Where(x => x.Unsuitability_Id == Unsuitability_Id).FirstOrDefault();
        }

        public void AddCPR_Unsuitability_Ruiling(CPR_Unsuitability_Ruiling rulingCreate)
        {
            db.CPR_Unsuitability_Ruiling.Add(rulingCreate);
        }

        public Person GetPerson(string First_Name, string Last_Name, string Identification_Number)
        {
            return db.Persons.Where(p => p.First_Name == First_Name && p.Last_Name == Last_Name && p.Identification_Number == Identification_Number).FirstOrDefault();
        }

        public void AddCPR_Unsuitability_Incedent(CPR_Unsuitability_Incedent incedent)
        {
            db.CPR_Unsuitability_Incedent.Add(incedent);
        }

        public CPR_Unsuitability_Incedent GetunsIncident(int UnsuitId)
        {
            return (from un in db.CPR_Unsuitability_Incedent
                    where un.Unsuitability_Id.Equals(UnsuitId)
                    select un).FirstOrDefault();
        }

        public int GetClient_Id(int Person_Id)
        {
            return (from a in db.Clients
                    where a.Person_Id == Person_Id
                    select a.Client_Id).FirstOrDefault();
        }

        public void AddCPR_Unsuitability(CPR_Unsuitability unsuitablePerson)
        {
            db.CPR_Unsuitability.Add(unsuitablePerson);
        }

        public int GetLocal_Municipality_Id(int? CityTown_Id)
        {
            return (from a in db.Towns
                    where a.Town_Id == CityTown_Id
                    select a.Local_Municipality_Id).FirstOrDefault();
        }
        public string GetForumName(int? Forum_Id)
        {
            return db.CPR_Unsuitability_Forum.Find(Forum_Id).Forum_Name;
        }

        public Client GetClientDetails(int? ChildId)
        {
            return db.Clients.FirstOrDefault(p => p.Client_Id == ChildId);
        }
        public CPR_Unsuitability_Conviction GetConviction(int? ConvictionId)
        {
            return db.CPR_Unsuitability_Conviction.FirstOrDefault(p => p.Conviction_Id == ConvictionId);
        }
        public string GetCourtReferenceNumber(int? Incident_Id)
        {
            return (from r in db.CPR_Childrens_Court_Detail_Items
                    where r.Incident_Id == Incident_Id
                    select r.Court_Order_Number).FirstOrDefault();
        }

        public Client_CareGiver GetClient_CareGivers(int? CareGiverId)
        {
            return db.Client_CareGivers.FirstOrDefault(p => p.Client_Caregiver_Id == CareGiverId);
        }

        public int GetIntakeAssId(int? incident_Id)
        {
            if ((from a in db.CPR_Incidents
                 where a.Incident_Id == incident_Id
                 select a).FirstOrDefault() != null)
            {
                int Result = db.CPR_Incidents.Find(incident_Id).Intake_Assessment_Id;
                if (Result != 0)
                {
                    return Result;
                }
                else return 0;
            }
            else return 0;


        }

        public string GetReference_Number(int? incident_Id)
        {
            if ((from a in db.CPR_Incidents
                 where a.Incident_Id == incident_Id
                 select a).FirstOrDefault() != null)
            {
                string Result = db.CPR_Incidents.Find(incident_Id).Reference_Number;
                if (Result != null && Result != "")
                {
                    return Result;
                }
                else return null;
            }
            else return null;
        }

        public int GetPerson_Id(int IntakeAssId)
        {
            if (IntakeAssId != 0) { 
            return (from p in db.Intake_Assessments
                    join q in db.Clients on p.Client_Id equals q.Client_Id
                    join r in db.Persons on q.Person_Id equals r.Person_Id
                    where p.Intake_Assessment_Id == IntakeAssId
                    select r.Person_Id).FirstOrDefault();
            }
            else
            {
                return 0;
            }
        }

        public string GetLast_Name(int ChildPersonId)
        {
            if (ChildPersonId != 0)
            {
                return db.Persons.Find(ChildPersonId).Last_Name;
            }
            else 
            {
                return "";
            }
        }

        public string GetFirst_Name(int ChildPersonId)
        {
            if (ChildPersonId != 0) { 
                return db.Persons.Find(ChildPersonId).First_Name;
            }
            else
            {
                return null;
            }
        }
        public string GetKnown_As(int ChildPersonId)
        {
            if (ChildPersonId != 0) { 
                return db.Persons.Find(ChildPersonId).Known_As;
            }
            else
            {
                return null;
            }
        }

        public int? GetSelectedIdType_Id(int ChildPersonId)
        {
            if (ChildPersonId != 0)
            {
                return db.Persons.Find(ChildPersonId).Identification_Type_Id;
            }
            else
            {
                return 0;
            }
        }

        public string GetIdentification_Number(int ChildPersonId)
        {
            if (ChildPersonId != 0) { 
            return db.Persons.Find(ChildPersonId).Identification_Number;
            }
            else
            {
                return null;
            }
        }
        public int? GetAge(int ChildPersonId)
        {
            if (ChildPersonId != 0) { 
            return db.Persons.Find(ChildPersonId).Age;
            }
            else
            {
                return 0;
            }
        }

        public DateTime? GetDate_Of_Birth(int ChildPersonId)
        {
            if (ChildPersonId != 0)
            {
                return db.Persons.Find(ChildPersonId).Date_Of_Birth;
            }
            else
            {
                return Convert.ToDateTime("");
            }
         }

        public int? GetPopulation_Group_Id(int ChildPersonId)
        {
            if (ChildPersonId != 0)
            {
                return db.Persons.Find(ChildPersonId).Population_Group_Id;
            }
            else
            {
                return 0;
            }
        }

        public int? GetGender_Id(int ChildPersonId)
        {
            if (ChildPersonId != 0)
            {
                return db.Persons.Find(ChildPersonId).Gender_Id;
            }
            else
            {
                return 0;
            }
        }
        public CPR_Unsuitability_Incedent GetIncedent(int IncidentId)
        {
            if (IncidentId != 0) { 
            return db.CPR_Unsuitability_Incedent.FirstOrDefault(p => p.Incedent_Id == IncidentId);
            }
            else
            {
                return null;
            }
        }

        public List<CPR_Unsuitability> GetCPRUnsauit(string SearchSurname, string SearchName, string SearchRecNumber, string searchFindingBy)
        {
            int tt = 0;

            var db = new SDIIS_DatabaseEntities();
            if (searchFindingBy == "Forum")
            {
                tt = 0;
            }
            if (searchFindingBy == "Court")
            {
                tt = 1;
            }
            if (searchFindingBy == "Select")
            {
                tt = -1;
            }
            return (from a in db.CPR_Unsuitability
                    join b in db.Persons on a.Person_Id equals b.Person_Id
                    where
                    (
                        (b.Last_Name.Contains(SearchSurname) || SearchSurname == "" || SearchSurname == null)
                        &&
                        (b.First_Name.Contains(SearchName) || SearchName == "" || SearchName == null)
                        &&
                        (a.CPRB_ReferenceNumber.Contains(SearchRecNumber) || SearchRecNumber == "" || SearchRecNumber == null)
                        &&
                        (a.RegistrationType == tt)
              //&&
              //(a.RegistrationTypeText.Contains(searchFindingBy) ||searchFindingBy==""||searchFindingBy==null)
              )
                    orderby a.NotificationDate
                    select a
                                ).OrderByDescending(a => a.NotificationDate).ToList();
        }

        public string GetAbuseType(string IncidentId)
        {
            if (IncidentId != null && IncidentId != "")
            {
                int newId = Convert.ToInt32(IncidentId);
                return (from a in db.Incident_Abuse_Types
                        join b in db.Abuse_Types on a.Abuse_Type_Id equals b.Abuse_Type_Id
                        where a.Incident_Id == newId
                        select b.Description).FirstOrDefault();
            }
            else return null;
        }

        public Person GetPerson(int PersonId)
        {
            if (PersonId != 0)
            {
                return db.Persons.Find(PersonId);
            }
            else
            {
                return null;
            }
        }
        public CPR_Incident GetIncidentToChild(string UnsuitabilityId)
        {
            if (UnsuitabilityId != null && UnsuitabilityId != "")
            {
                int newIncidentId_1 = Convert.ToInt32(UnsuitabilityId);
                List<CPR_Incident> Incidents = (from a in db.CPR_Unsuitability
                                   join b in db.Alleged_Offenders on a.Person_Id equals b.Person_Id
                                   join c in db.CPR_Incidents on b.Incident_Id equals c.Incident_Id
                                   where a.Unsuitablity_Id == newIncidentId_1
                                                select c).ToList();
                int IncidentId = (from f in Incidents
                                     select f.Incident_Id).LastOrDefault();
               //select b.Incident_Id).Last();
                if (IncidentId != 0)
                {
                    return db.CPR_Incidents.Find(IncidentId);
                }
                else { 
                return null;
                }
            }
            else return null;
        }

        public List<Abuse_Indicator> GetListOrIndicators(string Incident_Id)
        {
            if (Incident_Id != null && Incident_Id != "")
            {
                int newIncident_Id = Convert.ToInt32(Incident_Id);
                return (from a in db.Abuse_Indicators
                        where a.CPR_Incidents.Equals(newIncident_Id)
                        select a).ToList();
            }
            else return null;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
    public class CPRUnsuitabilityGridViewModel
    {
        public List<CPR_Unsuitability> Unsuitability_List { get; set; }
        public List<UnsuitabilityGridMain> Unsuitability_Lists { get; set; }
        public int Unsuitablity_Id { get; set; }
        public int? PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }
        public string Recnumber { get; set; }
        [Display(Name = "Find By")]
        public string FindingBy { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        [Display(Name = "Type of Offence")]

        public string TypeOfOffence { get; set; }
        [Display(Name = "Date Reported")]


        public DateTime DateReported { get; set; }
        [Display(Name = "Notification Date")]

        public DateTime? NotificationDate { get; set; }
        [Display(Name = "City/Town")]

        public string City_Town { get; set; }
        [Display(Name = "Magisterial District")]

        public string MagisterialDistrict { get; set; }


        public string Search_Surname { get; set; }
        public string Search_Name { get; set; }
        public string Search_RecNumber { get; set; }
        public string Search_FindingBy { get; set; }
        public bool Search_Ruiling { get; set; }
        public int PersonId { get; set; }
    }




}
