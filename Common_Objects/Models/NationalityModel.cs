using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NationalityModel
    {
        public Nationality GetSpecificNationality(int nationalityId)
        {
            Nationality nationality;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var nationalityList = (from r in dbContext.Nationalities
                                       where r.Nationality_Id.Equals(nationalityId)
                                       select r).ToList();

                nationality = (from r in nationalityList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nationality;
        }

        public List<Nationality> GetListOfNationalities()
        {
            List<Nationality> nationalities;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var nationalityList = (from r in dbContext.Nationalities
                                           select r).ToList();

                    nationalities = (from r in nationalityList
                                     select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return nationalities;
        }
    }
}
