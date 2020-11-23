using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDIIS.Models
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
                                         Selected = e.Head_Of_Department_Id.Equals(Head_Of_Department_Id)
                                     }).ToList();

                var selectList = new SelectList(employeesList, "Value", "Text", Head_Of_Department_Id);

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

                var selectList = new SelectList(serviceOfficesList, "Value", "Text", Salary_Level_Id);

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
}