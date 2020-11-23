using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class OrganizationModel
    {
        private SDIIS_DatabaseEntities _db = new SDIIS_DatabaseEntities();
        public Organization GetSpecificOrganization(int organizationId)
        {
            Organization organization;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var organizationList = (from r in dbContext.Organizations
                                        where r.Organization_Id.Equals(organizationId)
                                        select r).ToList();

                organization = (from r in organizationList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return organization;
        }

        public Organization GetSpecificOrganizationByCode(string siteCode)
        {
            Organization organization;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var organizationList = (from r in dbContext.Organizations
                                        where r.Site_Code.Equals(siteCode)
                                        select r).ToList();

                organization = (from r in organizationList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return organization;
        }

        public List<Organization> GetListOfOrganizations(bool showInActive, bool showDeleted, string SearchDescription)
        {
            List<Organization> organizations;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var organizationList = (from r in dbContext.Organizations
                                            where r.Is_Active || r.Is_Active.Equals(!showInActive)
                                            where !r.Is_Deleted || r.Is_Deleted.Equals(showDeleted)
                                            select r).ToList();
                    if (SearchDescription != null)
                    {
                        organizationList = (from r in dbContext.Organizations
                                            where r.Is_Active || r.Is_Active.Equals(!showInActive)
                                            where !r.Is_Deleted || r.Is_Deleted.Equals(showDeleted)
                                            where r.Description.Contains(SearchDescription)
                                            select r).ToList();
                    }

                    organizations = (from r in organizationList
                                     select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return organizations;
        }

        public List<Organization> GetListOfOrganizations(bool showInActive, bool showDeleted)
        {
            List<Organization> organizations;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var organizationList = (from r in dbContext.Organizations
                                            where r.Is_Active || r.Is_Active.Equals(!showInActive)
                                            where !r.Is_Deleted || r.Is_Deleted.Equals(showDeleted)
                                            select r).ToList();

                    organizations = (from r in organizationList
                                     select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return organizations;
        }

        public List<Organization> GetListOfOrganizationsByLocalMunicipality(int localMunicipalityId)
        {
            List<Organization> organizations;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var organizationList = (from r in dbContext.Organizations
                                            where r.Local_Municipality_Id == localMunicipalityId
                                            select r).ToList();

                    organizations = (from r in organizationList
                                     select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return organizations;
        }

        public Organization CreateOrganization(string description, string telephoneNumber, string faxNumber, string emailAddress, bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var organization = new Organization()
            {
                Description = description,
                Telephone_Number = telephoneNumber,
                Fax_Number = faxNumber,
                Email_Address = emailAddress,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                var newOrganization = dbContext.Organizations.Add(organization);

                dbContext.SaveChanges();

                return newOrganization;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Organization EditOrganization(int organizationId, string description, string telephoneNumber, string faxNumber, string emailAddress, bool isActive, bool isDeleted, DateTime dateLastModified, string modifiedBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editOrganization = (from mc in dbContext.Organizations
                                        where mc.Organization_Id.Equals(organizationId)
                                        select mc).FirstOrDefault();

                if (editOrganization == null) return null;

                editOrganization.Description = description;
                editOrganization.Telephone_Number = telephoneNumber;
                editOrganization.Fax_Number = faxNumber;
                editOrganization.Email_Address = emailAddress;
                editOrganization.Is_Active = isActive;
                editOrganization.Is_Deleted = isDeleted;
                editOrganization.Date_Last_Modified = dateLastModified;
                editOrganization.Modified_By = modifiedBy;

                dbContext.SaveChanges();

                return editOrganization;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Organization SetOrganizationIsActive(int organizationId, bool isActive)
        {
            Organization editOrganization;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editOrganization = (from e in dbContext.Organizations
                                        where e.Organization_Id.Equals(organizationId)
                                        select e).FirstOrDefault();

                    if (editOrganization == null) return null;

                    editOrganization.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editOrganization;
        }

        public Organization SetOrganizationIsDeleted(int organizationId, bool isDeleted)
        {
            Organization editOrganization;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editOrganization = (from e in dbContext.Organizations
                                        where e.Organization_Id.Equals(organizationId)
                                        select e).FirstOrDefault();

                    if (editOrganization == null) return null;

                    editOrganization.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editOrganization;
        }

        public string GetUserSpecificOrganisation(string username)
        {

            using (SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities())
            {
                return (from r in dbContext.Organizations
                        join e in dbContext.Employees on r.Organization_Id equals e.Organization_Id
                        join u in dbContext.Users on e.User_Id equals u.User_Id
                        where u.User_Name.Equals(username)
                        select r.Description).FirstOrDefault();
            }
            
        }

        public IQueryable<Organization> GetOrganisationList(int Id)
        {
            return from s in _db.Organizations
                   where s.Organisation_Type_Id == Id
                   select s;
        }


    }
}
