using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisServiceStatusModel
    {
        public NISIS_Service_Status GetSpecificServiceStatus(int serviceStatusId)
        {
            NISIS_Service_Status serviceStatus;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var serviceStatusList = (from r in dbContext.NISIS_Service_Statusses
                                         where r.Service_Status_Id.Equals(serviceStatusId)
                                         select r).ToList();

                serviceStatus = (from r in serviceStatusList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return serviceStatus;
        }

        public List<NISIS_Service_Status> GetListOfServiceStatusses()
        {
            List<NISIS_Service_Status> serviceStatusses;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var serviceStatusList = (from r in dbContext.NISIS_Service_Statusses
                                             orderby r.Service_Status_Id
                                             select r).ToList();

                    serviceStatusses = (from r in serviceStatusList
                                        select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return serviceStatusses;
        }
    }
}
