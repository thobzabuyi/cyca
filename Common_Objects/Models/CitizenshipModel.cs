using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class CitizenshipModel
    {
        public apl_Citizenship GetSpecificCitizenship(int citizenshipId)
        {
            apl_Citizenship citizenship;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var citizenshipList = (from r in dbContext.apl_Citizenship
                                   where r.Citizenship_Id.Equals(citizenshipId)
                                   select r).ToList();

                citizenship = (from r in citizenshipList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return citizenship;
        }

        public List<apl_Citizenship> GetListOfCitizenship()
        {
            List<apl_Citizenship> citizenship;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var citizenshipList = (from r in dbContext.apl_Citizenship
                                        select r).ToList();

                    citizenship = (from r in citizenshipList
                                   select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return citizenship;
        }
    }
}
