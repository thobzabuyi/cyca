using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDIIS.Models
{
    public class ServiceOfficeModel
    {
        public Service_Office GetSpecificServiceOffice(int serviceOfficeId)
        {
            Service_Office serviceOffice;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var serviceOfficeList = (from r in dbContext.Service_Offices
                                         where r.Service_Office_Id.Equals(serviceOfficeId)
                                         select r).ToList();

                serviceOffice = (from r in serviceOfficeList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return serviceOffice;
        }

        public List<Service_Office> GetListOfServiceOffices()
        {
            List<Service_Office> serviceOffices;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var serviceOfficeList = (from r in dbContext.Service_Offices
                                             select r).ToList();

                    serviceOffices = (from r in serviceOfficeList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return serviceOffices;
        }
    }
}