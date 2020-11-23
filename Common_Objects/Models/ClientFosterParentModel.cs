using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ClientFosterParentModel
    {
        public Client_Foster_Parent GetSpecificClientFosterParent(int clientFosterParentId)
        {
            Client_Foster_Parent clientFosterParent;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var clientFosterParentList = (from r in dbContext.Client_Foster_Parents
                                              where r.Client_Foster_Parent_Id.Equals(clientFosterParentId)
                                              select r).ToList();

                clientFosterParent = (from r in clientFosterParentList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return clientFosterParent;
        }

        public List<Client_Foster_Parent> GetListOfClientFosterParents()
        {
            List<Client_Foster_Parent> clientFosterParents;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var clientFosterParentList = (from r in dbContext.Client_Foster_Parents
                                                  select r).ToList();

                    clientFosterParents = (from r in clientFosterParentList
                                           select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return clientFosterParents;
        }

        public Client_Foster_Parent CreateClientFosterParent(int clientId, int personId, bool? isDeceased, DateTime? dateDeceased, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var fosterParent = new Client_Foster_Parent() { Client_Id = clientId, Person_Id = personId, Is_Deceased = isDeceased, Date_Deceased = dateDeceased, Date_Created = dateCreated, Created_By = createdBy, Is_Active = isActive, Is_Deleted = isDeleted };

            try
            {
                var newFosterParent = dbContext.Client_Foster_Parents.Add(fosterParent);

                dbContext.SaveChanges();

                return newFosterParent;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Client_Foster_Parent EditClientFosterParent(int itemId, int personId, bool? isDeceased, DateTime? dateDeceased, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Client_Foster_Parent editFosterParent;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editFosterParent = (from a in dbContext.Client_Foster_Parents
                                        where a.Client_Foster_Parent_Id.Equals(itemId)
                                        select a).FirstOrDefault();

                    if (editFosterParent == null) return null;

                    editFosterParent.Person_Id = personId;
                    editFosterParent.Is_Deceased = isDeceased;
                    editFosterParent.Date_Deceased = dateDeceased;
                    editFosterParent.Is_Active = isActive;
                    editFosterParent.Is_Deleted = isDeleted;
                    editFosterParent.Date_Last_Modified = dateLastModified;
                    editFosterParent.Modified_By = modifiedBy;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editFosterParent;
        }

    }
}
