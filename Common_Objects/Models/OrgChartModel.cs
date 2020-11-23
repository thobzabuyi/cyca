using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class OrgChartModel
    {
        private readonly SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public OrgChartModel()
        { 
        }
        public int GetFacilityIdByUserID(int UserId)
        {
            return (from f in db.apl_Cyca_Facility
                    join e in db.Employees on f.Facility_Id equals e.Facility_Id
                    join u in db.Users on e.User_Id equals u.User_Id
                    where u.User_Id == UserId
                    select f.Facility_Id).SingleOrDefault();
        }
        public void GetAllTempUsers()
        {
            
        }
    }
}
