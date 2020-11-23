using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class MaritalStatusModel
    {
        public Marital_Status GetSpecificMaritalStatus(int maritalStatusId)
        {
            Marital_Status maritalStatus;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var maritalStatusList = (from r in dbContext.Marital_Statusses
                                         where r.Marital_Status_Id.Equals(maritalStatusId)
                                         select r).ToList();

                maritalStatus = (from r in maritalStatusList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return maritalStatus;
        }

        public List<Marital_Status> GetListOfMaritalStatusses()
        {
            List<Marital_Status> maritalStatusses;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var maritalStatusList = (from r in dbContext.Marital_Statusses
                                             select r).ToList();

                    maritalStatusses = (from r in maritalStatusList
                                        select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return maritalStatusses;
        }

        

    }
}
