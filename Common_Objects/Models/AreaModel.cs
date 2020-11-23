using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class AreaModel
    {
        public Area GetSpecificArea(int areaId)
        {
            Area area;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var areaList = (from r in dbContext.Areas
                                where r.Area_Id.Equals(areaId)
                                select r).ToList();

                area = (from r in areaList
                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return area;
        }

        public List<Area> GetListOfAreas()
        {
            List<Area> areas;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var areaList = (from r in dbContext.Areas
                                select r).ToList();

                areas = (from r in areaList
                         select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return areas;
        }
    }
}
