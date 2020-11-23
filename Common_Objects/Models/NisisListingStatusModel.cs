using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisListingStatusModel
    {
        //public NISIS_Listing_Status GetSpecificNisisListingStatus(int nisisListingStatusId)
        //{
        //    NISIS_Listing_Status nisisListingStatus;

        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var nisisListingStatusList = (from r in dbContext.NISIS_Listing_Statusses
        //                                      where r.Listing_Status_Item_Id.Equals(nisisListingStatusId)
        //                                      select r).ToList();

        //        nisisListingStatus = (from r in nisisListingStatusList
        //                              select r).FirstOrDefault();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //    return nisisListingStatus;
        //}

        //public List<NISIS_Listing_Status> GetListOfNisisListingStatusses()
        //{
        //    List<NISIS_Listing_Status> nisisListingStatusses;

        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var nisisListingStatusList = (from x in dbContext.NISIS_Listing_Statusses
        //                                      select x).ToList();

        //        nisisListingStatusses = (from x in nisisListingStatusList
        //                                 select x).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //    return nisisListingStatusses;
        //}
    }
}
