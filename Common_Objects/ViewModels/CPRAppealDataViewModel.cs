using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common_Objects;
using Common_Objects.Models;
using Common_Objects.ViewModels;

namespace Common_Objects.ViewModels
{
    public class CPRAppealDataViewModel
    {
        private SDIIS_DatabaseEntities _db = new SDIIS_DatabaseEntities();
        public int Appeal_Id { get; set; }
        public int PersonId { get; set; }
        public DateTime DateNoticeRecieved { get; set; }
        public string ReasonForAppeal { get; set; }
        public DateTime LetterOfNotice { get; set; }
        public DateTime DateAppealLogged { get; set; }
        public string CourtOrderAvailable { get; set; }
        public string Whereabouts { get; set; }
        public DateTime DateOfCourtOrder { get; set; }
        public int AppealResult_id { get; set; }
        public CPR_Unsuitability UnsuitabilityRecord { get; set; }
        public PersonDetailViewModel UnsuitablePerson { get; set; }
        public CPR_Unsuitability_Conviction Conviction { get; set; }
        public CPR_AppealResults AppealResultsDetails { get; set; }

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

        public CPR_Incident GetIncidentDetails(string id)
        {
            return _db.CPR_Incidents.Where(x => x.Reference_Number == id).FirstOrDefault();
        }

        public List<CPR_Incident> GetCPRIncidents()
        {
            return (from f in _db.CPR_Incidents
                    where f.Reference_Number != null
                    select f).ToList();
        }

        public CPR_Unsuitability GetCPR_Unsuitability(int Unsuitablity_Id)
        {
            return _db.CPR_Unsuitability.Where(x => x.Unsuitablity_Id == Unsuitablity_Id).FirstOrDefault();
        }
        public void SaveInfo()
        {
            _db.SaveChanges();
        }

        public void AddCPR_Unsuitability_Conviction(CPR_Unsuitability_Conviction conv)
        {
            _db.CPR_Unsuitability_Conviction.Add(conv);
        }

        public CPR_APPEALS GetCPR_APPEAL(int Appeal_Id)
        {
            return _db.CPR_APPEALS.Where(x => x.Appeal_Id == Appeal_Id).FirstOrDefault();
        }

        public CPR_Unsuitability_Conviction GetCPR_Unsuitability_Conviction(int? Conviction_Id)
        {
            return _db.CPR_Unsuitability_Conviction.Where(x => x.Conviction_Id == Conviction_Id).FirstOrDefault();
        }

        public CPR_AppealResults GetCPR_AppealResults(int? AppealResult_Id)
        {
            return _db.CPR_AppealResults.Where(x => x.AppealResult_Id == AppealResult_Id).FirstOrDefault();
        }

        public CPR_Unsuitability GetCpr_UnsuitabilityByPersonId(int Person_Id)
        {
            return _db.CPR_Unsuitability.Where(x => x.Person_Id == Person_Id).FirstOrDefault();
        }

        public void AddCPR_AppealResults(CPR_AppealResults newResults)
        {
            _db.CPR_AppealResults.Add(newResults);
        }

        public CPR_Unsuitability_Conviction GetCPR_Unsuitability_ConvictionBYPersonId(int Unsuitablity_Id)
        {
            return _db.CPR_Unsuitability_Conviction.Where(x => x.Unsuitability_Id == Unsuitablity_Id).FirstOrDefault();
        }

        public void AddCPR_APPEAL(CPR_APPEALS appeal)
        {
            _db.CPR_APPEALS.Add(appeal);
        }
    }
}
