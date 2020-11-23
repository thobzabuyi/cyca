using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisServiceModel
    {
        public NISIS_Service GetSpecificService(int serviceId)
        {
            NISIS_Service service;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var serviceList = (from r in dbContext.NISIS_Services
                                   where r.Service_Id.Equals(serviceId)
                                   select r).ToList();

                service = (from r in serviceList
                           select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return service;
        }

        public List<NISIS_Service> GetListOfServices()
        {
            List<NISIS_Service> services;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var serviceList = (from r in dbContext.NISIS_Services
                                       select r).ToList();

                    services = (from r in serviceList
                                select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return services;
        }

        public List<NISIS_Service> GetListOfServices(int serviceCategoryId)
        {
            List<NISIS_Service> services;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var serviceList = (from x in dbContext.NISIS_Services
                                       where x.Service_Category_Id.Equals(serviceCategoryId)
                                       select x).ToList();

                    services = (from x in serviceList
                                select x).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return services;
        }
    }
}
