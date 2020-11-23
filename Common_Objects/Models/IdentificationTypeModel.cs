using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class IdentificationTypeModel
    {
        public Identification_Type GetSpecificIdentificationType(int identificationTypeId)
        {
            Identification_Type identificationType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var identificationTypeList = (from r in dbContext.Identification_Types
                                      where r.Identification_Type_Id.Equals(identificationTypeId)
                                      select r).ToList();

                identificationType = (from r in identificationTypeList
                              select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return identificationType;
        }

        public List<Identification_Type> GetListOfIdentificationTypes()
        {
            List<Identification_Type> identificationTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var identificationTypeList = (from r in dbContext.Identification_Types
                                          select r).ToList();

                    identificationTypes = (from r in identificationTypeList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return identificationTypes;
        }
    }
}
