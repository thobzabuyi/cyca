using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class IncidentLocationModel
    {
        public Incident_Location GetSpecificIncidentLocation(int incidentLocationId)
        {
            Incident_Location incidentLocation;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var incidentLocationList = (from r in dbContext.Incident_Locations
                                            where r.Incident_Location_Id.Equals(incidentLocationId)
                                            select r).ToList();

                incidentLocation = (from r in incidentLocationList
                                    select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return incidentLocation;
        }

        public List<Incident_Location> GetListOfIncidentLocations()
        {
            List<Incident_Location> incidentLocations;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var incidentLocationList = (from r in dbContext.Incident_Locations
                                                select r).ToList();

                    incidentLocations = (from r in incidentLocationList
                                         select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return incidentLocations;
        }
    }
}
