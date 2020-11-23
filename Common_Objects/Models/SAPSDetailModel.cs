using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class SAPSDetailModel
    {
        public CPR_SAPS_Detail GetSpecificCPRSAPSDetail(int cprSAPSDetailId)
        {
            CPR_SAPS_Detail cprSAPSDetail;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var cprSAPSDetailList = (from r in dbContext.CPR_SAPS_Detail_Items
                                         where r.SAPS_Detail_Id.Equals(cprSAPSDetailId)
                                         select r).ToList();

                cprSAPSDetail = (from r in cprSAPSDetailList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return cprSAPSDetail;
        }

        public List<CPR_SAPS_Detail> GetListOfCPRSAPSDetailItems()
        {
            List<CPR_SAPS_Detail> cprSAPSDetailItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var cprSAPSDetailList = (from r in dbContext.CPR_SAPS_Detail_Items
                                         select r).ToList();

                cprSAPSDetailItems = (from r in cprSAPSDetailList
                                      select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return cprSAPSDetailItems;
        }

        public CPR_SAPS_Detail CreateCPRSAPSDetail(int incidentId, bool isReportedToPolice, DateTime? dateCaseReported, string caseNumber, bool isPoliceInvervention, 
            int? reportedPoliceStationId, int? investigatingOfficer, string comments)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var cprSAPSDetail = new CPR_SAPS_Detail() { Incident_Id = incidentId, Is_Reported_To_SAPS = isReportedToPolice, Date_Case_Reported = dateCaseReported, Case_Number = caseNumber, Is_Police_Intervention = isPoliceInvervention, Reported_Police_Station_Id = reportedPoliceStationId, Investigating_Officer_Id = investigatingOfficer, Comments = comments };

            try
            {
                var newCPRSAPSDetail = dbContext.CPR_SAPS_Detail_Items.Add(cprSAPSDetail);

                dbContext.SaveChanges();

                return newCPRSAPSDetail;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CPR_SAPS_Detail EditCPRSAPSDetail(int cprSAPSDetailId, int incidentId, bool isReportedToPolice, DateTime? dateCaseReported, string caseNumber, bool isPoliceInvervention, 
            int? reportedPoliceStationId, int? investigatingOfficer, string comments)
        {
            CPR_SAPS_Detail editCPRSAPSDetail;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editCPRSAPSDetail = (from a in dbContext.CPR_SAPS_Detail_Items
                                         where a.SAPS_Detail_Id.Equals(cprSAPSDetailId)
                                         select a).FirstOrDefault();

                    if (editCPRSAPSDetail == null) return null;

                    if ((reportedPoliceStationId != null) && (editCPRSAPSDetail.Reported_Police_Station_Id != null) && (editCPRSAPSDetail.Reported_Police_Station_Id != reportedPoliceStationId))
                    {
                        AddSAPSReportedStationHistory(editCPRSAPSDetail.SAPS_Detail_Id, DateTime.Now, (int)editCPRSAPSDetail.Reported_Police_Station_Id);
                    }
                    if ((investigatingOfficer != null) && (editCPRSAPSDetail.Investigating_Officer_Id != null) && (editCPRSAPSDetail.Investigating_Officer_Id != (int)investigatingOfficer))
                    {
                        AddSAPSOfficialHistory(editCPRSAPSDetail.SAPS_Detail_Id, DateTime.Now, (int)editCPRSAPSDetail.Investigating_Officer_Id);
                    }

                    editCPRSAPSDetail.Incident_Id = incidentId;
                    editCPRSAPSDetail.Is_Reported_To_SAPS = isReportedToPolice;
                    editCPRSAPSDetail.Date_Case_Reported = dateCaseReported;
                    editCPRSAPSDetail.Case_Number = caseNumber;
                    editCPRSAPSDetail.Is_Police_Intervention = isPoliceInvervention;
                    editCPRSAPSDetail.Reported_Police_Station_Id = reportedPoliceStationId;
                    editCPRSAPSDetail.Investigating_Officer_Id = investigatingOfficer;
                    editCPRSAPSDetail.Comments = comments;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editCPRSAPSDetail;
        }

        public CPR_SAPS_Detail AddSAPSOfficialHistory(int cprSAPSDetailId, DateTime itemDate, int investigatingOfficerId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var cprSAPSDetail = dbContext.CPR_SAPS_Detail_Items.FirstOrDefault(x => x.SAPS_Detail_Id.Equals(cprSAPSDetailId));

            if (cprSAPSDetail != null)
            {
                var historyItem = new CPR_SAPS_Investigating_Officer_History() { SAPS_Detail_Id = cprSAPSDetail.SAPS_Detail_Id, Item_Date = itemDate, Investigating_Officer_Id = investigatingOfficerId };

                try
                {
                    var newHistoryItem = dbContext.CPR_SAPS_Investigating_Officer_History.Add(historyItem);

                    dbContext.SaveChanges();

                    return cprSAPSDetail;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }

        public List<CPR_SAPS_Investigating_Officer_History> GetSAPSOfficialHistory(int cprSAPSDetailId)
        {
            List<CPR_SAPS_Investigating_Officer_History> historyItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var historyList = (from x in dbContext.CPR_SAPS_Investigating_Officer_History
                                   where x.SAPS_Detail_Id.Equals(cprSAPSDetailId)
                                   select x).ToList();

                historyItems = (from x in historyList
                                select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return historyItems;
        }

        public CPR_SAPS_Detail AddSAPSReportedStationHistory(int cprSAPSDetailId, DateTime itemDate, int reportedPoliceStationId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var cprSAPSDetail = dbContext.CPR_SAPS_Detail_Items.FirstOrDefault(x => x.SAPS_Detail_Id.Equals(cprSAPSDetailId));

            if (cprSAPSDetail != null)
            {
                var historyItem = new CPR_SAPS_Reported_Police_Station_History() { SAPS_Detail_Id = cprSAPSDetail.SAPS_Detail_Id, Item_Date = itemDate, Reported_Police_Station_Id = reportedPoliceStationId };

                try
                {
                    var newHistoryItem = dbContext.CPR_SAPS_Reported_Police_Station_History.Add(historyItem);

                    dbContext.SaveChanges();

                    return cprSAPSDetail;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }

        public List<CPR_SAPS_Reported_Police_Station_History> GetReportedPoliceStationHistory(int cprSAPSDetailId)
        {
            List<CPR_SAPS_Reported_Police_Station_History> historyItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var historyList = (from x in dbContext.CPR_SAPS_Reported_Police_Station_History
                                   where x.SAPS_Detail_Id.Equals(cprSAPSDetailId)
                                   select x).ToList();

                historyItems = (from x in historyList
                                select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return historyItems;
        }
    }
}
