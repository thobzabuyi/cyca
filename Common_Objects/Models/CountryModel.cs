using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CountryModel
    {
        public Country GetSpecificCountry(int countryId)
        {
            Country country;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var countryList = (from r in dbContext.Countries
                                   where r.Country_Id.Equals(countryId)
                                   select r).ToList();

                country = (from r in countryList
                           select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return country;
        }

        public List<Country> GetListOfCountries()
        {
            List<Country> countries;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var countryList = (from r in dbContext.Countries
                                    select r).ToList();

                countries = (from r in countryList
                                select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return countries;
        }
    }
}
