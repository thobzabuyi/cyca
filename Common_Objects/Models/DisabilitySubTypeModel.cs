using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class DisabilitySubTypeModel
    {
        public apl_DisabilityType GetSpecificDisabilitySubType(int disabilityId)
        {
            apl_DisabilityType disabilitySubType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var disabilityList = (from r in dbContext.apl_DisabilityType
                                      where r.DisabilityType_Id.Equals(disabilityId)
                                      select r).ToList();

                disabilitySubType = (from r in disabilityList
                              select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return disabilitySubType;
        }

        public List<apl_DisabilityType> GetSelectedListOfDisabilitiesSubTyp(int personId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var query = (from pt in dbContext.apl_DisabilityType
                             join ttab in dbContext.int_Person_Disability_Category on pt.DisabilityType_Id equals ttab.DisabilityType_Id
                             where ttab.Person_Id == personId
                             select pt).ToList();

                return query;
               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<apl_DisabilityType> GetSelectedListOfDisabilitiesSubType(int personId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var query = (from pt in dbContext.apl_DisabilityType

                             where pt.DisabilityType_Id == personId
                             select pt).ToList();

                return query;

            }
            catch (Exception)
            {
                return null;
            }
        }




        public List<apl_DisabilityType> GetListOfDisabilitiesType()
        {
            List<apl_DisabilityType> disabilitiesSubType;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var disabilityList = (from r in dbContext.apl_DisabilityType
                                          select r).ToList();

                    disabilitiesSubType = (from r in disabilityList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return disabilitiesSubType;
        }
    }
}
