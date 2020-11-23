using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisSiteModel
    {
        public NISIS_Site GetSpecificNisisSite(int nisisSiteId)
        {
            NISIS_Site nisisSite;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteList = (from r in dbContext.NISIS_Sites
                                     where r.Site_Id.Equals(nisisSiteId)
                                     select r).ToList();

                nisisSite = (from r in nisisSiteList
                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisSite;
        }

        public NISIS_Site GetSpecificNisisSite(string nisisSiteName)
        {
            NISIS_Site nisisSite;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteList = (from r in dbContext.NISIS_Sites
                                     where r.Site_Name.Equals(nisisSiteName)
                                     select r).ToList();

                nisisSite = (from r in nisisSiteList
                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisSite;
        }

        public List<NISIS_Site> GetListOfNisisSites(bool showInActive, bool showDeleted)
        {
            List<NISIS_Site> nisisSites;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteList = (from x in dbContext.NISIS_Sites
                                     where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                     where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                     select x).ToList();

                nisisSites = (from x in nisisSiteList
                              select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisSites;
        }

        public List<NISIS_Site> GetListOfNisisSites(bool showInActive, bool showDeleted, int nisisWardId)
        {
            List<NISIS_Site> nisisSites;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteList = (from x in dbContext.NISIS_Sites
                                     where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                     where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                     where x.NISIS_Ward_Id.Equals(nisisWardId)
                                     select x).ToList();

                nisisSites = (from x in nisisSiteList
                              select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisSites;
        }

        public NISIS_Site CreateSite(int wardId, string siteName, string siteAlternativeName, string gpsCoordinatesLat, string gpsCoordinatesLong, int? registeredProgrammeId, int? registeredProgrammeStatusId, string responsibleOrganization,
            string prioritizationGroup, DateTime? targetStartDate, DateTime? targetEndDate, string primaryContact, int? listingMethodId, int? responsibleProgrammeId, int? estimatedPopuplation,
            string sourceOfPopulationEstimate, decimal? budgetCommitted, string createdBy, DateTime createdDate, bool isActive, bool isDeleted)
        {

            NISIS_Site newSite;

            var dbContext = new SDIIS_DatabaseEntities();

            var site = new NISIS_Site()
            {
                NISIS_Ward_Id = wardId,
                Site_Name = siteName,
                Site_Alternative_Name = siteAlternativeName,
                GPS_Coordinates_Lat = gpsCoordinatesLat,
                GPS_Coordinates_Long = gpsCoordinatesLong,
                Registered_Programme_Id = registeredProgrammeId,
                Registered_Programme_Status_Id = registeredProgrammeStatusId,
                Responsible_Organization = responsibleOrganization,
                Prioritization_Group = prioritizationGroup,
                Target_Start_Date = targetStartDate,
                Target_End_Date = targetEndDate,
                Primary_Contact = primaryContact,
                Listing_Method_Id = listingMethodId,
                Responsible_Programme_Id = responsibleProgrammeId,
                Estimated_Population = estimatedPopuplation,
                Source_of_Population_Estimate = sourceOfPopulationEstimate,
                Budget_Committed = budgetCommitted,
                QA_Status_Item_Id = (int)QAStatusEnum.NotSet,
                Created_By = createdBy,
                Created_Date = createdDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted
            };

            try
            {
                newSite = dbContext.NISIS_Sites.Add(site);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newSite;
        }

        public NISIS_Site EditSite(int siteId, int wardId, string siteName, string siteAlternativeName, string gpsCoordinatesLat, string gpsCoordinatesLong, int? registeredProgrammeId, int? registeredProgrammeStatusId, string responsibleOrganization,
            string prioritizationGroup, DateTime? targetStartDate, DateTime? targetEndDate, string primaryContact, int? listingMethodId, int? responsibleProgrammeId, int? estimatedPopuplation,
            string sourceOfPopulationEstimate, decimal? budgetCommitted, string modifiedBy, DateTime dateLastModified, bool isActive, bool isDeleted)
        {
            NISIS_Site editSite;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editSite = (from x in dbContext.NISIS_Sites
                            where x.Site_Id.Equals(siteId)
                            select x).FirstOrDefault();

                if (editSite == null) return null;

                editSite.NISIS_Ward_Id = wardId;
                editSite.Site_Name = siteName;
                editSite.Site_Alternative_Name = siteAlternativeName;
                editSite.GPS_Coordinates_Lat = gpsCoordinatesLat;
                editSite.GPS_Coordinates_Long = gpsCoordinatesLong;
                editSite.Registered_Programme_Id = registeredProgrammeId;
                editSite.Registered_Programme_Status_Id = registeredProgrammeStatusId;
                editSite.Responsible_Organization = responsibleOrganization;
                editSite.Prioritization_Group = prioritizationGroup;
                editSite.Target_Start_Date = targetStartDate;
                editSite.Target_End_Date = targetEndDate;
                editSite.Primary_Contact = primaryContact;
                editSite.Listing_Method_Id = listingMethodId;
                editSite.Responsible_Programme_Id = responsibleProgrammeId;
                editSite.Estimated_Population = estimatedPopuplation;
                editSite.Source_of_Population_Estimate = sourceOfPopulationEstimate;
                editSite.Budget_Committed = budgetCommitted;
                editSite.Modified_By = modifiedBy;
                editSite.Modified_Date = dateLastModified;
                editSite.Is_Active = isActive;
                editSite.Is_Deleted = isDeleted;

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return editSite;
        }

        public NISIS_Site AddRegisteredProgrammesToSite(int siteId, List<int> registeredProgrammeIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var siteToEdit = dbContext.NISIS_Sites.FirstOrDefault(x => x.Site_Id.Equals(siteId));
                if (siteToEdit == null) return null;

                siteToEdit.Registered_Programmes.Clear();

                foreach (var rpId in registeredProgrammeIds)
                {
                    var rpToAdd = dbContext.NISIS_Programmes.FirstOrDefault(x => x.NISIS_Programme_Id.Equals(rpId));
                    if (rpToAdd == null) return null;

                    siteToEdit.Registered_Programmes.Add(rpToAdd);
                }

                dbContext.SaveChanges();

                return siteToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public NISIS_Site AddProfilingMethodsToSite(int siteId, List<int> profilingMethodIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var siteToEdit = dbContext.NISIS_Sites.FirstOrDefault(x => x.Site_Id.Equals(siteId));
                if (siteToEdit == null) return null;

                siteToEdit.NISIS_Profiling_Methods.Clear();

                foreach (var pmId in profilingMethodIds)
                {
                    var pmToAdd = dbContext.NISIS_Profiling_Method_Items.FirstOrDefault(x => x.NISIS_Profiling_Method_Id.Equals(pmId));
                    if (pmToAdd == null) return null;

                    siteToEdit.NISIS_Profiling_Methods.Add(pmToAdd);
                }

                dbContext.SaveChanges();

                return siteToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public NISIS_Site AddProfilingToolsToSite(int siteId, List<int> profilingToolIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var siteToEdit = dbContext.NISIS_Sites.FirstOrDefault(x => x.Site_Id.Equals(siteId));
                if (siteToEdit == null) return null;

                siteToEdit.NISIS_Profiling_Tools.Clear();

                foreach (var ptId in profilingToolIds)
                {
                    var ptToAdd = dbContext.Profiling_Tools.FirstOrDefault(x => x.Profiling_Tool_Id.Equals(ptId));
                    if (ptToAdd == null) return null;

                    siteToEdit.NISIS_Profiling_Tools.Add(ptToAdd);
                }

                dbContext.SaveChanges();

                return siteToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public NISIS_Site AddGroupingFlagsToSite(int siteId, List<int> groupingFlagIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var siteToEdit = dbContext.NISIS_Sites.FirstOrDefault(x => x.Site_Id.Equals(siteId));
                if (siteToEdit == null) return null;

                siteToEdit.NISIS_Grouping_Flags.Clear();

                foreach (var gfId in groupingFlagIds)
                {
                    var gfToAdd = dbContext.NISIS_Grouping_Flag_Items.FirstOrDefault(x => x.NISIS_Grouping_Flag_Id.Equals(gfId));
                    if (gfToAdd == null) return null;

                    siteToEdit.NISIS_Grouping_Flags.Add(gfToAdd);
                }

                dbContext.SaveChanges();

                return siteToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public NISIS_Site SetSiteQAStatus(int siteId, int qaStatusId)
        {
            NISIS_Site editSite;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSite = (from x in dbContext.NISIS_Sites
                                where x.Site_Id.Equals(siteId)
                                select x).FirstOrDefault();

                    if (editSite == null) return null;

                    editSite.QA_Status_Item_Id = qaStatusId;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSite;
        }

        public NISIS_Site SetSiteIsActive(int siteId, bool isActive)
        {
            NISIS_Site editSite;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSite = (from x in dbContext.NISIS_Sites
                                where x.Site_Id.Equals(siteId)
                                select x).FirstOrDefault();

                    if (editSite == null) return null;

                    editSite.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSite;
        }

        public NISIS_Site SetSiteIsDeleted(int siteId, bool isDeleted)
        {
            NISIS_Site editSite;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSite = (from x in dbContext.NISIS_Sites
                                where x.Site_Id.Equals(siteId)
                                select x).FirstOrDefault();

                    if (editSite == null) return null;

                    editSite.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSite;
        }
    }
}
