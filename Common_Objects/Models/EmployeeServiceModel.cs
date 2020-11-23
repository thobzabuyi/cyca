using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class EmployeeServiceModel
    {
        public EmployeeService CreateEmployeeService(int employeeId, int serviceId, int loggedUserId)
        {
            EmployeeService employeeService = new EmployeeService();

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                employeeService.Employee_Id = employeeId;
                employeeService.Problem_Category_Id = serviceId;
                employeeService.Modified_By = loggedUserId;
                dbContext.EmployeeServices.Add(employeeService);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return employeeService;
        }

        public Problem_Category GetSpecificEmployeeService(int employeeServiceId)
        {
            Problem_Category employeeService;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employeeServiceList = (from r in dbContext.Problem_Categories
                                    where r.Problem_Category_Id.Equals(employeeServiceId)
                                    select r).ToList();

                employeeService = (from r in employeeServiceList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return employeeService;
        }

        public List<EmployeeService> EmployeeService(int employeeId)
        {

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var EmployeeServiceList = dbContext.EmployeeServices.Where(a => a.Employee_Id == employeeId).ToList();


                return EmployeeServiceList;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }


        }

        public List<Problem_Category> GetListOfEmployeeServices()
        {
            List<Problem_Category> employeeServices;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var employeeServiceList = (from r in dbContext.Problem_Categories
                                        select r).ToList();

                    employeeServices = (from r in employeeServiceList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return employeeServices;
        }

        public bool DeleteEmployeeServices(int employeeId)
        {

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    //List<Problem_Category> employeeServices;

                    var employeeServicesList = dbContext.EmployeeServices.Where(a => a.Employee_Id == employeeId).ToList();

                    dbContext.EmployeeServices.RemoveRange(employeeServicesList);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public int Delete(int employeeId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var employeeServicesRecord = dbContext.EmployeeServices.Where(a => a.Employee_Id == employeeId);
                dbContext.EmployeeServices.RemoveRange(employeeServicesRecord);
                dbContext.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

       
    }
}
