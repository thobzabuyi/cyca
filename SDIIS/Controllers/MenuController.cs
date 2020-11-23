using Common_Objects;
using Common_Objects.Models;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class MenuController : Controller
    {
        [CustomAuthorize("Main", "Menu", "Index")]
        public ActionResult Index(string SearchDescription)
        {
            var menuModel = new MenuModel();
            var menuList = menuModel.GetListOfMenus(false, false, SearchDescription);

            return View(menuList);
        }

        [CustomAuthorize("Main", "Menu", "Create")]
        public ActionResult Create()
        {
            var newMenu = new Menu() { Is_Active = true };

            return View(newMenu);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                var menuModel = new MenuModel();
                var createMenu = menuModel.CreateMenu(menu.Description, menu.Is_Active);

                if (createMenu == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(menu);
                }

                return RedirectToAction("Index", "Menu");
            }

            return View(menu);
        }

        [CustomAuthorize("Main", "Menu", "Edit")]
        public ActionResult Edit(string id)
        {
            var menuModel = new MenuModel();
            var menuToEdit = menuModel.GetSpecificMenu(int.Parse(id));

            return View(menuToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                var menuModel = new MenuModel();

                var updatedMenu = menuModel.EditMenu(menu.Menu_Id, menu.Description);

                if (updatedMenu == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(menu);
                }

                return RedirectToAction("Index", "Menu");
            }

            return View(menu);
        }

        [CustomAuthorize("Main", "Menu", "Delete")]
        public ActionResult Delete(string id)
        {
            var menuModel = new MenuModel();
            var deleteMenu = menuModel.GetSpecificMenu(int.Parse(id));

            return View(deleteMenu);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Menu menu)
        {
            var menuModel = new MenuModel();
            var deletedMenu = menuModel.SetMenuIsDeleted(menu.Menu_Id, true);

            if (deletedMenu == null)
            {
                ViewBag.Message = "An Error Occured, Contact support";
                return View(menu);
            }

            return RedirectToAction("Index", "Menu");
        }
	}
}