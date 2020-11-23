using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class OfficeTypeModel
    {
        public Office_Type GetSpecificOfficeType(int abuseTypeId)
        {
            Office_Type officeType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var officeTypeList = (from r in dbContext.Office_Types
                                      where r.Office_Type_Id.Equals(abuseTypeId)
                                      select r).ToList();

                officeType = (from r in officeTypeList
                              select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return officeType;
        }

        public List<Office_Type> GetListOfOfficeTypes()
        {
            List<Office_Type> officeTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var officeTypesList = (from r in dbContext.Office_Types
                                           select r).ToList();

                    officeTypes = (from r in officeTypesList
                                   select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return officeTypes;
        }
    }
}
