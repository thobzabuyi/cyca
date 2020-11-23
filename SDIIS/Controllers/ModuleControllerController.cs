using Common_Objects;
using Common_Objects.Models;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class ModuleControllerController : Controller
    {
        [CustomAuthorize("Main", "ModuleController", "Index")]
        public ActionResult Index(string SearchControllerName, string SearchModule)
        {
            var moduleControllerModel = new ModuleControllerModel();
            var moduleControllerList = moduleControllerModel.GetListOfModuleControllers(true, false, SearchControllerName, SearchModule);

            return View(moduleControllerList);
        }

        [CustomAuthorize("Main", "ModuleController", "Create")]
        public ActionResult Create()
        {
            var newModuleController = new Module_Controller();

            return View(newModuleController);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Module_Controller moduleController)
        {
            if (ModelState.IsValid)
            {
                var moduleControllerModel = new ModuleControllerModel();
                var createModuleController = moduleControllerModel.CreateModuleController(moduleController.Module_Id, moduleController.Module_Controller_Name);

                if (createModuleController == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(moduleController);
                }

                return RedirectToAction("Index", "ModuleController");
            }

            return View(moduleController);
        }

        [CustomAuthorize("Main", "ModuleController", "Edit")]
        public ActionResult Edit(string id)
        {
            var moduleControllerModel = new ModuleControllerModel();
            var moduleControllerToEdit = moduleControllerModel.GetSpecificModuleController(int.Parse(id));

            return View(moduleControllerToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Module_Controller moduleController)
        {
            if (ModelState.IsValid)
            {
                var moduleControllerModel = new ModuleControllerModel();

                var updatedModuleController = moduleControllerModel.EditModuleController(moduleController.Module_Controller_Id, moduleController.Module_Id, moduleController.Module_Controller_Name);

                if (updatedModuleController == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(moduleController);
                }

                return RedirectToAction("Index", "ModuleController");
            }

            return View(moduleController);
        }
	}
}