using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class RaceModel
    {
        public Race GetSpecificRace(int raceId)
        {
            Race race;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var raceList = (from r in dbContext.Races
                                where r.Race_Id.Equals(raceId)
                                select r).ToList();

                race = (from r in raceList
                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return race;
        }

        public List<Race> GetListOfRaces()
        {
            List<Race> races;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var raceList = (from r in dbContext.Races
                                    select r).ToList();

                    races = (from r in raceList
                             select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return races;
        }
    }
}