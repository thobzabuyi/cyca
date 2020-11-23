using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CYCA_Module_V2.Controllers
{
    public class TakePhotoController : Controller
    {
        // GET: TakePhoto
        public ActionResult Capture()
        {
            return View();
        }
    }
}