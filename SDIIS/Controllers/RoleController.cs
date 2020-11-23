using Common_Objects;
using Common_Objects.Models;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class RoleController : Controller
    {
        [CustomAuthorize("Main", "Role", "Index")]
        public ActionResult Index()
        {
            var roleModel = new RoleModel();
            var roleList = roleModel.GetListOfRoles(true, false);

            return View(roleList);
        }

        [CustomAuthorize("Main", "Role", "Create")]
        public ActionResult Create()
        {
            var newRole = new Role() { Is_Active = true };

            return View(newRole);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var roleModel = new RoleModel();
                var createRole = roleModel.CreateRole(role.Description, role.Is_Active);

                if (createRole == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(role);
                }

                return RedirectToAction("Index", "Role");
            }

            return View(role);
        }

        [CustomAuthorize("Main", "Role", "Edit")]
        public ActionResult Edit(string id)
        {
            var roleModel = new RoleModel();
            var roleToEdit = roleModel.GetSpecificRole(int.Parse(id));

            return View(roleToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                var roleModel = new RoleModel();

                var updatedRole = roleModel.EditRole(role.Role_Id, role.Description);

                if (updatedRole == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(role);
                }

                return RedirectToAction("Index", "Role");
            }

            return View(role);
        }

        [CustomAuthorize("Main", "Role", "Delete")]
        public ActionResult Delete(string id)
        {
            var roleModel = new RoleModel();
            var deleteRole = roleModel.GetSpecificRole(int.Parse(id));

            return View(deleteRole);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Role role)
        {
            var roleModel = new RoleModel();
            var deletedRole = roleModel.SetRoleIsDeleted(role.Role_Id, true);

            if (deletedRole == null)
            {
                ViewBag.Message = "An Error Occured, Contact support";
                return View(role);
            }

            return RedirectToAction("Index", "Role");
        }
	}
}