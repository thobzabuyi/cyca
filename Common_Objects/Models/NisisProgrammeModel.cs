using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisProgrammeModel
    {
        public NISIS_Programme GetSpecificNisisProgramme(int nisisProgrammeId)
        {
            NISIS_Programme nisisProgramme;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisProgrammeList = (from r in dbContext.NISIS_Programmes
                                          where r.NISIS_Programme_Id.Equals(nisisProgrammeId)
                                          select r).ToList();

                nisisProgramme = (from r in nisisProgrammeList
                                  select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisProgramme;
        }

        public List<NISIS_Programme> GetListOfNisisProgrammes()
        {
            List<NISIS_Programme> nisisProgrammes;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisProgrammeList = (from x in dbContext.NISIS_Programmes
                                          select x).ToList();

                nisisProgrammes = (from x in nisisProgrammeList
                                   select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisProgrammes;
        }
    }
}
