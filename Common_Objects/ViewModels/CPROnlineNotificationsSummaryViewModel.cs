using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using System.Globalization;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CPROnlineNotificationsSummaryViewModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public CPR_OnlineNotification__ChildDetails ChildDetails { get; set; }
        public CPR_OnlineNotification_AlledgedOffender AlledgedOffender { get; set; }
        public CPR_OnlineNotification_FirstReporter FirstReporter { get; set; }
        public CPR_OnlineNotifications_Incedent Incident { get; set; }
        public CPR_OnlineNotification_MandatoryReporter ManReporter { get; set; }
        public CPR_OnlineNotifications_Reporter Reporter { get; set; }



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

        public int SelectedGender_Id { get; set; }
        public SelectList GenderList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Genders
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGender_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGender_Id);

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

        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }

        public int RelationShipTypeId { get; set; }
        public SelectList RelationshipTypeList
        {
            get
            {
                var relationships = new RelationshipTypeModel();
                var listOfRelationships = relationships.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(RelationShipTypeId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", RelationShipTypeId);

                return selectList;
            }
        }


        public int SelectedTown_Id { get; set; }

        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

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


        public CPR_OnlineNotification__ChildDetails GetChildDetails(int Id)
        {
            return db.CPR_OnlineNotification__ChildDetails.Where(x => x.ChildDetails_Id == Id).FirstOrDefault();
        }

        public CPR_OnlineNotifications_Incedent GetIncident(int? Incedent_Id)
        {
            return db.CPR_OnlineNotifications_Incedent.Where(x => x.Incedent_Id == Incedent_Id).FirstOrDefault();
        }

        public CPR_OnlineNotification_AlledgedOffender GetAlledgedOffender(int? AllegedOffender_Id)
        {
            return db.CPR_OnlineNotification_AlledgedOffender.Where(x => x.AllegedOffender_Id == AllegedOffender_Id).FirstOrDefault();
        }
        public CPR_OnlineNotification_FirstReporter GetFirstReporter(int? FirstReporter_Id)
        {
            return db.CPR_OnlineNotification_FirstReporter.Where(x => x.FirstReporter_Id == FirstReporter_Id).FirstOrDefault();
        }
        public CPR_OnlineNotifications_Reporter GetReported(int? Reporter_Id)
        {
            return db.CPR_OnlineNotifications_Reporter.Where(x => x.Reporter_Id == Reporter_Id).FirstOrDefault();
        }
        public CPR_OnlineNotification_MandatoryReporter GetMandatoryReporter(int? MandatoryReporter_Id)
        {
            return db.CPR_OnlineNotification_MandatoryReporter.Where(x => x.Reporter_Id == MandatoryReporter_Id).FirstOrDefault();
        }
    }
}
