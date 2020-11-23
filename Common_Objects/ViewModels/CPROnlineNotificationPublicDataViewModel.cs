using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using System.Web.Mvc;
using System.Globalization;

namespace Common_Objects.ViewModels
{
    public class CPROnlineNotificationPublicDataViewModel
    {
        public CPR_OnlineNotification__ChildDetails ChildDetails { get; set; }
        public CPR_OnlineNotification_AlledgedOffender AlledgedOffender { get; set; }
        public CPR_OnlineNotification_FirstReporter FirstReporter { get; set; }
        public CPR_OnlineNotifications_Incedent Incident { get; set; }
        public CPR_OnlineNotifications_Reporter Reporter { get; set; }

        public int SelectedIdType_Id { get; set; }
        public SelectList IdTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIdTypes = (from m in _db.Identification_Types
                                     select m).ToList();

                var employers = (from m in listOfIdTypes
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Identification_Type_Id.Equals(SelectedIdType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedIdType_Id);

                return selectList;
            }
        }


        public int SelectedRiskIndicator_Id { get; set; }
        public SelectList RiskIndicators
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIndicators = (from r in _db.Risk_Indicators
                                        select r).ToList();

                var indicators = (from m in listOfIndicators
                                  select new SelectListItem()
                                  {
                                      Text = m.Description,
                                      Value = m.Risk_Indicator_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = m.Risk_Indicator_Id.Equals(SelectedRiskIndicator_Id)
                                  }).ToList();

                var selectList = new SelectList(indicators, "Value", "Text", SelectedRiskIndicator_Id);

                return selectList;
            }
        }

        public int SelectedCourt_Id { get; set; }

        public SelectList Court_List
        {
            get
            {
                var courtModel = new CourtModel();
                var listOfCourts = courtModel.GetListOfCourts();

                var courtsList = (from c in listOfCourts
                                  select new SelectListItem()
                                  {
                                      Text = c.Description,
                                      Value = c.Court_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = c.Court_Id.Equals(SelectedCourt_Id)
                                  }).ToList();

                var selectList = new SelectList(courtsList, "Value", "Text", SelectedCourt_Id);

                return selectList;
            }
        }

        //public int SelectedGender_Id { get; set; }
        //public SelectList GenderList
        //{
        //    get
        //    {
        //        var _db = new SDIIS_DatabaseEntities();
        //        var listOfTitles = (from m in _db.Genders
        //                            select m).ToList();

        //        var employers = (from m in listOfTitles
        //                         select new SelectListItem()
        //                         {
        //                             Text = m.Description,
        //                             Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
        //                             Selected = m.Gender_Id.Equals(SelectedGender_Id)
        //                         }).ToList();

        //        var selectList = new SelectList(employers, "Value", "Text", SelectedGender_Id);

        //        return selectList;
        //    }
        //}

        //public int SelectedAbuseType_Id { get; set; }
        //public SelectList AbuseTypeList
        //{
        //    get
        //    {
        //        var _db = new SDIIS_DatabaseEntities();
        //        var listOfTitles = (from m in _db.Abuse_Types
        //                            select m).ToList();

        //        var employers = (from m in listOfTitles
        //                         select new SelectListItem()
        //                         {
        //                             Text = m.Description,
        //                             Value = m.Abuse_Type_Id.ToString(CultureInfo.InvariantCulture),
        //                             Selected = m.Abuse_Type_Id.Equals(SelectedAbuseType_Id)
        //                         }).ToList();

        //        var selectList = new SelectList(employers, "Value", "Text", SelectedAbuseType_Id);

        //        return selectList;
        //    }
        //}

        //public int SelectedMagDistrict { get; set; }
        //public SelectList MagistrateList
        //{
        //    get
        //    {
        //        var districts = new DistrictModel();
        //        var listOfDistricts = districts.GetListOfDistricts();

        //        var employers = (from m in listOfDistricts
        //                         select new SelectListItem()
        //                         {
        //                             Text = m.Description,
        //                             Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
        //                             Selected = m.District_Id.Equals(SelectedMagDistrict)
        //                         }).ToList();

        //        var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

        //        return selectList;
        //    }
        //}

        //public int SelectedProvinceId { get; set; }
        //public SelectList ProvinceList
        //{
        //    get
        //    {
        //        var provinces = new ProvinceModel();
        //        var listOfProvinces = provinces.GetListOfProvinces();

        //        var employers = (from m in listOfProvinces
        //                         select new SelectListItem()
        //                         {
        //                             Text = m.Description,
        //                             Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
        //                             Selected = m.Province_Id.Equals(SelectedProvinceId)
        //                         }).ToList();

        //        var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

        //        return selectList;
        //    }
        //}

        //public int RelationShipTypeId { get; set; }
        //public SelectList RelationshipTypeList
        //{
        //    get
        //    {
        //        var relationships = new RelationshipTypeModel();
        //        var listOfRelationships = relationships.GetListOfRelationshipTypes();

        //        var employers = (from m in listOfRelationships
        //                         select new SelectListItem()
        //                         {
        //                             Text = m.Description,
        //                             Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
        //                             Selected = m.Relationship_Type_Id.Equals(RelationShipTypeId)
        //                         }).ToList();

        //        var selectList = new SelectList(employers, "Value", "Text", RelationShipTypeId);

        //        return selectList;
        //    }
        //}


        //public int SelectedTown_Id { get; set; }
        //public SelectList TownList
        //{
        //    get
        //    {
        //        var towns = new TownModel();
        //        var listOfTowns = towns.GetListOfTowns();

        //        var employers = (from m in listOfTowns
        //                         select new SelectListItem()
        //                         {
        //                             Text = m.Description,
        //                             Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
        //                             Selected = m.Town_Id.Equals(SelectedTown_Id)
        //                         }).ToList();

        //        var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

        //        return selectList;
        //    }
        //}

    }
}
