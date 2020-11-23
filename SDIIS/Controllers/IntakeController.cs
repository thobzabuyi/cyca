using Common_Objects;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Helpers;

namespace SDIIS.Controllers
{
    public class IntakeController : Controller
    {
        [CustomAuthorize("Main", "Intake", "Index")]
        public ActionResult Index()
        {
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;
            var currentUserProvinceId = -1;

            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }

            if (currentUser.Employees.Any())
                currentUserProvinceId = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            if (currentUser.apl_Social_Worker.Any())
                currentUserProvinceId = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            var intakeViewModel = new IntakeSearchViewModel { Person_List = new List<Person>(), Clients_Assessments_List = new List<ClientGridMain>(), Inbox_List = new List<InboxGridItem>() };

            //var personModel = new PersonModel();
            //var listOfPersons = personModel.GetListOfPersons(false, false);

            //// Initially just display items that I created.
            //listOfPersons.RemoveAll(x => !x.Created_By.Equals(userName));

            //intakeViewModel.Person_List.AddRange(listOfPersons);

            //var clientItems = listOfPersons.Select(x => new ClientGridMain()
            //{
            //    PersonId = x.Person_Id,
            //    FirstName = x.First_Name,
            //    LastName = x.Last_Name,
            //    AssessmentCount = x.Clients.Any() ? x.Clients.First().Intake_Assessments.Count : 0,
            //    NestedItems = !x.Clients.Any() ? new List<ClientGridNested>() : x.Clients.First().Intake_Assessments.Select(y => new ClientGridNested()
            //    {
            //        PersonId = x.Person_Id,
            //        AssessmentId = y.Intake_Assessment_Id,
            //        AssessmentDate = y.Assessment_Date
            //    }).ToList()
            //}).ToList();

            //intakeViewModel.Clients_Assessments_List.AddRange(clientItems);

            // Inbox Logic
            var assessmentModel = new IntakeAssessmentModel();
            var assessmentItems = assessmentModel.GetListOfIntakeAssessments(-1, currentUserProvinceId);

            if (assessmentItems != null)
            {
                // Create list of inbox items for social workers (case assignments)
                if (currentUser.apl_Social_Worker.Any())
                {
                    // If I'm a supervisor, get assessments that have not been assigned a case worker supervisor yet.
                    if (currentUser.apl_Social_Worker.First().apl_Social_Worker2 == null )
                    {

                        var supervisortInboxItems = assessmentItems.Where(x => x.Case_Manager_Supervisor_Id == currentUser.User_Id && x.Case_Manager_Id == null)
                            .Select(x => new InboxGridItem()
                            {
                                assessmentId = x.Intake_Assessment_Id,
                                clientId = x.Client_Id,
                                clientName = x.Client.Person.First_Name + " " + x.Client.Person.Last_Name,
                                clientDateOfBirth = x.Client.Person.Date_Of_Birth_With_Age,
                                assessmentDate = x.Assessment_Date,
                                assessedBy = x.Assessor.First_Name + " " + x.Assessor.Last_Name
                            }).ToList();


                        intakeViewModel.Inbox_List.AddRange(supervisortInboxItems);
                    }

                    // If I'm a case worker, get assessments where the case manager supervisor is my supervisor
                    if (currentUser.apl_Social_Worker.First().apl_Social_Worker2 != null)
                    {
                        var mySupervisorId = currentUser.apl_Social_Worker.First().apl_Social_Worker2.User_Id;

                        var caseManagerInboxItems = assessmentItems.Where(
                               x => x.Case_Manager_Supervisor != null
                               &&
                               x.Case_Manager_Supervisor_Id == currentUser.User_Id
                               && x.Case_Manager == null
                        )
                            .Select(x => new InboxGridItem()
                            {
                                assessmentId = x.Intake_Assessment_Id,
                                clientId = x.Client_Id,
                                clientName = x.Client.Person.First_Name + " " + x.Client.Person.Last_Name,
                                clientDateOfBirth = x.Client.Person.Date_Of_Birth_With_Age,
                                assessmentDate = x.Assessment_Date,
                                assessedBy = x.Assessor.First_Name + " " + x.Assessor.Last_Name
                            }).ToList();

                        intakeViewModel.Inbox_List.AddRange(caseManagerInboxItems);
                    }
                }
            }

            return View(intakeViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(IntakeSearchViewModel intakeViewModel)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;
            var currentUserProvinceId = -1;

            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }

            if (currentUser.Employees.Any())
                currentUserProvinceId = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            if (currentUser.apl_Social_Worker.Any())
                currentUserProvinceId = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;


            intakeViewModel.Inbox_List = new List<InboxGridItem>();


            var personModel = new PersonModel();
            var persons = personModel.GetListOfPersons(false, false);

            var query = from p in persons select p;

            if (!string.IsNullOrEmpty(intakeViewModel.Search_Client_Ref_No))
                query = query.Where(p => (p.Clients.Any() && (p.Clients.First().Reference_Number.Equals(intakeViewModel.Search_Client_Ref_No))));

            if (!string.IsNullOrEmpty(intakeViewModel.Search_First_Name))
                query = query.Where(p => p.First_Name.ToLower().Contains(intakeViewModel.Search_First_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeViewModel.Search_Last_Name))
                query = query.Where(p => p.Last_Name.ToLower().Contains(intakeViewModel.Search_Last_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeViewModel.Search_Client_ID_No))
                query = query.Where(p => p.Identification_Number.Contains(intakeViewModel.Search_Client_ID_No));

            DateTime parsedDate;
            if ((!string.IsNullOrEmpty(intakeViewModel.Search_Date_Of_Birth)) && (DateTime.TryParse(intakeViewModel.Search_Date_Of_Birth, out parsedDate)))
                query = query.Where(p => p.Date_Of_Birth.Equals(parsedDate));

            var filteredResults = query.ToList();

            intakeViewModel.Person_List = new List<Person>();
            intakeViewModel.Person_List.AddRange(filteredResults);

            var clientItems = filteredResults.Select(x => new ClientGridMain()
            {
                PersonId = x.Person_Id,
                FirstName = x.First_Name,
                LastName = x.Last_Name,
                AssessmentCount = x.Clients.Any() ? x.Clients.First().Intake_Assessments.Count : 0,
                NestedItems = !x.Clients.Any() ? new List<ClientGridNested>() : x.Clients.First().Intake_Assessments.Select(y => new ClientGridNested()
                {
                    PersonId = x.Person_Id,
                    AssessmentId = y.Intake_Assessment_Id,
                    AssessmentDate = y.Assessment_Date
                }).ToList()
            }).ToList();

            intakeViewModel.Clients_Assessments_List = new List<ClientGridMain>();
            intakeViewModel.Clients_Assessments_List.AddRange(clientItems);


            // Inbox Logic
            var assessmentModel = new IntakeAssessmentModel();
            var assessmentItems = assessmentModel.GetListOfIntakeAssessments(-1, currentUserProvinceId);


            // Create list of inbox items for social workers (case assignments)
            if (currentUser.apl_Social_Worker.Any())
            {
                // If I'm a supervisor, get assessments that have not been assigned a case worker supervisor yet.
                if (currentUser.apl_Social_Worker.First().apl_Social_Worker2 == null)
                {
                    var supervisortInboxItems = assessmentItems.Where(x => x.Case_Manager_Supervisor == null)
                        .Select(x => new InboxGridItem()
                        {
                            assessmentId = x.Intake_Assessment_Id,
                            clientId = x.Client_Id,
                            clientName = x.Client.Person.First_Name + " " + x.Client.Person.Last_Name,
                            clientDateOfBirth = x.Client.Person.Date_Of_Birth_With_Age,
                            assessmentDate = x.Assessment_Date,
                            assessedBy = x.Assessor.First_Name + " " + x.Assessor.Last_Name
                        }).ToList();


                    intakeViewModel.Inbox_List.AddRange(supervisortInboxItems);
                }

                // If I'm a case worker, get assessments where the case manager supervisor is my supervisor
                if (currentUser.apl_Social_Worker.First().apl_Social_Worker2 != null)
                {
                    var mySupervisorId = currentUser.apl_Social_Worker.First().Reports_To_Social_Worker_Id;

                    var caseManagerInboxItems = assessmentItems.Where(x => x.Case_Manager_Supervisor != null && x.Case_Manager_Supervisor_Id == mySupervisorId && x.Case_Manager == null)
                        .Select(x => new InboxGridItem()
                        {
                            assessmentId = x.Intake_Assessment_Id,
                            clientId = x.Client_Id,
                            clientName = x.Client.Person.First_Name + " " + x.Client.Person.Last_Name,
                            clientDateOfBirth = x.Client.Person.Date_Of_Birth_With_Age,
                            assessmentDate = x.Assessment_Date,
                            assessedBy = x.Assessor.First_Name + " " + x.Assessor.Last_Name
                        }).ToList();

                    intakeViewModel.Inbox_List.AddRange(caseManagerInboxItems);
                }
            }


            return View(intakeViewModel);
        }

        public ActionResult AcceptInboxItem(string id)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userId = -1;
            var userName = string.Empty;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                userName = currentUser.User_Name;
            }

            var assessmentModel = new IntakeAssessmentModel();
            var assessmentItem = assessmentModel.GetSpecificIntakeAssessment(int.Parse(id));


            // Test
            var superV = assessmentItem.Case_Manager_Supervisor_Id;
            var Mana = assessmentItem.Case_Manager_Id;
            // Test End

            if (assessmentItem.Case_Manager_Supervisor_Id == null)
                assessmentModel.AssignCaseManagerSupervisor(int.Parse(id), userId);

            //Kholo update
            //if ((assessmentItem.Case_Manager_Supervisor != null) && (assessmentItem.Case_Manager == null))
            if (assessmentItem.Case_Manager_Id != null)
                assessmentModel.AssignCaseManager(int.Parse(id), userId);

            return RedirectToAction("EditAssessment", new { id = int.Parse(id) });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchAjax(IntakeSearchViewModel intakeSearch)
        {
            var personModel = new PersonModel();
            var persons = personModel.GetListOfPersons(false, false);

            var query = from p in persons select p;

            if (!string.IsNullOrEmpty(intakeSearch.Search_Client_Ref_No))
                query = query.Where(p => (p.Clients.Any() && (p.Clients.First().Reference_Number.Equals(intakeSearch.Search_Client_Ref_No))));

            if (!string.IsNullOrEmpty(intakeSearch.Search_First_Name))
                query = query.Where(p => p.First_Name.ToLower().Contains(intakeSearch.Search_First_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeSearch.Search_Last_Name))
                query = query.Where(p => p.Last_Name.ToLower().Contains(intakeSearch.Search_Last_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeSearch.Search_Client_ID_No))
                query = query.Where(p => p.Identification_Number.Contains(intakeSearch.Search_Client_ID_No));

            DateTime parsedDate;
            if ((!string.IsNullOrEmpty(intakeSearch.Search_Date_Of_Birth)) && (DateTime.TryParse(intakeSearch.Search_Date_Of_Birth, out parsedDate)))
                query = query.Where(p => p.Date_Of_Birth.Equals(parsedDate));

            var filteredResults = query.ToList();

            intakeSearch.Person_List = new List<Person>();
            intakeSearch.Person_List.AddRange(filteredResults);

            return PartialView("_PersonSearchGrid", intakeSearch);
        }

        public ActionResult ClientIndex()
        {
            var intakeSearchModel = new IntakeSearchViewModel() { Person_List = new List<Person>() };

            var personModel = new PersonModel();
            var listOfPersons = personModel.GetListOfPersons(false, false);

            intakeSearchModel.Person_List.AddRange(listOfPersons);

            return View(intakeSearchModel);
        }

        public ActionResult Create()
        {
            var intakeDataViewModel = new IntakeDataViewModel()
            {
                ReceptionRegister = new Reception_Register(),
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address(),
            };

            #region disability multiselct Get
            var intakeClientViewModel = new IntakeClientViewModel();

            var disabilityType = new DisabilityModel();
            intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();


            if (intakeClientViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(intakeDataViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();
                intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs = new int[intakeClientViewModel.SelectedDisabilityType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilityType)
                {
                    intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs[index] = item.Disability_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(-1);
            }


            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilityType = new List<SelectListItem>();


            foreach (var item in disabilityType.GetListOfDisabilities())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs.Contains(item.Disability_Id))
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

            intakeClientViewModel.PostedDisabilityType.ListOfDisabilityTypeIDs = new SelectList(AvailableDisabilityType, "Value", "Text");
            #endregion

            #region Sub disability type

            var disabilitySubType = new DisabilitySubTypeModel();
            intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();


            if (intakeClientViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(intakeDataViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();
                intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs = new int[intakeClientViewModel.SelectedDisabilitySubType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilitySubType)
                {
                    intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs[index] = item.DisabilityType_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(-1);
            }


            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilitySubType = new List<SelectListItem>();


            foreach (var item in disabilitySubType.GetListOfDisabilitiesType())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs.Contains(item.DisabilityType_Id))
                    {
                        itemSelected = true;
                    }
                }

                AvailableDisabilitySubType.Add(new SelectListItem
                {
                    Text = item.TypeName,
                    Value = item.DisabilityType_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableDisabilitySubType = AvailableDisabilitySubType;

            intakeClientViewModel.PostedDisabilitySubType.ListOfDisabilitySubTypeIDs = new SelectList(AvailableDisabilitySubType, "Value", "Text");
            #endregion

            return View(intakeDataViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(IntakeDataViewModel intakeDataViewModel, FormCollection data)
        {
            if (ModelState.IsValid)
            {
                var currentUser = (User)Session["CurrentUser"];

                var userName = string.Empty;

                if (currentUser != null)
                {
                    userName = currentUser.User_Name;
                }

                var dateCreated = DateTime.Now;
                var createdBy = userName;
                const bool isActive = true;
                const bool isDeleted = false;

                // Create Person
                var personModel = new PersonModel();
                var createPerson = personModel.CreatePerson(intakeDataViewModel.Person.First_Name, intakeDataViewModel.Person.Last_Name, 
                    intakeDataViewModel.Person.Known_As, intakeDataViewModel.Person.Identification_Type_Id, intakeDataViewModel.Person.Identification_Number, 
                    false, null, intakeDataViewModel.Person.Date_Of_Birth, intakeDataViewModel.Person.Age, intakeDataViewModel.Person.Is_Estimated_Age, intakeDataViewModel.Person.Sexual_Orientation_Id,
                    intakeDataViewModel.Person.Language_Id, intakeDataViewModel.Person.Gender_Id, intakeDataViewModel.Person.Marital_Status_Id, 
                    intakeDataViewModel.Person.Religion_Id, intakeDataViewModel.Person.Preferred_Contact_Type_Id, intakeDataViewModel.Person.Phone_Number, 
                    intakeDataViewModel.Person.Mobile_Phone_Number, intakeDataViewModel.Person.Email_Address, intakeDataViewModel.Person.Population_Group_Id, 
                    intakeDataViewModel.Person.Nationality_Id, intakeDataViewModel.Person.Disability_Type_Id, intakeDataViewModel.Person.Citizenship_Id,dateCreated, createdBy,
                    isActive, isDeleted);

                if (createPerson == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(intakeDataViewModel);
                }
                else
                {
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Create New Person Details", "Intake");
                    
                }

                personModel.AddAddress(createPerson.Person_Id, (int)AddressTypeEnum.PhysicalAddress, intakeDataViewModel.PhysicalAddress.Address_Line_1, intakeDataViewModel.PhysicalAddress.Address_Line_2, intakeDataViewModel.PhysicalAddress.Town_Id, intakeDataViewModel.PhysicalAddress.Postal_Code);
                personModel.AddAddress(createPerson.Person_Id, (int)AddressTypeEnum.PostalAddress, intakeDataViewModel.PostalAddress.Address_Line_1, intakeDataViewModel.PostalAddress.Address_Line_2, intakeDataViewModel.PostalAddress.Town_Id, intakeDataViewModel.PostalAddress.Postal_Code);

                // Create Reception Register and Link Person
                var receptionRegisterModel = new ReceptionRegisterModel();
                var createReceptionRegister = receptionRegisterModel.CreateNewReceptionRegister(createPerson.Person_Id, intakeDataViewModel.ReceptionRegister.Reason_For_Visit, intakeDataViewModel.ReceptionRegister.Reception_Visit_Type_Id, intakeDataViewModel.ReceptionRegister.Visit_Date, intakeDataViewModel.ReceptionRegister.Reception_Action_Taken_Id, dateCreated, createdBy, isActive, isDeleted);

                if (createReceptionRegister == null)
                {
                    ViewBag.Message = "An Error Occurred, Please contact Support";
                    return View(intakeDataViewModel);
                }
                else
                {
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Create Reception Register", "Intake");
                    
                }
                #region disability multiselect post
                var intakeClientViewModel = new IntakeClientViewModel();

                var value = data["SelectedDisabilities"];
                var personDisabilityDetails = new PersonDisabilityModel();
                if (value!=null)
                {
                    string[] selectedDisabilityArray = data["SelectedDisabilities"].ToString().Split(',');
                    personDisabilityDetails.Delete(Convert.ToInt32(createPerson.Person_Id));

                    foreach (string i in selectedDisabilityArray)
                    {
                        personDisabilityDetails.Create(Convert.ToInt32(i), createPerson.Person_Id);
                    }

                    if (intakeClientViewModel.PostedDisabilityType != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs)
                        {
                            personDisabilityDetails.Create(Convert.ToInt32(item), createPerson.Person_Id);
                        }
                    }
                }


                #endregion

                #region disability Sub Type multiselect post
                var values = data["SelectedDisabilitiesType"];
                var personDisabilitySubTypeDetails = new PersonDisabilityTypeModel();
                if (values != null)
                {
                    string[] selectedDisabilityTypeArray = data["SelectedDisabilitiesType"].ToString().Split(',');
                    personDisabilitySubTypeDetails.Delete(Convert.ToInt32(createPerson.Person_Id));

                    foreach (string i in selectedDisabilityTypeArray)
                    {
                        personDisabilitySubTypeDetails.Create(Convert.ToInt32(i), createPerson.Person_Id);
                    }

                    if (intakeClientViewModel.PostedDisabilitySubType != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs)
                        {
                            personDisabilitySubTypeDetails.Create(Convert.ToInt32(item), createPerson.Person_Id);
                        }
                    }
                }


                #endregion


                return RedirectToAction("Index", "Intake");
            }

            return View(intakeDataViewModel);
        }

        public ActionResult CreateNewClient(string id)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userId = -1;
            var userName = string.Empty;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                userName = currentUser.User_Name;
            }

            var clientModel = new ClientModel();
            var newClient = clientModel.CreateClient(int.Parse(id), DateTime.Now, userName, true, false);

            if (newClient == null)
            {
                return View("Home");
            }

            // Assign Client Reference Number
            var assignToProvinceId = -1;

            var clientPhysicalAddress = newClient.Person == null ? null : newClient.Person.Addresses.FirstOrDefault(x => x.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress));

            if (clientPhysicalAddress != null && (clientPhysicalAddress.Town != null))
            {
                // Try to get the province based on the client's physical address
                assignToProvinceId = clientPhysicalAddress.Town.Local_Municipality.District.Province_Id;
            }
            else
            {
                // Client is not assigned a province, use the current logged-in user's province (Assumption: the client would be the same because he's reporting it in the logged-in user's province)
                var userModel = new UserModel();
                var userItem = userModel.GetSpecificUser(userId);

                if (userItem.Employees.Any())
                    assignToProvinceId = userItem.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                if (userItem.apl_Social_Worker.Any())
                    assignToProvinceId = userItem.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
            }

            var provinceModel = new ProvinceModel();
            var provinceItem = provinceModel.GetSpecificProvince(assignToProvinceId);

            if (provinceItem != null)
            {
                var nextSequenceNumber = clientModel.GetClientSequenceNumber(provinceItem.Abbreviation.ToUpper(), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));

                var clientRefNumber = "INT/" + provinceItem.Abbreviation + "/" + nextSequenceNumber.ToString(CultureInfo.InvariantCulture).PadLeft(8, '0') + "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                var isUnique = clientModel.IsReferenceNumberUnique(clientRefNumber);
                if (isUnique == null)
                {
                    // Handle Error
                }
                else
                {
                    newClient = clientModel.AssignClientReferenceNumber(newClient.Client_Id, clientRefNumber);
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Assign Client Reference No. "+ newClient.Reference_Number, "Intake");
                }
            }

            return RedirectToAction("CreateClient", new { id = newClient.Client_Id });
        }

        public ActionResult CreateClient(string id)
        {
            var intakeClientViewModel = new IntakeClientViewModel()
            {
                Client = new Client(),
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address(),
                EducationItems = new List<Person_Education>(),
                EmploymentItems = new List<Person_Employment>(),
                IntakeAssessment = new Intake_Assessment() { Intake_Assessment_Id = -1 }
            };

            var clientModel = new ClientModel();
            var loadClient = clientModel.GetSpecificClient(int.Parse(id));

            intakeClientViewModel.Client = loadClient;
            intakeClientViewModel.Person = loadClient.Person;
            intakeClientViewModel.PhysicalAddress = loadClient.Person.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? loadClient.Person.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
            intakeClientViewModel.PostalAddress = loadClient.Person.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? loadClient.Person.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();
            intakeClientViewModel.EducationItems.AddRange(loadClient.Person.Person_Education_Items);
            intakeClientViewModel.EmploymentItems.AddRange(loadClient.Person.int_Person_Employment);


            return View(intakeClientViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateClient(IntakeClientViewModel intakeClientViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = (User)Session["CurrentUser"];

                var userName = string.Empty;

                if (currentUser != null)
                {
                    userName = currentUser.User_Name;
                }

                var dateCreated = DateTime.Now;
                var dateLastModified = DateTime.Now;
                var createdBy = userName;
                var modifiedBy = userName;
                const bool isActive = true;
                const bool isDeleted = false;

                // First Update Person Details, it might have changed
                var personModel = new PersonModel();

                var updatedPerson = personModel.EditPerson(intakeClientViewModel.Person.Person_Id, intakeClientViewModel.Person.First_Name, intakeClientViewModel.Person.Last_Name, 
                    intakeClientViewModel.Person.Known_As, intakeClientViewModel.Person.Identification_Type_Id, intakeClientViewModel.Person.Identification_Number, intakeClientViewModel.Person.Date_Of_Birth, 
                    intakeClientViewModel.Person.Age, intakeClientViewModel.Person.Is_Estimated_Age, intakeClientViewModel.Person.Sexual_Orientation_Id, intakeClientViewModel.Person.Language_Id, intakeClientViewModel.Person.Gender_Id, 
                    intakeClientViewModel.Person.Marital_Status_Id, intakeClientViewModel.Person.Religion_Id, intakeClientViewModel.Person.Preferred_Contact_Type_Id, intakeClientViewModel.Person.Phone_Number,
                    intakeClientViewModel.Person.Mobile_Phone_Number, intakeClientViewModel.Person.Email_Address, intakeClientViewModel.Person.Population_Group_Id, intakeClientViewModel.Person.Nationality_Id,
                    intakeClientViewModel.Person.Disability_Type_Id, intakeClientViewModel.Person.Citizenship_Id,dateLastModified, modifiedBy, 
                    isActive, isDeleted);
                
                if (updatedPerson == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(intakeClientViewModel);
                }
                else
                {
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Create Client Reference No. " , "Intake");
                }

                // Update Address details
                var addressModel = new AddressModel();

                var updatedPhysicalAddress = addressModel.EditAddress(intakeClientViewModel.PhysicalAddress.Address_Id, (int)AddressTypeEnum.PhysicalAddress, intakeClientViewModel.PhysicalAddress.Address_Line_1, intakeClientViewModel.PhysicalAddress.Address_Line_2, intakeClientViewModel.PhysicalAddress.Town_Id, intakeClientViewModel.PhysicalAddress.Postal_Code);
                var updatedPostalAddress = addressModel.EditAddress(intakeClientViewModel.PostalAddress.Address_Id, (int)AddressTypeEnum.PostalAddress, intakeClientViewModel.PostalAddress.Address_Line_1, intakeClientViewModel.PostalAddress.Address_Line_2, intakeClientViewModel.PostalAddress.Town_Id, intakeClientViewModel.PostalAddress.Postal_Code);

                if ((updatedPhysicalAddress == null) || (updatedPostalAddress == null))
                {
                    ViewBag.Message = "An Error Occurred, Please contact Support";
                    return View(intakeClientViewModel);
                }

                var intakeAssessment = intakeClientViewModel.IntakeAssessment;

                var intakeAssessmentModel = new IntakeAssessmentModel();

                if (intakeAssessment.Intake_Assessment_Id == -1)
                {
                    // Create new Assessment
                    var newIntakeAssessment = intakeAssessmentModel.CreateIntakeAssessment(intakeClientViewModel.Client.Client_Id, intakeAssessment.Assessment_Date, intakeAssessment.Assessed_By_Id, intakeAssessment.Case_Manager_Supervisor_Id,
                        intakeAssessment.Case_Manager_Id, intakeAssessment.Preliminary_Assessment, intakeAssessment.Presenting_Problem, intakeAssessment.Problem_Sub_Category_Id, intakeAssessment.Is_Priority_Intervention,
                        intakeAssessment.Is_Referred_For_Assessment, intakeAssessment.Is_Referred_To_Other_Service_Provider, intakeAssessment.Is_Closed, intakeAssessment.ClosedDate, intakeAssessment.Treatment_Date,
                        intakeAssessment.Case_Background, intakeAssessment.Supervisor_Comments, intakeAssessment.Social_Worker_Comments, intakeAssessment.Referred_From_Organization_Id, intakeAssessment.Referred_To_Organization_Id,
                        intakeAssessment.Case_Allocation_Comments, intakeAssessment.Date_Allocated, intakeAssessment.Date_Due, isActive, isDeleted, dateCreated, createdBy);

                    if (newIntakeAssessment == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact support";
                        return View(intakeClientViewModel);
                    }
                    else
                    {
                        //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                        //Change task performed and module name
                        var auditTrail = new AuditTrailModel();
                        auditTrail.InsertAuditTrail(userName, "Create Client", "Intake");
                        
                    }

                    // Link selected referral focus areas
                    var referralFocusAreaIds = new List<int>();
                    if (intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas != null)
                    {
                        foreach (var focusAreaId in intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas.Referral_Focus_Area_IDs)
                        {
                            var focusAreaIdValue = int.Parse(focusAreaId);
                            referralFocusAreaIds.Add(focusAreaIdValue);
                        }
                    }

                    intakeAssessmentModel.AddReferralFocusAreaToIntakeAssessment(newIntakeAssessment.Intake_Assessment_Id, referralFocusAreaIds);

                    return RedirectToAction("Index", "Intake");
                }
                else
                {
                    // Update existing Intake Assessment
                    var editedIntakeAssessment = intakeAssessmentModel.EditIntakeAssessment(intakeAssessment.Intake_Assessment_Id, intakeClientViewModel.Client.Client_Id, intakeAssessment.Assessment_Date, intakeAssessment.Assessed_By_Id, intakeAssessment.Case_Manager_Supervisor_Id,
                        intakeAssessment.Case_Manager_Id, intakeAssessment.Preliminary_Assessment, intakeAssessment.Presenting_Problem, intakeAssessment.Problem_Sub_Category_Id, intakeAssessment.Is_Priority_Intervention,
                        intakeAssessment.Is_Referred_For_Assessment, intakeAssessment.Is_Referred_To_Other_Service_Provider, intakeAssessment.Is_Closed, intakeAssessment.ClosedDate, intakeAssessment.Treatment_Date,
                        intakeAssessment.Case_Background, intakeAssessment.Supervisor_Comments, intakeAssessment.Social_Worker_Comments, intakeAssessment.Referred_From_Organization_Id, intakeAssessment.Referred_To_Organization_Id,
                        intakeAssessment.Case_Allocation_Comments, intakeAssessment.Date_Allocated, intakeAssessment.Date_Due, isActive, isDeleted, dateLastModified, modifiedBy);

                    if (editedIntakeAssessment == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact support";
                        return View(intakeClientViewModel);
                    }

                    // Link selected referral focus areas
                    var referralFocusAreaIds = new List<int>();
                    if (intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas != null)
                    {
                        foreach (var focusAreaId in intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas.Referral_Focus_Area_IDs)
                        {
                            var focusAreaIdValue = int.Parse(focusAreaId);
                            referralFocusAreaIds.Add(focusAreaIdValue);
                        }
                    }

                    intakeAssessmentModel.AddReferralFocusAreaToIntakeAssessment(editedIntakeAssessment.Intake_Assessment_Id, referralFocusAreaIds);

                    return RedirectToAction("Index", "Intake");
                }
            }

            return View(intakeClientViewModel);
        }

        public ActionResult Edit(string id)
        {
            var intakeDataViewModel = new IntakeDataViewModel();

            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(int.Parse(id));
            Session["Pers_Id"] = personToEdit.Person_Id;
            var receptionRegister = personToEdit.Reception_Registers.Any() ? personToEdit.Reception_Registers.First() : new Reception_Register();

            var physicalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
            var postalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();

            if (physicalAddress.Town != null)
            {
                physicalAddress.Selected_Local_Municipality_Id = physicalAddress.Town.Local_Municipality.Local_Municipality_Id;
                physicalAddress.Selected_Municipality_Id = physicalAddress.Town.Local_Municipality.District_Municipality_Id;
                physicalAddress.Selected_Province_Id = physicalAddress.Town.Local_Municipality.District.Province_Id;
            }

            if (postalAddress.Town != null)
            {
                postalAddress.Selected_Local_Municipality_Id = postalAddress.Town.Local_Municipality.Local_Municipality_Id;
                postalAddress.Selected_Municipality_Id = postalAddress.Town.Local_Municipality.District_Municipality_Id;
                postalAddress.Selected_Province_Id = postalAddress.Town.Local_Municipality.District.Province_Id;
            }

            intakeDataViewModel.Person = personToEdit;
            intakeDataViewModel.ReceptionRegister = receptionRegister;
            intakeDataViewModel.PhysicalAddress = physicalAddress;
            intakeDataViewModel.PostalAddress = postalAddress;

            #region disability multiselct Get
            var intakeClientViewModel = new IntakeClientViewModel();

            var disabilityType = new DisabilityModel();
            intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();


            if (intakeDataViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(personToEdit.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();
                intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs = new int[intakeClientViewModel.SelectedDisabilityType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilityType)
                {
                    intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs[index] = item.Disability_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(-1);
            }

            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilityType = new List<SelectListItem>();


            foreach (var item in disabilityType.GetListOfDisabilities())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs.Contains(item.Disability_Id))
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

            intakeClientViewModel.PostedDisabilityType.ListOfDisabilityTypeIDs = new SelectList(AvailableDisabilityType, "Value", "Text");
            #endregion

            #region Sub disability type

            var disabilitySubType = new DisabilitySubTypeModel();
            intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();


            if (intakeDataViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(intakeDataViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();
                intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs = new int[intakeClientViewModel.SelectedDisabilitySubType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilitySubType)
                {
                    intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs[index] = item.DisabilityType_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(-1);
            }


            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilitySubType = new List<SelectListItem>();


            foreach (var item in disabilitySubType.GetListOfDisabilitiesType())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs.Contains(item.DisabilityType_Id))
                    {
                        itemSelected = true;
                    }
                }

                AvailableDisabilitySubType.Add(new SelectListItem
                {
                    Text = item.TypeName,
                    Value = item.DisabilityType_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableDisabilitySubType = AvailableDisabilitySubType;

            intakeClientViewModel.PostedDisabilitySubType.ListOfDisabilitySubTypeIDs = new SelectList(AvailableDisabilitySubType, "Value", "Text");
            #endregion

            return View(intakeDataViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IntakeDataViewModel intakeDataViewModel, FormCollection data)
        {
            if (ModelState.IsValid)
            {
                var currentUser = (User)Session["CurrentUser"];

                var userName = string.Empty;

                if (currentUser != null)
                {
                    userName = currentUser.User_Name;
                }

                var dateLastModified = DateTime.Now;
                var modifiedBy = userName;
                const bool isActive = true;
                const bool isDeleted = false;

                // Person
                intakeDataViewModel.Person.Person_Id = Convert.ToInt32(Session["Pers_Id"]);
                var personModel = new PersonModel();

                var updatedPerson = personModel.EditPerson(intakeDataViewModel.Person.Person_Id, intakeDataViewModel.Person.First_Name, intakeDataViewModel.Person.Last_Name,
                    intakeDataViewModel.Person.Known_As, intakeDataViewModel.Person.Identification_Type_Id, intakeDataViewModel.Person.Identification_Number,
                    intakeDataViewModel.Person.Date_Of_Birth, intakeDataViewModel.Person.Age, 
                    intakeDataViewModel.Person.Is_Estimated_Age, intakeDataViewModel.Person.Sexual_Orientation_Id, intakeDataViewModel.Person.Language_Id, 
                    intakeDataViewModel.Person.Gender_Id, intakeDataViewModel.Person.Marital_Status_Id, 
                    intakeDataViewModel.Person.Religion_Id, intakeDataViewModel.Person.Preferred_Contact_Type_Id, 
                    intakeDataViewModel.Person.Phone_Number, intakeDataViewModel.Person.Mobile_Phone_Number, 
                    intakeDataViewModel.Person.Email_Address, intakeDataViewModel.Person.Population_Group_Id, 
                    intakeDataViewModel.Person.Nationality_Id, intakeDataViewModel.Person.Disability_Type_Id, intakeDataViewModel.Person.Citizenship_Id,
                    dateLastModified, modifiedBy, isActive, isDeleted);

                if (updatedPerson == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(intakeDataViewModel);
                }
                else
                {
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Edit Person Details", "Intake");
                    
                }
                personModel.AddAddress(updatedPerson.Person_Id, (int)AddressTypeEnum.PhysicalAddress, intakeDataViewModel.PhysicalAddress.Address_Line_1, intakeDataViewModel.PhysicalAddress.Address_Line_2, intakeDataViewModel.PhysicalAddress.Town_Id, intakeDataViewModel.PhysicalAddress.Postal_Code);
                personModel.AddAddress(updatedPerson.Person_Id, (int)AddressTypeEnum.PostalAddress, intakeDataViewModel.PostalAddress.Address_Line_1, intakeDataViewModel.PostalAddress.Address_Line_2, intakeDataViewModel.PostalAddress.Town_Id, intakeDataViewModel.PostalAddress.Postal_Code);

                // Reception Register (not all items will have reception registers, because they're not necessarily clients)
                if (intakeDataViewModel.ReceptionRegister.Reception_Register_Id != 0)
                {
                    var receptionRegisterModel = new ReceptionRegisterModel();

                    var updatedReceptionRegister = receptionRegisterModel.EditReceptionRegister(intakeDataViewModel.ReceptionRegister.Reception_Register_Id, updatedPerson.Person_Id, intakeDataViewModel.ReceptionRegister.Reason_For_Visit, intakeDataViewModel.ReceptionRegister.Reception_Visit_Type_Id, intakeDataViewModel.ReceptionRegister.Visit_Date, intakeDataViewModel.ReceptionRegister.Reception_Action_Taken_Id, dateLastModified, modifiedBy, isActive, isDeleted);

                    if (updatedReceptionRegister == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact Support";
                        return View(intakeDataViewModel);
                    }
                    else
                    {
                        //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                        //Change task performed and module name
                        var auditTrail = new AuditTrailModel();
                        auditTrail.InsertAuditTrail(userName, "Edit Reception Register", "Intake");
                        
                    }
                }

                // Addresses
                var addressModel = new AddressModel();

                var updatedPhysicalAddress = addressModel.EditAddress(intakeDataViewModel.PhysicalAddress.Address_Id, (int)AddressTypeEnum.PhysicalAddress, intakeDataViewModel.PhysicalAddress.Address_Line_1, intakeDataViewModel.PhysicalAddress.Address_Line_2, intakeDataViewModel.PhysicalAddress.Town_Id, intakeDataViewModel.PhysicalAddress.Postal_Code);
                var updatedPostalAddress = addressModel.EditAddress(intakeDataViewModel.PostalAddress.Address_Id, (int)AddressTypeEnum.PostalAddress, intakeDataViewModel.PostalAddress.Address_Line_1, intakeDataViewModel.PostalAddress.Address_Line_2, intakeDataViewModel.PostalAddress.Town_Id, intakeDataViewModel.PostalAddress.Postal_Code);

                if ((updatedPhysicalAddress == null) || (updatedPostalAddress == null))
                {
                    ViewBag.Message = "An Error Occurred, Please contact Support";
                    return View(intakeDataViewModel);
                }

                #region disability multiselect post
                var intakeClientViewModel = new IntakeClientViewModel();

                var value = data["SelectedDisabilities"];
                var personDisabilityDetails = new PersonDisabilityModel();

                if (value!=null)
                {
                    string[] selectedDisabilityArray = data["SelectedDisabilities"].ToString().Split(',');
                    personDisabilityDetails.Delete(Convert.ToInt32(updatedPerson.Person_Id));

                    foreach (string i in selectedDisabilityArray)
                    {
                        personDisabilityDetails.Create(Convert.ToInt32(i), updatedPerson.Person_Id);
                    }

                    if (intakeClientViewModel.PostedDisabilityType != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs)
                        {
                            personDisabilityDetails.Create(Convert.ToInt32(item), updatedPerson.Person_Id);
                        }
                    }

                }


                #endregion

                #region disability Sub Type multiselect post
                var values = data["SelectedDisabilitiesType"];
                var personDisabilitySubTypeDetails = new PersonDisabilityTypeModel();
                if (values != null)
                {
                    string[] selectedDisabilityTypeArray = data["SelectedDisabilitiesType"].ToString().Split(',');
                    personDisabilitySubTypeDetails.Delete(Convert.ToInt32(updatedPerson.Person_Id));

                    foreach (string i in selectedDisabilityTypeArray)
                    {
                        personDisabilitySubTypeDetails.Create(Convert.ToInt32(i), updatedPerson.Person_Id);
                    }

                    if (intakeClientViewModel.PostedDisabilitySubType != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs)
                        {
                            personDisabilitySubTypeDetails.Create(Convert.ToInt32(item), updatedPerson.Person_Id);
                        }
                    }
                }


                #endregion

                return RedirectToAction("Index", "Intake");
            }

            return View(intakeDataViewModel);
        }

        public ActionResult EditClient(string id)
        {
            var intakeClientViewModel = new IntakeClientViewModel()
            {
                Client = new Client(),
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address(),
                EducationItems = new List<Person_Education>(),
                EmploymentItems = new List<Person_Employment>(),
                MedicalConditionItems = new List<IntakeMedicalConditionItem>(),
                IntakeAssessment = new Intake_Assessment()
            };

            var personModel = new PersonModel();
            var personToEdit = personModel.GetSpecificPerson(int.Parse(id));

            var physicalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
            var postalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();

            var intakeAssessment = personToEdit.Clients.Any() ? personToEdit.Clients.First().Intake_Assessments.FirstOrDefault() : null;

            intakeClientViewModel.Person = personToEdit;
            intakeClientViewModel.Client = personToEdit.Clients.Any() ? personToEdit.Clients.First() : new Client();
            intakeClientViewModel.PhysicalAddress = physicalAddress;
            intakeClientViewModel.PostalAddress = postalAddress;
            intakeClientViewModel.IntakeAssessment = intakeAssessment ?? new Intake_Assessment() { Intake_Assessment_Id = -1 };

            if ((intakeClientViewModel.IntakeAssessment.Intake_Assessment_Id != -1) && (intakeClientViewModel.IntakeAssessment.Problem_Sub_Category != null))
            {
                intakeClientViewModel.IntakeAssessment.Problem_Category_Id = intakeClientViewModel.IntakeAssessment.Problem_Sub_Category.Problem_Category_Id;
            }

            return View(intakeClientViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditClient(IntakeClientViewModel intakeClientViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = (User)Session["CurrentUser"];

                var userName = string.Empty;

                if (currentUser != null)
                {
                    userName = currentUser.User_Name;
                }

                var dateCreated = DateTime.Now;
                var dateLastModified = DateTime.Now;
                var createdBy = userName;
                var modifiedBy = userName;
                const bool isActive = true;
                const bool isDeleted = false;

                // First Update Person Details, it might have changed
                var personModel = new PersonModel();

                var updatedPerson = personModel.EditPerson(intakeClientViewModel.Person.Person_Id, intakeClientViewModel.Person.First_Name, intakeClientViewModel.Person.Last_Name, intakeClientViewModel.Person.Known_As, intakeClientViewModel.Person.Identification_Type_Id, intakeClientViewModel.Person.Identification_Number, intakeClientViewModel.Person.Date_Of_Birth, intakeClientViewModel.Person.Age, intakeClientViewModel.Person.Is_Estimated_Age, intakeClientViewModel.Person.Sexual_Orientation_Id, intakeClientViewModel.Person.Language_Id, intakeClientViewModel.Person.Gender_Id, intakeClientViewModel.Person.Marital_Status_Id, intakeClientViewModel.Person.Religion_Id, intakeClientViewModel.Person.Preferred_Contact_Type_Id, intakeClientViewModel.Person.Phone_Number, intakeClientViewModel.Person.Mobile_Phone_Number, intakeClientViewModel.Person.Email_Address, intakeClientViewModel.Person.Population_Group_Id, intakeClientViewModel.Person.Nationality_Id, intakeClientViewModel.Person.Disability_Type_Id, intakeClientViewModel.Person.Citizenship_Id,dateLastModified, modifiedBy, isActive, isDeleted);

                if (updatedPerson == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(intakeClientViewModel);
                }
                else
                {
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Edit Client", "Intake");
                    
                }

                // Update Address details
                var addressModel = new AddressModel();

                var updatedPhysicalAddress = addressModel.EditAddress(intakeClientViewModel.PhysicalAddress.Address_Id, (int)AddressTypeEnum.PhysicalAddress, intakeClientViewModel.PhysicalAddress.Address_Line_1, intakeClientViewModel.PhysicalAddress.Address_Line_2, intakeClientViewModel.PhysicalAddress.Town_Id, intakeClientViewModel.PhysicalAddress.Postal_Code);
                var updatedPostalAddress = addressModel.EditAddress(intakeClientViewModel.PostalAddress.Address_Id, (int)AddressTypeEnum.PostalAddress, intakeClientViewModel.PostalAddress.Address_Line_1, intakeClientViewModel.PostalAddress.Address_Line_2, intakeClientViewModel.PostalAddress.Town_Id, intakeClientViewModel.PostalAddress.Postal_Code);

                if ((updatedPhysicalAddress == null) || (updatedPostalAddress == null))
                {
                    ViewBag.Message = "An Error Occurred, Please contact Support";
                    return View(intakeClientViewModel);
                }

                var intakeAssessment = intakeClientViewModel.IntakeAssessment;

                var intakeAssessmentModel = new IntakeAssessmentModel();

                if (intakeAssessment.Intake_Assessment_Id == -1)
                {
                    // Create new Assessment
                    var newIntakeAssessment = intakeAssessmentModel.CreateIntakeAssessment(intakeClientViewModel.Client.Client_Id, intakeAssessment.Assessment_Date, intakeAssessment.Assessed_By_Id, intakeAssessment.Case_Manager_Supervisor_Id,
                        intakeAssessment.Case_Manager_Id, intakeAssessment.Preliminary_Assessment, intakeAssessment.Presenting_Problem, intakeAssessment.Problem_Sub_Category_Id, intakeAssessment.Is_Priority_Intervention,
                        intakeAssessment.Is_Referred_For_Assessment, intakeAssessment.Is_Referred_To_Other_Service_Provider, intakeAssessment.Is_Closed, intakeAssessment.ClosedDate, intakeAssessment.Treatment_Date,
                        intakeAssessment.Case_Background, intakeAssessment.Supervisor_Comments, intakeAssessment.Social_Worker_Comments, intakeAssessment.Referred_From_Organization_Id, intakeAssessment.Referred_To_Organization_Id,
                        intakeAssessment.Case_Allocation_Comments, intakeAssessment.Date_Allocated, intakeAssessment.Date_Due, isActive, isDeleted, dateCreated, createdBy);

                    if (newIntakeAssessment == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact support";
                        return View(intakeClientViewModel);
                    }

                    // Link selected referral focus areas
                    var referralFocusAreaIds = new List<int>();
                    if (intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas != null)
                    {
                        foreach (var focusAreaId in intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas.Referral_Focus_Area_IDs)
                        {
                            var focusAreaIdValue = int.Parse(focusAreaId);
                            referralFocusAreaIds.Add(focusAreaIdValue);
                        }
                    }

                    intakeAssessmentModel.AddReferralFocusAreaToIntakeAssessment(newIntakeAssessment.Intake_Assessment_Id, referralFocusAreaIds);

                    return RedirectToAction("Index", "Intake");
                }
                else
                {
                    // Update existing Intake Assessment
                    var editedIntakeAssessment = intakeAssessmentModel.EditIntakeAssessment(intakeAssessment.Intake_Assessment_Id, intakeClientViewModel.Client.Client_Id, intakeAssessment.Assessment_Date, intakeAssessment.Assessed_By_Id, intakeAssessment.Case_Manager_Supervisor_Id,
                        intakeAssessment.Case_Manager_Id, intakeAssessment.Preliminary_Assessment, intakeAssessment.Presenting_Problem, intakeAssessment.Problem_Sub_Category_Id, intakeAssessment.Is_Priority_Intervention,
                        intakeAssessment.Is_Referred_For_Assessment, intakeAssessment.Is_Referred_To_Other_Service_Provider, intakeAssessment.Is_Closed, intakeAssessment.ClosedDate, intakeAssessment.Treatment_Date,
                        intakeAssessment.Case_Background, intakeAssessment.Supervisor_Comments, intakeAssessment.Social_Worker_Comments, intakeAssessment.Referred_From_Organization_Id, intakeAssessment.Referred_To_Organization_Id,
                        intakeAssessment.Case_Allocation_Comments, intakeAssessment.Date_Allocated, intakeAssessment.Date_Due, isActive, isDeleted, dateLastModified, modifiedBy);

                    if (editedIntakeAssessment == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact support";
                        return View(intakeClientViewModel);
                    }

                    // Link selected referral focus areas
                    var referralFocusAreaIds = new List<int>();
                    if (intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas != null)
                    {
                        foreach (var focusAreaId in intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas.Referral_Focus_Area_IDs)
                        {
                            var focusAreaIdValue = int.Parse(focusAreaId);
                            referralFocusAreaIds.Add(focusAreaIdValue);
                        }
                    }

                    intakeAssessmentModel.AddReferralFocusAreaToIntakeAssessment(editedIntakeAssessment.Intake_Assessment_Id, referralFocusAreaIds);

                    return RedirectToAction("Index", "Intake");
                }
            }

            return View(intakeClientViewModel);
        }
        [HttpGet]
        public ActionResult CreateNewAssessment(string id)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userId = -1;
            var userName = string.Empty;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
                userName = currentUser.User_Name;
            }

            var personModel = new PersonModel();
            var person = personModel.GetSpecificPerson(int.Parse(id));

            if (person == null)
                return View("Home");

            // Is there already client details assigned to this person?
            if (!person.Clients.Any())
            {
                // Create Client and link Assessment
                var clientModel = new ClientModel();
                var newClient = clientModel.CreateClient(int.Parse(id), DateTime.Now, userName, true, false);

                if (newClient == null)
                {
                    return View("Home");
                }

                // Assign Client Reference Number
                var assignToProvinceId = -1;

                var clientPhysicalAddress = newClient.Person == null ? null : newClient.Person.Addresses.FirstOrDefault(x => x.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress));

                if (clientPhysicalAddress != null && (clientPhysicalAddress.Town != null))
                {
                    // Try to get the province based on the client's physical address
                    assignToProvinceId = clientPhysicalAddress.Town.Local_Municipality.District.Province_Id;
                }
                else
                {
                    // Client is not assigned a province, use the current logged-in user's province (Assumption: the client would be the same because he's reporting it in the logged-in user's province)
                    var userModel = new UserModel();
                    var userItem = userModel.GetSpecificUser(userId);

                    assignToProvinceId = userItem.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;
                }

                var provinceModel = new ProvinceModel();
                var provinceItem = provinceModel.GetSpecificProvince(assignToProvinceId);

                if (provinceItem != null)
                {
                    var nextSequenceNumber = clientModel.GetClientSequenceNumber(provinceItem.Abbreviation.ToUpper(), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));

                    var clientRefNumber = "INT/" + provinceItem.Abbreviation + "/" + nextSequenceNumber.ToString(CultureInfo.InvariantCulture).PadLeft(8, '0') + "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                    var isUnique = clientModel.IsReferenceNumberUnique(clientRefNumber);
                    if (isUnique == null)
                    {
                        // Handle Error
                    }
                    else
                    {
                        newClient = clientModel.AssignClientReferenceNumber(newClient.Client_Id, clientRefNumber);
                    }
                }
                // Kholo Update

                // 1. Get SuperVisor ServicePoint
                // userId
                var emplyeeModel = new EmployeeModel();
                //var temp = emplyeeModel.GetListOfEmployeesByCaseManagerList(false,false,userId);

                // 2. Populate dropdown

                ViewBag.CaseManagerSuperVisor = emplyeeModel.GetListOfEmployeesBySupervisorList(false, false, userId);
                //emplyeeModel.GetListOfEmployeesInServicePoint(false, false, userId); ;
                return RedirectToAction("CreateAssessment", new { id = newClient.Client_Id });
            }
            else
            {
                var client = person.Clients.First();

                return RedirectToAction("CreateAssessment", new { id = client.Client_Id });
            }
        }

        // Kholo Update
        //Probability that it is never used
        public ActionResult CreateAssessment(string id)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }

            var intakeClientViewModel = new IntakeClientViewModel()
            {
                Client = new Client(),
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address(),
                EducationItems = new List<Person_Education>(),
                EmploymentItems = new List<Person_Employment>(),
                IntakeAssessment = new Intake_Assessment() { Intake_Assessment_Id = -1, Assessment_Date = DateTime.Now, Assessed_By_Id = userId }
            };

            var clientModel = new ClientModel();
            var loadClient = clientModel.GetSpecificClient(int.Parse(id));
            var personToEdit = loadClient.Person;

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

            intakeClientViewModel.Client = loadClient;
            intakeClientViewModel.Person = loadClient.Person;
            intakeClientViewModel.PhysicalAddress = physicalAddress;
            intakeClientViewModel.PostalAddress = postalAddress;
            intakeClientViewModel.EducationItems.AddRange(loadClient.Person.Person_Education_Items);
            intakeClientViewModel.EmploymentItems.AddRange(loadClient.Person.int_Person_Employment);

            #region disability multiselct Get
            var disabilityType = new DisabilityModel();
            intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();


            if (intakeClientViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(intakeClientViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();
                intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs = new int[intakeClientViewModel.SelectedDisabilityType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilityType)
                {
                    intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs[index] = item.Disability_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(-1);
            }

            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilityType = new List<SelectListItem>();


            foreach (var item in disabilityType.GetListOfDisabilities())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs.Contains(item.Disability_Id))
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

            intakeClientViewModel.PostedDisabilityType.ListOfDisabilityTypeIDs = new SelectList(AvailableDisabilityType, "Value", "Text");
            #endregion

            #region Sub disability type

            var disabilitySubType = new DisabilitySubTypeModel();
            intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();


            if (intakeClientViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(intakeClientViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();
                intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs = new int[intakeClientViewModel.SelectedDisabilitySubType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilitySubType)
                {
                    intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs[index] = item.DisabilityType_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(-1);
            }


            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilitySubType = new List<SelectListItem>();


            foreach (var item in disabilitySubType.GetListOfDisabilitiesType())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs.Contains(item.DisabilityType_Id))
                    {
                        itemSelected = true;
                    }
                }

                AvailableDisabilitySubType.Add(new SelectListItem
                {
                    Text = item.TypeName,
                    Value = item.DisabilityType_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableDisabilitySubType = AvailableDisabilitySubType;

            intakeClientViewModel.PostedDisabilitySubType.ListOfDisabilitySubTypeIDs = new SelectList(AvailableDisabilitySubType, "Value", "Text");
            #endregion


            // Kholo Update

            // 1. Get SuperVisor ServicePoint
            // userId
            var emplyeeModel = new EmployeeModel();
            //var temp = emplyeeModel.GetListOfEmployeesByCaseManagerList(false,false,userId);

            // 2. Populate dropdown

            ViewBag.CaseManagerSuperVisor = emplyeeModel.GetListOfEmployeesBySupervisorList(false, false, userId);
            //emplyeeModel.GetListOfEmployeesInServicePoint(false, false, userId); ;


                     
            return View(intakeClientViewModel);
        }

        // kholo update
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAssessment(IntakeClientViewModel intakeClientViewModel, FormCollection data)
        {
            intakeClientViewModel.IntakeAssessment.Case_Manager_Supervisor_Id = intakeClientViewModel.IntakeAssessment.Case_Manager_Supervisor_Id;

            if (ModelState.IsValid)
            {
                var currentUser = (User)Session["CurrentUser"];

                var userName = string.Empty;

                if (currentUser != null)
                {
                    userName = currentUser.User_Name;
                }
                if (userName != null)
                {
                    UserModel uModel = new UserModel();
                    int? CurrentUserId = uModel.GetSpecificUser(userName).User_Id;
                }
                var dateCreated = DateTime.Now;
                var dateLastModified = DateTime.Now;
                var createdBy = userName;
                var modifiedBy = userName;
                const bool isActive = true;
                const bool isDeleted = false;

                // First Update Person Details, it might have changed
                var personModel = new PersonModel();

                var updatedPerson = personModel.EditPerson(intakeClientViewModel.Person.Person_Id, intakeClientViewModel.Person.First_Name,
                    intakeClientViewModel.Person.Last_Name, intakeClientViewModel.Person.Known_As, intakeClientViewModel.Person.Identification_Type_Id,
                    intakeClientViewModel.Person.Identification_Number, intakeClientViewModel.Person.Date_Of_Birth, intakeClientViewModel.Person.Age,
                    intakeClientViewModel.Person.Is_Estimated_Age, intakeClientViewModel.Person.Sexual_Orientation_Id, intakeClientViewModel.Person.Language_Id, intakeClientViewModel.Person.Gender_Id, 
                    intakeClientViewModel.Person.Marital_Status_Id, intakeClientViewModel.Person.Religion_Id, intakeClientViewModel.Person.Preferred_Contact_Type_Id, 
                    intakeClientViewModel.Person.Phone_Number, intakeClientViewModel.Person.Mobile_Phone_Number, intakeClientViewModel.Person.Email_Address, 
                    intakeClientViewModel.Person.Population_Group_Id, intakeClientViewModel.Person.Nationality_Id, intakeClientViewModel.Person.Disability_Type_Id, intakeClientViewModel.Person.Citizenship_Id,
                    dateLastModified, modifiedBy, isActive, isDeleted);
                

                if (updatedPerson == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(intakeClientViewModel);
                }
                else
                {
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Create Intake assesment", "Intake");
                }

                // Update Address details
                var addressModel = new AddressModel();

                var updatedPhysicalAddress = addressModel.EditAddress(intakeClientViewModel.PhysicalAddress.Address_Id, (int)AddressTypeEnum.PhysicalAddress, intakeClientViewModel.PhysicalAddress.Address_Line_1, intakeClientViewModel.PhysicalAddress.Address_Line_2, intakeClientViewModel.PhysicalAddress.Town_Id, intakeClientViewModel.PhysicalAddress.Postal_Code);
                var updatedPostalAddress = addressModel.EditAddress(intakeClientViewModel.PostalAddress.Address_Id, (int)AddressTypeEnum.PostalAddress, intakeClientViewModel.PostalAddress.Address_Line_1, intakeClientViewModel.PostalAddress.Address_Line_2, intakeClientViewModel.PostalAddress.Town_Id, intakeClientViewModel.PostalAddress.Postal_Code);

                if ((updatedPhysicalAddress == null) || (updatedPostalAddress == null))
                {
                    
                    ViewBag.Message = "An Error Occurred, Please contact Support";
                    return View(intakeClientViewModel);
                }

                var intakeAssessment = intakeClientViewModel.IntakeAssessment;

                var intakeAssessmentModel = new IntakeAssessmentModel();

                #region disability category
                var value = data["SelectedDisabilities"];
                var personDisabilityDetails = new PersonDisabilityModel();
                if (value != null)
                {
                    string[] selectedDisabilityArray = data["SelectedDisabilities"].ToString().Split(',');
                    personDisabilityDetails.Delete(Convert.ToInt32(updatedPerson.Person_Id));


                    foreach (string i in selectedDisabilityArray)
                    {
                        personDisabilityDetails.Create(Convert.ToInt32(i), intakeClientViewModel.Person.Person_Id);
                    }

                    if (intakeClientViewModel.PostedPresentationCondition != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedPresentationCondition.PresentationConditionIDs)
                        {
                            personDisabilityDetails.Create(Convert.ToInt32(item), intakeClientViewModel.Person.Person_Id);
                        }
                    }

                }

                #endregion


                #region disability Sub Type multiselect post
                var values = data["SelectedDisabilitiesType"];
                var personDisabilitySubTypeDetails = new PersonDisabilityTypeModel();
                if (values != null)
                {
                    string[] selectedDisabilityTypeArray = data["SelectedDisabilitiesType"].ToString().Split(',');
                    personDisabilitySubTypeDetails.Delete(Convert.ToInt32(updatedPerson.Person_Id));

                    foreach (string i in selectedDisabilityTypeArray)
                    {
                        personDisabilitySubTypeDetails.Create(Convert.ToInt32(i), updatedPerson.Person_Id);
                    }

                    if (intakeClientViewModel.PostedDisabilitySubType != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs)
                        {
                            personDisabilitySubTypeDetails.Create(Convert.ToInt32(item), updatedPerson.Person_Id);
                        }
                    }
                }


                #endregion

                #region Avoid Duplication inserted by Herman
                int Client_IdCheck = intakeClientViewModel.Client.Client_Id;
                int? Assessed_ByCheck = intakeAssessment.Assessed_By_Id;
                string PreLimAss = intakeAssessment.Preliminary_Assessment;
                DateTime? assesDate = intakeAssessment.Assessment_Date;
                int CheckIfExistIn_int_Intake_Assessment = intakeAssessmentModel.CheckIfAssessmentExists(Client_IdCheck, Assessed_ByCheck, PreLimAss);

               if (CheckIfExistIn_int_Intake_Assessment == 0)
                {
                    // Create new Assessment
                    var newIntakeAssessment = intakeAssessmentModel.CreateIntakeAssessment(intakeClientViewModel.Client.Client_Id, intakeAssessment.Assessment_Date, intakeAssessment.Assessed_By_Id, intakeAssessment.Case_Manager_Supervisor_Id,
                        intakeAssessment.Case_Manager_Id, intakeAssessment.Preliminary_Assessment, intakeAssessment.Presenting_Problem, intakeAssessment.Problem_Sub_Category_Id, intakeAssessment.Is_Priority_Intervention,
                        intakeAssessment.Is_Referred_For_Assessment, intakeAssessment.Is_Referred_To_Other_Service_Provider, intakeAssessment.Is_Closed, intakeAssessment.ClosedDate, intakeAssessment.Treatment_Date,
                        intakeAssessment.Case_Background, intakeAssessment.Supervisor_Comments, intakeAssessment.Social_Worker_Comments, intakeAssessment.Referred_From_Organization_Id, intakeAssessment.Referred_To_Organization_Id,
                        intakeAssessment.Case_Allocation_Comments, intakeAssessment.Date_Allocated, intakeAssessment.Date_Due, isActive, isDeleted, dateCreated, createdBy);

                    if (newIntakeAssessment == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact support";
                        return View(intakeClientViewModel);
                    }
                    else
                    {
                        // Link selected referral focus areas
                        var referralFocusAreaIds = new List<int>();
                        if (intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas != null)
                        {
                            foreach (var focusAreaId in intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas.Referral_Focus_Area_IDs)
                            {
                                var focusAreaIdValue = int.Parse(focusAreaId);
                                referralFocusAreaIds.Add(focusAreaIdValue);
                            }
                        }
                        intakeAssessmentModel.AddReferralFocusAreaToIntakeAssessment(newIntakeAssessment.Intake_Assessment_Id, referralFocusAreaIds);

                    }
                }
                if (CheckIfExistIn_int_Intake_Assessment > 0)
                {
                    RedirectToAction("Intake", "EditAssessment", new { model = intakeClientViewModel });
                }
                #endregion                           

                
                return RedirectToAction("Index", "Intake");
            }
            Session["caseMngId"] = intakeClientViewModel.IntakeAssessment.Case_Manager_Id;
            Session["caseMngSupId"] = intakeClientViewModel.IntakeAssessment.Case_Manager_Supervisor_Id;


            return View(intakeClientViewModel);
        }

        // Kholo Update
        public ActionResult EditAssessment(string id)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userId = -1;

            if (currentUser != null)
            {
                userId = currentUser.User_Id;
            }
            
            var intakeClientViewModel = new IntakeClientViewModel()
            {
                Client = new Client(),
                Person = new Person(),
                PhysicalAddress = new Address(),
                PostalAddress = new Address(),
                EducationItems = new List<Person_Education>(),
                EmploymentItems = new List<Person_Employment>(),
                MedicalConditionItems = new List<IntakeMedicalConditionItem>(),
                IntakeAssessment = new Intake_Assessment()
            };

            var assessmentModel = new IntakeAssessmentModel();
            var assessmentItem = assessmentModel.GetSpecificIntakeAssessment(int.Parse(id));
            //Session["IntAssId"] = assessmentItem.Intake_Assessment_Id;
            Session["IntAssId"] = id;
            if (assessmentItem == null)
                return RedirectToAction("Index", "Intake");

            var clientToEdit = assessmentItem.Client;
            var personToEdit = assessmentItem.Client.Person;

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




            intakeClientViewModel.Person = personToEdit;
            intakeClientViewModel.Client = clientToEdit;
            intakeClientViewModel.PhysicalAddress = physicalAddress;
            intakeClientViewModel.PostalAddress = postalAddress;
            intakeClientViewModel.IntakeAssessment = assessmentItem;

            if ((intakeClientViewModel.IntakeAssessment.Intake_Assessment_Id != -1) && (intakeClientViewModel.IntakeAssessment.Problem_Sub_Category != null))
            {
                intakeClientViewModel.IntakeAssessment.Problem_Category_Id = intakeClientViewModel.IntakeAssessment.Problem_Sub_Category.Problem_Category_Id;
            }

            #region disability multiselct Get
            var disabilityType = new DisabilityModel();
            intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();


            if (intakeClientViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(intakeClientViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilityType = new Posted_DisabilityType();
                intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs = new int[intakeClientViewModel.SelectedDisabilityType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilityType)
                {
                    intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs[index] = item.Disability_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilityType = disabilityType.GetSelectedListOfDisabilities(-1);
            }

            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilityType = new List<SelectListItem>();


            foreach (var item in disabilityType.GetListOfDisabilities())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs.Contains(item.Disability_Id))
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

            intakeClientViewModel.PostedDisabilityType.ListOfDisabilityTypeIDs = new SelectList(AvailableDisabilityType, "Value", "Text");
            #endregion

            #region Sub disability type

            var disabilitySubType = new DisabilitySubTypeModel();
            intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();


            if (intakeClientViewModel.Person != null)
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(intakeClientViewModel.Person.Person_Id);
                int index = 0;
                intakeClientViewModel.PostedDisabilitySubType = new Posted_DisabilitySubType();
                intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs = new int[intakeClientViewModel.SelectedDisabilitySubType.Count()];

                foreach (var item in intakeClientViewModel.SelectedDisabilitySubType)
                {
                    intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs[index] = item.DisabilityType_Id;
                    index++;
                }

            }
            else
            {
                intakeClientViewModel.SelectedDisabilitySubType = disabilitySubType.GetSelectedListOfDisabilitiesSubTyp(-1);
            }


            //var disabilityType = new DisabilityModel();
            List<SelectListItem> AvailableDisabilitySubType = new List<SelectListItem>();


            foreach (var item in disabilitySubType.GetListOfDisabilitiesType())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs != null)
                {
                    if (intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs.Contains(item.DisabilityType_Id))
                    {
                        itemSelected = true;
                    }
                }

                AvailableDisabilitySubType.Add(new SelectListItem
                {
                    Text = item.TypeName,
                    Value = item.DisabilityType_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableDisabilitySubType = AvailableDisabilitySubType;

            intakeClientViewModel.PostedDisabilitySubType.ListOfDisabilitySubTypeIDs = new SelectList(AvailableDisabilitySubType, "Value", "Text");
            #endregion

            // Kholo Update

            // 1. Get SuperVisor ServicePoint
            // userId
            var emplyeeModel = new EmployeeModel();
            //var temp = emplyeeModel.GetListOfEmployeesByCaseManagerList(false,false,userId);
            //Check if Supervisor is null
            bool CheckSuperIsnull = emplyeeModel.CheckIfsupervisorIsnull(intakeClientViewModel.IntakeAssessment.Intake_Assessment_Id);
            if (CheckSuperIsnull==false)
            {

                ViewBag.CaseManagerSuperVisor = emplyeeModel.GetListOfEmployeesBySupervisorList(false, false, userId);
                ViewBag.AssessedBy = assessmentModel.GetAssessedBy(assessmentItem.Intake_Assessment_Id);

            }
            else { 
            // 2. Populate dropdown
            int? ServiceId = assessmentItem.Problem_Category_Id;
            ViewBag.CaseManager = emplyeeModel.GetListOfEmployeesByCaseManagerList(false, false, userId, ServiceId);
            //ViewBag.CaseManagerSuperVisor = emplyeeModel.GetListOfEmployeesByCaseManagerList(false, false, userId);               
            ViewBag.AssessedBy = assessmentModel.GetAssessedBy(assessmentItem.Intake_Assessment_Id);
            
            Session["_CaseManager"] = emplyeeModel.GetListOfEmployeesByCaseManagerList(false, false, userId, ServiceId);
            }
            return View(intakeClientViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditAssessment(IntakeClientViewModel intakeClientViewModel, int? Case_Manager_TEMP, int? Case_Manager_Supervisor_Id_TEMP, FormCollection formmm)
        {
            SDIIS_DatabaseEntities dbObject = new SDIIS_DatabaseEntities();
            var tmpCaseManager = Session["_CaseManager"];//ViewBag.CaseManager;


            // Get specific Intake_Accessment
            var assessmentModel = new IntakeAssessmentModel();
            int? caseMngSupId = assessmentModel.GetCaseMngSupId(intakeClientViewModel.IntakeAssessment.Intake_Assessment_Id);
            int? caseMngId = assessmentModel.GetCaseMngId(intakeClientViewModel.IntakeAssessment.Intake_Assessment_Id);
            int trythisa = 0;
            if (formmm.GetValue("IntakeAssessment.Case_Manager_Id") != null) { 
            string TestToSeeIfCaseMngIsNotNull = formmm.GetValue("IntakeAssessment.Case_Manager_Id").AttemptedValue;
            if (formmm.GetValue("IntakeAssessment.Case_Manager_Id") != null && TestToSeeIfCaseMngIsNotNull!="")
            {
                trythisa = Convert.ToInt32(formmm.GetValue("IntakeAssessment.Case_Manager_Id").AttemptedValue);
            }
            }
            int? caseMngValueFromForm = Convert.ToInt32(trythisa);
            //string tmpValue = Convert.ToString(intakeClientViewModel.IntakeAssessment.Intake_Assessment_Id);

            var assessmentItem = assessmentModel.GetSpecificIntakeAssessment(int.Parse(Convert.ToString(intakeClientViewModel.IntakeAssessment.Intake_Assessment_Id)));

            intakeClientViewModel.IntakeAssessment.Case_Manager_Id = trythisa;//Case_Manager_TEMP;

            if (ModelState.IsValid)
            {
                var currentUser = (User)Session["CurrentUser"];

                var userName = string.Empty;

                if (currentUser != null)
                {
                    userName = currentUser.User_Name;
                }

                var dateLastModified = DateTime.Now;
                var modifiedBy = userName;
                const bool isActive = true;
                const bool isDeleted = false;

                // First Update Person Details, it might have changed
                var personModel = new PersonModel();

                var updatedPerson = personModel.EditPerson(intakeClientViewModel.Person.Person_Id, intakeClientViewModel.Person.First_Name,
                    intakeClientViewModel.Person.Last_Name, intakeClientViewModel.Person.Known_As,
                    intakeClientViewModel.Person.Identification_Type_Id, intakeClientViewModel.Person.Identification_Number, 
                    intakeClientViewModel.Person.Date_Of_Birth, intakeClientViewModel.Person.Age, intakeClientViewModel.Person.Is_Estimated_Age, intakeClientViewModel.Person.Sexual_Orientation_Id,
                    intakeClientViewModel.Person.Language_Id, intakeClientViewModel.Person.Gender_Id, intakeClientViewModel.Person.Marital_Status_Id, 
                    intakeClientViewModel.Person.Religion_Id, intakeClientViewModel.Person.Preferred_Contact_Type_Id, intakeClientViewModel.Person.Phone_Number, 
                    intakeClientViewModel.Person.Mobile_Phone_Number, intakeClientViewModel.Person.Email_Address, intakeClientViewModel.Person.Population_Group_Id,
                    intakeClientViewModel.Person.Nationality_Id, intakeClientViewModel.Person.Disability_Type_Id, intakeClientViewModel.Person.Citizenship_Id, dateLastModified, modifiedBy,
                    isActive, isDeleted);
                //var PersonDisability = personModel.CreatePersonDisability(intakeClientViewModel.Person.Person_Id, intakeClientViewModel.Person.SelectedDisabilityId, userName);
                //if (updatedPerson == null|| PersonDisability==null)
                //{
                //    ViewBag.Message = "An Error Occured, Please contact Support";
                //    return View(intakeClientViewModel);
                //}

                // Update Address details
                var addressModel = new AddressModel();

                var updatedPhysicalAddress = addressModel.EditAddress(intakeClientViewModel.PhysicalAddress.Address_Id, (int)AddressTypeEnum.PhysicalAddress, intakeClientViewModel.PhysicalAddress.Address_Line_1, intakeClientViewModel.PhysicalAddress.Address_Line_2, intakeClientViewModel.PhysicalAddress.Town_Id, intakeClientViewModel.PhysicalAddress.Postal_Code);
                var updatedPostalAddress = addressModel.EditAddress(intakeClientViewModel.PostalAddress.Address_Id, (int)AddressTypeEnum.PostalAddress, intakeClientViewModel.PostalAddress.Address_Line_1, intakeClientViewModel.PostalAddress.Address_Line_2, intakeClientViewModel.PostalAddress.Town_Id, intakeClientViewModel.PostalAddress.Postal_Code);

                if ((updatedPhysicalAddress == null) || (updatedPostalAddress == null))
                {
                    ViewBag.Message = "An Error Occurred, Please contact Support";
                    return View(intakeClientViewModel);
                }

                #region Disability multiselect
                var value = formmm["SelectedDisabilities"];
                var personDisabilityDetails = new PersonDisabilityModel();

                if (value != null)
                {
                    string[] selectedDisabilityArray = formmm["SelectedDisabilities"].ToString().Split(',');
                    personDisabilityDetails.Delete(Convert.ToInt32(updatedPerson.Person_Id));

                    foreach (string i in selectedDisabilityArray)
                    {
                        personDisabilityDetails.Create(Convert.ToInt32(i), intakeClientViewModel.Person.Person_Id);
                    }

                    if (intakeClientViewModel.PostedDisabilityType != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedDisabilityType.DisabilityTypeIDs)
                        {
                            personDisabilityDetails.Create(Convert.ToInt32(item), intakeClientViewModel.Person.Person_Id);
                        }
                    }

                }
                
                #endregion

                #region disability Sub Type multiselect post
                var values = formmm["SelectedDisabilitiesType"];
                var personDisabilitySubTypeDetails = new PersonDisabilityTypeModel();
                if (values != null)
                {
                    string[] selectedDisabilityTypeArray = formmm["SelectedDisabilitiesType"].ToString().Split(',');
                    personDisabilitySubTypeDetails.Delete(Convert.ToInt32(updatedPerson.Person_Id));

                    foreach (string i in selectedDisabilityTypeArray)
                    {
                        personDisabilitySubTypeDetails.Create(Convert.ToInt32(i), updatedPerson.Person_Id);
                    }

                    if (intakeClientViewModel.PostedDisabilitySubType != null)
                    {
                        foreach (var item in intakeClientViewModel.PostedDisabilitySubType.DisabilitySubTypeIDs)
                        {
                            personDisabilitySubTypeDetails.Create(Convert.ToInt32(item), updatedPerson.Person_Id);
                        }
                    }
                }


                #endregion

                var intakeAssessment = intakeClientViewModel.IntakeAssessment;
                intakeAssessment.Intake_Assessment_Id = Convert.ToInt32(Session["IntAssId"]);
                var intakeAssessmentModel = new IntakeAssessmentModel();

                //intakeAssessment.Date_Allocated = DateTime.Now;

                // Update existing Intake Assessment
                if ((caseMngSupId != null && caseMngId == null) || (caseMngSupId != null && caseMngId != null))
                {
                    intakeAssessment.Case_Manager_Supervisor_Id = caseMngSupId;
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Supervisor allocated" + caseMngSupId, "Intake");
                    
                }
                if (caseMngId == null && caseMngValueFromForm!=null && caseMngValueFromForm!=0)
                {
                    intakeAssessment.Case_Manager_Id = caseMngValueFromForm;
                    intakeAssessment.Case_Manager_Supervisor_Id = caseMngSupId;
                    //Insert AuditTrail to be included on all create and edit methods in all controller of all modules
                    //Change task performed and module name
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(userName, "Case manager allocated" + caseMngId, "Intake");
                    
                }
                if (caseMngId != null)
                {
                    intakeAssessment.Case_Manager_Id = caseMngId;
                }
                var editedIntakeAssessment = intakeAssessmentModel.EditIntakeAssessment(intakeAssessment.Intake_Assessment_Id, intakeClientViewModel.Client.Client_Id, intakeAssessment.Assessment_Date, intakeAssessment.Assessed_By_Id, intakeAssessment.Case_Manager_Supervisor_Id,
                       intakeAssessment.Case_Manager_Id, intakeAssessment.Preliminary_Assessment, intakeAssessment.Presenting_Problem, intakeAssessment.Problem_Sub_Category_Id, intakeAssessment.Is_Priority_Intervention,
                        intakeAssessment.Is_Referred_For_Assessment, intakeAssessment.Is_Referred_To_Other_Service_Provider, intakeAssessment.Is_Closed, intakeAssessment.ClosedDate, intakeAssessment.Treatment_Date,
                        intakeAssessment.Case_Background, intakeAssessment.Supervisor_Comments, intakeAssessment.Social_Worker_Comments, intakeAssessment.Referred_From_Organization_Id, intakeAssessment.Referred_To_Organization_Id,
                        intakeAssessment.Case_Allocation_Comments, intakeAssessment.Date_Allocated, intakeAssessment.Date_Due, isActive, isDeleted, dateLastModified, modifiedBy);

                    if (editedIntakeAssessment == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact support";
                        return View(intakeClientViewModel);
                    }

                    // Link selected referral focus areas
                    var referralFocusAreaIds = new List<int>();
                    if (intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas != null)
                    {
                        foreach (var focusAreaId in intakeClientViewModel.IntakeAssessment.Posted_Referral_Focus_Areas.Referral_Focus_Area_IDs)
                        {
                            var focusAreaIdValue = int.Parse(focusAreaId);
                            referralFocusAreaIds.Add(focusAreaIdValue);
                        }
                    }

                    intakeAssessmentModel.AddReferralFocusAreaToIntakeAssessment(editedIntakeAssessment.Intake_Assessment_Id, referralFocusAreaIds);
                
                return RedirectToAction("Index", "Intake");
            }

            return View(intakeClientViewModel);
        }

        public ActionResult GetListOfDisabilities(int Id)
        {
            var currentUser = (User)Session["CurrentUser"];


            PersonModel Model = new PersonModel();
            List<IntPersonDisabilitiesVM> disabilities = Model.GetListOfDisabilities(Id);
            return PartialView(disabilities);
        }

        public ActionResult DeleteDisability(int Id, int personId)
        {
            PersonModel Model = new PersonModel();
            Model.Delete_Disability(Id, personId);
            return RedirectToAction("Edit", new { id = personId });
        }

        public ActionResult Delete(string id)
        {
            var personModel = new PersonModel();
            var deletePerson = personModel.GetSpecificPerson(int.Parse(id));

            return View(deletePerson);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Person person)
        {
            var personModel = new PersonModel();
            var deletedPerson = personModel.SetPersonIsDeleted(person.Person_Id, true);

            if (deletedPerson == null)
            {
                ViewBag.Message = "An Error Occured, Contact support";
                return View(person);
            }

            return RedirectToAction("Index", "Intake");
        }


        /// <summary>
        /// Get list of Sub Disability
        /// Filter by disability category
        /// </summary>
        /// <param name="disabilityId"></param>
        /// <returns></returns>
        public ActionResult FilterFromDisabilityAjax(List<string> dataSelect)
        {
            var result = new Dictionary<int, string>();
            var disabilitySubTypeModel = new DisabilitySubTypeModel();

            
            if (dataSelect != null)
            {
                
               
                foreach (string i in dataSelect)
                {
                    var disabilitySubTypeList = disabilitySubTypeModel.GetSelectedListOfDisabilitiesSubType(Convert.ToInt32(i));

                    foreach(var j in disabilitySubTypeList)
                    {
                        result.Add(j.DisabilityType_Id, j.TypeName);
                    }
                   

                }
            }                 

            var returnResult = new SelectList(result.Select(m => new SelectListItem
            {
                Text = m.Value,
                Value = m.Key.ToString()
            }), "Value", "Text");
            

            return Json(returnResult, JsonRequestBehavior.AllowGet);
            
        }


        /// <summary>
        /// Get list of Districts
        /// Filter by provinceId
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public ActionResult FilterFromProvinceAjax(string provinceId)
        {
            //Use to generate VEP Search Reports
            Session["SearchProvinceId"] = provinceId;

            if (String.IsNullOrEmpty(provinceId))
            {
                provinceId = "-1";
            }

            var municipalityModel = new DistrictModel();
            var municipalitiesList = municipalityModel.GetListOfDistricts(int.Parse(provinceId));

            var result = (from x in municipalitiesList
                          orderby x.Description
                          select new
                          {
                              id = x.District_Id,
                              name = x.Description
                          })
                          
                          .ToList();
            ViewBag.AvailableDisabilitySubType = result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get list of Local municipality
        /// Filter by Distric id
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <returns></returns>

        public ActionResult FilterFromMunicipalityAjax(string municipalityId)
        {
            //Use to generate VEP Search Reports
            Session["SearchDistrictId"] = municipalityId;

            if (String.IsNullOrEmpty(municipalityId))
            {
                municipalityId = "-1";
            }

            var localMunicipalityModel = new LocalMunicipalityModel();
            var localMunicipalitiesList = localMunicipalityModel.GetListOfLocalMunicipalities(int.Parse(municipalityId));

            var result = (from x in localMunicipalitiesList
                          select new
                          {
                              id = x.Local_Municipality_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get list of towns
        /// filter by Local municipality
        /// </summary>
        /// <param name="localMunicipalityId"></param>
        /// <returns></returns>

        public ActionResult FilterFromLocalMunicipalityAjax(string localMunicipalityId)
        {

            if (String.IsNullOrEmpty(localMunicipalityId))
            {
                localMunicipalityId = "-1";
            }

            var townModel = new TownModel();
            var townsList = townModel.GetListOfTowns(int.Parse(localMunicipalityId));

            var result = (from x in townsList
                          select new
                          {
                              id = x.Town_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterFromOrganisationsAjax(string organizationId)
        {

            if (String.IsNullOrEmpty(organizationId))
            {
                organizationId = "-1";
            }

            var organizationModel = new OrganizationModel();
            var organizationList = organizationModel.GetListOfOrganizationsByLocalMunicipality(int.Parse(organizationId));

            var result = (from x in organizationList
                          select new
                          {
                              id = x.Organization_Id,
                              name = x.Site_Code
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterTown(string provinceIdString, string municipalityIdString, string localMunicipalityIdString)
        {
            var townModel = new TownModel();
            var townsList = townModel.GetListOfTowns();

            int provinceId;
            if (int.TryParse(provinceIdString, out provinceId))
                townsList.RemoveAll(x => !x.Local_Municipality.District.Province_Id.Equals(provinceId));

            int municipalityId;
            if (int.TryParse(municipalityIdString, out municipalityId))
                townsList.RemoveAll(x => !x.Local_Municipality.District_Municipality_Id.Equals(municipalityId));

            int localMunicipalityId;
            if (int.TryParse(localMunicipalityIdString, out localMunicipalityId))
                townsList.RemoveAll(x => !x.Local_Municipality_Id.Equals(localMunicipalityId));

            var result = (from x in townsList
                          select new
                          {
                              id = x.Town_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterFromTown(string townId)
        {
            var result = string.Empty;

            if (string.IsNullOrEmpty(townId))
            {
                result = "-1,-1,-1";
            }
            else
            {
                var townModel = new TownModel();
                var townItem = townModel.GetSpecificTown(int.Parse(townId));

                result = townItem.Local_Municipality.District.Province_Id + "," + townItem.Local_Municipality.District_Municipality_Id + "," + townItem.Local_Municipality_Id;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfProvinces()
        {
            var provinceModel = new ProvinceModel();
            var provinceList = provinceModel.GetListOfProvinces();

            var result = (from x in provinceList
                          select new
                          {
                              id = x.Province_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfMunicipalities(string provinceId)
        {
            var municipalityModel = new DistrictModel();
            var municipalityList = municipalityModel.GetListOfDistricts(int.Parse(provinceId));

            var result = (from x in municipalityList
                          select new
                          {
                              id = x.District_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfLocalMunicipalities(string municipalityId)
        {
            var localMunicipalityModel = new LocalMunicipalityModel();
            var localMunicipalityList = localMunicipalityModel.GetListOfLocalMunicipalities(int.Parse(municipalityId));

            var result = (from x in localMunicipalityList
                          select new
                          {
                              id = x.Local_Municipality_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClientGridAjaxPaging(IntakeSearchViewModel intakeGrid)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;
            var currentUserProvinceId = -1;

            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }

            if (currentUser.Employees.Any())
                currentUserProvinceId = currentUser.Employees.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            if (currentUser.apl_Social_Worker.Any())
                currentUserProvinceId = currentUser.apl_Social_Worker.First().apl_Service_Office.apl_Local_Municipality.District.Province_Id;

            var personModel = new PersonModel();
            var listOfPersons = personModel.GetListOfPersons(false, false);

            if (intakeGrid.showOnlyMyRecords)
                listOfPersons.RemoveAll(x => !x.Created_By.Equals(userName));

            var query = from p in listOfPersons select p;

            if (!string.IsNullOrEmpty(intakeGrid.Search_Client_Ref_No))
                query = query.Where(p => (p.Clients.Any() && (p.Clients.First().Reference_Number.Equals(intakeGrid.Search_Client_Ref_No))));

            if (!string.IsNullOrEmpty(intakeGrid.Search_First_Name))
                query = query.Where(p => p.First_Name.ToLower().Contains(intakeGrid.Search_First_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeGrid.Search_Last_Name))
                query = query.Where(p => p.Last_Name.ToLower().Contains(intakeGrid.Search_Last_Name.ToLower()));

            if (!string.IsNullOrEmpty(intakeGrid.Search_Client_ID_No))
                query = query.Where(p => p.Identification_Number.Contains(intakeGrid.Search_Client_ID_No));

            DateTime parsedDate;
            if ((!string.IsNullOrEmpty(intakeGrid.Search_Date_Of_Birth)) && (DateTime.TryParse(intakeGrid.Search_Date_Of_Birth, out parsedDate)))
                query = query.Where(p => p.Date_Of_Birth.Equals(parsedDate));

            var filteredResults = query.ToList();

            var clientItems = filteredResults.Select(x => new ClientGridMain()
            {
                PersonId = x.Person_Id,
                FirstName = x.First_Name,
                LastName = x.Last_Name,
                IDNumber = x.Identification_Number,
                AssessmentCount = x.Clients.Any() ? x.Clients.First().Intake_Assessments.Count : 0,
                NestedItems = !x.Clients.Any() ? new List<ClientGridNested>() : x.Clients.First().Intake_Assessments.Select(y => new ClientGridNested()
                {
                    PersonId = x.Person_Id,
                    AssessmentId = y.Intake_Assessment_Id,
                    AssessmentDate = y.Assessment_Date
                }).ToList()
            }).ToList();

            int skip = intakeGrid.pageNumber.HasValue ? intakeGrid.pageNumber.Value - 1 : 0;
            var data = clientItems.OrderBy(o => o.PersonId).Skip(skip * 5).Take(5).ToList();
            var grid = new WebGrid(data, canPage: true, rowsPerPage: 5, canSort: false);
            var htmlString = grid.GetHtml(tableStyle: "NestedMainGrid",
                                          headerStyle: "webgrid-header",
                                          alternatingRowStyle: "webgrid-alternating-row",
                                          selectedRowStyle: "webgrid-selected-row",
                                          rowStyle: "webgrid-row-style",
                                          htmlAttributes: new { id = "clientsListGrid" },
                                          mode: WebGridPagerModes.NextPrevious,
                                          columns: grid.Columns(
                                                grid.Column("FirstName", "First Name"),
                                                grid.Column("LastName", "Last Name"),
                                                grid.Column("IDNumber", "ID Number"),
                                                grid.Column("AssessmentCount", "No of Assessments"),
                                                grid.Column(header: "", style: "EditPersonWidth", format: (item) =>
                                                {
                                                    //var link = MvcHtmlString.Create(HtmlHelper.GenerateLink(this.ControllerContext.RequestContext, System.Web.Routing.RouteTable.Routes, "Edit Person", "", "Edit", "Intake", new System.Web.Routing.RouteValueDictionary(new { id = item.PersonId }), new System.Web.Routing.RouteValueDictionary(new { @class = "btn btn-success" })));
                                                    var link = MvcHtmlString.Create("<a class='btn btn-success' href='" + Url.Action("Edit", "Intake", new { id = item.PersonId }) + "'>Edit Person</a>");
                                                    return link;
                                                }),
                                                grid.Column(header: "", style: "CreateAssessmentWidth", format: (item) =>
                                                {
                                                    var link = MvcHtmlString.Create("<a class='btn btn-success' href='" + Url.Action("CreateNewAssessment", "Intake", new { id = item.PersonId }) + "'>Create Assessment</a>");
                                                    return link;
                                                }),
                                                grid.Column(format: (item) =>
                                                {
                                                    var subGrid = new WebGrid(source: item.NestedItems, canPage: false, canSort: false);
                                                    return subGrid.GetHtml(
                                                        tableStyle: "NestedSubGrid",
                                                        htmlAttributes: new { id = "assessmentsListGrid", @class = "NestedSubGrid", width = "100%" },
                                                        columns: subGrid.Columns(
                                                            subGrid.Column("AssessmentDate", "Assessment Date", format: (dateItem) => string.Format("{0:dd MMM yyyy}", dateItem.AssessmentDate)),
                                                            subGrid.Column(header: "", style: "EditAssessmentWidth", format: (subItem) =>
                                                            {
                                                                //var link = Html.ActionLink("Edit Assessment", "EditAssessment", "Intake", new { id = subItem.AssessmentId }, new { @class = "btn btn-success" });
                                                                var link = MvcHtmlString.Create("<a class='btn btn-success' href='" + Url.Action("EditAssessment", "Intake", new { id = subItem.AssessmentId}) + "'>Edit Assessment</a>");
                                                                return link;
                                                            })
                                                        )
                                                    );
                                                })
                                            )
                                        );
            return Json(new
            {
                Data = htmlString.ToHtmlString(),
                Count = (clientItems.Count() + 5 - 1) / 5,
                Page = intakeGrid.pageNumber ?? 1
            }, JsonRequestBehavior.AllowGet);
        }

        //public void AuditTrail(string username, string taskperformed, string module)
        //{
        //    var serviceOffice = new ServiceOfficeModel();
        //    var organisation = new OrganizationModel();
        //    var auditTrail = new AuditTrailModel();

        //    var userServiceOffice = serviceOffice.GetUserSpecificServiceOffice(username);
        //    var userOrganisation = organisation.GetUserSpecificOrganisation(username);
        //    var userAuditTrail = auditTrail.CreateAuditTrail(taskperformed, module, username, userServiceOffice, userOrganisation);
        //}
    }
}