using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDIIS.Models
{
    public class UserModel
    {
        public User DoLogin(string username, string password)
        {
            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var loginAgent = dbContext.Users.Include("Roles").FirstOrDefault(user => user.User_Name.Equals(username) && user.Password.Equals(password));

                return loginAgent;
            }
        }

        public User GetSpecificUser(int userId)
        {
            User user;

            var dbContext = new SDIIS_DatabaseEntities();
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

            var dbContext = new SDIIS_DatabaseEntities();
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

        public List<User> GetListOfUsers(bool showInActive, bool showDeleted)
        {
            List<User> listOfUsers;

            var dbContext = new SDIIS_DatabaseEntities();
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

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
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
            }

            return newUser;
        }

        public User EditUser(int userId, string firstName, string lastName)
        {
            User editUser;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editUser = (from a in dbContext.Users
                                where a.User_Id.Equals(userId)
                                select a).FirstOrDefault();

                    if (editUser == null) return null;

                    editUser.First_Name = firstName;
                    editUser.Last_Name = lastName;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editUser;
        }

        public User SetUserIsActive(int userId, bool isActive)
        {
            User editUser;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
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
            }

            return editUser;
        }

        public User SetUserIsDeleted(int userId, bool isDeleted)
        {
            User editUser;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
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
            }

            return editUser;
        }

        public User AddUserToRole(int agentId, int roleId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

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
            var dbContext = new SDIIS_DatabaseEntities();

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
            var dbContext = new SDIIS_DatabaseEntities();

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
            var dbContext = new SDIIS_DatabaseEntities();

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
            var dbContext = new SDIIS_DatabaseEntities();

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
            var dbContext = new SDIIS_DatabaseEntities();

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
            var dbContext = new SDIIS_DatabaseEntities();

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
            var dbContext = new SDIIS_DatabaseEntities();

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
    }
}