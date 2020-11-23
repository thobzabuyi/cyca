using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ClientCareGiverModel
    {
        public Client_CareGiver GetSpecificClientCareGiver(int clientCareGiverId)
        {
            Client_CareGiver clientCareGiver;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var clientCareGiverList = (from r in dbContext.Client_CareGivers
                                           where r.Client_Caregiver_Id.Equals(clientCareGiverId)
                                           select r).ToList();

                clientCareGiver = (from r in clientCareGiverList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return clientCareGiver;
        }

        public List<Client_CareGiver> GetListOfClientCareGivers()
        {
            List<Client_CareGiver> clientCareGivers;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var clientCareGiverList = (from r in dbContext.Client_CareGivers
                                               select r).ToList();

                    clientCareGivers = (from r in clientCareGiverList
                                        select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return clientCareGivers;
        }

        public Client_CareGiver CreateClientCaregiver(int clientId, int personId, int? relationshipTypeId, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var caregiver = new Client_CareGiver() { Client_Id = clientId, Person_Id = personId, Relationship_Type_Id = relationshipTypeId, Date_Created = dateCreated, Created_By = createdBy, Is_Active = isActive, Is_Deleted = isDeleted };

            try
            {
                var newCaregiver = dbContext.Client_CareGivers.Add(caregiver);

                dbContext.SaveChanges();

                return newCaregiver;
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

        public Client_CareGiver EditClientCaregiver(int itemId, int personId, int? relationshipTypeId, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Client_CareGiver editCaregiver;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editCaregiver = (from a in dbContext.Client_CareGivers
                                     where a.Client_Caregiver_Id.Equals(itemId)
                                     select a).FirstOrDefault();

                    if (editCaregiver == null) return null;

                    editCaregiver.Person_Id = personId;
                    editCaregiver.Relationship_Type_Id = relationshipTypeId;
                    editCaregiver.Is_Active = isActive;
                    editCaregiver.Is_Deleted = isDeleted;
                    editCaregiver.Date_Last_Modified = dateLastModified;
                    editCaregiver.Modified_By = modifiedBy;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editCaregiver;
        }
    }
}
