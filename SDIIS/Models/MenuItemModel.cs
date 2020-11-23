﻿using Common_Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SDIIS.Models
{
    public class MenuItemModel
    {
        public Menu_Item GetSpecificMenuItem(int menuItemId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            Menu_Item menuItem;

            try
            {
                var menuItemsList = (from mi in dbContext.Menu_Items
                                     where mi.Menu_Item_Id.Equals(menuItemId)
                                     select mi).ToList();

                menuItem = PopulateAdditionalItems(menuItemsList, dbContext).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return menuItem;
        }

        public List<Menu_Item> GetListOfMenuItems(bool showInActive, bool showDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            List<Menu_Item> menuItems;

            try
            {
                var menuItemsList = (from mi in dbContext.Menu_Items
                                     where mi.Is_Active.Equals(true) || mi.Is_Active.Equals(!showInActive)
                                     where mi.Is_Deleted.Equals(false) || mi.Is_Deleted.Equals(showDeleted)
                                     select mi).ToList();

                menuItems = PopulateAdditionalItems(menuItemsList, dbContext).ToList();

            }
            catch (Exception)
            {
                return null;
            }

            return menuItems;
        }

        public Menu_Item CreateMenuItem(int menuId, string menuText, string menuTooltip, int? moduleActionId, int? parentMenuItemId, bool isActive)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var menuItem = new Menu_Item() { Menu_Id = menuId, Menu_Text = menuText, Menu_Tooltip = menuTooltip, Module_Action_Id = moduleActionId, Parent_Menu_Item_Id = parentMenuItemId, Is_Active = isActive, Is_Deleted = false, Date_Created = DateTime.Now };

            try
            {
                var newMenuItem = dbContext.Menu_Items.Add(menuItem);

                dbContext.SaveChanges();

                return newMenuItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Menu_Item EditMenuItem(int menuItemId, int menuId, string menuText, string menuTooltip, int? moduleActionId, int? parentMenuItemId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editMenuItem = (from mi in dbContext.Menu_Items
                                    where mi.Menu_Item_Id.Equals(menuItemId)
                                    select mi).FirstOrDefault();

                if (editMenuItem == null) return null;

                editMenuItem.Menu_Id = menuId;
                editMenuItem.Menu_Text = menuText;
                editMenuItem.Menu_Tooltip = menuTooltip;
                editMenuItem.Module_Action_Id = moduleActionId;
                editMenuItem.Parent_Menu_Item_Id = parentMenuItemId;

                dbContext.SaveChanges();

                return editMenuItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Menu_Item SetMenuItemIsActive(int menuItemId, bool isActive)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editMenuItem = (from mi in dbContext.Menu_Items
                                    where mi.Menu_Item_Id.Equals(menuItemId)
                                    select mi).FirstOrDefault();

                if (editMenuItem == null) return null;

                editMenuItem.Is_Active = isActive;

                dbContext.SaveChanges();

                return editMenuItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Menu_Item SetMenuItemIsDeleted(int menuItemId, bool isDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editMenuItem = (from mi in dbContext.Menu_Items
                                    where mi.Menu_Item_Id.Equals(menuItemId)
                                    select mi).FirstOrDefault();

                if (editMenuItem == null) return null;

                editMenuItem.Is_Deleted = isDeleted;

                dbContext.SaveChanges();

                return editMenuItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region Helpers

        private IEnumerable<Menu_Item> PopulateAdditionalItems(IEnumerable<Menu_Item> listOfMenuItems, SDIIS_DatabaseEntities dbContext)
        {
            var populatedMenuItems = new List<Menu_Item>();

            populatedMenuItems = (from a in listOfMenuItems
                                  select a.Set(a1 =>
                                  {
                                      a1.Module_Action_Id = a.Module_Action == null ? null : (int?)a.Module_Action.Module_Action_Id;
                                      a1.Module_Controller_Id = a.Module_Action == null ? null : (int?)a.Module_Action.Module_Controller_Id;
                                  })).ToList();

            return populatedMenuItems;
        }

        #endregion
    }
}