using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class TreatmentGivenByModel
    {
        public Treatment_Given_By GetSpecificTreatmentGivenBy(int treatmentGivenById)
        {
            Treatment_Given_By treatmentGivenBy;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var treatmentGivenByList = (from r in dbContext.Treatment_Given_By_Items
                                            where r.Treatment_Given_By_Id.Equals(treatmentGivenById)
                                            select r).ToList();

                treatmentGivenBy = (from r in treatmentGivenByList
                                    select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return treatmentGivenBy;
        }

        public List<Treatment_Given_By> GetListOfTreatmentGivenByItems()
        {
            List<Treatment_Given_By> treatmentGivenByItems;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var treatmentGivenByList = (from r in dbContext.Treatment_Given_By_Items
                                                select r).ToList();

                    treatmentGivenByItems = (from r in treatmentGivenByList
                                             select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return treatmentGivenByItems;
        }
    }
}
