using Common_Objects.Models;
using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDIIS.Controllers
{
    public class PlaceNameController : Controller
    {
        private IntakePlaceNameModel Obj = new IntakePlaceNameModel();
        private SDIIS_DatabaseEntities dbCont = new SDIIS_DatabaseEntities();
        [Authorize(Roles = "SysAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        #region Province
        // GET: PlaceName
        [HttpGet]
        public ActionResult IndexProv()
        {
            List<Province> Obj1 = Obj.GetProvinces();

            return View(Obj1);
        }
        [HttpGet]
        public ActionResult CreateProv()
        {
            Province NewObject = new Province();
            return View(NewObject);
        }

        [HttpPost]
        public ActionResult CreateProv(Province NewEntry)
        {
            Province NewObject = Obj.CreateProv(NewEntry.Description, NewEntry.Abbreviation);
            return RedirectToAction("IndexProv");
        }

        [HttpGet]
        public ActionResult EditProv(int Id)
        {

            Province OldData = Obj.GetProvince(Id);
            return View(OldData);
        }

        [HttpPost]
        public ActionResult EditProv(Province UpdatedObj)
        {
            Province OldData = Obj.EditProvince(UpdatedObj.Province_Id, UpdatedObj.Description, UpdatedObj.Abbreviation);
            return RedirectToAction("IndexProv");
        }

        public ActionResult DeleteProv(int Id)
        {
            Obj.DeleteProv(Id);

            return RedirectToAction("IndexProvinces");


            }

        #endregion

        #region District

        public ActionResult IndexDistricts(string SearchProvince, string SearchDistrict)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            IEnumerable<SelectListItem> itemsD = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = itemsD;

            List<District> ShowList = Obj.GetDistricts(SearchProvince, SearchDistrict);


            return View(ShowList);
        }

        [HttpGet]
        public ActionResult CreateDistrict()
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            District NewObject = new District();
            return View(NewObject);
        }

        [HttpPost]
        public ActionResult CreateDistrict(District NewEntry)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            District NewObject = Obj.CreateDistrict(NewEntry);
            return RedirectToAction("IndexDistricts");
        }
        [HttpGet]
        public ActionResult EditDistrict(int Id)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            District OldData = Obj.GetDistrict(Id);
            return View(OldData);
        }

        [HttpPost]
        public ActionResult EditDistrict(District UpdatedObj)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            District OldData = Obj.EditDistrict(UpdatedObj);
            return RedirectToAction("IndexDistricts");
        }

        public ActionResult DeleteDistrict(int Id)
        {
            Obj.DeleteDistrict(Id);

            return RedirectToAction("IndexDistricts");
        }
        #endregion

        #region Municipality

        public ActionResult IndexMunicipalities(string SearchProvince, string SearchDistrict, string SearchMunicipality)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;

            List<Local_Municipality> ShowList = Obj.GetMunicipalities(SearchProvince, SearchDistrict, SearchMunicipality);
            ViewBag.NewNumber = 1;
            return View(ShowList);
        }

        [HttpGet]
        public ActionResult CreateMunicipality()
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            Local_Municipality NewObject = new Local_Municipality();
            return View(NewObject);
        }

        [HttpPost]
        public ActionResult CreateMunicipality(Local_Municipality NewEntry)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            Local_Municipality NewObject = Obj.CreateMunicipality(NewEntry);
            return RedirectToAction("IndexMunicipalities");
        }

        [HttpGet]
        public ActionResult EditMunicipality(int? Id)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;

            Local_Municipality OldData = Obj.GetMunicipality(Id);

            return PartialView("EditMunicipality", OldData);
        }

        [HttpPost]
        public ActionResult EditMunicipality(Local_Municipality UpdatedObj)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            Local_Municipality OldData = Obj.EditMunicipality(UpdatedObj);
            return RedirectToAction("IndexMunicipalities");
        }

        public ActionResult DeleteMunicipality(int Id)
        {
            Obj.DeleteProv(Id);

            return RedirectToAction("IndexMunicipalities");

        }

        #endregion

        #region Town

        public ActionResult IndexTowns(string SearchProvince, string SearchDistrict, string SearchMunicipality, string SearchTown)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;

            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.MunicipalityList = items_2;

            IEnumerable<SelectListItem> items_3 = dbCont.Towns.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Town_Id.ToString()

            });
            ViewBag.TownList = items_3;

            List<Town> ShowList = Obj.GetTowns(SearchProvince, SearchDistrict, SearchMunicipality, SearchTown);
            ViewBag.NewNumber = 1;
            return View(ShowList);
        }

        [HttpGet]
        public ActionResult CreateTown()
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;

            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.MunicipalityList = items_2;
            Town NewObject = new Town();
            return View(NewObject);
        }

        [HttpPost]
        public ActionResult CreateTown(Town NewEntry)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.MunicipalityList = items_2;
            Town NewObject = Obj.CreateTown(NewEntry);
            return RedirectToAction("IndexTowns");
        }

        [HttpGet]
        public ActionResult EditTown(int? Id)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.MunicipalityList = items_2;
            Town OldData = Obj.GetTown(Id);

            return PartialView("EditTown", OldData);
        }

        [HttpPost]
        public ActionResult EditTown(Town UpdatedObj)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.MunicipalityList = items_2;
            Town OldData = Obj.EditTown(UpdatedObj);
            return RedirectToAction("IndexTowns");
        }

        public ActionResult DeleteTown(int Id)
        {
            Obj.DeleteTown(Id);

            return RedirectToAction("IndexTowns");

        }

        #endregion

        #region Ward

        public ActionResult IndexWards(string SearchProvince, string SearchDistrict, string SearchLocal_Municipality, string SearchTown)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;

            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.MunicipalityList = items_2;

            IEnumerable<SelectListItem> items_3 = dbCont.Towns.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Town_Id.ToString()

            });
            ViewBag.TownList = items_3;


            List<NISIS_Ward> ShowList = Obj.GetWards(SearchProvince, SearchDistrict, SearchLocal_Municipality, SearchTown);
            ViewBag.NewNumber = 1;
            return View(ShowList);
        }

        [HttpGet]
        public ActionResult CreateWard()
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;

            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;

            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.Local_MunicipalityList = items_2;
            NISIS_Ward NewObject = new NISIS_Ward();
            return View(NewObject);
        }

        [HttpPost]
        public ActionResult CreateWard(NISIS_Ward NewEntry)
        {
            string loginName = User.Identity.Name;
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.Local_MunicipalityList = items_2;
            NISIS_Ward NewObject = Obj.CreateWard(NewEntry, loginName);
            return RedirectToAction("IndexWards");
        }

        [HttpGet]
        public ActionResult EditWard(int? Id)
        {
            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.Local_MunicipalityList = items_2;
            NISIS_Ward OldData = Obj.GetWard(Id);

            return PartialView("EditWard", OldData);
        }

        [HttpPost]
        public ActionResult EditWard(NISIS_Ward UpdatedObj)
        {
            string loginName = User.Identity.Name;

            IEnumerable<SelectListItem> items = dbCont.Provinces.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Province_Id.ToString()

            });
            ViewBag.ProvinceList = items;
            IEnumerable<SelectListItem> items_1 = dbCont.Districts.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.District_Id.ToString()

            });
            ViewBag.DistrictList = items_1;
            IEnumerable<SelectListItem> items_2 = dbCont.Local_Municipalities.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Local_Municipality_Id.ToString()

            });
            ViewBag.Local_MunicipalityList = items_2;
            NISIS_Ward OldData = Obj.EditWard(UpdatedObj, loginName);
            return RedirectToAction("IndexWards");
        }

        public ActionResult DeleteWard(int Id)
        {
            Obj.DeleteWard(Id);

            return RedirectToAction("IndexWards");

        }

        #endregion

        public ActionResult FilterFromProvinceAjax(string provinceId)
        {
            //Use to generate VEP Search Reports
            Session["SearchProvinceId"] = provinceId;

            if (String.IsNullOrEmpty(provinceId))
            {
                provinceId = "-1";
            }

            var municipalityModel = new DistrictModel();
            var municipalitiesList = municipalityModel.GetListOfDistricts(int.Parse(provinceId));

            var result = (from x in municipalitiesList
                          orderby x.Description
                          select new
                          {
                              id = x.District_Id,
                              name = x.Description
                          })

                          .ToList();
            ViewBag.AvailableDisabilitySubType = result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterFromMunicipalityAjax(string municipalityId)
        {
            //Use to generate VEP Search Reports
            Session["SearchDistrictId"] = municipalityId;

            if (String.IsNullOrEmpty(municipalityId))
            {
                municipalityId = "-1";
            }

            var localMunicipalityModel = new LocalMunicipalityModel();
            var localMunicipalitiesList = localMunicipalityModel.GetListOfLocalMunicipalities(int.Parse(municipalityId));

            var result = (from x in localMunicipalitiesList
                          select new
                          {
                              id = x.Local_Municipality_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterFromLocalMunicipalityAjax(string localMunicipalityId)
        {

            if (String.IsNullOrEmpty(localMunicipalityId))
            {
                localMunicipalityId = "-1";
            }

            var townModel = new TownModel();
            var townsList = townModel.GetListOfTowns(int.Parse(localMunicipalityId));

            var result = (from x in townsList
                          select new
                          {
                              id = x.Town_Id,
                              name = x.Description
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}