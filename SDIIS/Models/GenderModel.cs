using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDIIS.Models
{
    public class GenderModel
    {
        public Gender GetSpecificGender(int genderId)
        {
            Gender gender;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var genderList = (from g in dbContext.Genders
                                  where g.Gender_Id.Equals(genderId)
                                  select g).ToList();

                gender = (from g in genderList
                          select g).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return gender;
        }

        public List<Gender> GetListOfGenders()
        {
            List<Gender> genders;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var genderList = (from g in dbContext.Genders
                                      select g).ToList();

                    genders = (from g in genderList
                               select g).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return genders;
        }

        //public Menu CreateMenu(string description, bool isActive)
        //{
        //    var dbContext = new SDIIS_DatabaseEntities();

        //    var menu = new Menu() { Description = description, Is_Active = isActive, Is_Deleted = false };

        //    try
        //    {
        //        var newMenu = dbContext.Menus.Add(menu);

        //        dbContext.SaveChanges();

        //        return newMenu;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public Menu EditMenu(int menuId, string description)
        //{
        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var editMenu = (from m in dbContext.Menus
        //                        where m.Menu_Id.Equals(menuId)
        //                        select m).FirstOrDefault();

        //        if (editMenu == null) return null;

        //        editMenu.Description = description;

        //        dbContext.SaveChanges();

        //        return editMenu;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public Menu SetMenuIsActive(int menuId, bool isActive)
        //{
        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var editMenu = (from m in dbContext.Menus
        //                        where m.Menu_Id.Equals(menuId)
        //                        select m).FirstOrDefault();

        //        if (editMenu == null) return null;

        //        editMenu.Is_Active = isActive;

        //        dbContext.SaveChanges();

        //        return editMenu;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public Menu SetMenuIsDeleted(int menuId, bool isDeleted)
        //{
        //    var dbContext = new SDIIS_DatabaseEntities();

        //    try
        //    {
        //        var editMenu = (from m in dbContext.Menus
        //                        where m.Menu_Id.Equals(menuId)
        //                        select m).FirstOrDefault();

        //        if (editMenu == null) return null;

        //        editMenu.Is_Deleted = isDeleted;

        //        dbContext.SaveChanges();

        //        return editMenu;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}