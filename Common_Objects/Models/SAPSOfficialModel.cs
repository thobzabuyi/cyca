using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class SAPSOfficialModel
    {
        public SAPS_Official GetSpecificSAPSOfficial(int sapsOfficialId)
        {
            SAPS_Official sapsOfficial;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var sapsOfficialList = (from r in dbContext.SAPS_Officials
                                        where r.SAPS_Official_Id.Equals(sapsOfficialId)
                                        select r).ToList();

                sapsOfficial = (from r in sapsOfficialList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return sapsOfficial;
        }

        public List<SAPS_Official> GetListOfSAPSOfficials()
        {
            List<SAPS_Official> sapsOfficials;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var sapsOfficialList = (from r in dbContext.SAPS_Officials
                                        select r).ToList();

                sapsOfficials = (from r in sapsOfficialList
                                 select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return sapsOfficials;
        }

        public SAPS_Official CreateSAPSOfficial(string firstName, string lastName)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var sapsOfficial = new SAPS_Official()
            {
                First_Name = firstName,
                Last_Name = lastName
            };

            try
            {
                var newSAPSOfficial = dbContext.SAPS_Officials.Add(sapsOfficial);

                dbContext.SaveChanges();

                return newSAPSOfficial;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SAPS_Official EditSAPSOfficial(int sapsOfficialId, string firstName, string lastName)
        {
            SAPS_Official editSAPSOfficial;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSAPSOfficial = (from a in dbContext.SAPS_Officials
                                        where a.SAPS_Official_Id.Equals(sapsOfficialId)
                                        select a).FirstOrDefault();

                    if (editSAPSOfficial == null) return null;

                    editSAPSOfficial.First_Name = firstName;
                    editSAPSOfficial.Last_Name = lastName;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSAPSOfficial;
        }
    }
}
