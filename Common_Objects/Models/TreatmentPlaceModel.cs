using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class TreatmentPlaceModel
    {
        public Treatment_Place GetSpecificTreatmentPlace(int treatmentPlaceId)
        {
            Treatment_Place treatmentPlace;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var treatmentPlaceList = (from r in dbContext.Treatment_Places
                                          where r.Treatment_Place_Id.Equals(treatmentPlaceId)
                                          select r).ToList();

                treatmentPlace = (from r in treatmentPlaceList
                                  select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return treatmentPlace;
        }

        public List<Treatment_Place> GetListOfTreatmentPlaces()
        {
            List<Treatment_Place> treatmentPlaces;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var treatmentPlaceList = (from r in dbContext.Treatment_Places
                                              select r).ToList();

                    treatmentPlaces = (from r in treatmentPlaceList
                                       select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return treatmentPlaces;
        }
    }
}
