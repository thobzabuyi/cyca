using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ClientModel
    {
        private readonly SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();

        #region CreateReferenceNumber

        public string CreateReferenceNumber(string provinceCode, string yearCode, int? Problem_Category_Id)
        {
            string Problem_Category_Name = dbContext.Problem_Categories.Find(Problem_Category_Id).Source;
            int SeqNumber = (from a in dbContext.int_Client_Module_Registration
                             orderby a.Client_Module_Id descending
                             select a.Client_Id).FirstOrDefault();
            //string NewRefNumber = Problem_Category_Name + "/" + provinceCode + "/" + SeqNumber.ToString("000000000").Replace("(", "").Replace(")", "") + "/" + yearCode;
            string NewRefNumber = Problem_Category_Name + "/" 
                //+ provinceCode + "/" 
                + SeqNumber.ToString("000000000").Replace("(", "").Replace(")", "") + "/" + yearCode;

            return NewRefNumber;
        }
        #endregion

        #region CreateADOPTIONSReferenceNumber

        public string CreateADOPTIONSReferenceNumber(string yearCode, int? Problem_Category_Id, int client_ID)
        {
            string Problem_Category_Name = dbContext.Problem_Categories.Find(Problem_Category_Id).Source;

            string NewRefNumber = "ADOPT/12/8" + "/" + client_ID + "/" + yearCode;

            return NewRefNumber;
        }
        #endregion

        #region CreatePCMReferenceNumber

        public string CreatePCMReferenceNumber(string yearCode, int? Problem_Category_Id, int client_ID)
        {
            string Problem_Category_Name = dbContext.Problem_Categories.Find(Problem_Category_Id).Source;

            string NewRefNumber = "NAT/12/4" + "/" + client_ID + "/" + yearCode;

            return NewRefNumber;
        }
        #endregion


        public Client GetSpecificClient(int clientId)
        {
            Client client;

            try
            {
                var clients = (from c in dbContext.Clients
                               where c.Client_Id.Equals(clientId)
                               select c).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                client = (from c in clients
                          select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return client;
        }
        public int GetSpecificClientByPersonID(int personId)
        {

            try
            {
                int clientID = dbContext.Clients.SingleOrDefault(a => a.Person_Id == personId).Client_Id;

                return clientID;
                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                //client = (from c in clients
                //          select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return 0;
            }

            //return 0;
        }

        public IQueryable<dynamic> GetCarePlan(int id, int formType)
        {
            return (from f in dbContext.CYCA_Dynamic_Form
                         join fd in dbContext.CYCA_Dynamic_Form_Data on f.Dynamic_Form_Id equals fd.Dynamic_Form_Id
                         join u in dbContext.Users on fd.User_Id equals u.User_Id
                         join p in dbContext.Persons on fd.Client_Id equals p.Person_Id
                         join fc in dbContext.apl_Cyca_Facility on fd.Venue_Id equals fc.Facility_Id
                         where p.Person_Id == id && f.Dynamic_Form_Type_Id == formType

                         select new
                         {
                             Id = fd.Dynamic_Form_Data_Id,
                             CreatedBy = u.First_Name + " " + u.Last_Name,
                             CreatedById = u.User_Id,
                             CreatedFor = p.First_Name + " " + p.Last_Name,
                             DateCreated = fd.CreatedDate.ToString(),
                             CreatedForId = p.Person_Id,
                             Venue = fc.FacilityName,
                             File = fd.Data
                         });
        }
        public List<Client> GetListOfClients(bool showInActive, bool showDeleted)
        {
            List<Client> listOfClients;

            try
            {
                var clients = (from c in dbContext.Clients
                               where c.Is_Active || c.Is_Active.Equals(!showInActive)
                               where !c.Is_Deleted || c.Is_Deleted.Equals(showDeleted)
                               select c).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfClients = (from c in clients
                                 select c).ToList();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return null;
            }

            return listOfClients;
        }

        public int GetClientSequenceNumber(string provinceCode, string yearCode)
        {

            try
            {
                var maxId1 = (from r in dbContext.Clients
                              where r.Reference_Number != null
                              orderby r.Client_Id descending
                              select r.Client_Id).ToList();
                var newId = maxId1 == null ? 1 : maxId1.FirstOrDefault() + 1;
                return newId;

            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return -1;
            }
        }

        public int GetIntClientSequenceNumber(string provinceCode, string yearCode)
        {

            try
            {
                var maxId = (from x in dbContext.Clients.ToList()
                             where (x.Reference_Number != null && (x.Reference_Number.StartsWith("INT/" + provinceCode) && x.Reference_Number.EndsWith(yearCode)))
                             select new
                             {
                                 Id = x.Reference_Number.Substring(7, 4).Cast<int>()
                             }).ToList().OrderByDescending(x => x.Id).FirstOrDefault();

                var newId = maxId == null ? 1 : maxId.Id.First() + 1;

                return newId;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return -1;
            }
        }

        public bool? IsReferenceNumberUnique(string refNumber)
        {

            try
            {
                var itemsFound = (from x in dbContext.Clients
                                  where x.Reference_Number.Equals(refNumber)
                                  select x).Count();

                return itemsFound == 0;
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return null;
            }
        }

        public Client CreateClient(int? personId, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            Client newClient;

         
                var client = new Client()
                {
                    Person_Id = personId,
                    Date_Created = dateCreated,
                    Created_By = createdBy,
                    Is_Active = isActive,
                    Is_Deleted = isDeleted
                };

                try
                {
                    newClient = dbContext.Clients.Add(client);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            

            return newClient;
        }

        public Client AssignClientReferenceNumber(int clientId, string referenceNumber)
        {
            Client editClient;


            try
            {
                editClient = (from c in dbContext.Clients
                              where c.Client_Id.Equals(clientId)
                              select c).FirstOrDefault();

                if (editClient == null) return null;

                editClient.Reference_Number = referenceNumber;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editClient;
        }

        public Client EditPerson(int clientId, int? personId, string referenceNumber, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Client editClient;

           
                try
                {
                    editClient = (from c in dbContext.Clients
                                  where c.Client_Id.Equals(clientId)
                                  select c).FirstOrDefault();

                    if (editClient == null) return null;

                    editClient.Reference_Number = referenceNumber;
                    editClient.Person_Id = personId;
                    editClient.Date_Last_Modified = dateLastModified;
                    editClient.Modified_By = modifiedBy;
                    editClient.Is_Active = isActive;
                    editClient.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }

            return editClient;
        }

        public Client AddAdoptiveParent(int clientId, int adoptiveParentId)
        {
            Client editClient;

           
                try
                {
                    editClient = (from c in dbContext.Clients
                                  where c.Client_Id.Equals(clientId)
                                  select c).FirstOrDefault();

                    if (editClient == null) return null;

                    var addAdoptiveParent = (from a in dbContext.Client_Adoptive_Parents
                                             where a.Client_Adoptive_Parent_Id.Equals(adoptiveParentId)
                                             select a).FirstOrDefault();

                    if (addAdoptiveParent == null) return null;

                    editClient.Client_Adoptive_Parents.Add(addAdoptiveParent);

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }

            return editClient;
        }
    }
}
