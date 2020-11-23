using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CommunityProfilingInstanceModel
    {
        public Community_Profiling_Instance GetSpecificCommunityProfilingInstance(int communityProfilingInstanceId)
        {
            Community_Profiling_Instance communityProfilingInstances;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var communityProfilingInstanceList = (from x in dbContext.Community_Profiling_Instances
                                                      where x.Community_Profiling_Instance_Id.Equals(communityProfilingInstanceId)
                                                      select x).ToList();

                communityProfilingInstances = (from r in communityProfilingInstanceList
                                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return communityProfilingInstances;
        }

        public List<Community_Profiling_Instance> GetListOfProfilingInstances(bool showInActive, bool showDeleted)
        {
            List<Community_Profiling_Instance> communityProfilingInstances;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var communityProfilingInstancesList = (from x in dbContext.Community_Profiling_Instances
                                                       where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                       where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                       select x).ToList();

                communityProfilingInstances = (from x in communityProfilingInstancesList
                                               select x).ToList();
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return communityProfilingInstances;
        }

        public List<Community_Profiling_Instance> GetListOfProfilingInstances(bool showInActive, bool showDeleted, int capturedByUserId)
        {
            List<Community_Profiling_Instance> communityProfilingInstances;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var communityProfilingInstancesList = (from x in dbContext.Community_Profiling_Instances
                                                       where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                       where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                       where x.Captured_By_UserId.Equals(capturedByUserId)
                                                       select x).ToList();

                communityProfilingInstances = (from x in communityProfilingInstancesList
                                               select x).ToList();
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return communityProfilingInstances;
        }

        public List<Community_Profiling_Instance> GetListOfProfilingInstances(int provinceId)
        {
            List<Community_Profiling_Instance> communityProfilingInstances;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var communityProfilingInstancesList = (from x in dbContext.Community_Profiling_Instances
                                                       where x.Is_Active.Equals(true)
                                                       where x.Is_Deleted.Equals(false)
                                                       where x.NISIS_Site_EA.NISIS_Site.Ward.Local_Municipality.District.Province_Id.Equals(provinceId)
                                                       select x).ToList();

                communityProfilingInstances = (from x in communityProfilingInstancesList
                                               select x).ToList();
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return communityProfilingInstances;
        }

        public Community_Profiling_Instance CreateCommunityProfilingInstance(DateTime profilingDate, int profilingToolId, int capturedByUserId, int siteEAId, string createdBy, DateTime createdDate, bool isActive, bool isDeleted)
        {
            Community_Profiling_Instance newCommunityProfilingInstance;

            var dbContext = new SDIIS_DatabaseEntities();

            var communityProfilingInstance = new Community_Profiling_Instance()
            {
                Profiling_Date = profilingDate,
                Profiling_Tool_Id = profilingToolId,
                Captured_By_UserId = capturedByUserId,
                NISIS_Site_EA_Id = siteEAId,
                QA_Status_Item_Id = (int)QAStatusEnum.NotSet,
                Created_By = createdBy,
                Date_Created = createdDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted
            };

            try
            {
                newCommunityProfilingInstance = dbContext.Community_Profiling_Instances.Add(communityProfilingInstance);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newCommunityProfilingInstance;
        }

        public Community_Profiling_Instance EditCommunityProfilingInstance(int profilingInstanceId, DateTime profilingDate, int profilingToolId, int capturedByUserId, int siteEAId, string modifiedBy, DateTime dateLastModified, bool isActive, bool isDeleted)
        {
            Community_Profiling_Instance editCommunityProfilingInstance;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editCommunityProfilingInstance = (from x in dbContext.Community_Profiling_Instances
                                                  where x.Community_Profiling_Instance_Id.Equals(profilingInstanceId)
                                                  select x).FirstOrDefault();

                if (editCommunityProfilingInstance == null) return null;

                editCommunityProfilingInstance.Profiling_Date = profilingDate;
                editCommunityProfilingInstance.Profiling_Tool_Id = profilingToolId;
                editCommunityProfilingInstance.Captured_By_UserId = capturedByUserId;
                editCommunityProfilingInstance.NISIS_Site_EA_Id = siteEAId;
                editCommunityProfilingInstance.Modified_By = modifiedBy;
                editCommunityProfilingInstance.Date_Last_Modified = dateLastModified;
                editCommunityProfilingInstance.Is_Active = isActive;
                editCommunityProfilingInstance.Is_Deleted = isDeleted;

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return null;
            }

            return editCommunityProfilingInstance;
        }

        public Community_Profiling_Instance SetCommunityProfilingInstanceQAStatus(int communityProfilingInstanceId, int qaStatusId)
        {
            Community_Profiling_Instance editCommunityProfilingInstance;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editCommunityProfilingInstance = (from x in dbContext.Community_Profiling_Instances
                                                  where x.Community_Profiling_Instance_Id.Equals(communityProfilingInstanceId)
                                                  select x).FirstOrDefault();

                if (editCommunityProfilingInstance == null) return null;

                editCommunityProfilingInstance.QA_Status_Item_Id = qaStatusId;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editCommunityProfilingInstance;
        }
    }
}
