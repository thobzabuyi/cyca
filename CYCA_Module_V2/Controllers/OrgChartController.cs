using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using Newtonsoft.Json;
using System.Web.Security;
using System.Web.Helpers;
using CYCA_Module_V2.Common_Objects;
using Common_Objects;
using System.Configuration;

namespace CYCA_Module_V2.Controllers
{
   
    public class OrgChartController : Controller
    {
        public static List<FlatObject> CircularList { get; set; }
        public static List<FlatObject> CurrentList { get; set; }
        private readonly OrgChartModel orgChart = new OrgChartModel();
        public ActionResult OrgChart()
        {
            return PartialView("_Index");
        }
        [HttpGet]
        public JsonResult GetOrgStructure()
        {
            var currentUser = new User();
            if ((Session["CurrentUser"] == null) && (Request.Cookies[FormsAuthentication.FormsCookieName] != null))
            {
                var authUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

                var userModel = new UserModel();
                currentUser = userModel.GetSpecificUser(authUser);
            }
            else
            {
                if (Session["CurrentUser"] != null)
                {
                    var loggedInUser = (User)Session["CurrentUser"];

                    var userModel = new UserModel();
                    currentUser = userModel.GetSpecificUser(loggedInUser.User_Id);
                }
            }
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = orgChart.GetFacilityIdByUserID(userId);
            string teamLeader = ConfigurationManager.AppSettings["TeamLeaderRole"];
            string facilityManager = ConfigurationManager.AppSettings["CenterManagerRole"];
            string careWorker = ConfigurationManager.AppSettings["CareWorkerRole"];

            using (SDIIS_DatabaseEntities _context = new SDIIS_DatabaseEntities())
            {
                List<TempUser> usersAll = (from u in _context.Users
                                          join e in _context.Employees on u.User_Id equals e.User_Id
                                          join f in _context.apl_Cyca_Facility on e.Facility_Id equals f.Facility_Id
                                          where e.Facility_Id == facilityID &&
                                          (u.Roles.Any(r => r.Description == teamLeader) ||
                                          u.Roles.Any(r => r.Description == facilityManager) ||
                                          u.Roles.Any(r => r.Description == careWorker))
                                          select new TempUser
                                          {
                                              user = u,
                                              emp = e
                                          }).ToList();
                List<FlatObject> users = (from u in usersAll
                                          select new FlatObject()
                                          {
                                              Name = u.user.First_Name + " " + u.user.Last_Name,
                                              Id = u.emp.Employee_Id,
                                              ParentId = u.emp.CYCA_Supervisor ?? 0,
                                              Facility = String.Join(" ", u.user.Roles.Where(r => r.Description.Contains("CYCA")).Select(r => r.Description).ToArray()).Replace("CYCA-","")
                                          }
                                         ).Distinct().ToList();

                foreach (FlatObject u in users)
                {
                    if (u.ParentId == -1)
                    {
                        u.ParentId = 0;
                    }
                    if (u.Facility.Contains(facilityManager.Replace("CYCA-","")))
                    {
                        u.className = "FacilityManager";
                    }
                }
                var filteredUsers = users.ToList();
                List<FlatObject> childRecords = new List<FlatObject>();
                List<FlatObject> parentRecords = new List<FlatObject>();
                List<FlatObject> siblingRecords = new List<FlatObject>();

                
                    //Get All Child Records
                    foreach (FlatObject u in filteredUsers)
                    {
                        childRecords.AddRange(GetAllChildren(u.Id, users, 0));
                        parentRecords.AddRange(GetAllParent(u.ParentId, users, 0));
                        siblingRecords.AddRange(users.Where(uu => uu.ParentId == u.ParentId && uu.ParentId > 0));
                    }
                    filteredUsers.AddRange(childRecords);
                    filteredUsers.AddRange(parentRecords);
                    filteredUsers.AddRange(siblingRecords);

                
                var final = filteredUsers.Distinct().ToList();

                //Get All Parent Records


                var recursiveObjects = FillRecursive(final, 0,1);

                var overallParent = new RecursiveObject()
                {
                    Id = 0,
                    Name = "0. All",
                    Facility = "Department of Social Development",
                    ParentId = -1,
                    Role = "",
                    Level = "0",
                    children = new List<RecursiveObject>()
                };

                //Get All Parent Records
                if (CircularList != null)
                {
                    if (CircularList.Count > 0)
                    {
                        overallParent.Circulars = new List<FlatObject>();
                        overallParent.Circulars.AddRange(CircularList);
                    }

                }

                overallParent.children.AddRange(recursiveObjects);
                return Json(overallParent, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveStructure(RecursiveObject org)
        {
            var currentUser = new User();
            if ((Session["CurrentUser"] == null) && (Request.Cookies[FormsAuthentication.FormsCookieName] != null))
            {
                var authUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

                var userModel = new UserModel();
                currentUser = userModel.GetSpecificUser(authUser);
            }
            else
            {
                if (Session["CurrentUser"] != null)
                {
                    var loggedInUser = (User)Session["CurrentUser"];

                    var userModel = new UserModel();
                    currentUser = userModel.GetSpecificUser(loggedInUser.User_Id);
                }
            }
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = orgChart.GetFacilityIdByUserID(userId);

            List<FlatObject> fos = new List<FlatObject>();
            foreach (RecursiveObject child in org.children)
            {
                fos.AddRange(UpdateStruct(org.Id, child));
            }

            using (SDIIS_DatabaseEntities _context = new SDIIS_DatabaseEntities())
            {
                var users = _context.Employees.Where(e=>e.Facility_Id==facilityID).ToList();
                foreach (FlatObject fo in fos)
                {
                    if (fo.Id > 0)
                    {
                        var u = users.Where(uu => uu.Employee_Id == fo.Id).Single();
                        if (u.CYCA_Supervisor != fo.ParentId && fo.ParentId >= 0)
                        {
                            if (fo.ParentId > 0)
                            {
                                u.CYCA_Supervisor = fo.ParentId;
                            }
                            else
                            {
                                u.CYCA_Supervisor = null;
                            }
                            _context.SaveChanges();
                        }
                    }

                }
            }
            return Json(new { success = "success"});
        }
        private List<FlatObject> UpdateStruct(int ParentId, RecursiveObject obj)
        {
            var fos = new List<FlatObject>();
            fos.Add(new FlatObject()
            {
                Id = obj.Id,
                ParentId = ParentId
            });
            obj.ParentId = ParentId;
            foreach (RecursiveObject child in obj.children)
            {
                fos.AddRange(UpdateStruct(obj.Id, child));
            }
            return fos;
        }
        private static List<RecursiveObject> FillRecursive(List<FlatObject> flatObjects, int parentId,int level)
        {
            var recursiveObjects = new List<RecursiveObject>();
            foreach (var item in flatObjects.Where(x => x.ParentId.Equals(parentId)))
            {
                recursiveObjects.Add(new RecursiveObject
                {
                    Name = level.ToString() +". "+ item.Name,
                    Facility = item.Facility,
                    Role = item.Role,
                    Id = item.Id,
                    ParentId = item.ParentId,
                    className = item.className,
                    Level = level.ToString(),
                    children = FillRecursive(flatObjects, item.Id,level+1)
                });
            }
            return recursiveObjects;
        }
        private static List<FlatObject> GetAllChildren(int ParentId, List<FlatObject> allUsers, int nestedLevel)
        {
            var returnList = new List<FlatObject>();
            if (nestedLevel < 5)
            {
                List<FlatObject> childList = new List<FlatObject>();
                childList.AddRange(allUsers.Where(u => u.ParentId == ParentId).ToList());
                returnList.AddRange(childList);
                foreach (FlatObject child in childList)
                {
                    returnList.AddRange(GetAllChildren(child.Id, allUsers, nestedLevel + 1));
                }
            }
            return returnList;
        }
        private static List<FlatObject> GetAllParent(int ParentId, List<FlatObject> allUsers, int nestedLevel)
        {
            if (nestedLevel == 0)
            {
                CurrentList = new List<FlatObject>();
            }
            List<FlatObject> returnList = new List<FlatObject>();
            if (nestedLevel < 10)
            {
                List<FlatObject> childList = new List<FlatObject>();
                childList.AddRange(allUsers.Where(u => u.Id == ParentId).ToList());
                returnList.AddRange(childList);
                foreach (FlatObject child in childList)
                {
                    CurrentList.Add(child);
                    returnList.AddRange(GetAllParent(child.ParentId, allUsers, nestedLevel + 1));
                }
            }
            else
            {
                //Issue populate another list
                CircularList = new List<FlatObject>();
                CircularList.AddRange(CurrentList);
            }

            return returnList;
        }
   
    }
    public class TempUser
    {
        public User user { get; set; }
        public Employee emp { get; set; }
    }
    public class FlatObject
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Facility { get; set; }
        public string Role { get; set; }
        public string className { get; set; }
        public string level { get; set; }
        //public FlatObject(string name, int id, int parentId,string province, string role)
        //{
        //    Name = name;
        //    Id = id;
        //    ParentId = parentId;
        //    Province = province;
        //    Role = role;
        //}
    }
    public class RecursiveObject
    {
        public RecursiveObject()
        {
            this.children = new List<RecursiveObject>();
            this.Circulars = new List<FlatObject>();

        }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Facility { get; set; }
        public string className { get; set; }
        public string Role { get; set; }
        public string Level { get; set; }
        public List<RecursiveObject> children { get; set; }
        public List<FlatObject> Circulars { get; set; }

    }

}