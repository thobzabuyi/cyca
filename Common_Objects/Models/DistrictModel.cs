using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class DistrictModel
    {
        public District GetSpecificDistrict(int districtId)
        {
            District district;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var districtList = (from r in dbContext.Districts
                                    where r.District_Id.Equals(districtId)
                                    select r).ToList();

                district = (from r in districtList
                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return district;
        }

        public List<District> GetListOfDistricts()
        {
            List<District> districts;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var districtList = (from r in dbContext.Districts
                                        orderby r.Description
                                        select r).ToList();

                    districts = (from r in districtList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return districts;
        }

        public List<District> GetListOfDistricts(int provinceId)
        {
            List<District> districts;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var districtList = (from r in dbContext.Districts
                                        where r.Province_Id.Equals(provinceId)
                                        select r).ToList();

                    districts = (from r in districtList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return districts;
        }
    }
}
