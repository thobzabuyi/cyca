using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class SAPSStationModel
    {
        public SAPS_Station GetSpecificSAPSStation(int sapsStationId)
        {
            SAPS_Station sapsStation;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var sapsStationList = (from r in dbContext.SAPS_Stations
                                       where r.SAPS_Station_Id.Equals(sapsStationId)
                                       select r).ToList();

                sapsStation = (from r in sapsStationList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return sapsStation;
        }

        public List<SAPS_Station> GetListOfSAPSStations()
        {
            List<SAPS_Station> sapsStations;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var sapsStationList = (from r in dbContext.SAPS_Stations
                                       select r).ToList();

                sapsStations = (from r in sapsStationList
                                select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return sapsStations;
        }

        public SAPS_Station CreateSAPSStation(int townId, string description)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var sapsStation = new SAPS_Station()
            {
                Town_Id = townId,
                Description = description
            };

            try
            {
                var newSAPSStation = dbContext.SAPS_Stations.Add(sapsStation);

                dbContext.SaveChanges();

                return newSAPSStation;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SAPS_Station EditSAPSStation(int sapsStationId, int townId, string description)
        {
            SAPS_Station editSAPSStation;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSAPSStation = (from a in dbContext.SAPS_Stations
                                       where a.SAPS_Station_Id.Equals(sapsStationId)
                                       select a).FirstOrDefault();

                    if (editSAPSStation == null) return null;

                    editSAPSStation.Town_Id = townId;
                    editSAPSStation.Description = description;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSAPSStation;
        }
    }
}
