using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class PreferredContactTypeModel
    {
        public Preferred_Contact_Type GetSpecificPreferredContactType(int preferredContactTypeId)
        {
            Preferred_Contact_Type preferredContactType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var preferredContactTypeList = (from r in dbContext.Preferred_Contact_Types
                                                where r.Preferred_Contact_Type_Id.Equals(preferredContactTypeId)
                                                select r).ToList();

                preferredContactType = (from r in preferredContactTypeList
                                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return preferredContactType;
        }

        public List<Preferred_Contact_Type> GetListOfPreferredContactTypes()
        {
            List<Preferred_Contact_Type> preferredContactTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var preferredContactTypeList = (from r in dbContext.Preferred_Contact_Types
                                                    select r).ToList();

                    preferredContactTypes = (from r in preferredContactTypeList
                                             select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return preferredContactTypes;
        }
    }
}
