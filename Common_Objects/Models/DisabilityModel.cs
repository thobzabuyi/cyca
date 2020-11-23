using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class DisabilityModel
    {
        public Disability GetSpecificDisability(int disabilityId)
        {
            Disability disability;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var disabilityList = (from r in dbContext.Disabilities
                                      where r.Disability_Id.Equals(disabilityId)
                                      select r).ToList();

                disability = (from r in disabilityList
                              select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return disability;
        }

        public List<Disability> GetSelectedListOfDisabilities(int disabilityId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var query = (from pt in dbContext.Disabilities
                             join ttab in dbContext.Int_Person_Disability on pt.Disability_Id equals ttab.Disability_Id
                             where ttab.Person_Id == disabilityId
                             select pt).ToList();

                return query;
                //return dbContext.VEP_VictimizationType.Where(a => a. == victimizationTypeId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public List<Disability> GetSelectedListOfDisabilities(int disabilityId)
        //{
        //    List<Disability> disabilities;

        //    using (var dbContext = new SDIIS_DatabaseEntities())
        //    {
        //        try
        //        {
        //            var disabilityList = (from r in dbContext.Disabilities
        //                                  where r.Disability_Id.Equals(disabilityId)
        //                                  select r).ToList();

        //            disabilities = (from r in disabilityList
        //                            select r).ToList();
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }

        //    return disabilities;
        //}

        public List<Disability> GetListOfDisabilities()
        {
            List<Disability> disabilities;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var disabilityList = (from r in dbContext.Disabilities
                                          select r).ToList();

                    disabilities = (from r in disabilityList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return disabilities;
        }
    }
}
