using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Common_Objects;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using CYCA_Module_V2.Common_Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CYCA_Module_V2.Controllers
{
    public class ReportsController : Controller
    {
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        CYCAReportsViewModel reportModel = new CYCAReportsViewModel();
        // GET: Reports
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

            //test
            //quarters.Add(new  dynamic(){
            //    QuarterName = "Jan - Mar"
                
            //});

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
        public CYCAReportsViewModel GetFacilityAdmissions(int Facility_Id, int page,int quarter, int year)
        {
            var date = reportModel.GetDates(quarter, year);

            var admissionHistory = GetAdmissionReport(Facility_Id, page);
            var totalProvinceAdmission = GetTotalAdmittedKidsPerProvince(Facility_Id);           
            var totalFacilityAdmission = reportModel.GetTotalAdmittedKidsPerFacilty(Facility_Id, date);
            var totalRegionAdmission =reportModel.GetTotalAdmittedKidsPerDistrict(Facility_Id, date);
            var records = new CYCAReportsViewModel
            {
                Facility_Id = Facility_Id,
                AdmissionReportViewModels = admissionHistory,
                TotalProvinceAdmissionsViewModels = totalProvinceAdmission,
                TotalRegionAdmissionss = totalRegionAdmission,
                TotalFacilityAdmissionss = totalFacilityAdmission


            };

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

        public int GetFacilityIdByUserID(int UserId)
        {
            return (from f in db.apl_Cyca_Facility
                    join e in db.Employees on f.Facility_Id equals e.Facility_Id
                    join u in db.Users on e.User_Id equals u.User_Id
                    where u.User_Id == UserId
                    select f.Facility_Id).SingleOrDefault();

        }

        //public int GetProvinceIdByUserID(int UserId)
        //{
        //    return (from f in db.apl_Cyca_Facility
        //            join u in db.Users on f.Facility_Id equals u.Facility_Id
        //            join e in db.Employees on f.Facility_Id equals e.Facility_Id
        //            where u.User_Id == UserId
        //            select f.Facility_Id).SingleOrDefault();

        //}

        //public List<ChildrenWithIllegalItemsReportViewModel> GetChildrenWithIllegalItemsReport()
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
        //    int Facility_Id = GetProvinceIdByUserID(userId);
        //    var models = db.CYCA_NewAdmissionsFacilityReport(Facility_Id).ToList();
        //    var viewModels = new List<ChildrenWithIllegalItemsReportViewModel>();

        //    foreach (var m in models)
        //    {

        //        viewModels.Add(new ChildrenWithIllegalItemsReportViewModel
        //        {
        //            Facility_Id = m.Facility_Id,
        //            ProvinceName = m.Province,
        //            RegionName = m.Region,
        //            CenterName = m.FacilityName,
        //            AdmissionDate = m.Admission_Date.ToString(),
        //            FullName = m.FullName,
        //            admissionReason = m.Admission_Reason




        //        });
        //    }
        //    return viewModels;
        //}

        public List<AdmissionReportViewModel> GetAdmissionReport()
        {
            //var adIds = db.CYCA_Admissions_AdmissionDetails.Where(a => a.Client_Id == clientID).Select(ad => ad.Admission_Id).ToList();
            //var model = db.CYCA_Tatoos.Where(g => adIds.Contains(g.Admission_Id ?? default(int))).ToList();
            //var viewModels = new List<TatooVewModel>();



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

            int Facility_Id = GetFacilityIdByUserID(userId);
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
                    AdmissionDate = m.Admission_Date.ToString(),
                    FullName = m.FullName,
                    admissionReason = m.Admission_Reason




                });
            }

            return viewModels;
        }

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

                });
            }




            return viewModels;
        }

        public List<TotalRegionAdmissions> GetTotalAdmittedKidsPerDistrict(int Facility_Id)
        {

            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
            var StartDate = new DateTime(2020,04,01);
            var EndDate = new DateTime(2020,06,30);           
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
                        monthone= Convert.ToInt32(m.Total);
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

                var first = new MonthDetails() {
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

        //Facility
        public List<TotalFacilityAdmissions> GetTotalAdmittedKidsPerFacilty(int Facility_Id)
        {

            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
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
                var males = Malesmodels.Where(a => a.Facility_Id == m.Facility_Id).ToList();
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
    }
}

