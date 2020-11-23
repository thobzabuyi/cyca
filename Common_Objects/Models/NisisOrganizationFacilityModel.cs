using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisOrganizationFacilityModel
    {
        public NISIS_Organization_Facility GetSpecificOrganizationFacility(int organizationFacilityId)
        {
            NISIS_Organization_Facility organizationFacility;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var organizationFacilityList = (from r in dbContext.NISIS_Organization_Facilities
                                                where r.Organization_Facility_Id.Equals(organizationFacilityId)
                                                select r).ToList();

                organizationFacility = (from r in organizationFacilityList
                                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return organizationFacility;
        }

        public List<NISIS_Organization_Facility> GetListOfOrganizationFacilities()
        {
            List<NISIS_Organization_Facility> OrganizationFacilities;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var organizationFacilityList = (from r in dbContext.NISIS_Organization_Facilities
                                                    select r).ToList();

                    OrganizationFacilities = (from r in organizationFacilityList
                                              select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return OrganizationFacilities;
        }

        public List<NISIS_Organization_Facility> GetListOfOrganizationFacilities(int ownerOrganizationFacilityId)
        {
            List<NISIS_Organization_Facility> OrganizationFacilities;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var organizationFacilityList = (from r in dbContext.NISIS_Organization_Facilities
                                                    where r.Owner_Organization_Id.Equals(ownerOrganizationFacilityId)
                                                    select r).ToList();

                    OrganizationFacilities = (from r in organizationFacilityList
                                              select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return OrganizationFacilities;
        }
    }
}
