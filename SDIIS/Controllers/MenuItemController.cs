using Common_Objects;
using Common_Objects.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class MenuItemController : Controller
    {
        [CustomAuthorize("Main", "MenuItem", "Index")]
        public ActionResult Index(string SearchItemText, string SearchForMenu, string SearchParentItem)
        {
            var menuItemModel = new MenuItemModel();
            var menuItemList = menuItemModel.GetListOfMenuItems(true, false, SearchItemText, SearchForMenu, SearchParentItem);

            return View(menuItemList);
        }

        [CustomAuthorize("Main", "MenuItem", "Create")]
        public ActionResult Create()
        {
            var newMenuItem = new Menu_Item() { Is_Active = true };

            return View(newMenuItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Menu_Item menuItem)
        {
            if (ModelState.IsValid)
            {
                var menuItemModel = new MenuItemModel();
                var createMenuItem = menuItemModel.CreateMenuItem(menuItem.Menu_Id, menuItem.Menu_Text, menuItem.Menu_Tooltip, menuItem.Module_Action_Id, menuItem.Parent_Menu_Item_Id, menuItem.Is_Active);

                if (createMenuItem == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(menuItem);
                }

                return RedirectToAction("Index", "MenuItem");
            }

            return View(menuItem);
        }

        [CustomAuthorize("Main", "MenuItem", "Edit")]
        public ActionResult Edit(string id)
        {
            var menuItemModel = new MenuItemModel();
            var menuItemToEdit = menuItemModel.GetSpecificMenuItem(int.Parse(id));

            return View(menuItemToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Menu_Item menuItem)
        {
            if (ModelState.IsValid)
            {
                var menuItemModel = new MenuItemModel();

                var updatedMenuItem = menuItemModel.EditMenuItem(menuItem.Menu_Item_Id, menuItem.Menu_Id, menuItem.Menu_Text, menuItem.Menu_Tooltip, menuItem.Module_Action_Id, menuItem.Parent_Menu_Item_Id, menuItem.Is_Active);

                if (updatedMenuItem == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(menuItem);
                }

                return RedirectToAction("Index", "MenuItem");
            }

            return View(menuItem);
        }

        [CustomAuthorize("Main", "MenuItem", "Delete")]
        public ActionResult Delete(string id)
        {
            var menuItemModel = new MenuItemModel();
            var deleteMenuItem = menuItemModel.GetSpecificMenuItem(int.Parse(id));

            return View(deleteMenuItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Menu_Item menuItem)
        {
            var menuItemModel = new MenuItemModel();
            var deletedMenuItem = menuItemModel.SetMenuItemIsDeleted(menuItem.Menu_Item_Id, true);

            if (deletedMenuItem == null)
            {
                ViewBag.Message = "An Error Occured, Contact support";
                return View(menuItem);
            }

            return RedirectToAction("Index", "MenuItem");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetControllersForModule(string moduleId)
        {
            if (String.IsNullOrEmpty(moduleId))
            {
                throw new ArgumentNullException("moduleId");
            }

            var moduleControllerModel = new ModuleControllerModel();
            var controllerList = moduleControllerModel.GetListOfModuleControllers(false, false);

            controllerList.RemoveAll(x => x.Module_Id != int.Parse(moduleId));

            var result = (from c in controllerList
                          select new
                          {
                              id = c.Module_Controller_Id,
                              name = c.Module_Controller_Name
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetActionsForController(string controllerId)
        {
            if (String.IsNullOrEmpty(controllerId))
            {
                throw new ArgumentNullException("countryId");
            }

            var moduleActionModel = new ModuleActionModel();
            var actionList = moduleActionModel.GetListOfModuleActions(false, false);

            actionList.RemoveAll(x => x.Module_Controller_Id != int.Parse(controllerId));

            var result = (from a in actionList
                          select new
                          {
                              id = a.Module_Action_Id,
                              name = a.Module_Action_Name
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        } 
	}
}