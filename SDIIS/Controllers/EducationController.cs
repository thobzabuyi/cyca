using Common_Objects.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class EducationController : Controller
    {
        public ActionResult GetEducationItemsByAjax(string id)
        {
            var personEducationModel = new PersonEducationModel();
            var personEducationItems = personEducationModel.GetListOfPersonEducationItemsForPerson(int.Parse(id), false, false);

            return PartialView("_EducationGrid", personEducationItems);
        }

        public ActionResult Create(string id)
        {
            var personEducation = new Person_Education { Person_Id = int.Parse(id) };

            return PartialView("_EducationCreate", personEducation);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Person_Education education)
        {
            var dateCreated = DateTime.Now;
            const string createdBy = "jhendrikse";
            const bool isActive = true;
            const bool isDeleted = false;

            var personEducationModel = new PersonEducationModel();
            var newEducationDetail = personEducationModel.CreatePersonEducation(education.Person_Id, education.School_Id, education.Grade_Completed_Id, education.Year_Completed, education.Date_Last_Attended, education.Additional_Information, dateCreated, createdBy, isActive, isDeleted);

            var status = "OK";
            var message = "The education details have been successfully added";

            if (newEducationDetail == null)
            {
                status = "Error";
                message = "A technical error has occurred! Please try again later";
            }

            return new JsonResult { Data = new { status, message } };
        }

        public ActionResult Edit(string id)
        {
            var personEducationModel = new PersonEducationModel();
            var educationDetails = personEducationModel.GetSpecificPersonEducation(int.Parse(id));

            return PartialView("_EducationEdit", educationDetails);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Person_Education education)
        {
            var dateLastModified = DateTime.Now;
            const string modifiedBy = "jhendrikse";
            const bool isActive = true;
            const bool isDeleted = false;

            var personEducationModel = new PersonEducationModel();
            var editEducationDetail = personEducationModel.EditPersonEducation(education.Person_Education_Id, education.School_Id, education.Grade_Completed_Id, education.Year_Completed, education.Date_Last_Attended, education.Additional_Information, dateLastModified, modifiedBy, isActive, isDeleted);

            var status = "OK";
            var message = "The education details have been successfully edited";

            if (editEducationDetail == null)
            {
                status = "Error";
                message = "A technical error has occurred! Please try again later";
            }

            return new JsonResult { Data = new { status, message } };
        }

        /// <summary>
        /// Get list of Schools
        /// Filter by School_Type_Id
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public ActionResult FilterFromSchooltypeAjax(string SchoolTypeId)
        {
            //Use to generate VEP Search Reports
            //Session["SearchProvinceId"] = provinceId;

            if (String.IsNullOrEmpty(SchoolTypeId))
            {
                SchoolTypeId = "-1";
            }

            var schoolModel = new SchoolModel();
            var schoolList = schoolModel.GetListOfSchools(int.Parse(SchoolTypeId));

            var result = (from x in schoolList

                          select new
                          {
                              id = x.School_Id,
                              name = x.School_Name
                          })

                          .ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}