using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class EmployeeRolesModel
    {
        public bool CreateEmployeeJobPosition(int employeeId, int positionId, int createdBy)
        {            
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employeeJobPosition = new EmployeeRole();
                employeeJobPosition.Employee_Id = employeeId;
                employeeJobPosition.JobPosition_Id = positionId;
                employeeJobPosition.Created_By = createdBy;

                dbContext.EmployeeRoles.Add(employeeJobPosition);
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public List<EmployeeRole> GetEmployeeJobPosition(int employeeId)
        {
            
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
               return dbContext.EmployeeRoles.Where(a => a.Employee_Id.Equals(employeeId)).ToList();                
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        public bool DeleteEmployeeJobPositions(int employeeId)
        {            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    dbContext.EmployeeRoles.RemoveRange(dbContext.EmployeeRoles.Where(a => a.Employee_Id == employeeId).ToList());
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

    }
}

