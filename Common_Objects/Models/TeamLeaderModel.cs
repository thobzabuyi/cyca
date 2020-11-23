using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.ViewModels;

namespace Common_Objects.Models
{
   public class TeamLeaderModel
    {
        public TeamLeaderModel()
        { 
        }
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        //Is Assigned User a Team Leader
        public User IsTeamLeader(string teamLeaderRole, int facilityID, int? currentUser)
        {

            var isTeamLeader = (from u in db.Users
                                join e in db.Employees on u.User_Id equals e.User_Id
                                where e.Facility_Id == facilityID && u.User_Id == currentUser &&
                                u.Roles.Count(r => r.Description == teamLeaderRole) > 0
                                select u).SingleOrDefault();
            return isTeamLeader;
        }
        public User IsSuperVisorTeamLeader(string teamLeaderRole, int facilityID, int? currentUser)
        {
           User  isSuperVisorTeamLeader = (from u in db.Users
                                          join e in db.Employees on u.User_Id equals e.User_Id
                                          join sup in db.Employees on e.CYCA_Supervisor equals sup.Employee_Id
                                          join supUser in db.Users on sup.User_Id equals supUser.User_Id
                                          where e.Facility_Id == facilityID && u.User_Id == currentUser &&
                                          supUser.Roles.Count(r => r.Description == teamLeaderRole) > 0
                                          select supUser).SingleOrDefault();
            return isSuperVisorTeamLeader;
        }
        public List<User> GetTeamleaders(int facilityID, int userId, string teamLeader, string facilityManager)
        {
            var users = db.Users;
            var emps = db.Employees.Where(e => e.Facility_Id == facilityID)
                                   .Select(e => e.User_Id);
            var workers = users.Where(u => emps.Contains(u.User_Id)
                                        && u.Roles.Count(r => r.Description == teamLeader || r.Description == facilityManager) > 0
                                        && u.User_Id != userId).ToList();
            return workers;
            /*return (from u in db.Users
                    join e in db.Employees on u.User_Id equals e.User_Id
                    where e.Facility_Id == facilityID &&
                    u.Roles.Count(r => r.Description == teamLeader || r.Description == facilityManager) > 0 &&
                    u.User_Id != userId
                    //e.CYCA_Supervisor == loggedInEmployee.Employee_Id
                    select new TeamLeader()
                    {
                        FacilityId = facilityID,
                        Name = u.First_Name + " " + u.Last_Name,
                        Desciption = "",
                        Summary = "",
                        UserId = u.User_Id
                    }).ToList();*/
        }
        public List<User> GetAllTeamLeadersInFacility(int facilityID,string teamLeaderRole)
        {
            var allTeamLeaders = (from u in db.Users
                              join e in db.Employees on u.User_Id equals e.User_Id
                              where e.Facility_Id == facilityID &&
                              u.Roles.Count(r => r.Description == teamLeaderRole) > 0
                              select u).ToList();
           
            return allTeamLeaders;
        }
        public IEnumerable<TeamLeaderModelView> getTeamleaderAndFacilityManager(int facilityID, string teamLeader, string facilityManager, int userId)
        {
            var teamleaders = (from u in db.Users
                               join e in db.Employees on u.User_Id equals e.User_Id
                               where e.Facility_Id == facilityID &&
                               u.Roles.Count(r => r.Description == teamLeader || r.Description == facilityManager) > 0 &&
                               u.User_Id != userId
                               //e.CYCA_Supervisor == loggedInEmployee.Employee_Id
                               select new TeamLeaderModelView()
                               {
                                   FacilityId = facilityID,
                                   Name = u.First_Name + " " + u.Last_Name,
                                   Desciption = "",
                                   Summary = "",
                                   UserId = u.User_Id
                               }).ToList();
            return teamleaders;
        }
        public IEnumerable<TeamLeaderModelView> GetLevelOneUsers(int facilityID, string teamLeader, string facilityManager,string careWorker)
        {
            var teamleaders = (from u in db.Users
                               join e in db.Employees on u.User_Id equals e.User_Id
                               where e.Facility_Id == facilityID &&
                               u.Roles.Count(r => r.Description == teamLeader || r.Description == facilityManager || r.Description == careWorker) >0
                               && e.CYCA_Supervisor == null
                               select new TeamLeaderModelView()
                               {
                                   FacilityId = facilityID,
                                   Name = u.First_Name + " " + u.Last_Name,
                                   Desciption = "",
                                   Summary = "",
                                   UserId = u.User_Id,
                                   Roles = u.Roles.ToList()
                               }).ToList();
            return teamleaders;
        }
        public IEnumerable<TeamLeaderModelView> GetLevelTwoUsers(int facilityID, string teamLeader, string facilityManager, string careWorker,int userId)
        {
            var loggedInEmpId = db.Employees.Where(e => e.User_Id == userId).Single();
            var teamleaders = (from u in db.Users
                               join e in db.Employees on u.User_Id equals e.User_Id
                               where e.Facility_Id == facilityID &&
                               u.Roles.Count(r => r.Description == teamLeader || r.Description == facilityManager || r.Description == careWorker) > 0
                               && e.CYCA_Supervisor == loggedInEmpId.Employee_Id
                               select new TeamLeaderModelView()
                               {
                                   FacilityId = facilityID,
                                   Name = u.First_Name + " " + u.Last_Name,
                                   Desciption = "",
                                   Summary = "",
                                   UserId = u.User_Id,
                                   Roles = u.Roles.ToList()
                               }).ToList();
            return teamleaders;
        }


    }
    public class TeamLeaderModelView
    {
        public int UserId { get; set; }
        public string Name { get; set; } 
        public byte[] Image { get; set; }
        public int FacilityId { get; set; }
        public string Summary { get; set; }
        public string Desciption { get; set; }
        public List<CYCAChildAllocationViewModel> children { get; set; }
        public List<Role> Roles { get; set; }
    }

}
