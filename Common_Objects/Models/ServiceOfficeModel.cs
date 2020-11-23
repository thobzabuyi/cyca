using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
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

        public string GetUserSpecificServiceOffice(string username)
        {
            using (SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities())
            {
                return (from r in dbContext.Service_Offices
                        join e in dbContext.Employees on r.Service_Office_Id equals e.Service_Office_Id
                        join u in dbContext.Users on e.User_Id equals u.User_Id
                        where u.User_Name.Equals(username)
                        select r.Description).FirstOrDefault();
            }
            
        }

        public Service_Office CreateServiceOffice(string Description, string Municipality, string UserName)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            int munId = (from a in dbContext.Local_Municipalities
                         where a.Description == Municipality
                         select a.Local_Municipality_Id).FirstOrDefault();           
            var service_Office = new Service_Office()
            {
                Description = Description,
                Local_Municipality_Id= munId,

                Is_Active = true,
                Is_Deleted = false,
                Date_Created = DateTime.Now,
                Created_By = UserName
            };

            try
            {
                var newService_Office = dbContext.Service_Offices.Add(service_Office);

                dbContext.SaveChanges();

                return newService_Office;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public void EditServiceOffice(Service_Office Obj, string userName)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            var ServiceOffice = dbContext.Service_Offices.Find(Obj.Service_Office_Id);
            var service_Office = new Service_Office();


            service_Office.Description = Obj.Description;
            service_Office.Local_Municipality_Id = Obj.Local_Municipality_Id;

            service_Office.Is_Active = Obj.Is_Active;
            service_Office.Is_Deleted = Obj.Is_Deleted;
            service_Office.Date_Last_Modified = DateTime.Now;
            service_Office.Modified_By = userName;

            try
            {
                dbContext.SaveChanges();

            }
            catch (Exception)
            {
                 
            }
        }

    }
}