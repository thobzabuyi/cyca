using Common_Objects;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mail;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize(Roles = "SysAdmin")]
        public ActionResult Index(string SearchPersalNumber, string SearchEmail, string SearchFirstName, string SearchLastName)
        {
            var employeeModel = new EmployeeModel();
            var employeeList = employeeModel.GetListOfEmployees(true, false, SearchPersalNumber, SearchEmail, SearchFirstName, SearchLastName);

            return View(employeeList);
        }

        [CustomAuthorize("Main", "Employee", "Create")]
        public ActionResult Create()
        {
            var newEmployee = new Employee() { User = new User(), Is_Active = true, Is_Deleted = false, Date_Created = DateTime.Now, Created_By = "" };

            var employeeServices = new ProblemCategoryModel();
            var employeeJobPosition = new JobPositionModel();

            List<SelectListItem> AvailableEmployeeServices = new List<SelectListItem>();


            foreach (var item in employeeServices.GetListOfProblemCategories())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                AvailableEmployeeServices.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Problem_Category_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableEmployeeServices = AvailableEmployeeServices;

            List<SelectListItem> AvailableEmployeeJobPosition = new List<SelectListItem>();


            foreach (var item in employeeJobPosition.GetListOfJobPositions())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                AvailableEmployeeJobPosition.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Job_Position_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableJobPosition = AvailableEmployeeJobPosition;



            return View(newEmployee);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        //public async System.Threading.Tasks.Task<ActionResult> Create(Employee employee, FormCollection data)
        public ActionResult Create(Employee employee, FormCollection data)
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
            var auditTrail = new AuditTrailModel();


            #region ModelState && CreateEmployee
            if (ModelState.IsValid)
            {
                var userModel = new UserModel();

                #region Check Existance (Email, First Name, Last Name)
                var employeemodel = new EmployeeModel();
                bool checkIfEmployeeExists = employeemodel.CheckIfEmployeeExists(employee.User.Email_Address, employee.User.First_Name, employee.User.Last_Name);
                if (checkIfEmployeeExists == false)
                {
                    // TODO: fix created-by field
                    var createEmployee = employeemodel.CreateEmployee(employee.User.First_Name, employee.User.Last_Name, employee.User.Initials, employee.User.Email_Address,
                    employee.Persal_Number, employee.Head_Of_Department_Id, employee.Supervisor_Id, employee.Phone_Number, employee.Mobile_Phone_Number, employee.Gender_Id,
                    employee.Race_Id, employee.ID_Number, employee.Job_Position_Id, employee.Paypoint_Id, employee.Service_Office_Id, employee.Salary_Level_Id,
                    employee.Is_Shift_Worker, employee.Is_Casual_Worker, isActive, isDeleted, dateCreated, createdBy,
                    employee.Is_SocialWorker, employee.PracticeNumber, currentUser.User_Id);

                    if (createEmployee == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact support";
                        return View(employee);
                    }
                    else
                    {                        
                        auditTrail.InsertAuditTrail(userName, "Add New Employee Details", "Admin");
                    }
                    #endregion

                    #region Link Roles
                    var roleIds = new List<int>();
                    if (employee.User.Posted_Roles != null)
                    {
                        foreach (var roleId in employee.User.Posted_Roles.Role_IDs)
                        {
                            var roleIdValue = int.Parse(roleId);
                            roleIds.Add(roleIdValue);
                        }
                    }

                    userModel.AddUserToRole(createEmployee.User.User_Id, roleIds);
                    #endregion

                    #region link Selected Groups
                    var groupIds = new List<int>();
                    if (employee.User.Posted_Groups != null)
                    {
                        foreach (var groupId in employee.User.Posted_Groups.Group_IDs)
                        {
                            var groupIdValue = int.Parse(groupId);
                            groupIds.Add(groupIdValue);
                        }
                    }

                    userModel.AddUserToGroup(createEmployee.User.User_Id, groupIds);
                    #endregion

                    #region employee services and roles
                    var employeeServicesM = new EmployeeServiceModel();
                    var value = data["ServiceList"].ToString();

                    if (value != null)
                    {
                        string[] selectedServiceArray = data["ServiceList"].ToString().Split(',');
                        employeeServicesM.Delete(Convert.ToInt32(createEmployee.Employee_Id));

                        foreach (string i in selectedServiceArray)
                        {
                            employeeServicesM.CreateEmployeeService(createEmployee.Employee_Id, Convert.ToInt32(i), currentUser.User_Id);                            
                            
                        }

                        auditTrail.InsertAuditTrail(userName, "Add New Employee Services" + ":" + value, "Admin");

                    }
                    var employeerolesM = new EmployeeRolesModel();
                    var value2 = data["JobPositionList"].ToString();
                    if (value2 != null)
                    {
                        string[] selectedRoleArray = data["JobPositionList"].ToString().Split(',');
                        employeerolesM.DeleteEmployeeJobPositions(Convert.ToInt32(createEmployee.Employee_Id));

                        foreach (string i in selectedRoleArray)
                        {
                            employeerolesM.CreateEmployeeJobPosition(createEmployee.Employee_Id, Convert.ToInt32(i), currentUser.User_Id);
                            
                        }
                        auditTrail.InsertAuditTrail(userName, "Add New Employee Job position" + ":" + value2, "Admin");
                    }


                    #endregion

                    

                }
                else
                {
                    TempData["EnteredUser"] = "The entered employee named: " + employee.User.First_Name + " " + employee.User.Last_Name + " with persal #:" + employee.Persal_Number;
                    return RedirectToAction("DoesExist");

                }


            return RedirectToAction("Index", "Employee");
        }
        #endregion

        #region Services && Job position
        var employeeServices = new ProblemCategoryModel();
            var employeeJobPosition = new JobPositionModel();

            List<SelectListItem> AvailableEmployeeServices = new List<SelectListItem>();


            foreach (var item in employeeServices.GetListOfProblemCategories())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                AvailableEmployeeServices.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Problem_Category_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableEmployeeServices = AvailableEmployeeServices;

            List<SelectListItem> AvailableEmployeeJobPosition = new List<SelectListItem>();


            foreach (var item in employeeJobPosition.GetListOfJobPositions())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                AvailableEmployeeJobPosition.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Job_Position_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableJobPosition = AvailableEmployeeJobPosition;
            #endregion

            return View(employee);
        }

       
        public ActionResult DoesExist()
        {
            return View();
        }
        [CustomAuthorize("Main", "Employee", "Edit")]
        public ActionResult Edit(string id)
        {
            var employeeModel = new EmployeeModel();
           
             var employeeToEdit = employeeModel.GetSpecificEmployee(int.Parse(id));
            Session["EmployeeId"] = int.Parse(id);

            var employeeServices = new ProblemCategoryModel();
            var employeeJobPosition = new JobPositionModel();

            List<SelectListItem> AvailableEmployeeServices = new List<SelectListItem>();

            foreach (var item in employeeServices.GetListOfProblemCategories())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;

                if(employeeToEdit.SelectedEmployeeServiceList.Contains(item.Problem_Category_Id))
                {
                    itemSelected = true;
                }

                AvailableEmployeeServices.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Problem_Category_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableEmployeeServices = AvailableEmployeeServices;
            //employeeModel.PostedEmployeeServiceType.ListOfEmployeeServiceIDs = new SelectList(AvailableEmployeeServices, "Value", "Text");

            List<SelectListItem> AvailableEmployeeJobPosition = new List<SelectListItem>();


            foreach (var item in employeeJobPosition.GetListOfJobPositions())
            {
                var SelectItem = new SelectListItem();
                bool itemSelected = false;


                if (employeeToEdit.SelectedEmployeeRoleList.Contains(item.Job_Position_Id))
                {
                    itemSelected = true;
                }

                AvailableEmployeeJobPosition.Add(new SelectListItem
                {
                    Text = item.Description,
                    Value = item.Job_Position_Id.ToString(),
                    Selected = itemSelected
                });
            }

            ViewBag.AvailableJobPosition = AvailableEmployeeJobPosition;
            //employeeModel.PostedEmployeeRoleType.ListOfEmployeeRoleIDs = new SelectList(AvailableEmployeeJobPosition, "Value", "Text");

            return View(employeeToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Employee employee, FormCollection data)
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
            var auditTrail = new AuditTrailModel();

            if (ModelState.IsValid)
            {
                var userModel = new UserModel();
                var employeeModel = new EmployeeModel();
                var updatedEmployee = employeeModel.EditEmployee(employee.Employee_Id, employee.User.First_Name, employee.User.Last_Name, 
                    employee.User.Initials, employee.User.Email_Address, employee.Persal_Number, employee.Head_Of_Department_Id, 
                    employee.Supervisor_Id, employee.Phone_Number, employee.Mobile_Phone_Number, employee.Gender_Id, employee.Race_Id, 
                    employee.ID_Number, employee.Job_Position_Id, employee.Paypoint_Id, employee.Service_Office_Id, employee.Salary_Level_Id, 
                    employee.Is_Shift_Worker, employee.Is_Casual_Worker, isActive, isDeleted, dateLastModified, modifiedBy, employee.Is_SocialWorker, employee.PracticeNumber,employee.Facility_Id
                    );

                if (updatedEmployee == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(employee);
                }
                else
                {
                    auditTrail.InsertAuditTrail(userName, "Update Employee Details", "Admin");
                }

                // Link Selected Roles
                var roleIds = new List<int>();
                if (employee.User.Posted_Roles != null)
                {
                    foreach (var roleId in employee.User.Posted_Roles.Role_IDs)
                    {
                        var roleIdValue = int.Parse(roleId);
                        roleIds.Add(roleIdValue);
                    }
                }

                userModel.AddUserToRole(updatedEmployee.User.User_Id, roleIds);
               
                // Link Selected Groups
                var groupIds = new List<int>();
                if (employee.User.Posted_Groups != null)
                {
                    foreach (var groupId in employee.User.Posted_Groups.Group_IDs)
                    {
                        var groupIdValue = int.Parse(groupId);
                        groupIds.Add(groupIdValue);
                    }
                }

                userModel.AddUserToGroup(updatedEmployee.User.User_Id, groupIds);
               

                #region employee services and roles
                var employeeServicesM = new EmployeeServiceModel();
                if (data["ServiceList"] != null)
                {
                    //var value = data["ServiceList"].ToString();
                    string[] selectedServiceArray = data["ServiceList"].ToString().Split(',');
                    employeeServicesM.Delete(Convert.ToInt32(updatedEmployee.Employee_Id));

                    foreach (string i in selectedServiceArray)
                    {
                        employeeServicesM.CreateEmployeeService(updatedEmployee.Employee_Id, Convert.ToInt32(i), currentUser.User_Id);
                        
                    }
                    auditTrail.InsertAuditTrail(userName, "Update Employee Services" + ":" + data["ServiceList"], "Admin");
                }
                var employeerolesM = new EmployeeRolesModel();
                if (data["JobPositionList"] != null)
                {
                    //var value2 = data["JobPositionList"].ToString();
                    string[] selectedRoleArray = data["JobPositionList"].ToString().Split(',');
                    employeerolesM.DeleteEmployeeJobPositions(Convert.ToInt32(updatedEmployee.Employee_Id));

                    foreach (string i in selectedRoleArray)
                    {
                        employeerolesM.CreateEmployeeJobPosition(updatedEmployee.Employee_Id, Convert.ToInt32(i), currentUser.User_Id);
                       
                    }

                    auditTrail.InsertAuditTrail(userName, "Update Employee Job position" + ":" + data["JobPositionList"], "Admin");
                }                
                
                #endregion

                return RedirectToAction("Index", "Employee");
            }

            


            return View(employee);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SetEmployeeServices(List<string> employeeServicesList)
        {
            if (employeeServicesList != null)
            {
                Session["EmployeeServicesList"] = employeeServicesList;
            }
            else
            {
                Session["EmployeeServicesList"] = null;
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SetEmployeeJobPosition(List<int> employeeJobPositionList)
        {
            if (employeeJobPositionList != null)
            {
                Session["EmployeeJobPositionList"] = employeeJobPositionList;
            }
            else
            {
                Session["EmployeeJobPositionList"] = null;
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }


        private SelectList SetSelectedEmployeeServices(List<int> selectedEmployeeServiceList, SelectList employeeService_List)
        {
            foreach(var item in employeeService_List)
            {
                if(selectedEmployeeServiceList.Contains(Convert.ToInt32(item.Value)))
                {
                    item.Selected = true;
                }
                else { item.Selected = false; }
            }

            return employeeService_List;
        }

        public ActionResult GetListOfServices(int Id)
        {

            EmployeeModel Model = new EmployeeModel();
            List<EmployeeService_RoleVM> employeeServices =Model.GetListOfServices(Id);
            return PartialView(employeeServices);
        }

        public ActionResult GetListOfRoles(int Id)
        {
            var currentUser = (User)Session["CurrentUser"];


            EmployeeModel Model = new EmployeeModel();
            List<EmployeeService_RoleVM> EmployeeRoles =Model.GetListOfRoles(Id);
            return PartialView(EmployeeRoles);
        }

        public ActionResult DeleteService(int Id, int EmployeeId)
        {
            EmployeeModel Model = new EmployeeModel();
            Model.Delete_Service(Id, EmployeeId);
            return RedirectToAction("Edit", new { id = EmployeeId });
        }
        public ActionResult DeleteRole(int IdR, int EmployeeIdR)
        {
            EmployeeModel Model = new EmployeeModel();
            Model.Delete_Role(IdR, EmployeeIdR);
            return RedirectToAction("Edit", new { id = EmployeeIdR });

        }

        [CustomAuthorize("Main", "Employee", "Delete")]
        public ActionResult Delete(string id)
        {
            var employeeModel = new EmployeeModel();
            var deleteEmployee = employeeModel.GetSpecificEmployee(int.Parse(id));

            return View(deleteEmployee);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Employee employee)
        {
            var employeeModel = new EmployeeModel();
            var deletedEmployee = employeeModel.SetEmployeeIsDeleted(employee.Employee_Id, true);

            if (deletedEmployee == null)
            {
                ViewBag.Message = "An Error Occured, Contact support";
                return View(employee);
            }

            return RedirectToAction("Index", "Employee");
        }
	}
}