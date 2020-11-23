using System;
using System.Web.Mvc;
using Common_Objects.Models;

namespace SDIIS.Controllers
{
    public class EmploymentController : Controller
    {
        public ActionResult GetEmploymentItemsByAjax(string id)
        {
            var personEmploymentModel = new PersonEmploymentModel();
            var personEmploymentDetail = personEmploymentModel.GetListOfPersonEmploymentItemsForPerson(int.Parse(id), false, false);

            return PartialView("_PersonEmploymentGrid", personEmploymentDetail);
        }

        public ActionResult Create(string id)
        {
            var personEmployment = new Person_Employment() { Person_Id = int.Parse(id) };

            return PartialView("_PersonEmploymentCreate", personEmployment);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Person_Employment employment)
        {
            
            var dateCreated = DateTime.Now;
            const string createdBy = "jhendrikse";
            const bool isActive = true;
            const bool isDeleted = false;

            var personEmploymentModel = new PersonEmploymentModel();
            var newEmploymentDetail = personEmploymentModel.CreatePersonEmployment(employment.Person_Id, employment.Nature_Of_Employment_Id, employment.Occupation, employment.Income_Range_Id, dateCreated, createdBy, isActive, isDeleted, employment.NameOfEmployer);

            var status = "";
            var message = "";

            if (newEmploymentDetail == null)
            {
                status = "Error";
                message = "A technical error has occurred! Please try again later";
            }

            return new JsonResult { Data = new { status, message } };
            
        }



        public ActionResult Edit(string id)
        {
            var personEmploymentModel = new PersonEmploymentModel();
            var employmentDetails = personEmploymentModel.GetSpecificPersonEmployment(int.Parse(id));

            return PartialView("_PersonEmploymentEdit", employmentDetails);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Person_Employment employment)
        {
            var dateLastModified = DateTime.Now;
            const string modifiedBy = "jhendrikse";
            const bool isActive = true;
            const bool isDeleted = false;
            int InAassId = Convert.ToInt32(Session["IntAssId"]);

            var personEmploymentModel = new PersonEmploymentModel();
            var editEmploymentDetail = personEmploymentModel.EditPersonEmployment(employment.Person_Employment_Id,  employment.Nature_Of_Employment_Id, employment.Occupation, employment.Income_Range_Id, dateLastModified, modifiedBy, isActive, isDeleted, employment.NameOfEmployer);

            var status = "OK";
            var message = "The employment details have been successfully edited";

            if (editEmploymentDetail == null)
            {
                status = "Error";
                message = "A technical error has occurred! Please try again later";
            }

            return new JsonResult { Data = new { status, message } };
            
        }
	}
}