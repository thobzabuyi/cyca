using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class OccupationModel
    {
        public Occupation GetSpecificOccupation(int occupationId)
        {
            Occupation occupation;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var occupationList = (from r in dbContext.Occupations
                                      where r.Occupation_Id.Equals(occupationId)
                                      select r).ToList();

                occupation = (from r in occupationList
                              select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return occupation;
        }

        public List<Occupation> GetListOfOccupations()
        {
            List<Occupation> occupations;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var occupationList = (from r in dbContext.Occupations
                                          select r).ToList();

                    occupations = (from r in occupationList
                                   select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return occupations;
        }
    }
}
