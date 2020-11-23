using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace CYCA_Module_V2.Controllers
{
    public class AdmissionsController:Controller
    {
        private readonly ChildrenModel children = new ChildrenModel();
        //instanciate model repositry method manager
        private readonly CYCAdmissionModel admissionModel = new CYCAdmissionModel();
        private readonly VenueModel venues = new VenueModel();
        private readonly UserModel userModel = new UserModel();
        // GET: CYCAAdmission
        public ActionResult Index(int id)
        {
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

            //instanciate viewmodel
            CYCAAdmissionViewModel personVM = new CYCAAdmissionViewModel();

            //get person id by assessment id
            int personId = admissionModel.GetPCMPersonIdByintAssId(id);

            //get person record
            personVM = admissionModel.GetPCMPerson(personId);

            int ClientRefid = admissionModel.GetClientRefIdssId(id);
            //Get Module Reference number ....................
            string ClientRef = admissionModel.GetClientRef(ClientRefid);
            Session["ClientRef"] = ClientRef;
            var personIdUpdate = personId;
            if (ClientRef == null || ClientRef == "")
            {
                #region CreateReferenceNumber

                var dbContext = new SDIIS_DatabaseEntities();
                #region ObtainProvinceOfLoggedInUser
                //var currentUser = (User)Session["CurrentUser"];
                var userName = string.Empty;
                //var currentUserProvinceId = -1;
                //if (currentUser != null)
                //{
                //    userName = currentUser.User_Name;
                //}
                //if (currentUser.Employees.Any())
                //    userProvince = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                #endregion
                ClientModel clientModel = new ClientModel();
                int client_ID = dbContext.Intake_Assessments.Find(id).Client_Id;
                int module_ID = Convert.ToInt32(dbContext.Problem_Sub_Categories.Find(dbContext.Intake_Assessments.Find(id).Problem_Sub_Category_Id).Module_Id);
                var CheckExist = (from a in dbContext.int_Client_Module_Registration
                                  where a.Client_Id == client_ID && a.Module_Id == module_ID
                                  select a).ToList();
                if (CheckExist.Count() == 0)
                {
                    int ProblemCatId = dbContext.Problem_Sub_Categories.Find(dbContext.Intake_Assessments.Find(id).Problem_Sub_Category_Id).Problem_Category_Id;
                    int_Client_Module_Registration Adopt_int_Client = new int_Client_Module_Registration
                    {
                        //string ProvinceAbbr = dbContext.Provinces.Find(currentUserProvinceId).Abbreviation;
                        Client_Module_Ref_No = clientModel.CreatePCMReferenceNumber(DateTime.Now.Year.ToString(), ProblemCatId, client_ID),
                        Client_Id = client_ID,
                        Module_Id = module_ID
                    };
                    dbContext.int_Client_Module_Registration.Add(Adopt_int_Client);
                    dbContext.SaveChanges();

                    ClientRefid = admissionModel.GetClientRefIdssId(id);
                    //Get Module Reference number ....................
                    ClientRef = admissionModel.GetClientRef(ClientRefid);
                }
                #endregion

            }

            Session["ClientRef"] = ClientRef;
            ViewBag.ModuleRef = ClientRef;

            Session["PersonId"] = personIdUpdate;
            Session["Idassessment"] = id;

            return PartialView(personVM);
        }


        [HttpGet]
        public ActionResult PersonDetails()
        {
            //get current username
            string loginName = User.Identity.Name;
            Session["LoginName"] = loginName;

            //initialise model
            //initialise view model
            CYCAAdmissionViewModel personVM = new CYCAAdmissionViewModel();
            // Load Dropdown List calling the method in the model
            var personIdUpdate = Convert.ToInt32(Session["PersonId"]);
            var Idassessment = Convert.ToInt32(Session["Idassessment"]);

            return PartialView("Assessment", personVM);
        }

        public ActionResult Admission()
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
            string ClientRef = Convert.ToString(Session["ClientRef"]);
            ViewBag.ModuleRef = ClientRef;

            CYCAAdmissionViewModel assVM = new CYCAAdmissionViewModel();


            return PartialView("Admission", assVM);
        }





        #region ADMISSION 
        public ActionResult SaveAdmissionDetailsNew()
        {

            var admission_Types = admissionModel.GetAplAdmissionTypes();
            ViewBag.AdmissionTypeList = new SelectList(admission_Types, "Admission_Type_Id", "Description");

            var cyca_Venues = venues.GetAplCycaVenues();

            ViewBag.VenueList = new SelectList(cyca_Venues, "Venue_Id", "VenueName");

            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();


            return View(VM);
        }

        public JsonResult GetAdmissionDetailsList(int childId)
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

            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
            int ClientRef = admissionModel.GetClientRefByAssId(intAssessmentId);
            List<CYCAAdmissionViewModel> lis = new List<CYCAAdmissionViewModel>();
            lis.AddRange(admissionModel.GetAdmissionListByPCMClientModuleId(ClientRef).Select(x => new CYCAAdmissionViewModel
            {
                Admission_Id = x.Admission_Id,
                CaseStartDate = x.CaseStartDate,
                CaseEndDate = x.CaseEndDate,
                selectedAdmissionType = x.selectedAdmissionType,
                selectedClietRef = x.selectedClietRef
            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        //ADD ADMISSION TO DATABASE                   
        public JsonResult AddNewAdmission(CYCAAdmissionViewModel vm)
        {
            int id = Convert.ToInt32(vm.PcmAssNo);
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


            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
            int ClientRefid = admissionModel.GetClientRefIdssId(intAssessmentId);
            Session["clientRefId"] = ClientRefid;
            int clientref = Convert.ToInt32(Session["clientRefId"]);

            try
            {
                if (clientref > 0)
                {
                    admissionModel.AddAdmission(vm, clientref, userId);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Populate Request Details Modal
        public JsonResult GetAdmissionByAdmissionId(int AdmissionId)
        {
            
            CYCAAdmissionViewModel vm = new CYCAAdmissionViewModel();

            //int empid = admissionModel.GetBedSpaceRequestById(RequestId);

            vm = admissionModel.GetAdmissionByAdmissionId(AdmissionId);
            //var admissionid = Convert.ToInt32(Session["AdmissionId"]);
            ViewBag.AdminId = AdmissionId;
            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        //Edit Admission 
        public JsonResult UpdateAdmissionInDatabase(CYCAAdmissionViewModel vm)
        {
            int Id = Convert.ToInt32(vm.Admission_Id);

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

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);

            try
            {
                if (vm.Admission_Id > 0)
                {

                    admissionModel.UpdateAdmission(vm, Id, userId, Convert.ToInt32(vm.Admission_Type_Id), Convert.ToInt32(vm.VenueId));
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //ADD Re-ADMISSION TO DATABASE                   
        public JsonResult ReAdmission(CYCAAdmissionViewModel vm)
        {
            int id = Convert.ToInt32(vm.PcmAssNo);
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


            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
            int ClientRefid = admissionModel.GetClientRefIdssId(intAssessmentId);
            Session["clientRefId"] = ClientRefid;
            int clientref = Convert.ToInt32(Session["clientRefId"]);
            int admissionId = admissionModel.GetAdmissionIdByClientrefId(clientref);

            try
            {
                if (admissionId > 0)
                {
                    admissionModel.ReAdmit(vm, admissionId, userId);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Select(int AdmissionId)
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

            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
            //var ClientRefId = Convert.ToInt32(Session["ClientrefId"]);
            //int ClientRefId = admissionModel.GetClientRefByAssId(intAssessmentId);
            //AdmissionId = admissionModel.GetAdmissionIdByClientId(ClientRefId);
            Session["admissionid"] = AdmissionId;
            ViewBag.admissionid = AdmissionId;

            var adminID = admissionModel.SelectAdmissionByAdmissionId(AdmissionId);



            return Json(adminID, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region BODILY SEARCH     
        public ActionResult BodilySearchDetails()
        {
            //List<apl_Cyca_Bodily_Search_Reasons> search_Reasons = db.apl_Cyca_Bodily_Search_Reasons.ToList();
            //ViewBag.SearchReasonsList = new SelectList(search_Reasons, "Search_Reason_Id", "Description");

           var search_Reasons = admissionModel.getBodySearchReasons();
            ViewBag.SearchReasonList = new SelectList(search_Reasons, "Search_Reason_Id", "Description");

          var users = userModel.GetUserList();
            ViewBag.UserList = new SelectList(users, "[User_Id]", "First_Name");

            ViewBag.SearchReason =  admissionModel.GetFilteredBodySearchReasons();

            ViewBag.Userlist = admissionModel.getFilteredUserDetails();

            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            return View(VM);
        }

        //List All bodily search results
        public JsonResult GetBodilySearchList()
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

            

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
            //var ClientRefId = Convert.ToInt32(Session["ClientrefId"]);
            int ClientRefId = admissionModel.GetClientRefByAssId(intAssessmentId);
            int AdmissionId = admissionModel.GetAdmissionIdByClientId(ClientRefId);
            Session["admissionid"] = AdmissionId;
            ViewBag.admission = AdmissionId;

            List<CYCAAdmissionViewModel> lis = new List<CYCAAdmissionViewModel>();
            lis.AddRange(admissionModel.GetBodilySearchById(AdmissionId).Select(x => new CYCAAdmissionViewModel
            {
                Admission_Id = AdmissionId,
                Bodily_Search_Id = x.Bodily_Search_Id,
                Bodily_Search_Date = x.Bodily_Search_Date,
                Bodily_Search_Time = x.Bodily_Search_Time,
                selectedSearchReason = x.selectedSearchReason

            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddBodilySearch(CYCAAdmissionViewModel vm, int AdmissionId)
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

            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
            //var ClientRefId = Convert.ToInt32(Session["ClientrefId"]);
            //int ClientRefId = admissionModel.GetClientRefByAssId(intAssessmentId);
            //AdmissionId = admissionModel.GetAdmissionIdByClientId(ClientRefId);
            Session["admissionid"] = AdmissionId;
            ViewBag.admissionid = AdmissionId;

            var results = false;

            try
            {
                if (AdmissionId > 0)
                {
                    admissionModel.AddBodilySearch(vm, AdmissionId, userId);

                    results = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return Json(results, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetAdmissionId()
        //{
        //    var currentUser = (User)Session["CurrentUser"];
        //    var userProvince = -1;
        //    var userId = -1;


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

        //    CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

        //    var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
        //    //var ClientRefId = Convert.ToInt32(Session["ClientrefId"]);
        //    int ClientRefId = admissionModel.GetClientRefByAssId(intAssessmentId);
        //    int AdmissionId = admissionModel.GetAdmissionIdByClientId(ClientRefId);
        //    Session["admissionid"] = AdmissionId;

        //    return Json(Session["admissionid"], JsonRequestBehavior.AllowGet);
        //}

        //Populate Bodily Search Modal
        public JsonResult GetBodilySById(int BodilySearchId)
        {
            CYCAAdmissionViewModel vm = new CYCAAdmissionViewModel();

            //int empid = admissionModel.GetBedSpaceRequestById(RequestId);

            vm = admissionModel.GetBodilySById(BodilySearchId);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        //Edit Admission 
        public JsonResult UpdateBodilySearchInDatabase(CYCAAdmissionViewModel vm)
        {
            int Id = Convert.ToInt32(vm.Bodily_Search_Id);

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

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);

            try
            {
                if (Id > 0)
                {

                    admissionModel.UpdateBodilySearch(vm, Id, userId, Convert.ToInt32(vm.Search_Reason_Id), Convert.ToInt32(vm.Witnessed_By), Convert.ToInt32(vm.Conducted_By));
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


        #region ILLEGAL ITEMS FOUND
        public ActionResult IllegalItemsFound()
        {
            List<User> users = userModel.GetUserList();
            ViewBag.UsersList = new SelectList(users, "User_Id", "First_Name");

            ViewBag.Userlist = admissionModel.getFilteredUserDetails();

            //initialise view modeL
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            return View(VM);
        }

        //List all Illegal Items Found
        public JsonResult GetIllegalItemList()
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

            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
            //var ClientRefId = Convert.ToInt32(Session["ClientrefId"]);
            int ClientRefId = admissionModel.GetClientRefByAssId(intAssessmentId);
            int AdmissionId = admissionModel.GetAdmissionIdByClientId(ClientRefId);
            Session["admissionid"] = AdmissionId;
            ViewBag.admission = AdmissionId;

            List<CYCAAdmissionViewModel> lis = new List<CYCAAdmissionViewModel>();
            lis.AddRange(admissionModel.GetIllegalItemById(AdmissionId).Select(x => new CYCAAdmissionViewModel
            {
                Admission_Id = AdmissionId,
                Item_Found_Id = x.Item_Found_Id,
                Item_Description = x.Item_Description,
                Quantity = x.Quantity,
                selectedHandedBy = x.selectedHandedBy

            }).ToList());
            return Json(lis, JsonRequestBehavior.AllowGet);
        }

        //Populate Illegal Items Modal
        public JsonResult PopulateIllegalItemModalById(int IllegalItemId)
        {
            CYCAAdmissionViewModel vm = new CYCAAdmissionViewModel();

            //int empid = admissionModel.GetBedSpaceRequestById(RequestId);

            vm = admissionModel.GetEachIllegalItemId(IllegalItemId);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(vm, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        //Update Illegal Item in Database
        public JsonResult UpdateIllegalItemInDatabase(CYCAAdmissionViewModel vm)
        {
            int Id = Convert.ToInt32(vm.Item_Found_Id);

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

            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);

            try
            {
                if (vm.Item_Found_Id > 0)
                {

                    admissionModel.UpdateIllegalItems(vm, Id, userId, Convert.ToInt32(vm.Handed_By));
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

        #region ADMISSION DOCUMENTS
        public ActionResult UploadDocument()
        {
            var cyca_documents = admissionModel.GetCycaAdmissionDocumentTypeList();
            ViewBag.DocumentTypeList = new SelectList(cyca_documents, "DocType_Id", "Description");


            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            return View(VM);
        }

        public JsonResult AddDocuments(CYCAAdmissionViewModel vm, int admissionId = 8)
        {
            int id = Convert.ToInt32(vm.PcmAssNo);
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

                var document = new CYCA_Admissions_Document()
                {
                    Admission_Id = admissionId,
                    Document_Name = SaveToPhysicalLocation(vm.Document_Name),
                    Document_Ext = vm.Document_Ext,
                    Document_Type_Id = vm.Document_Type_Id,
                    DateSaved = DateTime.Now,
                    TimeSaved = DateTime.Now.ToString("HH:mm"),
                    Created_By = userId.ToString(),
                    Is_Active = true,
                    Is_Deleted = false
                };
                admissionModel.SaveCycaAdmissionDocument(document);        

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        private string SaveToPhysicalLocation(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                file.SaveAs(path);
                return path;
            }
            return string.Empty;
        }
        #endregion

        #region ADD UPDATE GENERAL DETAILS

        public ActionResult GetGeneralDetails()
        {
            var intAssessmentId = Convert.ToInt32(Session["Idassessment"]);
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
            string ClientRef = Convert.ToString(Session["ClientRef"]);
            ViewBag.ModuleRef = ClientRef;
            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

                int genAssid = admissionModel.GetPCMGeneralDetailsByassId(intAssessmentId);
                if (genAssid != 0)
                {

                    VM = admissionModel.GetGeneralDetailsList(genAssid);
                    Session["genupdate"] = genAssid;

                    return PartialView(VM);
                }
                else
                {
                    CYCAAdmissionViewModel VM1 = new CYCAAdmissionViewModel();
                    //initialise view model
                    CYCAAdmissionViewModel VMC = new CYCAAdmissionViewModel();

                    //funtion that calls insert Socio Economy
                    admissionModel.InsertGeneralDetails(VM1, intAssessmentId, userId);
                    int genAssidadd = admissionModel.GetPCMGeneralDetailsByassId(intAssessmentId);
                    VM1 = admissionModel.GetGeneralDetailsList(genAssidadd);
                    Session["genadd"] = genAssidadd;
                    ViewBag.AssI = intAssessmentId;
                    ViewBag.childid = intAssessmentId;
                    return PartialView("GetGeneralDetails", VM1);
                }

        }


        public ActionResult UpdateGeneralDetails()
        {
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
            //initialise model repositry

            //initialise view model
            CYCAAdmissionViewModel VM = new CYCAAdmissionViewModel();

            return PartialView("GetGeneralDetails", VM);

        }

        [HttpPost]
        public ActionResult UpdateGeneralDetails(CYCAAdmissionViewModel VM)
        {
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
            int intassid = Convert.ToInt32(Session["IntakeassId"]);
            int idcreate = Convert.ToInt32(Session["genadd"]);

            int Idupdate = Convert.ToInt32(Session["genupdate"]);
            int Ids;
            if (idcreate != 0)
            {

                Ids = idcreate;
            }
            else
            {
                Ids = Idupdate;
            }

            // Call Update Functions
            admissionModel.UpdatePCM_General_Details(VM, userId, Ids);
            ViewBag.Message = "Updated successfully";

            //return View("UpdatePCMCase", caseVM);
            return RedirectToAction("Index", "Assessment", new { Id = intassid });
        }


        #endregion


        #region Print Assessment report

        public ActionResult Reports()
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


            ViewBag.UserId_check = userId;
            ViewBag.AssId = intassid;

            return PartialView();
        }

        #endregion
    }
}