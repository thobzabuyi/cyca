using Common_Objects.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace SDIIS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        var username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

                        var userModel = new UserModel();
                        var user = userModel.GetSpecificUser(username);
                        var roles = user.Roles.Select(r => r.Description).ToList();

                        // Get Roles assigned via Groups
                        foreach (var group in user.Groups)
                        {
                            var groupRoles = group.Roles.Select(r => r.Description).ToList();
                            roles.AddRange(groupRoles);
                        }

                        // Add Roles added due to role delegation
                        var userRoleDelegation = user.Users_Delegated_From.FirstOrDefault(p => p.Date_From <= DateTime.Now && p.Date_To >= DateTime.Now);
                        if (userRoleDelegation != null)
                        {
                            var tempRoles = userRoleDelegation.Delegated_To_User.Roles.Select(r => r.Description).ToList();
                            roles.AddRange(tempRoles);
                        }

                        // Get Distinctive list of effective roles
                        var effectiveRoles = roles.Distinct().ToArray();

                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(username, "Forms"), effectiveRoles);
                    }
                    catch (Exception)
                    {
                        //something went wrong
                        // TODO: Log Error here
                    }
                }
            }
        }
    }
}
