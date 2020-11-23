using Common_Objects;
using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace CYCA_Module_V2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            var loginUser = new Login_User();
            return View(loginUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(Login_User user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var username = user.Username;
                var password = user.Password;

                var userModel = new UserModel();
                var loggedInAgent = userModel.DoLogin(username, password);

                if (loggedInAgent == null)
                {
                    ModelState.AddModelError("", "The Username or Password is incorrect! Please try again!");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    Session.Remove("CurrentUser");
                    Session.Remove("MenuLayout");
                    Session.Add("CurrentUser", loggedInAgent);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(user);
        }

        public ActionResult LogOff()
        {
            Session.Remove("CurrentUser");
            Session.Remove("MenuLayout");
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgotPassword()
        {
            var resetUser = new User_Reset_Password();
            return View(resetUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgotPassword(User_Reset_Password userToReset)
        {
            if (ModelState.IsValid)
            {
                var username = userToReset.Username;

                var userModel = new UserModel();
                var resetUser = userModel.GetSpecificUser(username);

                if (resetUser == null)
                {
                    ModelState.AddModelError("", "The Username specified cannot be found! Please try again!");
                }
                else
                {
                    ViewBag.Message = string.Format("Reset Instructions was sent to '{0}'", resetUser.Email_Address);

                    //TODO: Build logic for sending reset email 
                }
            }

            return View(userToReset);
        }

        public ActionResult Manage(string id)
        {
            var manageUserViewModel = new ManageUserViewModel() { User = new User() };

            var userModel = new UserModel();
            manageUserViewModel.User = userModel.GetSpecificUser(int.Parse(id));

            return View(manageUserViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Manage(ManageUserViewModel manageUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var userModel = new UserModel();

                var updatedUser = userModel.EditUser(manageUserViewModel.User.User_Id, manageUserViewModel.User.First_Name, manageUserViewModel.User.Last_Name, manageUserViewModel.User.Initials, manageUserViewModel.User.Email_Address);

                if (updatedUser == null)
                {
                    ViewBag.Message = "An Error Occurred, Please contact Support";
                    return View(manageUserViewModel);
                }

                if ((!string.IsNullOrEmpty(manageUserViewModel.OldPassword)) && (!string.IsNullOrEmpty(manageUserViewModel.NewPassword)) && (!string.IsNullOrEmpty(manageUserViewModel.ConfirmPassword)))
                {
                    if (manageUserViewModel.OldPassword != updatedUser.Password)
                    {
                        ModelState.AddModelError("", "The Old Password does not match the Current User Password! Please try again.");
                        manageUserViewModel.OldPassword = string.Empty;
                        manageUserViewModel.NewPassword = string.Empty;
                        manageUserViewModel.ConfirmPassword = string.Empty;
                        return View(manageUserViewModel);
                    }
                    if (manageUserViewModel.NewPassword != manageUserViewModel.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "The New Password and Confirm Password does not match! Please try again.");
                        manageUserViewModel.OldPassword = string.Empty;
                        manageUserViewModel.NewPassword = string.Empty;
                        manageUserViewModel.ConfirmPassword = string.Empty;
                        return View(manageUserViewModel);
                    }

                    // Change Password
                    updatedUser = userModel.ChangeUserPassword(manageUserViewModel.User.User_Id, manageUserViewModel.NewPassword);

                    if (updatedUser == null)
                    {
                        ViewBag.Message = "An Error Occurred, Please contact Support";
                        return View(manageUserViewModel);
                    }
                }

                FormsAuthentication.SetAuthCookie(updatedUser.User_Name, false);
                Session.Remove("CurrentUser");
                Session.Remove("MenuLayout");
                Session.Add("CurrentUser", updatedUser);

                return RedirectToAction("Index", "Home");
            }

            return View(manageUserViewModel);
        }

        [CustomAuthorize("CYCA", "User", "Index")]
        public ActionResult Index()
        {
            var userModel = new UserModel();
            var userList = userModel.GetListOfUsers(true, false);

            return View(userList);
        }
    }
}