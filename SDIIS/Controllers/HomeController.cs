using Common_Objects;
using Common_Objects.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace SDIIS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [CustomAuthorize("Main", "Home", "Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MenuLayout()
        {
            var currentUser = new User();

            if ((Session["CurrentUser"] == null) && (Request.Cookies[FormsAuthentication.FormsCookieName] != null))
            {
                var authUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

                var userModel = new UserModel();
                currentUser = userModel.GetSpecificUser(authUser);

                Session.Remove("CurrentUser");
                Session.Remove("MenuLayout");
                Session.Add("CurrentUser", currentUser);
            }
            else
            {
                if (Session["CurrentUser"] != null)
                {
                    var loggedInUser = (User)Session["CurrentUser"];

                    var userModel = new UserModel();
                    currentUser = userModel.GetSpecificUser(loggedInUser.User_Id);
                }
            }

            var renderMenu = new Menu();

            if (Session["MenuLayout"] == null)
            {
                var menuModel = new MenuModel();
                var menu = menuModel.GetSpecificMenu((int)MenuContainerEnum.MainMenu);

                var authorizedRoles = new List<Role>();

                authorizedRoles = currentUser.Roles.ToList();

                if (currentUser.Groups.Any())
                {
                    foreach (var group in currentUser.Groups)
                    {
                        var groupRoles = group.Roles.Select(r => r).ToList();
                        authorizedRoles.AddRange(groupRoles);
                    }
                }

                // TODO Add delegation here as well

                var effectiveRoles = authorizedRoles.Distinct().ToList();

                var menuItems = menu.Menu_Items.Where(x => x.Is_Active != false).ToList();

                Helpers.SetAuthorizedRolesVisibility(ref menuItems, effectiveRoles);

                renderMenu = menu.Menu_Items.Any() ? new Menu { Menu_Items = new List<Menu_Item>(menuItems) } : null;

                Session["MenuLayout"] = renderMenu;
            }
            else
            {
                renderMenu = (Menu)Session["MenuLayout"];
            }

            return PartialView("_MenuLayout", renderMenu);
        }
    }
}