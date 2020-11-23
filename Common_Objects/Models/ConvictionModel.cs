using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ConvictionModel
    {
        public CPR_Conviction GetSpecificConviction(int cprConvictionId)
        {
            CPR_Conviction cprConviction;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var cprConvictionList = (from r in dbContext.CPR_Convictions
                                         where r.CPR_Conviction_Id.Equals(cprConvictionId)
                                         select r).ToList();

                cprConviction = (from r in cprConvictionList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return cprConviction;
        }

        public List<CPR_Conviction> GetListOfConvictions()
        {
            List<CPR_Conviction> cprConvictions;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var cprConvictionList = (from r in dbContext.CPR_Convictions
                                             select r).ToList();

                    cprConvictions = (from r in cprConvictionList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return cprConvictions;
        }

        public List<CPR_Conviction> GetListOfConvictions(int allegedOffenderId)
        {
            List<CPR_Conviction> cprConvictions;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var cprConvictionList = (from r in dbContext.CPR_Convictions
                                             where r.Alleged_Offender_Id.Equals(allegedOffenderId)
                                             select r).ToList();

                    cprConvictions = (from r in cprConvictionList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return cprConvictions;
        }

        public CPR_Conviction CreateCPRConviction(int allegedOffenderId, string j14ReferenceNumber, string sapsCaseNumber, int? courtId, string conviction, string natureOfCharge, string sentence, DateTime? dateSentenced, string accountOfCharge)
        {
            CPR_Conviction newConviction;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var cprConviction = new CPR_Conviction()
                {
                    Alleged_Offender_Id = allegedOffenderId,
                    J14_Reference_Number = j14ReferenceNumber,
                    SAPS_Case_Number = sapsCaseNumber,
                    Court_Id = courtId,
                    Conviction = conviction,
                    Nature_Of_Charge = natureOfCharge,
                    Sentence = sentence,
                    Date_Sentenced = dateSentenced,
                    Account_Of_Charge_And_Conviction = accountOfCharge
                };

                try
                {
                    newConviction = dbContext.CPR_Convictions.Add(cprConviction);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exVal = ex.Message;
                    return null;
                }
            }

            return newConviction;
        }

        public CPR_Conviction EditCPRConviction(int cprConvictionId, int allegedOffenderId, string j14ReferenceNumber, string sapsCaseNumber, int? courtId, string conviction, string natureOfCharge, string sentence, DateTime? dateSentenced, string accountOfCharge)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editConviction = (from a in dbContext.CPR_Convictions
                                   where a.CPR_Conviction_Id.Equals(cprConvictionId)
                                   select a).FirstOrDefault();

                if (editConviction == null) return null;

                editConviction.Alleged_Offender_Id = allegedOffenderId;
                editConviction.J14_Reference_Number = j14ReferenceNumber;
                editConviction.SAPS_Case_Number = sapsCaseNumber;
                editConviction.Court_Id = courtId;
                editConviction.Conviction = conviction;
                editConviction.Nature_Of_Charge = natureOfCharge;
                editConviction.Sentence = sentence;
                editConviction.Date_Sentenced = dateSentenced;
                editConviction.Account_Of_Charge_And_Conviction = accountOfCharge;

                dbContext.SaveChanges();

                return editConviction;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
