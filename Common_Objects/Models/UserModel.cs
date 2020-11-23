using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Principal;
using Common_Objects.Models;

namespace Common_Objects.Models
{



    public class UserModel
    {
        private SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities(); 
        public User DoLogin(string username, string password)
        {

            var loginAgent = dbContext.Users.Include("Roles").FirstOrDefault(user => user.User_Name.Equals(username) && user.Password.Equals(password));

            return loginAgent;
        }
        public User GetSpecificUser(int userId)
        {
            User user;

            try
            {
                // Don't lazy-load roles.
                var users = (from u in dbContext.Users.Include("Roles").Include("Groups")
                             where u.User_Id.Equals(userId)
                             select u).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                user = (from u in users
                        select u).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return user;
        }
        public User GetSpecificUser(string userName)
        {
            User user;

            try
            {
                // Don't lazy-load roles.
                var users = (from u in dbContext.Users.Include("Roles").Include("Groups").Include("Groups.Roles")
                             where u.User_Name.Equals(userName)
                             select u).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                user = (from u in users
                        select u).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return user;
        }

        public User GetUserByGroupingID(int groupingId)
        {
            return dbContext.Users.Where(uu => uu.User_Id == groupingId).Single();

        }
        public List<User> GetUserList()
        {
            return dbContext.Users.ToList();
        }
            public List<User> getUserCYCAList()
        {
           

            return  dbContext.Roles.Where(r => r.Description.Contains("CYCA"))                
                             .SelectMany(x => x.Users)
                             .Distinct()
                             .ToList();

           
        }

        public List<User> GetUserListById(int userId)
        {
            var users = new List<User>();
            var facilityId = GetFacilityIdByUserId(userId);
           

            var roles =  dbContext.Roles
                            .Where(r => r.Description.Contains("CYCA"))
                            .SelectMany(x => x.Users.Where(u => u.User_Id != userId))
                            .Distinct()
                            .ToList();

            foreach (var item in roles)
            {
                var emp = dbContext.Employees.Where(e => e.User_Id == item.User_Id && e.Facility_Id == facilityId).SingleOrDefault();
                if (emp != null)
                {
                    users.Add(item);
                }
            }
            return users;
        }

        public List<User> GetListOfUsers(bool showInActive, bool showDeleted)
        {
            List<User> listOfUsers;

            try
            {
                var users = (from u in dbContext.Users
                             where u.Is_Active.Equals(true) || u.Is_Active.Equals(!showInActive)
                             where u.Is_Deleted.Equals(false) || u.Is_Deleted.Equals(showDeleted)
                             select u).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfUsers = (from u in users
                               select u).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfUsers;

        }
       
        public User CreateUser(string firstName, string lastName, bool isActive)
        {
            User newUser;


            // TODO: Generate unique username
            // TODO: Generate password

            var user = new User
            {
                User_Name = string.Format("{0}{1}", firstName.Substring(0, 1), lastName),
                Password = "password1",
                First_Name = firstName,
                Last_Name = lastName,
                Is_Active = isActive,
                Is_Deleted = false,
                Date_Created = DateTime.Now,
                
            };

            try
            {
                newUser = dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newUser;
        }

        public User EditUser(int userId, string firstName, string lastName, string initials, string emailAddress)
        {
            User editUser;


            try
            {
                editUser = (from a in dbContext.Users
                            where a.User_Id.Equals(userId)
                            select a).FirstOrDefault();

                if (editUser == null) return null;

                editUser.First_Name = firstName;
                editUser.Last_Name = lastName;
                editUser.Initials = initials;
                editUser.Email_Address = emailAddress;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editUser;
        }

        public User ChangeUserPassword(int userId, string newPassword)
        {
            User editUser;


            try
            {
                editUser = (from a in dbContext.Users
                            where a.User_Id.Equals(userId)
                            select a).FirstOrDefault();

                if (editUser == null) return null;

                editUser.Password = newPassword;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editUser;
        }

        public User SetUserIsActive(int userId, bool isActive)
        {
            User editUser;


            try
            {
                editUser = (from a in dbContext.Users
                            where a.User_Id.Equals(userId)
                            select a).FirstOrDefault();

                if (editUser == null) return null;

                editUser.Is_Active = isActive;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editUser;
        }

        public User SetUserIsDeleted(int userId, bool isDeleted)
        {
            User editUser;


            try
            {
                editUser = (from a in dbContext.Users
                            where a.User_Id.Equals(userId)
                            select a).FirstOrDefault();

                if (editUser == null) return null;

                editUser.Is_Deleted = isDeleted;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editUser;
        }

        public User AddUserToRole(int agentId, int roleId)
        {

            try
            {
                var roleToAdd = dbContext.Roles.FirstOrDefault(x => x.Role_Id.Equals(roleId));
                if (roleToAdd == null) return null;

                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Roles.Add(roleToAdd);

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User AddUserToRole(int agentId, List<int> roleIds)
        {

            try
            {
                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Roles.Clear();

                foreach (var roleId in roleIds)
                {
                    var roleToAdd = dbContext.Roles.FirstOrDefault(x => x.Role_Id.Equals(roleId));
                    if (roleToAdd == null) return null;

                    userToEdit.Roles.Add(roleToAdd);
                }

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User RemoveUserFromRole(int agentId, int roleId)
        {

            try
            {
                var roleToRemove = dbContext.Roles.FirstOrDefault(x => x.Role_Id.Equals(roleId));
                if (roleToRemove == null) return null;

                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Roles.Remove(roleToRemove);

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User ClearUserRoles(int agentId)
        {

            try
            {
                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Roles.Clear();

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User AddUserToGroup(int agentId, int groupId)
        {

            try
            {
                var groupToAdd = dbContext.Groups.FirstOrDefault(x => x.Group_Id.Equals(groupId));
                if (groupToAdd == null) return null;

                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Groups.Add(groupToAdd);

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User AddUserToGroup(int agentId, List<int> groupIds)
        {

            try
            {
                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Groups.Clear();

                foreach (var groupId in groupIds)
                {
                    var groupToAdd = dbContext.Groups.FirstOrDefault(x => x.Group_Id.Equals(groupId));
                    if (groupToAdd == null) return null;

                    userToEdit.Groups.Add(groupToAdd);
                }

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User RemoveUserFromGroup(int agentId, int groupId)
        {

            try
            {
                var groupToRemove = dbContext.Groups.FirstOrDefault(x => x.Group_Id.Equals(groupId));
                if (groupToRemove == null) return null;

                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Groups.Remove(groupToRemove);

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User ClearUserGroups(int agentId)
        {
            try
            {
                var userToEdit = dbContext.Users.FirstOrDefault(x => x.User_Id.Equals(agentId));
                if (userToEdit == null) return null;

                userToEdit.Groups.Clear();

                dbContext.SaveChanges();

                return userToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //get loggedin user facility
        public string GetFacilityByUserId(int userId)
        {

                return (from f in dbContext.apl_Cyca_Facility                        
                        join e in dbContext.Employees on f.Facility_Id equals e.Facility_Id
                        join u in dbContext.Users on e.User_Id equals u.User_Id
                        where u.User_Id == userId
                        select f.FacilityName).FirstOrDefault();

        }

        public int GetFacilityIdByUserId(int userId)
        {

            return (from f in dbContext.apl_Cyca_Facility
                    join e in dbContext.Employees on f.Facility_Id equals e.Facility_Id
                    join u in dbContext.Users on e.User_Id equals u.User_Id
                    where u.User_Id == userId
                    select f.Facility_Id).FirstOrDefault();

        }
    }
}