using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ReferralFocusAreaModel
    {
        public Referral_Focus_Area GetSpecificReferralFocusArea(int referralFocusAreaId)
        {
            Referral_Focus_Area referralFocusArea;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var referralFocusAreaList = (from r in dbContext.Referral_Focus_Areas
                                             where r.Referral_Focus_Area_Id.Equals(referralFocusAreaId)
                                             select r).ToList();

                referralFocusArea = (from r in referralFocusAreaList
                                     select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return referralFocusArea;
        }

        public List<Referral_Focus_Area> GetListOfReferralFocusAreas()
        {
            List<Referral_Focus_Area> referralFocusAreas;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var referralFocusAreaList = (from r in dbContext.Referral_Focus_Areas
                                                 select r).ToList();

                    referralFocusAreas = (from r in referralFocusAreaList
                                          select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return referralFocusAreas;
        }
    }
}
