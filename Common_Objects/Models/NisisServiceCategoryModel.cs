using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisServiceCategoryModel
    {
        public NISIS_Service_Category GetSpecificServiceCategory(int serviceCategoryId)
        {
            NISIS_Service_Category serviceCategory;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var serviceCategoryList = (from r in dbContext.NISIS_Service_Categories
                                           where r.NISIS_Service_Category_Id.Equals(serviceCategoryId)
                                           select r).ToList();

                serviceCategory = (from r in serviceCategoryList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return serviceCategory;
        }

        public List<NISIS_Service_Category> GetListOfServiceCategories()
        {
            List<NISIS_Service_Category> serviceCategories;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var serviceCategoryList = (from r in dbContext.NISIS_Service_Categories
                                            select r).ToList();

                serviceCategories = (from r in serviceCategoryList
                                        select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return serviceCategories;
        }

    }
}
