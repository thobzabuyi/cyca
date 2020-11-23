using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CYCAReportsViewModel
    {
        public int Facility_Id;
        public int admissionID;
        public int Province_Id;        
        public List<AdmissionReportViewModel> AdmissionReportViewModels { get; set; }
        public List<ChildrenWithIllegalItemsReportViewModel> ChildrenWithIllegalItemsReportViewModels { get; set; }
        public List<ChildrenInGangReportViewModel> ChildrenInGangReportViewModels { get; set; }
        public List<PregnantChildrenReportViewModel> PregnantChildrenReportViewModels { get; set; }
        public List<DischargedChildrenReportViewModel> DischargedChildrenReportViewModels { get; set; }
        public List<ReportableIncidentReportViewModel> ReportableIncidentReportViewModels { get; set; }
        public List<TotalProvinceAdmissionsViewModel> TotalProvinceAdmissionsViewModels { get; set; }
        public List<TotalRegionAdmissions> TotalRegionAdmissionss { get; set; }
        public List<TotalFacilityAdmissions> TotalFacilityAdmissionss { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

        public List<int> GetYears()
        {
            List<int> Years = new List<int>();
            DateTime startYear = DateTime.Now;
            while (startYear.Year <= DateTime.Now.AddYears(3).Year)
            {
                Years.Add(startYear.Year);
                startYear = startYear.AddYears(1);
            }
            return Years;
        }
        public SelectList GetQuarterMonths()
        {
            var list = new SelectList(new[]
                                         {
                                              new {ID="1",display="Jan - Mar"},
                                              new{ID="2",display="Apr - Jun"},
                                              new{ID="3",display="Jul - Sep"},
                                              new{ID="4",display="Oct - Dec"},
                                          },
                           "ID", "display", 1);
            return list;
        }
        public SelectList GetQuarterMonth(int selected)
        {
            var list = new SelectList(new[]
                                         {
                                              new {ID="1",display="Jan - Mar"},
                                              new{ID="2",display="Apr - Jun"},
                                              new{ID="3",display="Jul - Sep"},
                                              new{ID="4",display="Oct - Dec"},
                                          },
                           "ID", "display", selected);
            return list;
        }
        public Quartters GetDates(int quarter,int year)
        {
            var result = new Quartters();
            switch (quarter) {
                case 1:
                    var d = new DateTime(year, 01, 01);

                    result =  new Quartters {
                        StartDate = new DateTime(year, 01, 01),
                        EndDate = new DateTime(year, 03, 31)
                        };
                    break;
                case 2:
                    result = new Quartters
                    {
                        StartDate = new DateTime(year, 04, 04),
                        EndDate = new DateTime(year, 06, 30)
                    };
                    break;
                case 3:
                    result = new Quartters
                    {
                        StartDate = new DateTime(year, 07, 01),
                        EndDate = new DateTime(year, 09, 30)
                    };
                    break;
                case 4:
                    result = new Quartters {
                        StartDate = new DateTime(year, 10, 01),
                        EndDate = new DateTime(year, 12, 31)
                        };
                    break;
            }
            return result;
        }

        public List<TotalRegionAdmissions> GetTotalAdmittedKidsPerDistrict(int Facility_Id,Quartters dates)
        {

            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
      
            var models = db.CYCA_TotalFemalesAdmittedPerDistrict1(Province_Id, dates.StartDate, dates.EndDate).ToList();
            var Malesmodels = db.CYCA_TotalMalesAdmittedPerDistrict1(Province_Id, dates.StartDate, dates.EndDate).ToList();
            //test
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

        public List<TotalFacilityAdmissions> GetTotalAdmittedKidsPerFacilty(int Facility_Id, Quartters dates)
        {

            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
            //var StartDate = new DateTime(2020, 04, 01);
            //var EndDate = new DateTime(2020, 06, 30);
            var models = db.CYCA_TotalFemalesAdmittedPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();
            var Malesmodels = db.CYCA_TotalMalesAdmittedPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();

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
        //public List<TotalProvinceAdmissions> TotalProvinceAdmissionss { get; set; }
        //public TotalProvinceAdmissions TotalAdmit { get; set; }

        public List<TotalFacilityAdmissions> GetTotalPregnantChildrenPerFacilty(int Facility_Id, Quartters dates)
        {

            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
            //var StartDate = new DateTime(2020, 04, 01);
            //var EndDate = new DateTime(2020, 06, 30);
            var models = db.CYCA_TotalPregnantFemalesPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();
            //var Malesmodels = db.CYCA_TotalMalesAdmittedPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();

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
                //var males = Malesmodels.Where(a => a.Facility_Id == m.Facility_Id).ToList();
                //foreach (var item in males)
                //{
                //    var split = Convert.ToDateTime(item.MONTH).Month;
                //    if (split == splitDate)
                //    {
                //        Male = item;
                //    }
                //}

                switch (splitDate)
                {
                    case 1:
                        monthone = Convert.ToInt32(m.Total);
                        //monthoneMale = Convert.ToInt32(Male.Total);
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
                        //monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Apr";
                        break;
                    case 5:
                        monthtwo = Convert.ToInt32(m.Total);
                        //monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "May";
                        break;
                    case 6:
                        monththree = Convert.ToInt32(m.Total);
                        //monththreeMale = Convert.ToInt32(Male.Total);
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

        public List<TotalFacilityAdmissions> GetTotalDischargedPerFacilty(int Facility_Id, Quartters dates)
        {
            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
            //var StartDate = new DateTime(2020, 04, 01);
            //var EndDate = new DateTime(2020, 06, 30);
            var models = db.CYCA_TotalFemalesDischargedPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();
            var Malesmodels = db.CYCA_TotalMalesDischargedPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();

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
                var Male = new CYCA_TotalMalesDischargedPerFacility_Result();
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
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Feb";
                        break;
                    case 3:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
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
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Jul";
                        break;
                    case 8:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Aug";
                        break;
                    case 9:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
                        //reg.MonthThree.MonthName = "Sep";
                        break;
                    case 10:
                        monthone = Convert.ToInt32(m.Total);
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Oct";
                        break;
                    case 11:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Nov";
                        break;

                    case 12:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
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
        public List<TotalFacilityAdmissions> GetTotalChildrenInGangsPerFacilty(int Facility_Id, Quartters dates)
        {
            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
            //var StartDate = new DateTime(2020, 04, 01);
            //var EndDate = new DateTime(2020, 06, 30);
            var models = db.CYCA_TotalFemalesInGangsPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();
            var Malesmodels = db.CYCA_TotalMalesInGangsPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();

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
                var Male = new CYCA_TotalMalesInGangsPerFacility_Result();
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
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Feb";
                        break;
                    case 3:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
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
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Jul";
                        break;
                    case 8:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Aug";
                        break;
                    case 9:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
                        //reg.MonthThree.MonthName = "Sep";
                        break;
                    case 10:
                        monthone = Convert.ToInt32(m.Total);
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Oct";
                        break;
                    case 11:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Nov";
                        break;

                    case 12:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
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
        public List<TotalFacilityAdmissions> GetTotalChildrenWithIllegalItemsPerFacilty(int Facility_Id, Quartters dates)
        {
            int Province_Id = (from f in db.apl_Cyca_Facility
                               join p in db.Provinces on f.Province_Id equals p.Province_Id
                               where f.Facility_Id == Facility_Id
                               select p.Province_Id).SingleOrDefault();
            //var StartDate = new DateTime(2020, 04, 01);
            //var EndDate = new DateTime(2020, 06, 30);
            var models = db.CYCA_TotalFemalesWithIllegalItemsPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();
            var Malesmodels = db.CYCA_TotalMalesWithIllegalItemsPerFacility(Facility_Id, dates.StartDate, dates.EndDate).ToList();

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
                var Male = new CYCA_TotalMalesWithIllegalItemsPerFacility_Result();
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
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Feb";
                        break;
                    case 3:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
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
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Jul";
                        break;
                    case 8:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Aug";
                        break;
                    case 9:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
                        //reg.MonthThree.MonthName = "Sep";
                        break;
                    case 10:
                        monthone = Convert.ToInt32(m.Total);
                        monthoneMale = Convert.ToInt32(Male.Total);
                        //reg.MonthOne.MonthName = "Oct";
                        break;
                    case 11:
                        monthtwo = Convert.ToInt32(m.Total);
                        monthtwoMale = Convert.ToInt32(Male.Total);
                        //reg.MonthTwo.MonthName = "Nov";
                        break;

                    case 12:
                        monththree = Convert.ToInt32(m.Total);
                        monththreeMale = Convert.ToInt32(Male.Total);
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
   
    public class AdmissionReportViewModel
    {
        public int Facility_Id { get; set; }
        public string ProvinceName { get; set; }
        public string RegionName { get; set; }
        public string CenterName { get; set; }
        public string AdmissionDate { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string FullName { get; set; }
        public string admissionReason { get; set; }
       // public string Description { get; set; }
    }
    public class PregnantChildrenReportViewModel
    {
        public int Facility_Id { get; set; }
        public string ProvinceName { get; set; }
        public string RegionName { get; set; }
        public string CenterName { get; set; }
        public string AdmissionDate { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string FullName { get; set; }
        public string AdmissionReason { get; set; }
        public string PregnancyTested { get; set; }
        public string IsPregnant { get; set; }
        public string IsSexuallyMolested { get; set; }
    }

    public class DischargedChildrenReportViewModel
    {
        public int Facility_Id { get; set; }
        public string ProvinceName { get; set; }
        public string RegionName { get; set; }
        public string CenterName { get; set; }
        public string AdmissionDate { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string FullName { get; set; }
        public string AdmissionReason { get; set; }
        public string DischargeDate { get; set; }        
    }

    public class ReportableIncidentReportViewModel
    {
        public int Facility_Id { get; set; }
        public string ProvinceName { get; set; }
        public string RegionName { get; set; }
        public string CenterName { get; set; }
        public string AdmissionDate { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string FullName { get; set; }
        public string AdmissionReason { get; set; }
        public string IncidentDate { get; set; }
    }

    public class ChildrenWithIllegalItemsReportViewModel
    {
        public int Facility_Id { get; set; }
        public string ProvinceName { get; set; }
        public string RegionName { get; set; }
        public string CenterName { get; set; }
        public string AdmissionDate { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string FullName { get; set; }
        public string admissionReason { get; set; }
        public string IllegalItems { get; set; }
    }

    public class ChildrenInGangReportViewModel
    {
        public int Facility_Id { get; set; }
        public string ProvinceName { get; set; }
        public string RegionName { get; set; }
        public string CenterName { get; set; }
        public string AdmissionDate { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string FullName { get; set; }
        public string admissionReason { get; set; }
        public string GangMembership { get; set; }
    }

    public class TotalProvinceAdmissionsViewModel
    {
        public int Facility_Id { get; set; }
        public int Region_Id { get; set; }
        public int Province_Id { get; set; }
        public string RegionName { get; set; }
        public string ProvinceName { get; set; }
        public string Gender { get; set; }
        public string FacilityName { get; set; }
        public int? TotalMale { get; set; }
        public int? TotalFemales { get; set; }
        public List<TotalRegionAdmissions> TotalRegion { get; set; }

    }

    public class TotalRegionAdmissions
    {
        public int Facility_Id { get; set; }
        public int Region_Id { get; set; }
        public int Province_Id { get; set; }
        public string RegionName { get; set; }
        public string Gender { get; set; }
        public string FacilityName { get; set; }
        public int? TotalMale { get; set; }
        public int? TotalFemales { get; set; }
        public MonthDetails MonthOne { get; set; }
        public MonthDetails MonthTwo { get; set; }
        public MonthDetails MonthThree { get; set; }
        public DateTime Admission_Date { get; set; }
        public List<TotalFacilityAdmissions> TotalFacility { get; set; }
        
    }
    public class TotalFacilityAdmissions
    {
        public int Facility_Id { get; set; }
        public int Region_Id { get; set; }
        public int Province_Id { get; set; }
        public string Gender { get; set; }
        public string FacilityName { get; set; }
        public int? TotalMale { get; set; }
        public int? TotalFemales { get; set; }
        public MonthDetails MonthOne { get; set; }
        public MonthDetails MonthTwo { get; set; }
        public MonthDetails MonthThree { get; set; }
        public DateTime Admission_Date { get; set; }

        public static implicit operator TotalFacilityAdmissions(TotalRegionAdmissions v)
        {
            throw new NotImplementedException();
        }

    }

    //public class TotalPregnantChildrenPerFacilityViewModel
    //{
    //    public int Facility_Id { get; set; }
    //    public string ProvinceName { get; set; }
    //    public string RegionName { get; set; }
    //    public string CenterName { get; set; }
    //    public string AdmissionDate { get; set; }
    //    public string Gender { get; set; }
    //    public int? Age { get; set; }
    //    public string FullName { get; set; }
    //    public string AdmissionReason { get; set; }
    //    public string PregnancyTested { get; set; }
    //    public string IsPregnant { get; set; }
    //    public string IsSexuallyMolested { get; set; }

    //}
    public enum Months
    {
        Jan = 1,
        Feb =2,
        Mar = 3,
        Apr = 4,
        May = 5,
        Jun = 6,
        Jul = 7,
        Aug = 8,
        Sep = 9,
        Oct = 10,
        Nov = 11,
        Dec = 12
    }

    public class Quartters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class MonthDetails
    {
        public string MonthName { get; set; }
        public int TotalFemales { get; set; }
        public int TotalMales { get; set; }
    }

   

  



}
