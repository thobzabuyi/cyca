using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common_Objects;
using Common_Objects.Models;

namespace SDIIS.Controllers
{
    public class PersonController : Controller
    {
        public ActionResult Create(string personId)
        {
            var person = new PersonDetailViewModel()
            {
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address()
            };

            return PartialView("_PersonDetails", person);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(PersonDetailViewModel person, FormCollection data)
        {
            //#region disability multiselect post
            //var intakeClientViewModel = new IntakeClientViewModel();

            //var value = data["SelectedDisabilities"];
            //var personDisabilityDetails = new PersonDisabilityModel();

            //string[] selectedDisabilityArray = data["SelectedDisabilities"].ToString().Split(',');
            //personDisabilityDetails.Delete(Convert.ToInt32(person.));

            //foreach (string i in selectedDisabilityArray)
            //{
            //    personDisabilityDetails.Create(Convert.ToInt32(i), personId);
            //}

            //if (intakeClientViewModel.PostedPresentationCondition != null)
            //{
            //    foreach (var item in intakeClientViewModel.PostedPresentationCondition.PresentationConditionIDs)
            //    {
            //        personDisabilityDetails.Create(Convert.ToInt32(item), personItem.Person_Id);
            //    }
            //}

            //#endregion
            return View();
        }

       

        public ActionResult EditPersonalDetails(string personId,FormCollection data)
        {
            var personDetail = new PersonDetailViewModel()
            {
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address()
            };

            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(int.Parse(personId));

            var physicalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
            var postalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();

            if (physicalAddress.Town != null)
            {
                physicalAddress.Selected_Local_Municipality_Id = physicalAddress.Town.Local_Municipality_Id;
                physicalAddress.Selected_Municipality_Id = physicalAddress.Town.Local_Municipality.District_Municipality_Id;
                physicalAddress.Selected_Province_Id = physicalAddress.Town.Local_Municipality.District.Province_Id;
            }

            if (postalAddress.Town != null)
            {
                postalAddress.Selected_Local_Municipality_Id = postalAddress.Town.Local_Municipality_Id;
                postalAddress.Selected_Municipality_Id = postalAddress.Town.Local_Municipality.District_Municipality_Id;
                postalAddress.Selected_Province_Id = postalAddress.Town.Local_Municipality.District.Province_Id;
            }

            personDetail.Person = personToEdit;
            personDetail.PhysicalAddress = physicalAddress;
            personDetail.PostalAddress = postalAddress;

            #region disability multiselct Get
            var disabilityType = new DisabilityModel();
            personDetail.PostedDisabilityType = new Posted_DisabilityType();


            if (personDetail.Person != null)
            {
                personDetail.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(personDetail.Person.Person_Id);
                int index = 0;
                personDetail.PostedDisabilityType = new Posted_DisabilityType();
                personDetail.PostedDisabilityType.DisabilityTypeIDs = new int[personDetail.SelectedDisabilityType.Count()];

                foreach (var item in personDetail.SelectedDisabilityType)
                {
                    personDetail.PostedDisabilityType.DisabilityTypeIDs[index] = item.Disability_Id;
                    index++;
                }

            }
            else
            {
                personDetail.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(-1);
            }

            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilityType = new List<SelectListItem>();


            foreach (var item in disabilityType.GetListOfDisabilities())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (personDetail.PostedDisabilityType.DisabilityTypeIDs != null)
                {
                    if (personDetail.PostedDisabilityType.DisabilityTypeIDs.Contains(item.Disability_Id))
                    {
                        itemSelected = true;
                    }
                }

                AvailableDisabilityType.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Disability_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableDisabilityType = AvailableDisabilityType;

            personDetail.PostedDisabilityType.ListOfDisabilityTypeIDs = new SelectList(AvailableDisabilityType, "Value", "Text");
            #endregion

            return PartialView("_PersonalDetailsEdit", personDetail);
        }

        public ActionResult EditContactDetails(string personId)
        {
            var personDetail = new PersonDetailViewModel()
            {
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address()
            };

            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(int.Parse(personId));

            var physicalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
            var postalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();

            if (physicalAddress.Town != null)
            {
                physicalAddress.Selected_Local_Municipality_Id = physicalAddress.Town.Local_Municipality_Id;
                physicalAddress.Selected_Municipality_Id = physicalAddress.Town.Local_Municipality.District_Municipality_Id;
                physicalAddress.Selected_Province_Id = physicalAddress.Town.Local_Municipality.District.Province_Id;
            }

            if (postalAddress.Town != null)
            {
                postalAddress.Selected_Local_Municipality_Id = postalAddress.Town.Local_Municipality_Id;
                postalAddress.Selected_Municipality_Id = postalAddress.Town.Local_Municipality.District_Municipality_Id;
                postalAddress.Selected_Province_Id = postalAddress.Town.Local_Municipality.District.Province_Id;
            }

            personDetail.Person = personToEdit;
            personDetail.PhysicalAddress = physicalAddress;
            personDetail.PostalAddress = postalAddress;

            return PartialView("_ContactDetailsEdit", personDetail);
        }

        public ActionResult SavePersonDetails(PersonDetailViewModel personDetail)
        {
            var status = "Error";
            var message = "A technical error has occurred! Please try again later";

            var personModel = new PersonModel();
            if (personDetail.Person.Person_Id == -1)
            {
                var dateCreated = DateTime.Now;
                const string createdBy = "jhendrikse";
                const bool isActive = true;
                const bool isDeleted = false;

                var newPerson = personModel.CreatePerson(personDetail.Person.First_Name, personDetail.Person.Last_Name, personDetail.Person.Known_As, personDetail.Person.Identification_Type_Id, personDetail.Person.Identification_Number, false, null, personDetail.Person.Date_Of_Birth, personDetail.Person.Age, personDetail.Person.Is_Estimated_Age, personDetail.Person.Sexual_Orientation_Id, personDetail.Person.Language_Id, personDetail.Person.Gender_Id, personDetail.Person.Marital_Status_Id, personDetail.Person.Religion_Id, personDetail.Person.Preferred_Contact_Type_Id, personDetail.Person.Phone_Number, personDetail.Person.Mobile_Phone_Number, personDetail.Person.Email_Address, personDetail.Person.Population_Group_Id, personDetail.Person.Nationality_Id, personDetail.Person.Disability_Type_Id, personDetail.Person.Citizenship_Id, dateCreated, createdBy, isActive, isDeleted);

                if (newPerson == null)
                {
                    status = "OK";
                    message = "The person details have been successfully created.";
                }
            }
            else
            {
                var dateLastModified = DateTime.Now;
                const string modifiedBy = "jhendrikse";
                const bool isActive = true;
                const bool isDeleted = false;

                var updatedPerson = personModel.EditPerson(personDetail.Person.Person_Id, personDetail.Person.First_Name, personDetail.Person.Last_Name, personDetail.Person.Known_As, personDetail.Person.Identification_Type_Id, personDetail.Person.Identification_Number, personDetail.Person.Date_Of_Birth, personDetail.Person.Age, personDetail.Person.Is_Estimated_Age, personDetail.Person.Sexual_Orientation_Id, personDetail.Person.Language_Id, personDetail.Person.Gender_Id, personDetail.Person.Marital_Status_Id, personDetail.Person.Religion_Id, personDetail.Person.Preferred_Contact_Type_Id, personDetail.Person.Phone_Number, personDetail.Person.Mobile_Phone_Number, personDetail.Person.Email_Address, personDetail.Person.Population_Group_Id, personDetail.Person.Nationality_Id, personDetail.Person.Disability_Type_Id, personDetail.Person.Citizenship_Id, dateLastModified, modifiedBy, isActive, isDeleted);
                
                if (updatedPerson == null)
                {
                    status = "OK";
                    message = "The person details have been successfully edited.";
                }
            }

            return new JsonResult { Data = new { status, message } };
        }
    }
}