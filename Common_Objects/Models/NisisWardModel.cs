using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisWardModel
    {
        public NISIS_Ward GetSpecificNisisWard(int nisisWardId)
        {
            NISIS_Ward nisisWard;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisWardList = (from r in dbContext.NISIS_Wards
                                     where r.NISIS_Ward_Id.Equals(nisisWardId)
                                     select r).ToList();

                nisisWard = (from r in nisisWardList
                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisWard;
        }

        public List<NISIS_Ward> GetListOfNisisWards(bool showInActive, bool showDeleted)
        {
            List<NISIS_Ward> nisisWards;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisWardList = (from x in dbContext.NISIS_Wards
                                     where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                     where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                     select x).ToList();

                nisisWards = (from x in nisisWardList
                              select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisWards;
        }

        public List<NISIS_Ward> GetListOfNisisWards(bool showInActive, bool showDeleted, int localMunicipalityId)
        {
            List<NISIS_Ward> nisisWards;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisWardList = (from x in dbContext.NISIS_Wards
                                     where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                     where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                     where x.Local_Municipality_Id.Equals(localMunicipalityId)
                                     select x).ToList();

                nisisWards = (from x in nisisWardList
                              select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisWards;
        }
    }
}
