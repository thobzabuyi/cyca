using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProblemCategoryModel
    {
        public Problem_Category GetSpecificProblemCategory(int problemCategoryId)
        {
            Problem_Category problemCategory;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var problemCategoryList = (from r in dbContext.Problem_Categories
                                           where r.Problem_Category_Id.Equals(problemCategoryId)
                                           select r).ToList();

                problemCategory = (from r in problemCategoryList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return problemCategory;
        }

        public List<Problem_Category> GetListOfProblemCategories()
        {
            List<Problem_Category> problemCategories;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var problemCategoryList = (from r in dbContext.Problem_Categories
                                               select r).ToList();

                    problemCategories = (from r in problemCategoryList
                                         select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return problemCategories;
        }
    }
}
