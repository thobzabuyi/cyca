using Common_Objects.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class ProblemCategoryController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSubCategoriesForCategory(string problemCategoryId)
        {
            if (String.IsNullOrEmpty(problemCategoryId))
            {
                throw new ArgumentNullException("id");
            }

            var problemSubCategoryModel = new ProblemSubCategoryModel();
            var subCategoriesList = problemSubCategoryModel.GetListOfProblemSubCategories();

            subCategoriesList.RemoveAll(x => x.Problem_Category_Id != int.Parse(problemCategoryId));

            var result = (from c in subCategoriesList
                          select new
                          {
                              id = c.Problem_Sub_Category_Id,
                              name = c.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}