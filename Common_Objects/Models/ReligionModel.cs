using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ReligionModel
    {
        public Religion GetSpecificReligion(int religionId)
        {
            Religion religion;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var religionList = (from r in dbContext.Religions
                                    where r.Religion_Id.Equals(religionId)
                                    select r).ToList();

                religion = (from r in religionList
                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return religion;
        }

        public List<Religion> GetListOfReligions()
        {
            List<Religion> religions;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var religionList = (from r in dbContext.Religions
                                        select r).ToList();

                    religions = (from r in religionList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return religions;
        }

    }
}
