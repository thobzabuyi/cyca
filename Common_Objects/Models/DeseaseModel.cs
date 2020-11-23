using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class DeceaseModel
    {
        public apl_Decease GetSpecificDecease(int deceaseId)
        {
            apl_Decease decease;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var deceaseList = (from r in dbContext.apl_Decease
                                          where r.Decease_Id.Equals(deceaseId)
                                          select r).ToList();

                decease = (from r in deceaseList
                           select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return decease;
        }

        public List<apl_Decease> GetListOfDeseases()
        {
            List<apl_Decease> deceases;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var deceasesList = (from r in dbContext.apl_Decease
                                              select r).ToList();

                    deceases = (from r in deceasesList
                                        select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return deceases;
        }
    }

}
