using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;

namespace Common_Objects.Models
{
    public class LookupTableModel
    {
        private List<T> GetByCustomCriteria<T>() where T : class
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            var objectSet = objectContext.CreateObjectSet<T>();

            return new List<T>(objectSet);
        }

        private List<T> GetByCustomCriteria<T>(string criteria) where T : class
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            var objectSet = objectContext.CreateObjectSet<T>();

            return new List<T>(objectSet.Where(criteria));
        }

        public LookupDataItem GetSpecificLookupTableItem(int lookupDataTypeId, int lookupDataItemId)
        {
            var lookupData = new LookupDataItem();

            switch (lookupDataTypeId)
            {
                case (int)LookupTableEnum.AbuseTypes:
                    var abuseTypes = GetByCustomCriteria<Abuse_Type>(string.Format("it.Abuse_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in abuseTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Abuse_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();
                    break;
                case (int)LookupTableEnum.Allergies:
                    var allergies = GetByCustomCriteria<Allergy>(string.Format("it.Allergy_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in allergies
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Allergy_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();
                    break;

                case (int)LookupTableEnum.ChronicIllnesses:
                    var chronicIllnesses = GetByCustomCriteria<Chronic_Illness>(string.Format("it.Chronic_Illness_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in chronicIllnesses
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Chronic_Illness_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.ClientTypes:
                    var clientTypes = GetByCustomCriteria<Client_Type>(string.Format("it.Client_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in clientTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Client_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Disabilities:
                    var disabilities = GetByCustomCriteria<Disability>(string.Format("it.Disability_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in disabilities
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Disability_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.EyeColors:
                    var eyeColors = GetByCustomCriteria<Eye_Color>(string.Format("it.Eye_Color_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in eyeColors
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Eye_Color_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Genders:
                    var genders = GetByCustomCriteria<Gender>(string.Format("it.Gender_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in genders
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Gender_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Grades:
                    var grades = GetByCustomCriteria<Grade>(string.Format("it.Grade_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in grades
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Grade_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.HairColors:
                    var hairColors = GetByCustomCriteria<Hair_Color>(string.Format("it.Hair_Color_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in hairColors
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Hair_Color_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.IdenficationTypes:
                    var identificationTypes = GetByCustomCriteria<Identification_Type>(string.Format("it.Identification_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in identificationTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Identification_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.IncomeRanges:
                    var incomeRanges = GetByCustomCriteria<Income_Range>(string.Format("it.Income_Range_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in incomeRanges
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Income_Range_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.InjuryTypes:
                    var injuryTypes = GetByCustomCriteria<Injury_Type>(string.Format("it.Injury_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in injuryTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Injury_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.JobPositions:
                    var jobPositions = GetByCustomCriteria<Job_Position>(string.Format("it.Job_Position_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in jobPositions
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Job_Position_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Languages:
                    var languages = GetByCustomCriteria<Language>(string.Format("it.Language_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in languages
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Language_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.MaritalStatusses:
                    var maritalStatusses = GetByCustomCriteria<Marital_Status>(string.Format("it.Marital_Status_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in maritalStatusses
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Marital_Status_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Nationalities:
                    var nationalities = GetByCustomCriteria<Nationality>(string.Format("it.Nationality_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in nationalities
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Nationality_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Occupations:
                    var occupations = GetByCustomCriteria<Occupation>(string.Format("it.Occupation_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in occupations
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Occupation_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.OffenceCategories:
                    var offenceCategories = GetByCustomCriteria<Offence_Category>(string.Format("it.Offence_Category_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in offenceCategories
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Offence_Category_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.PopulationGroups:
                    var populationGroups = GetByCustomCriteria<Population_Group>(string.Format("it.Population_Group_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in populationGroups
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Population_Group_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.PreferredContactTypes:
                    var preferredContactTypes = GetByCustomCriteria<Preferred_Contact_Type>(string.Format("it.Preferred_Contact_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in preferredContactTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Preferred_Contact_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Races:
                    var races = GetByCustomCriteria<Race>(string.Format("it.Race_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in races
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Race_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.ReceptionActionTypes:
                    var receptionActionTakenItems = GetByCustomCriteria<Reception_Action_Taken>(string.Format("it.Reception_Action_Taken_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in receptionActionTakenItems
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Reception_Action_Taken_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.ReceptionVisitTypes:
                    var receptionVisitTypes = GetByCustomCriteria<Reception_Visit_Type>(string.Format("it.Reception_Visit_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in receptionVisitTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Reception_Visit_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.ReferralFocusArea:
                    var referralFoxusAreas = GetByCustomCriteria<Referral_Focus_Area>(string.Format("it.Reception_Focus_Area_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in referralFoxusAreas
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Referral_Focus_Area_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.RelationshipTypes:
                    var relationshipTypes = GetByCustomCriteria<Relationship_Type>(string.Format("it.Relationship_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in relationshipTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Relationship_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.Religions:
                    var religions = GetByCustomCriteria<Religion>(string.Format("it.Religion_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in religions
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Religion_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;
                case (int)LookupTableEnum.SchoolTypes:
                    var schoolTypes = GetByCustomCriteria<School_Type>(string.Format("it.School_Type_Id == {0}", lookupDataItemId.ToString(CultureInfo.InvariantCulture)));

                    lookupData = (from p in schoolTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.School_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).FirstOrDefault();

                    break;

            }

            return lookupData;
        }

        public List<LookupDataItem> GetLookupTableItems(int lookupDataTypeId)
        {
            var lookupData = new List<LookupDataItem>();

            switch (lookupDataTypeId)
            {
                case (int)LookupTableEnum.AbuseTypes:
                    var abuseTypes = GetByCustomCriteria<Abuse_Type>();

                    lookupData = (from p in abuseTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Abuse_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();
                    break;
                case (int)LookupTableEnum.Allergies:
                    var allergies = GetByCustomCriteria<Allergy>();

                    lookupData = (from p in allergies
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Allergy_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();
                    break;

                case (int)LookupTableEnum.ChronicIllnesses:
                    var chronicIllnesses = GetByCustomCriteria<Chronic_Illness>();

                    lookupData = (from p in chronicIllnesses
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Chronic_Illness_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.ClientTypes:
                    var clientTypes = GetByCustomCriteria<Client_Type>();

                    lookupData = (from p in clientTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Client_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Disabilities:
                    var disabilities = GetByCustomCriteria<Disability>();

                    lookupData = (from p in disabilities
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Disability_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.EyeColors:
                    var eyeColors = GetByCustomCriteria<Eye_Color>();

                    lookupData = (from p in eyeColors
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Eye_Color_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Genders:
                    var genders = GetByCustomCriteria<Gender>();

                    lookupData = (from p in genders
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Gender_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Grades:
                    var grades = GetByCustomCriteria<Grade>();

                    lookupData = (from p in grades
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Grade_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.HairColors:
                    var hairColors = GetByCustomCriteria<Hair_Color>();

                    lookupData = (from p in hairColors
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Hair_Color_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.IdenficationTypes:
                    var identificationTypes = GetByCustomCriteria<Identification_Type>();

                    lookupData = (from p in identificationTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Identification_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.IncomeRanges:
                    var incomeRanges = GetByCustomCriteria<Income_Range>();

                    lookupData = (from p in incomeRanges
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Income_Range_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.InjuryTypes:
                    var injuryTypes = GetByCustomCriteria<Injury_Type>();

                    lookupData = (from p in injuryTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Injury_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.JobPositions:
                    var jobPositions = GetByCustomCriteria<Job_Position>();

                    lookupData = (from p in jobPositions
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Job_Position_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Languages:
                    var languages = GetByCustomCriteria<Language>();

                    lookupData = (from p in languages
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Language_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.MaritalStatusses:
                    var maritalStatusses = GetByCustomCriteria<Marital_Status>();

                    lookupData = (from p in maritalStatusses
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Marital_Status_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Nationalities:
                    var nationalities = GetByCustomCriteria<Nationality>();

                    lookupData = (from p in nationalities
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Nationality_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Occupations:
                    var occupations = GetByCustomCriteria<Occupation>();

                    lookupData = (from p in occupations
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Occupation_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.OffenceCategories:
                    var offenceCategories = GetByCustomCriteria<Offence_Category>();

                    lookupData = (from p in offenceCategories
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Offence_Category_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.PopulationGroups:
                    var populationGroups = GetByCustomCriteria<Population_Group>();

                    lookupData = (from p in populationGroups
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Population_Group_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.PreferredContactTypes:
                    var preferredContactTypes = GetByCustomCriteria<Preferred_Contact_Type>();

                    lookupData = (from p in preferredContactTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Preferred_Contact_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Races:
                    var races = GetByCustomCriteria<Race>();

                    lookupData = (from p in races
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Race_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.ReceptionActionTypes:
                    var receptionActionTakenItems = GetByCustomCriteria<Reception_Action_Taken>();

                    lookupData = (from p in receptionActionTakenItems
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Reception_Action_Taken_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.ReceptionVisitTypes:
                    var receptionVisitTypes = GetByCustomCriteria<Reception_Visit_Type>();

                    lookupData = (from p in receptionVisitTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Reception_Visit_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.ReferralFocusArea:
                    var referralFocusAreas = GetByCustomCriteria<Referral_Focus_Area>();

                    lookupData = (from p in referralFocusAreas
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Referral_Focus_Area_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.RelationshipTypes:
                    var relationshipTypes = GetByCustomCriteria<Relationship_Type>();

                    lookupData = (from p in relationshipTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Relationship_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.Religions:
                    var religions = GetByCustomCriteria<Religion>();

                    lookupData = (from p in religions
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.Religion_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;
                case (int)LookupTableEnum.SchoolTypes:
                    var schoolTypes = GetByCustomCriteria<School_Type>();

                    lookupData = (from p in schoolTypes
                                  select new LookupDataItem()
                                  {
                                      ItemId = p.School_Type_Id,
                                      LookupTableTypeId = lookupDataTypeId,
                                      Description = p.Description,
                                      Source = p.Source,
                                      Definition = p.Definition
                                  }).ToList();

                    break;

            }

            return lookupData;
        }

        public LookupDataItem CreateLookupDataItem(int lookupDataTypeId, string description, string source, string definition)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var lookupDataItem = new LookupDataItem() { Description = description, Source = source, Definition = definition };

            try
            {
                switch (lookupDataTypeId)
                {
                    case (int)LookupTableEnum.AbuseTypes:
                        var abuseType = new Abuse_Type() { Description = description, Source = source, Definition = definition };
                        var newAbuseType = dbContext.Abuse_Types.Add(abuseType);

                        dbContext.SaveChanges();

                        if (newAbuseType == null) return null;

                        lookupDataItem.ItemId = newAbuseType.Abuse_Type_Id;
                        break;
                    case (int)LookupTableEnum.Allergies:
                        var allergy = new Allergy() { Description = description, Source = source, Definition = definition };
                        var newAllergy = dbContext.Allergies.Add(allergy);

                        dbContext.SaveChanges();

                        if (newAllergy == null) return null;

                        lookupDataItem.ItemId = newAllergy.Allergy_Id;
                        break;
                    case (int)LookupTableEnum.ChronicIllnesses:
                        var chronicIllness = new Chronic_Illness() { Description = description, Source = source, Definition = definition };
                        var newChronicIllness = dbContext.Chronic_Illnesses.Add(chronicIllness);

                        dbContext.SaveChanges();

                        if (newChronicIllness == null) return null;

                        lookupDataItem.ItemId = newChronicIllness.Chronic_Illness_Id;
                        break;
                    case (int)LookupTableEnum.ClientTypes:
                        var clientType = new Client_Type() { Description = description, Source = source, Definition = definition };
                        var newClientType = dbContext.Client_Types.Add(clientType);

                        dbContext.SaveChanges();

                        if (newClientType == null) return null;

                        lookupDataItem.ItemId = newClientType.Client_Type_Id;
                        break;
                    case (int)LookupTableEnum.Disabilities:
                        var disability = new Disability() { Description = description, Source = source, Definition = definition };
                        var newDisability = dbContext.Disabilities.Add(disability);

                        dbContext.SaveChanges();

                        if (newDisability == null) return null;

                        lookupDataItem.ItemId = newDisability.Disability_Id;
                        break;
                    case (int)LookupTableEnum.EyeColors:
                        var eyeColor = new Eye_Color() { Description = description, Source = source, Definition = definition };
                        var newEyeColor = dbContext.Eye_Colors.Add(eyeColor);

                        dbContext.SaveChanges();

                        if (newEyeColor == null) return null;

                        lookupDataItem.ItemId = newEyeColor.Eye_Color_Id;
                        break;
                    case (int)LookupTableEnum.Genders:
                        var gender = new Gender() { Description = description, Source = source, Definition = definition };
                        var newGender = dbContext.Genders.Add(gender);

                        dbContext.SaveChanges();

                        if (newGender == null) return null;

                        lookupDataItem.ItemId = newGender.Gender_Id;
                        break;
                    case (int)LookupTableEnum.Grades:
                        var grade = new Grade() { Description = description, Source = source, Definition = definition };
                        var newGrade = dbContext.Grades.Add(grade);

                        dbContext.SaveChanges();

                        if (newGrade == null) return null;

                        lookupDataItem.ItemId = newGrade.Grade_Id;
                        break;
                    case (int)LookupTableEnum.HairColors:
                        var hairColor = new Hair_Color() { Description = description, Source = source, Definition = definition };
                        var newHairColor = dbContext.Hair_Colors.Add(hairColor);

                        dbContext.SaveChanges();

                        if (newHairColor == null) return null;

                        lookupDataItem.ItemId = newHairColor.Hair_Color_Id;
                        break;
                    case (int)LookupTableEnum.IdenficationTypes:
                        var identificationType = new Identification_Type() { Description = description, Source = source, Definition = definition };
                        var newIdentificationType = dbContext.Identification_Types.Add(identificationType);

                        dbContext.SaveChanges();

                        if (newIdentificationType == null) return null;

                        lookupDataItem.ItemId = newIdentificationType.Identification_Type_Id;
                        break;
                    case (int)LookupTableEnum.IncomeRanges:
                        var incomeRange = new Income_Range() { Description = description, Source = source, Definition = definition };
                        var newIncomeRange = dbContext.Income_Ranges.Add(incomeRange);

                        dbContext.SaveChanges();

                        if (newIncomeRange == null) return null;

                        lookupDataItem.ItemId = newIncomeRange.Income_Range_Id;
                        break;
                    case (int)LookupTableEnum.InjuryTypes:
                        var injuryType = new Injury_Type() { Description = description, Source = source, Definition = definition };
                        var newInjuryType = dbContext.Injury_Types.Add(injuryType);

                        dbContext.SaveChanges();

                        if (newInjuryType == null) return null;

                        lookupDataItem.ItemId = newInjuryType.Injury_Type_Id;
                        break;
                    case (int)LookupTableEnum.JobPositions:
                        var jobPosition = new Job_Position() { Description = description, Source = source, Definition = definition };
                        var newJobPosition = dbContext.Job_Positions.Add(jobPosition);

                        dbContext.SaveChanges();

                        if (newJobPosition == null) return null;

                        lookupDataItem.ItemId = newJobPosition.Job_Position_Id;
                        break;
                    case (int)LookupTableEnum.Languages:
                        var language = new Language() { Description = description, Source = source, Definition = definition };
                        var newLanguage = dbContext.Languages.Add(language);

                        dbContext.SaveChanges();

                        if (newLanguage == null) return null;

                        lookupDataItem.ItemId = newLanguage.Language_Id;
                        break;
                    case (int)LookupTableEnum.MaritalStatusses:
                        var maritalStatus = new Marital_Status() { Description = description, Source = source, Definition = definition };
                        var newMaritalStatus = dbContext.Marital_Statusses.Add(maritalStatus);

                        dbContext.SaveChanges();

                        if (newMaritalStatus == null) return null;

                        lookupDataItem.ItemId = newMaritalStatus.Marital_Status_Id;
                        break;
                    case (int)LookupTableEnum.Nationalities:
                        var nationality = new Nationality() { Description = description, Source = source, Definition = definition };
                        var newNationality = dbContext.Nationalities.Add(nationality);

                        dbContext.SaveChanges();

                        if (newNationality == null) return null;

                        lookupDataItem.ItemId = newNationality.Nationality_Id;
                        break;
                    case (int)LookupTableEnum.Occupations:
                        var occupation = new Occupation() { Description = description, Source = source, Definition = definition };
                        var newOccupation = dbContext.Occupations.Add(occupation);

                        dbContext.SaveChanges();

                        if (newOccupation == null) return null;

                        lookupDataItem.ItemId = newOccupation.Occupation_Id;
                        break;
                    case (int)LookupTableEnum.OffenceCategories:
                        var offenceCategory = new Offence_Category() { Description = description, Source = source, Definition = definition };
                        var newOffenceCategory = dbContext.Offence_Categories.Add(offenceCategory);

                        dbContext.SaveChanges();

                        if (newOffenceCategory == null) return null;

                        lookupDataItem.ItemId = newOffenceCategory.Offence_Category_Id;
                        break;
                    case (int)LookupTableEnum.PopulationGroups:
                        var populationGroup = new Population_Group() { Description = description, Source = source, Definition = definition };
                        var newPopulationGroup = dbContext.Population_Groups.Add(populationGroup);

                        dbContext.SaveChanges();

                        if (newPopulationGroup == null) return null;

                        lookupDataItem.ItemId = newPopulationGroup.Population_Group_Id;
                        break;
                    case (int)LookupTableEnum.PreferredContactTypes:
                        var preferredContactType = new Preferred_Contact_Type() { Description = description, Source = source, Definition = definition };
                        var newPreferredContactType = dbContext.Preferred_Contact_Types.Add(preferredContactType);

                        dbContext.SaveChanges();

                        if (newPreferredContactType == null) return null;

                        lookupDataItem.ItemId = newPreferredContactType.Preferred_Contact_Type_Id;
                        break;
                    case (int)LookupTableEnum.Races:
                        var race = new Race() { Description = description, Source = source, Definition = definition };
                        var newRace = dbContext.Races.Add(race);

                        dbContext.SaveChanges();

                        if (newRace == null) return null;

                        lookupDataItem.ItemId = newRace.Race_Id;
                        break;
                    case (int)LookupTableEnum.ReceptionActionTypes:
                        var receptionActionTaken = new Reception_Action_Taken() { Description = description, Source = source, Definition = definition };
                        var newReceptionAction = dbContext.Reception_Action_Taken_Items.Add(receptionActionTaken);

                        dbContext.SaveChanges();

                        if (newReceptionAction == null) return null;

                        lookupDataItem.ItemId = newReceptionAction.Reception_Action_Taken_Id;
                        break;
                    case (int)LookupTableEnum.ReceptionVisitTypes:
                        var receptionVisitType = new Reception_Visit_Type() { Description = description, Source = source, Definition = definition };
                        var newReceptionVisitType = dbContext.Reception_Visit_Types.Add(receptionVisitType);

                        dbContext.SaveChanges();

                        if (newReceptionVisitType == null) return null;

                        lookupDataItem.ItemId = newReceptionVisitType.Reception_Visit_Type_Id;
                        break;
                    case (int)LookupTableEnum.ReferralFocusArea:
                        var referralFocusArea = new Referral_Focus_Area() { Description = description, Source = source, Definition = definition };
                        var newReferralFocusArea = dbContext.Referral_Focus_Areas.Add(referralFocusArea);

                        dbContext.SaveChanges();

                        if (newReferralFocusArea == null) return null;

                        lookupDataItem.ItemId = newReferralFocusArea.Referral_Focus_Area_Id;
                        break;
                    case (int)LookupTableEnum.RelationshipTypes:
                        var relationshipType = new Relationship_Type() { Description = description, Source = source, Definition = definition };
                        var newRelationshipType = dbContext.Relationship_Types.Add(relationshipType);

                        dbContext.SaveChanges();

                        if (newRelationshipType == null) return null;

                        lookupDataItem.ItemId = newRelationshipType.Relationship_Type_Id;
                        break;
                    case (int)LookupTableEnum.Religions:
                        var religion = new Religion() { Description = description, Source = source, Definition = definition };
                        var newReligion = dbContext.Religions.Add(religion);

                        dbContext.SaveChanges();

                        if (newReligion == null) return null;

                        lookupDataItem.ItemId = newReligion.Religion_Id;
                        break;
                    case (int)LookupTableEnum.SchoolTypes:
                        var schoolType = new School_Type() { Description = description, Source = source, Definition = definition };
                        var newSchoolType = dbContext.School_Types.Add(schoolType);

                        dbContext.SaveChanges();

                        if (newSchoolType == null) return null;

                        lookupDataItem.ItemId = newSchoolType.School_Type_Id;
                        break;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return lookupDataItem;
        }

        public LookupDataItem EditLookupDataItem(int lookupDataItemId, int lookupDataTypeId, string description, string source, string definition)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var lookupDataItem = new LookupDataItem() { Description = description, Source = source, Definition = definition };

            try
            {
                switch (lookupDataTypeId)
                {
                    case (int)LookupTableEnum.AbuseTypes:
                        var editAbuseType = (from p in dbContext.Abuse_Types
                                             where p.Abuse_Type_Id.Equals(lookupDataItemId)
                                             select p).FirstOrDefault();

                        if (editAbuseType == null) return null;

                        editAbuseType.Description = description;
                        editAbuseType.Source = source;
                        editAbuseType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Allergies:
                        var editAllergyType = (from p in dbContext.Allergies
                                               where p.Allergy_Id.Equals(lookupDataItemId)
                                               select p).FirstOrDefault();

                        if (editAllergyType == null) return null;

                        editAllergyType.Description = description;
                        editAllergyType.Source = source;
                        editAllergyType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.ChronicIllnesses:
                        var editChronicIllness = (from p in dbContext.Chronic_Illnesses
                                                  where p.Chronic_Illness_Id.Equals(lookupDataItemId)
                                                  select p).FirstOrDefault();

                        if (editChronicIllness == null) return null;

                        editChronicIllness.Description = description;
                        editChronicIllness.Source = source;
                        editChronicIllness.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.ClientTypes:
                        var editClientType = (from p in dbContext.Client_Types
                                              where p.Client_Type_Id.Equals(lookupDataItemId)
                                              select p).FirstOrDefault();

                        if (editClientType == null) return null;

                        editClientType.Description = description;
                        editClientType.Source = source;
                        editClientType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Disabilities:
                        var editDisability = (from p in dbContext.Disabilities
                                              where p.Disability_Id.Equals(lookupDataItemId)
                                              select p).FirstOrDefault();

                        if (editDisability == null) return null;

                        editDisability.Description = description;
                        editDisability.Source = source;
                        editDisability.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.EyeColors:
                        var editEyeColor = (from p in dbContext.Eye_Colors
                                            where p.Eye_Color_Id.Equals(lookupDataItemId)
                                            select p).FirstOrDefault();

                        if (editEyeColor == null) return null;

                        editEyeColor.Description = description;
                        editEyeColor.Source = source;
                        editEyeColor.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Genders:
                        var editGenders = (from p in dbContext.Genders
                                           where p.Gender_Id.Equals(lookupDataItemId)
                                           select p).FirstOrDefault();

                        if (editGenders == null) return null;

                        editGenders.Description = description;
                        editGenders.Source = source;
                        editGenders.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Grades:
                        var editGrade = (from p in dbContext.Grades
                                         where p.Grade_Id.Equals(lookupDataItemId)
                                         select p).FirstOrDefault();

                        if (editGrade == null) return null;

                        editGrade.Description = description;
                        editGrade.Source = source;
                        editGrade.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.HairColors:
                        var editHairColor = (from p in dbContext.Hair_Colors
                                             where p.Hair_Color_Id.Equals(lookupDataItemId)
                                             select p).FirstOrDefault();

                        if (editHairColor == null) return null;

                        editHairColor.Description = description;
                        editHairColor.Source = source;
                        editHairColor.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.IdenficationTypes:
                        var editIdentificationType = (from p in dbContext.Identification_Types
                                                      where p.Identification_Type_Id.Equals(lookupDataItemId)
                                                      select p).FirstOrDefault();

                        if (editIdentificationType == null) return null;

                        editIdentificationType.Description = description;
                        editIdentificationType.Source = source;
                        editIdentificationType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.IncomeRanges:
                        var editIncomeRanges = (from p in dbContext.Income_Ranges
                                                where p.Income_Range_Id.Equals(lookupDataItemId)
                                                select p).FirstOrDefault();

                        if (editIncomeRanges == null) return null;

                        editIncomeRanges.Description = description;
                        editIncomeRanges.Source = source;
                        editIncomeRanges.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.InjuryTypes:
                        var editInjuryTypes = (from p in dbContext.Injury_Types
                                               where p.Injury_Type_Id.Equals(lookupDataItemId)
                                               select p).FirstOrDefault();

                        if (editInjuryTypes == null) return null;

                        editInjuryTypes.Description = description;
                        editInjuryTypes.Source = source;
                        editInjuryTypes.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.JobPositions:
                        var editJobPositions = (from p in dbContext.Job_Positions
                                                where p.Job_Position_Id.Equals(lookupDataItemId)
                                                select p).FirstOrDefault();

                        if (editJobPositions == null) return null;

                        editJobPositions.Description = description;
                        editJobPositions.Source = source;
                        editJobPositions.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Languages:
                        var editLanguages = (from p in dbContext.Languages
                                             where p.Language_Id.Equals(lookupDataItemId)
                                             select p).FirstOrDefault();

                        if (editLanguages == null) return null;

                        editLanguages.Description = description;
                        editLanguages.Source = source;
                        editLanguages.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.MaritalStatusses:
                        var editMaritalStatusses = (from p in dbContext.Marital_Statusses
                                                    where p.Marital_Status_Id.Equals(lookupDataItemId)
                                                    select p).FirstOrDefault();

                        if (editMaritalStatusses == null) return null;

                        editMaritalStatusses.Description = description;
                        editMaritalStatusses.Source = source;
                        editMaritalStatusses.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Nationalities:
                        var editNationalities = (from p in dbContext.Nationalities
                                                 where p.Nationality_Id.Equals(lookupDataItemId)
                                                 select p).FirstOrDefault();

                        if (editNationalities == null) return null;

                        editNationalities.Description = description;
                        editNationalities.Source = source;
                        editNationalities.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Occupations:
                        var editOccupation = (from p in dbContext.Occupations
                                              where p.Occupation_Id.Equals(lookupDataItemId)
                                              select p).FirstOrDefault();

                        if (editOccupation == null) return null;

                        editOccupation.Description = description;
                        editOccupation.Source = source;
                        editOccupation.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.OffenceCategories:
                        var editOffenceCategory = (from p in dbContext.Offence_Categories
                                                   where p.Offence_Category_Id.Equals(lookupDataItemId)
                                                   select p).FirstOrDefault();

                        if (editOffenceCategory == null) return null;

                        editOffenceCategory.Description = description;
                        editOffenceCategory.Source = source;
                        editOffenceCategory.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.PopulationGroups:
                        var editPopulationGroup = (from p in dbContext.Population_Groups
                                                   where p.Population_Group_Id.Equals(lookupDataItemId)
                                                   select p).FirstOrDefault();

                        if (editPopulationGroup == null) return null;

                        editPopulationGroup.Description = description;
                        editPopulationGroup.Source = source;
                        editPopulationGroup.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.PreferredContactTypes:
                        var editPreferredContactType = (from p in dbContext.Preferred_Contact_Types
                                                        where p.Preferred_Contact_Type_Id.Equals(lookupDataItemId)
                                                        select p).FirstOrDefault();

                        if (editPreferredContactType == null) return null;

                        editPreferredContactType.Description = description;
                        editPreferredContactType.Source = source;
                        editPreferredContactType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Races:
                        var editRace = (from p in dbContext.Races
                                        where p.Race_Id.Equals(lookupDataItemId)
                                        select p).FirstOrDefault();

                        if (editRace == null) return null;

                        editRace.Description = description;
                        editRace.Source = source;
                        editRace.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.ReceptionActionTypes:
                        var editReceptionActionTaken = (from p in dbContext.Reception_Action_Taken_Items
                                                        where p.Reception_Action_Taken_Id.Equals(lookupDataItemId)
                                                        select p).FirstOrDefault();

                        if (editReceptionActionTaken == null) return null;

                        editReceptionActionTaken.Description = description;
                        editReceptionActionTaken.Source = source;
                        editReceptionActionTaken.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.ReceptionVisitTypes:
                        var editReceptionVisitType = (from p in dbContext.Reception_Visit_Types
                                                      where p.Reception_Visit_Type_Id.Equals(lookupDataItemId)
                                                      select p).FirstOrDefault();

                        if (editReceptionVisitType == null) return null;

                        editReceptionVisitType.Description = description;
                        editReceptionVisitType.Source = source;
                        editReceptionVisitType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.ReferralFocusArea:
                        var editReferralFocusArea = (from r in dbContext.Referral_Focus_Areas
                                                     where r.Referral_Focus_Area_Id.Equals(lookupDataItemId)
                                                     select r).FirstOrDefault();

                        if (editReferralFocusArea == null) return null;

                        editReferralFocusArea.Description = description;
                        editReferralFocusArea.Source = source;
                        editReferralFocusArea.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.RelationshipTypes:
                        var editRelationshipType = (from p in dbContext.Relationship_Types
                                                    where p.Relationship_Type_Id.Equals(lookupDataItemId)
                                                    select p).FirstOrDefault();

                        if (editRelationshipType == null) return null;

                        editRelationshipType.Description = description;
                        editRelationshipType.Source = source;
                        editRelationshipType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.Religions:
                        var editReligion = (from p in dbContext.Religions
                                            where p.Religion_Id.Equals(lookupDataItemId)
                                            select p).FirstOrDefault();

                        if (editReligion == null) return null;

                        editReligion.Description = description;
                        editReligion.Source = source;
                        editReligion.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                    case (int)LookupTableEnum.SchoolTypes:
                        var editSchoolType = (from p in dbContext.School_Types
                                              where p.School_Type_Id.Equals(lookupDataItemId)
                                              select p).FirstOrDefault();

                        if (editSchoolType == null) return null;

                        editSchoolType.Description = description;
                        editSchoolType.Source = source;
                        editSchoolType.Definition = definition;

                        dbContext.SaveChanges();

                        break;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return lookupDataItem;
        }
    }
}
