using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisProgrammeStatusModel
    {
        public NISIS_Programme_Status GetSpecificNisisProgrammeStatus(int nisisProgrammeStatusId)
        {
            NISIS_Programme_Status nisisProgrammeStatus;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisProgrammeStatusList = (from r in dbContext.NISIS_Programme_Statusses
                                                where r.Programme_Status_Item_Id.Equals(nisisProgrammeStatusId)
                                                select r).ToList();

                nisisProgrammeStatus = (from r in nisisProgrammeStatusList
                                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisProgrammeStatus;
        }

        public List<NISIS_Programme_Status> GetListOfNisisProgrammeStatusses()
        {
            List<NISIS_Programme_Status> nisisProgrammeStatusses;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisProgrammeStatusList = (from x in dbContext.NISIS_Programme_Statusses
                                                select x).ToList();

                nisisProgrammeStatusses = (from x in nisisProgrammeStatusList
                                           select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisProgrammeStatusses;
        }

    }
}
