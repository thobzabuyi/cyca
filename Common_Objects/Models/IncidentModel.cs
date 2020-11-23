using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class IncidentModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public CPR_Incident GetSpecificIncident(int incidentId)
        {
            CPR_Incident incident;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var incidentList = (from i in dbContext.CPR_Incidents
                                    where i.Incident_Id.Equals(incidentId)
                                    select i).ToList();

                incident = (from i in incidentList
                            select i).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return incident;
        }

        public List<CPR_Incident> GetListOfIncidents(bool showInActive, bool showDeleted)
        {
            List<CPR_Incident> incidents;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentList = (from i in dbContext.CPR_Incidents
                                    where i.Is_Active.Equals(true) || i.Is_Active.Equals(!showInActive)
                                    where i.Is_Deleted.Equals(false) || i.Is_Deleted.Equals(showDeleted)
                                    select i).ToList();

                incidents = (from i in incidentList
                             select i).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return incidents;
        }

        public List<CPR_Incident> GetListOfIncidents(bool showInActive, bool showDeleted, Alleged_Offender allegedOffender)
        {
            List<CPR_Incident> incidents;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentList = (from i in dbContext.CPR_Incidents
                                    where i.Is_Active.Equals(true) || i.Is_Active.Equals(!showInActive)
                                    where i.Is_Deleted.Equals(false) || i.Is_Deleted.Equals(showDeleted)
                                    where i.Alleged_Offenders.Contains(allegedOffender)
                                    select i).ToList();

                incidents = (from i in incidentList
                             select i).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return incidents;
        }

        public List<CPR_Incident> GetListOfIncidents(bool showInActive, bool showDeleted, bool showAllocated, bool showUnallocated)
        {
            List<CPR_Incident> incidents;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentList = (from i in dbContext.CPR_Incidents
                                    where i.Is_Active.Equals(true) || i.Is_Active.Equals(!showInActive)
                                    where i.Is_Deleted.Equals(false) || i.Is_Deleted.Equals(showDeleted)
                                    select i);

                if (showAllocated)
                    incidentList = incidentList.Where(x => x.Assigned_Social_Worker_Id != null);
                if (showUnallocated)
                    incidentList = incidentList.Where(x => x.Assigned_Social_Worker_Id == null);

                incidents = (from i in incidentList
                             select i).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return incidents;
        }

        public List<CPR_Incident> GetListOfIncidents(bool showInactive, bool showDeleted, int allocatedToCaseWorkerId)
        {
            List<CPR_Incident> incidents;

            var dbContext = new SDIIS_DatabaseEntities();
            var userIdForSocialWorker = dbContext.Social_Workers.Where(x => x.Social_Worker_Id == allocatedToCaseWorkerId).Select(y => y.User_Id).FirstOrDefault();
            try
            {
                var incidentList = (from i in dbContext.CPR_Incidents
                                    where i.Assigned_Social_Worker_Id == userIdForSocialWorker
                                    select i).ToList();

                incidents = (from i in incidentList
                             select i).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return incidents;
        }

        public CPR_Incident CreateIncident(int assessmentId, DateTime? incidentDate, DateTime? notificationDate, int? incidentLocationId, int? incidentAddressId, int? incidentDistrictId,
            bool isAbuseConfirmed, DateTime? dateAbuseConfirmed, int? riskIndicatorId, string abuseCircumstances, bool isMultipleAbuse,
            bool isCaseClosed, int? caseClosureReasonId, string caseClosureReasonOther, string caseClosureMotivation, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            CPR_Incident newIncident;

            var dbContext = new SDIIS_DatabaseEntities();

            var incident = new CPR_Incident()
            {
                Intake_Assessment_Id = assessmentId,
                Incident_Status_Id = (int)IncidentStatusEnum.New,
                Incident_Date = incidentDate,
                Notification_Date = notificationDate,
                Incident_Location_Id = incidentLocationId,
                Incident_Location_Address_Id = incidentAddressId,
                Incident_District_Id = incidentDistrictId,
                Is_Abuse_Confirmed = isAbuseConfirmed,
                Abuse_Confirmed_Date = dateAbuseConfirmed,
                Risk_Indicator_Id = riskIndicatorId,
                Abuse_Circumstances = abuseCircumstances,
                Is_Multiple_Abuse = isMultipleAbuse,
                Is_Case_Closed = isCaseClosed,
                Case_Closure_Reason_Id = caseClosureReasonId,
                Case_Closure_Reason_Other = caseClosureReasonOther,
                Case_Closure_Motivation = caseClosureMotivation,
                Date_Created = dateCreated,
                Created_By = createdBy,
                Is_Active = true,
                Is_Deleted = isDeleted
            };

            try
            {
                newIncident = dbContext.CPR_Incidents.Add(incident);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newIncident;
        }

        public CPR_Incident EditIncident(int incidentId, DateTime? incidentDate, DateTime? notificationDate, int? incidentLocationId, int? incidentAddressId, int? incidentDistrictId,
            bool isAbuseConfirmed, DateTime? dateAbuseConfirmed, int? riskIndicatorId, string abuseCircumstances, bool isMultipleAbuse,
            bool isCaseClosed, int? caseClosureReasonId, string caseClosureReasonOther, string caseClosureMotivation, DateTime? dateLastModified, string modifiedBy, bool isActive, bool isDeleted, int assignedSocialWorker_Id)
        {
            var dbContext = new SDIIS_DatabaseEntities();
             
            try
            {
                var editIncident = (from i in dbContext.CPR_Incidents
                                    where i.Incident_Id.Equals(incidentId)
                                    select i).FirstOrDefault();

                if (editIncident == null) return null;

                editIncident.Incident_Date = incidentDate;
                editIncident.Notification_Date = notificationDate;
                editIncident.Incident_Location_Id = incidentLocationId;
                editIncident.Incident_Location_Address_Id = incidentAddressId;
                editIncident.Incident_District_Id = incidentDistrictId;
                editIncident.Is_Abuse_Confirmed = isAbuseConfirmed;
                editIncident.Abuse_Confirmed_Date = dateAbuseConfirmed;
                editIncident.Risk_Indicator_Id = riskIndicatorId;
                editIncident.Abuse_Circumstances = abuseCircumstances;
                editIncident.Is_Multiple_Abuse = isMultipleAbuse;
                editIncident.Is_Case_Closed = isCaseClosed;
                editIncident.Case_Closure_Reason_Id = caseClosureReasonId;
                editIncident.Case_Closure_Reason_Other = caseClosureReasonOther;
                editIncident.Case_Closure_Motivation = caseClosureMotivation;
                editIncident.Date_Last_Modified = dateLastModified;
                editIncident.Modified_By = modifiedBy;
                editIncident.Is_Active = isActive;
                editIncident.Is_Deleted = isDeleted;
                editIncident.Assigned_Social_Worker_Id = assignedSocialWorker_Id;

                dbContext.SaveChanges();

                return editIncident;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        public CPR_Incident AssignReferenceNumber(int incidentId, string referenceNumber)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editIncident = (from i in dbContext.CPR_Incidents
                                    where i.Incident_Id.Equals(incidentId)
                                    select i).FirstOrDefault();

                if (editIncident == null) return null;

                editIncident.Reference_Number = referenceNumber;

                dbContext.SaveChanges();

                return editIncident;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public CPR_Incident AddPrimaryAbuseTypeToIncident(int incidentId, int abuseTypeId)
        public CPR_Incident AddPrimaryAbuseTypeToIncident(int incidentId, int abuseTypeId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentToEdit = dbContext.CPR_Incidents.FirstOrDefault(x => x.Incident_Id.Equals(incidentId));
                if (incidentToEdit == null) return null;

                //abuseTypeId =incidentToEdit.Incident_Abuse_Types.ToList().RemoveAll(p => p.Is_Primary_Abuse_Type == true);
                if (abuseTypeId != 0) { 
                    var newAbuseType = new Incident_Abuse_Type() { Incident_Id = incidentId, Abuse_Type_Id = abuseTypeId, Is_Primary_Abuse_Type = true };

                    incidentToEdit.Incident_Abuse_Types.Add(newAbuseType);
                }
                dbContext.SaveChanges();
                
                return incidentToEdit;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        public CPR_Incident AddSecondaryAbuseTypesToIncident(int incidentId, List<int> abuseTypeIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentToEdit = dbContext.CPR_Incidents.FirstOrDefault(x => x.Incident_Id.Equals(incidentId));
                if (incidentToEdit == null) return null;

                var abuseTypesToRemove = incidentToEdit.Incident_Abuse_Types.Where(x => !x.Is_Primary_Abuse_Type).ToList();

                dbContext.Incident_Abuse_Types.RemoveRange(abuseTypesToRemove);
                dbContext.SaveChanges();

                foreach (var abuseTypeId in abuseTypeIds)
                {
                    var newAbuseType = new Incident_Abuse_Type() { Incident_Id = incidentId, Abuse_Type_Id = abuseTypeId, Is_Primary_Abuse_Type = false };

                    dbContext.Incident_Abuse_Types.Add(newAbuseType);
                }

                dbContext.SaveChanges();

                return incidentToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CPR_Incident AddAbuseIndicatorsToIncident(int incidentId, List<int> abuseIndicatorIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var incidentToEdit = dbContext.CPR_Incidents.FirstOrDefault(x => x.Incident_Id.Equals(incidentId));
                if (incidentToEdit == null) return null;

                incidentToEdit.Abuse_Indicators.Clear();

                foreach (var abuseIndicatorId in abuseIndicatorIds)
                {
                    var indicatorToAdd = dbContext.Abuse_Indicators.FirstOrDefault(x => x.Abuse_Indicator_Id.Equals(abuseIndicatorId));
                    if (indicatorToAdd == null) return null;

                    incidentToEdit.Abuse_Indicators.Add(indicatorToAdd);
                }

                dbContext.SaveChanges();

                return incidentToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CPR_Incident AddAddress(int incidentId, int addressTypeId, string addressLine1, string addressLine2, int? townId, string postalCode)
        {
            CPR_Incident editIncident;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editIncident = (from i in dbContext.CPR_Incidents
                                    where i.Incident_Id.Equals(incidentId)
                                    select i).FirstOrDefault();

                    if (editIncident == null) return null;

                    editIncident.Addresses.Clear();
                    editIncident.Addresses.Add(new Address() { Address_Type_Id = addressTypeId, Address_Line_1 = addressLine1, Address_Line_2 = addressLine2, Town_Id = townId, Postal_Code = postalCode });

                    dbContext.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

            return editIncident;
        }

        public Address EditAddress(int addressId, int addressTypeId, string addressLine1, string addressLine2, int? townId, string postalCode)
        {
            Address editAddress;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                editAddress = (from a in dbContext.Addresses
                               where a.Address_Id.Equals(addressId)
                               select a).FirstOrDefault();

                if (editAddress == null) return null;

                editAddress.Address_Type_Id = addressTypeId;
                editAddress.Address_Line_1 = addressLine1;
                editAddress.Address_Line_2 = addressLine2;
                editAddress.Town_Id = townId;
                editAddress.Postal_Code = postalCode;

                dbContext.SaveChanges();
            }

            return editAddress;
        }

        #region Multiselect Addition

        public List<Abuse_Indicator> GetListOfAbuseIndicatorsForCPRI(int incident_ID)
        {
            List<Abuse_Indicator> abuseIndicators;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var abuseIndicatorList = dbContext.CPR_Incidents.Find(incident_ID).Abuse_Indicators;

                    abuseIndicators = (from r in abuseIndicatorList
                                       select r).ToList();
                }
                catch (Exception ex)
                {
                    var Test = ex.Message;
                    return null;
                }
            }

            return abuseIndicators;
        }

        public List<Abuse_Indicator> GetListOfAbuseIndicators()
        {
            List<Abuse_Indicator> abuseIndicators;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var abuseIndicatorList = (from r in dbContext.Abuse_Indicators
                                              select r).ToList();

                    abuseIndicators = (from r in abuseIndicatorList
                                       select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return abuseIndicators;
        }
        #endregion

        public CPR_Incident GetCPR_Incident(int assessmentId)
        {
            return db.CPR_Incidents.Where(x => x.Intake_Assessment_Id == assessmentId).FirstOrDefault();
        }

        public int GetLocal_Municipality_Id(int SelectedValue)
        {
            if (SelectedValue != 0)
            {
                return db.Towns.Find(SelectedValue).Local_Municipality_Id;
            }
            else return 0;
        }

        public int GetDistrict_Municipality_Id(int SelectedValue)
        {
            if (SelectedValue != 0) { 
            return db.Local_Municipalities.Find(db.Towns.Find(SelectedValue).Local_Municipality_Id).District_Municipality_Id;
            }
            else
            {
                return 0;
            }
        }
        public int GetDistrict_Municipality_Id_1(int SelectedValue)
        {
            if (SelectedValue != 0) { 
            int lom = db.Towns.Find(SelectedValue).Local_Municipality_Id;

            return db.Local_Municipalities.Find(lom).District_Municipality_Id;
            }
            else
            {
                return 0;
            }
        }

        public int GetProvince_Id(int SelectedValue)
        {
            if (SelectedValue != 0) { 
            return db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(SelectedValue).Local_Municipality_Id).District_Municipality_Id).Province_Id;
            }
            else
            {
                return 0;
            }
        }
        public ICollection<Abuse_Indicator> GetAbuse_Indicators(int Incident_Id)
        {
            return db.CPR_Incidents.Find(Incident_Id).Abuse_Indicators.ToList();
        }
        public string GetAbuse_IndicatorsDescription(int Abuse_Indicator_Id)
        {
            return db.Abuse_Indicators.Find(Abuse_Indicator_Id).Description;
        }
        public List<int> GetIncident_Abuse_Types(int Incident_Id)
        {
            return (from z in db.Incident_Abuse_Types
                    where z.Incident_Id == Incident_Id && z.Is_Primary_Abuse_Type == false
                    select z.Abuse_Type_Id).ToList();
        }
        public string GetAbuse_TypeDescription(int itemSecA)
        {
            return db.Abuse_Types.Find(itemSecA).Description;
        }
        public int GetClosureReasonId(string Reason)
        {
            if (Reason != null && Reason != "")
            {
                return (from a in db.Case_Closure_Reasons
                        where a.Description.Contains(Reason)
                        select a.Case_Closure_Reason_Id).FirstOrDefault();
            }
            else
                return 0;
        }

        public int? GetClosureReason_Id(string IncidentId)
        {
            if (IncidentId != null && IncidentId != "")
            {
                int newInId = Convert.ToInt32(IncidentId);
                return (from a in db.CPR_Incidents
                        where a.Incident_Id == newInId
                        select a.Case_Closure_Reason_Id).FirstOrDefault();
            }
            else return 0;
        }

        //public int GetDistrictId(int Id)
        //{
        //    if (Id != null)
        //    {
        //        return 
        //    }
        //}

        public string GetnewClientId(string id)
        {
            int nid = Convert.ToInt32(id);
            if (id != null && id != "")
            {
                return Convert.ToString((from a in db.CPR_Incidents
                        join b in db.Intake_Assessments on a.Intake_Assessment_Id equals b.Intake_Assessment_Id
                        join c in db.Clients on b.Client_Id equals c.Client_Id
                        where a.Incident_Id == nid
                        select c.Client_Id).FirstOrDefault());
            }
            else return null;
        }

        public Person GetPersonDetails(int PersonId)
        {

            return db.Persons.Find(PersonId);
        }

        public string GetReferenceNumber(int PersonId)
        {
            if (PersonId != 0)
            {
                return (from a in db.Clients
                        where a.Person_Id== PersonId
                        select a.Reference_Number).FirstOrDefault();
            }
            else return "";
        }

        public string GetTownDescription(int Town_Id)
        {
            return db.Towns.Find(Town_Id).Description;
        }
    }
}
