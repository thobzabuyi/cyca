using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ClientFamilyMemberModel
    {
        public Client_Family_Member GetSpecificClientFamilyMember(int clientFamilyMemberId)
        {
            Client_Family_Member clientFamilyMember;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var clientFamilyMemberList = (from r in dbContext.Client_Family_Members
                                              where r.Client_Family_Member_Id.Equals(clientFamilyMemberId)
                                              select r).ToList();

                clientFamilyMember = (from r in clientFamilyMemberList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return clientFamilyMember;
        }

        public List<Client_Family_Member> GetListOfClientFamilyMembers()
        {
            List<Client_Family_Member> clientFamilyMembers;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var clientFamilyMemberList = (from r in dbContext.Client_Family_Members
                                                  select r).ToList();

                    clientFamilyMembers = (from r in clientFamilyMemberList
                                           select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return clientFamilyMembers;
        }

        public Client_Family_Member CreateClientFamilyMember(int clientId, int personId, int? relationshipTypeId, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var familyMember = new Client_Family_Member() { Client_Id = clientId, Person_Id = personId, Relationship_Type_Id = relationshipTypeId, Date_Created = dateCreated, Created_By = createdBy, Is_Active = isActive, Is_Deleted = isDeleted };

            try
            {
                var newCaregiver = dbContext.Client_Family_Members.Add(familyMember);

                dbContext.SaveChanges();

                return newCaregiver;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Client_Family_Member EditClientFamilyMember(int itemId, int personId, int? relationshipTypeId, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Client_Family_Member editCaregiver;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editCaregiver = (from a in dbContext.Client_Family_Members
                                     where a.Client_Family_Member_Id.Equals(itemId)
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
