using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisReferralPriorityModel
    {
        public NISIS_Referral_Priority GetSpecificReferralPriority(int referralPriorityId)
        {
            NISIS_Referral_Priority referralPriority;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var referralPriorityList = (from r in dbContext.NISIS_Referral_Priorities
                                            where r.Referral_Priority_Id.Equals(referralPriorityId)
                                            select r).ToList();

                referralPriority = (from r in referralPriorityList
                                    select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return referralPriority;
        }

        public List<NISIS_Referral_Priority> GetListOfReferralPriorities()
        {
            List<NISIS_Referral_Priority> referralPriorities;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var referralPriorityList = (from r in dbContext.NISIS_Referral_Priorities
                                                select r).ToList();

                    referralPriorities = (from r in referralPriorityList
                                          select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return referralPriorities;
        }
    }
}
