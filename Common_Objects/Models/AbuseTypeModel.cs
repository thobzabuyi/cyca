using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class AbuseTypeModel
    {
        public Abuse_Type GetSpecificAbuseType(int abuseTypeId)
        {
            Abuse_Type abuseType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var abuseTypeList = (from r in dbContext.Abuse_Types
                                     where r.Abuse_Type_Id.Equals(abuseTypeId)
                                     select r).ToList();

                abuseType = (from r in abuseTypeList
                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return abuseType;
        }

        public Abuse_Type GetSpecificAbuseType(string abuseTypeDescription)
        {
            Abuse_Type abuseType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var abuseTypeList = (from r in dbContext.Abuse_Types
                                     where r.Description.Equals(abuseTypeDescription)
                                     select r).ToList();

                abuseType = (from r in abuseTypeList
                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return abuseType;
        }

        public List<Abuse_Type> GetListOfAbuseTypes()
        {
            List<Abuse_Type> abuseTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var abuseTypeList = (from r in dbContext.Abuse_Types
                                         select r).ToList();

                    abuseTypes = (from r in abuseTypeList
                                  select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return abuseTypes;
        }
    }
}
