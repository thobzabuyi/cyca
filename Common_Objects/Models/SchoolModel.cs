using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class SchoolModel
    {
        public School GetSpecificSchool(int schoolId)
        {
            School school;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var schoolList = (from r in dbContext.Schools
                                  where r.School_Id.Equals(schoolId)
                                  select r).ToList();

                school = (from r in schoolList
                          select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return school;
        }

        public List<School> GetListOfSchools()
        {
            List<School> schools;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var schoolList = (from r in dbContext.Schools
                                      select r).ToList();

                    schools = (from r in schoolList
                               select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return schools;
        }

        public List<School> GetListOfSchools(int School_Type_Id)
        {

            List<School> schools;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var schoolList = (from r in dbContext.Schools
                                      join st in dbContext.School_Types
                                      on r.School_Type_Id equals st.School_Type_Id
                                      where st.School_Type_Id.Equals(School_Type_Id)
                                      select r).ToList();

                    schools = (from r in schoolList
                               select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return schools;

        }
    }
}
