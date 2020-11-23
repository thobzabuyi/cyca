﻿using Common_Objects;
using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common_Objects.ViewModels;

namespace SDIIS.Controllers
{
    public class RelationController : Controller
    {
        public ActionResult GetRelationItemsByAjax(string id, string relationTypeId)
        {
            var personModel = new PersonModel();
            var person = personModel.GetSpecificPerson(int.Parse(id));

            var relationItems = new List<IntakeRelationItem>();

            var client = person.Clients.FirstOrDefault();
            if (string.IsNullOrEmpty(relationTypeId))
                relationTypeId = "-1";

            if (client != null)
            {
                if ((relationTypeId == "-1") || (int.Parse(relationTypeId) == (int)RelationTypeEnum.AdoptiveParents))
                {
                    relationItems.AddRange(from p in client.Client_Adoptive_Parents.ToList()
                                           select new IntakeRelationItem()
                                           {
                                               Item_Id = p.Client_Adoptive_Parent_Id,
                                               Person_Id = p.Person_Id,
                                               Relation_Person = p.Person,
                                               Relation_Type_Id = (int)RelationTypeEnum.AdoptiveParents,
                                               Relation_Type_Description = "Adoptive Parent"
                                           });
                }
                if ((relationTypeId == "-1") || (int.Parse(relationTypeId) == (int)RelationTypeEnum.BiologicalParents))
                {
                    relationItems.AddRange(from p in client.Client_Biological_Parents.ToList()
                                           select new IntakeRelationItem()
                                           {
                                               Item_Id = p.Client_Biological_Parent_Id,
                                               Person_Id = p.Person_Id,
                                               Relation_Person = p.Person,
                                               Relation_Type_Id = (int)RelationTypeEnum.BiologicalParents,
                                               Relation_Type_Description = "Biological Parent"
                                           });
                }
                if ((relationTypeId == "-1") || (int.Parse(relationTypeId) == (int)RelationTypeEnum.FamilyMembers))
                {
                    relationItems.AddRange(from p in client.Client_Family_Members.ToList()
                                           select new IntakeRelationItem()
                                           {
                                               Item_Id = p.Client_Family_Member_Id,
                                               Person_Id = p.Person_Id,
                                               Relation_Person = p.Person,
                                               Relation_Type_Id = (int)RelationTypeEnum.FamilyMembers,
                                               Relation_Type_Description = "Family Member"
                                           });
                }
                if ((relationTypeId == "-1") || (int.Parse(relationTypeId) == (int)RelationTypeEnum.FosterParents))
                {
                    relationItems.AddRange(from p in client.Client_Foster_Parents.ToList()
                                           select new IntakeRelationItem()
                                           {
                                               Item_Id = p.Client_Foster_Parent_Id,
                                               Person_Id = p.Person_Id,
                                               Relation_Person = p.Person,
                                               Relation_Type_Id = (int)RelationTypeEnum.FosterParents,
                                               Relation_Type_Description = "Foster Parent"
                                           });
                }
                if ((relationTypeId == "-1") || (int.Parse(relationTypeId) == (int)RelationTypeEnum.Caregivers))
                {
                    relationItems.AddRange(from p in client.Client_CareGivers.ToList()
                                           select new IntakeRelationItem()
                                           {
                                               Item_Id = p.Client_Caregiver_Id,
                                               Person_Id = p.Person_Id,
                                               Relation_Person = p.Person,
                                               Relation_Type_Id = (int)RelationTypeEnum.Caregivers,
                                               Relation_Type_Description = "Caregiver"
                                           });
                }
                if ((relationTypeId == "-1") || (int.Parse(relationTypeId) == (int)RelationTypeEnum.ProspectiveAdoptiveParents))
                {
                    relationItems.AddRange(from p in client.int_Client_ProspectiveAdoptiveParents.ToList()
                                           select new IntakeRelationItem()
                                           {
                                               Item_Id = p.Client_ProspectiveAdoptiveParents_Id,
                                               Person_Id = p.Person_Id,
                                               Relation_Person = p.int_Person,
                                               Relation_Type_Id = (int)RelationTypeEnum.ProspectiveAdoptiveParents,
                                               Relation_Type_Description = "ProspectiveAdoptiveParents"
                                           });
                }
            }

            return PartialView("_RelationGrid", relationItems);
        }

        public ActionResult Create(string id, string selectedRelationTypeId)
        {
            var relationItem = new IntakeRelationItem()
            {
                Relation_Type_Id = int.Parse(selectedRelationTypeId),
                Person_Id = int.Parse(id),
                SearchPerson = new IntakeSearchViewModel() { Person_List = new List<Person>() },
                CreatePerson = new PersonDetailViewModel() { Person = new Person() { Person_Id = -1 }, PhysicalAddress = new Address(), PostalAddress = new Address() }
            };

            relationItem.IsAdoptiveParentDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.AdoptiveParents;
            relationItem.IsBiologicalParentDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.BiologicalParents;
            relationItem.IsFosterParentDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.FosterParents;
            relationItem.IsCaregiverDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.Caregivers;
            relationItem.IsFamilyMemberDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.FamilyMembers;
            relationItem.IsProspectiveAdoptiveParentVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.ProspectiveAdoptiveParents;

           
            return PartialView("_RelationCreate", relationItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(IntakeRelationItem intakeRelationItem)
        {
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;

            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }
            var personModel = new PersonModel();
            var personItem = new Person();

            var dateCreated = DateTime.Now;
            const string createdBy = "jhendrikse";
            var dateLastModified = DateTime.Now;
            const string modifiedBy = "jhendrikse";
            const bool isActive = true;
            const bool isDeleted = false;

            var addToPerson = personModel.GetSpecificPerson(intakeRelationItem.Person_Id);
            var clientId = addToPerson.Clients.First().Client_Id;

            // Determine if a new Person has been added, or if an existing Person's details must be edited.
            if (intakeRelationItem.CreatePerson.Person.Person_Id == -1)
            {
                var newPersonDetail = intakeRelationItem.CreatePerson.Person;
                var physicalAddress = intakeRelationItem.CreatePerson.PhysicalAddress;
                var postalAddress = intakeRelationItem.CreatePerson.PostalAddress;

                // Create New Person
                personItem = personModel.CreatePerson(newPersonDetail.First_Name, newPersonDetail.Last_Name, newPersonDetail.Known_As, newPersonDetail.Identification_Type_Id, newPersonDetail.Identification_Number, false, null, newPersonDetail.Date_Of_Birth, newPersonDetail.Age, newPersonDetail.Is_Estimated_Age, newPersonDetail.Sexual_Orientation_Id, newPersonDetail.Language_Id, newPersonDetail.Gender_Id, newPersonDetail.Marital_Status_Id, newPersonDetail.Religion_Id, newPersonDetail.Preferred_Contact_Type_Id, newPersonDetail.Phone_Number, newPersonDetail.Mobile_Phone_Number, newPersonDetail.Email_Address, newPersonDetail.Population_Group_Id, newPersonDetail.Nationality_Id, newPersonDetail.Disability_Type_Id, newPersonDetail.Citizenship_Id, dateCreated, createdBy, isActive, isDeleted);
                //var PersonDisability = personModel.CreatePersonDisability(newPersonDetail.Person_Id, newPersonDetail.SelectedDisabilityId, userName);

                if (personItem != null)
                {
                    personModel.AddAddress(personItem.Person_Id, (int)AddressTypeEnum.PhysicalAddress, physicalAddress.Address_Line_1, physicalAddress.Address_Line_2, physicalAddress.Town_Id, physicalAddress.Postal_Code);
                    personModel.AddAddress(personItem.Person_Id, (int)AddressTypeEnum.PostalAddress, postalAddress.Address_Line_1, postalAddress.Address_Line_2, postalAddress.Town_Id, postalAddress.Postal_Code);
                }
            }
            else
            {
                var editPersonDetail = intakeRelationItem.CreatePerson.Person;

                // Edit Person
                personItem = personModel.EditPerson(editPersonDetail.Person_Id, editPersonDetail.First_Name, editPersonDetail.Last_Name, editPersonDetail.Known_As, editPersonDetail.Identification_Type_Id, editPersonDetail.Identification_Number, editPersonDetail.Date_Of_Birth, editPersonDetail.Age, editPersonDetail.Is_Estimated_Age, editPersonDetail.Sexual_Orientation_Id, editPersonDetail.Language_Id, editPersonDetail.Gender_Id, editPersonDetail.Marital_Status_Id, editPersonDetail.Religion_Id, editPersonDetail.Preferred_Contact_Type_Id, editPersonDetail.Phone_Number, editPersonDetail.Mobile_Phone_Number, editPersonDetail.Email_Address, editPersonDetail.Population_Group_Id, editPersonDetail.Nationality_Id, editPersonDetail.Disability_Type_Id, editPersonDetail.Citizenship_Id, dateLastModified, modifiedBy, isActive, isDeleted);
                //var PersonDisability = personModel.CreatePersonDisability(editPersonDetail.Person_Id, editPersonDetail.SelectedDisabilityId,userName);

            }

            var status = "OK";
            var message = "The education details have been successfully added";

            if (personItem != null)
            {
                switch (intakeRelationItem.Relation_Type_Id)
                {
                    case (int)RelationTypeEnum.AdoptiveParents:
                        var clientAdoptionModel = new ClientAdoptiveParentModel();
                        var newAdoptiveParent = clientAdoptionModel.CreateClientAdoptiveParent(clientId, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateCreated, createdBy, isActive, isDeleted);

                        if (newAdoptiveParent == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.BiologicalParents:
                        var clientBiologicalModel = new ClientBiologicalParentModel();
                        var newBiologicalParent = clientBiologicalModel.CreateClientBiologicalParent(clientId, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateCreated, createdBy, isActive, isDeleted);

                        if (newBiologicalParent == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.Caregivers:
                        var clientCaregiverModel = new ClientCareGiverModel();
                        var newClientCareverModel = clientCaregiverModel.CreateClientCaregiver(clientId, personItem.Person_Id, intakeRelationItem.Relation_Type_Id, dateCreated, createdBy, isActive, isDeleted);

                        if (newClientCareverModel == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.FamilyMembers:
                        var clientFamilymemberModel = new ClientFamilyMemberModel();
                        var newClientFamilyMember = clientFamilymemberModel.CreateClientFamilyMember(clientId, personItem.Person_Id, intakeRelationItem.Relation_Type_Id, dateCreated, createdBy, isActive, isDeleted);

                        if (newClientFamilyMember == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.FosterParents:
                        var clientFosterParentModel = new ClientFosterParentModel();
                        var newFosterParent = clientFosterParentModel.CreateClientFosterParent(clientId, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateCreated, createdBy, isActive, isDeleted);

                        if (newFosterParent == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.ProspectiveAdoptiveParents:
                        var clientProspectiveAdoptiveParentsModel = new ProspectiveAdoptiveParentModel();
                        var newClientProspectiveAdoptiveParentModel = clientProspectiveAdoptiveParentsModel.CreateClientProspectiveAdoptiveParents(clientId, personItem.Person_Id, intakeRelationItem.Relation_Type_Id, dateCreated, createdBy, isActive, isDeleted);

                        if (newClientProspectiveAdoptiveParentModel == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                }
            }

            

            return new JsonResult { Data = new { status, message } };
        }

        public ActionResult Edit(string id, string personId, string selectedRelationTypeId)
        {
            var personModel = new PersonModel();
            var relatedPerson = personModel.GetSpecificPerson(int.Parse(personId));

            var physicalAddress = relatedPerson.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? relatedPerson.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
            var postalAddress = relatedPerson.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? relatedPerson.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();

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

            var relationItem = new IntakeRelationItem()
            {
                Item_Id = int.Parse(id),
                Relation_Type_Id = int.Parse(selectedRelationTypeId),
                Person_Id = int.Parse(id),
                SearchPerson = new IntakeSearchViewModel() { Person_List = new List<Person>() },
                CreatePerson = new PersonDetailViewModel() { Person = relatedPerson, PhysicalAddress = physicalAddress, PostalAddress = postalAddress }
            };

            switch (int.Parse(selectedRelationTypeId))
            {
                case (int)RelationTypeEnum.AdoptiveParents:
                    var clientAdoptionModel = new ClientAdoptiveParentModel();
                    //var newAdoptiveParent = clientAdoptionModel.CreateClientAdoptiveParent(clientId, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateCreated, createdBy, isActive, isDeleted);

                    var adoptiveParent = clientAdoptionModel.GetSpecificClientAdoptiveParent(int.Parse(id));

                    if (adoptiveParent != null)
                    {
                        relationItem.IsDeceased = adoptiveParent.Is_Deceased != null && (bool)adoptiveParent.Is_Deceased;
                        relationItem.DateDeceased = adoptiveParent.Date_Deceased == null ? string.Empty : ((DateTime)adoptiveParent.Date_Deceased).ToString("dd MMMM yyyy HH:mm:ss");
                    }
                    break;
                case (int)RelationTypeEnum.ProspectiveAdoptiveParents:
                    var PAPModel = new ProspectiveAdoptiveParentModel();
                    //var newAdoptiveParent = clientAdoptionModel.CreateClientAdoptiveParent(clientId, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateCreated, createdBy, isActive, isDeleted);

                    var pAP = PAPModel.GetSpecificProspectiveAdoptiveParent(int.Parse(id));


                    break;

            }

            relationItem.IsAdoptiveParentDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.AdoptiveParents;
            relationItem.IsBiologicalParentDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.BiologicalParents;
            relationItem.IsFosterParentDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.FosterParents;
            relationItem.IsCaregiverDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.Caregivers;
            relationItem.IsFamilyMemberDetailVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.FamilyMembers;
            relationItem.IsProspectiveAdoptiveParentVisible = int.Parse(selectedRelationTypeId) == (int)RelationTypeEnum.ProspectiveAdoptiveParents;

            return PartialView("_RelationEdit", relationItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IntakeRelationItem intakeRelationItem, FormCollection Formssss)
        {
            foreach (var key in Formssss.AllKeys)
            {
                var add = Formssss.Get(key);
            }
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;

            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }
            var personModel = new PersonModel();
            var personItem = new Person();

            var dateCreated = DateTime.Now;
            const string createdBy = "jhendrikse";
            var dateLastModified = DateTime.Now;
            const string modifiedBy = "jhendrikse";
            const bool isActive = true;
            const bool isDeleted = false;

            // Determine if a new Person has been added, or if an existing Person's details must be edited.
            if (intakeRelationItem.CreatePerson.Person.Person_Id == -1)
            {
                var newPersonDetail = intakeRelationItem.CreatePerson.Person;
                var PersonDisability = personModel.CreatePersonDisability(intakeRelationItem.Person_Id, intakeRelationItem.SelectedDisabilityId, userName);
                var physicalAddress = intakeRelationItem.CreatePerson.PhysicalAddress;
                var postalAddress = intakeRelationItem.CreatePerson.PostalAddress;

                // Create New Person
                personItem = personModel.CreatePerson(newPersonDetail.First_Name, newPersonDetail.Last_Name, newPersonDetail.Known_As, newPersonDetail.Identification_Type_Id, newPersonDetail.Identification_Number, false, null, newPersonDetail.Date_Of_Birth, newPersonDetail.Age, newPersonDetail.Is_Estimated_Age, newPersonDetail.Sexual_Orientation_Id, newPersonDetail.Language_Id, newPersonDetail.Gender_Id, newPersonDetail.Marital_Status_Id, newPersonDetail.Religion_Id, newPersonDetail.Preferred_Contact_Type_Id, newPersonDetail.Phone_Number, newPersonDetail.Mobile_Phone_Number, newPersonDetail.Email_Address, newPersonDetail.Population_Group_Id, newPersonDetail.Nationality_Id, newPersonDetail.Disability_Type_Id, newPersonDetail.Citizenship_Id, dateCreated, createdBy, isActive, isDeleted);

                if (personItem != null)
                {
                    personModel.AddAddress(personItem.Person_Id, (int)AddressTypeEnum.PhysicalAddress, physicalAddress.Address_Line_1, physicalAddress.Address_Line_2, physicalAddress.Town_Id, physicalAddress.Postal_Code);
                    personModel.AddAddress(personItem.Person_Id, (int)AddressTypeEnum.PostalAddress, postalAddress.Address_Line_1, postalAddress.Address_Line_2, postalAddress.Town_Id, postalAddress.Postal_Code);
                }
            }
            else
            {
                var editPersonDetail = intakeRelationItem.CreatePerson.Person;

                // Edit Person
                personItem = personModel.EditPerson(editPersonDetail.Person_Id, editPersonDetail.First_Name, editPersonDetail.Last_Name, editPersonDetail.Known_As, editPersonDetail.Identification_Type_Id, editPersonDetail.Identification_Number, editPersonDetail.Date_Of_Birth, editPersonDetail.Age, editPersonDetail.Is_Estimated_Age, editPersonDetail.Sexual_Orientation_Id, editPersonDetail.Language_Id, editPersonDetail.Gender_Id, editPersonDetail.Marital_Status_Id, editPersonDetail.Religion_Id, editPersonDetail.Preferred_Contact_Type_Id, editPersonDetail.Phone_Number, editPersonDetail.Mobile_Phone_Number, editPersonDetail.Email_Address, editPersonDetail.Population_Group_Id, editPersonDetail.Nationality_Id, editPersonDetail.Disability_Type_Id, editPersonDetail.Citizenship_Id, dateLastModified, modifiedBy, isActive, isDeleted);
                int Pid = editPersonDetail.Person_Id;
                //List<int> DIDS = editPersonDetail.SelectedDisabilityId;
                string usre = userName;

            }

            var status = "OK";
            var message = "The education details have been successfully added";

            if (personItem != null)
            {
                switch (intakeRelationItem.Relation_Type_Id)
                {
                    case (int)RelationTypeEnum.AdoptiveParents:
                        var clientAdoptionModel = new ClientAdoptiveParentModel();
                        var newAdoptiveParent = clientAdoptionModel.EditClientAdoptiveParent(intakeRelationItem.Item_Id, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateLastModified, modifiedBy, isActive, isDeleted);

                        if (newAdoptiveParent == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.BiologicalParents:
                        var clientBiologicalModel = new ClientBiologicalParentModel();
                        var newBiologicalParent = clientBiologicalModel.EditClientBiologicalParent(intakeRelationItem.Item_Id, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateCreated, createdBy, isActive, isDeleted);

                        if (newBiologicalParent == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.Caregivers:
                        var clientCaregiverModel = new ClientCareGiverModel();
                        var newClientCareverModel = clientCaregiverModel.EditClientCaregiver(intakeRelationItem.Item_Id, personItem.Person_Id, intakeRelationItem.Person_Relationship_Type_Id, dateCreated, createdBy, isActive, isDeleted);

                        if (newClientCareverModel == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.FamilyMembers:
                        var clientFamilymemberModel = new ClientFamilyMemberModel();
                        var newClientFamilyMember = clientFamilymemberModel.EditClientFamilyMember(intakeRelationItem.Item_Id, personItem.Person_Id, intakeRelationItem.Person_Relationship_Type_Id, dateCreated, createdBy, isActive, isDeleted);

                        if (newClientFamilyMember == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.FosterParents:
                        var clientFosterParentModel = new ClientFosterParentModel();
                        var newFosterParent = clientFosterParentModel.EditClientFosterParent(intakeRelationItem.Item_Id, personItem.Person_Id, intakeRelationItem.IsDeceased, string.IsNullOrEmpty(intakeRelationItem.DateDeceased) ? (DateTime?)null : DateTime.Parse(intakeRelationItem.DateDeceased), dateCreated, createdBy, isActive, isDeleted);

                        if (newFosterParent == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                    case (int)RelationTypeEnum.ProspectiveAdoptiveParents:
                        var clientProspectiveAdoptiveParentModel = new ProspectiveAdoptiveParentModel();
                        var newClientProspectiveAdoptiveParentsModel = clientProspectiveAdoptiveParentModel.EditClientProspectiveAdoptiveParents(intakeRelationItem.Item_Id, personItem.Person_Id, intakeRelationItem.Relation_Type_Id, dateCreated, createdBy, isActive, isDeleted);

                        if (newClientProspectiveAdoptiveParentsModel == null)
                        {
                            status = "Error";
                            message = "A technical error has occurred! Please try again later";
                        }
                        break;
                }
            }

            return new JsonResult { Data = new { status, message } };
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
    }
}