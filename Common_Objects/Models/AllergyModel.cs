using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class AllergyModel
    {
        public Allergy GetSpecificAllergy(int allergyId)
        {
            Allergy allergy;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var allergyList = (from r in dbContext.Allergies
                                   where r.Allergy_Id.Equals(allergyId)
                                   select r).ToList();

                allergy = (from r in allergyList
                           select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return allergy;
        }

        public List<Allergy> GetListOfAllergies()
        {
            List<Allergy> allergies;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var allergyList = (from r in dbContext.Allergies
                                       select r).ToList();

                    allergies = (from r in allergyList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return allergies;
        }
    }
}
