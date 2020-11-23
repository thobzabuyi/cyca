using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common_Objects.Models;
using System.ComponentModel.DataAnnotations;

namespace Common_Objects.ViewModels
{
    public class CPRUnsuitabilityEditDataModel
    {
        public string WhereAbouts { get; set; }
        public string WorkAddAddLine1 { get; set; }
        public string WorkAddAddLine2 { get; set;}
        public string WorkAddPosCode { get; set; }
        public Address PhyAdd { get; set; }
        public Address PosAdd { get; set; }
        public CPR_Unsuitability_Findings cPR_Unsuitability_Findings { get; set; }
        public CPR_Incident IncidentToChild { get; set; }
        public int SelectedFindings_Id { get; set; }
        public SelectList FindingsList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var abuseList = (from f in _db.CPR_Finding_Type
                                 select f).ToList();

                var employers = (from m in abuseList
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.FindingType_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.FindingType_Id.Equals(SelectedFindings_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedFindings_Id);

                return selectList;


            }
        }

        public CPR_Unsuitability_Forum cPR_Unsuitability_Forum { get; set; }

        public Client CPR_ChildDetails { get; set; }
        public int SelectedTitle_Id { get; set; }
        public SelectList TitleList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.CPR_Title
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Title_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Title_Id.Equals(SelectedTitle_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTitle_Id);

                return selectList;
            }
        }

        public Client_CareGiver client_CareGiver { get; set; }

        public CPR_Unsuitability_Conviction cPR_Unsuitability_Conviction { get; set; }

        public CPR_Unsuitability_Incedent cPR_Unsuitability_Incedent { get; set; }
        public CPR_Unsuitability_Ruiling cPR_Unsuitability_Ruiling { get; set; }

        public int SelectedRuiling_Id { get; set; }
        public SelectList RuilingList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var abuseList = (from f in _db.CPR_Ruiling_Type
                                 select f).ToList();

                var employers = (from m in abuseList
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.RuilingType_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.RuilingType_Id.Equals(SelectedRuiling_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedRuiling_Id);

                return selectList;


            }
        }

        [Display(Name = "Municipality")]
        public int Selected_Local_Municipality_Id { get; set; }

        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Local_Municipality_Id);

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

        [Display(Name = "Relationship")]
        public int SelectedRelationship_Id { get; set; }
        public SelectList RelationshipType_List
        {
            get
            {
                var relationshipType = new RelationshipTypeModel();
                var listOfRelationships = relationshipType.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(SelectedRelationship_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedRelationship_Id);

                return selectList;
            }
        }

        [Display(Name = "Occupation")]
        public int SelectedOccupation_Id { get; set; }
        public SelectList Occupation_List
        {
            get
            {
                var occupations = new OccupationModel();
                var listOfOccupations = occupations.GetListOfOccupations();

                var employers = (from m in listOfOccupations
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Occupation_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Occupation_Id.Equals(SelectedOccupation_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedOccupation_Id);

                return selectList;
            }
        }

        //public CPRUnsuitabilityDataViewModel UnsuitablePersonDetails { get; set; }
        public int Unsuitability_Id { get; set; }

        public CPR_Unsuitability UnsuitablePerson { get; set; }
        public Client ChildDetails { get; set; }
        public CPR_Unsuitability_Findings Findings { get; set; }
        public CPR_Unsuitability_Incedent Incedent { get; set; }
        public Client_CareGiver CareGiver { get; set; }
        public CPR_Unsuitability_Conviction Conviction { get; set; }
        public CPR_Unsuitability_Ruiling Ruling { get; set; }
        public CPR_Unsuitability_Forum ForumDetails { get; set; }
        public string ForumName { get; set; }
        public string ForumAddress { get; set; }
        public int SelectedIdType_Id { get; set; }

        public int ForumNumber_Id { get; set; }

        public int? SelectedForumNumber { get; set; }

        public IEnumerable<Abuse_Indicator> AvailableAbuseIndicatorType { get; set; }
        public IList<Abuse_Indicator> SelectedAbuseIndicatorType { get; set; }
        public Posted_AbuseIndicatorType PostedAbuseIndicatorType { get; set; }

        public IEnumerable<CPR_Unsuitability_Incident_Abuse_Indicator> AvailableIncidentAbuseIndicatorType { get; set; }
        public IList<CPR_Unsuitability_Incident_Abuse_Indicator> SelectedIncidentAbuseIndicatorType { get; set; }
        public Posted_IncidentAbuseIndicatorType PostedIncidentAbuseIndicatorType { get; set; }

        public class Posted_AbuseIndicatorType
        {
            public int[] AbuseIndicatorTypeIDs { get; set; }

            public IEnumerable<SelectListItem> ListOfAbuseIndicatorTypeIDs { get; set; }
        }

        public class Posted_IncidentAbuseIndicatorType
        {
            public int[] IncidentAbuseIndicatorTypeIDs { get; set; }

            public IEnumerable<SelectListItem> ListOfIncidentAbuseIndicatorTypeIDs { get; set; }
        }


        public SelectList ForumList
        {
            get
            {
                var forums = new TownModel();
                var listofForums = forums.GetListOfForums();

                var newObj = (from l in listofForums
                              select new SelectListItem()
                              {
                                  Text = l.Forum_Name,
                                  Value = l.Forum_Id.ToString(CultureInfo.InvariantCulture),
                                  Selected = l.Forum_Id.Equals(SelectedForumNumber)
                              }).ToList();

                var selectList = new SelectList(newObj, "Value", "Text", SelectedForumNumber);
                return selectList;
            }
        }
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

        public int SelectedCourt_Id { get; set; }
        public SelectList CourtList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Courts
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Court_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Court_Id.Equals(SelectedCourt_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedCourt_Id);

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


        public int SelectedChildGender_Id { get; set; }
        public SelectList GenderChildList
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
                                     Selected = m.Gender_Id.Equals(SelectedChildGender_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedChildGender_Id);

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

        public int? SelectedTown_Id { get; set; }

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

        public CPR_Unsuitability_Incedent GetIncedentDetails(int UnsuitabilityId)
        {
            var db = new SDIIS_DatabaseEntities();

            return (from a in db.CPR_Unsuitability_Incedent
                         where a.Unsuitability_Id == UnsuitabilityId
                         select a).FirstOrDefault();


        }

        public int GetAbuse_Indicator_ID(int Id )
        {
            var db = new SDIIS_DatabaseEntities();
            return (from a in db.CPR_Unsuitability_Incedent
                    where a.Unsuitability_Id == Id
                    select a.Abuse_Type_Id).FirstOrDefault();
        }
    }
}
