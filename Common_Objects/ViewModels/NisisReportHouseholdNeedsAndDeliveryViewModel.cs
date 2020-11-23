using Common_Objects.Models;
using DotNet.Highcharts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class NisisReportHouseholdNeedsAndDeliveryViewModel
    {
        public int Selected_Province_Id { get; set; }

        public int Selected_Municipality_Id { get; set; }

        public int Selected_Local_Municipality_Id { get; set; }

        public int Selected_Ward_Id { get; set; }

        public int Selected_Site_Id { get; set; }

        public int Selected_Registered_Programme_Id { get; set; }

        public int Selected_Household_Delivery_Priority_Id { get; set; }

        public List<int> Selected_Grouping_Flag_Ids { get; set; }

        [Display(Name = "Province")]
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

        //public string Province_Description
        //{
        //    get
        //    {
        //        return Province_List.First(x => x.Selected).Text;
        //    }
        //}

        [Display(Name = "Municipality")]
        public SelectList Municipality_List
        {
            get
            {
                var municipalityModel = new DistrictModel();
                var listOfMunicipalities = municipalityModel.GetListOfDistricts(Selected_Province_Id);

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

        //public string Municipality_Description
        //{
        //    get
        //    {
        //        return Municipality_List.First(x => x.Selected).Text;
        //    }
        //}

        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Municipality_Id);

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

        //public string Local_Municipality_Description
        //{
        //    get
        //    {
        //        return Local_Municipality_List.First(x => x.Selected).Text;
        //    }
        //}

        [Display(Name = "Ward")]
        public SelectList Ward_List
        {
            get
            {
                var nisisWardModel = new NisisWardModel();
                var listOfNisisWards = nisisWardModel.GetListOfNisisWards(false, false, Selected_Local_Municipality_Id);

                var nisisWards = (from x in listOfNisisWards
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Ward_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Ward_Id.Equals(Selected_Ward_Id)
                                  }).ToList();

                var selectList = new SelectList(nisisWards, "Value", "Text", Selected_Ward_Id);

                return selectList;
            }
        }

        //public string Ward_Description
        //{
        //    get
        //    {
        //        return Ward_List.First(x => x.Selected).Text;
        //    }
        //}

        [Display(Name = "Site")]
        public SelectList Site_List
        {
            get
            {
                var siteModel = new NisisSiteModel();
                var listOfSites = siteModel.GetListOfNisisSites(false, false, Selected_Ward_Id);

                var sites = (from x in listOfSites
                             select new SelectListItem()
                             {
                                 Text = x.Site_Name,
                                 Value = x.Site_Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = x.Site_Id.Equals(Selected_Site_Id)
                             }).ToList();

                var selectList = new SelectList(sites, "Value", "Text", Selected_Site_Id);

                return selectList;
            }
        }

        //public string Site_Description
        //{
        //    get
        //    {
        //        return Site_List.First(x => x.Selected).Text;
        //    }
        //}

        [Display(Name = "Registered Programme")]
        public SelectList Registered_Programme_List
        {
            get
            {
                var programmeModel = new NisisProgrammeModel();
                var listOfProgrammes = programmeModel.GetListOfNisisProgrammes();

                var programmes = (from x in listOfProgrammes
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Programme_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Programme_Id.Equals(Selected_Registered_Programme_Id)
                                  }).ToList();

                var selectList = new SelectList(programmes, "Value", "Text", Selected_Registered_Programme_Id);

                return selectList;
            }
        }

        [Display(Name = "Priority")]
        public SelectList Household_Delivery_Priority_List
        {
            get
            {
                var referralPriorityModel = new NisisReferralPriorityModel();
                var listOfReferralPriorities = referralPriorityModel.GetListOfReferralPriorities();

                var referralPriorities = (from x in listOfReferralPriorities
                                          select new SelectListItem()
                                          {
                                              Text = x.Description,
                                              Value = x.Referral_Priority_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = x.Referral_Priority_Id.Equals(Selected_Household_Delivery_Priority_Id)
                                          }).ToList();

                var selectList = new SelectList(referralPriorities, "Value", "Text", Selected_Household_Delivery_Priority_Id);

                return selectList;
            }
        }

        [Display(Name = "Date From")]
        public DateTime? Date_From { get; set; }

        [Display(Name = "Date To")]
        public DateTime? Date_To { get; set; }

        [Display(Name = "Grouping Flags")]
        public List<NISIS_Grouping_Flag> Grouping_Flag_List
        {
            get
            {
                var groupingFlagList = new List<NISIS_Grouping_Flag>();

                var groupingFlagModel = new NisisGroupingFlagModel();
                var listOfGroupingFlags = groupingFlagModel.GetListOfGroupingFlags();

                groupingFlagList.AddRange(listOfGroupingFlags);

                return groupingFlagList;
            }

        }

        public bool DisplayReport { get; set; }

        public List<ServiceCategory> ServiceCategories { get; set; }

        public Highcharts ReferralsByStatusChart { get; set; }

        public Highcharts ReferralsByServiceCategory { get; set; }
    }

    public class ServiceStatusColumn
    {
        public string ColumnHeader { get; set; }
        public int ColumnValue { get; set; }
    }

    public class ServiceStatus
    {
        public string ServiceDescription { get; set; }
        public List<ServiceStatusColumn> ServiceColumns { get; set; }
    }

    public class ServiceCategory
    {
        public string ServiceCategoryDescription { get; set; }
        public List<ServiceStatus> Services { get; set; }
    }
}
