using Common_Objects.Models;
using System.Collections.Generic;
using System.Linq;

namespace SDIIS.Common
{
    public class Helpers
    {
        public static void SetAuthorizedRolesVisibility(ref List<Menu_Item> menuItems, List<Role> authorizedRoles)
        {
            foreach (var item in menuItems)
            {
                item.Is_Visible = true;

                if (item.Sub_Menu_Items.Any())
                {
                    var subMenuItems = item.Sub_Menu_Items.ToList();

                    SetAuthorizedRolesVisibility(ref subMenuItems, authorizedRoles);

                    // Set Parent Item Invisible if all SubItems are Invisible
                    item.Is_Visible = item.Sub_Menu_Items.Count(i => i.Is_Visible.Equals(false)) != item.Sub_Menu_Items.Count();
                }
                else
                {
                    var isAuthorized = false;

                    if (item.Module_Action.Roles.Any())
                    {
                        foreach (var role in item.Module_Action.Roles)
                        {
                            if (authorizedRoles.Count(ar => ar.Role_Id.Equals(role.Role_Id)) > 0)
                                isAuthorized = true;
                        }
                    }
                    else
                    {
                        // If no roles are specified for a Menu Item, we assume it's visible to all
                        isAuthorized = true;
                    }

                    item.Is_Visible = isAuthorized;
                }
            }
        }
    }
}