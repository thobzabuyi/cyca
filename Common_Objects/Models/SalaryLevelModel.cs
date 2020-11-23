using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class SalaryLevelModel
    {
        public Salary_Level GetSpecificSalaryLevel(int salarylevelId)
        {
            Salary_Level salarylevel;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var salarylevelList = (from r in dbContext.Salary_Levels
                                       where r.Salary_Level_Id.Equals(salarylevelId)
                                       select r).ToList();

                salarylevel = (from r in salarylevelList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return salarylevel;
        }

        public List<Salary_Level> GetListOfSalaryLevels()
        {
            List<Salary_Level> salarylevels;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var salarylevelList = (from r in dbContext.Salary_Levels
                                           select r).ToList();

                    salarylevels = (from r in salarylevelList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return salarylevels;
        }
    }
}