using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class InformantCapacityTypeModel
    {
        public Informant_Capacity_Type GetSpecificInformantCapacityType(int informantCapacityTypeId)
        {
            Informant_Capacity_Type informantCapacityType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var informantCapacityTypeList = (from r in dbContext.Informant_Capacity_Types
                                                 where r.Informant_Capacity_Type_Id.Equals(informantCapacityTypeId)
                                                 select r).ToList();

                informantCapacityType = (from r in informantCapacityTypeList
                                         select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return informantCapacityType;
        }

        public List<Informant_Capacity_Type> GetListOfInformantCapacityTypes()
        {
            List<Informant_Capacity_Type> informantCapacityTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var informantCapacityTypeList = (from r in dbContext.Informant_Capacity_Types
                                                     select r).ToList();

                    informantCapacityTypes = (from r in informantCapacityTypeList
                                              select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return informantCapacityTypes;
        }
    }
}
