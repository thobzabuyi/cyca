using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDIIS.Models
{
    public class EmployeeModel
    {
        public Employee GetSpecificEmployee(int employeeId)
        {
            Employee employee;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employees = (from e in dbContext.Employees
                                 where e.Employee_Id.Equals(employeeId)
                                 select e).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                employee = (from e in employees
                            select e).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return employee;
        }

        public Employee GetSpecificUser(string userName)
        {
            Employee employee;

            var dbContext = new SDIIS_DatabaseEntities();
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
                return null;
            }

            return employee;
        }

        public List<Employee> GetListOfEmployees(bool showInActive, bool showDeleted)
        {
            List<Employee> listOfEmployees;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employees = (from e in dbContext.Employees
                                 where e.Is_Active || e.Is_Active.Equals(!showInActive)
                                 where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
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

        public Employee CreateEmployee(string firstName, string lastName, string persalNumber, int? headOfDepartmentId, int? supervisorId, string phoneNumber, string mobilePhoneNumber, 
            int? genderId, int? raceId, string idNumber, int? jobPositionId, int? payPointId, int? serviceOfficeId, int? salaryLevelId, bool isShiftWorker, bool isCasualWorker, 
            bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
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
                    Is_Active = isActive,
                    Is_Deleted = isDeleted,
                    Date_Created = dateCreated,
                    Created_By = createdBy
                };

                try
                {
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
                    Date_Created = dateCreated,
                    Created_By = createdBy
                };

                try
                {
                    newEmployee = dbContext.Employees.Add(employee);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return newEmployee;
        }

        public Employee EditEmployee(int employeeId, string firstName, string lastName, string persalNumber, int? headOfDepartmentId, int? supervisorId, string phoneNumber, string mobilePhoneNumber, 
            int? genderId, int? raceId, string idNumber, int? jobPositionId, int? payPointId, int? serviceOfficeId, int? salaryLevelId, bool isShiftWorker, bool isCasualWorker, 
            bool isActive, bool isDeleted, DateTime dateLastModified, string modifiedBy)
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
                    
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editEmployee;
        }

        public Employee SetEmployeeIsActive(int employeeId, bool isActive)
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

                    editEmployee.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editEmployee;
        }

        public Employee SetEmployeeIsDeleted(int employeeId, bool isDeleted)
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

                    editEmployee.Is_Deleted = isDeleted;
                    editEmployee.User.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editEmployee;
        }
    }
}