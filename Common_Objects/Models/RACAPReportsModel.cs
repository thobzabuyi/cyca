﻿using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class RACAPReportsModel
    {

        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        #region GetLists
        public List<RACAPReportVM> GetTotalRegisteredChildren()
        {
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      where b.Problem_Sub_Category_Id == 19
                                      select d.Person_Id).Distinct().ToList();
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if(aDdresses.Count() != 0) { 
                RACAPAddresListVM newObj = new RACAPAddresListVM();
                newObj.Address_Id = clientPhysicalAddress.Address_Id;
                newObj.Person_Id = item;
                newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                if (clientPhysicalAddress.Town_Id != null)
                { 
                    newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                    newObj.Town_Id = clientPhysicalAddress.Town_Id;
                }
                newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                AddList.Add(newObj);
                }
            }

            var ListofTotalRegisteredChildren = (from a in AddList
                                                join d in db.Persons on a.Person_Id equals d.Person_Id 
                                                join e in AddList on d.Person_Id equals e.Person_Id
                                                join f in db.Towns on e.Town_Id equals f.Town_Id
                                                join g in db.Local_Municipalities on f.Local_Municipality_Id equals g.Local_Municipality_Id
                                                join h in db.Districts on g.District_Municipality_Id equals h.District_Id
                                                join i in db.Provinces on h.Province_Id equals i.Province_Id
                                                select new
                                                {e.Address_Id,
                                                    d.Person_Id,
                                                    i.Description,
                                                }).ToList();
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            var Provinces = db.Provinces;
            foreach(var rec in Provinces)
            {
                RACAPReportVM nObj = new RACAPReportVM();
                nObj.Province = rec.Description;
                nObj.RecordStatus = "Active";
                nObj.TotalChildren = (from c in ListofTotalRegisteredChildren
                                      where c.Description == rec.Description
                                      select c).Count();

                ResultList.Add(nObj);
            }


            return ResultList;
        }
        #endregion

        #region Dropdowns
        public int GetProvinceIds()
        {
            return (from k in db.Provinces
                    select k.Province_Id).FirstOrDefault();
        }
        public List<Province> GetProvinces()
        {
            return db.Provinces.ToList();
        }
        public int GetDistrictIds()
        {
            return (from k in db.Districts
                    select k.District_Id).FirstOrDefault();
        }
        public List<District> GetDistricts()
        {
            return db.Districts.ToList();
        }
        public int GetLocalMunicipalityIds()
        {
            return (from k in db.Local_Municipalities
                    select k.Local_Municipality_Id).FirstOrDefault();
        }
        public List<Local_Municipality> GetLocalMunicipalities()
        {
            return db.Local_Municipalities.ToList();
        }

        public int GetGenderIds()
        {
            return (from k in db.Genders
                    select k.Gender_Id).FirstOrDefault();
        }

        public List<Gender> GetGenders()
        {
            return db.Genders.ToList();
        }

        public int GetPopulation_GroupIds()
        {
            return (from k in db.Population_Groups
                    select k.Population_Group_Id).FirstOrDefault();
        }
        public List<Population_Group> GetPopulation_Groups()
        {
            return db.Population_Groups.ToList();
        }

        public int GetStatusIds()
        {

            return (from k in db.apl_RACAP_Record_Status
                    select k.RACAP_Record_Status_Id).FirstOrDefault();
        }

        public List<apl_RACAP_Record_Status> GetStatuses()
        {
            return db.apl_RACAP_Record_Status.ToList();
        }

        public int GetAdoptionReasonsIds()
        {
            return (from k in db.apl_Adoption_Reason
                    select k.Adoptions_Reason_Id).FirstOrDefault();
        }
        public List<apl_Adoption_Reason> GetAdoptionReasons()
        {
            return db.apl_Adoption_Reason.ToList();
        }
        public int GetMarital_StatusIds()
        {
            return (from k in db.Marital_Statusses
                    select k.Marital_Status_Id).FirstOrDefault();
        }
        public List<Marital_Status> GetMarital_Statuses()
        {
            return db.Marital_Statusses.ToList();
        }

        public int GetChild_preference_genderIds()
        {
            return (from k in db.Genders
                    select k.Gender_Id).FirstOrDefault();
        }
        public List<Gender> GetChildPreGenders()
        {
            return db.Genders.ToList();
        }
        public int GetChild_preference_population_GroupIds()
        {
            return (from k in db.Population_Groups
                    select k.Population_Group_Id).FirstOrDefault();
        }

        public List<Population_Group> GetChildPrePopulationGroups()
        {
            return db.Population_Groups.ToList();
        }
        #endregion

        public List<RACAPReportVM> RetrieveFirstReport(string ProvinceSearch, string DistrictSearch, string MunicipalitySearch,  string AdopTionReason, string GenderSearch,
                            string Population_GroupSearch, string MaritalStatusSearch, string Age_range_startSearch, string Age_range_endSearch,
                            string StatusSearch, DateTime? Date_Captured_From, DateTime? Date_Captured_To, string Type_of_adoptionSearch,
                            DateTime? Date_Registered_From, DateTime? Date_Registered_To, string Three_year_period_startSearch,
                            string Three_year_period_endSearch, string Marital_StatusSearch, string Child_preference_ageSearch,
                            string Child_preference_genderSearch, string Child_preference_population_GroupSearch)
        {//19
            //ReportType=Total children registered by province, child location, status and age
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason != "" && AdopTionReason != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch != "" && GenderSearch != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch != "" && StatusSearch != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch != "" && Population_GroupSearch != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }

            
            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch != "" && MaritalStatusSearch != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch != "" && DistrictSearch != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch != "" && MunicipalitySearch != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch != "" && Age_range_startSearch != null && Age_range_startSearch != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch != "" && Age_range_endSearch != null && Age_range_endSearch != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join n in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals n.RACAP_Case_Id into rAC
                                      from n in rAC.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason == null || AdopTionReason == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch == null || GenderSearch == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch == null || StatusSearch == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch == null || Population_GroupSearch == ""
                                      where i.Description.Contains(MaritalStatusSearch) || MaritalStatusSearch == null || MaritalStatusSearch == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch) || Child_preference_population_GroupSearch == null || Child_preference_population_GroupSearch == ""
                                      where k.Description.Contains(Child_preference_genderSearch) || Child_preference_genderSearch == null || Child_preference_genderSearch == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch == null || Age_range_startSearch == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch == null || Age_range_endSearch == ""
                                      where d.Date_Created>= Date_Captured_From
                                      where d.Date_Created <= Date_Captured_To
                                      where a.Date_Registered>= Date_Registered_From
                                      where a.Date_Registered<= Date_Registered_To
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                                select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                                join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                                where t.Town_Id == clientPhysicalAddress.Town_Id
                                                select u.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }

                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    
                    #region Address_Search
                    //Province
                    if (ProvinceSearch != null && ProvinceSearch != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch != null && DistrictSearch != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch != null && MunicipalitySearch != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        try { 
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    if (db.Persons.Find(item).Marital_Status_Id != null){
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated =(item.DateCreated);
                obj.ChildPreferences = item.ChildPreferencesS;
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;
                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveSecondReport(string ProvinceSearch_3, string DistrictSearch_3, string MunicipalitySearch_3, string AdopTionReason_3, string GenderSearch_3,
                            string Population_GroupSearch_3, string MaritalStatusSearch_3, string Age_range_startSearch_3, string Age_range_endSearch_3,
                            string StatusSearch_3, DateTime? Date_Captured_From_3, DateTime? Date_Captured_To_3, string Type_of_adoptionSearch_3,
                             DateTime? Date_Registered_From_3, DateTime? Date_Registered_To_3, string Three_year_period_startSearch_3,
                            string Three_year_period_endSearch_3, string Marital_StatusSearch_3, string Child_preference_ageSearch_3,
                            string Child_preference_genderSearch_3, string Child_preference_population_GroupSearch_3)
        {//19 20
            //ReportType=Total cases cancelled or ceased   
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_3 != "" && AdopTionReason_3 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_3);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_3 != "" && GenderSearch_3 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_3);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_3 != "" && StatusSearch_3 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_3);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_3 != "" && Population_GroupSearch_3 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_3);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_3 != "" && MaritalStatusSearch_3 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_3);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_3 != "" && DistrictSearch_3 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_3);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_3 != "" && MunicipalitySearch_3 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_3);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_3 != "" && Age_range_startSearch_3 != null && Age_range_startSearch_3 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_3);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_3 != "" && Age_range_endSearch_3 != null && Age_range_endSearch_3 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_3);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join n in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals n.RACAP_Case_Id into rAC
                                      from n in rAC.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19 && a.RACAP_Record_Status_Id==4
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_3 == null || AdopTionReason_3 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_3 == null || GenderSearch_3 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_3 == null || StatusSearch_3 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_3 == null || Population_GroupSearch_3 == ""
                                      where i.Description.Contains(MaritalStatusSearch_3) || MaritalStatusSearch_3 == null || MaritalStatusSearch_3 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_3) || Child_preference_population_GroupSearch_3 == null || Child_preference_population_GroupSearch_3 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_3) || Child_preference_genderSearch_3 == null || Child_preference_genderSearch_3 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_3 == null || Age_range_startSearch_3 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_3 == null || Age_range_endSearch_3 == ""
                                      where d.Date_Created >= Date_Captured_From_3
                                      where d.Date_Created <= Date_Captured_To_3
                                      where a.Date_Registered >= Date_Registered_From_3
                                      where a.Date_Registered <= Date_Registered_To_3
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_3 != null && ProvinceSearch_3 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_3;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_3 != null && DistrictSearch_3 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_3 != null && MunicipalitySearch_3 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();

                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveThirdReport(string ProvinceSearch_4, string DistrictSearch_4, string MunicipalitySearch_4, string AdopTionReason_4, string GenderSearch_4,
                            string Population_GroupSearch_4, string MaritalStatusSearch_4, string Age_range_startSearch_4, string Age_range_endSearch_4,
                            string StatusSearch_4, DateTime? Date_Captured_From_4, DateTime? Date_Captured_To_4, string Type_of_adoptionSearch_4,
                             DateTime? Date_Registered_From_4, DateTime? Date_Registered_To_4, string Three_year_period_startSearch_4,
                            string Three_year_period_endSearch_4, string Marital_StatusSearch_4, string Child_preference_ageSearch_4,
                            string Child_preference_genderSearch_4, string Child_preference_population_GroupSearch_4)
        {//20
            //ReportType=Total prospective adoptive parents registered by province, child location and status
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_4 != "" && AdopTionReason_4 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_4);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_4 != "" && GenderSearch_4 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_4);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_4 != "" && StatusSearch_4 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_4);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_4 != "" && Population_GroupSearch_4 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_4);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_4 != "" && MaritalStatusSearch_4 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_4);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_4 != "" && DistrictSearch_4 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_4);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_4 != "" && MunicipalitySearch_4 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_4);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_4 != "" && Age_range_startSearch_4 != null && Age_range_startSearch_4 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_4);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_4 != "" && Age_range_endSearch_4 != null && Age_range_endSearch_4 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_4);
            }

            #endregion
            #region PersonIdRequest
            var AllParentsInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                     join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                     from o in rOR.DefaultIfEmpty()
                                     where b.Problem_Sub_Category_Id == 20
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_4 == null || AdopTionReason_4 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_4 == null || GenderSearch_4 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_4 == null || StatusSearch_4 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_4 == null || Population_GroupSearch_4 == ""
                                      where i.Description.Contains(MaritalStatusSearch_4) || MaritalStatusSearch_4 == null || MaritalStatusSearch_4 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_4) || Child_preference_population_GroupSearch_4 == null || Child_preference_population_GroupSearch_4 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_4) || Child_preference_genderSearch_4 == null || Child_preference_genderSearch_4 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_4 == null || Age_range_startSearch_4 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_4 == null || Age_range_endSearch_4 == ""
                                      where d.Date_Created >= Date_Captured_From_4
                                      where d.Date_Created <= Date_Captured_To_4
                                      where a.Date_Registered >= Date_Registered_From_4
                                      where a.Date_Registered <= Date_Registered_To_4
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllParentsInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_4 != null && ProvinceSearch_4 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_4;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_4 != null && DistrictSearch_4 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_4 != null && MunicipalitySearch_4 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveFourthReport(string ProvinceSearch_5, string DistrictSearch_5, string MunicipalitySearch_5, string AdopTionReason_5, string GenderSearch_5,
                            string Population_GroupSearch_5, string MaritalStatusSearch_5, string Age_range_startSearch_5, string Age_range_endSearch_5,
                            string StatusSearch_5, DateTime? Date_Captured_From_5, DateTime? Date_Captured_To_5, string Type_of_adoptionSearch_5,
                             DateTime? Date_Registered_From_5, DateTime? Date_Registered_To_5, string Three_year_period_startSearch_5,
                            string Three_year_period_endSearch_5, string Marital_StatusSearch_5, string Child_preference_ageSearch_5,
                            string Child_preference_genderSearch_5, string Child_preference_population_GroupSearch_5)
        {//ReportType=Total records where the 60 day time period has lapsed
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_5 != "" && AdopTionReason_5 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_5);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_5 != "" && GenderSearch_5 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_5);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_5 != "" && StatusSearch_5 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_5);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_5 != "" && Population_GroupSearch_5 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_5);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_5 != "" && MaritalStatusSearch_5 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_5);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_5 != "" && DistrictSearch_5 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_5);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_5 != "" && MunicipalitySearch_5 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_5);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_5 != "" && Age_range_startSearch_5 != null && Age_range_startSearch_5 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_5);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_5 != "" && Age_range_endSearch_5 != null && Age_range_endSearch_5 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_5);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join n in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals n.RACAP_Case_Id into rAC
                                      from n in rAC.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19 
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_5 == null || AdopTionReason_5 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_5 == null || GenderSearch_5 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_5 == null || StatusSearch_5 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_5 == null || Population_GroupSearch_5 == ""
                                      where i.Description.Contains(MaritalStatusSearch_5) || MaritalStatusSearch_5 == null || MaritalStatusSearch_5 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_5) || Child_preference_population_GroupSearch_5 == null || Child_preference_population_GroupSearch_5 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_5) || Child_preference_genderSearch_5 == null || Child_preference_genderSearch_5 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_5 == null || Age_range_startSearch_5 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_5 == null || Age_range_endSearch_5 == ""
                                      where d.Date_Created >= Date_Captured_From_5
                                      where d.Date_Created <= Date_Captured_To_5
                                      where a.Date_Registered >= Date_Registered_From_5
                                      where a.Date_Registered <= Date_Registered_To_5
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0 )
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }

                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_5 != null && ProvinceSearch_5 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_5;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_5 != null && DistrictSearch_5 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_5 != null && MunicipalitySearch_5 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                if (newObj.DateRegistered.Value.AddDays(60) < DateTime.Now.Date)
                                                {
                                                    AddList.Add(newObj);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (newObj.DateRegistered.Value.AddDays(60) < DateTime.Now.Date)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (newObj.DateRegistered.Value.AddDays(60) < DateTime.Now.Date)
                                    {
                                        AddList.Add(newObj);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (newObj.DateRegistered.Value.AddDays(60) < DateTime.Now.Date)
                        {
                            AddList.Add(newObj);
                        }
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    if(newObj.DateRegistered.Value.AddDays(60) < DateTime.Now.Date)
                    { 
                    AddList.Add(newObj);
                    }
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveFifthReport(string ProvinceSearch_6, string DistrictSearch_6, string MunicipalitySearch_6, string AdopTionReason_6, string GenderSearch_6,
                            string Population_GroupSearch_6, string MaritalStatusSearch_6, string Age_range_startSearch_6, string Age_range_endSearch_6,
                            string StatusSearch_6, DateTime? Date_Captured_From_6, DateTime? Date_Captured_To_6, string Type_of_adoptionSearch_6,
                             DateTime? Date_Registered_From_6, DateTime? Date_Registered_To_6, string Three_year_period_startSearch_6,
                            string Three_year_period_endSearch_6, string Marital_StatusSearch_6, string Child_preference_ageSearch_6,
                            string Child_preference_genderSearch_6, string Child_preference_population_GroupSearch_6)
        {//ReportType=Total records within the 60 day time period
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_6 != "" && AdopTionReason_6 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_6);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_6 != "" && GenderSearch_6 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_6);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_6 != "" && StatusSearch_6 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_6);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_6 != "" && Population_GroupSearch_6 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_6);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_6 != "" && MaritalStatusSearch_6 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_6);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_6 != "" && DistrictSearch_6 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_6);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_6 != "" && MunicipalitySearch_6 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_6);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_6 != "" && Age_range_startSearch_6 != null && Age_range_startSearch_6 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_6);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_6 != "" && Age_range_endSearch_6 != null && Age_range_endSearch_6 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_6);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join n in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals n.RACAP_Case_Id into rAC
                                      from n in rAC.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19 
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_6 == null || AdopTionReason_6 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_6 == null || GenderSearch_6 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_6 == null || StatusSearch_6 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_6 == null || Population_GroupSearch_6 == ""
                                      where i.Description.Contains(MaritalStatusSearch_6) || MaritalStatusSearch_6 == null || MaritalStatusSearch_6 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_6) || Child_preference_population_GroupSearch_6 == null || Child_preference_population_GroupSearch_6 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_6) || Child_preference_genderSearch_6 == null || Child_preference_genderSearch_6 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_6 == null || Age_range_startSearch_6 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_6 == null || Age_range_endSearch_6 == ""
                                      where d.Date_Created >= Date_Captured_From_6
                                      where d.Date_Created <= Date_Captured_To_6
                                      where a.Date_Registered >= Date_Registered_From_6
                                      where a.Date_Registered <= Date_Registered_To_6
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_6 != null && ProvinceSearch_6 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_6;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_6 != null && DistrictSearch_6 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_6 != null && MunicipalitySearch_6 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                if(newObj.DateRegistered<DateTime.Now.Date && newObj.DateRegistered.Value.AddDays(60)>DateTime.Now.Date)
                                                { 
                                                    AddList.Add(newObj);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddDays(60) > DateTime.Now.Date)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddDays(60) > DateTime.Now.Date)
                                    {
                                        AddList.Add(newObj);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddDays(60) > DateTime.Now.Date)
                        {
                            AddList.Add(newObj);
                        }
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddDays(60) > DateTime.Now.Date)
                    {
                        AddList.Add(newObj);
                    }
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveSixthReport(string ProvinceSearch_7, string DistrictSearch_7, string MunicipalitySearch_7, string AdopTionReason_7, string GenderSearch_7,
                            string Population_GroupSearch_7, string MaritalStatusSearch_7, string Age_range_startSearch_7, string Age_range_endSearch_7,
                            string StatusSearch_7, DateTime? Date_Captured_From_7, DateTime? Date_Captured_To_7, string Type_of_adoptionSearch_7,
                             DateTime? Date_Registered_From_7, DateTime? Date_Registered_To_7, string Three_year_period_startSearch_7,
                            string Three_year_period_endSearch_7, string Marital_StatusSearch_7, string Child_preference_ageSearch_7,
                            string Child_preference_genderSearch_7, string Child_preference_population_GroupSearch_7)
        {//20
            //ReportType=Total record where the 3 year period of prospective adoptive parents has lapsed
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_7 != "" && AdopTionReason_7 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_7);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_7 != "" && GenderSearch_7 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_7);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_7 != "" && StatusSearch_7 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_7);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_7 != "" && Population_GroupSearch_7 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_7);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_7 != "" && MaritalStatusSearch_7 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_7);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_7 != "" && DistrictSearch_7 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_7);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_7 != "" && MunicipalitySearch_7 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_7);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_7 != "" && Age_range_startSearch_7 != null && Age_range_startSearch_7 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_7);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_7 != "" && Age_range_endSearch_7 != null && Age_range_endSearch_7 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_7);
            }

            #endregion
            #region PersonIdRequest
            var AllParentsInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                     join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                     from o in rOR.DefaultIfEmpty()
                                     where b.Problem_Sub_Category_Id == 20 
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_7 == null || AdopTionReason_7 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_7 == null || GenderSearch_7 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_7 == null || StatusSearch_7 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_7 == null || Population_GroupSearch_7 == ""
                                      where i.Description.Contains(MaritalStatusSearch_7) || MaritalStatusSearch_7 == null || MaritalStatusSearch_7 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_7) || Child_preference_population_GroupSearch_7 == null || Child_preference_population_GroupSearch_7 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_7) || Child_preference_genderSearch_7 == null || Child_preference_genderSearch_7 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_7 == null || Age_range_startSearch_7 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_7 == null || Age_range_endSearch_7 == ""
                                      where d.Date_Created >= Date_Captured_From_7
                                      where d.Date_Created <= Date_Captured_To_7
                                      where a.Date_Registered >= Date_Registered_From_7
                                      where a.Date_Registered <= Date_Registered_To_7
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllParentsInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_7 != null && ProvinceSearch_7 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_7;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_7 != null && DistrictSearch_7 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_7 != null && MunicipalitySearch_7 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                if (newObj.DateRegistered.Value.AddYears(3) < DateTime.Now.Date)
                                                {
                                                    AddList.Add(newObj);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (newObj.DateRegistered.Value.AddYears(3) < DateTime.Now.Date)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (newObj.DateRegistered.Value.AddYears(3) < DateTime.Now.Date)
                                    {
                                        AddList.Add(newObj);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (newObj.DateRegistered.Value.AddYears(3) < DateTime.Now.Date)
                        {
                            AddList.Add(newObj);
                        }
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    if (newObj.DateRegistered.Value.AddYears(3) < DateTime.Now.Date)
                    {
                        AddList.Add(newObj);
                    }
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddYears(3);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }

        public List<RACAPReportVM> RetrieveSeventhReport(string ProvinceSearch_8, string DistrictSearch_8, string MunicipalitySearch_8, string AdopTionReason_8, string GenderSearch_8,
                            string Population_GroupSearch_8, string MaritalStatusSearch_8, string Age_range_startSearch_8, string Age_range_endSearch_8,
                            string StatusSearch_8, DateTime? Date_Captured_From_8, DateTime? Date_Captured_To_8, string Type_of_adoptionSearch_8,
                             DateTime? Date_Registered_From_8, DateTime? Date_Registered_To_8, string Three_year_period_startSearch_8,
                            string Three_year_period_endSearch_8, string Marital_StatusSearch_8, string Child_preference_ageSearch_8,
                            string Child_preference_genderSearch_8, string Child_preference_population_GroupSearch_8)
        {//20
            //ReportType=Total records within the 3 year period
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_8 != "" && AdopTionReason_8 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_8);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_8 != "" && GenderSearch_8 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_8);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_8 != "" && StatusSearch_8 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_8);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_8 != "" && Population_GroupSearch_8 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_8);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_8 != "" && MaritalStatusSearch_8 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_8);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_8 != "" && DistrictSearch_8 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_8);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_8 != "" && MunicipalitySearch_8 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_8);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_8 != "" && Age_range_startSearch_8 != null && Age_range_startSearch_8 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_8);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_8 != "" && Age_range_endSearch_8 != null && Age_range_endSearch_8 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_8);
            }

            #endregion
            #region PersonIdRequest
            var AllParentsInRACAP = (from a in db.RACAP_Case_Details
                                     join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                     join c in db.Clients on b.Client_Id equals c.Client_Id
                                     join d in db.Persons on c.Person_Id equals d.Person_Id
                                     join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                     from e in gs.DefaultIfEmpty()
                                     join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                     from f in popgs.DefaultIfEmpty()
                                     join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                     from g in rRecSts.DefaultIfEmpty()
                                     join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                     from h in ls.DefaultIfEmpty()
                                     join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                     from i in mars.DefaultIfEmpty()
                                     join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                     from j in RAC.DefaultIfEmpty()
                                     join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                     from k in childPop.DefaultIfEmpty()
                                     join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                     from l in childGen.DefaultIfEmpty()
                                     join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                     from o in rOR.DefaultIfEmpty()
                                     where b.Problem_Sub_Category_Id == 20 
                                     where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_8 == null || AdopTionReason_8 == ""
                                     where e.Description.Contains(newGenderSearch) || GenderSearch_8 == null || GenderSearch_8 == ""
                                     where g.Description.Contains(newStatusSearch) || StatusSearch_8 == null || StatusSearch_8 == ""
                                     where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_8 == null || Population_GroupSearch_8 == ""
                                     where i.Description.Contains(MaritalStatusSearch_8) || MaritalStatusSearch_8 == null || MaritalStatusSearch_8 == ""
                                     where k.Description.Contains(Child_preference_population_GroupSearch_8) || Child_preference_population_GroupSearch_8 == null || Child_preference_population_GroupSearch_8 == ""
                                     where k.Description.Contains(Child_preference_genderSearch_8) || Child_preference_genderSearch_8 == null || Child_preference_genderSearch_8 == ""
                                     where d.Age >= newAgeStartSearch || Age_range_startSearch_8 == null || Age_range_startSearch_8 == ""
                                     where d.Age <= newAge_range_endSearch || Age_range_endSearch_8 == null || Age_range_endSearch_8 == ""
                                     where d.Date_Created >= Date_Captured_From_8
                                     where d.Date_Created <= Date_Captured_To_8
                                     where a.Date_Registered >= Date_Registered_From_8
                                     where a.Date_Registered <= Date_Registered_To_8
                                     select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllParentsInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_8 != null && ProvinceSearch_8 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_8;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_8 != null && DistrictSearch_8 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_8 != null && MunicipalitySearch_8 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddYears(3) > DateTime.Now.Date)
                                                {
                                                    AddList.Add(newObj);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddYears(3) > DateTime.Now.Date)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddYears(3) > DateTime.Now.Date)
                                    {
                                        AddList.Add(newObj);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddYears(3) > DateTime.Now.Date)
                        {
                            AddList.Add(newObj);
                        }
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    if (newObj.DateRegistered < DateTime.Now.Date && newObj.DateRegistered.Value.AddYears(3) > DateTime.Now.Date)
                    {
                        AddList.Add(newObj);
                    }
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddYears(3);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveEighthReport(string ProvinceSearch_9, string DistrictSearch_9, string MunicipalitySearch_9, string AdopTionReason_9, string GenderSearch_9,
                            string Population_GroupSearch_9, string MaritalStatusSearch_9, string Age_range_startSearch_9, string Age_range_endSearch_9,
                            string StatusSearch_9, DateTime? Date_Captured_From_9, DateTime? Date_Captured_To_9, string Type_of_adoptionSearch_9,
                             DateTime? Date_Registered_From_9, DateTime? Date_Registered_To_9, string Three_year_period_startSearch_9,
                            string Three_year_period_endSearch_9, string Marital_StatusSearch_9, string Child_preference_ageSearch_9,
                            string Child_preference_genderSearch_9, string Child_preference_population_GroupSearch_9)
        {//ReportType=Reason for adoption, sorted by type
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_9 != "" && AdopTionReason_9 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_9);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_9 != "" && GenderSearch_9 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_9);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_9 != "" && StatusSearch_9 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_9);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_9 != "" && Population_GroupSearch_9 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_9);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_9 != "" && MaritalStatusSearch_9 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_9);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_9 != "" && DistrictSearch_9 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_9);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_9 != "" && MunicipalitySearch_9 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_9);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_9 != "" && Age_range_startSearch_9 != null && Age_range_startSearch_9 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_9);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_9 != "" && Age_range_endSearch_9 != null && Age_range_endSearch_9 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_9);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join n in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals n.RACAP_Case_Id into rAC
                                      from n in rAC.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19 && a.Adoptions_Reason_Id!=null
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_9 == null || AdopTionReason_9 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_9 == null || GenderSearch_9 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_9 == null || StatusSearch_9 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_9 == null || Population_GroupSearch_9 == ""
                                      where i.Description.Contains(MaritalStatusSearch_9) || MaritalStatusSearch_9 == null || MaritalStatusSearch_9 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_9) || Child_preference_population_GroupSearch_9 == null || Child_preference_population_GroupSearch_9 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_9) || Child_preference_genderSearch_9 == null || Child_preference_genderSearch_9 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_9 == null || Age_range_startSearch_9 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_9 == null || Age_range_endSearch_9 == ""
                                      where d.Date_Created >= Date_Captured_From_9
                                      where d.Date_Created <= Date_Captured_To_9
                                      where a.Date_Registered >= Date_Registered_From_9
                                      where a.Date_Registered <= Date_Registered_To_9
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_9 != null && ProvinceSearch_9 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_9;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_9 != null && DistrictSearch_9 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_9 != null && MunicipalitySearch_9 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveNinethReport(string ProvinceSearch_10, string DistrictSearch_10, string MunicipalitySearch_10, string AdopTionReason_10, string GenderSearch_10,
                            string Population_GroupSearch_10, string MaritalStatusSearch_10, string Age_range_startSearch_10, string Age_range_endSearch_10,
                            string StatusSearch_10, DateTime? Date_Captured_From_10, DateTime? Date_Captured_To_10, string Type_of_adoptionSearch_10,
                             DateTime? Date_Registered_From_10, DateTime? Date_Registered_To_10, string Three_year_period_startSearch_10,
                            string Three_year_period_endSearch_10, string Marital_StatusSearch_10, string Child_preference_ageSearch_10,
                            string Child_preference_genderSearch_10, string Child_preference_population_GroupSearch_10)
        {//ReportTyoe=Type of adoption
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_10 != "" && AdopTionReason_10 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_10);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_10 != "" && GenderSearch_10 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_10);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_10 != "" && StatusSearch_10 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_10);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_10 != "" && Population_GroupSearch_10 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_10);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_10 != "" && MaritalStatusSearch_10 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_10);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_10 != "" && DistrictSearch_10 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_10);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_10 != "" && MunicipalitySearch_10 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_10);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_10 != "" && Age_range_startSearch_10 != null && Age_range_startSearch_10 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_10);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_10 != "" && Age_range_endSearch_10 != null && Age_range_endSearch_10 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_10);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join n in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals n.RACAP_Case_Id into rAC
                                      from n in rAC.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19 
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_10 == null || AdopTionReason_10 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_10 == null || GenderSearch_10 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_10 == null || StatusSearch_10 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_10 == null || Population_GroupSearch_10 == ""
                                      where i.Description.Contains(MaritalStatusSearch_10) || MaritalStatusSearch_10 == null || MaritalStatusSearch_10 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_10) || Child_preference_population_GroupSearch_10 == null || Child_preference_population_GroupSearch_10 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_10) || Child_preference_genderSearch_10 == null || Child_preference_genderSearch_10 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_10 == null || Age_range_startSearch_10 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_10 == null || Age_range_endSearch_10 == ""
                                      where d.Date_Created >= Date_Captured_From_10
                                      where d.Date_Created <= Date_Captured_To_10
                                      where a.Date_Registered >= Date_Registered_From_10
                                      where a.Date_Registered <= Date_Registered_To_10
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_10 != null && ProvinceSearch_10 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_10;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_10 != null && DistrictSearch_10 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_10 != null && MunicipalitySearch_10 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveTenthReport(string ProvinceSearch_11, string DistrictSearch_11, string MunicipalitySearch_11, string AdopTionReason_11, string GenderSearch_11,
                            string Population_GroupSearch_11, string MaritalStatusSearch_11, string Age_range_startSearch_11, string Age_range_endSearch_11,
                            string StatusSearch_11, DateTime? Date_Captured_From_11, DateTime? Date_Captured_To_11, string Type_of_adoptionSearch_11,
                             DateTime? Date_Registered_From_11, DateTime? Date_Registered_To_11, string Three_year_period_startSearch_11,
                            string Three_year_period_endSearch_11, string Marital_StatusSearch_11, string Child_preference_ageSearch_11,
                            string Child_preference_genderSearch_11, string Child_preference_population_GroupSearch_11)
        {//ReportType=Racial breakdown for children
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_11 != "" && AdopTionReason_11 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_11);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_11 != "" && GenderSearch_11 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_11);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_11 != "" && StatusSearch_11 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_11);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_11 != "" && Population_GroupSearch_11 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_11);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_11 != "" && MaritalStatusSearch_11 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_11);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_11 != "" && DistrictSearch_11 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_11);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_11 != "" && MunicipalitySearch_11 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_11);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_11 != "" && Age_range_startSearch_11 != null && Age_range_startSearch_11 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_11);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_11 != "" && Age_range_endSearch_11 != null && Age_range_endSearch_11 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_11);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      join n in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals n.RACAP_Case_Id into rAC
                                      from n in rAC.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19 &&d.Population_Group_Id !=null
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_11 == null || AdopTionReason_11 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_11 == null || GenderSearch_11 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_11 == null || StatusSearch_11 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_11 == null || Population_GroupSearch_11 == ""
                                      where i.Description.Contains(MaritalStatusSearch_11) || MaritalStatusSearch_11 == null || MaritalStatusSearch_11 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_11) || Child_preference_population_GroupSearch_11 == null || Child_preference_population_GroupSearch_11 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_11) || Child_preference_genderSearch_11 == null || Child_preference_genderSearch_11 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_11 == null || Age_range_startSearch_11 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_11 == null || Age_range_endSearch_11 == ""
                                      where d.Date_Created >= Date_Captured_From_11
                                      where d.Date_Created <= Date_Captured_To_11
                                      where a.Date_Registered >= Date_Registered_From_11
                                      where a.Date_Registered <= Date_Registered_To_11
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_11 != null && ProvinceSearch_11 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_11;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_11 != null && DistrictSearch_11 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_11 != null && MunicipalitySearch_11 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveEleventhReport(string ProvinceSearch_12, string DistrictSearch_12, string MunicipalitySearch_12, string AdopTionReason_12, string GenderSearch_12,
                            string Population_GroupSearch_12, string MaritalStatusSearch_12, string Age_range_startSearch_12, string Age_range_endSearch_12,
                            string StatusSearch_12, DateTime? Date_Captured_From_12, DateTime? Date_Captured_To_12, string Type_of_adoptionSearch_12,
                             DateTime? Date_Registered_From_12, DateTime? Date_Registered_To_12, string Three_year_period_startSearch_12,
                            string Three_year_period_endSearch_12, string Marital_StatusSearch_12, string Child_preference_ageSearch_12,
                            string Child_preference_genderSearch_12, string Child_preference_population_GroupSearch_12)
        {//20
            //ReportType=Racial breakdown for prospective adoptive parents
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_12 != "" && AdopTionReason_12 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_12);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_12 != "" && GenderSearch_12 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_12);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_12 != "" && StatusSearch_12 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_12);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_12 != "" && Population_GroupSearch_12 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_12);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_12 != "" && MaritalStatusSearch_12 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_12);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_12 != "" && DistrictSearch_12 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_12);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_12 != "" && MunicipalitySearch_12 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_12);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_12 != "" && Age_range_startSearch_12 != null && Age_range_startSearch_12 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_12);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_12 != "" && Age_range_endSearch_12 != null && Age_range_endSearch_12 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_12);
            }

            #endregion
            #region PersonIdRequest
            var AllParentsInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                     join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                     from o in rOR.DefaultIfEmpty()
                                     join m in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals m.RACAP_Case_Id into ChildPref
                                     from m in ChildPref.DefaultIfEmpty()
                                     where b.Problem_Sub_Category_Id == 20 && d.Population_Group_Id !=null
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_12 == null || AdopTionReason_12 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_12 == null || GenderSearch_12 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_12 == null || StatusSearch_12 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_12 == null || Population_GroupSearch_12 == ""
                                      where i.Description.Contains(MaritalStatusSearch_12) || MaritalStatusSearch_12 == null || MaritalStatusSearch_12 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_12) || Child_preference_population_GroupSearch_12 == null || Child_preference_population_GroupSearch_12 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_12) || Child_preference_genderSearch_12 == null || Child_preference_genderSearch_12 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_12 == null || Age_range_startSearch_12 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_12 == null || Age_range_endSearch_12 == ""
                                      where d.Date_Created >= Date_Captured_From_12
                                      where d.Date_Created <= Date_Captured_To_12
                                      where a.Date_Registered >= Date_Registered_From_12
                                      where a.Date_Registered <= Date_Registered_To_12
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllParentsInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                        join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                        join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                        join c in db.Clients on b.Client_Id equals c.Client_Id
                        join d in db.Persons on c.Person_Id equals d.Person_Id
                        where d.Person_Id == item
                        select e.Body_Structure_Id).FirstOrDefault() != null)
                        { 
                        newObj.ChildPreferencesS= db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                            join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                            join c in db.Clients on b.Client_Id equals c.Client_Id
                            join d in db.Persons on c.Person_Id equals d.Person_Id
                            where d.Person_Id == item
                            select e.Body_Structure_Id).FirstOrDefault()).Description;
                        }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_12 != null && ProvinceSearch_12 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_12;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_12 != null && DistrictSearch_12 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_12 != null && MunicipalitySearch_12 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveTwelvethReport(string ProvinceSearch_13, string DistrictSearch_13, string MunicipalitySearch_13, string AdopTionReason_13, string GenderSearch_13,
                            string Population_GroupSearch_13, string MaritalStatusSearch_13, string Age_range_startSearch_13, string Age_range_endSearch_13,
                            string StatusSearch_13, DateTime? Date_Captured_From_13, DateTime? Date_Captured_To_13, string Type_of_adoptionSearch_13,
                             DateTime? Date_Registered_From_13, DateTime? Date_Registered_To_13, string Three_year_period_startSearch_13,
                            string Three_year_period_endSearch_13, string Marital_StatusSearch_13, string Child_preference_ageSearch_13,
                            string Child_preference_genderSearch_13, string Child_preference_population_GroupSearch_13)
        {//ReportType=Special needs for children
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_13 != "" && AdopTionReason_13 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_13);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_13 != "" && GenderSearch_13 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_13);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_13 != "" && StatusSearch_13 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_13);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_13 != "" && Population_GroupSearch_13 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_13);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_13 != "" && MaritalStatusSearch_13 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_13);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_13 != "" && DistrictSearch_13 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_13);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_13 != "" && MunicipalitySearch_13 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_13);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_13 != "" && Age_range_startSearch_13 != null && Age_range_startSearch_13 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_13);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_13 != "" && Age_range_endSearch_13 != null && Age_range_endSearch_13 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_13);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      join m in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals m.RACAP_Case_Id into rAC
                                      from m in rAC.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 19 && m.Special_Needs_Id !=null
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_13 == null || AdopTionReason_13 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_13 == null || GenderSearch_13 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_13 == null || StatusSearch_13 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_13 == null || Population_GroupSearch_13 == ""
                                      where i.Description.Contains(MaritalStatusSearch_13) || MaritalStatusSearch_13 == null || MaritalStatusSearch_13 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_13) || Child_preference_population_GroupSearch_13 == null || Child_preference_population_GroupSearch_13 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_13) || Child_preference_genderSearch_13 == null || Child_preference_genderSearch_13 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_13 == null || Age_range_startSearch_13 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_13 == null || Age_range_endSearch_13 == ""
                                      where d.Date_Created >= Date_Captured_From_13
                                      where d.Date_Created <= Date_Captured_To_13
                                      where a.Date_Registered >= Date_Registered_From_13
                                      where a.Date_Registered <= Date_Registered_To_13
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_13 != null && ProvinceSearch_13 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_13;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_13 != null && DistrictSearch_13 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_13 != null && MunicipalitySearch_13 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Child).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Child).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveThirteenthReport(string ProvinceSearch_14, string DistrictSearch_14, string MunicipalitySearch_14, string AdopTionReason_14, string GenderSearch_14,
                            string Population_GroupSearch_14, string MaritalStatusSearch_14, string Age_range_startSearch_14, string Age_range_endSearch_14,
                            string StatusSearch_14, DateTime? Date_Captured_From_14, DateTime? Date_Captured_To_14, string Type_of_adoptionSearch_14,
                             DateTime? Date_Registered_From_14, DateTime? Date_Registered_To_14, string Three_year_period_startSearch_14,
                            string Three_year_period_endSearch_14, string Marital_StatusSearch_14, string Child_preference_ageSearch_14,
                            string Child_preference_genderSearch_14, string Child_preference_population_GroupSearch_14)
        {//20
            //ReportType=Special needs for prospective adoptive parents
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_14 != "" && AdopTionReason_14 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_14);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_14 != "" && GenderSearch_14 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_14);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_14 != "" && StatusSearch_14 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_14);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_14 != "" && Population_GroupSearch_14 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_14);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_14 != "" && MaritalStatusSearch_14 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_14);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_14 != "" && DistrictSearch_14 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_14);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_14 != "" && MunicipalitySearch_14 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_14);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_14 != "" && Age_range_startSearch_14 != null && Age_range_startSearch_14 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_14);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_14 != "" && Age_range_endSearch_14 != null && Age_range_endSearch_14 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_14);
            }

            #endregion
            #region PersonIdRequest
            var AllParentsInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join m in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals m.RACAP_Case_Id into rPP
                                      from m in rPP.DefaultIfEmpty()
                                     join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                     from o in rOR.DefaultIfEmpty()
                                     where b.Problem_Sub_Category_Id == 20 && m.Special_Needs_Id!=null
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_14 == null || AdopTionReason_14 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_14 == null || GenderSearch_14 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_14 == null || StatusSearch_14 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_14 == null || Population_GroupSearch_14 == ""
                                      where i.Description.Contains(MaritalStatusSearch_14) || MaritalStatusSearch_14 == null || MaritalStatusSearch_14 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_14) || Child_preference_population_GroupSearch_14 == null || Child_preference_population_GroupSearch_14 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_14) || Child_preference_genderSearch_14 == null || Child_preference_genderSearch_14 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_14 == null || Age_range_startSearch_14 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_14 == null || Age_range_endSearch_14 == ""
                                      where d.Date_Created >= Date_Captured_From_14
                                      where d.Date_Created <= Date_Captured_To_14
                                      where a.Date_Registered >= Date_Registered_From_14
                                      where a.Date_Registered <= Date_Registered_To_14
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllParentsInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_14 != null && ProvinceSearch_14 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_14;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_14 != null && DistrictSearch_14 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_14 != null && MunicipalitySearch_14 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;
                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveFourteenthReport(string ProvinceSearch_15, string DistrictSearch_15, string MunicipalitySearch_15, string AdopTionReason_15, string GenderSearch_15,
                            string Population_GroupSearch_15, string MaritalStatusSearch_15, string Age_range_startSearch_15, string Age_range_endSearch_15,
                            string StatusSearch_15, DateTime? Date_Captured_From_15, DateTime? Date_Captured_To_15, string Type_of_adoptionSearch_15,
                             DateTime? Date_Registered_From_15, DateTime? Date_Registered_To_15, string Three_year_period_startSearch_15,
                            string Three_year_period_endSearch_15, string Marital_StatusSearch_15, string Child_preference_ageSearch_15,
                            string Child_preference_genderSearch_15, string Child_preference_population_GroupSearch_15)
        {//20
            //ReportType=Parent’s marital status
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_15 != "" && AdopTionReason_15 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_15);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_15 != "" && GenderSearch_15 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_15);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_15 != "" && StatusSearch_15 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_15);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_15 != "" && Population_GroupSearch_15 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_15);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_15 != "" && MaritalStatusSearch_15 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_15);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_15 != "" && DistrictSearch_15 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_15);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_15 != "" && MunicipalitySearch_15 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_15);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_15 != "" && Age_range_startSearch_15 != null && Age_range_startSearch_15 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_15);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_15 != "" && Age_range_endSearch_15 != null && Age_range_endSearch_15 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_15);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join m in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals m.RACAP_Case_Id into ChildPref
                                      from m in ChildPref.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 20 && d.Marital_Status_Id != null
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_15 == null || AdopTionReason_15 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_15 == null || GenderSearch_15 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_15 == null || StatusSearch_15 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_15 == null || Population_GroupSearch_15 == ""
                                      where i.Description.Contains(MaritalStatusSearch_15) || MaritalStatusSearch_15 == null || MaritalStatusSearch_15 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_15) || Child_preference_population_GroupSearch_15 == null || Child_preference_population_GroupSearch_15 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_15) || Child_preference_genderSearch_15 == null || Child_preference_genderSearch_15 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_15 == null || Age_range_startSearch_15 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_15 == null || Age_range_endSearch_15 == ""
                                      where d.Date_Created >= Date_Captured_From_15
                                      where d.Date_Created <= Date_Captured_To_15
                                      where a.Date_Registered >= Date_Registered_From_15
                                      where a.Date_Registered <= Date_Registered_To_15
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_15 != null && ProvinceSearch_15 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_15;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_15 != null && DistrictSearch_15 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_15 != null && MunicipalitySearch_15 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }

        public List<RACAPReportVM> RetrieveFifteenthReport(string ProvinceSearch_16, string DistrictSearch_16, string MunicipalitySearch_16, string AdopTionReason_16, string GenderSearch_16,
                            string Population_GroupSearch_16, string MaritalStatusSearch_16, string Age_range_startSearch_16, string Age_range_endSearch_16,
                            string StatusSearch_16, DateTime? Date_Captured_From_16, DateTime? Date_Captured_To_16, string Type_of_adoptionSearch_16,
                             DateTime? Date_Registered_From_16, DateTime? Date_Registered_To_16, string Three_year_period_startSearch_16,
                            string Three_year_period_endSearch_16, string Marital_StatusSearch_16, string Child_preference_ageSearch_16,
                            string Child_preference_genderSearch_16, string Child_preference_population_GroupSearch_16)
        {//20
            //ReportTypes= Finalised or matched cases
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_16 != "" && AdopTionReason_16 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_16);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_16 != "" && GenderSearch_16 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_16);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_16 != "" && StatusSearch_16 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_16);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_16 != "" && Population_GroupSearch_16 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_16);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_16 != "" && MaritalStatusSearch_16 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_16);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_16 != "" && DistrictSearch_16 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_16);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_16 != "" && MunicipalitySearch_16 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_16);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_16 != "" && Age_range_startSearch_16 != null && Age_range_startSearch_16 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_16);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_16 != "" && Age_range_endSearch_16 != null && Age_range_endSearch_16 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_16);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join m in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals m.RACAP_Case_Id into ChildPref
                                      from m in ChildPref.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 20 && a.RACAP_Record_Status_Id == 3
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_16 == null || AdopTionReason_16 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_16 == null || GenderSearch_16 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_16 == null || StatusSearch_16 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_16 == null || Population_GroupSearch_16 == ""
                                      where i.Description.Contains(MaritalStatusSearch_16) || MaritalStatusSearch_16 == null || MaritalStatusSearch_16 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_16) || Child_preference_population_GroupSearch_16 == null || Child_preference_population_GroupSearch_16 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_16) || Child_preference_genderSearch_16 == null || Child_preference_genderSearch_16 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_16 == null || Age_range_startSearch_16 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_16 == null || Age_range_endSearch_16 == ""
                                      where d.Date_Created >= Date_Captured_From_16
                                      where d.Date_Created <= Date_Captured_To_16
                                      where a.Date_Registered >= Date_Registered_From_16
                                      where a.Date_Registered <= Date_Registered_To_16
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_16 != null && ProvinceSearch_16 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_16;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_16 != null && DistrictSearch_16 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_16 != null && MunicipalitySearch_16 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }
        public List<RACAPReportVM> RetrieveSixteenthReport(string ProvinceSearch_17, string DistrictSearch_17, string MunicipalitySearch_17, string AdopTionReason_17, string GenderSearch_17,
                            string Population_GroupSearch_17, string MaritalStatusSearch_17, string Age_range_startSearch_17, string Age_range_endSearch_17,
                            string StatusSearch_17, DateTime? Date_Captured_From_17, DateTime? Date_Captured_To_17, string Type_of_adoptionSearch_17,
                             DateTime? Date_Registered_From_17, DateTime? Date_Registered_To_17, string Three_year_period_startSearch_3,
                            string Three_year_period_endSearch_17, string Marital_StatusSearch_17, string Child_preference_ageSearch_17,
                            string Child_preference_genderSearch_17, string Child_preference_population_GroupSearch_17)
        {//20
            //ReportType=Type of child preference
            #region AllSearches
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
            #region ConvertSearchItems
            string newAdopTionReasonSearch = "";
            if (AdopTionReason_17 != "" && AdopTionReason_17 != null)
            {
                int? AdoptReasonId = Convert.ToInt32(AdopTionReason_17);
                newAdopTionReasonSearch = db.apl_Adopting_Reason.Find(AdoptReasonId).Description;
            }
            string newGenderSearch = "";
            if (GenderSearch_17 != "" && GenderSearch_17 != null)
            {
                int? GenId = Convert.ToInt32(GenderSearch_17);
                newGenderSearch = db.Genders.Find(GenId).Description;
            }
            string newStatusSearch = "";
            if (StatusSearch_17 != "" && StatusSearch_17 != null)
            {
                int? StatusId = Convert.ToInt32(StatusSearch_17);
                newStatusSearch = db.apl_RACAP_Record_Status.Find(StatusId).Description;
            }
            string newPopulation_GroupSearch = "";
            if (Population_GroupSearch_17 != "" && Population_GroupSearch_17 != null)
            {
                int? PopGroupId = Convert.ToInt32(Population_GroupSearch_17);
                newPopulation_GroupSearch = db.Population_Groups.Find(PopGroupId).Description;
            }


            string newMaritalStatusSearch = "";
            if (MaritalStatusSearch_17 != "" && MaritalStatusSearch_17 != null)
            {
                int? MarStatusId = Convert.ToInt32(MaritalStatusSearch_17);
                newMaritalStatusSearch = db.Marital_Statusses.Find(MarStatusId).Description;
            }

            string newDistrictSearch = "";
            if (DistrictSearch_17 != "" && DistrictSearch_17 != null)
            {
                int? DistrictSearchId = Convert.ToInt32(DistrictSearch_17);
                newDistrictSearch = db.Districts.Find(DistrictSearchId).Description;
            }
            string newMunicipalitySearch = "";
            if (MunicipalitySearch_17 != "" && MunicipalitySearch_17 != null)
            {
                int? MunicipalitySearchId = Convert.ToInt32(MunicipalitySearch_17);
                newMunicipalitySearch = db.Local_Municipalities.Find(MunicipalitySearchId).Description;
            }
            int newAgeStartSearch = 0;
            if (Age_range_startSearch_17 != "" && Age_range_startSearch_17 != null && Age_range_startSearch_17 != " ")
            {
                newAgeStartSearch = Convert.ToInt32(Age_range_startSearch_17);
            }
            int newAge_range_endSearch = 0;
            if (Age_range_endSearch_17 != "" && Age_range_endSearch_17 != null && Age_range_endSearch_17 != " ")
            {
                newAge_range_endSearch = Convert.ToInt32(Age_range_endSearch_17);
            }

            #endregion
            #region PersonIdRequest
            var AllChildrenInRACAP = (from a in db.RACAP_Case_Details
                                      join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                      join c in db.Clients on b.Client_Id equals c.Client_Id
                                      join d in db.Persons on c.Person_Id equals d.Person_Id
                                      join e in db.Genders on d.Gender_Id equals e.Gender_Id into gs
                                      from e in gs.DefaultIfEmpty()
                                      join f in db.Population_Groups on d.Population_Group_Id equals f.Population_Group_Id into popgs
                                      from f in popgs.DefaultIfEmpty()
                                      join g in db.apl_RACAP_Record_Status on a.RACAP_Record_Status_Id equals g.RACAP_Record_Status_Id into rRecSts
                                      from g in rRecSts.DefaultIfEmpty()
                                      join h in db.apl_Adoption_Reason on a.Adoptions_Reason_Id equals h.Adoptions_Reason_Id into ls
                                      from h in ls.DefaultIfEmpty()
                                      join i in db.Marital_Statusses on d.Marital_Status_Id equals i.Marital_Status_Id into mars
                                      from i in mars.DefaultIfEmpty()
                                      join j in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals j.RACAP_Case_Id into RAC
                                      from j in RAC.DefaultIfEmpty()
                                      join k in db.Population_Groups on j.Population_Group_Id equals k.Population_Group_Id into childPop
                                      from k in childPop.DefaultIfEmpty()
                                      join l in db.Genders on j.Gender_Id equals l.Gender_Id into childGen
                                      from l in childGen.DefaultIfEmpty()
                                      join m in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals m.RACAP_Case_Id into ChildPref
                                      from m in ChildPref.DefaultIfEmpty()
                                      join o in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals o.RACAP_Case_Id into rOR
                                      from o in rOR.DefaultIfEmpty()
                                      where b.Problem_Sub_Category_Id == 20 && m.Body_Structure_Id!=null
                                      where h.Description.Contains(newAdopTionReasonSearch) || AdopTionReason_17 == null || AdopTionReason_17 == ""
                                      where e.Description.Contains(newGenderSearch) || GenderSearch_17 == null || GenderSearch_17 == ""
                                      where g.Description.Contains(newStatusSearch) || StatusSearch_17 == null || StatusSearch_17 == ""
                                      where f.Description.Contains(newPopulation_GroupSearch) || Population_GroupSearch_17 == null || Population_GroupSearch_17 == ""
                                      where i.Description.Contains(MaritalStatusSearch_17) || MaritalStatusSearch_17 == null || MaritalStatusSearch_17 == ""
                                      where k.Description.Contains(Child_preference_population_GroupSearch_17) || Child_preference_population_GroupSearch_17 == null || Child_preference_population_GroupSearch_17 == ""
                                      where k.Description.Contains(Child_preference_genderSearch_17) || Child_preference_genderSearch_17 == null || Child_preference_genderSearch_17 == ""
                                      where d.Age >= newAgeStartSearch || Age_range_startSearch_17 == null || Age_range_startSearch_17 == ""
                                      where d.Age <= newAge_range_endSearch || Age_range_endSearch_17 == null || Age_range_endSearch_17 == ""
                                      where d.Date_Created >= Date_Captured_From_17
                                      where d.Date_Created <= Date_Captured_To_17
                                      where a.Date_Registered >= Date_Registered_From_17
                                      where a.Date_Registered <= Date_Registered_To_17
                                      select d.Person_Id).Distinct().ToList();
            #endregion
            #region EachPersonDetails
            List<RACAPAddresListVM> AddList = new List<RACAPAddresListVM>();
            foreach (var item in AllChildrenInRACAP)
            {
                var aDdresses = db.Persons.Find(item).Addresses;
                var clientPhysicalAddress = (from a in aDdresses
                                             select a).FirstOrDefault();
                if (aDdresses.Count() != 0)
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Address_Id = clientPhysicalAddress.Address_Id;
                    newObj.Person_Id = item;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }
                    newObj.AddressLine1 = clientPhysicalAddress.Address_Line_1;
                    newObj.AddressLine2 = clientPhysicalAddress.Address_Line_2;
                    if (clientPhysicalAddress.Town_Id != null)
                    {
                        newObj.TownS = db.Towns.Find(clientPhysicalAddress.Town_Id).Description;
                        newObj.Town_Id = clientPhysicalAddress.Town_Id;
                    }
                    newObj.PostalCode = clientPhysicalAddress.Postal_Code;
                    newObj.ProvinceS = (from t in db.Towns
                                        join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                        join v in db.Districts on u.District_Municipality_Id equals v.District_Id
                                        join w in db.Provinces on v.Province_Id equals w.Province_Id
                                        where t.Town_Id == clientPhysicalAddress.Town_Id
                                        select w.Description).FirstOrDefault();
                    newObj.ChildLocationS = (from t in db.Towns
                                             join u in db.Local_Municipalities on t.Local_Municipality_Id equals u.Local_Municipality_Id
                                             where t.Town_Id == clientPhysicalAddress.Town_Id
                                             select u.Description).FirstOrDefault();
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();

                    #region Address_Search
                    //Province
                    if (ProvinceSearch_17 != null && ProvinceSearch_17 != "")
                    {
                        int? TwnId = db.Addresses.Find(clientPhysicalAddress.Address_Id).Town_Id;
                        if (TwnId != null)
                        {
                            int? ProvId = db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Province_Id;

                            string NewIdP = ProvinceSearch_17;
                            if (NewIdP == Convert.ToString(ProvId))
                            {
                                if (DistrictSearch_17 != null && DistrictSearch_17 != "")
                                {
                                    if (db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).District_Municipality_Id).Description == newDistrictSearch)
                                    {
                                        if (MunicipalitySearch_17 != null && MunicipalitySearch_17 != "")
                                        {
                                            if (db.Local_Municipalities.Find(db.Towns.Find(TwnId).Local_Municipality_Id).Description == newMunicipalitySearch)
                                            {
                                                AddList.Add(newObj);
                                            }
                                        }
                                        else
                                        {
                                            AddList.Add(newObj);
                                        }
                                    }
                                }
                                else
                                {
                                    AddList.Add(newObj);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddList.Add(newObj);
                    }
                    #endregion

                }
                else
                {
                    RACAPAddresListVM newObj = new RACAPAddresListVM();
                    newObj.Person_Id = item;
                    newObj.RecordStatusS = (from z in db.apl_RACAP_Record_Status
                                            join a in db.RACAP_Case_Details on z.RACAP_Record_Status_Id equals a.RACAP_Record_Status_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    newObj.Age = db.Persons.Find(item).Age;
                    newObj.DateCreated = db.Persons.Find(item).Date_Created;
                    newObj.DateRegistered = (from a in db.RACAP_Case_Details
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select a.Date_Registered).FirstOrDefault();
                    if ((from a in db.RACAP_Case_Details
                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select f.Organization_Id_Parent).FirstOrDefault() != null)
                    {
                        newObj.FacilitationOrgS = db.Organizations.Find((from a in db.RACAP_Case_Details
                                                                         join f in db.RACAP_OrgansationResponsible on a.RACAP_Case_Id equals f.RACAP_Case_Id
                                                                         join e in db.RACAP_Adoptive_Child on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                         join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                         join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                         where d.Person_Id == item
                                                                         select f.Organization_Id_Parent).FirstOrDefault()).Description;
                    }
                    if ((from a in db.RACAP_Case_Details
                         join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                         join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                         join c in db.Clients on b.Client_Id equals c.Client_Id
                         join d in db.Persons on c.Person_Id equals d.Person_Id
                         where d.Person_Id == item
                         select e.Body_Structure_Id).FirstOrDefault() != null)
                    {
                        newObj.ChildPreferencesS = db.apl_Body_Structure.Find((from a in db.RACAP_Case_Details
                                                                               join e in db.RACAP_Prospective_Parent on a.RACAP_Case_Id equals e.RACAP_Case_Id
                                                                               join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                                                               join c in db.Clients on b.Client_Id equals c.Client_Id
                                                                               join d in db.Persons on c.Person_Id equals d.Person_Id
                                                                               where d.Person_Id == item
                                                                               select e.Body_Structure_Id).FirstOrDefault()).Description;
                    }
                    if (db.Persons.Find(item).Marital_Status_Id != null)
                    {
                        newObj.MaritalStatusS = db.Marital_Statusses.Find((db.Persons.Find(item).Marital_Status_Id)).Description;
                    }

                    if (db.Persons.Find(item).Gender_Id != null)
                    {
                        newObj.GenderS = db.Genders.Find(db.Persons.Find(item).Gender_Id).Description;
                    }
                    if (db.Persons.Find(item).Population_Group_Id != null)
                    {
                        newObj.PopulationGroupS = db.Population_Groups.Find(db.Persons.Find(item).Population_Group_Id).Description;
                    }
                    newObj.ReasonForAdopS = (from z in db.apl_Adoption_Reason
                                             join a in db.RACAP_Case_Details on z.Adoptions_Reason_Id equals a.Adoptions_Reason_Id
                                             join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                             join c in db.Clients on b.Client_Id equals c.Client_Id
                                             join d in db.Persons on c.Person_Id equals d.Person_Id
                                             where d.Person_Id == item
                                             select z.Description).FirstOrDefault();

                    newObj.SpecialNeedsS = (from z in db.apl_Special_Need
                                            join e in db.RACAP_Adoptive_Child on z.Special_Needs_Id equals e.Special_Needs_Id
                                            join a in db.RACAP_Case_Details on e.RACAP_Case_Id equals a.RACAP_Case_Id
                                            join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                                            join c in db.Clients on b.Client_Id equals c.Client_Id
                                            join d in db.Persons on c.Person_Id equals d.Person_Id
                                            where d.Person_Id == item
                                            select z.Description).FirstOrDefault();
                    AddList.Add(newObj);
                }

            }
            #endregion
            #region ResultsPerReportType
            List<RACAPReportVM> ResultList = new List<RACAPReportVM>();
            foreach (var item in AddList)
            {

                // initialising view model
                RACAPReportVM obj = new RACAPReportVM();

                obj.Report_Id = item.Person_Id;
                obj.Province = item.ProvinceS;
                obj.ChildLocation = item.ChildLocationS;
                obj.RecordStatus = item.RecordStatusS;
                obj.Age = item.Age;
                obj.Gender = item.GenderS;
                obj.PopulationGroup = item.PopulationGroupS;
                obj.SpecialNeeds = item.SpecialNeedsS;
                obj.ReasonForAdoption = item.ReasonForAdopS;
                obj.DateRegistered = (item.DateRegistered);
                obj.ExpiryDate = Convert.ToDateTime(item.DateRegistered).AddDays(60);
                obj.DateCreated = (item.DateCreated);
                obj.MaritalStatus = item.MaritalStatusS;
                obj.FacilitatingOrganisation = item.FacilitationOrgS;

                ResultList.Add(obj);
            }
            #endregion       
            return ResultList;
            #endregion
        }

        public int GetRACAP_Report_Type_Id()
        {
            return (from k in db.apl_RACAP_Report_Type
                    select k.RACAP_Report_Type_Id).FirstOrDefault();
        }

        public SelectList GetReportType()
        {
            return new SelectList(db.apl_RACAP_Report_Type, "RACAP_Report_Type_Id", "Description");
        }

        public int GetProvinceId()
        {
            return (from k in db.Provinces
                    orderby k.Description
                    select k.Province_Id).FirstOrDefault();
        }
        public SelectList GetProvincesForReports()
        {
            return new SelectList(db.Provinces, "Province_Id", "Description");
        }
    }
}
