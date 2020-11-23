using Common_Objects;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using CYCA_Module_V2.Common_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;

namespace CYCA_Module_V2.Controllers
{
    public class HomeController : Controller
    {

        private readonly ChildrenModel children = new ChildrenModel();
        private readonly UserModel userModel = new UserModel();
        private readonly TeamLeaderModel teamLeaders = new TeamLeaderModel();
        private readonly EmployeeModel employee = new EmployeeModel();
        private readonly CYCAdmissionModel cYCAAdmissions = new CYCAdmissionModel();
        private readonly CYCAChildAllocationModel model = new CYCAChildAllocationModel();
        private readonly CYCABedSpaceModel bedSpaceModel = new CYCABedSpaceModel();
        private readonly CYCA_UserRoleViewModel userRoleViewModel = new CYCA_UserRoleViewModel();
        private readonly VenueModel venues = new VenueModel();
        private readonly CareWorkerModel careworker = new CareWorkerModel();
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        [CustomAuthorize("CYCA", "Home", "Index")]
        public ActionResult Index()
        {
            //return RedirectToAction("Index", "Intake");
            var currentUser = new User();
            string facilityname = "";



            if ((Session["CurrentUser"] == null) && (Request.Cookies[FormsAuthentication.FormsCookieName] != null))
            {
                var authUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                currentUser = userModel.GetSpecificUser(authUser);
                ViewBag.CurrentUserId = currentUser.User_Id;
                var res = db.apl_User_Image.Where(ur => ur.User_Id == currentUser.User_Id).Select(us => us.Image_Content).SingleOrDefault();
                if (res.Length > 0)
                {
                    ViewBag.CurrentUserImage = Convert.ToBase64String(res);
                }
                Session.Remove("CurrentUser");
                Session.Remove("MenuLayout");
                Session.Add("CurrentUser", currentUser);
            }
            else
            {
                if (Session["CurrentUser"] != null)
                {
                    var loggedInUser = (User)Session["CurrentUser"];

                    var userModel = new UserModel();
                    currentUser = userModel.GetSpecificUser(loggedInUser.User_Id);
                    ViewBag.CurrentUserId = currentUser.User_Id;
                    var res = db.apl_User_Image.Where(ur => ur.User_Id == currentUser.User_Id).Select(us => us.Image_Content).SingleOrDefault();
                    if (res.Length > 0)
                    {
                        ViewBag.CurrentUserImage = Convert.ToBase64String(res);
                    }
                    facilityname = userModel.GetFacilityByUserId(loggedInUser.User_Id);
                }
            }
            var authorizedRoles = new List<Role>();
            List<Role> myRoles = new List<Role>();
            authorizedRoles = currentUser.Roles.Where(rr=>rr.Description.Contains("CYCA")).ToList();
            CYCARoleType defaultRole = CYCARoleType.Other;
            string teamLeader = ConfigurationManager.AppSettings["TeamLeaderRole"];
            string facilityManager = ConfigurationManager.AppSettings["CenterManagerRole"];
            string careWorker = ConfigurationManager.AppSettings["CareWorkerRole"];
            bool hasFacility = false;
            bool hasTeamLeader = false;
            bool hasCareWorker = false;
            foreach (Role r in authorizedRoles)
            {
                if (r.Description.ToLower() == facilityManager.ToLower())
                {
                    myRoles.Add(new Role()
                    {
                        Description = "Center Manager",
                        Role_Id = r.Role_Id
                    });
                    ViewBag.RoleId = r.Role_Id;
                    defaultRole = CYCARoleType.FacilityManager;
                    hasFacility = true;
                }
                else if (r.Description.ToLower() == teamLeader.ToLower())
                {
                    myRoles.Add(new Role()
                    {
                        Description = "Team Leader",
                        Role_Id = r.Role_Id
                    });
                    if (!hasFacility)
                    {
                        ViewBag.RoleId = r.Role_Id;
                        defaultRole = CYCARoleType.TeamLeader;
                        hasTeamLeader = true;
                    }
                }else if(r.Description.ToLower() == careWorker.ToLower())
                {
                    myRoles.Add(new Role()
                    {
                        Description = "Care Worker",
                        Role_Id = r.Role_Id
                    });
                    if (!hasFacility && !hasTeamLeader)
                    {
                        ViewBag.RoleId = r.Role_Id;
                        defaultRole = CYCARoleType.CareWorker;
                        hasCareWorker = true;
                    }
                }else
                {
                    myRoles.Add(new Role()
                    {
                        Description = r.Description,
                        Role_Id = r.Role_Id
                    });
                    if (!hasFacility && !hasTeamLeader && !hasCareWorker)
                    {
                        ViewBag.RoleId = r.Role_Id;
                        defaultRole = CYCARoleType.Other;
                    }
                }
            }
            ViewBag.Roles = new SelectList(myRoles, "Role_Id", "Description", ViewBag.RoleId);
            ViewBag.DefaultRole = defaultRole;
            ViewBag.FacilityName = facilityname;
            
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MenuLayout()
        {
            var currentUser = new User();

            if ((Session["CurrentUser"] == null) && (Request.Cookies[FormsAuthentication.FormsCookieName] != null))
            {
                var authUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

                var userModel = new UserModel();
                currentUser = userModel.GetSpecificUser(authUser);

                Session.Remove("CurrentUser");
                Session.Remove("MenuLayout");
                Session.Add("CurrentUser", currentUser);
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

            var renderMenu = new Menu();

            if (Session["MenuLayout"] == null)
            {
                var menuModel = new MenuModel();
                var menu = menuModel.GetSpecificMenu((int)MenuContainerEnum.CYCAMenu);

                var authorizedRoles = new List<Role>();

                authorizedRoles = currentUser.Roles.ToList();

                if (currentUser.Groups.Any())
                {
                    foreach (var group in currentUser.Groups)
                    {
                        var groupRoles = group.Roles.Select(r => r).ToList();
                        authorizedRoles.AddRange(groupRoles);
                    }
                }

                // TODO Add delegation here as well

                var effectiveRoles = authorizedRoles.Distinct().ToList();

                var menuItems = menu.Menu_Items.ToList();

                Helpers.SetAuthorizedRolesVisibility(ref menuItems, effectiveRoles);

                renderMenu = menu.Menu_Items.Any() ? new Menu { Menu_Items = new List<Menu_Item>(menuItems) } : null;

                Session["MenuLayout"] = renderMenu;
            }
            else
            {
                renderMenu = (Menu)Session["MenuLayout"];
            }

            return PartialView("_MenuLayout", renderMenu);
        }

        public ActionResult CareWorker()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }         

            var venue_list = venues.GetAplCycaVenues();
            ViewBag.VenueList = new SelectList(venue_list, "Venue_Id", "VenueName");

            var userList = userModel.getUserCYCAList();
            ViewBag.UsersList = new SelectList(userList, "User_Id", "fullname");

            var status_list = children.GetTransferStatusList();

            ViewBag.StatusList = new SelectList(status_list, "Tansfer_Status_Id", "Description");

            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            return View(vm);
        }
    
        public JsonResult SearchChild()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;


            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }

            // get client list

            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();


            List<CYCAChildAllocationViewModel> lis = new List<CYCAChildAllocationViewModel>();
            lis.AddRange(model.GetPersonListByUserId(userId).Select(x =>
            new CYCAChildAllocationViewModel
            {
                Child_Allocation_Id = x.Child_Allocation_Id,
                Child_First_Name = x.Child_First_Name,
                Child_Last_First_Name = x.Child_Last_First_Name,
                Child_ID_No = x.Child_ID_No,
                LoggedInUserName = x.LoggedInUserName,
                Person_Id = x.Person_Id
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        #region MOVEMENTS...
        //Get Care Worker in the Venue
        public JsonResult GetUserVenueId(int venueId)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }

            // get client list
           
           
            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            //int Facility = model.employee.GetFacilityIdByUserID(userId);
            //int venueId = model.GetVenueIdByFacilityId(Facility);

            List<CYCAChildAllocationViewModel> lis = new List<CYCAChildAllocationViewModel>();
            lis.AddRange(model.GetUserByVenueId(venueId).Select(x => new CYCAChildAllocationViewModel
            {
                Movement_Id = x.Movement_Id,
                CareWorker_Name = x.CareWorker_Name,
                Checked_In_Date = x.Checked_In_Date,
                Person_Id = x.Person_Id,
                Child_Name = x.Child_Name
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        //Populate Child Details Modal
        public JsonResult GetChildByAllocationId(int AllocationId)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;


            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }


            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            //int empid = model.GetBedSpaceRequestById(RequestId);



            vm = model.GetChildByAllocationId(AllocationId);
            string value = string.Empty;
            ViewBag.ChildAllocationId = AllocationId;

            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        //Add Child Movement To Database      
        public JsonResult MoveChild(List<CYCAChildMovementTransfer> cVM)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];
            //var userProvince = -1;
            var userId = 0;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var result = false;
            var facilityId = employee.GetFacilityIdByUserID(userId);
            
           //var 
            CYCAChildAllocationModel assModel = new CYCAChildAllocationModel();
            try
            {
                foreach(CYCAChildMovementTransfer m in cVM)
                {
                    var admissionId = cYCAAdmissions.GetActiveAdmissionID(m.Person_Id, facilityId);
                    m.Admission_Id = admissionId;
                    assModel.SaveChildMovement(m, userId);
                }
                result = true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                //Log errror
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region TRANSFER...
        //Get Care Worker in the Venue
        public JsonResult GetChildByUserId(int User_Id)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }

            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            //int Facility = model.employee.GetFacilityIdByUserID(userId);
            //int venueId = model.GetVenueIdByFacilityId(Facility);

            List<CYCAChildAllocationViewModel> lis = new List<CYCAChildAllocationViewModel>();
            lis.AddRange(model.GetPersonListByUserId(User_Id).Select(x => new CYCAChildAllocationViewModel
            {
                Transfer_Id = x.Transfer_Id,
                Child_Name = x.Child_Name,
                Date_Transferred = x.Date_Transferred,
                GangMembership = x.GangMembership


            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        //Populate Child Details Modal
        public JsonResult GetPersonByAllocationId(int AllocationId)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;


            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }


            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            //int empid = model.GetBedSpaceRequestById(RequestId);


            vm = model.GetPersonByAllocationId(AllocationId);
            string value = string.Empty;


            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        //Add Child Transfer To Database      
        public JsonResult TransferChild(List<CYCAChildMovementTransfer> cVM)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            var result = false;
            var facilityId = employee.GetFacilityIdByUserID(userId);
            CYCAChildAllocationModel assModel = new CYCAChildAllocationModel();

            try
            {
                foreach(CYCAChildMovementTransfer m in cVM)
                {
                    var admissionId = cYCAAdmissions.GetActiveAdmissionID(m.Person_Id, facilityId);
                    m.Admission_Id = admissionId;
                    assModel.AddChildTransferToDatabase(m, userId);
                }
                
                result = true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                //Log errror
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Get Child Transferred by logged in User
        public JsonResult ChildTransferredByUser()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }

            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            List<CYCAChildAllocationViewModel> lis = new List<CYCAChildAllocationViewModel>();
            lis.AddRange(model.GetTransferredPersonByTransferredBy(userId).Select(x => new CYCAChildAllocationViewModel
            {
                Transfer_Id = x.Transfer_Id,
                Child_First_Name = x.Child_First_Name,
                Transferred_By = x.Transferred_By,
                selectedTransferStatus = x.selectedTransferStatus,
                Person_Id = x.Person_Id
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }
        //Get Child Transferred by logged in User
        public JsonResult ChildTransferredToUser()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                if (currentUser.Employees.Any())
                {
                    userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
                if (currentUser.apl_Social_Worker.Any())
                {
                    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }
            }

            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel();

            List<CYCAChildAllocationViewModel> lis = new List<CYCAChildAllocationViewModel>();
            lis.AddRange(model.GetTransferredPersonByTransferredTo(userId).Select(x => new CYCAChildAllocationViewModel
            {
                Transfer_Id = x.Transfer_Id,
                Child_First_Name = x.Child_First_Name,
                Transferred_By = x.Transferred_By,
                selectedTransferStatus = x.selectedTransferStatus,
                Person_Id = x.Person_Id
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        //Populate Transfer Modal
        public JsonResult GetTransferById(int TransferId)
        {
            CYCAChildAllocationViewModel vm = new CYCAChildAllocationViewModel(); 

            vm = model.GetTransferById(TransferId);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTransferandAllocationInDatabase(CYCAChildAllocationViewModel vm)
        {
            int TransferId = Convert.ToInt32(vm.Transfer_Id);
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var result = false;
            try
            {

            var ca = children.getChildAllocation(TransferId);
            var t = children.GetChildTranfer(TransferId);
              
            switch(vm.selectedTransferStatus)
            {
                case "Accept":
                    t.Tansfer_Status_Id = 2;
                    ca.User_Id = userId;
                    ca.Date_Transferred = DateTime.Now;
                    ca.Sent_By = t.Transferred_By;
                    break;
                case "Reject":
                    t.Tansfer_Status_Id = 3;
                    t.Decline_Reason = vm.Decline_Reason;
                    break;
            }
          
            children.SaveChildrenChanges();
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public PartialViewResult ShowLeftContent(CYCARoleType type)
        {
            object viewModel;
            CYCA_CareWorkerViewModel model = new CYCA_CareWorkerViewModel();
            CYCA_CenterManagerViewModel modelCenterManager = new CYCA_CenterManagerViewModel();
            CYCA_TeamLeaderViewModel modelTeamLeader = new CYCA_TeamLeaderViewModel();
            switch (type)
            {
                case CYCARoleType.FacilityManager:
                    modelCenterManager = GetCenterManagerChildren();
                    modelCenterManager.TransferCount = modelCenterManager.childrenTransferByMe.Count() + modelCenterManager.childrenTransferToMe.Count();
                    viewModel = modelCenterManager;
                    return PartialView("~/Views/Home/_CenterManagerLeft.cshtml", viewModel);
                case CYCARoleType.TeamLeader:
                    modelTeamLeader = GetTeamLeaderChildren();
                    modelTeamLeader.TransferCount = modelTeamLeader.childrenTransferByMe.Count() + modelTeamLeader.childrenTransferToMe.Count();
                    viewModel = modelTeamLeader;
                    return PartialView("~/Views/Home/_TeamLeaderLeft.cshtml", viewModel);
                case CYCARoleType.ProvincialCoordinator:
                    //modelCenterManager = GetCenterManagerChildren();
                    //modelCenterManager.TransferCount = modelCenterManager.childrenTransferByMe.Count() + modelCenterManager.childrenTransferToMe.Count();
                    viewModel = modelCenterManager;
                    return PartialView("~/Views/Home/_ProvincialCoordinatorLeft.cshtml", viewModel);
                case CYCARoleType.CareWorker:
                default:
                    model = GetCareWorkerChildren();
                    model.TransferCount = model.childrenTransferByMe.Count() + model.childrenTransferToMe.Count();
                    viewModel = model;
                    return PartialView("~/Views/Home/_CareWorkerLeft.cshtml",viewModel);
            }
        }
        public PartialViewResult ShowRightContent( CYCARoleType type)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;


            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            object viewModel;

            switch (type)
            {
                case CYCARoleType.FacilityManager:
                    CYCA_CenterManagerRightViewModel returnModel = new CYCA_CenterManagerRightViewModel();
                    //returnModel = GetTeamLeadersAndChildren();
                    //returnModel = GetCenterManagerRightView();
                    returnModel = GetCenterManagerRightViewUpdated();
                    viewModel = returnModel;
                    return PartialView("~/Views/Home/_CenterManagerRight.cshtml",viewModel);
                case CYCARoleType.TeamLeader:
                    CYCA_TeamLeaderRightViewModel returnModelTeamLeader = new CYCA_TeamLeaderRightViewModel();
                    returnModelTeamLeader = GetCareWorkersAndChildren();;
                    viewModel = returnModelTeamLeader;
                    return PartialView("~/Views/Home/_TeamLeaderRight.cshtml", viewModel);
                case CYCARoleType.ProvincialCoordinator:                    
                    CYCABedSpaceRequestViewModel returnModelProv = new CYCABedSpaceRequestViewModel();
                    var StatList = bedSpaceModel.GetBedStats();
                    ViewBag.RequestStatusList = new SelectList(StatList, "Request_Status_Id", "Description");

                    var request_Outcomes = bedSpaceModel.GetBedSpaceRequestOutcome();
                    ViewBag.RequestOutcomeList = new SelectList(request_Outcomes, "Outcome_Id", "Description");

                    return PartialView("~/Views/Home/_ProvincialCoordinatorRight.cshtml");
                case CYCARoleType.CareWorker:
                default:
                    var venue_list = venues.GetAplCycaVenues();
                    ViewBag.VenueList = new SelectList(venue_list, "Venue_Id", "VenueName");
                    var userList = userModel.GetUserListById(userId);
                    ViewBag.UsersList = new SelectList(userList, "User_Id", "fullname");
                    return PartialView("~/Views/Home/_CareWorkerRight.cshtml");
            }
        }
        public CYCA_CareWorkerViewModel GetCareWorkerChildren()
        {
            CYCA_CareWorkerViewModel returnModel = new CYCA_CareWorkerViewModel();
            returnModel.children = new List<CYCAChildAllocationViewModel>();
            returnModel.childrenTransferByMe = new List<CYCAChildAllocationViewModel>();
            returnModel.childrenTransferToMe = new List<CYCAChildAllocationViewModel>();
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);
            // get client list
            var ListP = children.getListP(facilityID, userId);
          
            //Get Transfer Status = 1 and Transferred By User
            var ListByMe = children.getListByMe(userId, facilityID);

            var ListToMe = children.getListToMe(userId, facilityID);

            var finalChildrenByMe = ListP.Where(p=>!ListByMe.Any(x=>x.Person_Id==p.Person_Id)).ToList();
            var finalChildrenToMe = finalChildrenByMe.Where(p => !ListToMe.Any(x => x.Person_Id == p.Person_Id)).ToList();
            returnModel.children.AddRange(finalChildrenToMe);
            returnModel.childrenTransferByMe.AddRange(ListByMe);
            returnModel.childrenTransferToMe.AddRange(ListToMe);
            return returnModel;
        }
        public CYCA_CenterManagerViewModel GetCenterManagerChildren()
        {
            CYCA_CenterManagerViewModel returnModel = new CYCA_CenterManagerViewModel();
            returnModel.children = new List<CYCAChildAllocationViewModel>();
            returnModel.childrenTransferByMe = new List<CYCAChildAllocationViewModel>();
            returnModel.childrenTransferToMe = new List<CYCAChildAllocationViewModel>();
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);

            //view child class
        
            var ListP = children.getActiveChildrenWithNoAssignments(facilityID, currentUser);
            //Remove Children Assigned to me from left view

            //var ListP = children.getListP(facilityID, userId);
            //ListP.AddRange(ACtiveChildrenWithNoAssignmments);

            //Get Transfer Status = 1 and Transferred By User
            var ListByMe = children.getListByMe(userId, facilityID);
            var ListToMe = children.getListToMe(userId, facilityID);
        
            var finalChildrenByMe = ListP.Where(p => !ListByMe.Any(x => x.Person_Id == p.Person_Id)).ToList();
            var finalChildrenToMe = finalChildrenByMe.Where(p => !ListToMe.Any(x => x.Person_Id == p.Person_Id)).ToList();
            returnModel.children.AddRange(finalChildrenToMe);
            returnModel.childrenTransferByMe.AddRange(ListByMe);
            returnModel.childrenTransferToMe.AddRange(ListToMe);
            return returnModel;
        }
        public CYCA_TeamLeaderViewModel GetTeamLeaderChildren()
        {
            CYCA_TeamLeaderViewModel returnModel = new CYCA_TeamLeaderViewModel();
            returnModel.children = new List<CYCAChildAllocationViewModel>();
            returnModel.childrenTransferByMe = new List<CYCAChildAllocationViewModel>();
            returnModel.childrenTransferToMe = new List<CYCAChildAllocationViewModel>();
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);
            var ListP = children.getListPForTeamLeaderChildren(userId, facilityID);
   
            //Get Transfer Status = 1 and Transferred By User
            var ListByMe = children.getListByMe(userId, facilityID);
            //Get Transfer Status = 1 and Transferred By User
            var ListToMe = children.getListToMe(userId, facilityID);

            var finalChildrenByMe = ListP.Where(p => !ListByMe.Any(x => x.Person_Id == p.Person_Id)).ToList();
            var finalChildrenToMe = finalChildrenByMe.Where(p => !ListToMe.Any(x => x.Person_Id == p.Person_Id)).ToList();
            returnModel.children.AddRange(finalChildrenToMe);
            returnModel.childrenTransferByMe.AddRange(ListByMe);
            returnModel.childrenTransferToMe.AddRange(ListToMe);
            return returnModel;
        }

        public CYCA_CenterManagerRightViewModel GetCenterManagerRightViewUpdated()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);
            string teamLeader = ConfigurationManager.AppSettings["TeamLeaderRole"];
            string facilityManager = ConfigurationManager.AppSettings["CenterManagerRole"];
            string careWorker = ConfigurationManager.AppSettings["CareWorkerRole"];

            CYCA_CenterManagerRightViewModel returnModel = new CYCA_CenterManagerRightViewModel();
            returnModel.TeamLeaders = new List<TeamLeader>();

            var levelOne = teamLeaders.GetLevelOneUsers(facilityID,teamLeader,facilityManager,careWorker);
            
            foreach (TeamLeaderModelView tl in levelOne)
            {
                List<CYCAChildAllocationViewModel> allChildren = new List<CYCAChildAllocationViewModel>();
                //Get All Kids 
                allChildren.AddRange(children.getTeamLeaderChildren(tl.UserId, facilityID));
                var levelTwo = teamLeaders.GetLevelTwoUsers(facilityID, teamLeader, facilityManager, careWorker, tl.UserId);
                foreach(TeamLeaderModelView tll in levelTwo)
                {
                    allChildren.AddRange(children.getTeamLeaderChildren(tll.UserId, facilityID));
                }
                var levelOneLeader = new TeamLeader()
                {
                    FacilityId = facilityID,
                    Name = tl.Name,
                    Desciption = String.Join(" ", tl.Roles.Where(r => r.Description.Contains("CYCA")).Select(r => r.Description).ToArray()).Replace("CYCA-", ""),
                    Summary = "",
                    UserId = tl.UserId,
                   
                children = new List<CYCAChildAllocationViewModel>()
                };
                var res = db.apl_User_Image.Where(ur => ur.User_Id == tl.UserId).Select(us => us.Image_Content).SingleOrDefault();
                if (res.Length > 0)
                {
                    levelOneLeader.img = Convert.ToBase64String(res);
                }
                levelOneLeader.children.AddRange(allChildren);
                returnModel.TeamLeaders.Add(levelOneLeader);
            }
            
            return returnModel;

        }
        public CYCA_CenterManagerRightViewModel GetCenterManagerRightView()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);
            string teamLeaderRole = ConfigurationManager.AppSettings["TeamLeaderRole"];
            bool isCurrentTeamLeader = currentUser.Roles.Any(r => r.Description == teamLeaderRole);

            //Get All Children in facility that are assisgned to someone except the center manager
            var allChildren = children.getChildrenAssignedOtherThanCentreManager(facilityID, userId, currentUser);


            CYCA_CenterManagerRightViewModel returnModel = new CYCA_CenterManagerRightViewModel();
            returnModel.TeamLeaders = new List<TeamLeader>();
            foreach (CYCAChildAllocationViewModel c in allChildren)
            {
                var groupingId = 0;
                //Is Assigned User a Team Leader
               
                var isTeamLeader = teamLeaders.IsTeamLeader(teamLeaderRole,facilityID, c.User_Id);
                
                if(isTeamLeader==null)
                {
                    var isSuperVisorTeamLeader = teamLeaders.IsSuperVisorTeamLeader(teamLeaderRole, facilityID, c.User_Id);
              
                    if (isSuperVisorTeamLeader == null)
                    {
                        groupingId = c.User_Id??0;
                    }
                    else
                    {
                        groupingId = isSuperVisorTeamLeader.User_Id;
                    }
                }
                else
                {
                    groupingId = isTeamLeader.User_Id;
                }
                var leader = returnModel.TeamLeaders.Where(t => t.children.Any(ch => ch.User_Id == groupingId)).SingleOrDefault();
                if (leader == null)
                {
                    var l = userModel.GetUserByGroupingID(groupingId);
                    var teamLeader = new TeamLeader()
                    {
                        FacilityId = facilityID,
                        Name = l.First_Name + " " + l.Last_Name,
                        Desciption = String.Join(" ",l.Roles.Where(r=>r.Description.Contains("CYCA")).Select(r=>r.Description).ToArray()).Replace("CYCA-", ""),
                        Summary = "",
                        UserId = l.User_Id
                    };
                    var res = db.apl_User_Image.Where(ur => ur.User_Id == l.User_Id).Select(us => us.Image_Content).SingleOrDefault();
                    if (res.Length > 0)
                    {
                        teamLeader.img = Convert.ToBase64String(res);
                    }
                    teamLeader.children = new List<CYCAChildAllocationViewModel>();
                    if(c.User_Id!=groupingId)
                    {
                        //Belongs to another user below
                        c.User_Id = groupingId;
                        c.Child_Status = "2";
                    }
                    else
                    {
                        c.Child_Status = "1";
                    }
                    teamLeader.children.Add(c);
                    returnModel.TeamLeaders.Add(teamLeader);
                }
                else
                {
                    leader.children.Add(c);
                }
            }
            //Get All Teamleaders Not in above
            var allTeamLeaders = teamLeaders.GetAllTeamLeadersInFacility(facilityID,teamLeaderRole);
           
            foreach(User u in allTeamLeaders)
            {
                if(!returnModel.TeamLeaders.Exists(t=>t.UserId==u.User_Id))
                {
                    var teamLeader = new TeamLeader()
                    {
                        FacilityId = facilityID,
                        Name = u.First_Name + " " + u.Last_Name,
                        Desciption = String.Join(" ", u.Roles.Where(r => r.Description.Contains("CYCA")).Select(r => r.Description).ToArray()).Replace("CYCA-", ""),
                        Summary = "",
                        UserId = u.User_Id,
                      
                        children = new List<CYCAChildAllocationViewModel>()
                    };
                    var res = db.apl_User_Image.Where(ur => ur.User_Id == u.User_Id).Select(us => us.Image_Content).SingleOrDefault();
                    if (res.Length > 0)
                    {
                        teamLeader.img = Convert.ToBase64String(res);
                    }
                    returnModel.TeamLeaders.Add(teamLeader);
                }
            }
            return returnModel;
        }
        public CYCA_CenterManagerRightViewModel GetTeamLeadersAndChildren()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);
            CYCA_CenterManagerRightViewModel returnModel = new CYCA_CenterManagerRightViewModel();
            returnModel.TeamLeaders = new List<TeamLeader>();
            //CYCA-Team Leader
            //Team Leader Role 
            
            var loggedInEmployee = employee.GetLoggedInEmployee(userId);
            string teamLeader = ConfigurationManager.AppSettings["TeamLeaderRole"];
            string facilityManager = ConfigurationManager.AppSettings["CenterManagerRole"];
            string careWorker = ConfigurationManager.AppSettings["CareWorkerRole"];

            var teamleads =teamLeaders.GetTeamleaders(facilityID, userId, teamLeader, facilityManager);
            List<TeamLeader> leaderList = new List<TeamLeader>();
            foreach (User u in teamleads)
            {
                var worker = new TeamLeader();
                worker.FacilityId = facilityID;
                worker.Name = u.First_Name + " " + u.Last_Name;
                worker.Desciption = "Care Worker";
                worker.Summary = "Summary";
                worker.UserId = u.User_Id;
                var res =  db.apl_User_Image.Where(ur => ur.User_Id == u.User_Id).Select(us => us.Image_Content).SingleOrDefault();
                if (res.Length > 0)
                {
                    worker.img = Convert.ToBase64String(res);
                }

                leaderList.Add(worker);
            }

            returnModel.TeamLeaders.AddRange(leaderList);
  
            foreach (TeamLeader t in returnModel.TeamLeaders)
            {
          
                t.children = new List<CYCAChildAllocationViewModel>();
                t.children.AddRange(children.getTeamLeaderChildren(t.UserId, facilityID));
                //Get all kids below this too
                t.children.AddRange(children.getSupervisorChildren(t.UserId));
            }
            return returnModel;
        }
        public CYCA_TeamLeaderRightViewModel GetCareWorkersAndChildren()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            var facilityID = employee.GetFacilityIdByUserID(userId);
            CYCA_TeamLeaderRightViewModel returnModel = new CYCA_TeamLeaderRightViewModel();
            returnModel.CareWorkers = new List<CareWorker>();
            var loggedInEmployee = employee.GetLoggedInEmployee(userId);

           var workers = careworker.GetCareWorkers(facilityID, loggedInEmployee.Employee_Id);
            List<CareWorker> careList = new List<CareWorker>();
            foreach (User u in workers)
            {
                var worker = new CareWorker();
                worker.FacilityId = facilityID;
                worker.Name = u.First_Name + " " + u.Last_Name;
                worker.Desciption = "Care Worker";
                worker.Summary = "Summary";
                worker.UserId = u.User_Id;
                careList.Add(worker);
            }

            returnModel.CareWorkers.AddRange(careList);
            foreach (CareWorker t in returnModel.CareWorkers)
            {
                t.children = new List<CYCAChildAllocationViewModel>();
                t.children.AddRange(children.getCareWorkerChildren(facilityID, currentUser, t.UserId));
            }

            return returnModel;
        }

        
        public JsonResult AssignChildren(List<CYCAChildMovementTransfer> cVM)
        {
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];
            var userId = 0;
            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            var result = false;

            CYCAChildAllocationModel assModel = new CYCAChildAllocationModel();
            var facilityId = employee.GetFacilityIdByUserID(userId);
            
            try
            {
                foreach (CYCAChildMovementTransfer m in cVM)
                {

                    var admissionId = cYCAAdmissions.GetActiveAdmissionID(m.Person_Id,facilityId);
                    var ca = children.GetChildByAdmissionId(admissionId);
                    if (ca == null)
                    {
                        if (m.AssignedTo_Id > 0)
                        {
                            //Create New Allocation
                            CYCA_Child_Allocation allocation = new CYCA_Child_Allocation()
                            {
                                Admission_Id = admissionId,
                                Person_Id = m.Person_Id,
                                User_Id = m.AssignedTo_Id,
                                Sent_By = userId,
                                Date_Allocated = DateTime.Now,
                                Date_Created = DateTime.Now,
                                Created_By = userId.ToString(),
                                Is_Active = true,
                                Is_Deleted = false
                            };
                            model.saveChldToAllocation(allocation);
                        }
                    }
                    else
                    {
                        //Child Is Allocated
                        if (m.AssignedTo_Id == 0)
                        {
                            if (m.Role.ToLower() == "centermanager")
                            {
                                //Need to Delete Record
                                children.removeChild(children.GetChildByAllocationId(ca.Child_Allocation_Id));
                                var transfer = children.getPendingList(m.Person_Id);
                                foreach (CYCA_Child_Transfer t in transfer)
                                {
                                    t.Tansfer_Status_Id = 2;
                                }
                            }
                            else
                            {
                                //TeamLeader Assign to Logged In User
                                ca.Date_Allocated = DateTime.Now;
                                ca.Date_Last_Modified = DateTime.Now;
                                ca.Sent_By = userId;
                                ca.User_Id = userId;
                                //Set any pending transfers as rejected
                                var transfer = children.getPendingList(m.Person_Id);

                                foreach (CYCA_Child_Transfer t in transfer)
                                {
                                    t.Tansfer_Status_Id = 2;
                                }
                            }

                        }
                        else if (m.AssignedTo_Id > 0)
                        {
                            ca.Date_Allocated = DateTime.Now;
                            ca.Date_Last_Modified = DateTime.Now;
                            ca.Sent_By = userId;
                            ca.User_Id = m.AssignedTo_Id;
                            //Set any pending transfers as rejected
                            var transfer = children.getPendingList(m.Person_Id);

                            foreach (CYCA_Child_Transfer t in transfer)
                            {
                                t.Tansfer_Status_Id = 2;
                            }
                        }
                    }
                    children.SaveChildrenChanges();
                }

                result = true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                //Log errror
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}