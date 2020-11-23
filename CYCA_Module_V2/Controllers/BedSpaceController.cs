using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using Newtonsoft.Json;


namespace CYCA_Module_V2.Controllers
{
    public class BedSpaceController : Controller
    {
        private readonly CYCABedSpaceModel bedSpaceModel = new CYCABedSpaceModel();
        // GET: CYCABedSpace
        public ActionResult Index()
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
            var StatList = bedSpaceModel.GetBedStats();
            ViewBag.RequestStatusList = new SelectList(StatList, "Request_Status_Id", "Description");

            var request_Outcomes = bedSpaceModel.GetBedSpaceRequestOutcome();
            ViewBag.RequestOutcomeList = new SelectList(request_Outcomes, "Outcome_Id", "Description");

            return PartialView("Index");
        }

        #region GET FACILITY BED SPACE CAPACITY

        public ActionResult GetFacilitySpacedetails()
        {
            int intassid = Convert.ToInt32(Session["IntakeassId"]);
            // get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;
            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = 0;

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

            CYCABedSpaceRequestViewModel assVM = new CYCABedSpaceRequestViewModel();
            assVM.Province_List = bedSpaceModel.GetAllProvinces();
            ViewBag.provinceList = new SelectList(bedSpaceModel.GetAllProvincesFullInfo(), "Province_Id", "Description");
            assVM.AdmissionType_List = bedSpaceModel.GetAdmissionType();
            assVM.RequestStatus_List = bedSpaceModel.GetBedSpaceRequeststatus();

            ViewBag.AdmissionReasonList = new SelectList(bedSpaceModel.GetAdmissionTypeList(), "Admission_Type_Id", "Description");

            ViewBag.RequeststatusList = new SelectList(bedSpaceModel.GetBedStats(), "Request_Status_Id", "Description");


            return PartialView(assVM);
        }

        public JsonResult ListFacility()
        {
            int caseid = Convert.ToInt32(Session["IntakeassId"]);

            //initialise view model
            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();

            List<CYCABedSpaceRequestViewModel> List = bedSpaceModel.GetFacilityFormaloadList().Select(x => new CYCABedSpaceRequestViewModel
            {
                Facility_Id = x.Facility_Id,
                ProvinceDescription = x.ProvinceDescription,
                SelectFacility = x.SelectFacility,
                FacilityTell = x.FacilityTell,
                Facilityemail = x.Facilityemail,
                FacilityOfficialCapacity = x.FacilityOfficialCapacity
            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ListFacilitybyProvince(int id)
        {
            int caseid = Convert.ToInt32(Session["IntakeassId"]);
            //initialise view model
            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();

            List<CYCABedSpaceRequestViewModel> List = bedSpaceModel.GetFacilityList(id).Select(x => new CYCABedSpaceRequestViewModel
            {
                Facility_Id = x.Facility_Id,
                ProvinceDescription = x.ProvinceDescription,
                SelectFacility = x.SelectFacility,
                FacilityTell = x.FacilityTell,
                Facilityemail = x.Facilityemail,
                FacilityOfficialCapacity = x.FacilityOfficialCapacity
            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListMaleSpace()
        {
            var currentUser = (User)Session["CurrentUser"];           
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;             
            }
            int caseid = Convert.ToInt32(Session["IntakeassId"]);
            //initialise view model
            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();
            int Facility = bedSpaceModel.GetFacilityIdByUserID(userId);

            List<CYCABedSpaceRequestViewModel> List = bedSpaceModel.GetMaleSpaceList(Facility).Select(x => new CYCABedSpaceRequestViewModel
            {
                Facility_Id = x.Facility_Id,
                Male_Total_Space = x.Male_Total_Space,
                Male_Available_Space = x.Male_Available_Space,
                Male_Used_Space = x.Male_Used_Space
            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ListFemaleSpace()
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            int caseid = Convert.ToInt32(Session["IntakeassId"]);
            
            //initialise view model
            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();
            int Facility = bedSpaceModel.GetFacilityIdByUserID(userId);

            List<CYCABedSpaceRequestViewModel> List = bedSpaceModel.GetFemaleSpaceList(Facility).Select(x => new CYCABedSpaceRequestViewModel
            {
                Facility_Id = x.Facility_Id,
                Female_Total_Space = x.Female_Total_Space,
                Female_Available_Space = x.Female_Available_Space,
                Female_Used_Space = x.Female_Used_Space

            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListProgrammes(int fid)
        {
            int caseid = Convert.ToInt32(Session["IntakeassId"]);
            
            //initialise view model
            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();

            List<CYCABedSpaceRequestViewModel> List = bedSpaceModel.GetFacilityProgramList(fid).Select(x => new CYCABedSpaceRequestViewModel
            {

                ProgramNames = x.ProgramNames,
                ProgramDescription = x.ProgramDescription,
                ProgramStartDate = x.ProgramStartDate,
                ProgramEndDate = x.ProgramEndDate,
                ProgramCapacity = x.ProgramCapacity
            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFacilityById(int Id)
        {
                CYCABedSpaceRequestViewModel vm = new CYCABedSpaceRequestViewModel();
                vm.Province_List = bedSpaceModel.GetAllProvinces();
                ViewBag.provinceList = new SelectList(bedSpaceModel.GetAllProvincesFullInfo(), "Province_Id", "Description");
                vm.AdmissionType_List = bedSpaceModel.GetAdmissionType();
                vm.RequestStatus_List = bedSpaceModel.GetBedSpaceRequeststatus();

                ViewBag.AdmissionReasonList = new SelectList(bedSpaceModel.GetAdmissionTypeList(), "Admission_Type_Id", "Description");

                ViewBag.RequeststatusList = new SelectList(bedSpaceModel.GetBedStats(), "Request_Status_Id", "Description");
                vm = bedSpaceModel.GetFacilityMailList(Id);
                string value = string.Empty;
                value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GET BED SPACE REQUESTS
            
        public ActionResult BedSpaceRequest()
        {   
            return PartialView();
        }

        public JsonResult GetBedSpaceList()
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

            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();
            ViewBag.OffenceCategory = new SelectList(bedSpaceModel.GetBedSpaceRequestOutcome(), "Outcome_Id", "Description");
            int Facility = bedSpaceModel.GetFacilityIdByUserID(userId);
            List<CYCABedSpaceRequestViewModel> lis = new List<CYCABedSpaceRequestViewModel>();
            lis.AddRange(bedSpaceModel.GetFacilitybedSpaceListByFacility(Facility).Select(x => new CYCABedSpaceRequestViewModel
            {
                Request_Id = x.Request_Id,
                Date_Created = x.Date_Created,
                selectPropationOfficer = x.selectPropationOfficer,
                courtName = x.courtName,
                Date_Recieved = x.Date_Recieved,
                Time_Recieved = x.Time_Recieved,
                RequestOpenClose = x.RequestOpenClose,
                Days_Lapsed = x.Days_Lapsed,
                selectBedRequestStatus = x.selectBedRequestStatus,
                Request_Status_Id = x.Request_Status_Id,
                Hours_Lapsed = x.Hours_Lapsed,
                Hours_Modified = x.Hours_Modified,
                Count_Accepted = x.Count_Accepted,
                Count_Declined = x.Count_Declined
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        //Populate Request Details Modal
        public JsonResult GetRequestById(int RequestId)
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

            CYCABedSpaceRequestViewModel vm = new CYCABedSpaceRequestViewModel();            
            vm = bedSpaceModel.GetRequestById(RequestId, userId);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }
                   
        //[HttpPost]
        //public JsonResult AcceptDeclineBedspace(CYCABedSpaceRequestViewModel assVM, int id, string acceptdecline)
        //{
        //    int intassid = Convert.ToInt32(Session["IntakeassId"]);
        //    // get current username
        //    string loginName = User.Identity.Name;
        //    Session["LoginName"] = loginName;
        //    var currentUser = (User)Session["CurrentUser"];
        //    var userProvince = -1;
        //    var userId = 0;
        //    if (currentUser != null)
        //    {
        //        userId = currentUser.User_Id;
        //        if (currentUser.Employees.Any())
        //        {
        //            userProvince = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
        //        }
        //        if (currentUser.apl_Social_Worker.Any())
        //        {
        //            userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
        //        }
        //    }
        //    CYCABedSpaceModel assModel = new CYCABedSpaceModel();


        //    var result = false;
        //    if (id != 0)
        //    {

        //        try
        //        {

        //            assModel.UpdateFacilitybedSpaceAcceptDetails(assVM, id, userId, Convert.ToInt32(assVM.Outcome_Id));
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            //Log errror
        //        }
        //    }

        //    //return "failed";

        //    return Json(result, JsonRequestBehavior.AllowGet);     
        //}

        public JsonResult UpdateBedSpaceInDatabase(CYCABedSpaceRequestViewModel vm, string acceptdecline)
        {
            int Id = Convert.ToInt32(vm.Request_Id);
            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            var currentUser = (User)Session["CurrentUser"];
            var userProvince = -1;
            var userId = 0;

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

            var result = false;
            int caseid = Convert.ToInt32(Session["IntakeassId"]);
            int FacilityId = bedSpaceModel.GetFacilityIdByUserID(userId);
            int fbId = bedSpaceModel.GetFemaleBedspaceIdByFacilityId(FacilityId);
            //int AvailableFemaleSpace = Model.GetFemaleAvailableSpaceByFacilityId(fbId);
            //int AvailableMaleSpace = Model.GetMaleAvailableSpaceByFacilityId(fbId);

            //ViewBag.FemaleSpace = AvailableFemaleSpace;
            //ViewBag.MaleSpace = AvailableMaleSpace;


            try
            {
                if (vm.Request_Id > 0)
                {

                    bedSpaceModel.UpdateFemaleMaleSpaceCapacity(vm, FacilityId, userId, fbId, Convert.ToInt32(vm.Outcome_Id));
                    bedSpaceModel.UpdateFacilitybedSpaceAcceptDetails(vm, Id, userId, Convert.ToInt32(vm.Outcome_Id));
                        result = true;                                                             
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GET ARCHIVES      
        public JsonResult GetArchives()
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
            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();
            ViewBag.OffenceCategory = new SelectList(bedSpaceModel.GetBedSpaceRequestOutcome(), "Outcome_Id", "Description");
            int Facility = bedSpaceModel.GetFacilityIdByUserID(userId);
            List<CYCABedSpaceRequestViewModel> lis = new List<CYCABedSpaceRequestViewModel>();
            lis.AddRange(bedSpaceModel.GetFacilitybedSpaceListByFacility(Facility).Select(x => new CYCABedSpaceRequestViewModel
            {
                Request_Id = x.Request_Id,
                selectPropationOfficer = x.selectPropationOfficer,
                courtName = x.courtName,
                Date_Recieved = x.Date_Recieved,
                Time_Recieved = x.Time_Recieved,
                RequestOpenClose = x.RequestOpenClose,
                Days_Lapsed = x.Days_Lapsed,
                selectBedRequestStatus = x.selectBedRequestStatus,
                Request_Status_Id = x.Request_Status_Id,
                Hours_Lapsed = x.Hours_Lapsed,
                Count_Accepted = x.Count_Accepted,
                Count_Declined = x.Count_Declined
            }).ToList());

            //List<CYCABedSpaceRequestViewModel> lis = model.GetFacilitybedSpaceList(Facity).Select(x => new CYCABedSpaceRequestViewModel
            //{
            //    Request_Id = x.Request_Id,
            //    selectPropationOfficer = x.selectPropationOfficer,
            //    courtName = x.courtName,
            //    Date_Recieved = x.Date_Recieved,
            //    Time_Recieved = x.Time_Recieved,
            //    RequestOpenClose = x.RequestOpenClose,
            //    Days_Lapsed = x.Days_Lapsed,
            //    selectBedRequestStatus = x.selectBedRequestStatus,
            //    Request_Status_Id = x.Request_Status_Id
            //}).ToList();
            return Json(lis, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PROVINCIAL COORDINATOR

        public ActionResult ProvCoordinator()
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
            var StatList = bedSpaceModel.GetBedStats(); ;
            ViewBag.RequestStatusList = new SelectList(StatList, "Request_Status_Id", "Description");

            var request_Outcomes = bedSpaceModel.GetBedSpaceRequestOutcome();
            ViewBag.RequestOutcomeList = new SelectList(request_Outcomes, "Outcome_Id", "Description");
            return PartialView("_ProvincialCoordinator");
        }
        public JsonResult GetBedSpaceListDeclinedByCenterManager()
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

            CYCABedSpaceRequestViewModel VM = new CYCABedSpaceRequestViewModel();
            ViewBag.OffenceCategory = new SelectList(bedSpaceModel.GetBedSpaceRequestOutcome(), "Outcome_Id", "Description");
            int Facility = bedSpaceModel.GetFacilityIdByUserID(userId);
            List<CYCABedSpaceRequestViewModel> lis = new List<CYCABedSpaceRequestViewModel>();
            lis.AddRange(bedSpaceModel.GetBedSpaceListDeclinedByCenterManager().Select(x => new CYCABedSpaceRequestViewModel
            {
                Request_Id = x.Request_Id,
                Date_Created = x.Date_Created,
                selectPropationOfficer = x.selectPropationOfficer,
                courtName = x.courtName,
                Date_Recieved = x.Date_Recieved,
                Time_Recieved = x.Time_Recieved,
                RequestOpenClose = x.RequestOpenClose,
                Days_Lapsed = x.Days_Lapsed,
                selectBedRequestStatus = x.selectBedRequestStatus,
                Request_Status_Id = x.Request_Status_Id,
                Hours_Lapsed = x.Hours_Lapsed
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}