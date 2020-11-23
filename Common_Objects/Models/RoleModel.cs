using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class RoleModel
    {
        public Role GetSpecificRole(int roleId)
        {
            Role role;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var roles = (from r in dbContext.Roles
                                 where r.Role_Id.Equals(roleId)
                                 select r).ToList();

                    //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                    role = (from r in roles
                            select r).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return role;
        }

        public List<Role> GetListOfRoles(bool showInactive, bool showDeleted)
        {
            List<Role> listOfRoles;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {

                try
                {
                    var roles = (from r in dbContext.Roles
                                 where r.Is_Active.Equals(true) || r.Is_Active.Equals(!showInactive)
                                 where r.Is_Deleted.Equals(false) || r.Is_Deleted.Equals(showDeleted)
                                 select r).ToList();

                    listOfRoles = (from r in roles
                                   select r).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return listOfRoles;
        }

        public Role CreateRole(string description, bool isActive)
        {
            Role newRole;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var role = new Role
                {
                    Description = description,
                    Is_Active = isActive,
                    Is_Deleted = false,
                    Date_Created = DateTime.Now,
                };

                try
                {
                    newRole = dbContext.Roles.Add(role);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return newRole;
        }

        public Role EditRole(int roleId, string description)
        {
            Role editRole;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editRole = (from r in dbContext.Roles
                                where r.Role_Id.Equals(roleId)
                                select r).FirstOrDefault();

                    if (editRole == null) return null;

                    editRole.Description = description;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editRole;
        }

        public Role SetRoleIsActive(int roleId, bool isActive)
        {
            Role editRole;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editRole = (from r in dbContext.Roles
                                where r.Role_Id.Equals(roleId)
                                select r).FirstOrDefault();

                    if (editRole == null) return null;

                    editRole.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editRole;
        }

        public Role SetRoleIsDeleted(int roleId, bool isDeleted)
        {
            Role editRole;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editRole = (from r in dbContext.Roles
                                where r.Role_Id.Equals(roleId)
                                select r).FirstOrDefault();

                    if (editRole == null) return null;

                    editRole.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editRole;
        }
    }
}