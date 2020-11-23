using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisOwnerOrganizationModel
    {
        public NISIS_Owner_Organization GetSpecificOwnerOrganization(int ownerOrganizationId)
        {
            NISIS_Owner_Organization ownerOrganization;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var ownerOrganizationList = (from r in dbContext.NISIS_Owner_Organizations
                                             where r.Owner_Organization_Id.Equals(ownerOrganizationId)
                                             select r).ToList();

                ownerOrganization = (from r in ownerOrganizationList
                                     select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return ownerOrganization;
        }

        public List<NISIS_Owner_Organization> GetListOfOwnerOrganizations()
        {
            List<NISIS_Owner_Organization> ownerOrganizations;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var ownerOrganizationList = (from r in dbContext.NISIS_Owner_Organizations
                                                 select r).ToList();

                    ownerOrganizations = (from r in ownerOrganizationList
                                          select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return ownerOrganizations;
        }
    }
}
