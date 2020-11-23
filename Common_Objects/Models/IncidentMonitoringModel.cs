using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class IncidentMonitoringModel
    {
        public Incident_Monitoring_Item GetSpecificIncidentMonitoringItem(int incidentMonitoringItemId)
        {
            Incident_Monitoring_Item incidentMonitoringItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentMonitoringItemList = (from r in dbContext.Incident_Monitoring_Items
                                                  where r.Incident_Monitoring_Id.Equals(incidentMonitoringItemId)
                                                  select r).ToList();

                incidentMonitoringItem = (from r in incidentMonitoringItemList
                                          select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return incidentMonitoringItem;
        }

        public List<Incident_Monitoring_Item> GetListOfIncidentMonitoringItems()
        {
            List<Incident_Monitoring_Item> incidentMonitoringItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentMonitoringItemList = (from r in dbContext.Incident_Monitoring_Items
                                                  select r).ToList();

                incidentMonitoringItems = (from r in incidentMonitoringItemList
                                           select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return incidentMonitoringItems;
        }

        public Incident_Monitoring_Item CreateIncidentMonitoringItem(int incidentId, bool isNormalMonitoringActive, bool isForm36MonitoringActive, bool isChildrensCourtMonitoringActive, bool isCriminalCourtMonitoringActive, 
            DateTime? normal21Date, DateTime? normal30Date, DateTime? normal60Date, DateTime? form3648HoursDate, DateTime? form36WeeklyDate, DateTime? form36MonthlyDate, 
            DateTime? childrensCourt3MonthsDate, DateTime? childrensCourt6MonthsDate, DateTime? criminalCourt1YearDate, DateTime? criminalCourt2YearDate, 
            bool isNormalMonitoringSwitchedOff, bool isForm36MonitoringSwitchedOff, bool isChildrensCourtMonitoringSwitchedOff, bool isCriminalCourtMonitoringSwitchedOff,
            DateTime? normalMonitoringOffDate, DateTime? form36MonitoringOffDate, DateTime? childrensCourtMonitoringOffDate, DateTime? criminalCourtMonitoringOffDate,
            string normalMonitoringOffReason, string form36MonitoringOffReason, string childrensCourtMonitoringOffReason, string criminalCourtMonitoringOffReason)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var incidentMonitoringItem = new Incident_Monitoring_Item()
            {
                Incident_Id = incidentId,
                Is_Normal_Monitoring_Active = isNormalMonitoringActive,
                Is_Form36_Monitoring_Active = isForm36MonitoringActive,
                Is_Childrens_Court_Monitoring_Active = isChildrensCourtMonitoringActive,
                Is_Criminal_Court_Monitoring_Active = isCriminalCourtMonitoringActive,
                Normal_21Days_Date = normal21Date, Normal_30Days_Date = normal30Date, Normal_60Days_Date = normal60Date,
                Form36_48Hours_Date = form3648HoursDate, Form36_Weekly_Date = form36WeeklyDate, Form36_Monthly_Date = form36MonthlyDate,
                Childrens_Court_3Months_Date = childrensCourt3MonthsDate, Childrens_Court_6Months_Date = childrensCourt6MonthsDate,
                Criminal_Court_1Year_Date = criminalCourt1YearDate, Criminal_Court_2Year_Date = criminalCourt2YearDate,
                Is_Normal_Monitoring_Switched_Off = isNormalMonitoringSwitchedOff,
                Is_Form36_Monitoring_Switched_Off = isForm36MonitoringSwitchedOff,
                Is_Childrens_Court_Monitoring_Switched_Off = isChildrensCourtMonitoringSwitchedOff,
                Is_Criminal_Court_Monitoring_Switched_Off = isCriminalCourtMonitoringSwitchedOff,
                Normal_Monitoring_Off_Date = normalMonitoringOffDate,
                Form36_Monitoring_Off_Date = form36MonitoringOffDate,
                Childrens_Court_Monitoring_Off_Date = childrensCourtMonitoringOffDate,
                Criminal_Court_Monitoring_Off_Date = criminalCourtMonitoringOffDate,
                Normal_Monitoring_Off_Reason = normalMonitoringOffReason,
                Form36_Monitoring_Off_Reason = form36MonitoringOffReason,
                Childrens_Court_Monitoring_Off_Reason = childrensCourtMonitoringOffReason,
                Criminal_Court_Monitoring_Off_Reason = criminalCourtMonitoringOffReason
            };

            try
            {
                var newIncidentMonitoringItem = dbContext.Incident_Monitoring_Items.Add(incidentMonitoringItem);

                dbContext.SaveChanges();

                return newIncidentMonitoringItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Incident_Monitoring_Item EditIncidentMonitoringItem(int incidentMonitoringItemId, int incidentId, bool isNormalMonitoringActive, bool isForm36MonitoringActive, bool isChildrensCourtMonitoringActive, bool isCriminalCourtMonitoringActive,
            DateTime? normal21Date, DateTime? normal30Date, DateTime? normal60Date, DateTime? form3648HoursDate, DateTime? form36WeeklyDate, DateTime? form36MonthlyDate,
            DateTime? childrensCourt3MonthsDate, DateTime? childrensCourt6MonthsDate, DateTime? criminalCourt1YearDate, DateTime? criminalCourt2YearDate,
            bool isNormalMonitoringSwitchedOff, bool isForm36MonitoringSwitchedOff, bool isChildrensCourtMonitoringSwitchedOff, bool isCriminalCourtMonitoringSwitchedOff,
            DateTime? normalMonitoringOffDate, DateTime? form36MonitoringOffDate, DateTime? childrensCourtMonitoringOffDate, DateTime? criminalCourtMonitoringOffDate,
            string normalMonitoringOffReason, string form36MonitoringOffReason, string childrensCourtMonitoringOffReason, string criminalCourtMonitoringOffReason)
        {
            Incident_Monitoring_Item editIncidentMonitoringItem;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editIncidentMonitoringItem = (from a in dbContext.Incident_Monitoring_Items
                                                  where a.Incident_Monitoring_Id.Equals(incidentMonitoringItemId)
                                                  select a).FirstOrDefault();

                    if (editIncidentMonitoringItem == null) return null;

                    editIncidentMonitoringItem.Incident_Id = incidentId;
                    editIncidentMonitoringItem.Is_Normal_Monitoring_Active = isNormalMonitoringActive;
                    editIncidentMonitoringItem.Is_Form36_Monitoring_Active = isForm36MonitoringActive;
                    editIncidentMonitoringItem.Is_Childrens_Court_Monitoring_Active = isChildrensCourtMonitoringActive;
                    editIncidentMonitoringItem.Is_Criminal_Court_Monitoring_Active = isCriminalCourtMonitoringActive;
                    editIncidentMonitoringItem.Normal_21Days_Date = normal21Date;
                    editIncidentMonitoringItem.Normal_30Days_Date = normal30Date;
                    editIncidentMonitoringItem.Normal_60Days_Date = normal60Date;
                    editIncidentMonitoringItem.Form36_48Hours_Date = form3648HoursDate;
                    editIncidentMonitoringItem.Form36_Monthly_Date = form36WeeklyDate;
                    editIncidentMonitoringItem.Form36_Monthly_Date = form36MonthlyDate;
                    editIncidentMonitoringItem.Childrens_Court_3Months_Date = childrensCourt3MonthsDate;
                    editIncidentMonitoringItem.Childrens_Court_6Months_Date = childrensCourt6MonthsDate;
                    editIncidentMonitoringItem.Criminal_Court_1Year_Date = criminalCourt1YearDate;
                    editIncidentMonitoringItem.Criminal_Court_2Year_Date = criminalCourt2YearDate;
                    editIncidentMonitoringItem.Is_Normal_Monitoring_Switched_Off = isNormalMonitoringSwitchedOff;
                    editIncidentMonitoringItem.Is_Form36_Monitoring_Switched_Off = isForm36MonitoringSwitchedOff;
                    editIncidentMonitoringItem.Is_Childrens_Court_Monitoring_Switched_Off = isChildrensCourtMonitoringSwitchedOff;
                    editIncidentMonitoringItem.Is_Criminal_Court_Monitoring_Switched_Off = isCriminalCourtMonitoringSwitchedOff;
                    editIncidentMonitoringItem.Normal_Monitoring_Off_Date = normalMonitoringOffDate;
                    editIncidentMonitoringItem.Form36_Monitoring_Off_Date = form36MonitoringOffDate;
                    editIncidentMonitoringItem.Childrens_Court_Monitoring_Off_Date = childrensCourtMonitoringOffDate;
                    editIncidentMonitoringItem.Criminal_Court_Monitoring_Off_Date = criminalCourtMonitoringOffDate;
                    editIncidentMonitoringItem.Normal_Monitoring_Off_Reason = normalMonitoringOffReason;
                    editIncidentMonitoringItem.Form36_Monitoring_Off_Reason = form36MonitoringOffReason;
                    editIncidentMonitoringItem.Childrens_Court_Monitoring_Off_Reason = childrensCourtMonitoringOffReason;
                    editIncidentMonitoringItem.Criminal_Court_Monitoring_Off_Reason = criminalCourtMonitoringOffReason;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editIncidentMonitoringItem;
        }
    }
}
