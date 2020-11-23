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

namespace CYCA_Module_V2.Controllers
{
    public class DynamicFormController : Controller
    {   
        private readonly CYCADynamicFormModel dynamicModel = new CYCADynamicFormModel();
        public JsonResult GetDynamicForm(int dynamicFormTypeId)
        {
            return Json(dynamicModel.GetDynamicForm(dynamicFormTypeId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDynamicFormAndAnswer(int dynamicFormTypeId,int dynamicFormDataId)
        {
            var dynamicForm = dynamicModel.GetDynamicForm(dynamicFormTypeId);
            var answer = dynamicModel.GetDynamicFormData(dynamicFormDataId);
            dynamicForm.Answer = answer.Data;
            dynamicForm.AnswerId = dynamicFormDataId;
            return Json(dynamicForm, JsonRequestBehavior.AllowGet);
        }
        public bool  SaveDynamicForm(CYCADynamicFormViewModel model)
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
                   
            model.UserId = currentUser.User_Id;
            CYCADynamicFormModel formModel = new CYCADynamicFormModel();
            CYCA_Dynamic_Form_Data data = new CYCA_Dynamic_Form_Data()
            {

                Client_Id = dynamicModel.GetClientIdByPersonId(model.ChildId),
                //Client_Id = model.ChildId,
                CreatedDate = DateTime.Now,
                Data = model.Answer,
                User_Id = model.UserId,
                Dynamic_Form_Id = model.DynamicFormId,
                Dynamic_Form_Data_Id = model.AnswerId,
                Venue_Id = dynamicModel.GetFacilityIdByUserID(model.UserId)
            };
            formModel.AddOrUpdateDynamicFormDatas(data);
            return true;
        }

        public JsonResult ClientGridAjaxPaging(DynamicFormSearchViewModel viewModel)
        {
            var currentUser = (User)Session["CurrentUser"];
            CYCADynamicFormModel model = new CYCADynamicFormModel();
            var listOfDynamicItems = model.GetDynamicFormDatasForClient(viewModel.DynamicFormId, viewModel.ChildId);
            
            var query = from p in listOfDynamicItems select p;

            var filteredResults = query.ToList();
            var child = model.GetChild(viewModel.ChildId);
            var clientItems = filteredResults.Select(x => new DynamicFormGridMain()
            {
                ChildId = x.Client_Id,
                ChildName = child.First_Name + " " + child.Last_Name,
                CreateDate = x.CreatedDate,
                DynamicFormDataId = x.Dynamic_Form_Data_Id,
                UserId = currentUser.User_Id,
                UserName = currentUser.fullname
            }).ToList();

            var data = clientItems.OrderBy(o => o.CreateDate).ToList();
            var grid = new WebGrid(data, canPage: true, rowsPerPage: 20, canSort: false);
            var htmlString = grid.GetHtml(tableStyle: "NestedMainGrid",
                                          headerStyle: "webgrid-header",
                                          alternatingRowStyle: "webgrid-alternating-row",
                                          selectedRowStyle: "webgrid-selected-row",
                                          rowStyle: "webgrid-row-style",
                                          htmlAttributes: new { id = "clientsListGrid" },
                                          mode: WebGridPagerModes.NextPrevious,
                                          columns: grid.Columns(
                                                grid.Column("UserName", "Performed By"),
                                                grid.Column("Date", "CreateDate"))
                                        );
            return Json(new
            {
                Data = htmlString.ToHtmlString(),
                Count = clientItems.Count(),
                Page = 1
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropDown(int Type)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            return Json(dynamicModel.GetDynamicDropDown(Type, userId),  JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropDownForKids(int Type)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            return Json(dynamicModel.GetDropDownFOrKidsInTheCenter(Type, userId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropDownForVenue(int Type)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            return Json(dynamicModel.GetDropDownFOrVenuesInTheCenter(Type, userId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropDownFOrRolesInTheCenter(int Type)
        {
            var currentUser = (User)Session["CurrentUser"];
            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            return Json(dynamicModel.GetDropDownFOrRolesInTheCenter(Type, userId), JsonRequestBehavior.AllowGet);
        }

    }
    
}