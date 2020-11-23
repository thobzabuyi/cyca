﻿using Common_Objects.Models;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class RoleDelegationController : Controller
    {
        public ActionResult Index(string SearchDelegatedFrom, string SearchDelegatedTo, string SearchDateFrom, string SearchDateTo)
        {
            var roleDelegationModel = new RoleDelegationModel();
            var roleDelegationList = roleDelegationModel.GetListOfRoleDelegations(SearchDelegatedFrom, SearchDelegatedTo, SearchDateFrom, SearchDateTo);

            return View(roleDelegationList);
        }

        public ActionResult Create()
        {
            var roleDelegation = new User_Role_Delegation();

            return View(roleDelegation);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(User_Role_Delegation roleDelegation)
        {
            if (ModelState.IsValid)
            {
                var roleDelegationModel = new RoleDelegationModel();
                var createRoleDelegation = roleDelegationModel.CreateRoleDelegation(roleDelegation.From_User_Id, roleDelegation.To_User_Id, roleDelegation.Date_From, roleDelegation.Date_To);

                if (createRoleDelegation == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(roleDelegation);
                }

                return RedirectToAction("Index", "RoleDelegation");
            }
            
            return View(roleDelegation);
        }

        public ActionResult Edit(string id)
        {
            var roleDelegationModel = new RoleDelegationModel();
            var roleDelegationToEdit = roleDelegationModel.GetSpecificRoleDeletagion(int.Parse(id));

            return View(roleDelegationToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(User_Role_Delegation roleDelegation)
        {
            if (ModelState.IsValid)
            {
                var roleDelegationModel = new RoleDelegationModel();

                var updatedRoleDelegation = roleDelegationModel.EditRoleDelegation(roleDelegation.User_Role_Delegation_Id, roleDelegation.From_User_Id, roleDelegation.To_User_Id, roleDelegation.Date_From, roleDelegation.Date_To);

                if (updatedRoleDelegation == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(roleDelegation);
                }

                return RedirectToAction("Index", "RoleDelegation");
            }

            return View(roleDelegation);
        }
    }
}