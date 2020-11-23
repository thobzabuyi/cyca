using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class IncidentCorrespondenceModel
    {
        public Incident_Correspondence GetSpecificIncidentCorrespondence(int incidentCorrespondenceId)
        {
            Incident_Correspondence incidentCorrespondence;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var incidentCorrespondenceList = (from r in dbContext.Incident_Correspondence_Items
                                                  where r.Incident_Correspondence_Id.Equals(incidentCorrespondenceId)
                                                  select r).ToList();

                incidentCorrespondence = (from r in incidentCorrespondenceList
                                          select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return incidentCorrespondence;
        }

        public List<Incident_Correspondence> GetListOfincidentCorrespondences()
        {
            List<Incident_Correspondence> incidentCorrespondences;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var incidentCorrespondenceList = (from r in dbContext.Incident_Correspondence_Items
                                                      select r).ToList();

                    incidentCorrespondences = (from r in incidentCorrespondenceList
                                               select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return incidentCorrespondences;
        }

        public List<Incident_Correspondence> GetListOfincidentCorrespondences(int incidentId)
        {
            List<Incident_Correspondence> incidentCorrespondences;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var incidentCorrespondenceList = (from r in dbContext.Incident_Correspondence_Items
                                                      where r.Incident_Correspondence_Id.Equals(incidentId)
                                                      select r).ToList();

                    incidentCorrespondences = (from r in incidentCorrespondenceList
                                               select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return incidentCorrespondences;
        }

        public Incident_Correspondence CreateIncidentCorrespondence(int incidentId, int correspondenceTypeId, DateTime dateCorrespondenceSent)
        {
            Incident_Correspondence newIncidentCorrespondence;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var incidentCorrespondence = new Incident_Correspondence()
                {
                    Incident_Id = incidentId,
                    Correspondence_Type_Id = correspondenceTypeId,
                    Date_Correspondence_Sent = dateCorrespondenceSent
                };

                try
                {
                    newIncidentCorrespondence = dbContext.Incident_Correspondence_Items.Add(incidentCorrespondence);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exVal = ex.Message;
                    return null;
                }
            }

            return newIncidentCorrespondence;
        }

        public Incident_Correspondence EditIncidentCorrespondence(int incidentCorrespondenceId, int incidentId, int correspondenceTypeId, DateTime dateCorrespondenceSent)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editincidentCorrespondence = (from a in dbContext.Incident_Correspondence_Items
                                                  where a.Incident_Correspondence_Id.Equals(incidentCorrespondenceId)
                                                  select a).FirstOrDefault();

                if (editincidentCorrespondence == null) return null;

                editincidentCorrespondence.Incident_Id = incidentId;
                editincidentCorrespondence.Correspondence_Type_Id = correspondenceTypeId;
                editincidentCorrespondence.Date_Correspondence_Sent = dateCorrespondenceSent;

                dbContext.SaveChanges();

                return editincidentCorrespondence;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
