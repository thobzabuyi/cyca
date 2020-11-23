using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class GradeModel
    {
        public Grade GetSpecificGrade(int gradeId)
        {
            Grade grade;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var gradeList = (from r in dbContext.Grades
                                 where r.Grade_Id.Equals(gradeId)
                                 select r).ToList();

                grade = (from r in gradeList
                         select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return grade;
        }

        public List<Grade> GetListOfGrades()
        {
            List<Grade> grades;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var gradeList = (from r in dbContext.Grades
                                     select r).ToList();

                    grades = (from r in gradeList
                              select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return grades;
        }
    }
}
