using Common_Objects.Models;
using System.Linq;
using System.Web.Mvc;

namespace Common_Objects
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public CustomAuthorize(string moduleName, string controllerName, string actionName)
        {
            var moduleModel = new ModuleModel();
            var module = moduleModel.GetSpecificModule(moduleName);

            var controller = module.Module_Controllers.First(x => x.Module_Controller_Name.Equals(controllerName));
            var action = controller.Module_Actions.FirstOrDefault(x => x.Module_Action_Name.Equals(actionName));

            if ((action != null) && (action.Roles.Any()))
                Roles = string.Join(",", action.Roles.Select(r => r.Description).ToArray());
        }
    }
}
