using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class School_TypeModel
    {
        public School_Type GetSpecificSchooltype(int schooltypeId)
        {
            School_Type schooltype;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var schoolList = (from r in dbContext.School_Types
                                  where r.School_Type_Id.Equals(schooltypeId)
                                  select r).ToList();

                schooltype = (from r in schoolList
                          select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return schooltype;
        }

        public List<School_Type> GetListOfSchooltype()
        {
            List<School_Type> schoolstype;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var schoolList = (from r in dbContext.School_Types
                                      select r).ToList();

                    schoolstype = (from r in schoolList
                               select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return schoolstype;
        }
    }
}
