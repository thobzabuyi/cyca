using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class ProspectiveAdoptiveParentModel
    {
        public int_Client_ProspectiveAdoptiveParents GetSpecificProspectiveAdoptiveParent(int prospectiveAdoptiveParentId)
        {
            int_Client_ProspectiveAdoptiveParents prospectiveAdoptiveParents;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var prospectiveAdoptiveParentList = (from r in dbContext.int_Client_ProspectiveAdoptiveParents
                                           where r.Client_ProspectiveAdoptiveParents_Id.Equals(prospectiveAdoptiveParentId)
                                           select r).ToList();

                prospectiveAdoptiveParents = (from r in prospectiveAdoptiveParentList
                                              select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return prospectiveAdoptiveParents;
        }

        public List<int_Client_ProspectiveAdoptiveParents> GetListOfClientProspectiveAdoptiveParents()
        {
            List<int_Client_ProspectiveAdoptiveParents> clientProspectiveAdoptiveParents;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var prospectiveAdoptiveParentsList = (from r in dbContext.int_Client_ProspectiveAdoptiveParents
                                               select r).ToList();

                    clientProspectiveAdoptiveParents = (from r in prospectiveAdoptiveParentsList
                                                        select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return clientProspectiveAdoptiveParents;
        }

        public int_Client_ProspectiveAdoptiveParents CreateClientProspectiveAdoptiveParents(int clientId, int personId, int? relationshipTypeId, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var ProspectiveAdoptiveParent = new int_Client_ProspectiveAdoptiveParents() { Client_Id = clientId, Person_Id = personId, Relationship_Type_Id = relationshipTypeId, Date_Created = dateCreated, Created_By = createdBy, Is_Active = isActive, Is_Deleted = isDeleted };

            try
            {
                var newProspectiveAdoptiveParent = dbContext.int_Client_ProspectiveAdoptiveParents.Add(ProspectiveAdoptiveParent);

                dbContext.SaveChanges();

                return newProspectiveAdoptiveParent;
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

        public int_Client_ProspectiveAdoptiveParents EditClientProspectiveAdoptiveParents(int itemId, int personId, int? relationshipTypeId, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            int_Client_ProspectiveAdoptiveParents editProspectiveAdoptiveParent;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editProspectiveAdoptiveParent = (from a in dbContext.int_Client_ProspectiveAdoptiveParents
                                     where a.Client_ProspectiveAdoptiveParents_Id.Equals(itemId)
                                     select a).FirstOrDefault();

                    if (editProspectiveAdoptiveParent == null) return null;

                    editProspectiveAdoptiveParent.Person_Id = personId;
                    editProspectiveAdoptiveParent.Relationship_Type_Id = relationshipTypeId;
                    editProspectiveAdoptiveParent.Is_Active = isActive;
                    editProspectiveAdoptiveParent.Is_Deleted = isDeleted;
                    editProspectiveAdoptiveParent.Date_Last_Modified = dateLastModified;
                    editProspectiveAdoptiveParent.Modified_By = modifiedBy;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editProspectiveAdoptiveParent;
        }


    }
}
