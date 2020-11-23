using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class EmployeeModel
    {
        private SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();
        public Employee GetSpecificEmployee(int employeeId)
        {
            Employee employee;
         
            try
            {
                var employees = (from e in dbContext.Employees
                                 where e.Employee_Id.Equals(employeeId)
                                 select e).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();



                employee = (from e in employees
                            select e).FirstOrDefault();

                var categoryList = dbContext.EmployeeServices.Where(a => a.Employee_Id == employeeId)
                    .Select(c => new
                    {
                        categoryId = c.Problem_Category_Id
                    }).ToList();

                employee.SelectedEmployeeServiceList = new List<int>();

                foreach (var categoryId in categoryList)
                {
                    employee.SelectedEmployeeServiceList.Add(Convert.ToInt32(categoryId.categoryId));
                }

                var jobList = dbContext.EmployeeRoles.Where(a => a.Employee_Id == employeeId)
                    .Select(c => new
                    {
                        jobId = c.EmployeeRole_Id
                    }).ToList();
                employee.SelectedEmployeeRoleList = new List<int>();

                foreach (var jobId in jobList)
                {
                    employee.SelectedEmployeeRoleList.Add(Convert.ToInt32(jobId.jobId));
                }

                if (employee.Is_SocialWorker)
                {
                    var socialWorkerPracticeNumber = dbContext.Social_Workers.Where(x => x.User_Id == employee.User_Id).Select(y => y.SocialWorkerPracticeNumber).FirstOrDefault();
                    employee.PracticeNumber = socialWorkerPracticeNumber;
                }

            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return employee;
        }

        public Employee GetSpecificUser(string userName)
        {
            Employee employee;

           
            try
            {
                var employees = (from e in dbContext.Employees
                                 where e.User.User_Name.Equals(userName)
                                 select e).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                employee = (from e in employees
                            select e).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return employee;
        }

        public List<Employee> GetListOfEmployees(bool showInActive, bool showDeleted, string SearchPersalNumber, string SearchEmail, string SearchFirstName, string SearchLastName)
        {
            List<Employee> listOfEmployees;
            try
            {
                var employees = (from e in dbContext.Employees
                                 where e.Is_Active || e.Is_Active.Equals(!showInActive)
                                 where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                                 select e).ToList();
                if ((SearchPersalNumber != null && SearchPersalNumber != "")
                    ||
                    (SearchPersalNumber != null && SearchEmail != "" )
                    ||
                    (SearchFirstName != null && SearchFirstName != "")
                    ||
                    (SearchLastName != null && SearchLastName != "" )
                    )
                    {
                    employees = (from e in dbContext.Employees
                                 where e.Is_Active || e.Is_Active.Equals(!showInActive)
                                 where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                                 where e.Persal_Number.Contains(SearchPersalNumber) || SearchPersalNumber==""
                                 where e.User.Email_Address.Contains(SearchEmail) || SearchEmail==""
                                 where e.User.First_Name.Contains(SearchFirstName)|| SearchFirstName==""
                                 where e.User.Last_Name.Contains(SearchLastName)|| SearchLastName==""
                                 select e).ToList();
                }            



                    //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                    listOfEmployees = (from e in employees
                                       select e).ToList();
               
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfEmployees;

        }

        public List<Employee> GetListOfEmployees(bool showInActive, bool showDeleted)
        {
            List<Employee> listOfEmployees;
            try
            {
                var employees = (from e in dbContext.Employees
                                 join f in dbContext.EmployeeRoles on e.Employee_Id equals f.Employee_Id
                                 join g in dbContext.Service_Offices on e.Service_Office_Id equals g.Service_Office_Id
                                 join h in dbContext.Local_Municipalities on g.Local_Municipality_Id equals h.Local_Municipality_Id
                                 join i in dbContext.Districts on h.District_Municipality_Id equals i.District_Id
                                 join j in dbContext.Provinces on i.Province_Id equals j.Province_Id
                                 where e.Is_Active || e.Is_Active.Equals(!showInActive)
                                 where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                                 where f.JobPosition_Id==10
                                 //where j.Province_Id==
                                 select e).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfEmployees = (from e in employees
                                   select e).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfEmployees;

        }

        public List<apl_Cyca_Facility> GetListfFaciities(bool showInActive, bool showDeleted)
        {
            List<apl_Cyca_Facility> listOfFacilities;

            try
            {
                var facilities = (from e in dbContext.apl_Cyca_Facility
                                 select e).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfFacilities = (from e in facilities
                                    select e).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfFacilities;
        }
        //Herman Update
        public bool CheckIfSupervisor(int Id)
        {
            int? CheckIfSupervisor = (from a in dbContext.Employees
                                     where a.User_Id == Id
                                     select a.Supervisor_Id).FirstOrDefault();
            if (CheckIfSupervisor != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // Kholo Update
        public SelectList GetListOfEmployeesInServicePoint(bool showInActive, bool showDeleted, int UserID)
        {
            //List<Employee> listOfEmployees;
            //var selectListEmployeT;

            // Emp Details
            var empDetail = (from e in dbContext.Employees
                             where e.User_Id == UserID
                             select e).FirstOrDefault();


            // Super Details
            //var empSuperVDetail = (from e in dbContext.Employees
            //                       where e.User_Id == empDetail.Supervisor_Id
            //                       select e).FirstOrDefault();


            // int? ServicePointId = empSuperVDetail.Service_Office_Id;

            //try
            //{
            var employees = (from e in dbContext.Employees
                             where e.Is_Active || e.Is_Active.Equals(!showInActive)
                             where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                             where e.Supervisor_Id == empDetail.User_Id
                             //Apl_JobPosition equal supervisor
                             where e.Job_Position_Id == 10
                             select e).ToList();

            //listOfAgents = PopulateAdditionalItems(agents, dbContext);

            var listOfEmployees = (from e in employees
                                   join b in dbContext.Users on e.User_Id equals b.User_Id
                                   orderby b.First_Name, b.Last_Name
                                   select new
                                   {
                                       e.Employee_Id,
                                       e.User_Id,
                                       e.Supervisor_Id,
                                       NameSurname = b.First_Name + " " + b.Last_Name
                                   }).ToList();

            var selectListEmploye = new SelectList(listOfEmployees, "User_Id", "NameSurname", 0);
            //selectListEmployeT = selectListEmploye;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

            return selectListEmploye;

        }

        public SelectList GetListOfEmployeesBySupervisorList(bool showInActive, bool showDeleted, int userId)
        {            

            var employees = new List<Employee>();
            

            int? ProvId = (from p in dbContext.Employees
                          join a in dbContext.Users on p.User_Id equals a.User_Id
                          
                          join c in dbContext.Service_Offices on p.Service_Office_Id equals c.Service_Office_Id
                          join d in dbContext.Local_Municipalities on c.Local_Municipality_Id equals d.Local_Municipality_Id
                          join e in dbContext.Districts on d.District_Municipality_Id equals e.District_Id
                          join x in dbContext.Provinces on e.Province_Id equals x.Province_Id
                          where p.Is_Active || p.Is_Active.Equals(!showInActive)
                          where !p.Is_Deleted || p.Is_Deleted.Equals(showDeleted)
                          where p.User_Id == userId
                          select x.Province_Id).FirstOrDefault();
  
            
            var listOfEmployees = (from e in dbContext.Employees
                                   join b in dbContext.Users on e.User_Id equals b.User_Id
                                   join f in dbContext.EmployeeRoles on e.Employee_Id equals f.Employee_Id
                                   join z in dbContext.EmployeeServices on e.Employee_Id equals z.Employee_Id
                                   join g in dbContext.Service_Offices on e.Service_Office_Id equals g.Service_Office_Id
                                   join h in dbContext.Local_Municipalities on g.Local_Municipality_Id equals h.Local_Municipality_Id
                                   join i in dbContext.Districts on h.District_Municipality_Id equals i.District_Id
                                   join j in dbContext.Provinces on i.Province_Id equals j.Province_Id
                                   where e.Is_Active || e.Is_Active.Equals(!showInActive)
                                   where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                                   //Condition Supervisor
                                   where f.JobPosition_Id == 10
                                   //Condition to Add Province
                                   where j.Province_Id==ProvId

                                   select new {
                                       e.Employee_Id,
                                       e.User_Id,
                                       e.Supervisor_Id,
                                       NameSurname = b.First_Name + " " + b.Last_Name

                                   }).Distinct().ToList();

            var selectListEmploye = new SelectList(listOfEmployees, "User_Id", "NameSurname", 0);

            
            return selectListEmploye;

        }

        public List<Employee> GetListOfEmployeesCM(int userId)
        {
            List<Employee> employees;

           // using (var dbContext = new SDIIS_DatabaseEntities())
           // {
                try
                {
                    var employeesList = (from r in dbContext.Employees
                                        where r.Supervisor_Id.Equals(userId)
                                        select r).ToList();

                    employees = (from r in employeesList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
           // }

            return employees;
        }


        public SelectList GetListOfEmployeesByCaseManagerList(bool showInActive, bool showDeleted, int UserID, int? ServiceId)
        {
            //List<Employee> listOfEmployees;
            //var selectListEmployeT;

            // Emp Details
            var empDetail = (from e in dbContext.Employees
                             where e.User_Id == UserID
                             select e).FirstOrDefault();

            int? ProvId = (from p in dbContext.Employees
                           join a in dbContext.Users on p.User_Id equals a.User_Id

                           join c in dbContext.Service_Offices on p.Service_Office_Id equals c.Service_Office_Id
                           join d in dbContext.Local_Municipalities on c.Local_Municipality_Id equals d.Local_Municipality_Id
                           join e in dbContext.Districts on d.District_Municipality_Id equals e.District_Id
                           join x in dbContext.Provinces on e.Province_Id equals x.Province_Id
                           where p.Is_Active || p.Is_Active.Equals(!showInActive)
                           where !p.Is_Deleted || p.Is_Deleted.Equals(showDeleted)
                           where p.User_Id == UserID
                           select x.Province_Id).FirstOrDefault();

            var employees = (from e in dbContext.Employees
                             join b in dbContext.Users on e.User_Id equals b.User_Id
                             join f in dbContext.EmployeeRoles on e.Employee_Id equals f.Employee_Id
                             join z in dbContext.EmployeeServices on e.Employee_Id equals z.Employee_Id
                             join g in dbContext.Service_Offices on e.Service_Office_Id equals g.Service_Office_Id
                             join h in dbContext.Local_Municipalities on g.Local_Municipality_Id equals h.Local_Municipality_Id
                             join i in dbContext.Districts on h.District_Municipality_Id equals i.District_Id
                             join j in dbContext.Provinces on i.Province_Id equals j.Province_Id
                             where e.Is_Active || e.Is_Active.Equals(!showInActive)
                             where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                             //Condition Supervisor
                             where f.JobPosition_Id == 1010 || f.JobPosition_Id==1 || f.JobPosition_Id== ServiceId
                             ||f.JobPosition_Id==5||f.JobPosition_Id==6||f.JobPosition_Id==7
                             //Condition to Add Province
                             where j.Province_Id == ProvId

                             select new
                             {
                                 e.Employee_Id,
                                 e.User_Id,
                                 e.Supervisor_Id,
                                 NameSurname = b.First_Name + " " + b.Last_Name

                             }).Distinct().ToList();

            //listOfAgents = PopulateAdditionalItems(agents, dbContext);

            var listOfEmployees = (from e in employees
                                   join b in dbContext.Users on e.User_Id equals b.User_Id
                                   orderby b.First_Name, b.Last_Name
                                   select new
                                   {
                                       e.Employee_Id,
                                       e.User_Id,
                                       e.Supervisor_Id,
                                       NameSurname = b.First_Name + " " + b.Last_Name
                                   }).ToList();

            var selectListEmploye = new SelectList(listOfEmployees, "User_Id", "NameSurname", 0);
            //selectListEmployeT = selectListEmploye;
            

            return selectListEmploye;

        }

        //herman
        public bool CheckIfsupervisorIsnull(int Id)
        {

            int? Check = dbContext.Intake_Assessments.Find(Id).Case_Manager_Supervisor_Id;

            if (Check != 0 && Check!=null)
            {
                return true;
            }
            else
            { 
            return false;
            }
        }


        // Kholo Update
        public SelectList GetListOfEmployeesCaseManager(bool showInActive, bool showDeleted, int UserID)
        {

            int employee_Id = (from a in dbContext.Employees
                               where a.User_Id == UserID
                               select a.Employee_Id).FirstOrDefault();
            // Get subordinates
            var empDetail = (from b in dbContext.Employees
                             where b.Supervisor_Id == employee_Id
                             select b).ToList();


            //try
            //{
            var employees = (from e in empDetail // dbContext.Employees
                             where e.Is_Active || e.Is_Active.Equals(!showInActive)
                             where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                             //where e.Service_Office_Id == ServicePointId
                             where e.Supervisor_Id != null
                             select e).ToList();

            //listOfAgents = PopulateAdditionalItems(agents, dbContext);

            var listOfEmployees = (from e in empDetail
                                   join b in dbContext.Users on e.User_Id equals b.User_Id
                                   orderby b.First_Name, b.Last_Name
                                   select new
                                   {
                                       e.Employee_Id,
                                       e.User_Id,
                                       e.Supervisor_Id,
                                       NameSurname = b.First_Name + " " + b.Last_Name
                                   }).ToList();

            var selectListEmploye = new SelectList(listOfEmployees, "User_Id", "NameSurname", 0);
            //selectListEmployeT = selectListEmploye;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

            return selectListEmploye;

        }

        //Addtion to Check if Employee exists by Herman
        public bool CheckIfEmployeeExists(string Email_Address, string First_Name, string Last_Name)
        {
           // using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
           // {
                return dbContext.Users.Where(b => b.Email_Address.Equals(Email_Address)&& b.First_Name.Equals(First_Name) && b.Last_Name.Equals(Last_Name)).Any();
          //  }

        }
        //End Herman
        public Employee CreateEmployee(string firstName, string lastName, string initials, string emailAddress, string persalNumber, int? headOfDepartmentId, int? supervisorId, string phoneNumber, string mobilePhoneNumber, 
            int? genderId, int? raceId, string idNumber, int? jobPositionId, int? payPointId, int serviceOfficeId, int? salaryLevelId, bool isShiftWorker, bool isCasualWorker, 
            bool isActive, bool isDeleted, DateTime dateCreated, string createdBy, bool isSocialWorker, string practiceNumber, int UserId)
        {
            Employee newEmployee;
            
            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                User newUser;

                var user = new User
                {
                    User_Name = string.Format("{0}{1}", firstName.Substring(0, 1), lastName),
                    Password = "password1",
                    First_Name = firstName,
                    Last_Name = lastName,
                    Initials = initials,
                    Email_Address = emailAddress,
                    Is_Active = isActive,
                    Is_Deleted = isDeleted,
                    Date_Created = dateCreated,
                    Created_By = createdBy
                };

                try
                {
                    int NumberOfUserNames = (from a in dbContext.Users
                                             where a.User_Name.Contains(user.User_Name)
                                             select a).Count();
                    if (NumberOfUserNames > 1)
                    {
                        user.User_Name += NumberOfUserNames;
                    }
                    newUser = dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }

                if (newUser == null)
                {
                    return null;
                }

                var employee = new Employee
                {
                    User_Id = newUser.User_Id,
                    Persal_Number = persalNumber,
                    Head_Of_Department_Id = headOfDepartmentId,
                    Supervisor_Id = supervisorId,
                    Phone_Number = phoneNumber,
                    Mobile_Phone_Number = mobilePhoneNumber,
                    Gender_Id = genderId,
                    Race_Id = raceId,
                    ID_Number = idNumber,
                    Job_Position_Id = jobPositionId,
                    Paypoint_Id = payPointId,
                    Service_Office_Id = serviceOfficeId,
                    Salary_Level_Id = salaryLevelId,
                    Is_Shift_Worker = isShiftWorker,
                    Is_Casual_Worker = isCasualWorker,
                    Is_Active = isActive,
                    Is_Deleted = isDeleted,
                    Is_SocialWorker = isSocialWorker,
                    Date_Created = dateCreated,
                    Created_By = createdBy
                };

                try
                {
                    newEmployee = dbContext.Employees.Add(employee);
                    dbContext.SaveChanges();


                    if (isSocialWorker == true)
                    {
                        int userid = newEmployee.User_Id;

                        //get the supervisor employee ID to be used to get the user id that we will unse to link the social worker table correctly
                        var employeeUserId = dbContext.Employees.Where(x => x.Employee_Id == newEmployee.Supervisor_Id).Select(y => y.User_Id).FirstOrDefault();

                        //get socialworker id for the supervisor
                        var supervisorSocialWorkerID = dbContext.Social_Workers.Where(x => x.User_Id == employeeUserId).Select(y => y.Social_Worker_Id).FirstOrDefault();

                        var socialWorkerDetails = (from s in dbContext.Social_Workers
                                                   where s.User_Id == userid
                                                   select s).FirstOrDefault();

                        if (socialWorkerDetails == null)
                        {
                            Social_Worker sw = new Social_Worker();
                            sw.Gender_Id = newEmployee.Gender_Id;
                            sw.ID_Number = newEmployee.ID_Number;
                            sw.User_Id = newEmployee.User_Id;
                            sw.Is_Active = newEmployee.Is_Active;
                            sw.Is_Deleted = newEmployee.Is_Deleted;
                            sw.Mobile_Phone_Number = newEmployee.Mobile_Phone_Number;
                            sw.Modified_By = createdBy;
                            sw.Organization_Id = 1;//Thabiso to verify
                            sw.Service_Office_Id = newEmployee.Service_Office_Id;
                            sw.SocialWorkerPracticeNumber = practiceNumber;
                            sw.Date_Created = DateTime.Now;
                            sw.Date_Last_Modified = DateTime.Now;
                            sw.Created_By = createdBy;
                            sw.Phone_Number = newEmployee.Phone_Number;
                            if (supervisorId != null)
                            {
                                sw.Reports_To_Social_Worker_Id = supervisorSocialWorkerID;
                            }
                           

                            dbContext.Social_Workers.Add(sw);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            socialWorkerDetails.Gender_Id = newEmployee.Gender_Id;
                            socialWorkerDetails.ID_Number = newEmployee.ID_Number;
                            socialWorkerDetails.Is_Active = newEmployee.Is_Active;
                            socialWorkerDetails.Is_Deleted = newEmployee.Is_Deleted;
                            socialWorkerDetails.Mobile_Phone_Number = newEmployee.Mobile_Phone_Number;
                            socialWorkerDetails.Modified_By = createdBy;
                            socialWorkerDetails.Organization_Id = 1;//Thabiso to verify
                            socialWorkerDetails.Service_Office_Id = newEmployee.Service_Office_Id;
                            socialWorkerDetails.SocialWorkerPracticeNumber = practiceNumber;
                            socialWorkerDetails.Date_Created = DateTime.Now;
                            socialWorkerDetails.Date_Last_Modified = DateTime.Now;
                            socialWorkerDetails.Created_By = createdBy;
                            socialWorkerDetails.Phone_Number = newEmployee.Phone_Number;
                            if (supervisorId != null)
                            {
                                socialWorkerDetails.Reports_To_Social_Worker_Id = supervisorSocialWorkerID;
                            }
                           
                            dbContext.SaveChanges();
                        }

                    }

                }
                catch (Exception)
                {
                    return null;
                }
                //if (selectedEmployeeService_Ids != null && selectedJobPosition_Ids != null)
                //{
                //    var employeeService = new EmployeeService();

                //    foreach (var itemss in selectedEmployeeService_Ids)
                //    {
                //        employeeService.Employee_Id = (from a in dbContext.Employees
                //                                       where a.User_Id == newUser.User_Id
                //                                       select a.Employee_Id).FirstOrDefault();

                //        employeeService.Problem_Category_Id = itemss;
                //        employeeService.CreatedTimeStamp = DateTime.Now;
                //        employeeService.Created_By = UserId;
                //        employeeService.Is_Active = Convert.ToBoolean(1);
                //        employeeService.Is_Deleted = Convert.ToBoolean(0);
                //        dbContext.EmployeeServices.Add(employeeService);
                //        dbContext.SaveChanges();



                //    }
                //    var jobPosition = new EmployeeRole();

                //    foreach (var itemsss in selectedJobPosition_Ids)
                //    {
                //        jobPosition.Employee_Id = (from a in dbContext.Employees
                //                                   where a.User_Id == newUser.User_Id
                //                                   select a.Employee_Id).FirstOrDefault();
                //        jobPosition.JobPosition_Id = itemsss;
                //        jobPosition.CreatedTimeStamp = DateTime.Now;
                //        jobPosition.Created_By = UserId;
                //        jobPosition.Is_Active = Convert.ToBoolean(1);
                //        jobPosition.Is_Deleted = Convert.ToBoolean(0);
                //        dbContext.EmployeeRoles.Add(jobPosition);
                //        dbContext.SaveChanges();
                //    }

                //}

            }

            return newEmployee;
        }

        public Employee EditEmployee(int employeeId, string firstName, string lastName, string initials, string emailAddress, string persalNumber, int? headOfDepartmentId, int? supervisorId, string phoneNumber, string mobilePhoneNumber, 
            int? genderId, int? raceId, string idNumber, int? jobPositionId, int? payPointId, int serviceOfficeId, int? salaryLevelId, bool isShiftWorker, bool isCasualWorker, 
            bool isActive, bool isDeleted, DateTime dateLastModified, string modifiedBy, bool isSocialWorker, string practiceNumber,int? facilityId)
        {
            Employee editEmployee;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editEmployee = (from e in dbContext.Employees
                                    where e.Employee_Id.Equals(employeeId)
                                    select e).FirstOrDefault();

                    if (editEmployee == null) return null;

                    editEmployee.User.First_Name = firstName;
                    editEmployee.User.Last_Name = lastName;
                    editEmployee.User.Initials = initials;
                    editEmployee.User.Email_Address = emailAddress;
                    editEmployee.Persal_Number = persalNumber;
                    editEmployee.Head_Of_Department_Id = headOfDepartmentId;
                    editEmployee.Supervisor_Id = supervisorId;
                    editEmployee.Phone_Number = phoneNumber;
                    editEmployee.Mobile_Phone_Number = mobilePhoneNumber;
                    editEmployee.Gender_Id = genderId;
                    editEmployee.Race_Id = raceId;
                    editEmployee.ID_Number = idNumber;
                    editEmployee.Job_Position_Id = jobPositionId;
                    editEmployee.Paypoint_Id = payPointId;
                    editEmployee.Service_Office_Id = serviceOfficeId;
                    editEmployee.Salary_Level_Id = salaryLevelId;
                    editEmployee.Is_Shift_Worker = isShiftWorker;
                    editEmployee.Is_Casual_Worker = isCasualWorker;
                    editEmployee.Is_Active = isActive;
                    editEmployee.Is_Deleted = isDeleted;
                    editEmployee.Date_Last_Modified = dateLastModified;
                    editEmployee.Modified_By = modifiedBy;
                    editEmployee.Is_SocialWorker = isSocialWorker;
                    editEmployee.Facility_Id = facilityId;
                    
                    dbContext.SaveChanges();

                    if (isSocialWorker == true)
                    {
                        int userid = editEmployee.User_Id;


                        //get the supervisor employee ID to be used to get the user id that we will unse to link the social worker table correctly
                        var employeeUserId = dbContext.Employees.Where(x => x.Employee_Id == editEmployee.Supervisor_Id).Select(y => y.User_Id).FirstOrDefault();

                        //get socialworker id for the supervisor
                        var supervisorSocialWorkerID = dbContext.Social_Workers.Where(x => x.User_Id == employeeUserId).Select(y => y.Social_Worker_Id).FirstOrDefault();

                        var socialWorkerDetails = (from s in dbContext.Social_Workers
                                                   where s.User_Id == userid
                                                   select s).FirstOrDefault();

                        if (socialWorkerDetails == null)
                        {
                            Social_Worker sw = new Social_Worker();
                            sw.Gender_Id = editEmployee.Gender_Id;
                            sw.ID_Number = editEmployee.ID_Number;
                            sw.User_Id = editEmployee.User_Id;
                            sw.Is_Active = editEmployee.Is_Active;
                            sw.Is_Deleted = editEmployee.Is_Deleted;
                            sw.Mobile_Phone_Number = editEmployee.Mobile_Phone_Number;
                            sw.Modified_By = modifiedBy;
                            sw.Organization_Id = 1;//Thabiso to verify
                            sw.Service_Office_Id = editEmployee.Service_Office_Id;
                            sw.SocialWorkerPracticeNumber = practiceNumber;
                            sw.Date_Created = DateTime.Now;
                            sw.Date_Last_Modified = DateTime.Now;
                            sw.Created_By = modifiedBy;
                            sw.Phone_Number = editEmployee.Phone_Number;
                            if (supervisorId != null)
                            {
                                sw.Reports_To_Social_Worker_Id = supervisorSocialWorkerID;
                            }
                            
                            dbContext.Social_Workers.Add(sw);
                            dbContext.SaveChanges();
                        }else
                        {
                            socialWorkerDetails.Gender_Id = editEmployee.Gender_Id;
                            socialWorkerDetails.ID_Number = editEmployee.ID_Number;
                            socialWorkerDetails.Is_Active = editEmployee.Is_Active;
                            socialWorkerDetails.Is_Deleted = editEmployee.Is_Deleted;
                            socialWorkerDetails.Mobile_Phone_Number = editEmployee.Mobile_Phone_Number;
                            socialWorkerDetails.Modified_By = modifiedBy;
                            socialWorkerDetails.Organization_Id = 1;//Thabiso to verify
                            socialWorkerDetails.Service_Office_Id = editEmployee.Service_Office_Id;
                            socialWorkerDetails.SocialWorkerPracticeNumber = practiceNumber;
                            socialWorkerDetails.Date_Created = DateTime.Now;
                            socialWorkerDetails.Date_Last_Modified = DateTime.Now;
                            socialWorkerDetails.Created_By = modifiedBy;
                            socialWorkerDetails.Phone_Number = editEmployee.Phone_Number;
                            if (supervisorId != null)
                            {
                                socialWorkerDetails.Reports_To_Social_Worker_Id = supervisorSocialWorkerID;
                            }
                            
                            dbContext.SaveChanges();
                        }
                        

                        
                    }

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

            }

            return editEmployee;
        }

        public Employee SetEmployeeIsActive(int employeeId, bool isActive)
        {
            Employee editEmployee;

          //  using (var dbContext = new SDIIS_DatabaseEntities())
           // {
                try
                {
                    editEmployee = (from e in dbContext.Employees
                                    where e.Employee_Id.Equals(employeeId)
                                    select e).FirstOrDefault();

                    if (editEmployee == null) return null;

                    editEmployee.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
           // }

            return editEmployee;
        }

        public Employee SetEmployeeIsDeleted(int employeeId, bool isDeleted)
        {
            Employee editEmployee;

          //  using (var dbContext = new SDIIS_DatabaseEntities())
          //  {
                try
                {
                    editEmployee = (from e in dbContext.Employees
                                    where e.Employee_Id.Equals(employeeId)
                                    select e).FirstOrDefault();

                    if (editEmployee == null) return null;

                    editEmployee.Is_Deleted = isDeleted;
                    editEmployee.User.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
           // }

            return editEmployee;
        }

        public List<EmployeeService_RoleVM> GetListOfServices(int Id)
        {
           // using (SDIIS_DatabaseEntities dbcontext = new SDIIS_DatabaseEntities()) {
                var employeeServices = (from a in dbContext.EmployeeServices
                                       where a.Employee_Id == Id
                                       select a).ToList();
                List<EmployeeService_RoleVM> NewModelListS = new List<EmployeeService_RoleVM>();
                foreach (var item in employeeServices)
                {
                    EmployeeService_RoleVM NewModelS = new EmployeeService_RoleVM();
                    NewModelS.Employee_Id = Id;
                    NewModelS.EmployeeServiceId = item.Problem_Category_Id;
                    NewModelS.ServiceDescription = dbContext.Problem_Categories.Find(item.Problem_Category_Id).Description;
                    NewModelListS.Add(NewModelS);
                }
                return NewModelListS;
          //  }
        }

        public List<EmployeeService_RoleVM> GetListOfRoles(int Id)
        {
          //  using (SDIIS_DatabaseEntities dbcontext = new SDIIS_DatabaseEntities())
          //  {
                var employeeRoles = (from a in dbContext.EmployeeRoles
                                       where a.Employee_Id == Id
                                       select a).ToList();
                List<EmployeeService_RoleVM> NewModelList = new List<EmployeeService_RoleVM>();
                foreach (var item in employeeRoles) {
                    EmployeeService_RoleVM NewModel = new EmployeeService_RoleVM();
                    NewModel.Employee_Id = Id;
                    NewModel.EmployeeRoleId = item.JobPosition_Id;
                    NewModel.RoleDescription = dbContext.Job_Positions.Find(item.JobPosition_Id).Description;
                    NewModelList.Add(NewModel);
                }
                return NewModelList;
           // }
        }

        public void Delete_Role(int IdR, int User_IdR)
        {
           // using (SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities())
          //  {
                var DelRole = (from a in dbContext.EmployeeRoles
                               where a.JobPosition_Id == IdR && a.Employee_Id== User_IdR
                               select a).FirstOrDefault();
                dbContext.EmployeeRoles.Remove(DelRole);
                dbContext.SaveChanges();

          //  }
        }
        public void Delete_Service(int Id, int User_Id)
        {
            //using (SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities())
         //   {
                var DelRole = (from a in dbContext.EmployeeServices
                               where a.Problem_Category_Id == Id && a.Employee_Id==User_Id
                               select a).FirstOrDefault();
                dbContext.EmployeeServices.Remove(DelRole);
                dbContext.SaveChanges();
          //  }
        }

        public Employee GetLoggedInEmployee(int userId)
        {
            return  dbContext.Employees.Where(ee => ee.User_Id == userId).Single();
        }
        public int GetFacilityIdByUserID(int UserId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                //return (from p in db.Employees
                //        join c in db.Users on p.User_Id equals c.User_Id
                //       // join i in db.Organizations on p.Organization_Id equals i.Organization_Id
                //        join f in db.apl_Cyca_Facility on p.Organization_Id equals f.Organization_Id
                //        where c.User_Id == (UserId)
                //        select f.Facility_Id).SingleOrDefault();
                return (from f in db.apl_Cyca_Facility
                        join e in db.Employees on f.Facility_Id equals e.Facility_Id
                        join u in db.Users on e.User_Id equals u.User_Id
                        where u.User_Id == UserId
                        select f.Facility_Id).SingleOrDefault();
            }
        }
        public Posted_EmployeeServices PostedEmployeeServiceType { get; set; }
        public Posted_EmployeeRoles PostedEmployeeRoleType { get; set; }
    }
}