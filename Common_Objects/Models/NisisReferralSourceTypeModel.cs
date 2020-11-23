using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisReferralSourceTypeModel
    {
        public NISIS_Referral_Source_Type GetSpecificReferralSourceType(int referralSourceTypeId)
        {
            NISIS_Referral_Source_Type referralSourceType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var referralSourceTypeList = (from r in dbContext.NISIS_Referral_Source_Types
                                              where r.Referral_Soure_Type_Id.Equals(referralSourceTypeId)
                                              select r).ToList();

                referralSourceType = (from r in referralSourceTypeList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return referralSourceType;
        }

        public List<NISIS_Referral_Source_Type> GetListOfReferralSourceTypes()
        {
            List<NISIS_Referral_Source_Type> referralSourceTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var referralSourceTypeList = (from r in dbContext.NISIS_Referral_Source_Types
                                                  select r).ToList();

                    referralSourceTypes = (from r in referralSourceTypeList
                                           select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return referralSourceTypes;
        }
    }
}
