using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Helpers;
using Common_Objects.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using System.Data.Entity;

namespace Common_Objects.ViewModels
{
    public class CPR_ReportingViewModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public int Selected_PartA_ReportId { get; set; }
        public SelectList PartA_ReportTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var evaluationList = (from a in _db.CPR_PartA_ReportTypes
                                      select a).ToList();

                var evaluation = (from m in evaluationList
                                  select new SelectListItem()
                                  {
                                      Text = m.Description,
                                      Value = m.Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = m.Id.Equals(Selected_PartA_ReportId)
                                  }).ToList();

                var selectList = new SelectList(evaluation, "Value", "Text", Selected_PartA_ReportId);
                return selectList;
            }
        }

        public int SelectedAbuseType_Id { get; set; }
        public SelectList AbuseTypeList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Abuse_Types
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Abuse_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Abuse_Type_Id.Equals(SelectedAbuseType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedAbuseType_Id);

                return selectList;
            }
        }
    
        public int? Selected_Province_Id { get; set; }

        public int? Selected_Municipality_Id { get; set; }

        public int? Selected_Local_Municipality_Id { get; set; }

        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provinces = (from x in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Province_Id.Equals(Selected_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }

        public SelectList Municipality_List
        {
            get
            {
                var municipalityModel = new DistrictModel();
                var listOfMunicipalities = municipalityModel.GetListOfDistricts(Selected_Province_Id ?? -1);

                var municipalities = (from x in listOfMunicipalities
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.District_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.District_Id.Equals(Selected_Municipality_Id)
                                      }).ToList();

                var selectList = new SelectList(municipalities, "Value", "Text", Selected_Municipality_Id);

                return selectList;
            }
        }

        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Municipality_Id ?? -1);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Local_Municipality_Id.Equals(Selected_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Selected_Local_Municipality_Id);

                return selectList;
            }
        }

        public int SelectedPopulationGroup_Id { get; set; }
        public SelectList PopulationGroupList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Population_Groups
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Population_Group_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Population_Group_Id.Equals(SelectedPopulationGroup_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedPopulationGroup_Id);

                return selectList;
            }
        }

        public int SelectedGenderId { get; set; }
        public SelectList GenderList
        {
            get
            {
                var genders = new GenderModel();
                var listOfGenders = genders.GetListOfGenders();

                var employers = (from m in listOfGenders
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGenderId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGenderId);

                return selectList;
            }
        }

        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        public int Selected_Removals_SubId { get; set; }
        public SelectList Removals_SubReportList
        {
            get
            {
                List<SelectListItem> PartBReportTypes = new List<SelectListItem>();
                SelectListItem reportType = new SelectListItem();

                reportType.Text = "Individual";
                reportType.Value = "1";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Employer";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Both";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                var selectList = new SelectList(PartBReportTypes, "Value", "text", Selected_Removals_SubId);

                return selectList;
            }
        }

        //individual, employer, both
        public int Selected_Inquiries_SubId { get; set; }
        public SelectList Inquiries_SubReportList
        {
            get
            {
                List<SelectListItem> PartBReportTypes = new List<SelectListItem>();
                SelectListItem reportType = new SelectListItem();

                reportType.Text = "Individual";
                reportType.Value = "1";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Employer";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Both";
                reportType.Value = "3";

                PartBReportTypes.Add(reportType);

                var selectList = new SelectList(PartBReportTypes, "Value", "text", Selected_Inquiries_SubId);

                return selectList;
            }
        }
        
        //nr of closed cases, convictions
        public int Selected_CourtCases_SubId { get; set; }
        public SelectList CourtCases_SubReportList
        {
            get
            {
                List<SelectListItem> PartBReportTypes = new List<SelectListItem>();
                SelectListItem reportType = new SelectListItem();

                reportType.Text = "Nr of Closed Cases";
                reportType.Value = "1";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Convictions";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                var selectList = new SelectList(PartBReportTypes, "Value", "text", Selected_CourtCases_SubId);

                return selectList;
            }
        }

        //finalised cases, outstanding cases
        public int Selected_Form36_SubId { get; set; }
        public SelectList Form36_SubReportList
        {
            get
            {
                List<SelectListItem> PartBReportTypes = new List<SelectListItem>();
                SelectListItem reportType = new SelectListItem();

                reportType.Text = "Finalised Cases";
                reportType.Value = "1";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Outstanding Cases";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                var selectList = new SelectList(PartBReportTypes, "Value", "text", Selected_Form36_SubId);

                return selectList;
            }
        }

        //mandatory obliged, member of the public
        public int Selected_OnlineNotification_ReportId { get; set; }
        public SelectList OnlineNotification_ReportList
        {
            get
            {
                //(Unsuitability, Inquires, Removals)
                List<SelectListItem> PartBReportTypes = new List<SelectListItem>();
                SelectListItem reportType = new SelectListItem();

                reportType.Text = "Mandatory Obliged";
                reportType.Value = "1";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Member Of the Public";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                var selectList = new SelectList(PartBReportTypes, "Value", "text", Selected_OnlineNotification_ReportId);

                return selectList;
            }
        }

        //(individual, employer, both)
        public int Selected_Notification_ReportId { get; set; }

        public SelectList Notification_ReportList
        {
            get
            {
                List<SelectListItem> PartBReportTypes = new List<SelectListItem>();
                SelectListItem reportType = new SelectListItem();

                reportType.Text = "Individual";
                reportType.Value = "1";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Employer";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Both";
                reportType.Value = "3";

                PartBReportTypes.Add(reportType);

                var selectList = new SelectList(PartBReportTypes, "Value", "text", Selected_Notification_ReportId);

                return selectList;
            }
        }

        public int Selected_PartB_ReportId { get; set; }
        public SelectList PartB_ReportList
        {
            get
            {
                //(Unsuitability, Inquires, Removals)
                List<SelectListItem> PartBReportTypes = new List<SelectListItem>();
                SelectListItem reportType = new SelectListItem();

                reportType.Text = "Unsuitability";
                reportType.Value = "1";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Inquires";
                reportType.Value = "2";

                PartBReportTypes.Add(reportType);

                reportType = new SelectListItem();

                reportType.Text = "Removals";
                reportType.Value = "3";

                PartBReportTypes.Add(reportType);

                var selectList = new SelectList(PartBReportTypes, "Value", "text", Selected_PartB_ReportId);

                return selectList;
            }
        }

        public int Selected_InformantType_Id { get; set; }
        public SelectList InformantTypeId
        {
            get

            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.CPR_MandatoryObliged
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.MandatoryObliged_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.MandatoryObliged_Id.Equals(Selected_InformantType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", Selected_InformantType_Id);

                return selectList;
            }
        }

        public int SelectedObliged_Id { get; set; }
        public SelectList MandatoryObligedList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.CPR_MandatoryObliged
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.MandatoryObliged_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.MandatoryObliged_Id.Equals(SelectedObliged_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedObliged_Id);

                return selectList;
            }
        }


        //CPR General Info for report views
        public Highcharts OverallIncidents { get; set; }
        public List<CPR_Report_CourtCases> OverallIncidentsList { get; set; }

        public Highcharts QuarterlyIncidents { get; set; }
        public List<fullYearTable> QuarterleyIncidentsList { get; set; }

        public Highcharts OverallUnsuitabilityInquiries { get; set; }
        public List<CPR_Report_Unsuitabiluity> OverallUnsuitabilityInquiriesList { get; set; }

        public Highcharts UnsuitabilityOverallForYear { get; set; }
        public List<fullYearTable> UnsuitabilityOverallForYearList { get; set; }

        public Highcharts InquiriesOverallForYear { get; set; }
        public List<InquiriesTable> InquiriesOverallForYearList { get; set; }

        public Highcharts PartAReportChart { get; set; }
        public List<CPR_Report_CourtCases> PartAReportResults { get; set; }
        public int PartAReportRecordCount { get; set; }

        public Highcharts OnlineChart { get; set; }
        public List<CPR_Report_Notifications_Mandatory> OnlineResultsMan { get; set; }
        public List<CPR_Report_Notification_Public> OnlineResultsPub { get; set; }
        public int OnlineRecordCount { get; set; }
        public string OnlineReportTypeID { get; set; }



        public Highcharts PartBReportChart { get; set; }
        public List<CPR_Report_Unsuitabiluity> PartBReportResults { get; set; }
        public List<CPR_Report_Inquiies> PartBReportResultsInquiries { get; set; }
        public List<CPR_Report_Removals> PartBReportResultsRemovals { get; set; }
        public int PartBReportRecordCount { get; set; }
        public string PartBReportTypeID { get; set; }


        public  List<WebGridColumn> DynamicGridColumns { get; set; }
        
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }

    }
}
