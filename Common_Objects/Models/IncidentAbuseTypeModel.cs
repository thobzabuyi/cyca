using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class IncidentAbuseTypeModel
    {
        public Incident_Abuse_Type GetSpecificIncidentAbuseType(int incidentAbuseTypeId)
        {
            Incident_Abuse_Type incidentAbuseType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var incidentAbuseTypeList = (from r in dbContext.Incident_Abuse_Types
                                             where r.Incident_Abuse_Type_Id.Equals(incidentAbuseTypeId)
                                             select r).ToList();

                incidentAbuseType = (from r in incidentAbuseTypeList
                                     select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return incidentAbuseType;
        }

        public List<Incident_Abuse_Type> GetListOfIncidentAbuseTypes()
        {
            List<Incident_Abuse_Type> incidentAbuseTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var incidentAbuseTypeList = (from r in dbContext.Incident_Abuse_Types
                                                 select r).ToList();

                    incidentAbuseTypes = (from r in incidentAbuseTypeList
                                          select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return incidentAbuseTypes;
        }

        public Incident_Abuse_Type CreateIncidentAbuseType(int incidentId, int abuseTypeId, bool isPrimaryAbuse)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var incidentAbuseType = new Incident_Abuse_Type() { Incident_Id = incidentId, Abuse_Type_Id = abuseTypeId, Is_Primary_Abuse_Type = isPrimaryAbuse };

            try
            {
                var newIncidentAbuseType = dbContext.Incident_Abuse_Types.Add(incidentAbuseType);

                dbContext.SaveChanges();

                return newIncidentAbuseType;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Incident_Abuse_Type EditAddress(int incidentAbuseTypeId, int incidentId, int abuseTypeId, bool isPrimaryAbuse)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editIncidentAbuseType = (from i in dbContext.Incident_Abuse_Types
                                             where i.Incident_Abuse_Type_Id.Equals(incidentAbuseTypeId)
                                             select i).FirstOrDefault();

                if (editIncidentAbuseType == null) return null;

                editIncidentAbuseType.Incident_Id = incidentId;
                editIncidentAbuseType.Abuse_Type_Id = abuseTypeId;
                editIncidentAbuseType.Is_Primary_Abuse_Type = isPrimaryAbuse;

                dbContext.SaveChanges();

                return editIncidentAbuseType;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
