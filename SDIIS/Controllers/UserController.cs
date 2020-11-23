using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Common_Objects;
using Common_Objects.Models;

namespace SDIIS.Controllers
{
    public class UserController : Controller
    {
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
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(username, "User loggedIn"+":"+ username, "SDICMS");

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
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;


            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }

            var auditTrail = new AuditTrailModel();
            auditTrail.InsertAuditTrail(userName, "User loggedOff" + ":" + userName, "SDICMS");

            Session.Remove("CurrentUser");
            Session.Remove("MenuLayout");
            FormsAuthentication.SignOut();
           

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgotPassword()
        {
            var resetPassword = new User_Forgot_Password();
            return View(resetPassword);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgotPassword(User_Forgot_Password userToReset)
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
                    var tempPassword = Membership.GeneratePassword(8, 2);
                    var userFullName = resetUser.First_Name + " " + resetUser.Last_Name;

                    userModel.ChangeUserPassword(resetUser.User_Id, tempPassword);
                    var auditTrail = new AuditTrailModel();
                    auditTrail.InsertAuditTrail(username, "User reset Password" + ":" + userFullName, "SDICMS");

                    var message = "Dear " + userFullName;
                    message += "<br /><br />";
                    message += "A password reset process was requested for your username.<br /><br />";
                    message += "Your new temporary password is: " + tempPassword + "<br /></br />";
                    message += "Please click <a href='" + Url.Action("ResetPassword", "User", null, Request.Url.Scheme, null) + "'>here</a> to reset your password.";

                    var mailSent = Mailer.SendMail(userFullName, resetUser.Email_Address, "Email Reset Request", message);

                    if (mailSent)
                        ViewBag.Message = string.Format("Reset Instructions was sent to '{0}'. Please review the email and follow the instructions to reset your password", resetUser.Email_Address);
                    else
                        ViewBag.Message = "Reset Instructions email could not be sent due to a technical difficulty. Please try again later!";
                }
            }

            return View(userToReset);
        }

        public ActionResult ResetPassword()
        {
            var resetPassword = new User_Reset_Password();
            return View(resetPassword);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResetPassword(User_Reset_Password resetPassword)
        {
            var userModel = new UserModel();
            var user = string.IsNullOrEmpty(resetPassword.Username) ? new User() : userModel.GetSpecificUser(resetPassword.Username);

            if (!string.IsNullOrEmpty(resetPassword.Username))
            {
                if (user == null)
                {
                    ModelState.AddModelError("Username", "The Username supplied does not exist!");
                }
            }

            if (resetPassword.NewPassword != resetPassword.ConfirmNewPassword)
                ModelState.AddModelError("ConfirmNewPassword", "The New Password and Confirm Password fields does not match!");

            if (resetPassword.OldPassword != user.Password)
                ModelState.AddModelError("OldPassword", "The Old (Current) Password is not valid!");

            if (ModelState.IsValid)
            {
                if ((user != null) && (user.User_Id != 0))
                {
                    var updatedUser = userModel.ChangeUserPassword(user.User_Id, resetPassword.NewPassword);

                    if (updatedUser == null)
                    {
                        ModelState.AddModelError("", "The Username specified cannot be found! Please try again!");
                        return View(resetPassword);
                    }

                    return RedirectToAction("Login");
                }
            }

            return View(resetPassword);
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
            var currentUser = (User)Session["CurrentUser"];

            var userName = string.Empty;


            if (currentUser != null)
            {
                userName = currentUser.User_Name;
            }
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
                    else
                    {
                        var auditTrail = new AuditTrailModel();
                        auditTrail.InsertAuditTrail(userName, "User Change Password" + ":" + userName, "SDICMS");
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

        [CustomAuthorize("Main", "User", "Index")]
        public ActionResult Index()
        {
            var userModel = new UserModel();
            var userList = userModel.GetListOfUsers(true, false);

            return View(userList);
        }

        [CustomAuthorize("Main", "User", "Create")]
        public ActionResult Create()
        {
            var newUser = new User() { Is_Active = true };

            return View(newUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var userModel = new UserModel();
                var createUser = userModel.CreateUser(user.First_Name, user.Last_Name, user.Is_Active);

                if (createUser == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(user);
                }

                // Link selected roles
                var roleIds = new List<int>();
                if (user.Posted_Roles != null)
                {
                    foreach (var roleId in user.Posted_Roles.Role_IDs)
                    {
                        var roleIdValue = int.Parse(roleId);
                        roleIds.Add(roleIdValue);
                    }
                }

                userModel.AddUserToRole(createUser.User_Id, roleIds);

                // Link Selected Groups
                var groupIds = new List<int>();
                if (user.Posted_Groups != null)
                {
                    foreach (var groupId in user.Posted_Groups.Group_IDs)
                    {
                        var groupIdValue = int.Parse(groupId);
                        groupIds.Add(groupIdValue);
                    }
                }

                userModel.AddUserToGroup(createUser.User_Id, groupIds);

                return RedirectToAction("Index", "User");
            }

            return View(user);
        }

        [CustomAuthorize("Main", "User", "Edit")]
        public ActionResult Edit(string id)
        {
            var userModel = new UserModel();
            var userToEdit = userModel.GetSpecificUser(int.Parse(id));

            return View(userToEdit);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var userModel = new UserModel();

                var updatedUser = userModel.EditUser(user.User_Id, user.First_Name, user.Last_Name, user.Initials, user.Email_Address);

                if (updatedUser == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(user);
                }

                // Link Selected Roles
                var roleIds = new List<int>();
                if (user.Posted_Roles != null)
                {
                    foreach (var roleId in user.Posted_Roles.Role_IDs)
                    {
                        var roleIdValue = int.Parse(roleId);
                        roleIds.Add(roleIdValue);
                    }
                }

                userModel.AddUserToRole(updatedUser.User_Id, roleIds);

                // Link Selected Groups
                var groupIds = new List<int>();
                if (user.Posted_Groups != null)
                {
                    foreach (var groupId in user.Posted_Groups.Group_IDs)
                    {
                        var groupIdValue = int.Parse(groupId);
                        groupIds.Add(groupIdValue);
                    }
                }

                userModel.AddUserToGroup(updatedUser.User_Id, groupIds);

                return RedirectToAction("Index", "User");
            }

            return View(user);
        }

        [CustomAuthorize("Main", "User", "Delete")]
        public ActionResult Delete(string id)
        {
            var userModel = new UserModel();
            var deleteUser = userModel.GetSpecificUser(int.Parse(id));

            return View(deleteUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(User user)
        {
            var userModel = new UserModel();
            var deletedUser = userModel.SetUserIsDeleted(user.User_Id, true);

            if (deletedUser == null)
            {
                ViewBag.Message = "An Error Occured, Contact support";
                return View(user);
            }

            return RedirectToAction("Index", "User");
        }
    }
}