using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
   public class CareWorkerModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public List<User> GetCareWorkers(int facilityID, int employee_Id)
        {
            var users = db.Users;
            var emps = db.Employees.Where(e => e.Facility_Id == facilityID && e.CYCA_Supervisor == employee_Id)
                                   .Select(e => e.User_Id);
            var workers = users.Where(u => emps.Contains(u.User_Id)).ToList();
            return workers;
         
            /*  var workers = (from u in db.Users
                           join e in db.Employees on u.User_Id equals e.User_Id
                           where e.Facility_Id == facilityID && e.CYCA_Supervisor == employee_Id
                           select new CareWorker()
                           {
                               FacilityId = facilityID,
                               Name = u.First_Name + " " + u.Last_Name,
                               Desciption = "Care Worker",
                               Summary = "Summary",
                               UserId = u.User_Id
                           }).ToList(); 
            return workers;*/
            
        }
    }
}
