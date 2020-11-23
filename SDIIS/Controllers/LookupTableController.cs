using System.Web.Mvc;
using Common_Objects.Models;

namespace SDIIS.Controllers
{
    public class LookupTableController : Controller
    {
        public ActionResult Index()
        {
            var lookupData = new LookupData();

            var lookupTableModel = new LookupTableModel();
            lookupData.LookupDataItems = lookupTableModel.GetLookupTableItems(0);

            return View(lookupData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(LookupData lookupData)
        {
            var lookupTableModel = new LookupTableModel();
            lookupData.LookupDataItems = lookupTableModel.GetLookupTableItems(lookupData.SelectedLookupTableId);

            return View(lookupData);
        }

        public ActionResult Create()
        {
            var newLookupItem = new LookupDataItem();

            return View(newLookupItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(LookupDataItem lookupDataItem)
        {
            if (ModelState.IsValid)
            {
                var lookupTableModel = new LookupTableModel();
                var createLookupDataItem = lookupTableModel.CreateLookupDataItem(lookupDataItem.LookupTableTypeId, lookupDataItem.Description, lookupDataItem.Source, lookupDataItem.Definition);

                if (createLookupDataItem == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(lookupDataItem);
                }

                return RedirectToAction("Index", "LookupTable");
            }

            return View(lookupDataItem);
        }

        public ActionResult Edit(string id, string lookupDataTypeId)
        {
            var lookupTableModel = new LookupTableModel();
            var editLookupDataItem = lookupTableModel.GetSpecificLookupTableItem(int.Parse(lookupDataTypeId), int.Parse(id));

            return View(editLookupDataItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(LookupDataItem lookupDataItem)
        {
            if (ModelState.IsValid)
            {
                var lookupTableModel = new LookupTableModel();
                var updatedLookupDataItem = lookupTableModel.EditLookupDataItem(lookupDataItem.ItemId, lookupDataItem.LookupTableTypeId, lookupDataItem.Description, lookupDataItem.Source, lookupDataItem.Definition);

                if (updatedLookupDataItem == null)
                {
                    ViewBag.Message = "An Error Occured, Please contact Support";
                    return View(lookupDataItem);
                }

                return RedirectToAction("Index", "LookupTable");
            }

            return View(lookupDataItem);
        }
	}
}