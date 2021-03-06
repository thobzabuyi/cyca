﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common_Objects.Models;

namespace Common_Objects.ViewModels
{
    public class CPRUnsuitabilityDataViewModel
    {
        public PersonDetailViewModel UnsuitablePerson { get; set; }
        public string DriversLicense { get; set; }
        public string PrisonerNumber { get; set; }

        public int ForumNumber_Id { get; set; }

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

        public int SelectedTown_Id { get; set; }

        public string SelectedTown { get; set; }
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

        public string PostalCode { get; set; }
        public string SelectedForumNumber { get; set; }

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

        public int SelectedRelationship_Id { get; set; }
        [Display(Name = "Relationship")]
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

        public int SelectedOccupation_Id { get; set; }
        [Display(Name = "Occupation")]
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

        public int Selected_Local_Municipality_Id { get; set; }
        public SelectList LocalMunicipalityList
        {
            get
            {
                var municipalities = new LocalMunicipalityModel();
                var listOfMunicipalities = municipalities.GetListOfLocalMunicipalities();

                var employers = (from m in listOfMunicipalities
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Local_Municipality_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", Selected_Local_Municipality_Id);

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

        public bool FingerPrints { get; set; }
        public bool CourtOrder { get; set; }
        public bool Photo { get; set; }
        public bool CourtFindings { get; set; }
        public bool MinutesOfForum { get; set; }
        public bool KnownOffender { get; set; }
        public bool InternationalNotification { get; set; }


        public string searchOffenderRecord { get; set; }

    }
}
