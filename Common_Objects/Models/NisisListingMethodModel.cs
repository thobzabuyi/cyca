using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class NisisListingMethodModel
    {
        public NISIS_Listing_Method GetSpecificNisisListingMethod(int nisisListingMethodId)
        {
            NISIS_Listing_Method nisisListingmethod;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisListingmethodList = (from r in dbContext.NISIS_Listing_Methods
                                              where r.NISIS_Listing_Method_Id.Equals(nisisListingMethodId)
                                              select r).ToList();

                nisisListingmethod = (from r in nisisListingmethodList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisListingmethod;
        }

        public List<NISIS_Listing_Method> GetListOfNisisListingMethods()
        {
            List<NISIS_Listing_Method> nisisListingmethods;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisListingmethodList = (from x in dbContext.NISIS_Listing_Methods
                                              select x).ToList();

                nisisListingmethods = (from x in nisisListingmethodList
                                       select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisListingmethods;
        }
    }
}
