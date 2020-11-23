using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ClientAdoptiveParentModel
    {
        public Client_Adoptive_Parent GetSpecificClientAdoptiveParent(int clientAdoptiveParentId)
        {
            Client_Adoptive_Parent clientAdoptiveParent;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var clientAdoptiveParentList = (from r in dbContext.Client_Adoptive_Parents
                                                where r.Client_Adoptive_Parent_Id.Equals(clientAdoptiveParentId)
                                                select r).ToList();

                clientAdoptiveParent = (from r in clientAdoptiveParentList
                                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return clientAdoptiveParent;
        }

        public List<Client_Adoptive_Parent> GetListOfClientAdoptiveParents()
        {
            List<Client_Adoptive_Parent> clientAdoptiveParents;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var clientAdoptiveParentList = (from r in dbContext.Client_Adoptive_Parents
                                                    select r).ToList();

                    clientAdoptiveParents = (from r in clientAdoptiveParentList
                                             select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return clientAdoptiveParents;
        }

        public Client_Adoptive_Parent CreateClientAdoptiveParent(int clientId, int personId, bool? isDeceased, DateTime? dateDeceased, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var adoptiveParent = new Client_Adoptive_Parent() { Client_Id = clientId, Person_Id = personId, Is_Deceased = isDeceased, Date_Deceased = dateDeceased, Date_Created = dateCreated, Created_By = createdBy, Is_Active = isActive, Is_Deleted = isDeleted };

            try
            {
                var newAdoptiveParent = dbContext.Client_Adoptive_Parents.Add(adoptiveParent);

                dbContext.SaveChanges();

                return newAdoptiveParent;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Client_Adoptive_Parent EditClientAdoptiveParent(int itemId, int personId, bool? isDeceased, DateTime? dateDeceased, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Client_Adoptive_Parent editAdoptiveParent;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editAdoptiveParent = (from a in dbContext.Client_Adoptive_Parents
                                          where a.Client_Adoptive_Parent_Id.Equals(itemId)
                                          select a).FirstOrDefault();

                    if (editAdoptiveParent == null) return null;

                    editAdoptiveParent.Person_Id = personId;
                    editAdoptiveParent.Is_Deceased = isDeceased;
                    editAdoptiveParent.Date_Deceased = dateDeceased;
                    editAdoptiveParent.Is_Active = isActive;
                    editAdoptiveParent.Is_Deleted = isDeleted;
                    editAdoptiveParent.Date_Last_Modified = dateLastModified;
                    editAdoptiveParent.Modified_By = modifiedBy;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editAdoptiveParent;
        }
    }
}
