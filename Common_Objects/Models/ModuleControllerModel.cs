using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ModuleControllerModel
    {
        public Module_Controller GetSpecificModuleController(int moduleControllerId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            Module_Controller moduleController;

            try
            {
                var moduleControllersList = (from mc in dbContext.Module_Controllers
                                             where mc.Module_Controller_Id.Equals(moduleControllerId)
                                             select mc).ToList();

                moduleController = (from mc in moduleControllersList
                                    select mc).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return moduleController;
        }

        public List<Module_Controller> GetListOfModuleControllers(bool showInActive, bool showDeleted, string SearchControllerName, string SearchModule)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            List<Module_Controller> moduleControllers;

            try
            {
                var moduleControllersList = (from mc in dbContext.Module_Controllers
                                             select mc).ToList();

                if (SearchControllerName != null || SearchModule != null)
                {
                    moduleControllersList = (from mc in dbContext.Module_Controllers
                                             where mc.Module_Controller_Name.Contains(SearchControllerName)
                                             where mc.Module.Description.Contains(SearchModule)
                                             select mc).ToList();
                }

                moduleControllers = (from moduleController in moduleControllersList
                                     select moduleController).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return moduleControllers;
        }

        public List<Module_Controller> GetListOfModuleControllers(bool showInActive, bool showDeleted)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            List<Module_Controller> moduleControllers;

            try
            {
                var moduleControllersList = (from mc in dbContext.Module_Controllers
                                             select mc).ToList();

                moduleControllers = (from moduleController in moduleControllersList
                                     select moduleController).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return moduleControllers;
        }

        public Module_Controller CreateModuleController(int moduleId, string moduleControllerName)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var moduleController = new Module_Controller() { Module_Id = moduleId, Module_Controller_Name = moduleControllerName };

            try
            {
                var newModuleController = dbContext.Module_Controllers.Add(moduleController);

                dbContext.SaveChanges();

                return newModuleController;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Module_Controller EditModuleController(int moduleControllerId, int moduleId, string moduleControllerName)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editModuleController = (from mc in dbContext.Module_Controllers
                                            where mc.Module_Controller_Id.Equals(moduleControllerId)
                                            select mc).FirstOrDefault();

                if (editModuleController == null) return null;

                editModuleController.Module_Controller_Id = moduleControllerId;
                editModuleController.Module_Controller_Name = moduleControllerName;

                dbContext.SaveChanges();

                return editModuleController;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}