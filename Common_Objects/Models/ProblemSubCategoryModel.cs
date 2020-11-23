using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProblemSubCategoryModel
    {
        public Problem_Sub_Category GetSpecificProblemSubCategory(int problemSubCategoryId)
        {
            Problem_Sub_Category problemSubCategory;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var problemSubCategoryList = (from r in dbContext.Problem_Sub_Categories
                                              where r.Problem_Sub_Category_Id.Equals(problemSubCategoryId)
                                              select r).ToList();

                problemSubCategory = (from r in problemSubCategoryList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return problemSubCategory;
        }

        public List<Problem_Sub_Category> GetListOfProblemSubCategories()
        {
            List<Problem_Sub_Category> problemSubCategories;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var problemSubCategoryList = (from r in dbContext.Problem_Sub_Categories
                                                  select r).ToList();

                    problemSubCategories = (from r in problemSubCategoryList
                                            select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return problemSubCategories;
        }
    }
}
