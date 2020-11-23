using System.Collections.Generic;
using System.Web.Mvc;
using Common_Objects;
using Common_Objects.Models;

namespace SDIIS.Controllers
{
    public class GroupController : Controller
    {
        [CustomAuthorize("Main", "Group", "Index")]
        public ActionResult Index(string SearchDescription)
        {
            var groupModel = new GroupModel();
            var groupList = groupModel.GetListOfGroups(true, false, SearchDescription);

            return View(groupList);
        }

        [CustomAuthorize("Main", "Group", "Create")]
        public ActionResult Create()
        {
            var newGroup = new Group() { Is_Active = true };

            return View(newGroup);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                var groupModel = new GroupModel();
                var createGroup = groupModel.CreateGroup(group.Description, group.Is_Active);

                if (createGroup == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(group);
                }

                // Link selected roles
                var roleIds = new List<int>();
                if (group.Posted_Roles != null)
                {
                    foreach (var roleId in group.Posted_Roles.Role_IDs)
                    {
                        var roleIdValue = int.Parse(roleId);
                        roleIds.Add(roleIdValue);
                    }
                }
                groupModel.AddGroupToRole(createGroup.Group_Id, roleIds);

                return RedirectToAction("Index", "Group");
            }

            return View(group);
        }

        [CustomAuthorize("Main", "Group", "Edit")]
        public ActionResult Edit(string id)
        {
            var groupModel = new GroupModel();
            var groupToEdit = groupModel.GetSpecificGroup(int.Parse(id));

            return View(groupToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                var groupModel = new GroupModel();

                var updatedGroup = groupModel.EditGroup(group.Group_Id, group.Description);

                if (updatedGroup == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(group);
                }

                // Link selected roles
                var roleIds = new List<int>();
                if (group.Posted_Roles != null)
                {
                    foreach (var roleId in group.Posted_Roles.Role_IDs)
                    {
                        var roleIdValue = int.Parse(roleId);
                        roleIds.Add(roleIdValue);
                    }
                }
                groupModel.AddGroupToRole(updatedGroup.Group_Id, roleIds);

                return RedirectToAction("Index", "Group");
            }

            return View(group);
        }

        [CustomAuthorize("Main", "Group", "Delete")]
        public ActionResult Delete(string id)
        {
            var groupModel = new GroupModel();
            var deleteGroup = groupModel.GetSpecificGroup(int.Parse(id));

            return View(deleteGroup);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Group group)
        {
            var groupModel = new GroupModel();
            var deletedGroup = groupModel.SetGroupIsDeleted(group.Group_Id, true);

            if (deletedGroup == null)
            {
                ViewBag.Message = "An Error Occured, Contact support";
                return View(group);
            }

            return RedirectToAction("Index", "Group");
        }
    }
}