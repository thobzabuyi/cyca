using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProvinceModel
    {
        public Province GetSpecificProvince(int provinceId)
        {
            Province province;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var provinceList = (from r in dbContext.Provinces
                                    where r.Province_Id.Equals(provinceId)
                                    select r).ToList();

                province = (from r in provinceList
                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return province;
        }

        public List<Province> GetListOfProvinces()
        {
            List<Province> provinces;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var provinceList = (from r in dbContext.Provinces
                                    orderby r.Description
                                    select r).ToList();

                provinces = (from r in provinceList
                                select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return provinces;
        }
    }
}
