using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class TreatmentTypeModel
    {
        public Treatment_Type GetSpecificTreatmentType(int treatmentTypeId)
        {
            Treatment_Type treatmentType;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var treatmentTypeList = (from r in dbContext.Treatment_Types
                                         where r.Treatment_Type_Id.Equals(treatmentTypeId)
                                         select r).ToList();

                treatmentType = (from r in treatmentTypeList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return treatmentType;
        }

        public List<Treatment_Type> GetListOfTreatmentTypes()
        {
            List<Treatment_Type> treatmentTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var treatmentTypeList = (from r in dbContext.Treatment_Types
                                             select r).ToList();

                    treatmentTypes = (from r in treatmentTypeList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return treatmentTypes;
        }
    }
}
