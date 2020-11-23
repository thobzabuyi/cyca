using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ModuleModel
    {
        public Module GetSpecificModule(int moduleId)
        {
            Module module;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var moduleItems = (from m in dbContext.Modules
                                   where m.Module_Id.Equals(moduleId)
                                   select m).ToList();

                // Set Additional properties
                module = (from m in moduleItems
                          select m).FirstOrDefault();
            }
            catch (Exception)
            {
                module = null;
            }

            return module;
        }

        public Module GetSpecificModule(string moduleDescription)
        {
            Module module;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var moduleItems = (from m in dbContext.Modules
                                   where m.Description.Equals(moduleDescription)
                                   select m).ToList();

                // Set Additional properties
                module = (from m in moduleItems
                          select m).FirstOrDefault();
            }
            catch (Exception ex)
            {
                module = null;
            }

            return module;
        }

        public List<Module> GetListOfModules(bool showInActive, bool showDeleted, string SearchModuleDescription)
        {
            List<Module> modules;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var modulesList = (from m in dbContext.Modules
                                   where m.Is_Active.Equals(true) || m.Is_Active.Equals(!showInActive)
                                   where m.Is_Deleted.Equals(false) || m.Is_Deleted.Equals(showDeleted)
                                   select m).ToList();

                if(SearchModuleDescription!=null)
                    {
                    modulesList = (from m in dbContext.Modules
                                   where m.Is_Active.Equals(true) || m.Is_Active.Equals(!showInActive)
                                   where m.Is_Deleted.Equals(false) || m.Is_Deleted.Equals(showDeleted)
                                   where m.Description.Contains(SearchModuleDescription)
                                   select m).ToList();
                    }
                modules = (from module in modulesList
                           select module).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return modules;
        }

        public List<Module> GetListOfModules(bool showInActive, bool showDeleted)
        {
            List<Module> modules;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var modulesList = (from m in dbContext.Modules
                                   where m.Is_Active.Equals(true) || m.Is_Active.Equals(!showInActive)
                                   where m.Is_Deleted.Equals(false) || m.Is_Deleted.Equals(showDeleted)
                                   select m).ToList();

                modules = (from module in modulesList
                           select module).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return modules;
        }

        public Module CreateModule(string description, string baseUrl, bool isActive)
        {
            Module newModule;

            var dbContext = new SDIIS_DatabaseEntities();

            var module = new Module() { Description = description, Base_URL = baseUrl, Is_Active = isActive, Is_Deleted = false, Date_Created = DateTime.Now };

            try
            {
                newModule = dbContext.Modules.Add(module);

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                newModule = null;
            }

            return newModule;
        }

        public Module EditModule(int moduleId, string description, string baseUrl)
        {
            Module editModule;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editModule = (from m in dbContext.Modules
                              where m.Module_Id.Equals(moduleId)
                              select m).FirstOrDefault();

                if (editModule == null) return null;

                editModule.Description = description;
                editModule.Base_URL = baseUrl;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                editModule = null;
            }

            return editModule;
        }

        public Module SetModuleIsActive(int moduleId, bool isActive)
        {
            Module editModule;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editModule = (from m in dbContext.Modules
                              where m.Module_Id.Equals(moduleId)
                              select m).FirstOrDefault();

                if (editModule == null) return null;

                editModule.Is_Active = isActive;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                editModule = null;
            }

            return editModule;
        }

        public Module SetModuleIsDeleted(int moduleId, bool isDeleted)
        {
            Module editModule;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editModule = (from m in dbContext.Modules
                              where m.Module_Id.Equals(moduleId)
                              select m).FirstOrDefault();

                if (editModule == null) return null;

                editModule.Is_Deleted = isDeleted;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                editModule = null;
            }

            return editModule;
        }
    }
}