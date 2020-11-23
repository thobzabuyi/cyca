using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Common_Objects;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using CYCA_Module_V2.Common_Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CYCA_Module_V2.Controllers
{
    public class ReportChartController : Controller
    {
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        CYCAReportsViewModel reportModel = new CYCAReportsViewModel();
        // GET: ReportChart
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
            var dynamicModel = new CYCADynamicFormModel();
            ViewBag.Years = reportModel.GetYears();
            ViewBag.Months = reportModel.GetQuarterMonths();
            var info = GetFacilityAdmissions(dynamicModel.GetFacilityIdByUserID(userId), 0, 1, Convert.ToInt32(DateTime.Now.Year.ToString()));
            info.Year = new DateTime().Year;
            DisplayMonths(1);
            return PartialView("Index", info);
        }

        public void DisplayMonths(int quarter)
        {
            switch (quarter)
            {
                case 1: ViewBag.MonthTitles = new string[] { "Jan", "Feb", "Mar" }; break;
                case 2: ViewBag.MonthTitles = new string[] { "Apr", "May", "Jun" }; break;
                case 3: ViewBag.MonthTitles = new string[] { "Jul", "Aug", "Sep" }; break;
                case 4: ViewBag.MonthTitles = new string[] { "Oct", "Nov", "Dec" }; break;
                default: ViewBag.MonthTitles = new string[] { "Jan", "Feb", "Mar" }; break;
            }
        }
        public PartialViewResult ShowTabs(CYCAReportTabType type)
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

            ViewBag.Years = reportModel.GetYears();
            ViewBag.Months = reportModel.GetQuarterMonths();

            var dynamicModel = new CYCADynamicFormModel();

            switch (type)
            {
                case CYCAReportTabType.admission:
                    var info = GetFacilityAdmissions(dynamicModel.GetFacilityIdByUserID(userId), 0,1, Convert.ToInt32(DateTime.Now.Year.ToString()));
                    return PartialView("~/Views/Reports/NewAdmissionReports.cshtml", info);
                case CYCAReportTabType.bedspace:
                    return PartialView("~/Views/Reports/BedSpaceReports.cshtml");
                case CYCAReportTabType.gang:
                    var gangInfo = GetChildrenInGangs(dynamicModel.GetFacilityIdByUserID(userId), 0, 1, Convert.ToInt32(DateTime.Now.Year.ToString()));
                    return PartialView("~/Views/Reports/ChildrenInGangs.cshtml", gangInfo);
                case CYCAReportTabType.illigalitems:
                    var illegalInfo = GetChildrenWithIllegalItems(dynamicModel.GetFacilityIdByUserID(userId), 0, 1, Convert.ToInt32(DateTime.Now.Year.ToString()));
                    return PartialView("~/Views/Reports/IllegalItemsReports.cshtml", illegalInfo);
                case CYCAReportTabType.pregnantchildren:
                    var pregreport = PregnantChildrenReport(dynamicModel.GetFacilityIdByUserID(userId), 0, 1, Convert.ToInt32(DateTime.Now.Year.ToString()));
                    return PartialView("~/Views/Reports/PregnantChildren.cshtml", pregreport);
                case CYCAReportTabType.dischargedchidren:
                    var discharge = DischargedChildrenReport(dynamicModel.GetFacilityIdByUserID(userId), 0, 1, Convert.ToInt32(DateTime.Now.Year.ToString()));
                    return PartialView("~/Views/Reports/DischargedChildren.cshtml", discharge);
                case CYCAReportTabType.reportableincident:
                    var incident = ReportableIncidentChildrenReport(dynamicModel.GetFacilityIdByUserID(userId),0);
                    return PartialView("~/Views/Reports/ReportableIncident.cshtml", incident);
                default:
                    return null;
            }

        }

        public PartialViewResult ShowTab(CYCAReportTabType type, int page, int quarter, int year)
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


            var dynamicModel = new CYCADynamicFormModel();

            switch (type)
            {
                case CYCAReportTabType.admission:
                    var info = GetFacilityAdmissions(dynamicModel.GetFacilityIdByUserID(userId), page, quarter, year);
                    return PartialView("~/Views/Reports/NewAdmissionReports.cshtml", info);
                case CYCAReportTabType.bedspace:
                    return PartialView("~/Views/Reports/BedSpaceReports.cshtml");
                case CYCAReportTabType.gang:
                    var gangInfo = GetChildrenInGangs(dynamicModel.GetFacilityIdByUserID(userId), page, quarter, year);
                    return PartialView("~/Views/Reports/ChildrenInGangs.cshtml", gangInfo);
                case CYCAReportTabType.illigalitems:
                    var illegalInfo = GetChildrenWithIllegalItems(dynamicModel.GetFacilityIdByUserID(userId), page, quarter, year);
                    return PartialView("~/Views/Reports/IllegalItemsReports.cshtml", illegalInfo);
                case CYCAReportTabType.pregnantchildren:
                    var pregreport = PregnantChildrenReport(dynamicModel.GetFacilityIdByUserID(userId),page, quarter, year);
                    return PartialView("~/Views/Reports/PregnantChildren.cshtml", pregreport);
                case CYCAReportTabType.dischargedchidren:
                    var discharge = DischargedChildrenReport(dynamicModel.GetFacilityIdByUserID(userId), page, quarter, year);
                    return PartialView("~/Views/Reports/DischargedChildren.cshtml", discharge);
                case CYCAReportTabType.reportableincident:
                    var incident = ReportableIncidentChildrenReport(dynamicModel.GetFacilityIdByUserID(userId), page);
                    return PartialView("~/Views/Reports/ReportableIncident.cshtml", incident);
                default:
                    return null;
            }

        }
        public PartialViewResult PageData(int facilityId, int page)
        {
            var info = GetFacilityAdmissions(facilityId, page, 1,Convert.ToInt32( DateTime.Now.Year.ToString()));
            return PartialView("~/Views/Reports/NewAdmissionReports.cshtml", info);

        }
        public ActionResult AdmissionChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();
           
          
            var data = db.CYCA_NumberOfNewAdmissionsPerFacilityAnnually(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 600, height: 300)
            .AddTitle("Number of Children Admitted")    
            .AddSeries(
                name: "Admissionss",
                xValue: data, xField: "MONTH",
                yValues: data, yFields: "TotalAdmissions");
            MyChart.Write("bmp");
            return null;
        }
        public ActionResult BedSpaceChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_NumberOfNewAdmissionsPerFacilityAnnually(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 600, height: 300)
            .AddTitle("Number of Bed Space Used")
            .AddSeries(
                name: "Admissionss",
                xValue: data, xField: "MONTH",
                yValues: data, yFields: "TotalAdmissions");
            MyChart.Write("bmp");
            return null;
        }
        public ActionResult IllegalItemChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_NumberOfIllegalItemsPerFacilityAnnually(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 600, height: 300)
            .AddTitle("Number of Children Found With Illegal Items")
            .AddSeries(
                name: "IllegalItems",
                xValue: data, xField: "MONTH",
                yValues: data, yFields: "Total");
            MyChart.Write("bmp");
            return null;
        }
        public ActionResult GangChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_NumberOfChildrenInGangsPerFacilityAnnually(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 600, height: 300)
            .AddTitle("Number of Children In Gangs")
            .AddSeries(
                name: "Gang",
                xValue: data, xField: "MONTH",
                yValues: data, yFields: "Total");
            MyChart.Write("bmp");
            return null;
        }
        public ActionResult PregnancyChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_NumberOfPregnantPerFacilityAnnually(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 600, height: 300)
            .AddTitle("Number of Pregnant Children")
            .AddSeries(
                name: "Pregnancy",
                xValue: data, xField: "MONTH",
                yValues: data, yFields: "Total");
            MyChart.Write("bmp");
            return null;
        }
        public ActionResult DischargedChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_NumberOfDischargedChildrenPerFacilityAnnually(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 600, height: 300)
            .AddTitle("Number of Discharged Children")
            .AddSeries(
                name: "Discharged",
                xValue: data, xField: "MONTH",
                yValues: data, yFields: "TotalDischarge");
            MyChart.Write("bmp");
            return null;
        }
        public ActionResult ReportableIncidentChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_ReportableIncidentPerFacilityAnnually(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 600, height: 300)
            .AddTitle("Number of Reportable Incidents")
            .AddSeries(
                name: "Reportable Incident",
                xValue: data, xField: "MONTH",
                yValues: data, yFields: "TotalDischarge");
            MyChart.Write("bmp");
            return null;
        }

        public ActionResult PieChartDisplay()
        {
            var MyChart = new Chart(width: 300, height: 250)
        .AddTitle("Admissions Per Age Groups")
        .AddSeries(chartType: "Pie",
            name: "Employee",
            xValue: new[] { "13 Years", "14 Years", "15 Years", "16 Years", "17 Years" },
            yValues: new[] { "2", "6", "4", "5", "3" });

            MyChart.Write("bmp");
            return null;
        }
        public ActionResult AdmissionPieChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_TotalAgeGroupsForAdmissionPerFacility(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            foreach (var item in data)
            {
                item.AGE = item.AGE + "(" + item.TotalAge + ")";
            }
            var MyChart = new Chart(width: 300, height: 250)
        .AddTitle("Admissions Per Age Groups")
        .AddLegend()
        .AddSeries(chartType: "Pie",
            name: "Admission",
            xValue: data, xField: "AGE",
            yValues: data, yFields: "TotalAge");

            MyChart.Write("bmp");
            return null;
        }
        public ActionResult PregnancyPieChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_TotalAgeGroupsForPregnantKidsPerFacility(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 300, height: 250)
            .AddTitle("Pregnacy results by Age Groups")
            .AddSeries(chartType: "Pie",
                name: "Pregnancy",
                xValue: data, xField: "AGE",
                yValues: data, yFields: "TotalAge");

            MyChart.Write("bmp");
            return null;
        }
        public ActionResult DischargedPieChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_TotalAgeGroupsForDischargedPerFacility(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 300, height: 250)
            .AddTitle("Discharged Children results by Age Groups")
            .AddSeries(chartType: "Pie",
                name: "Discharged",
                xValue: data, xField: "AGE",
                yValues: data, yFields: "TotalAge");

            MyChart.Write("bmp");
            return null;
        }
        public ActionResult ReportableIncidentPieChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_TotalAgeGroupsForReportableIncidentPerFacility(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 300, height: 250)
            .AddTitle("Reportable Incidents results by Age Groups")
            .AddSeries(chartType: "Pie",
                name: "Reportable Incident",
                xValue: data, xField: "AGE",
                yValues: data, yFields: "TotalAge");

            MyChart.Write("bmp");
            return null;
        }
        public ActionResult IllegalItemsPieChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_TotalAgeGroupsForChildrenFoundWithIllegalItemsPerFacility(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 300, height: 250)
            .AddTitle("Children Found with Illegal Items by Age Groups")
            .AddSeries(chartType: "Pie",
                name: "IllegalItems",
                xValue: data, xField: "AGE",
                yValues: data, yFields: "TotalAge");

            MyChart.Write("bmp");
            return null;
        }
        public ActionResult GangsPieChartDisplay()
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
            var dynamicModel = new CYCADynamicFormModel();


            var data = db.CYCA_TotalAgeGroupsForChildrenInGangPerFacility(dynamicModel.GetFacilityIdByUserID(userId)).ToList();
            var MyChart = new Chart(width: 300, height: 250)
            .AddTitle("Children in Gangs results by Age Groups")
            .AddLegend()
            .AddSeries(chartType: "Pie",
                name: "Gang",
                xValue: data, xField: "AGE",
                yValues: data, yFields: "TotalAge");

            MyChart.Write("bmp");
            return null;
        }

        public CYCAReportsViewModel GetFacilityAdmissions(int Facility_Id, int page, int quarter, int year)
        {
            var date = reportModel.GetDates(quarter, year);
            //test
            var admissionHistory = GetAdmissionReport(Facility_Id, page);
            var totalProvinceAdmission = GetTotalAdmittedKidsPerProvince(Facility_Id);
            var totalFacilityAdmission = reportModel.GetTotalAdmittedKidsPerFacilty(Facility_Id, date);
            var totalRegionAdmission = reportModel.GetTotalAdmittedKidsPerDistrict(Facility_Id, date);
            var records = new CYCAReportsViewModel
            {
                Facility_Id = Facility_Id,
                AdmissionReportViewModels = admissionHistory,
                TotalProvinceAdmissionsViewModels = totalProvinceAdmission,
                TotalRegionAdmissionss = totalRegionAdmission,
                TotalFacilityAdmissionss = totalFacilityAdmission
            };

            ViewBag.Years = year;
            ViewBag.Months = reportModel.GetQuarterMonth(quarter);
            DisplayMonths(quarter);
            ViewBag.Facility_Id = Facility_Id;
            return records;
        }
        public List<AdmissionReportViewModel> GetAdmissionReport(int Facility_Id, int page)
        {

            var models = db.CYCA_NewAdmissionsFacilityReport(Facility_Id).ToList();
            var viewModels = new List<AdmissionReportViewModel>();

            foreach (var m in models)
            {

                viewModels.Add(new AdmissionReportViewModel
                {
                    Facility_Id = m.Facility_Id,
                    ProvinceName = m.Province,
                    RegionName = m.Region,
                    CenterName = m.FacilityName,
                    AdmissionDate = Convert.ToDateTime(m.Admission_Date).ToShortDateString(),
                    FullName = m.FullName,
                    admissionReason = m.Admission_Reason,
                    Age = m.Age,
                    Gender = m.Description

                });
            }

            var pageSize = 10;
            var count = viewModels.Count();
            var data = viewModels.Skip(page * pageSize).Take(pageSize).ToList();
            this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;

            return data;
        }

        public CYCAReportsViewModel PregnantChildrenReport(int Facility_Id, int page, int quarter, int year)
        {
            var date = reportModel.GetDates(quarter, year);
            var pregnancyHistory = GetPregnantChildrenReport(Facility_Id, page);
            var ProvincePregnancyHistory = GetTotalAdmittedKidsPerProvince(Facility_Id);
            var totalPregnantChildren = reportModel.GetTotalPregnantChildrenPerFacilty(Facility_Id, date);
            var records = new CYCAReportsViewModel
            {
                Facility_Id = Facility_Id,
                PregnantChildrenReportViewModels = pregnancyHistory,
                TotalFacilityAdmissionss = totalPregnantChildren
                //TotalProvinceAdmissionss = ProvincePregnancyHistory

            };
            ViewBag.Years = year;
            ViewBag.Months = reportModel.GetQuarterMonth(quarter);
            DisplayMonths(quarter);
            ViewBag.Facility_Id = Facility_Id;
            return records;
        }
        public List<PregnantChildrenReportViewModel> GetPregnantChildrenReport(int Facility_Id, int page)
        {

            var models = db.CYCA_PregnantChildrenDetailedFacilityReport(Facility_Id).ToList();
            var viewModels = new List<PregnantChildrenReportViewModel>();
            this.ViewBag.Page = page;
            foreach (var m in models)
            {
                string preg = "";
                //string illiness = "";
                string molested = "";
                string medicalassessment = m.Data;
                string[] medasslist = medicalassessment.Split(',');
                for (int i = 0; i < medasslist.Length; i++)
                {
                    //Pregnancy Test Results
                    if (medasslist[i] == "\"Pregnant\":\"no\"")
                    {
                        preg = "Negative";
                    }
                    else if (medasslist[i] == "\"Pregnant\":\"yes\"")
                    {
                        preg = "Positive";
                    }

                    //Sexual Molestation
                    if (medasslist[i] == "\"SexualMolestation\":\"no\"")
                    {
                        molested = "No";
                    }
                    else if (medasslist[i] == "\"SexualMolestation\":\"yes\"")
                    {
                        molested = "Yes";
                    }

                    //Illiness
                    //if (medasslist[i] == "\"illiness\":\"item1\"")
                    //{
                    //    molestation = "No";
                    //}
                    //else if (medasslist[i] == "\"SexualMolestation\":\"yes\"")
                    //{
                    //    molestation = "Yes";
                    //}

                }


                viewModels.Add(new PregnantChildrenReportViewModel
                {
                    Facility_Id = m.Facility_Id,
                    ProvinceName = m.Province,
                    RegionName = m.Region,
                    CenterName = m.FacilityName,
                    AdmissionDate = Convert.ToDateTime(m.Admission_Date).ToShortDateString(),
                    FullName = m.FullName,
                    AdmissionReason = m.Admission_Reason,
                    Age = m.Age,
                    Gender = m.Description,
                    IsPregnant = preg,
                    IsSexuallyMolested = molested


                });
            }


            var pageSize = 10;
            var count = viewModels.Count();
            var data = viewModels.Skip(page * pageSize).Take(pageSize).ToList();
            this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;

            return data;
        }


        public CYCAReportsViewModel DischargedChildrenReport(int Facility_Id, int page, int quarter, int year)
        {
            var date = reportModel.GetDates(quarter, year);
            var dischargeHistory = GetDischargedChildrenReport(Facility_Id, page);
            var totalDischargedChildren = reportModel.GetTotalDischargedPerFacilty(Facility_Id, date);
            //var ProvincePregnancyHistory = GetTotalAdmittedKidsPerProvince(Facility_Id);
            var records = new CYCAReportsViewModel
            {
                Facility_Id = Facility_Id,
                DischargedChildrenReportViewModels = dischargeHistory,
                TotalFacilityAdmissionss = totalDischargedChildren
                //TotalProvinceAdmissionss = ProvincePregnancyHistory

            };
            ViewBag.Years = year;
            ViewBag.Months = reportModel.GetQuarterMonth(quarter);
            DisplayMonths(quarter);
            ViewBag.Facility_Id = Facility_Id;
            return records;
        }
        public List<DischargedChildrenReportViewModel> GetDischargedChildrenReport(int Facility_Id, int page)
        {

            var models = db.CYCA_DischargedChildrenDetailedFacilityReport(Facility_Id).ToList();
            var viewModels = new List<DischargedChildrenReportViewModel>();

            foreach (var m in models)
            {
                viewModels.Add(new DischargedChildrenReportViewModel
                {

                    ProvinceName = m.Province,
                    RegionName = m.Region,
                    CenterName = m.FacilityName,
                    AdmissionDate = Convert.ToDateTime(m.Admission_Date).ToShortDateString(),
                    FullName = m.FullName,
                    AdmissionReason = m.AdmissionReason,
                    Age = m.Age,
                    Gender = m.Gender,
                    DischargeDate = m.DischargeDate.ToString("yyyy/MM/dd")
                });
            }

            var pageSize = 10;
            var count = viewModels.Count();
            var data = viewModels.Skip(page * pageSize).Take(pageSize).ToList();
            this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;

            return data;
        }

        public CYCAReportsViewModel ReportableIncidentChildrenReport(int Facility_Id, int page)
        {

            var incidentHistory = GetReportableIncidentReport(Facility_Id, page);
            //var ProvincePregnancyHistory = GetTotalAdmittedKidsPerProvince(Facility_Id);
            var records = new CYCAReportsViewModel
            {
                Facility_Id = Facility_Id,
                ReportableIncidentReportViewModels = incidentHistory,
                //TotalProvinceAdmissionss = ProvincePregnancyHistory

            };

            ViewBag.Facility_Id = Facility_Id;
            return records;
        }
        public List<ReportableIncidentReportViewModel> GetReportableIncidentReport(int Facility_Id, int page)
        {

            var models = db.CYCA_ReportableIncidentDetailedFacilityReport(Facility_Id).ToList();
            var viewModels = new List<ReportableIncidentReportViewModel>();

            foreach (var m in models)
            {
                viewModels.Add(new ReportableIncidentReportViewModel
                {

                    ProvinceName = m.Province,
                    RegionName = m.Region,
                    CenterName = m.FacilityName,
                    AdmissionDate = Convert.ToDateTime(m.Admission_Date).ToShortDateString(),
                    FullName = m.FullName,
                    AdmissionReason = m.AdmissionReason,
                    Age = m.Age,
                    Gender = m.Gender,
                    IncidentDate = m.IncidentDate.ToString("yyyy/MM/dd")
                });
            }


            var pageSize = 10;
            var count = viewModels.Count();
            var data = viewModels.Skip(page * pageSize).Take(pageSize).ToList();
            this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;

            return data;
        }

        //public CYCAReportsViewModel GetTotalAdmittedKids(int Facility_Id)
        //{

        //    var pregnancyHistory = GetTotalAdmittedKidsPerProvince(Facility_Id);
        //    var records = new CYCAReportsViewModel
        //    {
        //        Facility_Id = Facility_Id,
        //        TotalProvinceAdmissionss = pregnancyHistory

        //    };

        //    ViewBag.Province_Id = Facility_Id;
        //    return records;
        //}

        public CYCAReportsViewModel GetChildrenWithIllegalItems(int Facility_Id, int page, int quarter, int year)
        {
            var date = reportModel.GetDates(quarter, year);
            var admissionHistory = GetChildrenWithIllegalItemsReport(Facility_Id, page);
            var totalChildrenWithIllegalItems = reportModel.GetTotalChildrenWithIllegalItemsPerFacilty(Facility_Id, date);
            var records = new CYCAReportsViewModel
            {
                Facility_Id = Facility_Id,
                ChildrenWithIllegalItemsReportViewModels = admissionHistory,
                TotalFacilityAdmissionss = totalChildrenWithIllegalItems

            };

            ViewBag.Years = year;
            ViewBag.Months = reportModel.GetQuarterMonth(quarter);
            DisplayMonths(quarter);
            ViewBag.Facility_Id = Facility_Id;
            return records;
        }

        public CYCAReportsViewModel GetChildrenInGangs(int Facility_Id,int page, int quarter, int year)
        {
            var date = reportModel.GetDates(quarter, year);
            var admissionHistory = GetChildrenInGangReport(Facility_Id, page);
            var totalChildrenInGangs = reportModel.GetTotalChildrenInGangsPerFacilty(Facility_Id, date);
            var records = new CYCAReportsViewModel
            {
                Facility_Id = Facility_Id,
                ChildrenInGangReportViewModels = admissionHistory,
                TotalFacilityAdmissionss = totalChildrenInGangs

            };

            ViewBag.Years = year;
            ViewBag.Months = reportModel.GetQuarterMonth(quarter);
            DisplayMonths(quarter);
            ViewBag.Facility_Id = Facility_Id;
            return records;
        }

        public List<ChildrenWithIllegalItemsReportViewModel> GetChildrenWithIllegalItemsReport(int Facility_Id, int page)
        {

            var models = db.CYCA_ChildrenFoundWithIllegalItemsReport(Facility_Id).ToList();
            var viewModels = new List<ChildrenWithIllegalItemsReportViewModel>();

            foreach (var m in models)
            {

                viewModels.Add(new ChildrenWithIllegalItemsReportViewModel
                {
                    Facility_Id = m.Facility_Id,
                    ProvinceName = m.Province,
                    RegionName = m.Region,
                    CenterName = m.FacilityName,
                    AdmissionDate = Convert.ToDateTime(m.Admission_Date).ToShortDateString(),
                    FullName = m.First_Name + " " + m.Last_Name,
                    admissionReason = m.Admission_Reason,
                    Age = m.Age,
                    Gender = m.Gender,
                    IllegalItems = m.ItemsFound

                });
            }

            var pageSize = 10;
            var count = viewModels.Count();
            var data = viewModels.Skip(page * pageSize).Take(pageSize).ToList();
            this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;

            return data;
        }

        public List<ChildrenInGangReportViewModel> GetChildrenInGangReport(int Facility_Id, int page)
        {

            var models = db.CYCA_ChildrenInGangsReport(Facility_Id).ToList();
            var viewModels = new List<ChildrenInGangReportViewModel>();

            foreach (var m in models)
            {

                viewModels.Add(new ChildrenInGangReportViewModel
                {
                    Facility_Id = m.Facility_Id,
                    ProvinceName = m.Province,
                    RegionName = m.Region,
                    CenterName = m.FacilityName,
                    AdmissionDate = Convert.ToDateTime(m.Admission_Date).ToShortDateString(),
                    FullName = m.First_Name + " " + m.Last_Name,
                    admissionReason = m.Admission_Reason,
                    Age = m.Age,
                    Gender = m.Gender,
                    GangMembership = m.GangMembership

                });
            }
            var pageSize = 10;
            var count = viewModels.Count();
            var data = viewModels.Skip(page * pageSize).Take(pageSize).ToList();
            this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;
            return data;
        }



        #region SUMMARY VIEW

        public List<TotalProvinceAdmissionsViewModel> GetTotalAdmittedKidsPerProvince(int Facility_Id)
        {

            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();

            var models = db.CYCA_TotalFemalesAdmittedPerProvince(Province_Id).ToList();
            var Malesmodels = db.CYCA_TotalMalesAdmittedPerProvince(Province_Id).ToList();
            var viewModels = new List<TotalProvinceAdmissionsViewModel>();

            int? totMale = 0;
            foreach (var m in Malesmodels)
            {
                totMale = m.Total;
            }

            foreach (var m in models)
            {
                //var totalDistrict = db.CYCA_TotalFemalesAdmittedPerDistrict(m.Province_Id).ToList();
                viewModels.Add(new TotalProvinceAdmissionsViewModel
                {
                    Province_Id = m.Province_Id,
                    ProvinceName = m.ProvinceName,
                    Gender = m.Gender,
                    TotalFemales = m.Total,
                    TotalMale = totMale,
                    //Region_Id = Convert.ToInt32(totalDistrict.Where(a => a.Province_Id == m.Province_Id)),
                    //RegionName = totalDistrict.Where(a => a.Province_Id == m.Province_Id).ToString()           
                    //RegionName = totalDistrict.Select(a => a.RegionName).ToString()

            });
            }
            return viewModels;
        }

        //public List<TotalRegionAdmissions> GetTotalAdmittedKidsPerDistrict(int Facility_Id)
        //{

        //    int Province_Id = (from f in db.apl_Cyca_Facility
        //                       join p in db.Provinces on f.Province_Id equals p.Province_Id
        //                       where f.Facility_Id == Facility_Id
        //                       select p.Province_Id).SingleOrDefault();
        //    var StartDate = new DateTime(2020,05,01);
        //    var EndDate = new DateTime(2020,07,31);

        //    var models = db.CYCA_TotalFemalesAdmittedPerDistrict(Province_Id, StartDate, EndDate).ToList();
        //    var Malesmodels = db.CYCA_TotalMalesAdmittedPerDistrict(Province_Id, StartDate, EndDate).ToList();
        //    var viewModels = new List<TotalRegionAdmissions>();

        //    //int? totMale = 0;


        //    foreach (var m in models)
        //    {
        //        //foreach (var i in Malesmodels)
        //        //{
        //        //    totMale = i.Total;
        //        //}
        //        var males = Malesmodels.Where(a => a.District_Id == m.District_Id).SingleOrDefault();
        //        viewModels.Add(new TotalRegionAdmissions
        //        {
        //            Province_Id = m.Province_Id,
        //            Gender = m.Gender,
        //            TotalFemales = m.Total,
        //            TotalMale = males.Total,

        //            RegionName = m.RegionName

        //        });
        //    }




        //    return viewModels;
        //}

        public List<TotalRegionAdmissions> GetTotalAdmittedKidsPerDistrict(int Facility_Id)
        {

            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
            var StartDate = new DateTime(2020, 04, 01);
            var EndDate = new DateTime(2020, 06, 30);
            var models = db.CYCA_TotalFemalesAdmittedPerDistrict1(Province_Id, StartDate, EndDate).ToList();
            var Malesmodels = db.CYCA_TotalMalesAdmittedPerDistrict1(Province_Id, StartDate, EndDate).ToList();

            var viewModels = new List<TotalRegionAdmissions>();
            var monthone = 0;
            var monthtwo = 0;
            var monththree = 0;
            var monthoneMale = 0;
            var monthtwoMale = 0;
            var monththreeMale = 0;
            foreach (var m in models)
            {
                var splitDate = Convert.ToDateTime(m.MONTH).Month;
                var Male = new CYCA_TotalMalesAdmittedPerDistrict1_Result();
                var reg = new TotalRegionAdmissions
                {
                    Province_Id = m.Province_Id,
                    Region_Id = m.District_Id,
                    Gender = m.Gender,
                    RegionName = m.RegionName
                };
                var males = Malesmodels.Where(a => a.District_Id == m.District_Id).ToList();
                foreach (var item in males)
                {
                    var split = Convert.ToDateTime(item.MONTH).Month;
                    if (split == splitDate)
                    {
                        Male = item;
                    }
                }

                switch (splitDate)
                {
                    case 1:
                        monthone = Convert.ToInt32(m.Total);
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Jan";
                        break;
                    case 2:
                        monthtwo = Convert.ToInt32(m.Total);
                        //reg.MonthTwo.MonthName = "Feb";
                        break;
                    case 3:
                        monththree = Convert.ToInt32(m.Total);
                        //reg.MonthThree.MonthName = "Mar";
                        break;
                    case 4:
                        monthone = Convert.ToInt32(m.Total);
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Apr";
                        break;
                    case 5:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "May";
                        break;
                    case 6:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
                        //reg.MonthThree.MonthName = "Jun";
                        break;
                    case 7:
                        monthone = Convert.ToInt32(m.Total);
                        //reg.MonthOne.MonthName = "Jul";
                        break;
                    case 8:
                        monthtwo = Convert.ToInt32(m.Total);
                        //reg.MonthTwo.MonthName = "Aug";
                        break;
                    case 9:
                        monththree = Convert.ToInt32(m.Total);
                        //reg.MonthThree.MonthName = "Sep";
                        break;
                    case 10:
                        monthone = Convert.ToInt32(m.Total);
                        //reg.MonthOne.MonthName = "Oct";
                        break;
                    case 11:
                        monthtwo = Convert.ToInt32(m.Total);
                        //reg.MonthTwo.MonthName = "Nov";
                        break;

                    case 12:
                        monththree = Convert.ToInt32(m.Total);
                        //reg.MonthThree.MonthName = "Dec";
                        break;
                    default:
                        break;
                }

                var first = new MonthDetails()
                {
                    MonthName = "Jan",
                    TotalFemales = monthone,
                    TotalMales = monthoneMale
                };
                var second = new MonthDetails()
                {
                    MonthName = "Feb",
                    TotalFemales = monthtwo,
                    TotalMales = monthtwoMale
                };
                var third = new MonthDetails()
                {
                    MonthName = "Mar",
                    TotalFemales = monththree,
                    TotalMales = monththreeMale
                };
                var disctrictExists = false;
                if (viewModels.Count > 0)
                {
                    foreach (var mod in viewModels)
                    {
                        if (mod.Region_Id == reg.Region_Id)
                        {
                            var model = viewModels.Where(v => v.Region_Id == reg.Region_Id).FirstOrDefault();
                            //var males = Malesmodels.Where(a => a.District_Id == m.District_Id).FirstOrDefault();
                            int index = viewModels.IndexOf(model);
                            disctrictExists = true;

                            if (first.TotalFemales != 0)
                            {
                                model.MonthOne.TotalFemales = first.TotalFemales;
                            }
                            if (second.TotalFemales != 0)
                            {
                                model.MonthTwo.TotalFemales = second.TotalFemales;
                            }
                            if (third.TotalFemales != 0)
                            {
                                model.MonthThree.TotalFemales = third.TotalFemales;
                            }


                            if (first.TotalMales != 0)
                            {
                                model.MonthOne.TotalMales = first.TotalMales;
                            }
                            if (second.TotalMales != 0)
                            {
                                model.MonthTwo.TotalMales = second.TotalMales;
                            }
                            if (third.TotalMales != 0)
                            {
                                model.MonthThree.TotalMales = third.TotalMales;
                            }
                            viewModels[index] = model;
                            break;
                        }
                        else
                        {
                            //monthone = 0;
                            //monthtwo = 0;
                            //monththree = 0;
                            disctrictExists = false;
                        }
                    }
                }
                else
                {
                    disctrictExists = true;
                    reg.MonthOne = new MonthDetails()
                    {
                        MonthName = "Jan",
                        TotalFemales = monthone,
                        TotalMales = monthoneMale
                    };
                    reg.MonthTwo = new MonthDetails()
                    {
                        MonthName = "Feb",
                        TotalFemales = monthtwo,
                        TotalMales = monthtwoMale
                    };
                    reg.MonthThree = new MonthDetails()
                    {
                        MonthName = "Mar",
                        TotalFemales = monththree,
                        TotalMales = monththreeMale
                    };
                    viewModels.Add(reg);
                }
                if (disctrictExists)
                {

                }
                else
                {
                    reg.MonthOne = new MonthDetails()
                    {
                        MonthName = "Jan",
                        TotalFemales = monthone,
                        TotalMales = monthoneMale
                    };
                    reg.MonthTwo = new MonthDetails()
                    {
                        MonthName = "Feb",
                        TotalFemales = monthtwo,
                        TotalMales = monthtwoMale
                    };
                    reg.MonthThree = new MonthDetails()
                    {
                        MonthName = "Mar",
                        TotalFemales = monththree,
                        TotalMales = monththreeMale
                    };
                    viewModels.Add(reg);
                }
                //foreach (var i in Malesmodels)
                //{
                //    totMale = i.Total;
                //}

                //viewModels.Add(reg);
            }




            return viewModels;
        }

        //Facility Admission
        public List<TotalFacilityAdmissions> GetTotalAdmittedKidsPerFacilty(int Facility_Id)
        {

            //int Province_Id = (from f in db.apl_Cyca_Facility
            //                   join p in db.Provinces on f.Province_Id equals p.Province_Id
            //                   where f.Facility_Id == Facility_Id
            //                   select p.Province_Id).SingleOrDefault();
            var StartDate = new DateTime(2020, 04, 01);
            var EndDate = new DateTime(2020, 06, 30);
            var models = db.CYCA_TotalFemalesAdmittedPerFacility(Facility_Id, StartDate, EndDate).ToList();
            var Malesmodels = db.CYCA_TotalMalesAdmittedPerFacility(Facility_Id, StartDate, EndDate).ToList();

            var viewModels = new List<TotalFacilityAdmissions>();
            var monthone = 0;
            var monthtwo = 0;
            var monththree = 0;
            var monthoneMale = 0;
            var monthtwoMale = 0;
            var monththreeMale = 0;
            foreach (var m in models)
            {
                var splitDate = Convert.ToDateTime(m.MONTH).Month;
                var Male = new CYCA_TotalMalesAdmittedPerFacility_Result();
                var reg = new TotalFacilityAdmissions
                {
                    Province_Id = m.Province_Id,
                    Region_Id = m.District_Id,
                    Facility_Id = m.Facility_Id,
                    Gender = m.Gender,
                    FacilityName = m.FacilityName
                };
                var males = Malesmodels.Where(a => a.District_Id == m.District_Id).ToList();
                foreach (var item in males)
                {
                    var split = Convert.ToDateTime(item.MONTH).Month;
                    if (split == splitDate)
                    {
                        Male = item;
                    }
                }

                switch (splitDate)
                {
                    case 1:
                        monthone = Convert.ToInt32(m.Total);
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Jan";
                        break;
                    case 2:
                        monthtwo = Convert.ToInt32(m.Total);
                        //reg.MonthTwo.MonthName = "Feb";
                        break;
                    case 3:
                        monththree = Convert.ToInt32(m.Total);
                        //reg.MonthThree.MonthName = "Mar";
                        break;
                    case 4:
                        monthone = Convert.ToInt32(m.Total);
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Apr";
                        break;
                    case 5:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "May";
                        break;
                    case 6:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
                        //reg.MonthThree.MonthName = "Jun";
                        break;
                    case 7:
                        monthone = Convert.ToInt32(m.Total);
                        //reg.MonthOne.MonthName = "Jul";
                        break;
                    case 8:
                        monthtwo = Convert.ToInt32(m.Total);
                        //reg.MonthTwo.MonthName = "Aug";
                        break;
                    case 9:
                        monththree = Convert.ToInt32(m.Total);
                        //reg.MonthThree.MonthName = "Sep";
                        break;
                    case 10:
                        monthone = Convert.ToInt32(m.Total);
                        //reg.MonthOne.MonthName = "Oct";
                        break;
                    case 11:
                        monthtwo = Convert.ToInt32(m.Total);
                        //reg.MonthTwo.MonthName = "Nov";
                        break;

                    case 12:
                        monththree = Convert.ToInt32(m.Total);
                        //reg.MonthThree.MonthName = "Dec";
                        break;
                    default:
                        break;
                }

                var first = new MonthDetails()
                {
                    MonthName = "Jan",
                    TotalFemales = monthone,
                    TotalMales = monthoneMale
                };
                var second = new MonthDetails()
                {
                    MonthName = "Feb",
                    TotalFemales = monthtwo,
                    TotalMales = monthtwoMale
                };
                var third = new MonthDetails()
                {
                    MonthName = "Mar",
                    TotalFemales = monththree,
                    TotalMales = monththreeMale
                };
                var disctrictExists = false;
                if (viewModels.Count > 0)
                {
                    foreach (var mod in viewModels)
                    {
                        if (mod.Facility_Id == reg.Facility_Id)
                        {
                            var model = viewModels.Where(v => v.Facility_Id == reg.Facility_Id).FirstOrDefault();
                            //var males = Malesmodels.Where(a => a.District_Id == m.District_Id).FirstOrDefault();
                            int index = viewModels.IndexOf(model);
                            disctrictExists = true;

                            if (first.TotalFemales != 0)
                            {
                                model.MonthOne.TotalFemales = first.TotalFemales;
                            }
                            if (second.TotalFemales != 0)
                            {
                                model.MonthTwo.TotalFemales = second.TotalFemales;
                            }
                            if (third.TotalFemales != 0)
                            {
                                model.MonthThree.TotalFemales = third.TotalFemales;
                            }


                            if (first.TotalMales != 0)
                            {
                                model.MonthOne.TotalMales = first.TotalMales;
                            }
                            if (second.TotalMales != 0)
                            {
                                model.MonthTwo.TotalMales = second.TotalMales;
                            }
                            if (third.TotalMales != 0)
                            {
                                model.MonthThree.TotalMales = third.TotalMales;
                            }
                            viewModels[index] = model;
                            break;
                        }
                        else
                        {
                            //monthone = 0;
                            //monthtwo = 0;
                            //monththree = 0;
                            disctrictExists = false;
                        }
                    }
                }
                else
                {
                    disctrictExists = true;
                    reg.MonthOne = new MonthDetails()
                    {
                        MonthName = "Jan",
                        TotalFemales = monthone,
                        TotalMales = monthoneMale
                    };
                    reg.MonthTwo = new MonthDetails()
                    {
                        MonthName = "Feb",
                        TotalFemales = monthtwo,
                        TotalMales = monthtwoMale
                    };
                    reg.MonthThree = new MonthDetails()
                    {
                        MonthName = "Mar",
                        TotalFemales = monththree,
                        TotalMales = monththreeMale
                    };
                    viewModels.Add(reg);
                }
                if (disctrictExists)
                {

                }
                else
                {
                    reg.MonthOne = new MonthDetails()
                    {
                        MonthName = "Jan",
                        TotalFemales = monthone,
                        TotalMales = monthoneMale
                    };
                    reg.MonthTwo = new MonthDetails()
                    {
                        MonthName = "Feb",
                        TotalFemales = monthtwo,
                        TotalMales = monthtwoMale
                    };
                    reg.MonthThree = new MonthDetails()
                    {
                        MonthName = "Mar",
                        TotalFemales = monththree,
                        TotalMales = monththreeMale
                    };
                    viewModels.Add(reg);
                }
                //foreach (var i in Malesmodels)
                //{
                //    totMale = i.Total;
                //}

                //viewModels.Add(reg);
            }




            return viewModels;
        }
        #endregion  


    }
}