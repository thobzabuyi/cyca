using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class GroupModel
    {
        public Group GetSpecificGroup(int groupId)
        {
            Group group;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var groupsList = (from g in dbContext.Groups
                                  where g.Group_Id.Equals(groupId)
                                  select g).ToList();

                group = (from g in groupsList
                         select g).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return group;
        }

        public List<Group> GetListOfGroups(bool showInActive, bool showDeleted, string SearchDescription)
        {
            List<Group> groups;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var groupsList = (from g in dbContext.Groups
                                      where g.Is_Active.Equals(true) || g.Is_Active.Equals(!showInActive)
                                      where g.Is_Deleted.Equals(false) || g.Is_Deleted.Equals(showDeleted)
                                      select g).ToList();
                    if (SearchDescription != null)
                    {
                        groupsList = (from g in dbContext.Groups
                                      where g.Is_Active.Equals(true) || g.Is_Active.Equals(!showInActive)
                                      where g.Is_Deleted.Equals(false) || g.Is_Deleted.Equals(showDeleted)
                                      where g.Description.Contains(SearchDescription)
                                      select g).ToList();
                    }

                    groups = (from g in groupsList
                              select g).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return groups;
        }


        public List<Group> GetListOfGroups(bool showInActive, bool showDeleted)
        {
            List<Group> groups;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var groupsList = (from g in dbContext.Groups
                                      where g.Is_Active.Equals(true) || g.Is_Active.Equals(!showInActive)
                                      where g.Is_Deleted.Equals(false) || g.Is_Deleted.Equals(showDeleted)
                                      select g).ToList();

                    groups = (from g in groupsList
                              select g).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return groups;
        }

        public Group CreateGroup(string description, bool isActive)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var group = new Group() { Description = description, Is_Active = isActive, Is_Deleted = false, Date_Created = DateTime.Now };

            try
            {
                var newGroup = dbContext.Groups.Add(group);

                dbContext.SaveChanges();

                return newGroup;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Group EditGroup(int groupId, string description)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editGroup = (from g in dbContext.Groups
                                 where g.Group_Id.Equals(groupId)
                                 select g).FirstOrDefault();

                if (editGroup == null) return null;

                editGroup.Description = description;

                dbContext.SaveChanges();

                return editGroup;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Group AddGroupToRole(int groupId, int roleId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var roleToAdd = dbContext.Roles.FirstOrDefault(x => x.Role_Id.Equals(roleId));
                if (roleToAdd == null) return null;

                var groupToEdit = dbContext.Groups.FirstOrDefault(x => x.Group_Id.Equals(groupId));
                if (groupToEdit == null) return null;

                groupToEdit.Roles.Add(roleToAdd);

                dbContext.SaveChanges();

                return groupToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Group AddGroupToRole(int groupId, List<int> roleIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var groupToEdit = dbContext.Groups.FirstOrDefault(x => x.Group_Id.Equals(groupId));
                if (groupToEdit == null) return null;

                groupToEdit.Roles.Clear();

                foreach (var roleId in roleIds)
                {
                    var roleToAdd = dbContext.Roles.FirstOrDefault(x => x.Role_Id.Equals(roleId));
                    if (roleToAdd == null) return null;

                    groupToEdit.Roles.Add(roleToAdd);
                }

                dbContext.SaveChanges();

                return groupToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Group RemoveMenuItemFromRole(int groupId, int roleId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var roleToRemove = dbContext.Roles.FirstOrDefault(x => x.Role_Id.Equals(roleId));
                if (roleToRemove == null) return null;

                var groupToEdit = dbContext.Groups.FirstOrDefault(x => x.Group_Id.Equals(groupId));
                if (groupToEdit == null) return null;

                groupToEdit.Roles.Remove(roleToRemove);

                dbContext.SaveChanges();

                return groupToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Group ClearMenuItemRoles(int groupId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var groupToEdit = dbContext.Groups.FirstOrDefault(x => x.Group_Id.Equals(groupId));
                if (groupToEdit == null) return null;

                groupToEdit.Roles.Clear();

                dbContext.SaveChanges();

                return groupToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Group SetGroupIsActive(int groupId, bool isActive)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editGroup = (from g in dbContext.Groups
                                 where g.Group_Id.Equals(groupId)
                                 select g).FirstOrDefault();

                if (editGroup == null) return null;

                editGroup.Is_Active = isActive;

                dbContext.SaveChanges();

                return editGroup;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Group SetGroupIsDeleted(int groupId, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editGroup = (from g in dbContext.Groups
                                 where g.Group_Id.Equals(groupId)
                                 select g).FirstOrDefault();

                if (editGroup == null) return null;

                editGroup.Is_Deleted = isDeleted;

                dbContext.SaveChanges();

                return editGroup;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}