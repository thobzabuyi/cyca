using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Common_Objects.ViewModels;

namespace Common_Objects.Models
{
    public class SocialWorkerModel
    {
        private SDIIS_DatabaseEntities dbContext_1 = new SDIIS_DatabaseEntities();
        public Social_Worker GetSpecificSocialWorker(int employeeId)
        {
            Social_Worker employee;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employees = (from e in dbContext.Social_Workers
                                 where e.Social_Worker_Id.Equals(employeeId)
                                 select e).ToList();

                employee = (from e in employees
                            select e).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return employee;
        }

        public Social_Worker GetSpecificUser(string userName)
        {
            Social_Worker employee;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employees = (from e in dbContext.Social_Workers
                                 where e.apl_User.User_Name.Equals(userName)
                                 select e).ToList();

                employee = (from e in employees
                            select e).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return employee;
        }

        public List<Social_Worker> GetListOfSocialWorkers(bool showInActive, bool showDeleted)
        {
            List<Social_Worker> listOfSocialWorkers;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employees = (from e in dbContext.Social_Workers
                                 where e.Is_Active || e.Is_Active.Equals(!showInActive)
                                 where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                                 select e).ToList();

                listOfSocialWorkers = (from e in employees
                                       select e).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfSocialWorkers;

        }

        public List<Social_Worker> GetListOfSocialWorkers(bool showInActive, bool showDeleted, int serviceOfficeId)
        {
            List<Social_Worker> listOfSocialWorkers;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var employees = (from e in dbContext.Social_Workers
                                 where e.Is_Active || e.Is_Active.Equals(!showInActive)
                                 where !e.Is_Deleted || e.Is_Deleted.Equals(showDeleted)
                                 where e.Service_Office_Id.Equals(serviceOfficeId)
                                 select e).ToList();

                listOfSocialWorkers = (from e in employees
                                       select e).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfSocialWorkers;

        }

        public Social_Worker CreateSocialWorker(string firstName, string lastName, string initials, string emailAddress, string phoneNumber, string mobilePhoneNumber,
            int? genderId, int? raceId, string idNumber, bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
        {
            Social_Worker newSocialWorker;

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

                var employee = new Social_Worker
                {
                    User_Id = newUser.User_Id,
                    Phone_Number = phoneNumber,
                    Mobile_Phone_Number = mobilePhoneNumber,
                    Gender_Id = genderId,
                    Race_Id = raceId,
                    ID_Number = idNumber,
                    Is_Active = isActive,
                    Is_Deleted = isDeleted,
                    Date_Created = dateCreated,
                    Created_By = createdBy
                };

                try
                {
                    newSocialWorker = dbContext.Social_Workers.Add(employee);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return newSocialWorker;
        }

        public Social_Worker EditSocialWorker(int socialWorkerId, string firstName, string lastName, string initials, string emailAddress, string phoneNumber, string mobilePhoneNumber,
            int? genderId, int? raceId, string idNumber, bool isActive, bool isDeleted, DateTime dateLastModified, string modifiedBy)
        {
            Social_Worker editSocialWorker;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSocialWorker = (from e in dbContext.Social_Workers
                                        where e.Social_Worker_Id.Equals(socialWorkerId)
                                        select e).FirstOrDefault();

                    if (editSocialWorker == null) return null;

                    editSocialWorker.apl_User.First_Name = firstName;
                    editSocialWorker.apl_User.Last_Name = lastName;
                    editSocialWorker.apl_User.Initials = initials;
                    editSocialWorker.apl_User.Email_Address = emailAddress;
                    editSocialWorker.Phone_Number = phoneNumber;
                    editSocialWorker.Mobile_Phone_Number = mobilePhoneNumber;
                    editSocialWorker.Gender_Id = genderId;
                    editSocialWorker.Race_Id = raceId;
                    editSocialWorker.ID_Number = idNumber;
                    editSocialWorker.Is_Active = isActive;
                    editSocialWorker.Is_Deleted = isDeleted;
                    editSocialWorker.Date_Last_Modified = dateLastModified;
                    editSocialWorker.Modified_By = modifiedBy;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSocialWorker;
        }

        public Social_Worker SetSocialWorkerIsActive(int employeeId, bool isActive)
        {
            Social_Worker editSocialWorker;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSocialWorker = (from e in dbContext.Social_Workers
                                        where e.Social_Worker_Id.Equals(employeeId)
                                        select e).FirstOrDefault();

                    if (editSocialWorker == null) return null;

                    editSocialWorker.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSocialWorker;
        }

        public Social_Worker SetSocialWorkerIsDeleted(int employeeId, bool isDeleted)
        {
            Social_Worker editSocialWorker;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSocialWorker = (from e in dbContext.Social_Workers
                                        where e.Social_Worker_Id.Equals(employeeId)
                                        select e).FirstOrDefault();

                    if (editSocialWorker == null) return null;

                    editSocialWorker.Is_Deleted = isDeleted;
                    editSocialWorker.apl_User.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSocialWorker;
        }

        public Social_Worker AllocateToIncidents(int socialWorkerId, List<int> incidentIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var socialWorkerToAlloc = dbContext.Social_Workers.FirstOrDefault(x => x.Social_Worker_Id.Equals(socialWorkerId));
                if (socialWorkerToAlloc == null) return null;

                foreach (var incidentId in incidentIds)
                {
                    var incidentToEdit = dbContext.CPR_Incidents.FirstOrDefault(x => x.Incident_Id.Equals(incidentId));
                    if (incidentToEdit == null) return null;

                    incidentToEdit.Assigned_Social_Worker_Id = socialWorkerToAlloc.User_Id;
                }

                dbContext.SaveChanges();

                return socialWorkerToAlloc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Social_Worker DeallocateFromIncidents(List<int> incidentIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                foreach (var incidentId in incidentIds)
                {
                    var incidentToEdit = dbContext.CPR_Incidents.FirstOrDefault(x => x.Incident_Id.Equals(incidentId));
                    if (incidentToEdit == null) return null;

                    incidentToEdit.Assigned_Social_Worker_Id = null;
                }

                dbContext.SaveChanges();

                return new Social_Worker();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Social_Worker TransferAllocatedCases(int fromSocialWorkerId, int toSocialWorkerId, List<int> incidentIds )
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var socialWorkerFrom = dbContext.Social_Workers.FirstOrDefault(x => x.Social_Worker_Id.Equals(fromSocialWorkerId));
                if (socialWorkerFrom == null) return null;

                var socialWorkerTo = dbContext.Social_Workers.FirstOrDefault(x => x.Social_Worker_Id.Equals(toSocialWorkerId));
                if (socialWorkerTo == null) return null;

                foreach (var incidentId in incidentIds)
                {
                    var incidentToEdit = dbContext.CPR_Incidents.FirstOrDefault(x => x.Incident_Id.Equals(incidentId) && x.Assigned_Social_Worker_Id == fromSocialWorkerId);
                    if (incidentToEdit == null) return null;

                    incidentToEdit.Assigned_Social_Worker_Id = socialWorkerTo.Social_Worker_Id;
                }

                dbContext.SaveChanges();

                return socialWorkerFrom;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IQueryable<Social_Worker> GetSocialWorkerList(int id)
        {

            return from c in dbContext_1.Social_Workers
                   join b in dbContext_1.Users on c.User_Id equals b.User_Id
                   where c.Organization_Id == id
                   select c;
        }
    }
}
