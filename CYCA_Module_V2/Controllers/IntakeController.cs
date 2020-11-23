using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace CYCA_Module_V2.Controllers
{

    
    public class IntakeController : Controller
    {
        private readonly AffisModel affisModel = new AffisModel();
        private readonly PersonModel person = new PersonModel();
        //GET: Search Child
        public ActionResult Index()
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
            var userName = string.Empty;
            var currentUserProvinceId = -1;

            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }

            if (currentUser.Employees.Any())
            {
                currentUserProvinceId = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
            }


            if (currentUser.apl_Social_Worker.Any())
                currentUserProvinceId = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            var intakeViewModel = new CYCASearchViewModel { Person_List = new List<Person>(), Clients_Assessments_List = new List<CycaClientGridMain>(), Inbox_List = new List<InboxGridItem>() };


            return View(intakeViewModel);
        }

        public JsonResult ClientGridAjaxPaging(IntakeSearchViewModel intakeGrid)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;
            var currentUserProvinceId = -1;

            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }

            if (currentUser.Employees.Any())
                currentUserProvinceId = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            if (currentUser.apl_Social_Worker.Any())
                currentUserProvinceId = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            var personModel = new PersonModel();
            List<Person> listOfPersons = new List<Person>();

            var persons = person.GetExistingPeople();

            //listOfAgents = PopulateAdditionalItems(agents, dbContext);

            listOfPersons = (from p in persons
                             select p).OrderBy(x => x.Last_Name).ToList();

            if (intakeGrid.showOnlyMyRecords)
                listOfPersons.RemoveAll(x => !x.Created_By.Equals(userName));

            var query = from p in listOfPersons select p;

            if (!string.IsNullOrEmpty(intakeGrid.Search_Client_Ref_No))
                query = query.Where(p => (p.Clients.Any() && (p.Clients.First().Reference_Number.Equals(intakeGrid.Search_Client_Ref_No))));

            if (!string.IsNullOrEmpty(intakeGrid.Search_First_Name))
                query = query.Where(p => p.First_Name.ToLower().Contains(intakeGrid.Search_First_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeGrid.Search_Last_Name))
                query = query.Where(p => p.Last_Name.ToLower().Contains(intakeGrid.Search_Last_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeGrid.Search_Client_ID_No))
                query = query.Where(p => p.Identification_Number.Contains(intakeGrid.Search_Client_ID_No));

            DateTime parsedDate;
            if ((!string.IsNullOrEmpty(intakeGrid.Search_Date_Of_Birth)) && (DateTime.TryParse(intakeGrid.Search_Date_Of_Birth, out parsedDate)))
                query = query.Where(p => p.Date_Of_Birth.Equals(parsedDate));

            var filteredResults = query.ToList();

            var clientItems = filteredResults.Select(x => new ClientGridMain()
            {
                PersonId = x.Person_Id,
                FirstName = x.First_Name,
                LastName = x.Last_Name,
                IDNumber = x.Identification_Number,
                AssessmentCount = x.Clients.Any() ? x.Clients.First().Intake_Assessments.Count : 0,
                NestedItems = !x.Clients.Any() ? new List<ClientGridNested>() : x.Clients.First().Intake_Assessments.Select(y => new ClientGridNested()
                {
                    PersonId = x.Person_Id,
                    AssessmentId = y.Intake_Assessment_Id,
                    AssessmentDate = y.Assessment_Date
                }).ToList()
            }).ToList();

            int skip = intakeGrid.pageNumber.HasValue ? intakeGrid.pageNumber.Value - 1 : 0;
            var data = clientItems.OrderBy(o => o.PersonId).Skip(skip * 5).Take(5).ToList();
            var grid = new WebGrid(data, canPage: true, rowsPerPage: 5, canSort: false);
            var htmlString = grid.GetHtml(tableStyle: "NestedMainGrid",
                                          headerStyle: "webgrid-header",
                                          alternatingRowStyle: "webgrid-alternating-row",
                                          selectedRowStyle: "webgrid-selected-row",
                                          rowStyle: "webgrid-row-style",
                                          htmlAttributes: new { id = "clientsListGrid" },
                                          mode: WebGridPagerModes.NextPrevious,
                                          columns: grid.Columns(
                                                grid.Column("FirstName", "First Name"),
                                                grid.Column("LastName", "Last Name"),
                                                grid.Column("IDNumber", "ID Number"),
                                                grid.Column("AssessmentCount", "No of Assessments"),
                                                grid.Column(header: "", style: "EditPersonWidth", format: (item) =>
                                                {
                                                    //var link = MvcHtmlString.Create(HtmlHelper.GenerateLink(this.ControllerContext.RequestContext, System.Web.Routing.RouteTable.Routes, "Edit Person", "", "Edit", "Intake", new System.Web.Routing.RouteValueDictionary(new { id = item.PersonId }), new System.Web.Routing.RouteValueDictionary(new { @class = "btn btn-success" })));
                                                    var link = MvcHtmlString.Create("<a class='btn btn-success' href='" + Url.Action("Index", "Client", new { id = item.PersonId }) + "'>View Child</a>");
                                                    //var link = MvcHtmlString.Create("<a class='btn btn-success' href='" + Url.Action("Index", "Client", new { id = item.PersonId }) + "'>View Child</a><a class='btn btn-success' href='" + Url.Action("SaveAdmissionDetailsNew", "Admissions") + "'>Admit Child</a>");
                                                    return link;
                                                }),
                                                grid.Column(format: (item) =>
                                                {
                                                    var subGrid = new WebGrid(source: item.NestedItems, canPage: false, canSort: false);
                                                    return subGrid.GetHtml(
                                                        tableStyle: "NestedSubGrid",
                                                        htmlAttributes: new { id = "assessmentsListGrid", @class = "NestedSubGrid", width = "100%" },
                                                        columns: subGrid.Columns(
                                                            subGrid.Column("AssessmentDate", "Assessment Date", format: (dateItem) => string.Format("{0:dd MMM yyyy}", dateItem.AssessmentDate)),
                                                            subGrid.Column(header: "", style: "EditAssessmentWidth", format: (subItem) =>
                                                            {
                                                                //var link = Html.ActionLink("Edit Assessment", "EditAssessment", "Intake", new { id = subItem.AssessmentId }, new { @class = "btn btn-success" });
                                                                var link = MvcHtmlString.Create("<a class='btn btn-success' href='" + Url.Action("EditAssessment", "Intake", new { id = subItem.AssessmentId }) + "'>Edit Assessment</a>");
                                                                return link;
                                                            })
                                                        )
                                                    );
                                                })
                                            )
                                        );
            return Json(new
            {
                Data = htmlString.ToHtmlString(),
                Count = (clientItems.Count() + 5 - 1) / 5,
                Page = intakeGrid.pageNumber ?? 1
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindChildByGuid(string Id)
        {
            var g = new Guid(Id);
            var upid = affisModel.CheckGuid(g);

            if (upid == null)
            {
                return null;
            }
            else
            {
                var Persons = person.GetPersonByGuid(g);
                return Json(Url.Action("Index", "Client", new { id = upid.Person_Id }));
            }

        }
    }
}