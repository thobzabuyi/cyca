﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common_Objects.ViewModels;

//using VEP_Module.Models;

namespace Common_Objects.Models
{
    public partial class Login_User
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The User Name field is required")]
        [Display(Name = "Username", Description = "The current user's user name")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Password field is required")]
        [Display(Name = "Password", Description = "The current user's password")]
        [DataType(DataType.Text)]
        public string Password { get; set; }
    }

    public partial class User_Forgot_Password
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The User Name field is required")]
        [Display(Name = "Username", Description = "The current user's user name")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
    }

    public partial class User_Reset_Password
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The User Name field is required")]
        [Display(Name = "Username", Description = "The current user's user name")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Old Password field is required")]
        [Display(Name = "Current Password", Description = "The user's old (current) password")]
        [DataType(DataType.Text)]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The New Password field is required")]
        [Display(Name = "New Password", Description = "The user's new password")]
        [DataType(DataType.Text)]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Confirm New Password field is required")]
        [Display(Name = "Confirm Password", Description = "The user's new (confirmed) password")]
        [DataType(DataType.Text)]
        public string ConfirmNewPassword { get; set; }
    }

    public partial class ManageUserViewModel
    {
        public User User { get; set; }

        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public List<Role> Available_Roles
        {
            get
            {
                var roleModel = new RoleModel();
                var rolesList = roleModel.GetListOfRoles(false, false);

                return rolesList;
            }
        }
        public PostedRoles Posted_Roles { get; set; }
        public List<Group> Available_Groups
        {
            get
            {
                var groupModel = new GroupModel();
                var groupList = groupModel.GetListOfGroups(false, false);

                return groupList;
            }
        }
        public PostedGroups Posted_Groups { get; set; }
        public object Facility_Id { get; internal set; }
        public string fullname
        {
            get
            {
                return this.First_Name + " " + this.Last_Name;
            }
        }
    }

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        [Display(Name = "Head of Department", Description = "The employee's Head of Department")]
        public SelectList Heads_of_Department_List
        {
            get
            {
                var employeeModel = new EmployeeModel();
                var listOfEmployees = employeeModel.GetListOfEmployees(false, false);

                var employeesList = (from e in listOfEmployees
                                     select new SelectListItem()
                                     {
                                         Text = string.Format("{0} {1}", e.User.First_Name, e.User.Last_Name),
                                         Value = e.Employee_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = e.Head_Of_Department_Id.Equals(Head_Of_Department_Id)
                                     }).ToList();

                var selectList = new SelectList(employeesList, "Value", "Text", Head_Of_Department_Id);

                return selectList;
            }
        }
        [Display(Name = "Facilities", Description = "The faciltiy the employee belongs too")]
        public SelectList Facility_List
        {
            get
            {
                var employeeModel = new EmployeeModel();
                var listOfFacilities = employeeModel.GetListfFaciities(false, false);

                var facilitiesList = (from e in listOfFacilities
                                     select new SelectListItem()
                                     {
                                         Text = string.Format("{0}", e.FacilityName),
                                         Value = e.Facility_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = e.Facility_Id.Equals(Facility_Id)
                                     }).ToList();

                var selectList = new SelectList(facilitiesList, "Value", "Text", Facility_Id);

                return selectList;
            }
        }

        public SelectList Roles_List
        {
            get
            {
                var roleModel = new RoleModel();
                var rolesList = roleModel.GetListOfRoles(false, false);

                var rList = (from e in rolesList
                             select new SelectListItem()
                             {
                                 Text = string.Format("{0} ", e.Description),
                                 Value = e.Role_Id.ToString(CultureInfo.InvariantCulture)
                                 //,
                                 //Selected = e.Head_Of_Department_Id.Equals(Head_Of_Department_Id)
                             }).ToList();

                var selectList = new SelectList(rList, "Value", "Text");

                return selectList;
            }
        }

        [Display(Name = "Supervisor", Description = "The employee's Supervisor")]
        public SelectList Supervisors_List
        {
            get
            {
                var employeeModel = new EmployeeModel();
                var listOfEmployees = employeeModel.GetListOfEmployees(false, false);

                var employeesList = (from e in listOfEmployees
                                     select new SelectListItem()
                                     {
                                         Text = string.Format("{0} {1}", e.User.First_Name, e.User.Last_Name),
                                         Value = e.Employee_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = e.Supervisor_Id.Equals(Supervisor_Id)
                                     }).ToList();

                var selectList = new SelectList(employeesList, "Value", "Text", Supervisor_Id);

                return selectList;
            }
        }

        [Display(Name = "Gender", Description = "The employee's Gender")]
        public SelectList Gender_List
        {
            get
            {
                var genderModel = new GenderModel();
                var listOfGenders = genderModel.GetListOfGenders();

                var gendersList = (from g in listOfGenders
                                   select new SelectListItem()
                                   {
                                       Text = g.Description,
                                       Value = g.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = g.Gender_Id.Equals(Gender_Id)
                                   }).ToList();

                var selectList = new SelectList(gendersList, "Value", "Text", Gender_Id);

                return selectList;
            }
        }

        [Display(Name = "Race", Description = "The employee's Race")]
        public SelectList Race_List
        {
            get
            {
                var raceModel = new RaceModel();
                var listOfRaces = raceModel.GetListOfRaces();

                var racesList = (from r in listOfRaces
                                 select new SelectListItem()
                                 {
                                     Text = r.Description,
                                     Value = r.Race_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = r.Race_Id.Equals(Gender_Id)
                                 }).ToList();

                var selectList = new SelectList(racesList, "Value", "Text", Gender_Id);

                return selectList;
            }
        }

        [Display(Name = "Job Position", Description = "The Employee's Job Position")]
        public SelectList Job_Position_List
        {
            get
            {
                var jobPositionModel = new JobPositionModel();
                var listOfJobPositions = jobPositionModel.GetListOfJobPositions();

                var jobPositionsList = (from r in listOfJobPositions
                                        select new SelectListItem()
                                        {
                                            Text = r.Description,
                                            Value = r.Job_Position_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = r.Job_Position_Id.Equals(Job_Position_Id)
                                        }).ToList();

                var selectList = new SelectList(jobPositionsList, "Value", "Text", Job_Position_Id);

                return selectList;
            }
        }

        [Display(Name = "Pay Point", Description = "The Employee's Pay Point")]
        public SelectList Paypoint_List
        {
            get
            {
                var paypointModel = new PaypointModel();
                var listOfPaypoints = paypointModel.GetListOfPayPoints();

                var paypointsList = (from pp in listOfPaypoints
                                     select new SelectListItem()
                                     {
                                         Text = pp.Description,
                                         Value = pp.Paypoint_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = pp.Paypoint_Id.Equals(Paypoint_Id)
                                     }).ToList();

                var selectList = new SelectList(paypointsList, "Value", "Text", Paypoint_Id);

                return selectList;
            }
        }

        //EmployeeService added by Herman
        [Display(Name = "Service of employee", Description = "The Employee's Service")]
        public SelectList EmployeeService_List
        {
            get
            {
                var employeeServiceModel = new EmployeeServiceModel();
                var listOfEmployeeServices = employeeServiceModel.GetListOfEmployeeServices();

                var employeeServicesList = (from pp in listOfEmployeeServices
                                            select new SelectListItem()
                                            {
                                                Text = pp.Description,
                                                Value = pp.Problem_Category_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = pp.Problem_Category_Id.Equals(Employee_Id)
                                            }).ToList();

                var selectList = new SelectList(employeeServicesList, "Value", "Text", Employee_Id);

                return selectList;
            }
        }

        [Display(Name = "Service Office", Description = "The Employee's Service Office")]
        public SelectList Service_Office_List
        {
            get
            {
                var serviceOfficeModel = new ServiceOfficeModel();
                var listOfServiceOffices = serviceOfficeModel.GetListOfServiceOffices();

                var serviceOfficesList = (from s in listOfServiceOffices
                                          select new SelectListItem()
                                          {
                                              Text = s.Description,
                                              Value = s.Service_Office_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = s.Service_Office_Id.Equals(Service_Office_Id)
                                          }).ToList();

                var selectList = new SelectList(serviceOfficesList, "Value", "Text", Service_Office_Id);

                return selectList;
            }
        }

        [Display(Name = "Salary Level", Description = "The Employee's Salary Level")]
        public SelectList Salary_Level_List
        {
            get
            {
                var salaryLevelModel = new SalaryLevelModel();
                var listOfSalaryLevels = salaryLevelModel.GetListOfSalaryLevels();

                var salaryLevelsList = (from s in listOfSalaryLevels
                                        select new SelectListItem()
                                        {
                                            Text = s.Description,
                                            Value = s.Salary_Level_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = s.Salary_Level_Id.Equals(Salary_Level_Id)
                                        }).ToList();

                var selectList = new SelectList(salaryLevelsList, "Value", "Text", Salary_Level_Id);

                return selectList;
            }
        }

        [Display(Name = "Practice Number", Description = "The Social Worker's Practice Number")]
        public string PracticeNumber { get; set; }

        public List<int> SelectedEmployeeServiceList { get; set; }

        public List<int> SelectedEmployeeRoleList { get; set; }
    }

    // Helper class to make posting back selected values easier
    public class PostedRoles
    {
        public string[] Role_IDs { get; set; }
    }

    // Helper class to make posting back selected values easier
    public class PostedGroups
    {
        public string[] Group_IDs { get; set; }
    }

    [MetadataType(typeof(RoleMetadata))]
    public partial class Role
    { }

    [MetadataType(typeof(RoleDelegationMetadata))]
    public partial class User_Role_Delegation
    {
        [Display(Name = "Delegated From")]
        public SelectList Delegated_From_User_List
        {
            get
            {
                var userModel = new UserModel();
                var listOfUsers = userModel.GetListOfUsers(false, false);

                var usersList = (from e in listOfUsers
                                 select new SelectListItem()
                                 {
                                     Text = string.Format("{0} {1}", e.First_Name, e.Last_Name),
                                     Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = e.User_Id.Equals(From_User_Id)
                                 }).ToList();

                var selectList = new SelectList(usersList, "Value", "Text", From_User_Id);

                return selectList;
            }
        }

        public string Delegated_From_User_Name
        {
            get
            {
                return string.Format("{0} {1}", Delegated_From_User.First_Name, Delegated_From_User.Last_Name);
            }
        }

        [Display(Name = "Delegated To")]
        public SelectList Delegated_To_User_List
        {
            get
            {
                var userModel = new UserModel();
                var listOfUsers = userModel.GetListOfUsers(false, false);

                var usersList = (from e in listOfUsers
                                 select new SelectListItem()
                                 {
                                     Text = string.Format("{0} {1}", e.First_Name, e.Last_Name),
                                     Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = e.User_Id.Equals(To_User_Id)
                                 }).ToList();

                var selectList = new SelectList(usersList, "Value", "Text", To_User_Id);

                return selectList;
            }
        }

        public string Delegated_To_User_Name
        {
            get
            {
                return string.Format("{0} {1}", Delegated_To_User.First_Name, Delegated_To_User.Last_Name);
            }
        }
    }

    [MetadataType(typeof(GroupMetadata))]
    public partial class Group
    {
        public List<Role> Available_Roles
        {
            get
            {
                var roleModel = new RoleModel();
                var rolesList = roleModel.GetListOfRoles(false, false);

                return rolesList;
            }
        }

        public PostedRoles Posted_Roles { get; set; }
    }

    [MetadataType(typeof(MenuMetadata))]
    public partial class Menu
    { }

    [MetadataType(typeof(MenuItemMetadata))]
    public partial class Menu_Item
    {
        public int? Module_Id { get; set; }

        [Display(Name = "Destination Module", Description = "The Destination Module this item should link to when clicked")]
        public SelectList Module_List
        {
            get
            {
                var moduleModel = new ModuleModel();
                var listOfModules = moduleModel.GetListOfModules(false, false);

                var modulesList = (from m in listOfModules
                                   select new SelectListItem()
                                   {
                                       Text = m.Description,
                                       Value = m.Module_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = m.Module_Id.Equals(Module_Id)
                                   }).ToList();

                var selectList = new SelectList(modulesList, "Value", "Text", Module_Id);

                return selectList;
            }
        }

        public int? Module_Controller_Id { get; set; }

        [Display(Name = "Destination Controller", Description = "The Destination Controller this item should link to when clicked")]
        public SelectList Module_Controller_List
        {
            get
            {
                var moduleControllerModel = new ModuleControllerModel();
                var listOfModuleControllers = moduleControllerModel.GetListOfModuleControllers(false, false);

                listOfModuleControllers.RemoveAll(c => c.Module_Id != (Module_Id ?? -1));

                var controllersList = (from c in listOfModuleControllers
                                       select new SelectListItem()
                                       {
                                           Text = c.Module_Controller_Name,
                                           Value = c.Module_Controller_Id.ToString(CultureInfo.InvariantCulture),
                                           Selected = c.Module_Controller_Id.Equals(Module_Controller_Id)
                                       }).ToList();

                var selectList = new SelectList(controllersList, "Value", "Text", Module_Controller_Id);

                return selectList;
            }
        }

        [Display(Name = "Destination Action", Description = "The Destination Action this item should link to when clicked")]
        public SelectList Module_Action_List
        {
            get
            {
                var moduleActionModel = new ModuleActionModel();
                var listOfModuleActions = moduleActionModel.GetListOfModuleActions(false, false);

                listOfModuleActions.RemoveAll(x => x.Module_Controller_Id != (Module_Controller_Id ?? -1));

                var actionsList = (from a in listOfModuleActions
                                   select new SelectListItem()
                                   {
                                       Text = a.Module_Action_Name,
                                       Value = a.Module_Action_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = a.Module_Action_Id.Equals(Module_Action_Id)
                                   }).ToList();

                var selectList = new SelectList(actionsList, "Value", "Text", Module_Action_Id);

                return selectList;
            }
        }

        [Display(Name = "Container Menu", Description = "The Menu Structure this item should be a child of")]
        public SelectList Container_Menu_List
        {
            get
            {
                var menuModel = new MenuModel();
                var listOfMenus = menuModel.GetListOfMenus(false, false);

                var menusList = (from m in listOfMenus
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Menu_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Menu_Id.Equals(Menu_Id)
                                 }).ToList();

                var selectList = new SelectList(menusList, "Value", "Text", Menu_Id);

                return selectList;
            }
        }

        [Display(Name = "Parent MenuItem", Description = "The Menu item this item should be a child of")]
        public SelectList Parent_Menu_Item_List
        {
            get
            {
                var menuItemModel = new MenuItemModel();
                var listOfMenuItems = menuItemModel.GetListOfMenuItems(false, false);

                var menuItemsList = (from mi in listOfMenuItems
                                     where mi.Parent_Menu_Item == null
                                     select new SelectListItem()
                                     {
                                         Text = mi.Menu_Text,
                                         Value = mi.Menu_Item_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = mi.Parent_Menu_Item_Id.Equals(Parent_Menu_Item_Id)
                                     }).ToList();

                var selectList = new SelectList(menuItemsList, "Value", "Text", Parent_Menu_Item_Id);

                return selectList;
            }
        }

        public List<Role> Available_Roles
        {
            get
            {
                var roleModel = new RoleModel();
                var rolesList = roleModel.GetListOfRoles(false, false);

                return rolesList;
            }
        }

        public PostedRoles Posted_Roles { get; set; }

        [Display(Name = "Is Visible?", Description = "Indicates if the current Menu Item will be visible based on the logged-in user's roles")]
        public bool Is_Visible { get; set; }
    }

    [MetadataType(typeof(ModuleMetadata))]
    public partial class Module
    { }

    [MetadataType(typeof(ModuleControllerMetadata))]
    public partial class Module_Controller
    {
        [Display(Name = "Module", Description = "The Module this item should be a child of")]
        public SelectList Module_List
        {
            get
            {
                var moduleModel = new ModuleModel();
                var listOfModules = moduleModel.GetListOfModules(false, false);

                var modulesList = (from m in listOfModules
                                   select new SelectListItem()
                                   {
                                       Text = m.Description,
                                       Value = m.Module_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = m.Module_Id.Equals(Module_Id)
                                   }).ToList();

                var selectList = new SelectList(modulesList, "Value", "Text", Module_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(ModuleActionMetadata))]
    public partial class Module_Action
    {
        public int? Module_Id { get; set; }

        [Display(Name = "Destination Module", Description = "The Destination Module this item should link to when clicked")]
        public SelectList Module_List
        {
            get
            {
                var moduleModel = new ModuleModel();
                var listOfModules = moduleModel.GetListOfModules(false, false);

                var modulesList = (from m in listOfModules
                                   select new SelectListItem()
                                   {
                                       Text = m.Description,
                                       Value = m.Module_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = m.Module_Id.Equals(Module_Id)
                                   }).ToList();

                var selectList = new SelectList(modulesList, "Value", "Text", Module_Id);

                return selectList;
            }
        }

        [Display(Name = "Controller", Description = "The Controller this item should be a child of")]
        public SelectList Module_Controller_List
        {
            get
            {
                var moduleControllerModel = new ModuleControllerModel();
                var listOfModuleControllers = moduleControllerModel.GetListOfModuleControllers(false, false);

                listOfModuleControllers.RemoveAll(x => x.Module_Id != (Module_Id ?? -1));

                var moduleControllersList = (from m in listOfModuleControllers
                                             select new SelectListItem()
                                             {
                                                 Text = m.Module_Controller_Name,
                                                 Value = m.Module_Controller_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = m.Module_Controller_Id.Equals(Module_Controller_Id)
                                             }).ToList();

                var selectList = new SelectList(moduleControllersList, "Value", "Text", Module_Controller_Id);

                return selectList;
            }
        }

        public List<Role> Available_Roles
        {
            get
            {
                var roleModel = new RoleModel();
                var rolesList = roleModel.GetListOfRoles(false, false);

                return rolesList;
            }
        }

        public PostedRoles Posted_Roles { get; set; }
    }

    [MetadataType(typeof(OrganizationMetadata))]
    public partial class Organization
    { }

    [MetadataType(typeof(ReceptionRegisterMetadata))]
    public partial class Reception_Register
    {
        [Display(Name = "Visit Type", Description = "Classification of the type of visit")]
        public SelectList Reception_Vist_Type_List
        {
            get
            {
                var receptionVisitTypeModel = new ReceptionVisitTypeModel();
                var listOfReceptionVisitTypes = receptionVisitTypeModel.GetListOfReceptionVisitTypes();

                var visitTypes = (from m in listOfReceptionVisitTypes
                                  select new SelectListItem()
                                  {
                                      Text = m.Description,
                                      Value = m.Reception_Visit_Type_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = m.Reception_Visit_Type_Id.Equals(Reception_Visit_Type_Id)
                                  }).ToList();

                var selectList = new SelectList(visitTypes, "Value", "Text", Reception_Visit_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Action Taken", Description = "Action taken for visit")]
        public SelectList Reception_Action_Taken_List
        {
            get
            {
                var receptionActionTakenModel = new ReceptionActionTakenModel();
                var listOfReceptionActionTakenItems = receptionActionTakenModel.GetListOfReceptionActionTakenItems();

                var actionItems = (from m in listOfReceptionActionTakenItems
                                   select new SelectListItem()
                                   {
                                       Text = m.Description,
                                       Value = m.Reception_Action_Taken_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = m.Reception_Action_Taken_Id.Equals(Reception_Action_Taken_Id)
                                   }).ToList();

                var selectList = new SelectList(actionItems, "Value", "Text", Reception_Action_Taken_Id);

                return selectList;
            }
        }
    }

    public partial class InboxGridItem
    {
        public int assessmentId { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
        public string clientDateOfBirth { get; set; }
        public DateTime assessmentDate { get; set; }
        public string assessedBy { get; set; }
    }

    public partial class ClientGridMain
    {
        public int PersonId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public int AssessmentCount { get; set; }
        public List<ClientGridNested> NestedItems { get; set; }
    }

    public partial class CycaClientGridMain
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public string childGender { get; set; }
        public DateTime? birthdate { get; set; }
        public string INTAKERefNumber { get; set; }
        public string PCMRefNumber { get; set; }
        public string CYCARefNumber { get; set; }
        public int PCMCaseId { get; set; }
        public string ProbationOfficer { get; set; }
        public string AdmissionType { get; set; }
        public int AssessmentCount { get; set; }
        public string FullName { get; set; }
        public List<CycaClientGridNested> NestedItems { get; set; }
    }

    public partial class CycaClientGridNested
    {
        public int AssessmentId { get; set; }
        public int PersonId { get; set; }
        public int clientId { get; set; }
        public DateTime? AssessmentDate { get; set; }
        public DateTime? AssessmentTime { get; set; }
        public string INTAKERefNumber { get; set; }
        public string PCMRefNumber { get; set; }
        public string ProvinceRefNumber { get; set; }
    }


    public partial class UnsuitabilityGridMain
    {
        public int UnsuitabilityId { get; set; }

        public string Lastname { get; set; }
        public string FirstName { get; set; }
        public string Recnumber { get; set; }
        public string FindingBy { get; set; }
        public string TypeOfOffence { get; set; }
        public DateTime? NotificationDate { get; set; }
        public string City_Town { get; set; }
        public string MagisterialDistrict { get; set; }
    }

    public partial class ClientGridNested
    {
        public int AssessmentId { get; set; }
        public int PersonId { get; set; }
        public DateTime? AssessmentDate { get; set; }
        public DateTime? AssessmentTime { get; set; }
    }

    public partial class PersonGridMain
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public int AssessmentCount { get; set; }
        public List<IncidentGridNested> NestedItems { get; set; }
    }

    [MetadataType(typeof(PersonMetadata))]
    public partial class Person
    {
        public string Identification_Type_Description
        {
            get
            {
                return Identification_Type_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Identification_Type_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Identification Type")]
        public SelectList Identification_Type_List
        {
            get
            {
                var identificationTypeModel = new IdentificationTypeModel();
                var listOfIdentificationTypes = identificationTypeModel.GetListOfIdentificationTypes();

                var identificationTypes = (from m in listOfIdentificationTypes
                                           select new SelectListItem()
                                           {
                                               Text = m.Description,
                                               Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = m.Identification_Type_Id.Equals(Identification_Type_Id)
                                           }).ToList();

                var selectList = new SelectList(identificationTypes, "Value", "Text", Identification_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Language Type")]
        public SelectList Language_Type_List
        {
            get
            {
                var languageModel = new LanguageModel();
                var listOfLanguages = languageModel.GetListOfLanguages();

                var languages = (from m in listOfLanguages
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Language_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Language_Id.Equals(Language_Id)
                                 }).ToList();

                var selectList = new SelectList(languages, "Value", "Text", Language_Id);

                return selectList;
            }
        }

        [Display(Name = "Sexual Orientation")]
        public SelectList SexualOrientation_List
        {
            get
            {
                var sexualOrientationModel = new VEP_SexualOrientationModel();
                var listOfsexualOrientation = sexualOrientationModel.GetListOfsexualOrientation();

                var sexualOrientationList = (from g in listOfsexualOrientation
                                             select new SelectListItem()
                                             {
                                                 Text = g.Sexual_Orientation,
                                                 Value = g.Sexual_Orientation_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = g.Sexual_Orientation_Id.Equals(Sexual_Orientation_Id)
                                             }).ToList();

                var selectList = new SelectList(sexualOrientationList, "Value", "Text", Sexual_Orientation_Id);

                return selectList;
            }
        }

        [Display(Name = "Citizenship")]
        public SelectList Citizenship_List
        {
            get
            {
                var CitizenshipModel = new CitizenshipModel();
                var listOfCitizenship = CitizenshipModel.GetListOfCitizenship();

                var CitizenshipModelList = (from g in listOfCitizenship
                                            select new SelectListItem()
                                            {
                                                Text = g.Description,
                                                Value = g.Citizenship_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = g.Citizenship_Id.Equals(Citizenship_Id)
                                            }).ToList();

                var selectList = new SelectList(CitizenshipModelList, "Value", "Text", Citizenship_Id);

                return selectList;
            }
        }

        [Display(Name = "Gender")]
        public SelectList Gender_List
        {
            get
            {
                var genderModel = new GenderModel();
                var listOfGenders = genderModel.GetListOfGenders();

                var gendersList = (from g in listOfGenders
                                   select new SelectListItem()
                                   {
                                       Text = g.Description,
                                       Value = g.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = g.Gender_Id.Equals(Gender_Id)
                                   }).ToList();

                var selectList = new SelectList(gendersList, "Value", "Text", Gender_Id);

                return selectList;
            }
        }

        [Display(Name = "Gender")]
        public string Gender_Description
        {
            get
            {
                return Gender_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Gender_List.First(x => x.Selected).Text;
            }
        }

        /* [Display(Name = "Closure_Reasons")]
         public SelectList Closure_Reasons_List
         {
             get
             {
                 var closure_ReasonsModel = new CaseClosureReasonModel();
                 var listOfClosure_Reasonses = closure_ReasonsModel.GetListOfClosure_Reasonses();

                 var closure_ReasonsesList = (from g in listOfClosure_Reasonses
                                             select new SelectListItem()
                                             {
                                                 Text = g.Description,
                                                 Value = g.Case_Closure_Reason_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = g.Case_Closure_Reason_Id.Equals(Case_Closure_Reason_Id)
                                             }).ToList();

                 var selectList = new SelectList(closure_ReasonsesList, "Value", "Text", Case_Closure_Reason_Id);

                 return selectList;
             }
         }

         [Display(Name = "Closure_Reasons")]
         public string Closure_Reasons_Description
         {
             get
             {
                 return Closure_Reasons_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Closure_Reasons_List.First(x => x.Selected).Text;
             }
         }*/

        [Display(Name = "Marital Status")]
        public SelectList Marital_Status_List
        {
            get
            {
                var maritalStatusModel = new MaritalStatusModel();
                var listOfMaritalStatusses = maritalStatusModel.GetListOfMaritalStatusses();

                var maritalStatussesList = (from g in listOfMaritalStatusses
                                            select new SelectListItem()
                                            {
                                                Text = g.Description,
                                                Value = g.Marital_Status_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = g.Marital_Status_Id.Equals(Marital_Status_Id)
                                            }).ToList();

                var selectList = new SelectList(maritalStatussesList, "Value", "Text", Marital_Status_Id);

                return selectList;
            }
        }

        [Display(Name = "Marital Status")]
        public string Marital_Status_Description
        {
            get
            {
                return Marital_Status_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Marital_Status_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Preferred Contact Type")]
        public SelectList Preferred_Contact_Type_List
        {
            get
            {
                var preferredContactTypeModel = new PreferredContactTypeModel();
                var listOfPreferredContactTypes = preferredContactTypeModel.GetListOfPreferredContactTypes();

                var preferredContactTypesList = (from g in listOfPreferredContactTypes
                                                 select new SelectListItem()
                                                 {
                                                     Text = g.Description,
                                                     Value = g.Preferred_Contact_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                     Selected = g.Preferred_Contact_Type_Id.Equals(Preferred_Contact_Type_Id)
                                                 }).ToList();

                var selectList = new SelectList(preferredContactTypesList, "Value", "Text", Gender_Id);

                return selectList;
            }
        }

        [Display(Name = "Religion")]
        public SelectList Religion_List
        {
            get
            {
                var religionModel = new ReligionModel();
                var listOfReligions = religionModel.GetListOfReligions();

                var religionsList = (from g in listOfReligions
                                     select new SelectListItem()
                                     {
                                         Text = g.Description,
                                         Value = g.Religion_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = g.Religion_Id.Equals(Religion_Id)
                                     }).ToList();

                var selectList = new SelectList(religionsList, "Value", "Text", Religion_Id);

                return selectList;
            }
        }

        [Display(Name = "Religion")]
        public string Religion_Description
        {
            get
            {
                return Religion_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Religion_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Population Group")]
        public SelectList Population_Group_List
        {
            get
            {
                var populationGroupModel = new PopulationGroupModel();
                var listOfPopulationGroups = populationGroupModel.GetListOfPopulationGroups();

                var populationGroupsList = (from g in listOfPopulationGroups
                                            select new SelectListItem()
                                            {
                                                Text = g.Description,
                                                Value = g.Population_Group_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = g.Population_Group_Id.Equals(Population_Group_Id)
                                            }).ToList();

                var selectList = new SelectList(populationGroupsList, "Value", "Text", Population_Group_Id);

                return selectList;
            }
        }

        [Display(Name = "Population Group")]
        public string Population_Group_Description
        {
            get
            {
                return Population_Group_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Population_Group_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Nationality")]
        public SelectList Nationality_List
        {
            get
            {
                var nationalityModel = new NationalityModel();
                var listOfNationalities = nationalityModel.GetListOfNationalities();

                var nationalitiesList = (from g in listOfNationalities
                                         select new SelectListItem()
                                         {
                                             Text = g.Description,
                                             Value = g.Nationality_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = g.Nationality_Id.Equals(Nationality_Id)
                                         }).ToList();

                var selectList = new SelectList(nationalitiesList, "Value", "Text", Population_Group_Id);

                return selectList;
            }
        }

        [Display(Name = "Nationality")]
        public string Nationality_Description
        {
            get
            {
                return Nationality_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Nationality_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Disability")]
        public SelectList Disability_List
        {
            get
            {
                var disabilityModel = new DisabilityModel();
                var listOfDisabilities = disabilityModel.GetListOfDisabilities();

                var disabilitiesList = (from g in listOfDisabilities
                                        select new SelectListItem()
                                        {
                                            Text = g.Description,
                                            Value = g.Disability_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = g.Disability_Id.Equals(Disability_Type_Id)
                                        }).ToList();

                var selectList = new SelectList(disabilitiesList, "Value", "Text", Disability_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Disability")]
        public string Disability_Description
        {
            get
            {
                return Disability_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Disability_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Date of Birth")]
        public string Date_Of_Birth_With_Age
        {
            get
            {
                if (Date_Of_Birth == null)
                    return null;

                var now = DateTime.Today;
                var bday = (DateTime)Date_Of_Birth;

                var age = now.Year - bday.Year;
                if (bday > now.AddYears(-age)) age--;

                return string.Format("{0} ({1} years)", bday.ToString("dd MMM yyyy"), age.ToString(CultureInfo.InvariantCulture));
            }
        }

        [Display(Name = "Current Photo")]
        public Person_Image Current_Photo
        {
            get
            {
                return Images.Any() ? Images.Where(x => x.Image_Type_Id.Equals((int)ImageTypeEnum.ProfilePhoto)).OrderByDescending(x => x.Date_Created).FirstOrDefault() : new Person_Image();
            }
        }

        [Display(Name = "Upload New Photo")]
        public HttpPostedFileBase Posted_Photo { get; set; }
    }

    //[MetadataType(typeof(VEPSexualOrientationMetaData))]
    //public partial class VEP_Sexual_Orientation
    //{
    //    public int? Selected_SexualOrientation_Id { get; set; }

    //    public SelectList SexualOrientation_List
    //    {
    //        get
    //        {
    //            var sexualOrientationModel = new VEP_SexualOrientationModel();
    //            var listOfsexualOrientation = sexualOrientationModel.GetListOfsexualOrientation();

    //            var sexualOrientationList = (from g in listOfsexualOrientation
    //                                         select new SelectListItem()
    //                                         {
    //                                             Text = g.Sexual_Orientation,
    //                                             Value = g.Sexual_Orientation_Id.ToString(CultureInfo.InvariantCulture),
    //                                             Selected = g.Sexual_Orientation_Id.Equals(Selected_SexualOrientation_Id)
    //                                         }).ToList();

    //            var selectList = new SelectList(sexualOrientationList, "Value", "Text", Selected_SexualOrientation_Id);

    //            return selectList;
    //        }
    //    }

    //    [Display(Name = "Sexual Orientation")]
    //    public string SexualOrientation_Description
    //    {
    //        get
    //        {
    //            return SexualOrientation_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : SexualOrientation_List.First(x => x.Selected).Text;
    //        }
    //    }
    //}

    [MetadataType(typeof(AddressMetadata))]
    public partial class Address
    {
        public int? Selected_Province_Id { get; set; }

        public int? Selected_Municipality_Id { get; set; }

        public int? Selected_Local_Municipality_Id { get; set; }

        public int? SelectedMagistrateDistrict_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provinces = (from x in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Province_Id.Equals(Selected_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }

        [Display(Name = "District Municipality")]
        public SelectList District_Municipality_List
        {
            get
            {
                var districtMunicipalityModel = new DistrictModel();
                var listOfDistrictMunicipalities = districtMunicipalityModel.GetListOfDistricts(Selected_Province_Id ?? -1);

                var municipalities = (from x in listOfDistrictMunicipalities
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.District_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.District_Id.Equals(Selected_Municipality_Id)
                                      }).ToList();

                var selectList = new SelectList(municipalities, "Value", "Text", Selected_Municipality_Id);

                return selectList;
            }
        }

        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Municipality_Id ?? -1);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Local_Municipality_Id.Equals(Selected_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Selected_Local_Municipality_Id);

                return selectList;
            }
        }

        [Display(Name = "Town")]
        public SelectList Town_List
        {
            get
            {
                var townModel = new TownModel();
                var listOfTowns = townModel.GetListOfTowns(Selected_Local_Municipality_Id ?? -1);

                var towns = (from m in listOfTowns
                             select new SelectListItem()
                             {
                                 Text = m.Description,
                                 Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = m.Town_Id.Equals(Town_Id)
                             }).ToList();

                var selectList = new SelectList(towns, "Value", "Text", Town_Id);

                return selectList;
            }
        }
    }

    //public int? ServiceTypeId { get; set; }

    [MetadataType(typeof(ServiceTypeMetadata))]
    public partial class Client_Services
    {
        [Display(Name = "Service Type")]

        public int ServiceType_Id { get; set; }
        public SelectList Service_List
        {
            get
            {
                var serviceModel = new VEPServiceTypeModel();
                var listOfSchools = serviceModel.GetListOfServiceType();

                var services = (from m in listOfSchools
                                select new SelectListItem()
                                {
                                    Text = m.ServiceType,
                                    Value = m.ServiceTypeId.ToString(CultureInfo.InvariantCulture),
                                    Selected = m.ServiceTypeId.Equals(ServiceType_Id)
                                }).ToList();

                var selectList = new SelectList(listOfSchools, "Value", "Text", ServiceType_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(VEPSettlementMetadata))]
    public partial class VEP_SettlementType
    {
        [Display(Name = "Settlement Type")]

        public int SettlementType_Id { get; set; }
        public SelectList SettlementType_List
        {
            get
            {
                var settlementTypeModel = new VEPSettlementTypeModel();
                var listOfSettlement = settlementTypeModel.GetListOfSettlementType();

                var towns = (from m in listOfSettlement
                             select new SelectListItem()
                             {
                                 Text = m.Settlement,
                                 Value = m.Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = m.Id.Equals(SettlementType_Id)
                             }).ToList();

                var selectList = new SelectList(towns, "Value", "Text", SettlementType_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(PersonEducationMetadata))]
    public partial class Person_Education
    {
        [Display(Name = "School")]
        public SelectList School_List
        {
            get
            {
                var schoolModel = new SchoolModel();
                var listOfSchools = schoolModel.GetListOfSchools();

                var schools = (from m in listOfSchools
                               select new SelectListItem()
                               {
                                   Text = m.School_Name,
                                   Value = m.School_Id.ToString(CultureInfo.InvariantCulture)
                               }).ToList();

                var selectList = new SelectList(schools, "Value", "Text");

                return selectList;
            }
        }

        [Display(Name = "Last Grade Completed")]
        public SelectList Grade_Completed_List
        {
            get
            {
                var gradeModel = new GradeModel();
                var listOfGrades = gradeModel.GetListOfGrades();

                var grades = (from m in listOfGrades
                              select new SelectListItem()
                              {
                                  Text = m.Description,
                                  Value = m.Grade_Id.ToString(CultureInfo.InvariantCulture),
                                  Selected = m.Grade_Id.Equals(Grade_Completed_Id)
                              }).ToList();

                var selectList = new SelectList(grades, "Value", "Text", Grade_Completed_Id);

                return selectList;
            }
        }


        [Display(Name = "Education Type")]
        public SelectList School_List_Type
        {
            get
            {
                var schoolModel = new School_TypeModel();
                var listOfSchooltype = schoolModel.GetListOfSchooltype();

                var schools = (from m in listOfSchooltype
                               select new SelectListItem()
                               {
                                   Text = m.Description,
                                   Value = m.School_Type_Id.ToString(CultureInfo.InvariantCulture)
                               }).ToList();

                var selectList = new SelectList(schools, "Value", "Text");

                return selectList;
            }
        }

        public int? Selected_Province_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provinces = (from x in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Province_Id.Equals(Selected_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(VEPServicesTypeMetadata))]
    public partial class VEP_Services
    {
        [Display(Name = "Services")]
        public SelectList Service_List
        {
            get
            {
                var serviceModel = new VEPServiceTypeModel();
                var listOfService = serviceModel.GetListOfServiceType();

                var services = (from m in listOfService
                                select new SelectListItem()
                                {
                                    Text = m.ServiceType,
                                    Value = m.ServiceTypeId.ToString(CultureInfo.InvariantCulture)
                                }).ToList();

                var selectList = new SelectList(services, "Value", "Text");

                return selectList;
            }
        }
    }

    [MetadataType(typeof(VEPReferalMetadata))]
    public partial class VEP_Referals
    {
        [Display(Name = "Referals")]
        public SelectList Referals_List
        {
            get
            {
                var referalModel = new VEPReferralType();
                var listOfReferals = referalModel.GetListOfReferralType();

                var referals = (from m in listOfReferals
                                select new SelectListItem()
                                {
                                    Text = m.Description,
                                    Value = m.Department_Id.ToString(CultureInfo.InvariantCulture)
                                }).ToList();

                var selectList = new SelectList(referals, "Value", "Text");

                return selectList;
            }
        }
    }


    [MetadataType(typeof(PersonEmploymentMetadata))]
    public partial class Person_Employment
    {
        [Display(Name = "Nature of Employment")]
        public SelectList Nature_Of_Employment_List
        {
            get
            {
                var natureOfEmploymentModel = new NatureOfEmploymentModel();
                var listOfNatureOfEmploymentItems = natureOfEmploymentModel.GetListOfNatureOfEmploymentItems();

                var natureOfEmploymentItems = (from m in listOfNatureOfEmploymentItems
                                               select new SelectListItem()
                                               {
                                                   Text = m.Description,
                                                   Value = m.Nature_of_Employment_Id.ToString(CultureInfo.InvariantCulture),
                                                   Selected = m.Nature_of_Employment_Id.Equals(Nature_Of_Employment_Id)

                                               }).ToList();

                var selectList = new SelectList(natureOfEmploymentItems, "Value", "Text", Nature_Of_Employment_Id);

                return selectList;
            }
        }

        [Display(Name = "Income Range")]
        public SelectList Income_Range_List
        {
            get
            {
                var incomeRangeModel = new IncomeRangeModel();
                var listOfIncomeRanges = incomeRangeModel.GetListOfIncomeRanges();

                var incomeRanges = (from m in listOfIncomeRanges
                                    select new SelectListItem()
                                    {
                                        Text = m.Description,
                                        Value = m.Income_Range_Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = m.Income_Range_Id.Equals(Income_Range_Id)
                                    }).ToList();

                var selectList = new SelectList(incomeRanges, "Value", "Text", Income_Range_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(SchoolMetadata))]
    public partial class School
    {

    }

    [MetadataType(typeof(IntakeAssessmentMetadata))]
    public partial class Intake_Assessment
    {
        public int? Problem_Category_Id { get; set; }

        [Display(Name = "Problem Category")]
        public SelectList Problem_Category_List
        {
            get
            {
                var problemCategoryModel = new ProblemCategoryModel();
                var listOfProblemCategories = problemCategoryModel.GetListOfProblemCategories();

                var problemCategories = (from p in listOfProblemCategories
                                         select new SelectListItem()
                                         {
                                             Text = p.Description,
                                             Value = p.Problem_Category_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = p.Problem_Category_Id.Equals(Problem_Category_Id)
                                         }).ToList();

                var selectList = new SelectList(problemCategories, "Value", "Text", Problem_Category_Id);

                return selectList;
            }
        }

        [Display(Name = "Problem Sub-Category")]
        public SelectList Problem_Sub_Category_List
        {
            get
            {
                var problemSubCategoryModel = new ProblemSubCategoryModel();
                var listOfProblemSubCategories = problemSubCategoryModel.GetListOfProblemSubCategories();

                listOfProblemSubCategories.RemoveAll(c => c.Problem_Category_Id != (Problem_Category_Id ?? -1));

                var problemSubCategories = (from p in listOfProblemSubCategories
                                            select new SelectListItem()
                                            {
                                                Text = p.Description,
                                                Value = p.Problem_Sub_Category_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = p.Problem_Sub_Category_Id.Equals(Problem_Sub_Category_Id)
                                            }).ToList();

                var selectList = new SelectList(problemSubCategories, "Value", "Text", Problem_Sub_Category_Id);

                return selectList;
            }
        }

        [Display(Name = "Referring Organization")]
        public SelectList Referred_From_Organization_List
        {
            get
            {
                var organizationModel = new OrganizationModel();
                var listOfOrganizations = organizationModel.GetListOfOrganizations(false, false);

                var organizations = (from o in listOfOrganizations
                                     select new SelectListItem()
                                     {
                                         Text = o.Description,
                                         Value = o.Organization_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = o.Organization_Id.Equals(Referred_From_Organization_Id)
                                     }).ToList();

                var selectList = new SelectList(organizations, "Value", "Text", Referred_From_Organization_Id);

                return selectList;
            }
        }

        [Display(Name = "Organization Referred To")]
        public SelectList Referred_To_Organization_List
        {
            get
            {
                var organizationModel = new OrganizationModel();
                var listOfOrganizations = organizationModel.GetListOfOrganizations(false, false);

                var organizations = (from o in listOfOrganizations
                                     select new SelectListItem()
                                     {
                                         Text = o.Description,
                                         Value = o.Organization_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = o.Organization_Id.Equals(Referred_To_Organization_Id)
                                     }).ToList();

                var selectList = new SelectList(organizations, "Value", "Text", Referred_To_Organization_Id);

                return selectList;
            }
        }

        public List<Referral_Focus_Area> Available_Referral_Focus_Areas
        {
            get
            {
                var referralFocusAreaModel = new ReferralFocusAreaModel();
                var focusAreasList = referralFocusAreaModel.GetListOfReferralFocusAreas();

                return focusAreasList;
            }
        }

        public List<VEP_VictimizationType> Available_Victimization_Types
        {
            get
            {
                var victimizationType = new VEPVictimizationTypeModel();
                var victimizationTypeList = victimizationType.GetListOfVictimizationType();

                return victimizationTypeList;
            }
        }

        public PostedReferralFocusAreas Posted_Referral_Focus_Areas { get; set; }

        public PostedClientVictimizationTypes Posted_Client_Victimization_Types { get; set; }

        [Display(Name = "Allocated Supervisor")]
        public SelectList Case_Manager_Supervisor_List
        {

            get {
                var employeeModel = new EmployeeModel();
                var userM = new UserModel();

                var listOfEmployees = employeeModel.GetListOfEmployees(false, false);

                var employees = (from e in listOfEmployees
                                 select new SelectListItem()
                                 {
                                     Text = string.Format("{0} {1}", e.User.First_Name, e.User.Last_Name),
                                     Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = e.User_Id.Equals(Case_Manager_Supervisor_Id)
                                 }).ToList();

                var selectList = new SelectList(employees, "Value", "Text", Case_Manager_Supervisor_Id);

                return selectList;
            }
        }
        //public SelectList Case_Manager_Supervisor_List
        //{
        //    get
        //    {
        //        var assessedById = Assessed_By_Id;

        //        if (assessedById == null)
        //        {
        //            // For whatever reason, there's no assessed by id present, which means we cannot derive the service office, so just return a list of employees (this should be handled better)
        //            var employeeModel = new EmployeeModel();
        //            var listOfEmployees = employeeModel.GetListOfEmployees(false, false);

        //            var employees = (from e in listOfEmployees
        //                             select new SelectListItem()
        //                             {
        //                                 Text = string.Format("{0} {1}", e.User.First_Name, e.User.Last_Name),
        //                                 Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
        //                                 Selected = e.User_Id.Equals(Case_Manager_Supervisor_Id)
        //                             }).ToList();

        //            var selectList = new SelectList(employees, "Value", "Text", Case_Manager_Supervisor_Id);

        //            return selectList;
        //        }
        //        else
        //        {
        //            // Get the service office for the user indicated by Assessed By
        //            var userModel = new UserModel();
        //            var assessedByUser = userModel.GetSpecificUser((int)assessedById);

        //            var serviceOfficeId = -1;

        //            // Is the user a Social Worker?
        //            if (assessedByUser.apl_Social_Worker.Any())
        //            {
        //                var socialWorker = assessedByUser.apl_Social_Worker.First(x => x.User_Id.Equals(assessedByUser.User_Id));
        //                serviceOfficeId = socialWorker.Service_Office_Id;
        //            }
        //            // Is the user an Employee?
        //            if (assessedByUser.Employees.Any())
        //            {
        //                var employee = assessedByUser.Employees.First(x => x.User_Id.Equals(assessedByUser.User_Id));
        //                serviceOfficeId = employee.Service_Office_Id;
        //            }

        //            // Get social workers linked to service office
        //            var socialworkerModel = new SocialWorkerModel();
        //            var listOfSocialWorkers = socialworkerModel.GetListOfSocialWorkers(false, false, serviceOfficeId);

        //            listOfSocialWorkers.RemoveAll(x => x.Reports_To_Social_Worker_Id != null);

        //            var socialWorkers = (from e in listOfSocialWorkers
        //                                 select new SelectListItem()
        //                                 {
        //                                     Text = string.Format("{0} {1}", e.apl_User.First_Name, e.apl_User.Last_Name),
        //                                     Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
        //                                     Selected = e.User_Id.Equals(Case_Manager_Supervisor_Id)
        //                                 }).ToList();

        //            var selectList = new SelectList(socialWorkers, "Value", "Text", Case_Manager_Supervisor_Id);

        //            return null;
        //        }
        //    }
        //}

        [Display(Name = "Allocated Case Manager")]
        public SelectList Case_Manager_List
        {
            get
            {
                //var employeeModel = new EmployeeModel();
                //var listOfEmployees = employeeModel.GetListOfEmployees(false, false);

                //var employees = (from e in listOfEmployees
                //                 select new SelectListItem()
                //                 {
                //                     Text = string.Format("{0} {1}", e.User.First_Name, e.User.Last_Name),
                //                     Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
                //                     Selected = e.User_Id.Equals(Case_Manager_Id)
                //                 }).ToList();

                //var selectList = new SelectList(employees, "Value", "Text", Case_Manager_Id);

                //return selectList;

                var assessedById = Assessed_By_Id;

                if (assessedById == null)
                {
                    // For whatever reason, there's no assessed by id present, which means we cannot derive the service office, so just return a list of employees (this should be handled better)
                    var employeeModel = new EmployeeModel();
                    var listOfEmployees = employeeModel.GetListOfEmployees(false, false);

                    var employees = (from e in listOfEmployees
                                     select new SelectListItem()
                                     {
                                         Text = string.Format("{0} {1}", e.User.First_Name, e.User.Last_Name),
                                         Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = e.User_Id.Equals(Case_Manager_Supervisor_Id)
                                     }).ToList();

                    var selectList = new SelectList(employees, "Value", "Text", Case_Manager_Supervisor_Id);

                    return selectList;
                }
                else
                {
                    // Get the service office for the user indicated by Assessed By
                    var userModel = new UserModel();
                    var assessedByUser = userModel.GetSpecificUser((int)assessedById);

                    var serviceOfficeId = -1;

                    // Is the user a Social Worker?
                    if (assessedByUser.apl_Social_Worker.Any())
                    {
                        var socialWorker = assessedByUser.apl_Social_Worker.First(x => x.User_Id.Equals(assessedByUser.User_Id));
                        serviceOfficeId = socialWorker.Service_Office_Id;
                    }
                    // Is the user an Employee?
                    if (assessedByUser.Employees.Any())
                    {
                        var employee = assessedByUser.Employees.First(x => x.User_Id.Equals(assessedByUser.User_Id));
                        serviceOfficeId = employee.Service_Office_Id;
                    }

                    // Get social workers linked to service office
                    var socialworkerModel = new SocialWorkerModel();
                    var listOfSocialWorkers = socialworkerModel.GetListOfSocialWorkers(false, false, serviceOfficeId);

                    listOfSocialWorkers.RemoveAll(x => x.apl_Social_Worker2 == null); // Remove all that's not supervisors

                    if (Case_Manager_Supervisor_Id != null)
                        listOfSocialWorkers.RemoveAll(x => !x.apl_Social_Worker2.User_Id.Equals(Case_Manager_Supervisor_Id)); // Remove all that's not reporting to selected supervisor

                    var socialWorkers = (from e in listOfSocialWorkers
                                         select new SelectListItem()
                                         {
                                             Text = string.Format("{0} {1}", e.apl_User.First_Name, e.apl_User.Last_Name),
                                             Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = e.User_Id.Equals(Case_Manager_Supervisor_Id)
                                         }).ToList();

                    var selectList = new SelectList(socialWorkers, "Value", "Text", Case_Manager_Supervisor_Id);

                    return selectList;
                }

            }
        }

        [Display(Name = "Assessed By")]
        public SelectList Assessed_By_List
        {
            get
            {
                var employeeModel = new EmployeeModel();
                var listOfEmployees = employeeModel.GetListOfEmployees(false, false);

                var employees = (from e in listOfEmployees
                                 select new SelectListItem()
                                 {
                                     Text = string.Format("{0} {1}", e.User.First_Name, e.User.Last_Name),
                                     Value = e.User_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = e.User_Id.Equals(Assessed_By_Id)
                                 }).ToList();

                var selectList = new SelectList(employees, "Value", "Text", Assessed_By_Id);

                return selectList;
            }
        }
    }

    // Helper class to make posting back selected values easier
    public class PostedReferralFocusAreas
    {
        public string[] Referral_Focus_Area_IDs { get; set; }
    }

    public class PostedClientVictimizationTypes
    {
        public string[] Client_Victimization_Types { get; set; }
    }

    public partial class LookupDataItem
    {
        public int ItemId { get; set; }

        public int LookupTableTypeId { get; set; }

        [Display(Name = "Lookup Table", Description = "The Lookup Table to manipulate")]
        public SelectList LookupTableList
        {
            get
            {
                var lookupTables = Helpers.GetLookupDataItems();

                var selectList = new SelectList(lookupTables, "Value", "Description", LookupTableTypeId);

                return selectList;
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Description field is required")]
        [StringLength(150, ErrorMessage = "The Description field cannot be more than 150 characters in length")]
        [Display(Name = "Description", Description = "The Item's Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Source field is required")]
        [StringLength(150, ErrorMessage = "The Source field cannot be more than 150 characters in length")]
        [Display(Name = "Source", Description = "The Item's Value Source")]
        [DataType(DataType.Text)]
        public string Source { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Definition field is required")]
        [Display(Name = "Definition", Description = "The Item's Definition")]
        [DataType(DataType.Text)]
        public string Definition { get; set; }
    }

    public partial class LookupData
    {
        public int SelectedLookupTableId { get; set; }

        [Display(Name = "Lookup Table", Description = "The Lookup Table to manipulate")]
        public SelectList LookupTableList
        {
            get
            {
                var lookupTables = Helpers.GetLookupDataItems();

                var selectList = new SelectList(lookupTables, "Value", "Description", SelectedLookupTableId);

                return selectList;
            }
        }

        public List<LookupDataItem> LookupDataItems { get; set; }
    }

    public partial class IntakeDataViewModel
    {
        public List<CYCA_DynamicDataBaseModel> cYCA_DynamicDataBaseModel { get; set; }

        public string ImgUrl { get; set; }
        public bool CanAdmit { get; set; }
        public bool CanDischarge { get; set; }
        public bool SameFacility { get; set; }
        public string AdmittedAt { get; set; }
        public Reception_Register ReceptionRegister { get; set; }
        public Person Person { get; set; }
        public Address PhysicalAddress { get; set; }
        public Address PostalAddress { get; set; }

        //PERSONAL/FACILITY INVENTORY 
        public int Inventory_Id { get; set; }
        public int Person_Id { get; set; }
        public int Facility_Id { get; set; }
        public int Admission_Id { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public int? Inventory_Type_Id { get; set; }

        [Display(Name ="Inventory Type")]        
        public string selectedInventoryType { get; set; }

        [Display(Name = "Item Type")]
        [StringLength(30, ErrorMessage = "Text entered is too long")]
        [Required(ErrorMessage = "This field is Required")]
        public string Item_Type { get; set; }

        [Display(Name = "Item Color")]
        [StringLength(15, ErrorMessage ="Text entered is too long")]
        [Required(ErrorMessage = "This field is Required")]
        public string Item_Color { get; set; }

        [Display(Name = "Quantity")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please type number")]        
        [Required(ErrorMessage = "This field is Required")]
        public string Item_Quantity { get; set; }

        [Display(Name = "Item Description")]
        public string Item_Description { get; set; }

        [Display(Name = "Date Handed in/Date Issued")]      
        [Required(ErrorMessage = "This field is Required")]               
        public DateTime Date_Handed_In { get; set; }

        [Display(Name = "Date handed In")]
        [Required(ErrorMessage = "This field is Required")]
        public string Date_Handed_Inn { get; set; }

        public int Item_Handed_By { get; set; }        
        public int? Item_Handed_To { get; set; }

        [Display(Name = "Item(s) Received/Issued by")]
        [Required(ErrorMessage = "This field is Required")]
        public string selectedUser { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public int? Return_Status_Id { get; set; }

        [Display(Name = "Return Status")]
        [Required(ErrorMessage = "This field is Required")]
        public string selectedReturnStatus { get; set; }

        [Display(Name = "Date Returned")]       
        [Required(ErrorMessage = "This field is Required")]
        public DateTime? Date_Returned { get; set; }

        [Display(Name = "Date Returned")]
        [Required(ErrorMessage = "This field is Required")]
        public string Date_Returnedd { get; set; }

        [Display(Name = "Quantity Returned")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please type number")]
        [Required(ErrorMessage = "This field is Required")]
        public string Quantity_Returned { get; set; }

        [Display(Name = "Returned By")]
        [Required(ErrorMessage = "This field is Required")]
        public int? Returned_By { get; set; }

        [Display(Name = "Returned/ Accepted By")]
        [Required(ErrorMessage = "This field is Required")]
        public string selectedReturnedBy { get; set; }

        [Display(Name = "Reason (if not returned)")]
        //[Required(ErrorMessage = "This field is Required")]
        public string Reason_Not_Returned { get; set; }

        public DateTime Date_Created { get; set; }
        public int Created_By { get; set; }
        public DateTime Date_Last_Modified { get; set; }
        public int Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }

        public List<IntakeDataViewModel> IntakeDataViewModels { get; set; }
        public int PersonId { get; set; }

        public class CYCA_DynamicDataBaseModel
        {
            public List<CYCA_DynamicDataModel> dynamicDataModels { get; set; }
            public int ChildId { get; set; }

            public class CYCA_DynamicDataModel
            {
            }
        }


    }


    //public class CYCAInventoriesPartialViewModel
    //{
    //    public List<IntakeDataViewModel> IntakeDataViewModels { get; set; }
    //    public int PersonId { get; set; }
    //}

    public partial class IntakeClientViewModel
    {
        public Client Client { get; set; }
        public Person Person { get; set; }
        public Address PhysicalAddress { get; set; }
        public Address PostalAddress { get; set; }
        public List<Person_Education> EducationItems { get; set; }
        public List<Person_Employment> EmploymentItems { get; set; }
        public List<IntakeMedicalConditionItem> MedicalConditionItems { get; set; }
        public Intake_Assessment IntakeAssessment { get; set; }
        public int? Relation_Type_Id { get; set; }
        [Display(Name = "Relation Type", Description = "The Type of Relation")]
        public SelectList Relation_Type_List
        {
            get
            {
                var relationTypes = Helpers.GetRelationTypes();

                var selectList = new SelectList(relationTypes, "Value", "Description", Relation_Type_Id);

                return selectList;
            }
        }

        public IEnumerable<Disability> AvailableDisabilityType { get; set; }
        public IList<Disability> SelectedDisabilityType { get; set; }
        public Posted_DisabilityType PostedDisabilityType { get; set; }

        public IEnumerable<apl_DisabilityType> AvailableDisabilitySubType { get; set; }
        public IList<apl_DisabilityType> SelectedDisabilitySubType { get; set; }
        public Posted_DisabilitySubType PostedDisabilitySubType { get; set; }

        public IEnumerable<apl_Special_Need> AvailableSpecialNeedType { get; set; }
        public IList<apl_Special_Need> SelectedSpecialNeedType { get; set; }
        public Posted_SpecialNeedType PostedSpecialNeedType { get; set; }

        public VEP_Cases VEP_Case { get; set; }

        public Disability Disabilities { get; set; }

        public VEP_SettlementType SettlementType { get; set; }

        public IEnumerable<VEP_VictimizationType> AvailableVictimizationType { get; set; }
        public IList<VEP_VictimizationType> SelectedVictimizationType { get; set; }
        public Posted_VictimizationType PostedVictimizationType { get; set; }

        public class Posted_SpecialNeedType
        {
            public int[] SpecialNeedTypeIDs { get; set; }

            public IEnumerable<SelectListItem> ListOfSpecialNeedTypeIDs { get; set; }
        }

        public IList<VEP_PresentationCondition> AvailablePresentationCondition { get; set; }
        public IList<VEP_PresentationCondition> SelectedPresentationCondition { get; set; }
        public Posted_PresentationCondition PostedPresentationCondition { get; set; }

        public VEP_Sexual_Orientation VEP_SexualOrientation { get; set; }

        public List<VEP_Services> VEP_Services { get; set; }

        public List<VEP_Referals> VEP_Referals { get; set; }
    }

    public class Posted_ServiceCategory
    {
        public int[] ServiceCategoryIDs { get; set; }

        public IEnumerable<SelectListItem> ListOfServiceCategoryIDs { get; set; }
    }

    public class Posted_VictimizationType
    {
        public int[] VictimizationTypeIDs { get; set; }

        public IEnumerable<SelectListItem> ListOfVictimizationTypeIDs { get; set; }
    }

    public class Posted_DisabilityType
    {
        public int[] DisabilityTypeIDs { get; set; }

        public IEnumerable<SelectListItem> ListOfDisabilityTypeIDs { get; set; }
    }

    public class Posted_DisabilitySubType
    {
        public int[] DisabilitySubTypeIDs { get; set; }

        public IEnumerable<SelectListItem> ListOfDisabilitySubTypeIDs { get; set; }
    }

    public class Posted_EmployeeServices
    {
        public int[] EmployeeServiceIDs { get; set; }

        public IEnumerable<SelectListItem> ListOfEmployeeServiceIDs { get; set; }
    }

    public class Posted_EmployeeRoles
    {
        public int[] EmployeeRoleIDs { get; set; }

        public IEnumerable<SelectListItem> ListOfEmployeeRoleIDs { get; set; }
    }

    public class Posted_PresentationCondition
    {
        public string[] PresentationConditionIDs { get; set; }
    }

    public partial class VictimRecord
    {
        private SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();

        public Client Client { get; set; }
        public Person Person { get; set; }

        public List<int> VictimTypeSelectedList { get; set; }

        public List<VEP_VictimizationTypeDetails> SelectedCases { get; set; }

        public Address PhysicalAddress { get; set; }
        public Address PostalAddress { get; set; }
        public List<Person_Education> EducationItems { get; set; }
        public List<Person_Employment> EmploymentItems { get; set; }
        public VEP_Cases IncidentInformation { get; set; }
        public VEP_VictimsConditions VictimConditions { get; set; }
        public List<VEP_Services> Services { get; set; }
        public List<VEP_Referals> referals { get; set; }

        public CheckBoxListViewModel CheckBoxListViewModel { get; set; }

        public CheckBoxListViewModel VictimizationTypeCheckBoxListViewModel { get; set; }

        public int? Relation_Type_Id { get; set; }
        [Display(Name = "Relation Type", Description = "The Type of Relation")]
        public SelectList Relation_Type_List
        {
            get
            {
                var relationTypes = Helpers.GetRelationTypes();

                var selectList = new SelectList(relationTypes, "Value", "Description", Relation_Type_Id);

                return selectList;
            }
        }

        public class Posted_EmployeeService
        {
            public int[] EmployeeServiceIDs { get; set; }

            public IEnumerable<SelectListItem> ListOfEmployeeServices { get; set; }
        }

        // public List<ServiceCategory> Services { get; set; }

        public List<VEP_VictimizationType> Fruits { get; set; }
        public int[] FruitIds { get; set; }

        //public VEPReferalsViewModel GetReferralInformation(int Case_Id)
        //{
        //    var Obj = (from a in dbContext.VEP_Referals
        //                        where a.CaseId == Case_Id
        //                        select a).FirstOrDefault();
        //    VEPReferalsViewModel NewObj = new VEPReferalsViewModel();
        //    NewObj.CreateDate = Obj.CreateDate;
        //    NewObj.Createdby = Obj.Createdby;
        //    NewObj.FromDepartment = Obj.FromDepartment;
        //    NewObj.ToDepartment = Obj.ToDepartment;
        //    NewObj.Notes = Obj.Notes;
        //    NewObj.Caseid = Obj.CaseId;
        //    return NewObj;
        //}

        public bool CheckIfExistInDB(int Id)
        {
            var DBResult = (from c in dbContext.VEP_Cases
                            where c.ClientId == Id
                            select c.CaseId).FirstOrDefault();
            if (DBResult != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int SelectvictimizationTypeId { get; set; }

        public SelectList VictimizationTypeList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var ListofvictimizationType = (from a in _db.VEP_VictimizationType
                                               orderby a.VictimizationType
                                               select new
                                               {
                                                   a.Id,
                                                   a.VictimizationType
                                               }).ToList();
                var selectList = new SelectList(ListofvictimizationType, "Value", "Text", SelectvictimizationTypeId);
                return selectList;

            }

        }

    }


    public partial class IntakeMedicalConditionItem
    {
        public int Person_Id { get; set; }

        public int Item_Id { get; set; }

        public int Remove_Item_Type_Id { get; set; }

        public int Remove_Item_Id { get; set; }

        public int Medical_Condition_Type_Id { get; set; }

        public string Medical_Condition_Type_Description { get; set; }

        [Display(Name = "Type", Description = "The Type of Medical Condition")]
        public SelectList Medical_Condition_Type_List
        {
            get
            {
                var medicalConditionTypes = Helpers.GetMedicalConditionTypes();

                var selectList = new SelectList(medicalConditionTypes, "Value", "Description", Medical_Condition_Type_Id);

                return selectList;
            }
        }

        public int Medical_Condition_Id { get; set; }

        public string Medical_Condition_Description { get; set; }

        public List<LookupDataItem> Medical_Conditions { get; set; }

        [Display(Name = "Condition")]
        public SelectList Medical_Condition_List
        {
            get
            {
                if (Medical_Conditions == null)
                    Medical_Conditions = new List<LookupDataItem>();

                var selectList = new SelectList(Medical_Conditions, "ItemId", "Description", Medical_Condition_Id);

                return selectList;
            }
        }
    }

    public partial class IntakeRelationItem
    {
        public int Person_Id { get; set; }
        public List<int> SelectedDisabilityId { get; set; }
        public int Item_Id { get; set; }

        public int Remove_Item_Type_Id { get; set; }
        //public List<int> SelectedDisabilitiesPopupId { get; set; }


        public int Remove_Item_Id { get; set; }

        public int Relation_Type_Id { get; set; }

        public string Relation_Type_Description { get; set; }

        public int Relation_Person_Id { get; set; }

        public Person Relation_Person { get; set; }

        //public List<int> SelectedDisabilityId { get; set; }
        public IntakeSearchViewModel SearchPerson { get; set; }

        public PersonDetailViewModel CreatePerson { get; set; }

        public bool IsAdoptiveParentDetailVisible { get; set; }

        public bool IsBiologicalParentDetailVisible { get; set; }

        public bool IsFosterParentDetailVisible { get; set; }

        public bool IsFamilyMemberDetailVisible { get; set; }

        public bool IsCaregiverDetailVisible { get; set; }
        public bool IsProspectiveAdoptiveParentVisible { get; set; }

        [Display(Name = "Is Deceased?")]
        public bool IsDeceased { get; set; }

        [Display(Name = "Date Deceased")]
        public string DateDeceased { get; set; }

        public int Person_Relationship_Type_Id { get; set; }

        [Display(Name = "Relationship Type")]
        public SelectList Person_Relationship_Type_List
        {
            get
            {
                var relationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = relationshipTypeModel.GetListOfRelationshipTypes();

                var relationshipTypesList = (from g in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = g.Description,
                                                 Value = g.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = g.Relationship_Type_Id.Equals(Person_Relationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(relationshipTypesList, "Value", "Text", Person_Relationship_Type_Id);

                return selectList;
            }
        }
    }

    public class CustomTable
    {
        public int Year { get; set; }
        public int Values { get; set; }
    }

    public class UnsuitInqTable
    {
        public string TypeSet { get; set; }
        public int Year { get; set; }
        public int Values { get; set; }
    }

    public class fullYearTable
    {
        public int year { get; set; }
        public int Quarter1 { get; set; }
        public int Quarter2 { get; set; }
        public int Quarter3 { get; set; }
        public int Quarter4 { get; set; }
    }

    public class InquiriesTable
    {
        public string TypeInq { get; set; }
        public int Quarter1 { get; set; }
        public int Quarter2 { get; set; }
        public int Quarter3 { get; set; }
        public int Quarter4 { get; set; }
    }

    public class CPR_Report_Unsuitabiluity
    {

        public string CPRReferenceNumber { get; set; }
        public string TypeOfAbuse { get; set; }
        public string RelationToChild { get; set; }
        public string CurrentPlacement { get; set; }
        public string SAPSReference { get; set; }
        public string CourtReference { get; set; }
        public string Conviction { get; set; }
        public string SummaryOfCourtFindings { get; set; }
        public int? Age { get; set; }
        public string Province { get; set; }
        public int? PopulationGroupId { get; set; }
        public DateTime? DateRegistered { get; set; }
        public DateTime? IncidentDate { get; set; }
        public int? TownId { get; set; }
        public int? Prov { get; set; }
    }

    public class CPR_Report_Notification_Public
    {

        public string ReferenceNumber { get; set; }
        public string TypeOfAbuse { get; set; }
        public string RelationToChild { get; set; }
        public string Age { get; set; }
        public string Province { get; set; }
        public int? PopulationGroupId { get; set; }
        public DateTime? DateofIncident { get; set; }
        public string DistrictName { get; set; }
    }

    public class CPR_Report_Notifications_Mandatory
    {
        public string ReferenceNumber { get; set; }
        public string TypeOfAbuse { get; set; }
        public string MandatoryObligedOrganisation { get; set; }
        public string MandatoryObligedType { get; set; }
        public string Age { get; set; }
        public string Province { get; set; }
        public int? PopulationGroupId { get; set; }
        public DateTime? DateofIncident { get; set; }
        public string DistrictName { get; set; }
    }

    public class CPR_Report_Removals
    {
        public string CPRReferenceNumber { get; set; }
        public bool? IntentToRemove { get; set; }
        public DateTime? DateNoticeRecieved { get; set; }
        public string ReasonForDispute { get; set; }
        public string ReasonForFinding { get; set; }
        public DateTime? DateOfApplication { get; set; }
        public bool? RemovalApproved { get; set; }
        public string CourtForumRegerence { get; set; }
        public string CourtForumCase { get; set; }
        public string OutcomeSummary { get; set; }
        public int? Province { get; set; }
        public int? PopulationGroupId { get; set; }
        public int? TownId { get; set; }
        public int? Prob { get; set; }
    }

    public class CPR_Report_Inquiies
    {
        public string CPRReferenceNumber { get; set; }
        public string EmployerOrg { get; set; }
        public string EmployerName { get; set; }
        public string IndividualName { get; set; }
        public string ProvinceName { get; set; }
        public int? Province { get; set; }
        public int? PopulationGroupId { get; set; }
        public DateTime? DateRegistered { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string InquiryType { get; set; }
        public int? TownId { get; set; }
        public int? Prov { get; set; }
    }


    public class CPR_Report_CourtCases
    {
        public string IncidentId { get; set; }
        public string CPRReferenceNumber { get; set; }
        public string PrimaryAbuseType { get; set; }
        public string SubPrimaryAbuseType { get; set; }
        public DateTime? IncidentDate { get; set; }
        public bool AbuseConfirmed { get; set; }
        public string RiskToChild { get; set; }
        public string MultipleAbuse { get; set; }
        public int? Age { get; set; }
        public string Province { get; set; }
        public int? PopulationGroupId { get; set; }
        public bool? isCaseClosed { get; set; }
        public int? OblidgedId { get; set; }
        public int? TownId { get; set; }
        public int? ProvId { get; set; }
    }



    public partial class CPR_Correspondence
    {
        [Display(Name = "Correspondence Letter Type")]
        public SelectList Correspondence_Type_List
        {
            get
            {
                var correspondenceTypeModel = new CorrespondenceTypeModel();
                var listOfCorrespondenceTypes = correspondenceTypeModel.GetListOfCorrespondenceTypes();

                var correspondenceTypeList = (from x in listOfCorrespondenceTypes
                                              where x.CPR_Correspondence_Letter_Id == 1
                                              select new SelectListItem()
                                              {
                                                  Text = x.Description,
                                                  Value = x.CPR_Correspondence_Letter_Id.ToString(CultureInfo.InvariantCulture),
                                                  Selected = x.CPR_Correspondence_Letter_Id.Equals(CPR_Correspondence_Letter_Id)
                                              }).ToList();

                var selectList = new SelectList(correspondenceTypeList, "Value", "Text", CPR_Correspondence_Letter_Id);

                return selectList;
            }
        }

        public string Sent_By_User
        {
            get
            {
                return apl_User == null ? "" : apl_User.First_Name + " " + apl_User.Last_Name;
            }
        }

        public string Sent_To_Person
        {
            get
            {
                return int_Person == null ? "" : int_Person.First_Name + " " + int_Person.Last_Name;
            }
        }
    }

    public partial class CPR_Unsuitability_Conviction
    {
        public string CPRB_ReferenceNumber { get; set; }
        public string CprIndidentRefNumber { get; set; }

        public int SelectedCourt_Id { get; set; }

        public SelectList CourtList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var courts = (from f in _db.Courts
                              select f).ToList();

                var employers = (from m in courts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Court_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Court_Id.Equals(SelectedCourt_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedCourt_Id);

                return selectList;
            }
        }
    }


    public partial class Client_CareGiver
    {
        public int Unsuitability_Id { get; set; }

        public int SelectedIdType_Id { get; set; }
        public SelectList IdTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIdTypes = (from m in _db.Identification_Types
                                     select m).ToList();

                var employers = (from m in listOfIdTypes
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Identification_Type_Id.Equals(SelectedIdType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedIdType_Id);

                return selectList;
            }
        }

        public int SelectedGender_Id { get; set; }
        public SelectList GenderList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Genders
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGender_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGender_Id);

                return selectList;
            }
        }

        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }

        public int SelectedRelationShipTypeId { get; set; }
        public SelectList RelationshipTypeList
        {
            get
            {
                var relationships = new RelationshipTypeModel();
                var listOfRelationships = relationships.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(SelectedRelationShipTypeId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedRelationShipTypeId);

                return selectList;
            }
        }

        public int SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

                return selectList;
            }
        }
    }

    public partial class CPR_Unsuitability_Incedent
    {
        public int SelectedAbuseType_Id { get; set; }
        public SelectList AbuseTypeList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Abuse_Types
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Abuse_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Abuse_Type_Id.Equals(SelectedAbuseType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedAbuseType_Id);

                return selectList;
            }
        }
    }

    public partial class CPR_OnlineNotification_FirstReporter
    {
        public int RelationShipTypeId { get; set; }
        public SelectList RelationshipTypeList
        {
            get
            {
                var relationships = new RelationshipTypeModel();
                var listOfRelationships = relationships.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(RelationShipTypeId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", RelationShipTypeId);

                return selectList;
            }
        }

    }

    public partial class CPR_OnlineNotification_AlledgedOffender
    {
        public int RelationShipTypeId { get; set; }
        public SelectList RelationshipTypeList
        {
            get
            {
                var relationships = new RelationshipTypeModel();
                var listOfRelationships = relationships.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(RelationShipTypeId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", RelationShipTypeId);

                return selectList;
            }
        }

        public int SelectedGender_Id { get; set; }
        public SelectList GenderList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Genders
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGender_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGender_Id);

                return selectList;
            }
        }


        public int SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

                return selectList;
            }
        }

        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }
    }

    public partial class CPR_OnlineNotification__ChildDetails
    {
        public int SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

                return selectList;
            }
        }

        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }

        public int SelectedGender_Id { get; set; }
        public SelectList GenderList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Genders
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGender_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGender_Id);

                return selectList;
            }
        }

        public int SelectedIdType_Id { get; set; }
        public SelectList IdTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIdTypes = (from m in _db.Identification_Types
                                     select m).ToList();

                var employers = (from m in listOfIdTypes
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Identification_Type_Id.Equals(SelectedIdType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedIdType_Id);

                return selectList;
            }
        }

        public string GenderName
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var selectedGender = _db.Genders.Where(p => p.Gender_Id == this.Gender_Id).Select(x => x.Description).FirstOrDefault();
                return selectedGender;
            }
        }

        public CPR_OnlineNotifications_Incedent IncedentDetails
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var selectedIncedent = _db.CPR_OnlineNotifications_Incedent.Where(p => p.Incedent_Id == this.Incedent_Id).FirstOrDefault();
                return selectedIncedent;
            }


        }
    }

    public partial class CPR_OnlineNotifications_Incedent
    {
        public int SelectedInDanger_Id { get; set; }
        public SelectList InDangerList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.ACM_YesNoOption
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.YesNoOption_Id.Equals(SelectedInDanger_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedInDanger_Id);

                return selectList;
            }
        }

        public int SelectedAbuseType_Id { get; set; }
        public SelectList AbuseTypeList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Abuse_Types
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Abuse_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Abuse_Type_Id.Equals(SelectedAbuseType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedAbuseType_Id);

                return selectList;
            }
        }

        public int SelectedRiskIndicator_Id { get; set; }
        public SelectList RiskIndicators
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIndicators = (from r in _db.Risk_Indicators
                                        select r).ToList();

                var indicators = (from m in listOfIndicators
                                  select new SelectListItem()
                                  {
                                      Text = m.Description,
                                      Value = m.Risk_Indicator_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = m.Risk_Indicator_Id.Equals(SelectedRiskIndicator_Id)
                                  }).ToList();

                var selectList = new SelectList(indicators, "Value", "Text", SelectedRiskIndicator_Id);

                return selectList;
            }
        }

    }

    public partial class CPR_APPEALS
    {
        public Person PersonAppealing
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var persDetails = (from p in _db.Persons
                                   where p.Person_Id == this.Person_Id
                                   select p).FirstOrDefault();

                return persDetails;
            }

        }
        public CPR_Unsuitability_Conviction Convictions
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var convDetails = (from p in _db.CPR_Unsuitability_Conviction
                                   where p.Conviction_Id == this.Conviction_Id
                                   select p).FirstOrDefault();

                return convDetails;
            }

        }

    }

    //public partial class CYCA_AdmissionViewModel
    //{
    //    public CYCA_Case_Admission CYCA_Case_Admission { get; set; }
    //    //public apl_Cyca_Venue Cyca_Venue { get; set; }
    //    public apl_Admission_Type Admission_Reason { get; set; }
    //    SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();

    //    public SelectList VenueList
    //    {
    //        get
    //        {
    //            var vList = (from p in dbContext.apl_Cyca_Venue
    //                         select new SelectListItem()
    //                         {
    //                             Text = p.VenueName,
    //                             Value = p.Venue_Id.ToString(),
    //                             Selected = p.Venue_Id.Equals(selectedVenueId)
    //                         }).ToList();
    //            var selectList = new SelectList(vList, "Value", "Text", selectedVenueId);

    //            return selectList;
    //        }
    //    }

    //    public int selectedVenueId { get; set; }

    //}




    public partial class PersonDetailViewModel
    {
        public Person Person { get; set; }
        public Address PhysicalAddress { get; set; }
        public Address PostalAddress { get; set; }
        SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();

        [Display(Name = "Title")]
        public SelectList TitleList
        {
            get
            {
                var tList = (from p in dbContext.CPR_Title
                             select new SelectListItem()
                             {
                                 Text = p.Description,
                                 Value = p.Title_Id.ToString(),//ToString(CultureInfo.InvariantCulture),
                                 Selected = p.Title_Id.Equals(SelectedTitleId)
                             }).ToList();

                var selectList = new SelectList(tList, "Value", "Text", SelectedTitleId);

                return selectList;
            }
        }

        //[Required(ErrorMessage = "Please select a title")]
        public int SelectedTitleId { get; set; }
        public List<int> SelectedDisabilityId { get; set; }
        public int SelectedEmployer_Id { get; set; }
        [Display(Name = "Employer")]
        public SelectList Employer_List
        {
            get
            {
                var employerModel = new EmployerModel();
                var listOfEmployers = employerModel.GetListOfEmployers();

                var employers = (from m in listOfEmployers
                                 select new SelectListItem()
                                 {
                                     Text = m.Employer_Name,
                                     Value = m.Employer_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Employer_Id.Equals(SelectedEmployer_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedEmployer_Id);

                return selectList;
            }
        }


        [Display(Name = "Gender")]
        public int SelectedGenderId { get; set; }
        public SelectList GenderList
        {
            get
            {
                var genders = new GenderModel();
                var listOfGenders = genders.GetListOfGenders();

                var employers = (from m in listOfGenders
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGenderId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGenderId);

                return selectList;
            }
        }

        [Display(Name = "City / Town")]
        public int SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

                return selectList;
            }
        }

        [Display(Name = "Relationship")]
        public int SelectedRelationship_Id { get; set; }
        public SelectList RelationshipType_List
        {
            get
            {
                var relationshipType = new RelationshipTypeModel();
                var listOfRelationships = relationshipType.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(SelectedRelationship_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedRelationship_Id);

                return selectList;
            }
        }

        [Display(Name = "Occupation")]
        public int SelectedOccupation_Id { get; set; }
        public SelectList Occupation_List
        {
            get
            {
                var occupations = new OccupationModel();
                var listOfOccupations = occupations.GetListOfOccupations();

                var employers = (from m in listOfOccupations
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Occupation_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Occupation_Id.Equals(SelectedOccupation_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedOccupation_Id);

                return selectList;
            }
        }

     
        public int SelectedIdType_Id { get; set; }
        public SelectList IdTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIdTypes = (from m in _db.Identification_Types
                                     select m).ToList();

                var employers = (from m in listOfIdTypes
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Identification_Type_Id.Equals(SelectedIdType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedIdType_Id);

                return selectList;
            }
        }

        [Display(Name = "Magistrate")]
        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        [Display(Name = "Province")]
        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }
        public IEnumerable<Disability> AvailableDisabilityType { get; set; }
        public IList<Disability> SelectedDisabilityType { get; set; }
        public Posted_DisabilityType PostedDisabilityType { get; set; }
    }

    #region CPR

    public partial class CPR_EnquiriesByEmployer
    {
        public CPR_EnquiriesEmployerDetails EmployerDetails
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var empDetails = (from emp in _db.CPR_EnquiriesEmployerDetails
                                  where emp.Enquiry_Id == this.Enquiry_Id
                                  select emp).FirstOrDefault();

                return empDetails;
            }

        }

        public int SelectedInquiriesEmploymentType_Id { get; set; }
        public SelectList InquiriesEmployementTypeList
        {
            get
            {
                List<SelectListItem> tlist = new List<SelectListItem>();
                var item1 = new SelectListItem()
                {
                    Text = "Employee",
                    Value = "1",
                    Selected = "1".Equals(SelectedInquiriesEmploymentType_Id)
                };

                var item2 = new SelectListItem()
                {
                    Text = "Potential Employee",
                    Value = "2",
                    Selected = "2".Equals(SelectedInquiriesEmploymentType_Id)
                };

                tlist.Add(item1);
                tlist.Add(item2);

                var selectList = new SelectList(tlist, "Value", "Text", SelectedInquiriesEmploymentType_Id);

                return selectList;
            }
        }

        public int SelectedEmployee_Id { get; set; }
        public SelectList EmployeeList
        {
            get
            {
                var dbContext = new SDIIS_DatabaseEntities();

                var tList = (from p in dbContext.Employees
                             select new SelectListItem()
                             {
                                 Text = p.User.First_Name + " " + p.User.Last_Name,
                                 Value = p.Employee_Id.ToString(),//ToString(CultureInfo.InvariantCulture),
                                 Selected = p.Employee_Id.Equals(SelectedEmployee_Id)
                             }).ToList();


                var selectList = new SelectList(tList, "Value", "Text", SelectedEmployee_Id);

                return selectList;
            }
        }

        public CPR_EquiriesPersonDetails PersonDetails
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var pDetails = (from p in _db.CPR_EquiriesPersonDetails
                                where p.Enquiry_Id == this.Enquiry_Id
                                select p).FirstOrDefault();

                return pDetails;
            }
        }
    }

    public partial class CPR_EnquiriesEmployerDetails
    {
        public int SelectedMagistrateDistrict_Id { get; set; }

        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagistrateDistrict_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagistrateDistrict_Id);

                return selectList;
            }

        }

        public int SelectedProvince_Id { get; set; }

        public SelectList ProvinceList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var provinces = (from f in _db.Provinces
                                 select f).ToList();

                var provincelist = (from m in provinces
                                    select new SelectListItem()
                                    {
                                        Text = m.Description,
                                        Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = m.Province_Id.Equals(SelectedProvince_Id)
                                    }).ToList();

                var selectList = new SelectList(provincelist, "Value", "Text", SelectedProvince_Id);

                return selectList;


            }
        }

        public int SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var townList = (from f in _db.Towns
                                select f).ToList();

                var townlist = (from m in townList
                                select new SelectListItem()
                                {
                                    Text = m.Description,
                                    Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                    Selected = m.Town_Id.Equals(SelectedTown_Id)
                                }).ToList();

                var selectList = new SelectList(townlist, "Value", "Text", SelectedTown_Id);

                return selectList;


            }
        }

        public int SelectedTitle_Id { get; set; }
        public SelectList TitleList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var listOfTitles = (from t in _db.CPR_Title
                                    select t).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Title_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Title_Id.Equals(SelectedTitle_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTitle_Id);

                return selectList;
            }

        }
    }

    public partial class CPR_EquiriesPersonDetails
    {

        public int SelectedTitle_Id { get; set; }
        public SelectList TitleList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from t in _db.CPR_Title
                                    select t).ToList();

                var titles = (from m in listOfTitles
                              select new SelectListItem()
                              {
                                  Text = m.Description,
                                  Value = m.Title_Id.ToString(CultureInfo.InvariantCulture),
                                  Selected = m.Title_Id.Equals(SelectedTitle_Id)
                              }).ToList();

                var selectList = new SelectList(titles, "Value", "Text", SelectedTitle_Id);

                return selectList;
            }

        }
        public int SelectedMagistrateDistrict_Id { get; set; }

        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagistrateDistrict_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagistrateDistrict_Id);

                return selectList;
            }

        }

        public int SelectedProvince_Id { get; set; }

        public SelectList ProvinceList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var provinces = (from f in _db.Provinces
                                 select f).ToList();

                var provincelist = (from m in provinces
                                    select new SelectListItem()
                                    {
                                        Text = m.Description,
                                        Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = m.Province_Id.Equals(SelectedProvince_Id)
                                    }).ToList();

                var selectList = new SelectList(provincelist, "Value", "Text", SelectedProvince_Id);

                return selectList;


            }
        }

        public int SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var townList = (from f in _db.Towns
                                select f).ToList();

                var townlist = (from m in townList
                                select new SelectListItem()
                                {
                                    Text = m.Description,
                                    Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                    Selected = m.Town_Id.Equals(SelectedTown_Id)
                                }).ToList();

                var selectList = new SelectList(townlist, "Value", "Text", SelectedTown_Id);

                return selectList;


            }
        }

    }

    public partial class CPR_EnquiriesByIndividual
    {
        public CPR_EquiriesPersonDetails PersonDetails
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var pDetails = (from p in _db.CPR_EquiriesPersonDetails
                                where p.PersonDetails_Id == this.PersonDetails_Id
                                select p).FirstOrDefault();

                return pDetails;
            }
        }
    }

    public partial class Client
    {
        public int Unsuitability_Id { get; set; }
        public string CPRB_ReferenceNumber { get; set; }
        public int? SelectedIdType_Id { get; set; }
        public SelectList IdTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIdTypes = (from m in _db.Identification_Types
                                     select m).ToList();

                var employers = (from m in listOfIdTypes
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Identification_Type_Id.Equals(SelectedIdType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedIdType_Id);

                return selectList;
            }
        }


        public int? SelectedGender_Id { get; set; }
        public SelectList GenderList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Genders
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGender_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGender_Id);

                return selectList;
            }
        }

        public int? SelectedPopulationGroup_Id { get; set; }
        public SelectList PopulationGroupList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.Population_Groups
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Population_Group_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Population_Group_Id.Equals(SelectedPopulationGroup_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedPopulationGroup_Id);

                return selectList;
            }
        }
    }

    public partial class CPR_Unsuitability_Findings
    {


        public int SelectedFindings_Id { get; set; }
        public SelectList FindingsList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var abuseList = (from f in _db.CPR_Finding_Type
                                 select f).ToList();

                var employers = (from m in abuseList
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.FindingType_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.FindingType_Id.Equals(SelectedFindings_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedFindings_Id);

                return selectList;


            }
        }
    }

    public partial class CPR_Unsuitability_Ruiling
    {
        public int SelectedRuiling_Id { get; set; }
        public SelectList RuilingList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var abuseList = (from f in _db.CPR_Ruiling_Type
                                 select f).ToList();

                var employers = (from m in abuseList
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.RuilingType_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.RuilingType_Id.Equals(SelectedRuiling_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedRuiling_Id);

                return selectList;


            }
        }
    }

    public partial class CPR_Unsuitability_Forum
    {
        public int Unsuitibility_Id { get; set; }
    }

    [MetadataType(typeof(UnsuitabilityMetadata))]
    public partial class CPR_Unsuitability
    {
        //public bool KnownOffender { get; set; }
        //public bool InternationalNotification { get; set; }
        //add addresses

        public Address PostalAddress
        {
            get
            {
                var personModel = new PersonModel();
                var personToEdit = personModel.GetSpecificPerson(this.Person_Id);

                //var physicalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
                var postalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();
                if (postalAddress != null)
                {
                    return postalAddress;
                }else
                {
                    return new Address();
                }
                
            }
        }

        public Address PhysicalAddress
        {
            get
            {
                var personModel = new PersonModel();
                var personToEdit = personModel.GetSpecificPerson(this.Person_Id);

                var physicalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PhysicalAddress)) : new Address();
                //var postalAddress = personToEdit.Addresses.Any(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) ? personToEdit.Addresses.First(a => a.Address_Type_Id.Equals((int)AddressTypeEnum.PostalAddress)) : new Address();
                if (physicalAddress != null)
                {
                    return physicalAddress;
                }
                else
                {
                    return new Address();
                }
                
            }
        }


        [Display(Name = "Gender")]
        public int SelectedGenderId { get; set; }
        public SelectList GenderList
        {
            get
            {
                var genders = new GenderModel();
                var listOfGenders = genders.GetListOfGenders();

                var employers = (from m in listOfGenders
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Gender_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Gender_Id.Equals(SelectedGenderId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedGenderId);

                return selectList;
            }
        }

        [Display(Name = "City / Town")]
        public int? SelectedTown_Id { get; set; }
        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

                return selectList;
            }
        }

        [Display(Name = "Relationship")]
        public int SelectedRelationship_Id { get; set; }
        public SelectList RelationshipType_List
        {
            get
            {
                var relationshipType = new RelationshipTypeModel();
                var listOfRelationships = relationshipType.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(SelectedRelationship_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedRelationship_Id);

                return selectList;
            }
        }

        [Display(Name = "Occupation")]
        public int? SelectedOccupation_Id { get; set; }
        public SelectList Occupation_List
        {
            get
            {
                var occupations = new OccupationModel();
                var listOfOccupations = occupations.GetListOfOccupations();

                var employers = (from m in listOfOccupations
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Occupation_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Occupation_Id.Equals(SelectedOccupation_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedOccupation_Id);

                return selectList;
            }
        }

        public int SelectedTitle_Id { get; set; }
        public SelectList TitleList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.CPR_Title
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Title_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Title_Id.Equals(SelectedTitle_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTitle_Id);

                return selectList;
            }
        }

        public int SelectedIdType_Id { get; set; }
        public SelectList IdTypes
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfIdTypes = (from m in _db.Identification_Types
                                     select m).ToList();

                var employers = (from m in listOfIdTypes
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Identification_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Identification_Type_Id.Equals(SelectedIdType_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedIdType_Id);

                return selectList;
            }
        }

        [Display(Name ="Municipality")]
        public int Selected_Local_Municipality_Id { get; set; }
        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Local_Municipality_Id);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Local_Municipality_Id.Equals(Selected_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Selected_Local_Municipality_Id);

                return selectList;
            }
        }

        [Display(Name = "Magistrate")]
        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        [Display(Name = "Province")]
        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }

        [Display(Name = "Relationship")]
        public int RelationShipTypeId { get; set; }
        public SelectList RelationshipTypeList
        {
            get
            {
                var relationships = new RelationshipTypeModel();
                var listOfRelationships = relationships.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(RelationShipTypeId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", RelationShipTypeId);

                return selectList;
            }
        }

        public string RequestToApeal
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                string strRequestToApeal = "No";

                if (this.CPR_Unsuitability_Findings != null)
                {
                    var findingDetails = _db.CPR_Unsuitability_Findings.Where(x => x.Unsuitability_Id == this.Unsuitablity_Id).FirstOrDefault();
                    if (findingDetails.RequestAppeal == true)
                    {
                        strRequestToApeal = "Yes";

                    }
                }

                if (this.CPR_Unsuitability_Ruiling != null)
                {
                    var rulingDetails = _db.CPR_Unsuitability_Ruiling.Where(x => x.Unsuitability_Id == this.Unsuitablity_Id).FirstOrDefault();
                    if (rulingDetails.RequestAppeal == true)
                    {
                        strRequestToApeal = "Yes";
                    }

                }

                return strRequestToApeal;
            }
        }

        public string Town_Name
        {
            get
            {
                string strTownName = "";
                if (this.apl_Town == null)
                {
                    strTownName = "n/a";
                }
                else
                {
                    strTownName = this.apl_Town.Description;
                }

                return strTownName;
            }
        }

        public string Distric_Name
        {
            get
            {
                string strDistrictName = "";
                if (this.apl_District == null)
                {
                    strDistrictName = "n/a";
                }
                else
                {
                    strDistrictName = this.apl_District.Description;
                }

                return strDistrictName;
            }
        }

        //public string Municipality_1_Name
        //{
        //    get
        //    {
        //        string strDistrictName_1 = "";
        //        if (this.apl_Municipality == null)
        //        {
        //            strDistrictName_1 = "n/a";
        //        }
        //        else
        //        {
        //            strDistrictName_1 = this.apl_Municipality.Description;
        //        }

        //        return strDistrictName_1;
        //    }
        //}

        public string Abuse_Type
        {
            get
            {
                string strAbuseText = "";
                var _db = new SDIIS_DatabaseEntities();
                var incidentDetails = _db.CPR_Unsuitability_Incedent.Where(x => x.Unsuitability_Id == this.Unsuitablity_Id).FirstOrDefault();
                if (incidentDetails == null)
                {
                    strAbuseText = "n.a.";
                }
                else
                {
                    strAbuseText = incidentDetails.apl_Abuse_Type.Description;
                }
                return strAbuseText;
            }
        }

        public string hasRuiling
        {
            get
            {
                string RuilingOutcome = "No";
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                bool isRuiling = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                var _db = new SDIIS_DatabaseEntities();
                var ruilingDetails = _db.CPR_Unsuitability_Ruiling.Where(x => x.Unsuitability_Id == this.Unsuitablity_Id).FirstOrDefault();
                if (ruilingDetails == null)
                {
                    RuilingOutcome = "No";
                    isRuiling = false;
                    var findingsDetails = _db.CPR_Unsuitability_Findings.Where(x => x.Unsuitability_Id == this.Unsuitablity_Id).FirstOrDefault();
                    if (findingsDetails == null)
                    {
                        RuilingOutcome = "No";
                        isRuiling = false;
                    }
                    else
                    {
                        RuilingOutcome = "Yes";
                        isRuiling = true;
                    }
                }
                else
                {
                    RuilingOutcome = "Yes";
                    isRuiling = true;
                }

                return RuilingOutcome;
            }
        }

        public string RegistrationTypeText
        {
            get
            {
                switch (this.RegistrationType)
                {
                    case 1:
                        return "Court";
                        break;
                    case 0:
                        return "Forum";
                        break;
                    default:
                        return "Not Set";
                        break;
                }
            }
        }

        public Person UnsuitablePerson
        {
            get
            {
                var person = new PersonModel();
                var personDetails = person.GetSpecificPerson(this.Person_Id);

                return personDetails;
            }
        }
        public string PhysicalAddress1 { get; set; }
        public string PhysicalAddress2 { get; set; }
        public string PhysicalPostalCode { get; set; }

        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        //public string PostalCode { get; set; }
    }

    [MetadataType(typeof(IncidentMetadata))]
    public partial class CPR_Incident
    {
        public bool Is_Current_User_Supervisor { get; set; }

        [Display(Name = "Supervisor Password")]
        public string Supervisor_Password { get; set; }

        [Display(Name = "Incident Location")]
        public SelectList Incident_Location_List
        {
            get
            {
                var incidentLocationModel = new IncidentLocationModel();
                var listOfIncidentLocations = incidentLocationModel.GetListOfIncidentLocations();

                var incidentLocationList = (from i in listOfIncidentLocations
                                            select new SelectListItem()
                                            {
                                                Text = i.Description,
                                                Value = i.Incident_Location_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = i.Incident_Location_Id.Equals(Incident_Location_Id)
                                            }).ToList();

                var selectList = new SelectList(incidentLocationList, "Value", "Text", Incident_Location_Id);

                return selectList;
            }
        }

        public int? Selected_Province_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provincesList = (from p in listOfProvinces
                                     select new SelectListItem()
                                     {
                                         Text = p.Description,
                                         Value = p.Province_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = p.Province_Id.Equals(Selected_Province_Id)
                                     }).ToList();

                var selectList = new SelectList(provincesList, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }

        [Display(Name = "District")]
        public SelectList District_List
        {
            get
            {
                var districtModel = new DistrictModel();
                var listOfDistricts = new List<District>();

                listOfDistricts = districtModel.GetListOfDistricts(Selected_Province_Id ?? -1);

                var districtList = (from d in listOfDistricts
                                    select new SelectListItem()
                                    {
                                        Text = d.Description,
                                        Value = d.District_Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = d.District_Id.Equals(Incident_District_Id)
                                    }).ToList();


                var selectList = new SelectList(districtList, "Value", "Text", Incident_District_Id);

                return selectList;
            }
        }

        public int Selected_Abuse_Type_Id
        {
            get
            {
                var primaryAbuseTypeId = (from a in Incident_Abuse_Types
                                          where a.Is_Primary_Abuse_Type
                                          select a.Abuse_Type.Abuse_Type_Id).FirstOrDefault();

                return primaryAbuseTypeId;
            }
        }

        public Abuse_Type Selected_Primary_Abuse_Type
        {
            get
            {
                var primaryAbuseType = (from a in Incident_Abuse_Types
                                        where a.Is_Primary_Abuse_Type
                                        select a.Abuse_Type).LastOrDefault();

                return primaryAbuseType;
            }
        }

        public List<int> Selected_Secondary_Abuse_Type_Ids { get; set; }

        [Display(Name = "Primary Abuse Type")]
        public SelectList Abuse_Type_List
        {
            get
            {
                var abuseTypeModel = new AbuseTypeModel();
                var listOfAbuseTypes = abuseTypeModel.GetListOfAbuseTypes();

                var abuseTypeslist = (from a in listOfAbuseTypes
                                      select new SelectListItem()
                                      {
                                          Text = a.Description,
                                          Value = a.Abuse_Type_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = a.Abuse_Type_Id.Equals(Selected_Abuse_Type_Id)
                                      }).ToList();

                var selectlist = new SelectList(abuseTypeslist, "Value", "Text", Selected_Abuse_Type_Id);

                return selectlist;
            }
        }

        [Display(Name = "Multiple Abuse")]
        public List<Abuse_Type> Secondary_Abuse_Type_List
        {
            get
            {
                var abuseTypesList = new List<Abuse_Type>();

                var abuseTypeModel = new AbuseTypeModel();
                var listOfAbuseTypes = abuseTypeModel.GetListOfAbuseTypes();

                abuseTypesList.AddRange(listOfAbuseTypes);

                return abuseTypesList;
            }

        }

        public List<int> Selected_Abuse_Indicator_Ids { get; set; }

        [Display(Name = "Abuse Indicators")]
        public List<Abuse_Indicator> Abuse_Indicator_List
        {
            get
            {
                var abuseIndicatorList = new List<Abuse_Indicator>();

                var abuseIndicatorModel = new AbuseIndicatorModel();
                var listOfAbuseIndicators = abuseIndicatorModel.GetListOfAbuseIndicators(Selected_Abuse_Type_Id);

                abuseIndicatorList.AddRange(listOfAbuseIndicators);

                return abuseIndicatorList;
            }
        }

        [Display(Name = "Risk to Child")]
        public SelectList Risk_Indicator_List
        {
            get
            {
                var riskIndicatorModel = new RiskIndicatorModel();
                var listOfRiskIndicators = riskIndicatorModel.GetListOfRiskIndicators();

                var riskIndicatorsList = (from r in listOfRiskIndicators
                                          select new SelectListItem()
                                          {
                                              Text = r.Description,
                                              Value = r.Risk_Indicator_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = r.Risk_Indicator_Id.Equals(Risk_Indicator_Id)
                                          }).ToList();

                var selectList = new SelectList(riskIndicatorsList, "Value", "Text", Risk_Indicator_Id);

                return selectList;
            }
        }

        [Display(Name = "Case Closure Reason")]
        public List<Case_Closure_Reason> CaseClosureReasonList
        {
            get
            {
                var caseClosureReasonList = new List<Case_Closure_Reason>();

                var caseClosureReasonModel = new CaseClosureReasonModel();
                var listOfCaseClosureReasons = caseClosureReasonModel.GetListOfCaseClosureReadings();

                caseClosureReasonList.AddRange(listOfCaseClosureReasons);

                return caseClosureReasonList;
            }
        }
    }

    [MetadataType(typeof(MedicalDetailMetadata))]
    public partial class Medical_Detail
    {
        [Display(Name = "Treatment Given")]
        public SelectList Treatment_Type_List
        {
            get
            {
                var treatmentTypeModel = new TreatmentTypeModel();
                var listOfTreatmentTypes = treatmentTypeModel.GetListOfTreatmentTypes();

                var treatmentTypesList = (from t in listOfTreatmentTypes
                                          select new SelectListItem()
                                          {
                                              Text = t.Description,
                                              Value = t.Treatment_Type_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = t.Treatment_Type_Id.Equals(Treatment_Type_Id)
                                          }).ToList();

                var selectList = new SelectList(treatmentTypesList, "Value", "Text", Treatment_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Treatment Given By")]
        public SelectList Treatment_Given_By_List
        {
            get
            {
                var treatmentGivenByModel = new TreatmentGivenByModel();
                var listOfTreatmentGivenByItems = treatmentGivenByModel.GetListOfTreatmentGivenByItems();

                var treatmentGivenByList = (from t in listOfTreatmentGivenByItems
                                            select new SelectListItem()
                                            {
                                                Text = t.Description,
                                                Value = t.Treatment_Given_By_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = t.Treatment_Given_By_Id.Equals(Treatment_Type_Id)
                                            }).ToList();

                var selectList = new SelectList(treatmentGivenByList, "Value", "Text", Treatment_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Place Treatment Given")]
        public SelectList Treatment_Place_List
        {
            get
            {
                var treatmentPlaceModel = new TreatmentPlaceModel();
                var listOfTreatmentPlaces = treatmentPlaceModel.GetListOfTreatmentPlaces();

                var treatmentPlacesList = (from t in listOfTreatmentPlaces
                                           select new SelectListItem()
                                           {
                                               Text = t.Description,
                                               Value = t.Treatment_Place_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = t.Treatment_Place_Id.Equals(Treatment_Type_Id)
                                           }).ToList();

                var selectList = new SelectList(treatmentPlacesList, "Value", "Text", Treatment_Type_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(AllegedOffenderMetadata))]
    public partial class Alleged_Offender
    {
        [Display(Name = "Relationship to Child")]
        public SelectList Child_Relationship_Type_List
        {
            get
            {
                var relationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = relationshipTypeModel.GetListOfRelationshipTypes();

                var relationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(Child_Relationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(relationshipTypesList, "Value", "Text", Child_Relationship_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Occupation")]
        public SelectList Occupation_List
        {
            get
            {
                var occupationModel = new OccupationModel();
                var listOfOccupations = occupationModel.GetListOfOccupations();

                var occupationsList = (from o in listOfOccupations
                                       select new SelectListItem()
                                       {
                                           Text = o.Description,
                                           Value = o.Occupation_Id.ToString(CultureInfo.InvariantCulture),
                                           Selected = o.Occupation_Id.Equals(Occupation_Id)
                                       }).ToList();

                var selectList = new SelectList(occupationsList, "Value", "Text", Occupation_Id);

                return selectList;
            }
        }
    }

    public partial class CPR_OnlineNotifications_Reporter
    {
        public int RelationShipTypeId { get; set; }
        public SelectList RelationshipTypeList
        {
            get
            {
                var relationships = new RelationshipTypeModel();
                var listOfRelationships = relationships.GetListOfRelationshipTypes();

                var employers = (from m in listOfRelationships
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Relationship_Type_Id.Equals(RelationShipTypeId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", RelationShipTypeId);

                return selectList;
            }
        }
    }


    public partial class CPR_OnlineNotification_MandatoryReporter
    {
        public int SelectedTown_Id { get; set; }

        public SelectList TownList
        {
            get
            {
                var towns = new TownModel();
                var listOfTowns = towns.GetListOfTowns();

                var employers = (from m in listOfTowns
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Town_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Town_Id.Equals(SelectedTown_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedTown_Id);

                return selectList;
            }
        }


        public int SelectedMagDistrict { get; set; }
        public SelectList MagistrateList
        {
            get
            {
                var districts = new DistrictModel();
                var listOfDistricts = districts.GetListOfDistricts();

                var employers = (from m in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.District_Id.Equals(SelectedMagDistrict)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedMagDistrict);

                return selectList;
            }
        }

        public int SelectedProvinceId { get; set; }
        public SelectList ProvinceList
        {
            get
            {
                var provinces = new ProvinceModel();
                var listOfProvinces = provinces.GetListOfProvinces();

                var employers = (from m in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.Province_Id.Equals(SelectedProvinceId)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedProvinceId);

                return selectList;
            }
        }

        public int SelectedObliged_Id { get; set; }

        public SelectList MandatoryObligedList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfTitles = (from m in _db.CPR_MandatoryObliged
                                    select m).ToList();

                var employers = (from m in listOfTitles
                                 select new SelectListItem()
                                 {
                                     Text = m.Description,
                                     Value = m.MandatoryObliged_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = m.MandatoryObliged_Id.Equals(SelectedObliged_Id)
                                 }).ToList();

                var selectList = new SelectList(employers, "Value", "Text", SelectedObliged_Id);

                return selectList;
            }
        }
    }

    

    [MetadataType(typeof(CPRSection153Metadata))]
    public partial class CPR_Section_153
    {
        public List<Available_Section_153_Item> Available_Section_153_Items
        {
            get
            {
                var section153ItemModel = new Section153ItemModel();
                var section153ItemList = section153ItemModel.GetListOfSection153Items();

                var availableItems = (from x in section153ItemList
                                      let items = x.CPR_Section_153_Items.Where(y => y.Alleged_Offender_Id.Equals(Alleged_Offender_Id)).FirstOrDefault()
                                      select new Available_Section_153_Item
                                      {
                                          Section_153_Item_Id = x.Section_153_Item_Id,
                                          Description = x.Description,
                                          Tooltip = x.Tooltip,
                                          Is_Selected = items != null ? items.Section_153_Items.FirstOrDefault(y => y.Section_153_Item_Id.Equals(x.Section_153_Item_Id)) == null ? false : true : false
                                      }).ToList();

                return availableItems;
            }
        }

        public PostedSection153Items Posted_Section_153_Items { get; set; }

        public string Section_153_Items_String { get; set; }
    }

    public partial class Available_Section_153_Item
    {
        public int Section_153_Item_Id { get; set; }
        public string Description { get; set; }
        public string Tooltip { get; set; }
        public bool Is_Selected { get; set; }
    }

    // Helper class to make posting back selected values easier
    public class PostedSection153Items
    {
        public string[] Section_153_Item_IDs { get; set; }
    }

    [MetadataType(typeof(CPRConvictionMetadata))]
    public partial class CPR_Conviction
    {
        [Display(Name = "Court")]
        public SelectList Court_List
        {
            get
            {
                var courtModel = new CourtModel();
                var listOfCourts = courtModel.GetListOfCourts();

                var courtsList = (from c in listOfCourts
                                  select new SelectListItem()
                                  {
                                      Text = c.Description,
                                      Value = c.Court_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = c.Court_Id.Equals(Court_Id)
                                  }).ToList();

                var selectList = new SelectList(courtsList, "Value", "Text", Court_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(CPRInformantMetadata))]
    public partial class CPR_Informant
    {
        [Display(Name = "District")]
        public SelectList District_List
        {
            get
            {
                var districtModel = new DistrictModel();
                var listOfDistricts = districtModel.GetListOfDistricts();

                var districtList = (from d in listOfDistricts
                                    select new SelectListItem()
                                    {
                                        Text = d.Description,
                                        Value = d.District_Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = d.District_Id.Equals(District_Id)
                                    }).ToList();

                var selectList = new SelectList(districtList, "Value", "Text", District_Id);

                return selectList;
            }
        }

        [Display(Name = "Capacity")]
        public SelectList Informant_Capacity_Type_List
        {
            get
            {
                var informantCapacityTypeModel = new InformantCapacityTypeModel();
                var listOfInformantCapacityTypes = informantCapacityTypeModel.GetListOfInformantCapacityTypes();

                var informantCapacityTypesList = (from i in listOfInformantCapacityTypes
                                                  select new SelectListItem()
                                                  {
                                                      Text = i.Description,
                                                      Value = i.Informant_Capacity_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                      Selected = i.Informant_Capacity_Type_Id.Equals(Informant_Capacity_Type_Id)
                                                  }).ToList();

                var selectList = new SelectList(informantCapacityTypesList, "Value", "Text", Informant_Capacity_Type_Id);

                return selectList;
            }
        }
        
        //public int Child_Relationship_Type_Id { get; set; }
        [Display(Name = "Relationship to Child")]
        public SelectList Child_Relationship_Type_List
        {
            get
            {
                var relationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = relationshipTypeModel.GetListOfRelationshipTypes();

                var relationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(Relationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(relationshipTypesList, "Value", "Text", Relationship_Type_Id);

                return selectList;
            }
        }

        public string Relationship_Type_Description
        {
            get
            {
                string RelationshipText = "Unknown";
                var _db = new SDIIS_DatabaseEntities();

                var relationship = _db.Relationship_Types.Where(x => x.Relationship_Type_Id == this.Relationship_Type_Id).FirstOrDefault();
                if (relationship == null)
                {
                    RelationshipText = "Unknown";
                }else
                {
                    RelationshipText = relationship.Description;
                }

                return RelationshipText;
            }
        }
    }

    public partial class CPR_First_Reporter
    {
        [Display(Name = "District")]
        public SelectList District_List
        {
            get
            {
                var districtModel = new DistrictModel();
                var listOfDistricts = districtModel.GetListOfDistricts();

                var districtList = (from d in listOfDistricts
                                    select new SelectListItem()
                                    {
                                        Text = d.Description,
                                        Value = d.District_Id.ToString(CultureInfo.InvariantCulture),
                                        Selected = d.District_Id.Equals(District_Id)
                                    }).ToList();

                var selectList = new SelectList(districtList, "Value", "Text", District_Id);

                return selectList;
            }
        }

        [Display(Name = "Relationship to Child")]
        public SelectList Child_Relationship_Type_List
        {
            get
            {
                var relationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = relationshipTypeModel.GetListOfRelationshipTypes();

                var relationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(Child_Relationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(relationshipTypesList, "Value", "Text", Child_Relationship_Type_Id);

                return selectList;
            }
        }

        public int Selected_First_Reporter_Type_Id { get; set; }
        public List<CPR_First_Reporter_Type> First_Reporter_Type_List
        {
            get
            {
                var cprReporterTypeSocial = new CPR_First_Reporter_Type() { First_Reporter_Type_Description = "Social Worker", First_Reporter_Type_Id = 1 };
                var cprReporterTypePublic = new CPR_First_Reporter_Type() { First_Reporter_Type_Description = "Member of the Public", First_Reporter_Type_Id = 2 };

                return new List<CPR_First_Reporter_Type> { cprReporterTypeSocial, cprReporterTypePublic };
            }
        }

        public int Selected_Social_Worker_Id { get; set; }

        [Display(Name = "Social Worker")]
        public SelectList Social_Worker_List
        {
            get
            {
                var socialWorkerModel = new SocialWorkerModel();
                var listOfSocialWorkers = socialWorkerModel.GetListOfSocialWorkers(false, false);

                var socialWorkerList = (from x in listOfSocialWorkers
                                        select new SelectListItem()
                                        {
                                            Text = string.Format("{0} {1}", x.apl_User.First_Name, x.apl_User.Last_Name),
                                            Value = x.User_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = x.User_Id.Equals(Selected_Social_Worker_Id)

                                        }).ToList();

                var selectList = new SelectList(socialWorkerList, "Value", "Text", Selected_Social_Worker_Id);

                return selectList;
            }
        }
    }

    public partial class CPR_First_Reporter_Type
    {
        public int First_Reporter_Type_Id { get; set; }
        public string First_Reporter_Type_Description { get; set; }
    }

    [MetadataType(typeof(CPRSAPSDetailMetadata))]
    public partial class CPR_SAPS_Detail
    {
        [Display(Name = "Reported At Police Station")]
        public SelectList Reported_Police_Station_List
        {
            get
            {
                var sapsStationModel = new SAPSStationModel();
                var listOfSAPSStations = sapsStationModel.GetListOfSAPSStations().OrderBy(x=>x.Description);

                var sapsStationsList = (from s in listOfSAPSStations
                                        select new SelectListItem()
                                        {
                                            Text = s.Description,
                                            Value = s.SAPS_Station_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = s.SAPS_Station_Id.Equals(Reported_Police_Station_Id)
                                        }).ToList();

                var selectList = new SelectList(sapsStationsList, "Value", "Text", Reported_Police_Station_Id);

                return selectList;
            }
        }

        [Display(Name = "Investigating Officer")]
        public SelectList Investigating_Officer_List
        {
            get
            {
                var sapsOfficialModel = new SAPSOfficialModel();
                var listOfSAPSOfficials = sapsOfficialModel.GetListOfSAPSOfficials();

                var sapsOfficialsList = (from s in listOfSAPSOfficials
                                         select new SelectListItem()
                                         {
                                             Text = s.First_Name + ' ' + s.Last_Name,
                                             Value = s.SAPS_Official_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = s.SAPS_Official_Id.Equals(Investigating_Officer_Id)
                                         }).ToList();

                var selectList = new SelectList(sapsOfficialsList, "Value", "Text", Investigating_Officer_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(CPRChildrensCourtDetailMetadata))]
    public partial class CPR_Childrens_Court_Detail
    {
        public List<Section_173_Item> Available_Section_173_Items
        {
            get
            {
                var section173ItemModel = new Section173Model();
                var section173ItemList = section173ItemModel.GetListOfSection173Items();

                return section173ItemList;
            }
        }

        public PostedSection173Items Posted_Section_173_Items { get; set; }

        public string Section_173_Items_String { get; set; }

        [Display(Name = "Outcome")]
        public List<Statutory_Outcome_Item> Statutory_Outcome_List
        {
            get
            {
                var statutoryOutcomeModel = new StatutoryOutcomeModel();
                var statutoryOutcomeList = statutoryOutcomeModel.GetListOfStatutoryOutcomeItems();

                return statutoryOutcomeList;
            }
        }

        public int Selected_Statutory_Outcome_Id { get; set; }

        [Display(Name = "Court Name")]
        public SelectList Statutory_Court_List
        {
            get
            {
                var courtModel = new CourtModel();
                var selectedCourtItem = courtModel.GetSpecificCourt(Court_Id ?? -1);

                var listOfCourts = courtModel.GetListOfCourts();

                if (selectedCourtItem != null) listOfCourts.RemoveAll(x => x.District_Id != selectedCourtItem.District_Id);

                var courtsList = (from c in listOfCourts
                                  select new SelectListItem()
                                  {
                                      Text = c.Description,
                                      Value = c.Court_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = c.Court_Id.Equals(Court_Id)
                                  }).ToList();

                var selectList = new SelectList(courtsList, "Value", "Text", Court_Id);

                return selectList;
            }
        }

        public List<Need_for_Care_Reason_Item> Available_Need_For_Care_Reason_Items
        {
            get
            {
                var needForCareReasonModel = new NeedForCareReasonModel();
                var needForCareReasonItemList = needForCareReasonModel.GetListOfNeedForCareReasonItems();

                return needForCareReasonItemList;
            }
        }

        public PostedNeedForCareReasonItems Posted_Need_For_Care_Reason_Items { get; set; }

        public string Need_For_Care_Items_String { get; set; }

        public List<Court_Outcome_Item> Available_Court_Outcome_Items
        {
            get
            {
                var courtOutcomeModel = new CourtOutcomeModel();
                var courtOutcomeItemList = courtOutcomeModel.GetListOfCourtOutcomeItems();

                return courtOutcomeItemList;
            }
        }

        public int Selected_Court_Outcome_Id
        {
            get
            {
                return Court_Outcomes.Any() ? Court_Outcomes.First().Court_Outcome_Id : 0;
            }
        }

        public string Court_Outcome_Items_String { get; set; }
    }

    public partial class Court_Outcome_Item
    {
        public string Full_Description
        {
            get
            {
                var item = this;
                var fullDescription = item.Description;

                var courtOutcomeModel = new CourtOutcomeModel();

                var courtOutcomeHeadingItem = courtOutcomeModel.GetCourtOutcomeItemHeading(item.Court_Outcome_Id);
                if (courtOutcomeHeadingItem != null) fullDescription = courtOutcomeHeadingItem.Item_Number + " " + fullDescription;

                while (item.Parent_Court_Outcome_Item != null)
                {
                    fullDescription = item.Parent_Court_Outcome_Item.Item_Number + " " + fullDescription;

                    var parentHeadingItem = courtOutcomeModel.GetCourtOutcomeItemHeading(item.Parent_Id ?? -1);
                    if (parentHeadingItem != null) fullDescription = parentHeadingItem.Item_Number + " " + fullDescription;

                    item = item.Parent_Court_Outcome_Item;
                }

                return "156 (1) " + fullDescription;
            }
        }
    }



    // Helper class to make posting back selected values easier
    public class PostedSection173Items
    {
        public string[] Section_173_Item_IDs { get; set; }
    }

    public class PostedNeedForCareReasonItems
    {
        public string[] Need_For_Care_Reason_Item_Ids { get; set; }
    }

    [MetadataType(typeof(CPRIncidentMonitoringMetadata))]
    public partial class Incident_Monitoring_Item
    {
        public string Normal_Monitoring_Stopped_Date_And_Reason
        {
            get
            {
                return Normal_Monitoring_Off_Date == null ? null : string.Format("Stopped: {0} - Reason: {1}", ((DateTime)Normal_Monitoring_Off_Date).ToString("dd MMM yyy"), Normal_Monitoring_Off_Reason);
            }
        }

        public string Form36_Monitoring_Stopped_Date_And_Reason
        {
            get
            {
                return Form36_Monitoring_Off_Date == null ? null : string.Format("Stopped: {0} - Reason: {1}", ((DateTime)Form36_Monitoring_Off_Date).ToString("dd MMM yyy"), Form36_Monitoring_Off_Reason);
            }
        }

        public string Childrens_Court_Monitoring_Stopped_Date_And_Reason
        {
            get
            {
                return Childrens_Court_Monitoring_Off_Reason == null ? null : string.Format("Stopped: {0} - Reason: {1}", ((DateTime)Childrens_Court_Monitoring_Off_Date).ToString("dd MMM yyy"), Childrens_Court_Monitoring_Off_Reason);
            }
        }

        public string Criminal_Court_Monitoring_Stopped_Date_And_Reason
        {
            get
            {
                return Criminal_Court_Monitoring_Off_Date == null ? null : string.Format("Stopped: {0} - Reason: {1}", ((DateTime)Criminal_Court_Monitoring_Off_Date).ToString("dd MMM yyy"), Criminal_Court_Monitoring_Off_Reason);
            }
        }

        [Display(Name = "Extension Date")]
        public DateTime? Normal_Extension_Date { get; set; }

        [Display(Name = "Extension Date")]
        public DateTime? Form36_Extension_Date { get; set; }

        [Display(Name = "Extension Date")]
        public DateTime? ChildrensCourt_Extension_Date { get; set; }

        [Display(Name = "Extension Date")]
        public DateTime? CriminalCourt_Extension_Date { get; set; }

        public bool Normal_Monitoring_Currently_Stopped { get; set; }

        public bool Form36_Monitoring_Currently_Stopped { get; set; }

        public bool Childrens_Court_Monitoring_Currently_Stopped { get; set; }

        public bool Criminal_Court_Monitoring_Currently_Stopped { get; set; }

        public string Monitoring_Stop_Reason { get; set; }
    }

    [MetadataType(typeof(CaseNoteMetadata))]
    public partial class Case_Note
    {
        public int Selected_Office_Type_Id { get; set; }

        [Display(Name = "Office")]
        public List<Office_Type> Office_Type_List
        {
            get
            {
                var officeTypeModel = new OfficeTypeModel();
                var officeTypeList = officeTypeModel.GetListOfOfficeTypes();

                return officeTypeList;
            }
        }
    }

    [MetadataType(typeof(ActionTakenMetadata))]
    public partial class Action_Taken
    {
        public int Selected_Office_Type_Id { get; set; }

        [Display(Name = "Office")]
        public List<Office_Type> Office_Type_List
        {
            get
            {
                var officeTypeModel = new OfficeTypeModel();
                var officeTypeList = officeTypeModel.GetListOfOfficeTypes();

                return officeTypeList;
            }
        }
    }

    public partial class IncidentGridMain
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public int AssessmentCount { get; set; }
        public int IncidentCount { get; set; }
        public List<IncidentGridNested> NestedItems { get; set; }
        public DateTime DateReported { get; set; }
    }

    public partial class IncidentGridNested
    {
        public int ClientId { get; set; }
        public int AssessmentId { get; set; }
        public DateTime AssessmentDate { get; set; }
        public DateTime DateReported { get; set; }
        public int? IncidentId { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string CPRReferenceNumber { get; set; }
        public string DistrictDescription { get; set; }
        public string ProvinceDescription { get; set; }
        public string PrimaryAbuse { get; set; }
        public string IsCaseClosed { get; set; }
    }

    #endregion

    #region NISIS

    [MetadataType(typeof(NisisSiteMetadata))]
    public partial class NISIS_Site
    {
        public int Selected_Province_Id { get; set; }

        public int Selected_Municipality_Id { get; set; }

        public int Selected_Local_Municipality_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provinces = (from x in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Province_Id.Equals(Selected_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }

        public string Province_Description
        {
            get
            {
                return Province_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "District Municipality")]
        public SelectList Municipality_List
        {
            get
            {
                var municipalityModel = new DistrictModel();
                var listOfMunicipalities = municipalityModel.GetListOfDistricts(Selected_Province_Id);

                var municipalities = (from x in listOfMunicipalities
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.District_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.District_Id.Equals(Selected_Municipality_Id)
                                      }).ToList();

                var selectList = new SelectList(municipalities, "Value", "Text", Selected_Municipality_Id);

                return selectList;
            }
        }

        public string Municipality_Description
        {
            get
            {
                return Municipality_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Municipality_Id);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Local_Municipality_Id.Equals(Selected_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Selected_Local_Municipality_Id);

                return selectList;
            }
        }

        public string Local_Municipality_Description
        {
            get
            {
                return Local_Municipality_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Ward")]
        public SelectList Ward_List
        {
            get
            {
                var nisisWardModel = new NisisWardModel();
                var listOfNisisWards = nisisWardModel.GetListOfNisisWards(false, false, Selected_Local_Municipality_Id);

                var nisisWards = (from x in listOfNisisWards
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Ward_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Ward_Id.Equals(NISIS_Ward_Id)
                                  }).ToList();

                var selectList = new SelectList(nisisWards, "Value", "Text", NISIS_Ward_Id);

                return selectList;
            }
        }

        public string Ward_Description
        {
            get
            {
                return Ward_List.First(x => x.Selected).Text;
            }
        }

        public string Grouping_Flag_Description
        {
            get
            {
                return NISIS_Grouping_Flags == null ? "Not Specified" : string.Join(",", NISIS_Grouping_Flags.Select(x => x.Description).ToArray());
            }
        }

        [Display(Name = "Grouping Flags")]
        public List<NISIS_Grouping_Flag> Grouping_Flag_List
        {
            get
            {
                var groupingFlagList = new List<NISIS_Grouping_Flag>();

                var groupingFlagModel = new NisisGroupingFlagModel();
                var listOfGroupingFlags = groupingFlagModel.GetListOfGroupingFlags();

                groupingFlagList.AddRange(listOfGroupingFlags);

                return groupingFlagList;
            }

        }

        public List<int> Selected_Grouping_Flag_Ids { get; set; }

        [Display(Name = "Registered Programme")]
        public SelectList Registered_Programme_SelectList
        {
            get
            {
                var programmeModel = new NisisProgrammeModel();
                var listOfProgrammes = programmeModel.GetListOfNisisProgrammes();

                var programmes = (from x in listOfProgrammes
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Programme_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Programme_Id.Equals(Registered_Programme_Id)
                                  }).ToList();

                var selectList = new SelectList(programmes, "Value", "Text", Registered_Programme_Id);

                return selectList;
            }
        }

        [Display(Name = "Registered Programme")]
        public List<NISIS_Programme> Registered_Programme_List
        {
            get
            {
                var registeredProgrammeList = new List<NISIS_Programme>();

                var programmeModel = new NisisProgrammeModel();
                var listOfProgrammes = programmeModel.GetListOfNisisProgrammes();

                registeredProgrammeList.AddRange(listOfProgrammes);

                return registeredProgrammeList;
            }

        }

        public List<int> Selected_Registered_Programme_Ids { get; set; }


        public string Registered_Programme_Description
        {
            get
            {
                return Registered_Programmes == null ? "Not Specified" : string.Join(",", Registered_Programmes.Select(x => x.Description).ToArray());
            }
        }

        [Display(Name = "Programme Status")]
        public SelectList Registered_Programme_Status_List
        {
            get
            {
                var programmeStatusModel = new NisisProgrammeStatusModel();
                var listOfProgrammeStatusses = programmeStatusModel.GetListOfNisisProgrammeStatusses();

                var programmeStatusses = (from x in listOfProgrammeStatusses
                                          select new SelectListItem()
                                          {
                                              Text = x.Description,
                                              Value = x.Programme_Status_Item_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = x.Programme_Status_Item_Id.Equals(Registered_Programme_Status_Id)
                                          }).ToList();

                var selectList = new SelectList(programmeStatusses, "Value", "Text", Registered_Programme_Status_Id);

                return selectList;
            }
        }

        public string Registered_Programme_Status_Description
        {
            get
            {
                return Registered_Programme_Status_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Registered_Programme_Status_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Responsible Programme")]
        public SelectList Responsible_Programme_List
        {
            get
            {
                var programmeModel = new NisisProgrammeModel();
                var listOfProgrammes = programmeModel.GetListOfNisisProgrammes();

                var programmes = (from x in listOfProgrammes
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Programme_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Programme_Id.Equals(Responsible_Programme_Id)
                                  }).ToList();

                var selectList = new SelectList(programmes, "Value", "Text", Responsible_Programme_Id);

                return selectList;
            }
        }

        public string Responsible_Programme_Description
        {
            get
            {
                return Responsible_Programme_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Responsible_Programme_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Listing Method")]
        public SelectList Listing_Method_List
        {
            get
            {
                var listingMethodModel = new NisisListingMethodModel();
                var listOfListingMethods = listingMethodModel.GetListOfNisisListingMethods();

                var listingMethods = (from x in listOfListingMethods
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.NISIS_Listing_Method_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.NISIS_Listing_Method_Id.Equals(Listing_Method_Id)
                                      }).ToList();

                var selectList = new SelectList(listingMethods, "Value", "Text", Listing_Method_Id);

                return selectList;
            }
        }

        public string Listing_Method_Description
        {
            get
            {
                return Listing_Method_List.FirstOrDefault(x => x.Selected) == null ? "Not Specified" : Listing_Method_List.First(x => x.Selected).Text;
            }
        }

        public string Profiling_Method_Description
        {
            get
            {
                return NISIS_Profiling_Methods == null ? "Not Specified" : string.Join(",", NISIS_Profiling_Methods.Select(x => x.Description).ToArray());
            }
        }

        [Display(Name = "Profiling Method")]
        public List<NISIS_Profiling_Method> Profiling_Method_List
        {
            get
            {
                var profilingMethodList = new List<NISIS_Profiling_Method>();

                var profilingMethodModel = new ProfilingMethodModel();
                var listOfProfilingMethods = profilingMethodModel.GetListOfProfilingMethods();

                profilingMethodList.AddRange(listOfProfilingMethods);

                return profilingMethodList;
            }

        }

        public List<int> Selected_Profiling_Method_Ids { get; set; }

        public string Profiling_Tool_Description
        {
            get
            {
                return NISIS_Profiling_Tools == null ? "Not Specified" : string.Join(",", NISIS_Profiling_Tools.Select(x => x.Description).ToArray());
            }
        }

        [Display(Name = "Household Profiling Tool")]
        public List<Profiling_Tool> Household_Profiling_Tool_List
        {
            get
            {
                var profilingToolList = new List<Profiling_Tool>();

                var profilingToolModel = new ProfilingToolModel();
                var listOfProfilingTools = profilingToolModel.GetListOfProfilingTools(false, false, (int)ProfilingToolTypeEnum.HouseholdProfiling);

                profilingToolList.AddRange(listOfProfilingTools);

                return profilingToolList;
            }
        }

        [Display(Name = "Community Profiling Tool")]
        public List<Profiling_Tool> Community_Profiling_Tool_List
        {
            get
            {
                var profilingToolList = new List<Profiling_Tool>();

                var profilingToolModel = new ProfilingToolModel();
                var listOfProfilingTools = profilingToolModel.GetListOfProfilingTools(false, false, (int)ProfilingToolTypeEnum.CommunityProfiling);

                profilingToolList.AddRange(listOfProfilingTools);

                return profilingToolList;
            }
        }

        public List<int> Selected_Houshold_Profiling_Tool_Ids { get; set; }
    }

    public partial class NisisSiteEAGridMain
    {
        public int NISIS_Site_EA_Id { get; set; }
        public string EA_Code { get; set; }
        public string Community_Name { get; set; }
        public int SegmentCount { get; set; }
        public List<NisisSiteEAGridNested> NestedItems { get; set; }
    }

    public partial class NisisSiteEAGridNested
    {
        public int NISIS_Site_EA_Segment_Id { get; set; }
        public int NISIS_Site_EA_Id { get; set; }
        public int Segment_Number { get; set; }
        public string Boundary_Description { get; set; }
        public string Listing_Start_Point_Description { get; set; }
        public string Listing_Route { get; set; }
    }

    [MetadataType(typeof(NisisSiteEAMetadata))]
    public partial class NISIS_Site_EA
    {
        private readonly Lazy<ICollection<NISIS_Site_EA_Segment>> lazySegments;
        public NISIS_Site_EA(Func<ICollection<NISIS_Site_EA_Segment>> loadSegments)
        {
            this.lazySegments = new Lazy<ICollection<NISIS_Site_EA_Segment>>(loadSegments);
        }
    }

    [MetadataType(typeof(NisisSiteEASegmentMetadata))]
    public partial class NISIS_Site_EA_Segment
    {
        public string EA_Code_With_Segment
        {
            get
            {
                return string.Format("{0}:{1}", NISIS_Site_EA.EA_Code, Segment_Id.ToString(CultureInfo.InvariantCulture));
            }
        }
    }

    [MetadataType(typeof(NisisListingMetadata))]
    public partial class NISIS_Listing
    {
        [Display(Name = "Structure Type")]
        public SelectList Structure_Type_List
        {
            get
            {
                var structureTypeModel = new StructureTypeModel();
                var listOfStructureTypes = structureTypeModel.GetListOfStructureTypes();

                var structureTypes = (from x in listOfStructureTypes
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.Structure_Type_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.Structure_Type_Id.Equals(Structure_Type_Id)
                                      }).ToList();

                var selectList = new SelectList(structureTypes, "Value", "Text", Structure_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Is QA Approved?")]
        public bool Site_QA_Approved { get; set; }
    }

    [MetadataType(typeof(NisisProfilingInstanceMetadata))]
    public partial class Profiling_Instance
    {
        [Display(Name = "Captured by User")]
        public SelectList Captured_By_User_List
        {
            get
            {
                var userModel = new UserModel();
                var listOfUsers = userModel.GetListOfUsers(false, false);

                var users = (from x in listOfUsers
                             select new SelectListItem()
                             {
                                 Text = string.Format("{0} {1}", x.First_Name, x.Last_Name),
                                 Value = x.User_Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = x.User_Id.Equals(Captured_By_UserId)
                             }).ToList();

                var selectList = new SelectList(users, "Value", "Text", Captured_By_UserId);

                return selectList;
            }
        }

        public int Selected_Province_Id { get; set; }

        public int Selected_Municipality_Id { get; set; }

        public int Selected_Local_Municipality_Id { get; set; }

        public int Selected_Ward_Id { get; set; }

        public int Selected_Site_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provinces = (from x in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Province_Id.Equals(Selected_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }

        public string Province_Description
        {
            get
            {
                return NISIS_Site_EA.NISIS_Site.Ward.Local_Municipality.District.Province.Description;
            }
        }

        [Display(Name = "Municipality")]
        public SelectList Municipality_List
        {
            get
            {
                var municipalityModel = new DistrictModel();
                var listOfMunicipalities = municipalityModel.GetListOfDistricts(Selected_Province_Id);

                var municipalities = (from x in listOfMunicipalities
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.District_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.District_Id.Equals(Selected_Municipality_Id)
                                      }).ToList();

                var selectList = new SelectList(municipalities, "Value", "Text", Selected_Municipality_Id);

                return selectList;
            }
        }

        public string District_Municipality_Description
        {
            get
            {
                return NISIS_Site_EA.NISIS_Site.Ward.Local_Municipality.District.Description;
            }
        }

        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Municipality_Id);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Local_Municipality_Id.Equals(Selected_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Selected_Local_Municipality_Id);

                return selectList;
            }
        }

        public string Local_Municipality_Description
        {
            get
            {
                return NISIS_Site_EA.NISIS_Site.Ward.Local_Municipality.Description;
            }
        }

        [Display(Name = "Ward")]
        public SelectList Ward_List
        {
            get
            {
                var nisisWardModel = new NisisWardModel();
                var listOfNisisWards = nisisWardModel.GetListOfNisisWards(false, false, Selected_Local_Municipality_Id);

                var nisisWards = (from x in listOfNisisWards
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Ward_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Ward_Id.Equals(Selected_Ward_Id)
                                  }).ToList();

                var selectList = new SelectList(nisisWards, "Value", "Text", Selected_Ward_Id);

                return selectList;
            }
        }

        public string Ward_Description
        {
            get
            {
                return NISIS_Site_EA.NISIS_Site.Ward.Description;
            }
        }

        [Display(Name = "Site")]
        public SelectList Site_List
        {
            get
            {
                var siteModel = new NisisSiteModel();
                var listOfSites = siteModel.GetListOfNisisSites(false, false, Selected_Ward_Id);

                var sites = (from x in listOfSites
                             select new SelectListItem()
                             {
                                 Text = x.Site_Name,
                                 Value = x.Site_Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = x.Site_Id.Equals(Selected_Site_Id)
                             }).ToList();

                var selectList = new SelectList(sites, "Value", "Text", Selected_Site_Id);

                return selectList;
            }
        }

        public string Site_Description
        {
            get
            {
                return NISIS_Site_EA.NISIS_Site.Site_Name;
            }
        }

        [Display(Name = "EA")]
        public SelectList Site_EA_List
        {
            get
            {
                var eaModel = new NisisSiteEAModel();
                var listofEAs = eaModel.GetListOfNisisSiteEAs(false, false, Selected_Site_Id);

                var eas = (from x in listofEAs
                           select new SelectListItem()
                           {
                               Text = x.EA_Code,
                               Value = x.NISIS_Site_EA_Id.ToString(CultureInfo.InvariantCulture),
                               Selected = x.NISIS_Site_EA_Id.Equals(NISIS_Site_EA_Id)
                           }).ToList();

                var selectList = new SelectList(eas, "Value", "Text", NISIS_Site_EA_Id);

                return selectList;
            }
        }

        public string Site_EA_Description
        {
            get
            {
                return NISIS_Site_EA.EA_Code;
            }
        }

        [Display(Name = "Profiling Tool")]
        public SelectList Profiling_Tool_List
        {
            get
            {
                var profilingToolModel = new ProfilingToolModel();
                var listOfProfilingTools = profilingToolModel.GetListOfProfilingTools(false, false, (int)ProfilingToolTypeEnum.HouseholdProfiling);

                var profilingTools = (from x in listOfProfilingTools
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.Profiling_Tool_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.Profiling_Tool_Id.Equals(Profiling_Tool_Id)
                                      }).ToList();

                var selectList = new SelectList(profilingTools, "Value", "Text", Profiling_Tool_Id);

                return selectList;
            }
        }

        [Display(Name = "Is QA Approved?")]
        public bool Site_QA_Approved { get; set; }
    }

    public partial class Profiling_Participant
    {
        public string Gender_Description
        {
            get
            {
                return Person.Gender == null ? string.Empty : Person.Gender.Description;
            }
        }
    }

    [MetadataType(typeof(NisisCommunityProfilingInstanceMetadata))]
    public partial class Community_Profiling_Instance
    {
        [Display(Name = "Captured by User")]
        public SelectList Captured_By_User_List
        {
            get
            {
                var userModel = new UserModel();
                var listOfUsers = userModel.GetListOfUsers(false, false);

                var users = (from x in listOfUsers
                             select new SelectListItem()
                             {
                                 Text = string.Format("{0} {1}", x.First_Name, x.Last_Name),
                                 Value = x.User_Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = x.User_Id.Equals(Captured_By_UserId)
                             }).ToList();

                var selectList = new SelectList(users, "Value", "Text", Captured_By_UserId);

                return selectList;
            }
        }

        public int Selected_Province_Id { get; set; }

        public int Selected_Municipality_Id { get; set; }

        public int Selected_Local_Municipality_Id { get; set; }

        public int Selected_Ward_Id { get; set; }

        public int Selected_Site_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var provinceModel = new ProvinceModel();
                var listOfProvinces = provinceModel.GetListOfProvinces();

                var provinces = (from x in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Province_Id.Equals(Selected_Province_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", Selected_Province_Id);

                return selectList;
            }
        }

        public string Province_Description
        {
            get
            {
                return Province_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Municipality")]
        public SelectList District_Municipality_List
        {
            get
            {
                var districtMunicipalityModel = new DistrictModel();
                var listOfMunicipalities = districtMunicipalityModel.GetListOfDistricts(Selected_Province_Id);

                var municipalities = (from x in listOfMunicipalities
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.District_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.District_Id.Equals(Selected_Municipality_Id)
                                      }).ToList();

                var selectList = new SelectList(municipalities, "Value", "Text", Selected_Municipality_Id);

                return selectList;
            }
        }

        public string District_Municipality_Description
        {
            get
            {
                return District_Municipality_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Local Municipality")]
        public SelectList Local_Municipality_List
        {
            get
            {
                var localMunicipalityModel = new LocalMunicipalityModel();
                var listOfLocalMunicipalities = localMunicipalityModel.GetListOfLocalMunicipalities(Selected_Municipality_Id);

                var localMunicipalities = (from x in listOfLocalMunicipalities
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Local_Municipality_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Local_Municipality_Id.Equals(Selected_Local_Municipality_Id)
                                           }).ToList();

                var selectList = new SelectList(localMunicipalities, "Value", "Text", Selected_Local_Municipality_Id);

                return selectList;
            }
        }

        public string Local_Municipality_Description
        {
            get
            {
                return Local_Municipality_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Ward")]
        public SelectList Ward_List
        {
            get
            {
                var nisisWardModel = new NisisWardModel();
                var listOfNisisWards = nisisWardModel.GetListOfNisisWards(false, false, Selected_Local_Municipality_Id);

                var nisisWards = (from x in listOfNisisWards
                                  select new SelectListItem()
                                  {
                                      Text = x.Description,
                                      Value = x.NISIS_Ward_Id.ToString(CultureInfo.InvariantCulture),
                                      Selected = x.NISIS_Ward_Id.Equals(Selected_Ward_Id)
                                  }).ToList();

                var selectList = new SelectList(nisisWards, "Value", "Text", Selected_Ward_Id);

                return selectList;
            }
        }

        public string Ward_Description
        {
            get
            {
                return Ward_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Site")]
        public SelectList Site_List
        {
            get
            {
                var siteModel = new NisisSiteModel();
                var listOfSites = siteModel.GetListOfNisisSites(false, false, Selected_Ward_Id);

                var sites = (from x in listOfSites
                             select new SelectListItem()
                             {
                                 Text = x.Site_Name,
                                 Value = x.Site_Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = x.Site_Id.Equals(Selected_Site_Id)
                             }).ToList();

                var selectList = new SelectList(sites, "Value", "Text", Selected_Site_Id);

                return selectList;
            }
        }

        public string Site_Description
        {
            get
            {
                return Site_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "EA")]
        public SelectList Site_EA_List
        {
            get
            {
                var eaModel = new NisisSiteEAModel();
                var listofEAs = eaModel.GetListOfNisisSiteEAs(false, false, Selected_Site_Id);

                var eas = (from x in listofEAs
                           select new SelectListItem()
                           {
                               Text = x.EA_Code,
                               Value = x.NISIS_Site_EA_Id.ToString(CultureInfo.InvariantCulture),
                               Selected = x.NISIS_Site_EA_Id.Equals(NISIS_Site_EA_Id)
                           }).ToList();

                var selectList = new SelectList(eas, "Value", "Text", NISIS_Site_EA_Id);

                return selectList;
            }
        }

        public string Site_EA_Description
        {
            get
            {
                return Site_EA_List.First(x => x.Selected).Text;
            }
        }

        [Display(Name = "Profiling Tool")]
        public SelectList Profiling_Tool_List
        {
            get
            {
                var profilingToolModel = new ProfilingToolModel();
                var listOfProfilingTools = profilingToolModel.GetListOfProfilingTools(false, false, (int)ProfilingToolTypeEnum.CommunityProfiling);

                var profilingTools = (from x in listOfProfilingTools
                                      select new SelectListItem()
                                      {
                                          Text = x.Description,
                                          Value = x.Profiling_Tool_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = x.Profiling_Tool_Id.Equals(Profiling_Tool_Id)
                                      }).ToList();

                var selectList = new SelectList(profilingTools, "Value", "Text", Profiling_Tool_Id);

                return selectList;
            }
        }

        [Display(Name = "Is QA Approved?")]
        public bool Site_QA_Approved { get; set; }
    }

    [MetadataType(typeof(NisisProfilingInstanceReferralMetadata))]
    public partial class NISIS_Profiling_Instance_Referral
    {
        [Display(Name = "Rollout Site")]
        public string Rollout_Site_Description
        {
            get
            {
                return NISIS_Profiling_Instance.Province_Description + " > " + NISIS_Profiling_Instance.District_Municipality_Description + " > " + NISIS_Profiling_Instance.Local_Municipality_Description + " > " + NISIS_Profiling_Instance.Ward_Description + " > " + NISIS_Profiling_Instance.Site_Description;
            }
        }

        [Display(Name = "Referral Source")]
        public SelectList Referral_Source_Type_List
        {
            get
            {
                var referralSourceTypeModel = new NisisReferralSourceTypeModel();
                var listOfReferralSourceTypes = referralSourceTypeModel.GetListOfReferralSourceTypes();

                var referralSourceTypes = (from x in listOfReferralSourceTypes
                                           select new SelectListItem()
                                           {
                                               Text = x.Description,
                                               Value = x.Referral_Soure_Type_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = x.Referral_Soure_Type_Id.Equals(Referral_Source_Type_Id)
                                           }).ToList();

                var selectList = new SelectList(referralSourceTypes, "Value", "Text", Referral_Source_Type_Id);

                return selectList;
            }
        }

        [Display(Name = "Referral Source")]
        public string Referral_Source_Type_Description
        {
            get
            {
                return NISIS_Referral_Source_Type.Description ?? string.Empty;
            }
        }

        [Display(Name = "Service Category")]
        public SelectList Service_Category_List
        {
            get
            {
                var serviceCategoryModel = new NisisServiceCategoryModel();
                var listOfServiceCategories = serviceCategoryModel.GetListOfServiceCategories();

                var serviceCategories = (from x in listOfServiceCategories
                                         select new SelectListItem()
                                         {
                                             Text = x.Description,
                                             Value = x.NISIS_Service_Category_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = x.NISIS_Service_Category_Id.Equals(NISIS_Service.Service_Category_Id)
                                         }).ToList();

                var selectList = new SelectList(serviceCategories, "Value", "Text", NISIS_Service.Service_Category_Id);

                return selectList;
            }
        }

        [Display(Name = "Service")]
        public SelectList Service_List
        {
            get
            {
                var serviceModel = new NisisServiceModel();
                var listOfServices = serviceModel.GetListOfServices();

                var services = (from x in listOfServices
                                select new SelectListItem()
                                {
                                    Text = x.Description,
                                    Value = x.Service_Id.ToString(CultureInfo.InvariantCulture),
                                    Selected = x.Service_Id.Equals(Service_Id)
                                }).ToList();

                var selectList = new SelectList(services, "Value", "Text", Service_Id);

                return selectList;
            }
        }

        [Display(Name = "Service")]
        public string Service_Description
        {
            get
            {
                return NISIS_Service == null ? string.Empty : NISIS_Service.Description ?? string.Empty;
            }
        }

        [Display(Name = "Referral Priority")]
        public SelectList Referral_Priority_List
        {
            get
            {
                var referralPriorityModel = new NisisReferralPriorityModel();
                var listOfReferralPriorities = referralPriorityModel.GetListOfReferralPriorities();

                var referralPriorities = (from x in listOfReferralPriorities
                                          select new SelectListItem()
                                          {
                                              Text = x.Description,
                                              Value = x.Referral_Priority_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = x.Referral_Priority_Id.Equals(Referral_Priority_Id)
                                          }).ToList();

                var selectList = new SelectList(referralPriorities, "Value", "Text", Service_Id);

                return selectList;
            }
        }

        [Display(Name = "Referral Priority")]
        public string Referral_Priority_Description
        {
            get
            {
                return NISIS_Referral_Priority.Description ?? string.Empty;
            }
        }

        [Display(Name = "Responsible Person")]
        public SelectList Responsible_Person_List
        {
            get
            {
                var userModel = new UserModel();
                var listOfUsers = userModel.GetListOfUsers(false, false);

                var users = (from x in listOfUsers
                             select new SelectListItem()
                             {
                                 Text = string.Format("{0} {1}", x.First_Name, x.Last_Name),
                                 Value = x.User_Id.ToString(CultureInfo.InvariantCulture),
                                 Selected = x.User_Id.Equals(Responsible_Person_Id)
                             }).ToList();

                var selectList = new SelectList(users, "Value", "Text", Responsible_Person_Id);

                return selectList;
            }
        }

        [Display(Name = "Owner Organization")]
        public SelectList Owner_Organization_List
        {
            get
            {
                var ownerOrganizationModel = new NisisOwnerOrganizationModel();
                var listOfOwnerOrganizations = ownerOrganizationModel.GetListOfOwnerOrganizations();

                var ownerOrganizations = (from x in listOfOwnerOrganizations
                                          select new SelectListItem()
                                          {
                                              Text = x.Description,
                                              Value = x.Owner_Organization_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = x.Owner_Organization_Id.Equals(Owner_Organization_Id)
                                          }).ToList();

                var selectList = new SelectList(ownerOrganizations, "Value", "Text", Service_Id);

                return selectList;
            }
        }

        [Display(Name = "Organization Facilities")]
        public SelectList Organization_Facility_List
        {
            get
            {
                var organizationFacilityModel = new NisisOrganizationFacilityModel();
                var listOfOwnerOrganizations = organizationFacilityModel.GetListOfOrganizationFacilities(Owner_Organization_Id ?? -1);

                var ownerOrganizations = (from x in listOfOwnerOrganizations
                                          select new SelectListItem()
                                          {
                                              Text = x.Description,
                                              Value = x.Owner_Organization_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = x.Owner_Organization_Id.Equals(Organization_Facility_Id)
                                          }).ToList();

                var selectList = new SelectList(ownerOrganizations, "Value", "Text", Organization_Facility_Id);

                return selectList;
            }
        }

        [Display(Name = "Service Status")]
        public SelectList Service_Status_List
        {
            get
            {
                var serviceStatusModel = new NisisServiceStatusModel();
                var listOfServiceStatusses = serviceStatusModel.GetListOfServiceStatusses();

                var serviceStatusses = (from x in listOfServiceStatusses
                                        select new SelectListItem()
                                        {
                                            Text = x.Description,
                                            Value = x.Service_Status_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = x.Service_Status_Id.Equals(Service_Status_Id)
                                        }).ToList();

                var selectList = new SelectList(serviceStatusses, "Value", "Text", Service_Id);

                return selectList;
            }
        }

        [Display(Name = "Service Status")]
        public string Service_Status_Description
        {
            get
            {
                return NISIS_Service_Status.Description ?? string.Empty;
            }
        }

        [Display(Name = "External Verification Status")]
        public SelectList External_Verification_Status_List
        {
            get
            {
                var externalVerificationStatusModel = new NisisExtermalVerificationStatusModel();
                var listOfExternalVerificationStatusses = externalVerificationStatusModel.GetListOfExternalVerificationStatusses();

                var externalVerificationStatusses = (from x in listOfExternalVerificationStatusses
                                                     select new SelectListItem()
                                                     {
                                                         Text = x.Description,
                                                         Value = x.External_Verification_Status_Id.ToString(CultureInfo.InvariantCulture),
                                                         Selected = x.External_Verification_Status_Id.Equals(External_Verification_Status_Id)
                                                     }).ToList();

                var selectList = new SelectList(externalVerificationStatusses, "Value", "Text", Service_Id);

                return selectList;
            }
        }

        [Display(Name = "Captured By")]
        public string Captured_By
        {
            get
            {
                return NISIS_Profiling_Instance.CapturedByUser.First_Name + " " + NISIS_Profiling_Instance.CapturedByUser.Last_Name;
            }
        }

        [Display(Name = "Captured Date")]
        public DateTime Captured_Date
        {
            get
            {
                if (NISIS_Profiling_Instance == null) return DateTime.Now;

                return NISIS_Profiling_Instance.Profiling_Date;
            }
        }
    }

    [MetadataType(typeof(ProfilingToolMetadata))]
    public partial class Profiling_Tool
    {
        [Display(Name = "Profiling Tool Type")]
        public SelectList Profiling_Tool_Type_List
        {
            get
            {
                var profilingToolTypeModel = new ProfilingToolTypeModel();
                var listOfProfilingToolTypes = profilingToolTypeModel.GetListOfProfilingToolTypes();

                var profilingToolTypes = (from x in listOfProfilingToolTypes
                                          select new SelectListItem()
                                          {
                                              Text = x.Description,
                                              Value = x.Profiling_Tool_Type_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = x.Profiling_Tool_Type_Id.Equals(Profiling_Tool_Type_Id)
                                          }).ToList();

                var selectList = new SelectList(profilingToolTypes, "Value", "Text", Profiling_Tool_Type_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(QuestionnaireSectionMetadata))]
    public partial class Questionnaire_Section
    {
        public int MaxColumns { get; set; }
        public QuestionnaireGrid ControlMatrix { get; set; }
    }

    [MetadataType(typeof(QuestionnaireQuestionMetadata))]
    public partial class Questionnaire_Question
    {
        public bool IsDependentOnQuestion { get; set; }

        public int SelectedSectionId { get; set; }

        public SelectList SectionList
        {
            get
            {
                var sectionModel = new QuestionnaireSectionModel();
                var listOfSections = sectionModel.GetListOfQuestionnaireSections(false, false, this.Questionnaire_Section.Questionnaire.Questionnaire_Id);

                var sections = (from x in listOfSections
                                select new SelectListItem()
                                {
                                    Text = x.Section_Name,
                                    Value = x.Questionnaire_Section_Id.ToString(CultureInfo.InvariantCulture),
                                    Selected = x.Questionnaire_Section_Id.Equals(SelectedSectionId)
                                }).ToList();

                var selectList = new SelectList(sections, "Value", "Text", SelectedSectionId);

                return selectList;
            }
        }

        public SelectList QuestionList
        {
            get
            {
                var questionModel = new QuestionnaireQuestionModel();
                var listOfQuestions = questionModel.GetListOfQuestionnaireQuestions(false, false, SelectedSectionId);

                var questions = (from x in listOfQuestions
                                 select new SelectListItem()
                                 {
                                     Text = x.Question_Text,
                                     Value = x.Questionnaire_Question_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = x.Questionnaire_Question_Id.Equals(Depends_on_Question_Id)
                                 }).ToList();

                var selectList = new SelectList(questions, "Value", "Text", Depends_on_Question_Id);

                return selectList;
            }
        }

        public SelectList OptionList
        {
            get
            {
                var optionModel = new QuestionnaireQuestionOptionModel();
                var listOfOptions = optionModel.GetListOfQuestionnaireQuestionOptions(false, false, Questionnaire_Question_Id);

                var options = (from x in listOfOptions
                               select new SelectListItem
                               {
                                   Text = x.Option_Text,
                                   Value = x.Option_Id.ToString(CultureInfo.InvariantCulture),
                                   Selected = x.Option_Id.Equals(Depends_on_Question_Option_Id)
                               }).ToList();

                var selectList = new SelectList(options, "Value", "Text", Depends_on_Question_Option_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(QuestionnaireQuestionOptionMetadata))]
    public partial class Questionnaire_Question_Option
    {
        public int Selected_Service_Category_Id { get; set; }

        public int Selected_Service_Id { get; set; }

        [Display(Name = "Service Referral Category")]
        public SelectList Service_Referral_Category_List
        {
            get
            {
                var serviceCategoryModel = new NisisServiceCategoryModel();
                var listOfServiceCategories = serviceCategoryModel.GetListOfServiceCategories();

                var serviceCategories = (from x in listOfServiceCategories
                                         select new SelectListItem()
                                         {
                                             Text = x.Description,
                                             Value = x.NISIS_Service_Category_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = x.NISIS_Service_Category_Id.Equals(Selected_Service_Category_Id)
                                         }).ToList();

                var selectList = new SelectList(serviceCategories, "Value", "Text", Selected_Service_Category_Id);

                return selectList;
            }
        }

        [Display(Name = "Service Referral")]
        public SelectList Service_Referral_List
        {
            get
            {
                var serviceModel = new NisisServiceModel();
                var listOfServices = serviceModel.GetListOfServices(Selected_Service_Category_Id);

                var services = (from x in listOfServices
                                select new SelectListItem()
                                {
                                    Text = x.Description,
                                    Value = x.Service_Id.ToString(CultureInfo.InvariantCulture),
                                    Selected = x.Service_Id.Equals(Selected_Service_Id)
                                }).ToList();

                var selectList = new SelectList(services, "Value", "Text", Selected_Service_Id);

                return selectList;
            }
        }
    }

    [MetadataType(typeof(QuestionnaireQuestionColumnMetadata))]
    public partial class Questionnaire_Question_Column
    {
        [Display(Name = "Question Type")]
        public SelectList Question_Type_List
        {
            get
            {
                var questionTypeModel = new QuestionnaireQuestionTypeModel();
                var listOfQuestionTypes = questionTypeModel.GetListOfQuestionnaireQuestionTypes();

                var services = (from x in listOfQuestionTypes
                                select new SelectListItem()
                                {
                                    Text = x.Description,
                                    Value = x.Questionnaire_Question_Type_Id.ToString(CultureInfo.InvariantCulture),
                                    Selected = x.Questionnaire_Question_Type_Id.Equals(Questionnaire_Question_Type_Id)
                                }).ToList();

                var selectList = new SelectList(services, "Value", "Text", Questionnaire_Question_Type_Id);

                return selectList;
            }
        }
    }

    #endregion

    #region NISIS QuestionnaireMatrix Helpers

    public class QuestionnaireCell
    {
        public List<Questionnaire_Question_Option> DropdownOptions { get; set; }
        public bool IsAddRowCell { get; set; }
        public bool IsTextCell { get; set; }
        public bool IsVisible { get; set; }
        public string CellText { get; set; }
        public int colSpan { get; set; }
        public int rowSpan { get; set; }
        public int ControlType { get; set; }
        public string ControlId { get; set; }
        public int QuestionId { get; set; }
        public bool IsRequired { get; set; }
        public int ParticipantId { get; set; }
        public int ColumnId { get; set; }
        public string OptionText { get; set; }
        public int OptionId { get; set; }
        public string CellAnswer { get; set; }
        public string DependsOnQuestion { get; set; }
        public string DependsOnOption { get; set; }
        public string ValidationTypes { get; set; }
        public string PopulatedByQuestion { get; set; }
        public string PopulatedByColumn { get; set; }
        public string PopulatedAsType { get; set; }
    }

    public class QuestionnaireRow
    {
        public List<QuestionnaireCell> QuestionnaireCells { get; set; }
    }

    public class QuestionnaireGrid
    {
        public List<QuestionnaireRow> QuestionnaireRows { get; set; }
    }

    #endregion

    #region ACM
    public partial class ACM_AuthorityToRemove
    {
        public int SelectedRelationship_Type_Id { get; set; }
        public SelectList Relationship_Type_List
        {
            get
            {
                var RelationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = RelationshipTypeModel.GetListOfRelationshipTypes();
                var RelationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(SelectedRelationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(RelationshipTypesList, "Value", "Text", SelectedRelationship_Type_Id);
                return selectList;
            }
        }
    }

    public partial class ACM_AcknowledgementOfReceipt
    {
        public int SelectedPlacementType_Id { get; set; }

        [Display(Name = "Type of Placement")]
        public SelectList PlacementType_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var placementtype = (from a in _db.ACM_PlacementType
                                     select a).ToList();

                var placementTypes = (from m in placementtype
                                      select new SelectListItem()
                                      {
                                          Text = m.Description,
                                          Value = m.PlacementType_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = m.PlacementType_Id.Equals(SelectedPlacementType_Id)
                                      }).ToList();

                var selectList = new SelectList(placementTypes, "Value", "Text", SelectedPlacementType_Id);

                return selectList;
            }
        }
    }

    public partial class ACM_ChildAssessment
    {
        public int SelectedRelationship_Type_Id { get; set; }
        public SelectList Relationship_Type_List
        {
            get
            {
                var RelationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = RelationshipTypeModel.GetListOfRelationshipTypes();
                var RelationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(SelectedRelationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(RelationshipTypesList, "Value", "Text", SelectedRelationship_Type_Id);
                return selectList;
            }
        }


        public int SelectedEngagementTypeList_Id { get; set; }
        public SelectList EngagementType_List
        {
            get
            {
                var engagementTypeModel = new EngagementTypeModel();
                var listOfEngagementTypes = engagementTypeModel.GetListOfEngagementTypes();
                var EngagementTypesList = (from i in listOfEngagementTypes
                                           select new SelectListItem()
                                           {
                                               Text = i.Description,
                                               Value = i.EngagementType_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = i.EngagementType_Id.Equals(SelectedEngagementTypeList_Id)
                                           }).ToList();

                var selectList = new SelectList(EngagementTypesList, "Value", "Text", SelectedEngagementTypeList_Id);
                return selectList;
            }
        }
    }

    public partial class ACM_InterviewProcessNote
    {
        //public int CaseWorklist_Id { get; set; }
        public int SelectedInterviewType_Id { get; set; }

        [Display(Name = "Type of Interview")]
        public SelectList InterviewType_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var interviewtype = (from a in _db.ACM_Interview_Type
                                     select a).ToList();

                var interviewTypes = (from m in interviewtype
                                      select new SelectListItem()
                                      {
                                          Text = m.Description,
                                          Value = m.Interview_Type_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = m.Interview_Type_Id.Equals(SelectedInterviewType_Id)
                                      }).ToList();

                var selectList = new SelectList(interviewTypes, "Value", "Text", SelectedInterviewType_Id);

                return selectList;
            }
        }

        public List<ACM_InterviewedPersons> InterviewedPersons_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var interviewedPersons = _db.ACM_InterviewedPersons.Where(x => x.ProcessNote_Id == this.ProcessNote_Id).ToList();
                if (interviewedPersons.Count == 0)
                {
                    interviewedPersons = new List<Models.ACM_InterviewedPersons>();
                }
                return interviewedPersons;
            }
        }
    }

    public partial class ACM_Investigation
    {
        public List<ACM_InterviewProcessNote> ProcessNotesList { get; set; }
        public List<Person> ClientList { get; set; }
    }

    public partial class ACM_IdentifyingChildDetails
    {
        public int CaseWorklist_Id { get; set; }

        public List<ACM_IdentifyingChildDetails> ChildrenFormingPartOfReport_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var childrenFormingPartOfReport = _db.ACM_IdentifyingChildDetails.Where(x => x.ChildDetails_Id == this.ChildDetails_Id).ToList();
                if (childrenFormingPartOfReport.Count == 0)
                {
                    childrenFormingPartOfReport = new List<Models.ACM_IdentifyingChildDetails>();
                }
                return childrenFormingPartOfReport;
            }
        }
    }

    public partial class ACM_SouthAfricanSafetyAssessmentTool
    {
        public List<ACM_ParentCaregiversAssessed> CaregiversAndGuardians
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var listOfCaregivers = _db.ACM_ParentCaregiversAssessed.Where(x => x.Sasat_Id == this.Id).ToList();
                if (listOfCaregivers == null)
                {
                    listOfCaregivers = new List<Models.ACM_ParentCaregiversAssessed>();
                }

                return listOfCaregivers;
            }
        }

        public int SelectedParentGuardianCareGiver_Id { get; set; }

        public SelectList ParentGuardianCareGiver_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var clientId = this.Client_Id;
                var client = _db.Clients.Where(x => x.Client_Id == clientId).FirstOrDefault();
                var test = client.Client_Biological_Parents;
                var test2 = client.Client_Adoptive_Parents;
                var test3 = client.Client_CareGivers;

                var aditionalCareGivers = new List<Person>();
                foreach (var biol in test)
                {
                    aditionalCareGivers.Add(biol.Person);
                }
                foreach (var adopt in test2)
                {
                    aditionalCareGivers.Add(adopt.Person);
                }
                foreach (var care in test3)
                {
                    aditionalCareGivers.Add(care.Person);
                }

                var allCaregivers = (from m in aditionalCareGivers
                                     select new SelectListItem()
                                     {
                                         Text = m.First_Name + " " + m.Last_Name,
                                         Value = m.Person_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = m.Person_Id.Equals(SelectedParentGuardianCareGiver_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedParentGuardianCareGiver_Id);

                return selectList;
            }
        }

    }

    public partial class ACM_ProtectiveCapacities
    {
        public List<ACM_ProtectiveCapacitiesCareGivers> CaregiversAndGuardians { get; set; }

        public int SelectedWillingAndAble_Id { get; set; }
        public SelectList WillingAndAble_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedWillingAndAble_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedWillingAndAble_Id);

                return selectList;
            }
        }

        public int SelectedPhysicalAndEmotionalCapacity_Id { get; set; }
        public SelectList PhysicalAndEmotionalCapacity_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedPhysicalAndEmotionalCapacity_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedPhysicalAndEmotionalCapacity_Id);

                return selectList;
            }
        }

        public int SelectedWillingToReconize_Id { get; set; }
        public SelectList WillingToReconize_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedWillingToReconize_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedWillingToReconize_Id);

                return selectList;
            }
        }

        public int SelectedAbilityToAccessAndUse_Id { get; set; }
        public SelectList AbilityToAccessAndUse_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedAbilityToAccessAndUse_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedAbilityToAccessAndUse_Id);

                return selectList;
            }
        }

        public int SelectedSupportiveReltionships_Id { get; set; }
        public SelectList SupportiveReltionships_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedSupportiveReltionships_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedSupportiveReltionships_Id);

                return selectList;
            }
        }

        public int SelectedAcceptTheInvolvement_Id { get; set; }
        public SelectList AcceptTheInvolvement_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedAcceptTheInvolvement_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedAcceptTheInvolvement_Id);

                return selectList;
            }
        }

        public int SelectedHealthyRelationship_Id { get; set; }
        public SelectList HealthyRelationship_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedHealthyRelationship_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedHealthyRelationship_Id);

                return selectList;
            }
        }

        //AwareOfAndCommited
        public int SelectedAwareOfAndCommited_Id { get; set; }
        public SelectList AwareOfAndCommited_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedAwareOfAndCommited_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedAwareOfAndCommited_Id);

                return selectList;
            }
        }

        //EffectiveProblemsolving
        public int SelectedEffectiveProblemsolving_Id { get; set; }
        public SelectList EffectiveProblemsolving_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var allCaregivers = (from m in _db.ACM_ParentCaregiversAssessed
                                     where m.Sasat_Id == this.Sasat_Id
                                     select new SelectListItem()
                                     {
                                         Text = m.int_Person.First_Name + " " + m.int_Person.Last_Name + " [" + m.apl_Relationship_Type.Description + "]",
                                         Value = m.Person_Id.ToString(),
                                         Selected = m.Person_Id.Equals(SelectedEffectiveProblemsolving_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedEffectiveProblemsolving_Id);

                return selectList;
            }
        }

        public int SelectedParentGuardianCareGiver_Id { get; set; }
        public SelectList ParentGuardianCareGiver_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var clientId = this.ACM_SouthAfricanSafetyAssessmentTool.Client_Id;
                var client = _db.Clients.Where(x => x.Client_Id == clientId).FirstOrDefault();
                var test = client.Client_Biological_Parents;
                var test2 = client.Client_Adoptive_Parents;
                var test3 = client.Client_CareGivers;

                var aditionalCareGivers = new List<Person>();
                foreach (var biol in test)
                {
                    aditionalCareGivers.Add(biol.Person);
                }
                foreach (var adopt in test2)
                {
                    aditionalCareGivers.Add(adopt.Person);
                }
                foreach (var care in test3)
                {
                    aditionalCareGivers.Add(care.Person);
                }

                var allCaregivers = (from m in aditionalCareGivers
                                     select new SelectListItem()
                                     {
                                         Text = m.First_Name + " " + m.Last_Name,
                                         Value = m.Person_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = m.Person_Id.Equals(SelectedParentGuardianCareGiver_Id)
                                     }).ToList();

                var selectList = new SelectList(allCaregivers, "Value", "Text", SelectedParentGuardianCareGiver_Id);

                return selectList;
            }
        }
    }

    public partial class ACM_ChildrensCourtReport
    {
        public List<Client_Biological_Parent> BiologicalParents { get; set; }
        public List<ACM_InformationSources> InformationSources {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var informationSources = _db.ACM_InformationSources.Where(x => x.ChildrensCourtReport_Id == this.ChildrensCourtReport_Id).ToList();

                return informationSources;
            }
        }
    }


    public partial class ACM_CaseWorkList
    {
        public string ClientFullNames
        {
            get
            {
                var ClientName = this.int_Intake_Assessment.Client.Person.First_Name + " " + this.int_Intake_Assessment.Client.Person.Last_Name;

                return ClientName;
            }
        }

        public List<ACM_InterviewProcessNote> CaseProcessNotesList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();

                var processNotes = _db.ACM_InterviewProcessNote.Where(x => x.CaseWorklist_Id == CaseWoklist_Id && x.IsDeleted == false).ToList();

                return processNotes;
            }
        }

    }

    public partial class ACM_ActuarialRiskAssessment
    {
        public List<ACM_AraSourcesOfInformation> SourcesOfInformationList { get; set; }
        public List<Person> ParentsCaregives_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var aditionalCareGivers = new List<Person>();
                //var caseWorkList = _db.ACM_CaseWorkList.Where(x => x.CaseWoklist_Id == this.Caseworklist_Id).FirstOrDefault();

                //var biological = caseWorkList.int_Intake_Assessment.Client.Client_Biological_Parents;
                //var adoptive = caseWorkList.int_Intake_Assessment.Client.Client_Adoptive_Parents;
                //var caregivers = caseWorkList.int_Intake_Assessment.Client.Client_CareGivers;
                //var familyMembers = caseWorkList.int_Intake_Assessment.Client.Client_Family_Members;

                //aditionalCareGivers = new List<Person>();
                //foreach (var adopt in adoptive)
                //{
                //    aditionalCareGivers.Add(adopt.Person);
                //}
                //foreach (var care in caregivers)
                //{
                //    aditionalCareGivers.Add(care.Person);
                //}
                //foreach (var bio in biological)
                //{
                //    aditionalCareGivers.Add(bio.Person);
                //}

                


                return aditionalCareGivers;
            }
        }
        public ACM_ActuarialRiskAssessmentScoring Scoring { get; set; }

    }

    public partial class ACM_IDPParentsDetails
    {
        public List<ACM_IDPParentsDetails> ACMIDPParentsDetails_List { get; set; }
        public int SelectedRelationship_Type_Id { get; set; }
        public SelectList Relationship_Type_List
        {
            get
            {
                var RelationshipTypeModel = new RelationshipTypeModel();
                var listOfRelationshipTypes = RelationshipTypeModel.GetListOfRelationshipTypes();
                var RelationshipTypesList = (from r in listOfRelationshipTypes
                                             select new SelectListItem()
                                             {
                                                 Text = r.Description,
                                                 Value = r.Relationship_Type_Id.ToString(CultureInfo.InvariantCulture),
                                                 Selected = r.Relationship_Type_Id.Equals(SelectedRelationship_Type_Id)
                                             }).ToList();

                var selectList = new SelectList(RelationshipTypesList, "Value", "Text", SelectedRelationship_Type_Id);
                return selectList;
            }
        }
    }

    public partial class ACM_ChildDetailsRemovalAlreadyInAlternativeCare
    {
        //public int CaseWorklist_Id { get; set; }
        public int SelectedPlacementType_Id { get; set; }

        [Display(Name = "Type of Placement")]
        public SelectList PlacementType_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var placementtype = (from a in _db.ACM_PlacementType
                                     select a).ToList();

                var placementTypes = (from m in placementtype
                                      select new SelectListItem()
                                      {
                                          Text = m.Description,
                                          Value = m.PlacementType_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = m.PlacementType_Id.Equals(SelectedPlacementType_Id)
                                      }).ToList();

                var selectList = new SelectList(placementTypes, "Value", "Text", SelectedPlacementType_Id);

                return selectList;
            }
        }

        public int SelectedCourtOrderType_Id { get; set; }

        [Display(Name = "Type of Court Order")]
        public SelectList CourtOrderType_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var courtOrderType = (from a in _db.ACM_CourtOrderType
                                      select a).ToList();

                var courtOrderTypes = (from m in courtOrderType
                                       select new SelectListItem()
                                       {
                                           Text = m.Description,
                                           Value = m.CourtOrderType_Id.ToString(CultureInfo.InvariantCulture),
                                           Selected = m.CourtOrderType_Id.Equals(SelectedCourtOrderType_Id)
                                       }).ToList();

                var selectList = new SelectList(courtOrderTypes, "Value", "Text", SelectedCourtOrderType_Id);

                return selectList;
            }
        }

        public int SelectedTemporarySafeCare_Id { get; set; }

        public SelectList TemporarySafeCare_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var temporarySafeCare = (from a in _db.ACM_TemporarySafeCare
                                         select a).ToList();

                var temporarySafeCares = (from m in temporarySafeCare
                                          select new SelectListItem()
                                          {
                                              Text = m.Description,
                                              Value = m.TemporarySafeCare_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = m.TemporarySafeCare_Id.Equals(SelectedTemporarySafeCare_Id)
                                          }).ToList();

                var selectList = new SelectList(temporarySafeCares, "Value", "Text", SelectedTemporarySafeCare_Id);

                return selectList;
            }
        }

    }

    public partial class ACM_CourtOutcome
    {
        public List<Province> Province { get; set; }
        public List<Court> Court { get; set; }
        public List<District> District { get; set; }
        public List<ACM_CourtOutcomeStatus> ACMCourtOutcomeStatus { get; set; }
        public List<ACM_TypeOfCourtOutcome> ACMTypeOfCourtOutcome { get; set; }

        public int SelectedProvince_Id { get; set; }
        public int SelectedCourt_Id { get; set; }
        public int SelectedDistrict_Id { get; set; }
        public int SelectedCourtOutcomeStatus_Id { get; set; }
        public int SelectedTypeOfCourtOutcome_Id { get; set; }
        public int SelectedAlternativeCareType_Id { get; set; }
        public int SelectedPlacementSection_Id { get; set; }

        [Display(Name = "Province")]
        public SelectList Province_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfProvinces = (from a in _db.Provinces
                                       select a).ToList();

                var provinces = (from p in listOfProvinces
                                 select new SelectListItem()
                                 {
                                     Text = p.Description,
                                     Value = p.Province_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = p.Province_Id.Equals(SelectedProvince_Id)
                                 }).ToList();

                var selectList = new SelectList(provinces, "Value", "Text", SelectedProvince_Id);

                return selectList;
            }
        }

        [Display(Name = "Court")]
        public SelectList Court_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfCourts = (from a in _db.Courts
                                    select a).ToList();

                var courts = (from c in listOfCourts
                              select new SelectListItem()
                              {
                                  Text = c.Description,
                                  Value = c.Court_Id.ToString(CultureInfo.InvariantCulture),
                                  Selected = c.Court_Id.Equals(SelectedCourt_Id)
                              }).ToList();

                var selectList = new SelectList(courts, "Value", "Text", SelectedCourt_Id);
                return selectList;
            }
        }

        [Display(Name = "District")]
        public SelectList District_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfDistricts = (from a in _db.Districts
                                       select a).ToList();

                var districts = (from d in listOfDistricts
                                 select new SelectListItem()
                                 {
                                     Text = d.Description,
                                     Value = d.District_Id.ToString(CultureInfo.InvariantCulture),
                                     Selected = d.District_Id.Equals(SelectedDistrict_Id)
                                 }).ToList();

                var selectList = new SelectList(districts, "Value", "Text", SelectedDistrict_Id);
                return selectList;
            }
        }

        [Display(Name = "Court Outcome Status")]
        public SelectList ACMCourtOutcomeStatus_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfACMCourtOutcomeStatuses = (from a in _db.ACM_CourtOutcomeStatus
                                                     select a).ToList();

                var ACMCourtOutcomeStatuses = (from d in listOfACMCourtOutcomeStatuses
                                               select new SelectListItem()
                                               {
                                                   Text = d.Description,
                                                   Value = d.CourtOutcomeStatus_Id.ToString(CultureInfo.InvariantCulture),
                                                   Selected = d.CourtOutcomeStatus_Id.Equals(SelectedCourtOutcomeStatus_Id)
                                               }).ToList();

                var selectList = new SelectList(ACMCourtOutcomeStatuses, "Value", "Text", SelectedCourtOutcomeStatus_Id);
                return selectList;
            }
        }

        [Display(Name = "Type of Court Outcome")]
        public SelectList ACMTypeOfCourtOutcome_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listOfACMTypeOfCourtOutcome = (from a in _db.ACM_TypeOfCourtOutcome
                                                   select a).ToList();

                var ACMTypeOfCourtOutcomes = (from o in listOfACMTypeOfCourtOutcome
                                              select new SelectListItem()
                                              {
                                                  Text = o.Description,
                                                  Value = o.TypeOfCourtOutcome_Id.ToString(CultureInfo.InvariantCulture),
                                                  Selected = o.TypeOfCourtOutcome_Id.Equals(SelectedTypeOfCourtOutcome_Id)
                                              }).ToList();

                var selectList = new SelectList(ACMTypeOfCourtOutcomes, "Value", "Text", SelectedTypeOfCourtOutcome_Id);
                return selectList;
            }
        }

        [Display(Name = "Alternative Care Type")]
        public SelectList ACMAlternativeCareType_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listofacmalernativetypes = (from a in _db.ACM_AlternativeCareType
                                                select a).ToList();

                var acmalernativetypes = (from o in listofacmalernativetypes
                                          select new SelectListItem()
                                          {
                                              Text = o.Description,
                                              Value = o.AlternativeCareType_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = o.AlternativeCareType_Id.Equals(SelectedAlternativeCareType_Id)
                                          }).ToList();

                var selectList = new SelectList(acmalernativetypes, "Value", "Text", SelectedAlternativeCareType_Id);
                return selectList;
            }
        }

        [Display(Name = "Placement Section")]
        public SelectList ACMPlacementSection_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var listofacmplacementsections = (from a in _db.ACM_PlacementSection
                                                  select a).ToList();

                var acmplacementsections = (from o in listofacmplacementsections
                                            select new SelectListItem()
                                            {
                                                Text = o.Description,
                                                Value = o.PlacementSection_Id.ToString(CultureInfo.InvariantCulture),
                                                Selected = o.PlacementSection_Id.Equals(SelectedPlacementSection_Id)
                                            }).ToList();

                var selectList = new SelectList(acmplacementsections, "Value", "Text", SelectedPlacementSection_Id);
                return selectList;
            }
        }
    }

    public partial class ACM_FosterCareViewsOfChildren
    {

        public int SelectedIsViewsOfChildren_Id { get; set; }
        public SelectList IsViewsOfChildren_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isviewsofchildrenlist = (from a in _db.ACM_YesNoOption
                                             select a).ToList();

                var IsViewsOfChildren = (from m in isviewsofchildrenlist
                                         select new SelectListItem()
                                         {
                                             Text = m.Description,
                                             Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = m.YesNoOption_Id.Equals(SelectedIsViewsOfChildren_Id)
                                         }).ToList();

                var selectList = new SelectList(IsViewsOfChildren, "Value", "Text", SelectedIsViewsOfChildren_Id);
                return selectList;
            }
        }
    }

    public partial class ACM_Attachment
    {
        public HttpPostedFileBase file { get; set; }
        public int CaseWoklist_Id { get; set; }
        public List<AttachmentViewModel> CurrentAttachments { get; set; }
        public int SelectedAttachementType_Id { get; set; }
        public SelectList AttachmentType_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var attachementtypelist = (from a in _db.ACM_TypeOfDocument
                                           select a).ToList();

                var attachementtypes = (from m in attachementtypelist
                                        select new SelectListItem()
                                        {
                                            Text = m.Description,
                                            Value = m.TypeOfDocument_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = m.TypeOfDocument_Id.Equals(SelectedAttachementType_Id)
                                        }).ToList();

                var selectList = new SelectList(attachementtypes, "Value", "Text", SelectedAttachementType_Id);
                return selectList;
            }
        }

        public List<AttachmentViewModel> CurrentCaseAttatchments
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var ccc = _db.ACM_Attachment.Join(
                    _db.ACM_TypeOfDocument, c => c.TypeOfDocument_Id, t => t.TypeOfDocument_Id, (c, t) => 
                    new { c.DocumentName, t.Description, c.Document_Url, c.CaseWorklist_Id, c.Attachment_Id });

                var aaa = ccc.Where(x => x.CaseWorklist_Id == this.CaseWorklist_Id).ToList();

                var attachmentList = (from a in aaa
                                      select new AttachmentViewModel
                                      {
                                          DocumentName = a.DocumentName,
                                          DocumentType = a.Description,
                                          AttachmentId = a.Attachment_Id,
                                          AttachmentUrl = a.Document_Url
                                      }).ToList();


                //var attachmentList = (from a in _db.ACM_Attachment
                //                      join b in _db.ACM_TypeOfDocument on a.TypeOfDocument_Id equals b.TypeOfDocument_Id
                //                      select new AttachmentViewModel
                //                      {
                //                          DocumentName = a.DocumentName,
                //                          DocumentType = b.Description,
                //                          AttachmentId = a.Attachment_Id,
                //                          AttachmentUrl = a.Document_Url
                //                      }).ToList();

                if (attachmentList == null)
                {
                    attachmentList = new List<AttachmentViewModel>();
                }

                return attachmentList;
            }
        }
    }


    public partial class ACM_ParentsGuardiansAssentingToSurgicalOperation
    {
        public List<ACM_ParentsGuardiansAssentingToSurgicalOperation> ParentsGuardiansAssenting { get; set; }
    }

    public partial class ACM_ParentAgedBelowEighteenYearsGivingConsent
    {
        public List<ACM_ParentAgedBelowEighteenYearsGivingConsent> ParentsAgedBelow18YearsGivingConsent { get; set; }
    }


    public partial class ACM_ConsentToSurgicalOperationByAChild
    {
        public int SelectedParentGuardian_Id { get; set; }

        public SelectList ParentGuardian_List
        {
            get
            {

                var _db = new SDIIS_DatabaseEntities();

                var caseWorklist = _db.ACM_CaseWorkList.Where(x => x.CaseWoklist_Id == this.CaseWorklist_Id).FirstOrDefault();

                //var biological = this.ACM_CaseWorkList.int_Intake_Assessment.Client.Client_Biological_Parents;
                //var adoptive = this.ACM_CaseWorkList.int_Intake_Assessment.Client.Client_Adoptive_Parents;

                var biological = caseWorklist.int_Intake_Assessment.Client.Client_Biological_Parents;
                var adoptive = caseWorklist.int_Intake_Assessment.Client.Client_Adoptive_Parents;

                var ParentsGuardians = new List<Person>();
                foreach (var biol in biological)
                {
                    ParentsGuardians.Add(biol.Person);
                }

                foreach (var adopt in adoptive) //Guardian
                {
                    ParentsGuardians.Add(adopt.Person);
                }

                var allParentsGuardians = (from m in ParentsGuardians
                                           select new SelectListItem()
                                           {
                                               Text = m.First_Name + " " + m.Last_Name,
                                               Value = m.Person_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = m.Person_Id.Equals(SelectedParentGuardian_Id)
                                           }).ToList();
                var selectList = new SelectList(allParentsGuardians, "Value", "Text", SelectedParentGuardian_Id);
                return selectList;
            }
        }
    }

    public partial class ACM_CarePlanReviewCaregiverDetails
    {

        public List<ACM_CarePlanReviewPersonSeen> CarePlanReviewReviewedList { get; set; }

    }


    public partial class ACM_SummaryOfDevelopmentArea
    {
        public List<ACMSummaryOfDevelopmentAreaModel> SummaryDevelopmentalAreas { get; set; }


        [Required(ErrorMessage = "Please make a selection")]
        [Display(Name = "Developmental Area")]
        public int SelectedSummaryDevelopmentArea_Id { get; set; }

        public SelectList SummaryDevelopmentArea_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var SummaryDevelopmentArealist = (from a in _db.ACM_IDPDevelopmentArea
                                                  select a).ToList();

                var SummaryDevelopmentAreas = (from m in SummaryDevelopmentArealist
                                               select new SelectListItem()
                                               {
                                                   Text = m.Description,
                                                   Value = m.IDPDevelopmentArea_Id.ToString(CultureInfo.InvariantCulture),
                                                   Selected = m.IDPDevelopmentArea_Id.Equals(SelectedSummaryDevelopmentArea_Id)
                                               }).ToList();

                var selectList = new SelectList(SummaryDevelopmentAreas, "Value", "Text", SelectedSummaryDevelopmentArea_Id);
                return selectList;
            }
        }
    }


    public partial class ACM_PlacementSupervisionAndAfterCare
    {
        public List<Person> ClientList { get; set; }
    }

    public partial class ACM_IndividualDevelopmentPlan
    {
         //public ACM_IndividualDevelopmentPlan ACMIndividualDevelopmentPlan { get; set; }
	        public List<ACM_IDPWellbeing> IDPWellBeingList { get; set; }
	        public List<ACM_IDPDevelopmentalAreaBeloning> IDPBelongList { get; set; }
	        public List<ACM_IDPDevelopmentalAreaMastery> IDPMasteryList { get; set; }
	        public List<ACM_IDPDevelopmentalAreaIndependence> IDPIndependenceList { get; set; }
	        public List<ACM_IDPDevelopmentalAreaGenerosity> IDPGenerosityList { get; set; }
	
	        public List<ACM_IDPConsultedPeople> ConsultedPeopleList
	        {
	            get
	            {
	                var _db = new SDIIS_DatabaseEntities();
	                var consultedPersons = _db.ACM_IDPConsultedPeople.Where(x => x.IndivisualDevelopmentPlan_Id == this.IndividualDevelopmentPlan_Id).ToList();
	                if (consultedPersons.Count == 0)
	                {
	                    consultedPersons = new List<Models.ACM_IDPConsultedPeople>();
	                }
	                return consultedPersons;
	            }
	        }
	
	        public List<ACM_IDPMeetingPersonPresence> PersonPresenceList
	        {
	            get
	            {
	                var _db = new SDIIS_DatabaseEntities();
	                var personPresence = _db.ACM_IDPMeetingPersonPresence.Where(x => x.IndividualDevelopmentPlan_Id == this.IndividualDevelopmentPlan_Id).ToList();
	                if (personPresence.Count == 0)
	                {
	                    personPresence = new List<Models.ACM_IDPMeetingPersonPresence>();
	                }
	                return personPresence;
	            }
	        }
	
	        public List<ACM_IDPPeopleWhoHadParentalResponsibilities> HadParentalResponsibilitiesList
	        {
	            get
	            {
	                var _db = new SDIIS_DatabaseEntities();
	                var HadParentalResponsibilities = _db.ACM_IDPPeopleWhoHadParentalResponsibilities.Where(x => x.IndividualDevelopmentPlan_Id == this.IndividualDevelopmentPlan_Id).ToList();
	                if (HadParentalResponsibilities.Count == 0)
	                {
	                    HadParentalResponsibilities = new List<Models.ACM_IDPPeopleWhoHadParentalResponsibilities>();
	                }
	                return HadParentalResponsibilities;
	            }
	        }

    }

  public partial class ACM_FosterCareBiologicalParentsOrGuardians
    {

        public List<ACM_FosterCareBiologicalParentsOrGuardians> BiologicalParentsOrGuardianList
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var BiologicalParentsOrGuardian = _db.ACM_FosterCareBiologicalParentsOrGuardians.Where(x => x.CaseWorklist_Id == this.CaseWorklist_Id).ToList();
                if (BiologicalParentsOrGuardian.Count == 0)
                {
                    BiologicalParentsOrGuardian = new List<Models.ACM_FosterCareBiologicalParentsOrGuardians>();
                }
                return BiologicalParentsOrGuardian;
            }
        }
    }
    
    public partial class Client_Foster_Parent
        {
            public List<Client_Foster_Parent> ClientFosterParentList
            {
                get
                {
                    var _db = new SDIIS_DatabaseEntities();
                    var ClientFosterParent = _db.Client_Foster_Parents.Where(x => x.Client_Id == this.Client_Id).ToList();
                    if (ClientFosterParent.Count == 0)
                    {
                        ClientFosterParent = new List<Models.Client_Foster_Parent>();
                    }
                    return ClientFosterParent;
                }
            }
        }
    
        public partial class Client_Biological_Parent
        {
            public List<Client_Biological_Parent> ClientBiologicalParentList
            {
                get
                {
                    var _db = new SDIIS_DatabaseEntities();
                    var ClientBiologicalParent = _db.Client_Biological_Parents.Where(x => x.Client_Id == this.Client_Id).ToList();
                    if (ClientBiologicalParent == null)
                    {
                        ClientBiologicalParent = new List<Models.Client_Biological_Parent>();
                    }
                    return ClientBiologicalParent;
                }
            }
        }

    
    public partial class ACM_FosterParent
        {
            public List<ACM_FosterParent> FosterParentList
            {
                get
                {
                    var _db = new SDIIS_DatabaseEntities();
                    var FosterParent = _db.ACM_FosterParent.Where(x => x.CaseWorklist_Id == this.CaseWorklist_Id).ToList();
                    if (FosterParent.Count == 0)
                    {
                        FosterParent = new List<Models.ACM_FosterParent>();
                    }
                    return FosterParent;
                }
            }
    }
    
    public partial class ACM_IDPDevelopmentalAreaMastery
    {
        public int SelectedSchoolLastAttended_Id { get; set; }
        public SelectList SchoolLastAttended_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var schoollastattendedlist = (from a in _db.Schools
                                              select a).ToList();

                var SchoolLastAttended = (from m in schoollastattendedlist
                                          select new SelectListItem()
                                          {
                                              Text = m.School_Name,
                                              Value = m.School_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = m.School_Id.Equals(SelectedSchoolLastAttended_Id)
                                          }).ToList();

                var selectList = new SelectList(SchoolLastAttended, "Value", "Text", SelectedSchoolLastAttended_Id);
                return selectList;
            }
        }

        public int SelectedGradeChildWasIn_Id { get; set; }
        public SelectList IsGradeChildWasIn_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var gradeschildwasinlist = (from a in _db.Grades
                                            select a).ToList();

                var GradeTheChildWasIn = (from m in gradeschildwasinlist
                                          select new SelectListItem()
                                          {
                                              Text = m.Description,
                                              Value = m.Grade_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = m.Grade_Id.Equals(SelectedGradeChildWasIn_Id)
                                          }).ToList();

                var selectList = new SelectList(GradeTheChildWasIn, "Value", "Text", SelectedGradeChildWasIn_Id);
                return selectList;
            }
        }

        public int SelectedWasChildPerformanceGood_Id { get; set; }
        public SelectList WasChildPerformanceGood_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var waschildformancegoodlist = (from a in _db.ACM_YesNoOption
                                                select a).ToList();

                var WasChildPerformancesGood = (from m in waschildformancegoodlist
                                                select new SelectListItem()
                                                {
                                                    Text = m.Description,
                                                    Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                    Selected = m.YesNoOption_Id.Equals(SelectedWasChildPerformanceGood_Id)
                                                }).ToList();

                var selectList = new SelectList(WasChildPerformancesGood, "Value", "Text", SelectedWasChildPerformanceGood_Id);
                return selectList;
            }
        }

        public int SelectedWasPositiveRelationships_Id { get; set; }
        public SelectList WasPositiveRelationships_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var waspositiverelationshiplist = (from a in _db.ACM_YesNoOption
                                                   select a).ToList();

                var WasPositiveRelationship = (from m in waspositiverelationshiplist
                                               select new SelectListItem()
                                               {
                                                   Text = m.Description,
                                                   Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                   Selected = m.YesNoOption_Id.Equals(SelectedWasPositiveRelationships_Id)
                                               }).ToList();

                var selectList = new SelectList(WasPositiveRelationship, "Value", "Text", SelectedWasPositiveRelationships_Id);
                return selectList;
            }
        }

        public int SelectedIsChildCurrentlyAttending_Id { get; set; }
        public SelectList IsChildCurrentlyAttending_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var ischildcurrentlyattendinglist = (from a in _db.ACM_SchoolType
                                                     select a).ToList();

                var IsChildCurrentlyAttending = (from m in ischildcurrentlyattendinglist
                                                 select new SelectListItem()
                                                 {
                                                     Text = m.Description,
                                                     Value = m.SchoolType_Id.ToString(CultureInfo.InvariantCulture),
                                                     Selected = m.SchoolType_Id.Equals(SelectedIsChildCurrentlyAttending_Id)
                                                 }).ToList();

                var selectList = new SelectList(IsChildCurrentlyAttending, "Value", "Text", SelectedIsChildCurrentlyAttending_Id);
                return selectList;
            }
        }

        public int SelectedChildLikeBeingInThatSchool_Id { get; set; }
        public SelectList ChildLikeBeingInThatSchool_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var childlikebeinginthatschoollist = (from a in _db.ACM_YesNoOption
                                                      select a).ToList();

                var ChildLikeBeingInThatSchool = (from m in childlikebeinginthatschoollist
                                                  select new SelectListItem()
                                                  {
                                                      Text = m.Description,
                                                      Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                      Selected = m.YesNoOption_Id.Equals(SelectedChildLikeBeingInThatSchool_Id)
                                                  }).ToList();

                var selectList = new SelectList(ChildLikeBeingInThatSchool, "Value", "Text", SelectedChildLikeBeingInThatSchool_Id);
                return selectList;
            }
        }

        public int SelectedDoesChildAttendSchoolRegularly_Id { get; set; }
        public SelectList DoesChildAttendSchoolRegularly_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var doeschildsttendschoolregularlylist = (from a in _db.ACM_YesNoOption
                                                          select a).ToList();

                var DoesChildAttendSchoolRegularly = (from m in doeschildsttendschoolregularlylist
                                                      select new SelectListItem()
                                                      {
                                                          Text = m.Description,
                                                          Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                          Selected = m.YesNoOption_Id.Equals(SelectedDoesChildAttendSchoolRegularly_Id)
                                                      }).ToList();

                var selectList = new SelectList(DoesChildAttendSchoolRegularly, "Value", "Text", SelectedDoesChildAttendSchoolRegularly_Id);
                return selectList;
            }
        }

        public int SelectedDoesChildHaveLearningDisabilities_Id { get; set; }
        public SelectList DoesChildHaveLearningDisabilities_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var doeschildhaveLearningdisabilitieslist = (from a in _db.ACM_YesNoOption
                                                             select a).ToList();

                var DoesChildHaveLearningDisabilities = (from m in doeschildhaveLearningdisabilitieslist
                                                         select new SelectListItem()
                                                         {
                                                             Text = m.Description,
                                                             Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                             Selected = m.YesNoOption_Id.Equals(SelectedDoesChildHaveLearningDisabilities_Id)
                                                         }).ToList();

                var selectList = new SelectList(DoesChildHaveLearningDisabilities, "Value", "Text", SelectedDoesChildHaveLearningDisabilities_Id);
                return selectList;
            }
        }

        public int SelectedDoesChildSubmitHomeWorkOnTime_Id { get; set; }
        public SelectList DoesChildSubmitHomeWorkOnTime_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var DoeschildSubmithomeworkonimelist = (from a in _db.ACM_YesNoOption
                                                        select a).ToList();

                var DoesChildSubmitHomeWorkOnTime = (from m in DoeschildSubmithomeworkonimelist
                                                     select new SelectListItem()
                                                     {
                                                         Text = m.Description,
                                                         Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                         Selected = m.YesNoOption_Id.Equals(SelectedDoesChildSubmitHomeWorkOnTime_Id)
                                                     }).ToList();

                var selectList = new SelectList(DoesChildSubmitHomeWorkOnTime, "Value", "Text", SelectedDoesChildSubmitHomeWorkOnTime_Id);
                return selectList;
            }
        }

        public int SelectedGradeChildIsIn_Id { get; set; }
        public SelectList GradeChildIsIn_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var GradeChildIsInlist = (from a in _db.Grades
                                          select a).ToList();

                var GradeChildIsIn = (from m in GradeChildIsInlist
                                      select new SelectListItem()
                                      {
                                          Text = m.Description,
                                          Value = m.Grade_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = m.Grade_Id.Equals(SelectedGradeChildIsIn_Id)
                                      }).ToList();

                var selectList = new SelectList(GradeChildIsIn, "Value", "Text", SelectedGradeChildIsIn_Id);
                return selectList;
            }
        }

        public int SelectedIsAssessedByEducationalPsychologist_Id { get; set; }
        public SelectList IsAssessedByEducationalPsychologist_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isAssessedbyeducationalpsychologistlist = (from a in _db.ACM_YesNoOption
                                                               select a).ToList();

                var IsAssessedByEducationalPsychologist = (from m in isAssessedbyeducationalpsychologistlist
                                                           select new SelectListItem()
                                                           {
                                                               Text = m.Description,
                                                               Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                               Selected = m.YesNoOption_Id.Equals(SelectedIsAssessedByEducationalPsychologist_Id)
                                                           }).ToList();

                var selectList = new SelectList(IsAssessedByEducationalPsychologist, "Value", "Text", SelectedIsAssessedByEducationalPsychologist_Id);
                return selectList;
            }
        }

        public int selectedIsChildOnMedication_Id { get; set; }
        public SelectList IsChildOnMedication_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var Ischildonmedicationlist = (from a in _db.ACM_YesNoOption
                                               select a).ToList();

                var IsChildOnMedication = (from m in Ischildonmedicationlist
                                           select new SelectListItem()
                                           {
                                               Text = m.Description,
                                               Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                               Selected = m.YesNoOption_Id.Equals(selectedIsChildOnMedication_Id)
                                           }).ToList();

                var selectList = new SelectList(IsChildOnMedication, "Value", "Text", selectedIsChildOnMedication_Id);
                return selectList;
            }
        }

    }


    public partial class ACM_IDPDevelopmentalAreaBeloning
    {

        public int SelectedIsBirthCertificate_Id { get; set; }
        public SelectList IsBirthCertificate_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isbirthcertificatelist = (from a in _db.ACM_YesNoOption
                                              select a).ToList();

                var IsBirthCertificate = (from m in isbirthcertificatelist
                                          select new SelectListItem()
                                          {
                                              Text = m.Description,
                                              Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = m.YesNoOption_Id.Equals(SelectedIsBirthCertificate_Id)
                                          }).ToList();

                var selectList = new SelectList(IsBirthCertificate, "Value", "Text", SelectedIsBirthCertificate_Id);
                return selectList;
            }
        }



        public int SelectedIsId_Id { get; set; }
        public SelectList IsId_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isids = (from a in _db.ACM_YesNoOption
                             select a).ToList();

                var IsSignsOfAbuses = (from m in isids
                                       select new SelectListItem()
                                       {
                                           Text = m.Description,
                                           Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                           Selected = m.YesNoOption_Id.Equals(SelectedIsId_Id)
                                       }).ToList();

                var selectList = new SelectList(IsSignsOfAbuses, "Value", "Text", SelectedIsId_Id);
                return selectList;
            }
        }


    }


    public partial class ACM_IDPWellbeing
    {
        public int SelectedBasicHearingAndEyesightTest_Id { get; set; }
        public SelectList IsBasicHearingAndEyesightTest_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isbasichearingandayesighttestlist = (from a in _db.ACM_YesNoOption
                                                         select a).ToList();

                var isBasicHearingAndayEsighttest = (from m in isbasichearingandayesighttestlist
                                                     select new SelectListItem()
                                                     {
                                                         Text = m.Description,
                                                         Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                                         Selected = m.YesNoOption_Id.Equals(SelectedBasicHearingAndEyesightTest_Id)
                                                     }).ToList();

                var selectList = new SelectList(isBasicHearingAndayEsighttest, "Value", "Text", SelectedBasicHearingAndEyesightTest_Id);
                return selectList;
            }
        }

        public int SelectedIsRoadToHealthCard_Id { get; set; }
        public SelectList IsRoadToHealthCard_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isroadtohealthcard = (from a in _db.ACM_YesNoOption
                                          select a).ToList();

                var RoadToHealthCards = (from m in isroadtohealthcard
                                         select new SelectListItem()
                                         {
                                             Text = m.Description,
                                             Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = m.YesNoOption_Id.Equals(SelectedIsRoadToHealthCard_Id)
                                         }).ToList();

                var selectList = new SelectList(RoadToHealthCards, "Value", "Text", SelectedIsRoadToHealthCard_Id);
                return selectList;
            }
        }

        public int SelectedIsSignsOfAbuse_Id { get; set; }
        public SelectList IsSignsOfAbuse_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var issignsofabuse = (from a in _db.ACM_YesNoOption
                                      select a).ToList();

                var IsSignsOfAbuses = (from m in issignsofabuse
                                       select new SelectListItem()
                                       {
                                           Text = m.Description,
                                           Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                           Selected = m.YesNoOption_Id.Equals(SelectedIsSignsOfAbuse_Id)
                                       }).ToList();

                var selectList = new SelectList(IsSignsOfAbuses, "Value", "Text", SelectedIsSignsOfAbuse_Id);
                return selectList;
            }
        }

        public int SelectedIsChronicIllness_Id { get; set; }
        public SelectList IsChronicIllness_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var ischronicillness = (from a in _db.ACM_YesNoOption
                                        select a).ToList();

                var IsChronicIllnesses = (from m in ischronicillness
                                          select new SelectListItem()
                                          {
                                              Text = m.Description,
                                              Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                              Selected = m.YesNoOption_Id.Equals(SelectedIsChronicIllness_Id)
                                          }).ToList();

                var selectList = new SelectList(IsChronicIllnesses, "Value", "Text", SelectedIsChronicIllness_Id);
                return selectList;
            }
        }

        public int SelectedIsAwareOfHiv_Id { get; set; }
        public SelectList IsAwareOfHiv_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isawareofhiv = (from a in _db.ACM_YesNoOption
                                    select a).ToList();

                var IsAwareOfHivs = (from m in isawareofhiv
                                     select new SelectListItem()
                                     {
                                         Text = m.Description,
                                         Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                         Selected = m.YesNoOption_Id.Equals(SelectedIsAwareOfHiv_Id)
                                     }).ToList();

                var selectList = new SelectList(IsAwareOfHivs, "Value", "Text", SelectedIsAwareOfHiv_Id);
                return selectList;
            }
        }

        public int SelectedIsAwareOfChronic_Id { get; set; }
        public SelectList IsAwareOfChronic_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isawareofchronic = (from a in _db.ACM_YesNoOption
                                        select a).ToList();

                var IsAwareOfChronics = (from m in isawareofchronic
                                         select new SelectListItem()
                                         {
                                             Text = m.Description,
                                             Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = m.YesNoOption_Id.Equals(SelectedIsAwareOfChronic_Id)
                                         }).ToList();

                var selectList = new SelectList(IsAwareOfChronics, "Value", "Text", SelectedIsAwareOfChronic_Id);
                return selectList;
            }
        }

        public int SelectedIsAcuteIllness_Id { get; set; }
        public SelectList IsAcuteIllness_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isacuteillness = (from a in _db.ACM_YesNoOption
                                      select a).ToList();

                var IsAcuteIllnesses = (from m in isacuteillness
                                        select new SelectListItem()
                                        {
                                            Text = m.Description,
                                            Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = m.YesNoOption_Id.Equals(SelectedIsAcuteIllness_Id)
                                        }).ToList();

                var selectList = new SelectList(IsAcuteIllnesses, "Value", "Text", SelectedIsAcuteIllness_Id);
                return selectList;
            }
        }

        public int SelectedIsAcuteResponded_Id { get; set; }
        public SelectList IsAcuteResponded_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isacuteresponded = (from a in _db.ACM_YesNoOption
                                        select a).ToList();

                var IsAcutesResponded = (from m in isacuteresponded
                                         select new SelectListItem()
                                         {
                                             Text = m.Description,
                                             Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = m.YesNoOption_Id.Equals(SelectedIsAcuteResponded_Id)
                                         }).ToList();

                var selectList = new SelectList(IsAcutesResponded, "Value", "Text", SelectedIsAcuteResponded_Id);
                return selectList;
            }
        }

        public int SelectedIsDisability_Id { get; set; }
        public SelectList IsDisability_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isdisability = (from a in _db.ACM_YesNoOption
                                    select a).ToList();

                var IsDisabilities = (from m in isdisability
                                      select new SelectListItem()
                                      {
                                          Text = m.Description,
                                          Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = m.YesNoOption_Id.Equals(SelectedIsDisability_Id)
                                      }).ToList();

                var selectList = new SelectList(IsDisabilities, "Value", "Text", SelectedIsDisability_Id);
                return selectList;
            }
        }

        public int SelectedIsAssistiveDevices_Id { get; set; }
        public SelectList IsAssistiveDevices_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var isassistivedevices = (from a in _db.ACM_YesNoOption
                                          select a).ToList();

                var AssistiveDevices = (from m in isassistivedevices
                                        select new SelectListItem()
                                        {
                                            Text = m.Description,
                                            Value = m.YesNoOption_Id.ToString(CultureInfo.InvariantCulture),
                                            Selected = m.YesNoOption_Id.Equals(SelectedIsAssistiveDevices_Id)
                                        }).ToList();

                var selectList = new SelectList(AssistiveDevices, "Value", "Text", SelectedIsAssistiveDevices_Id);
                return selectList;
            }
        }
    }//End of ACM_IDPWellbeing partial class



    public partial class ACM_LeaveOfAbsence
    {
        public int SelectedPlacementType_Id { get; set; }

        [Display(Name = "Type of Placement")]
        public SelectList PlacementType_List
        {
            get
            {
                var _db = new SDIIS_DatabaseEntities();
                var placementtype = (from a in _db.ACM_PlacementType
                                     select a).ToList();

                var placementTypes = (from m in placementtype
                                      select new SelectListItem()
                                      {
                                          Text = m.Description,
                                          Value = m.PlacementType_Id.ToString(CultureInfo.InvariantCulture),
                                          Selected = m.PlacementType_Id.Equals(SelectedPlacementType_Id)
                                      }).ToList();

                var selectList = new SelectList(placementTypes, "Value", "Text", SelectedPlacementType_Id);

                return selectList;
            }
        }
    }

    #endregion

    #region ADOPTIONS
    public partial class AdoptCaseGridMain
    {
        public int ClientId { get; set; }
        public int Adopt_Record_Status_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public int AssessmentCount { get; set; }
        public int CaseCount { get; set; }
        public DateTime IntakeAssessmentDate { get; set; }
        //public int ClientId { get; set; }
        public int IntakeAssessmentId { get; set; }
        //public DateTime IntakeAssessmentDate { get; set; }
        public int? CaseId { get; set; }
        public DateTime? CaseDate { get; set; }
        public string PCMReferenceNumber { get; set; }
        public string DetensionPlace { get; set; }
        public string RecordStatusDescription { get; set; }
        public string DescriptionGender { get; set; }
        public string CourtDescription { get; set; }
        public string Description { get; set; }
        public string DistrictDescription { get; set; }
        public string DescriptionPopulation_Group { get; set; }
        public string AdoptionReasonDescription { get; set; }
        public string DescriptionAdoption_Type { get; set; }
        public int? Population_Group_Id { get; set; }
        public int? Gender_Id { get; set; }
        public int? Province_Id { get; set; }
        public int Court_Id { get; set; }
        public int District_Id { get; set; }
        public int? Adopt_Type_Id { get; set; }
        public DateTime? Date_Registered { get; set; }
        public TimeSpan? Date_Of_Birth { get; set; }


        public string VEpRecordStatusDescription { get; set; }
        public List<AdoptinsCaseGridNested> NestedItems { get; set; }

        public virtual ICollection<apl_Adoption_Record_Status> apl_Adoption_Record_Status { get; set; }
    }


    public partial class AdoptinsCaseGridNested
    {
        public int ClientId { get; set; }
        public int IntakeAssessmentId { get; set; }
        public DateTime IntakeAssessmentDate { get; set; }
        public int? CaseId { get; set; }
        public DateTime? CaseDate { get; set; }
        public string PCMReferenceNumber { get; set; }
        public string DetensionPlace { get; set; }

    }


    public partial class AdoptionWorkload
    {


        public virtual ICollection<apl_Adoption_Record_Status> apl_Adoption_Record_Status { get; set; }

        public virtual ICollection<apl_VEP_Record_Status> apl_VEP_Record_Status { get; set; }

        public virtual Intake_Assessment int_Intake_Assessment { get; set; }

        public List<AdoptionWorkloadList> Adoptionlist { get; set; }
    }

    public partial class AdoptionWorkloadList
    {


        public int Adopt_CaseWoklist_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Allocated_By { get; set; }
        public int? Allocate_To { get; set; }
        public DateTime? Date_Allocated { get; set; }
        public string Reference_Number { get; set; }
        public int? Adopt_Record_Status_Id { get; set; }
        public string RecordStatusDescription { get; set; }
        public Nullable<System.DateTime> Date_Acknowledged { get; set; }

        public string VEpRecordStatusDescription { get; set; }
        public Nullable<System.DateTime> Date_Accepted { get; set; }

    }



    #endregion

    #region PCM
    public partial class PCMCaseGridMain
    {
        public List<PCMCaseGridMain> PCMallassessment { get; set; }
        public int ClientId { get; set; }

        public int? Person_Id { get; set; }

        public int? Age { get; set; }

        public string Phone_Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public int AssessmentCount { get; set; }
        public int CaseCount { get; set; }
        public int IntakeAssessmentId { get; set; }
        public string RecordStatusDescription { get; set; }
        public List<PCMCaseGridNested> PCMNestedItems { get; set; }
        public string PCMReferenceNumber { get; set; }
        public DateTime? Assessment_Date { get; set; }

        public int Assesment_Register_Id { get; set; }

        public int Probation_Officer_Id { get; set; }

        public string POName { get; set; }

        public int PersonId { get; set; }

    }

    public partial class PCMCaseGridNested
    {

        public string RecordStatusDescription { get; set; }
        public string POName { get; set; }
        public int AssessmentId { get; set; }
        public int? PersonId { get; set; }
        public DateTime? AssessmentDate { get; set; }

    }
    #endregion


    #region RACAP
    public partial class RACAPCaseGridMain
    {
        public int? ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public int? RACAP_Prospective_Parent_Id { get; set; }
        public int? RACAP_Adoptive_Child_Id { get; set; }
        public string SocialWorkerName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string RefNumber { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public int AssessmentCount { get; set; }
        public int CaseCount { get; set; }
        public DateTime IntakeAssessmentDate { get; set; }
        //public int ClientId { get; set; }
        public int? IntakeAssessmentId { get; set; }
        //public DateTime IntakeAssessmentDate { get; set; }

        public string RACAPRecordStatusDescription { get; set; }
        public string RecordStatusDescription { get; set; }
        
        public int? CaseId { get; set; }
        public int? Problem_Sub_Category_Id { get; set; }
        public DateTime? CaseDate { get; set; }

        public DateTime? DateAllocated { get; set; }
        public string PCMReferenceNumber { get; set; }
        public string DetensionPlace { get; set; }
        public List<RACAPCaseGridNested> NestedItems { get; set; }
    }

    public partial class RACAPCaseGridNested
    {
        public int ClientId { get; set; }
        public int IntakeAssessmentId { get; set; }
        public DateTime IntakeAssessmentDate { get; set; }
        public int? CaseId { get; set; }
        public DateTime? CaseDate { get; set; }
        public string PCMReferenceNumber { get; set; }
        public string DetensionPlace { get; set; }

    }

    public partial class RACAPWorkload
    {


        public virtual ICollection<apl_RACAP_Record_Status> apl_RACAP_Record_Status { get; set; }

        public virtual Intake_Assessment int_Intake_Assessment { get; set; }

        public List<RACAPWorkloadList> RACAPlist { get; set; }
    }

    public partial class RACAPWorkloadList
    {


        public int RACAP_CaseWoklist_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Allocated_By { get; set; }
        public int? Allocate_To { get; set; }
        public DateTime? Date_Allocated { get; set; }
        public string Reference_Number { get; set; }
        public int? RACAP_Record_Status_Id { get; set; }
        public string RecordStatusDescription { get; set; }
        public Nullable<System.DateTime> Date_Accepted { get; set; }

    }

    #endregion


    #region VEP_Reporting_Clases
    //VEP National Stats partial class
    public partial class NationalStatsGrid
    {
        public string provinceName { get; set; }
        public string siteCode { get; set; }
        public string district { get; set; }
        public string vepCase { get; set; }
        public string assessment { get; set; }
        public string user { get; set; }
        public string employee { get; set; }
        public string caseId { get; set; }
        public int siteId { get; set; }
        public int totalDistricts { get; set; }
        public string LocalMunicipality { get; set; }
        public int totalLocalMunicipality { get; set; }
        public int totalSites { get; set; }
        public int serviceId { get; set; }
        public string service { get; set; }
        public int totalVictimsCaptured { get; set; }
        public int totalServicesRendered { get; set; }
        public int totalMales { get; set; }
        public int totalFemales { get; set; }
        public int totalHeterosexual { get; set; }
        public int totalLGBTIQsexual { get; set; }
    }

    //VEP Provicial Stats
    public partial class DistrictStatsGrid
    {
        public int districtId { get; set; }

        public string districtName { get; set; }
        public int totalDistricts { get; set; }
        public string LocalMunicipality { get; set; }
        public int totalLocalMunicipality { get; set; }
        public int totalSites { get; set; }
        public int serviceId { get; set; }
        public string service { get; set; }
        public int totalVictimsCaptured { get; set; }
        public int totalServicesRendered { get; set; }
    }



    public partial class LocalMunicipalityDataReport
    {
        public string RefNo { get; set; }

        public string ProvinceName { get; set; }

        public string DistrictName { get; set; }

        public string LocalMunicipalityName { get; set; }

        public string VictimisationType { get; set; }

        public string SiteCode { get; set; }

        public string PopulationGroup { get; set; }

        public string Gender { get; set; }

        public string SexualOrientation { get; set; }

        public string MaritalStatus { get; set; }

        public int? Age { get; set; }

        public string DisabilityType { get; set; }

        public string Citizenship { get; set; }

        public string Settlement { get; set; }

        public string KnownPerpetrator { get; set; }

        public string PerpFamilyMember { get; set; }

        public string PerpCommunityMember { get; set; }

        public string ReportedToPolice { get; set; }

        public string DateOfIncident { get; set; }

        public string DateOfReporting { get; set; }
    }

    public partial class VEPFilterDataReport
    {
        public string RefNo { get; set; }

        public int CaseId { get; set; }

        public int PersonId { get; set; }

        public int ProvinceID { get; set; }

        public int DistrictID { get; set; }

        public int LocalMunicipalityID { get; set; }

        public string VictimisationType { get; set; }

        public string SiteCode { get; set; }

        public int PopulationGroupID { get; set; }

        public int GenderID { get; set; }

        public int? SexualOrientationID { get; set; }

        public int MaritalStatusID { get; set; }

        public int? Age { get; set; }

        public int DisabilityTypeID { get; set; }

        public int CitizenshipID { get; set; }

        public int? SettlementID { get; set; }

        public string KnownPerpetrator { get; set; }

        public string PerpFamilyMember { get; set; }

        public string PerpCommunityMember { get; set; }

        public string ReportedToPolice { get; set; }

        public DateTime? DateOfIncident { get; set; }

        public DateTime? DateOfReporting { get; set; }
    }


    public partial class LocalMunicipalityStatsGrid
    {
        public int localMunicipalityId { get; set; }

        public string localMunicipalityName { get; set; }
        public int totalSites { get; set; }
        public int totalVictimsCaptured { get; set; }
        public int totalServicesRendered { get; set; }
    }

    public partial class SiteStatsGrid
    {
        public int organisationId { get; set; }

        public string siteCode { get; set; }
        public int totalSites { get; set; }
        public int totalVictimsCaptured { get; set; }
        public int totalServicesRendered { get; set; }
    }
    #endregion


    #region   


    public partial class CYCACaseGridMain
    {

        public long Request_Id { get; set; }
        public string Sent_By { get; set; }
        public string Created_By { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        [Display(Name = "Subject")]
        public string Request_Subject { get; set; }
        public DateTime? Date_Recieved { get; set; }
        public string Time_Recieved { get; set; }
        public DateTime? Date_Created { get; set; }
        public DateTime? Date_Closed { get; set; }

        [Display(Name = "Comments")]
        public string Request_Comments { get; set; }

        [Display(Name = "Facility")]
        public int? Facility_Id { get; set; }
        [Display(Name = "Facility")]
        public string SelectFacility { get; set; }

        [Display(Name = "Status")]
        public int? Request_Status_Id { get; set; }

        public string Request_Status_Comments { get; set; }
        [Display(Name = "Admission Type")]
        public string selectAdmissionType { get; set; }
        [Display(Name = "Bed Request Status")]
        public string selectBedRequestStatus { get; set; }
        public string selectPropationOfficer { get; set; }
        public string courtName { get; set; }
        public string RequestOpenClose { get; set; }

    }

    #endregion

}