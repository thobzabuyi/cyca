using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ClientBiologicalParentModel
    {
        public Client_Biological_Parent GetSpecificClientBiologicalParent(int clientBiologicalParentId)
        {
            Client_Biological_Parent clientBiologicalParent;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var clientBiologicalParentList = (from r in dbContext.Client_Biological_Parents
                                                  where r.Client_Biological_Parent_Id.Equals(clientBiologicalParentId)
                                                  select r).ToList();

                clientBiologicalParent = (from r in clientBiologicalParentList
                                          select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return clientBiologicalParent;
        }

        public List<Client_Biological_Parent> GetListOfClientBiologicalParents()
        {
            List<Client_Biological_Parent> clientBiologicalParents;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var clientBiologicalParentList = (from r in dbContext.Client_Biological_Parents
                                                      select r).ToList();

                    clientBiologicalParents = (from r in clientBiologicalParentList
                                               select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return clientBiologicalParents;
        }

        public Client_Biological_Parent CreateClientBiologicalParent(int clientId, int personId, bool? isDeceased, DateTime? dateDeceased, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var biologicalParent = new Client_Biological_Parent() { Client_Id = clientId, Person_Id = personId, Is_Deceased = isDeceased, Date_Deceased = dateDeceased, Date_Created = dateCreated, Created_By = createdBy, Is_Active = isActive, Is_Deleted = isDeleted };

            try
            {
                var newBiologicalParent = dbContext.Client_Biological_Parents.Add(biologicalParent);

                dbContext.SaveChanges();

                return newBiologicalParent;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Client_Biological_Parent EditClientBiologicalParent(int itemId, int personId, bool? isDeceased, DateTime? dateDeceased, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Client_Biological_Parent editBiologicalParent;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editBiologicalParent = (from a in dbContext.Client_Biological_Parents
                                            where a.Client_Biological_Parent_Id.Equals(itemId)
                                            select a).FirstOrDefault();

                    if (editBiologicalParent == null) return null;

                    editBiologicalParent.Person_Id = personId;
                    editBiologicalParent.Is_Deceased = isDeceased;
                    editBiologicalParent.Date_Deceased = dateDeceased;
                    editBiologicalParent.Is_Active = isActive;
                    editBiologicalParent.Is_Deleted = isDeleted;
                    editBiologicalParent.Date_Last_Modified = dateLastModified;
                    editBiologicalParent.Modified_By = modifiedBy;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editBiologicalParent;
        }
    }
}
