using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CourtModel
    {
        public Court GetSpecificCourt(int courtId)
        {
            Court court;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var courtList = (from r in dbContext.Courts
                                 where r.Court_Id.Equals(courtId)
                                 select r).ToList();

                court = (from r in courtList
                         select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return court;
        }

        public List<Court> GetListOfCourts()
        {
            List<Court> courts;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var courtList = (from r in dbContext.Courts
                                     select r).ToList();

                    courts = (from r in courtList
                              select r).OrderBy(r=>r.Description).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return courts;
        }

        public List<Court> GetListOfCourts(int? districtId)
        {
            List<Court> courts;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var courtList = (from r in dbContext.Courts
                                     where r.District_Id == districtId
                                     select r).ToList();

                    courts = (from r in courtList
                              select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return courts;
        }
    }
}
