using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class StructureTypeModel
    {
        public Structure_Type GetSpecificStructureType(int structureTypeId)
        {
            Structure_Type structureType;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var structureTypeList = (from r in dbContext.Structure_Types
                                         where r.Structure_Type_Id.Equals(structureTypeId)
                                         select r).ToList();

                structureType = (from r in structureTypeList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return structureType;
        }

        public List<Structure_Type> GetListOfStructureTypes()
        {
            List<Structure_Type> structureTypes;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var structureTypeList = (from x in dbContext.Structure_Types
                                         select x).ToList();

                structureTypes = (from x in structureTypeList
                                  select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return structureTypes;
        }
    }
}
