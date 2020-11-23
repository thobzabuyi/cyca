using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisCommunityModel
    {
        //public NISIS_Community GetSpecificNisisCommunity(int nisisCommunityId)
        //{
        //    NISIS_Community nisisCommunity;

        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var nisisCommunityList = (from r in dbContext.NISIS_Communities
        //                                  where r.NISIS_Community_Id.Equals(nisisCommunityId)
        //                                  select r).ToList();

        //        nisisCommunity = (from r in nisisCommunityList
        //                          select r).FirstOrDefault();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //    return nisisCommunity;
        //}

        //public List<NISIS_Community> GetListOfNisisCommunities(bool showInActive, bool showDeleted)
        //{
        //    List<NISIS_Community> nisisCommunities;

        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var nisisCommunityList = (from x in dbContext.NISIS_Communities
        //                                  where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
        //                                  where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
        //                                  select x).ToList();

        //        nisisCommunities = (from x in nisisCommunityList
        //                            select x).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //    return nisisCommunities;
        //}

        //public List<NISIS_Community> GetListOfNisisCommunities(bool showInActive, bool showDeleted, int localMunicipalityId)
        //{
        //    List<NISIS_Community> nisisCommunities;

        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var nisisCommunityList = (from x in dbContext.NISIS_Communities
        //                                  where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
        //                                  where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
        //                                  where x.Local_Municipality_Id.Equals(localMunicipalityId)
        //                                  select x).ToList();

        //        nisisCommunities = (from x in nisisCommunityList
        //                            select x).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //    return nisisCommunities;
        //}
    }
}
