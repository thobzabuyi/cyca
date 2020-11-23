using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class MunicipalityModel
    {
        public Municipality GetSpecificLocalMunicipality(int municipalityId)
        {
            Municipality municipality;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var municipalityList = (from r in dbContext.Municipalities
                                        where r.Municipality_Id.Equals(municipalityId)
                                        orderby r.Description
                                        select r).ToList();

                municipality = (from r in municipalityList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return municipality;
        }

        //public Municipality GetSpecificLocalMunicipality(string municipalityDescription)
        //{
        //    Municipality municipality;

        //    var dbContext = new SDIIS_DatabaseEntities();
        //    try
        //    {
        //        var municipalityList = (from r in dbContext.Municipalities
        //                                     where r.Description.Equals(municipalityDescription)
        //                                     select r).ToList();

        //        municipality = (from r in municipalityList
        //                             select r).FirstOrDefault();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //    return municipality;
        //}

        public List<Municipality> GetListOfMunicipalities()
        {
            List<Municipality> municipalities;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var municipalityList = (from r in dbContext.Municipalities
                                            select r).ToList();

                    municipalities = (from r in municipalityList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return municipalities;
        }

        public List<Municipality> GetListOfMunicipalities(int provinceId)
        {
            List<Municipality> municipalities;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var municipalityList = (from r in dbContext.Municipalities
                                            where r.Area.District.Province_Id.Equals(provinceId)
                                            orderby r.Description
                                            select r).ToList();

                    municipalities = (from r in municipalityList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return municipalities;
        }
    }
}
