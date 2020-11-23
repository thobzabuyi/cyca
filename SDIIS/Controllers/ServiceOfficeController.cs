using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class ServiceOfficeController : Controller
    {
        // GET: ServiceOffice
        public ActionResult Index()
        {
            ServiceOfficeModel ServiceOffModel = new ServiceOfficeModel();
            List<Service_Office> ListOfSerOffVm = ServiceOffModel.GetListOfServiceOffices();
            return PartialView(ListOfSerOffVm);
        }

        public ActionResult ServiceOfficeDetail(int serviceOfficeId)
        {
            ServiceOfficeModel ServiceOffModel = new ServiceOfficeModel();
            Service_Office service_Office = ServiceOffModel.GetSpecificServiceOffice(serviceOfficeId);
            return PartialView(service_Office);
        }

        public ActionResult CreateServiceOffice(string Description, string Municipality, string UserName)
        {
            ServiceOfficeModel ServiceOffModel = new ServiceOfficeModel();
            Service_Office service_Office = ServiceOffModel.CreateServiceOffice(Description, Municipality, UserName);
            return PartialView(service_Office);
        }
        [HttpGet]
        public ActionResult EditServiceOffice(int serviceOfficeId)
        {
            ServiceOfficeModel ServiceOffModel = new ServiceOfficeModel();
            Service_Office service_Office = ServiceOffModel.GetSpecificServiceOffice(serviceOfficeId);
            return PartialView(service_Office);
        }

        [HttpPost]
        public ActionResult EditServiceOffice(Service_Office service_Office)
        {
            var UserName = Convert.ToString(Session["UserName"]);
            ServiceOfficeModel ServiceOffModel = new ServiceOfficeModel();
            ServiceOffModel.EditServiceOffice(service_Office, UserName);
            return PartialView(service_Office);
        }
    }
}