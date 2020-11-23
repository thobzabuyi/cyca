using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProfilingInstanceModel
    {
        public Profiling_Instance GetSpecificProfilingInstance(int profilingInstanceId)
        {
            Profiling_Instance profilingInstance;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingInstanceList = (from x in dbContext.Profiling_Instances
                                             where x.Profiling_Instance_Id.Equals(profilingInstanceId)
                                             where x.Is_Active && !x.Is_Deleted
                                             select x).ToList();

                profilingInstance = (from r in profilingInstanceList
                                     select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingInstance;
        }

        public List<Profiling_Instance> GetListOfProfilingInstances(bool showInActive, bool showDeleted)
        {
            List<Profiling_Instance> profilingInstances;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingInstancesList = (from x in dbContext.Profiling_Instances
                                              where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                              where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                              select x).ToList();

                profilingInstances = (from x in profilingInstancesList
                                      select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingInstances;
        }

        public List<Profiling_Instance> GetListOfProfilingInstances(bool showInActive, bool showDeleted, int capturedByUserId)
        {
            List<Profiling_Instance> profilingInstances;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingInstancesList = (from x in dbContext.Profiling_Instances
                                              where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                              where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                              where x.Captured_By_UserId.Equals(capturedByUserId)
                                              select x).ToList();

                profilingInstances = (from x in profilingInstancesList
                                      select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingInstances;
        }

        public List<Profiling_Instance> GetListOfProfilingInstances(int provinceId)
        {
            List<Profiling_Instance> profilingInstances;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingInstancesList = (from x in dbContext.Profiling_Instances
                                              where x.Is_Active.Equals(true)
                                              where x.Is_Deleted.Equals(false)
                                              where x.NISIS_Site_EA.NISIS_Site.Ward.Local_Municipality.District.Province_Id.Equals(provinceId)
                                              select x).ToList();

                profilingInstances = (from x in profilingInstancesList
                                      select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingInstances;
        }

        public Profiling_Instance CreateProfilingInstance(DateTime profilingDate, int profilingToolId, int capturedByUserId, string generatedHHID, int siteEAId, int? dwellingUnitNumber, int? householdNumber, int? householdNumberOfMales, int? householdNumberOfFemales, string dwellingUnitAddress, string dwellingUnitDescription, string createdBy, DateTime createdDate, bool isActive, bool isDeleted)
        {
            Profiling_Instance newProfilingInstance;

            var dbContext = new SDIIS_DatabaseEntities();

            var profilingInstance = new Profiling_Instance()
            {
                Profiling_Date = profilingDate,
                Profiling_Tool_Id = profilingToolId,
                Profiling_Method_Id = (int)ProfilingMethodEnum.PaperQuestionnaire,
                Captured_By_UserId = capturedByUserId,
                HHID = generatedHHID,
                NISIS_Site_EA_Id = siteEAId,
                Dwelling_Unit_Number = dwellingUnitNumber,
                Household_Number = householdNumber,
                Household_Number_Of_Males = householdNumberOfMales,
                Household_Number_Of_Females = householdNumberOfFemales,
                Dwelling_Unit_Address = dwellingUnitAddress,
                Dwelling_Unit_Description = dwellingUnitDescription,
                QA_Status_Item_Id = (int)QAStatusEnum.NotSet,
                Created_By = createdBy,
                Date_Created = createdDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted
            };

            try
            {
                newProfilingInstance = dbContext.Profiling_Instances.Add(profilingInstance);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newProfilingInstance;
        }

        public Profiling_Instance EditProfilingInstance(int profilingInstanceId, DateTime profilingDate, int profilingToolId, int capturedByUserId, string generatedHHID, int siteEAId, int? dwellingUnitNumber, int? householdNumber, int? householdNumberOfMales, int? householdNumberOfFemales, string dwellingUnitAddress, string dwellingUnitDescription, string modifiedBy, DateTime? dateLastModified, bool isActive, bool isDeleted)
        {
            Profiling_Instance editProfilingInstance;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editProfilingInstance = (from x in dbContext.Profiling_Instances
                                         where x.Profiling_Instance_Id.Equals(profilingInstanceId)
                                         select x).FirstOrDefault();

                if (editProfilingInstance == null) return null;

                editProfilingInstance.Profiling_Date = profilingDate;
                editProfilingInstance.Profiling_Tool_Id = profilingToolId;
                editProfilingInstance.Captured_By_UserId = capturedByUserId;
                editProfilingInstance.HHID = generatedHHID;
                editProfilingInstance.NISIS_Site_EA_Id = siteEAId;
                editProfilingInstance.Dwelling_Unit_Number = dwellingUnitNumber;
                editProfilingInstance.Household_Number = householdNumber;
                editProfilingInstance.Household_Number_Of_Males = householdNumberOfMales;
                editProfilingInstance.Household_Number_Of_Females = householdNumberOfFemales;
                editProfilingInstance.Dwelling_Unit_Address = dwellingUnitAddress;
                editProfilingInstance.Dwelling_Unit_Description = dwellingUnitDescription;
                editProfilingInstance.Modified_By = modifiedBy;
                editProfilingInstance.Date_Last_Modified = dateLastModified;
                editProfilingInstance.Is_Active = isActive;
                editProfilingInstance.Is_Deleted = isDeleted;

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return editProfilingInstance;
        }

        public Profiling_Instance SetProfilingInstanceQAStatus(int profilingInstanceId, int qaStatusId)
        {
            Profiling_Instance editProfilingInstance;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editProfilingInstance = (from x in dbContext.Profiling_Instances
                                         where x.Profiling_Instance_Id.Equals(profilingInstanceId)
                                         select x).FirstOrDefault();

                if (editProfilingInstance == null) return null;

                editProfilingInstance.QA_Status_Item_Id = qaStatusId;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editProfilingInstance;
        }
    }
}
