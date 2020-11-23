using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisExtermalVerificationStatusModel
    {
        public NISIS_External_Verification_Status GetSpecificExternalVerificationStatus(int externalVerificationStatusId)
        {
            NISIS_External_Verification_Status externalVerificationStatus;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var externalVerificationStatusList = (from r in dbContext.NISIS_External_Verification_Statusses
                                                      where r.External_Verification_Status_Id.Equals(externalVerificationStatusId)
                                                      select r).ToList();

                externalVerificationStatus = (from r in externalVerificationStatusList
                                              select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return externalVerificationStatus;
        }

        public List<NISIS_External_Verification_Status> GetListOfExternalVerificationStatusses()
        {
            List<NISIS_External_Verification_Status> externalVerificationStatusses;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var externalVerificationStatusList = (from r in dbContext.NISIS_External_Verification_Statusses
                                                          select r).ToList();

                    externalVerificationStatusses = (from r in externalVerificationStatusList
                                                     select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return externalVerificationStatusses;
        }
    }
}
