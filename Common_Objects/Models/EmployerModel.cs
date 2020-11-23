using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class EmployerModel
    {
        public Employer GetSpecificEmployer(int employerId)
        {
            Employer employer;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employerList = (from r in dbContext.Employers
                                    where r.Employer_Id.Equals(employerId)
                                    select r).ToList();

                employer = (from r in employerList
                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return employer;
        }

        public List<Employer> GetListOfEmployers()
        {
            List<Employer> employers;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var employerList = (from r in dbContext.Employers
                                        select r).ToList();

                    employers = (from r in employerList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return employers;
        }
    }
}
