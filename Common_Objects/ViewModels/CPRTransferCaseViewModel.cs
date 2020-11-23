using Common_Objects.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CPRTransferCaseViewModel
    {
        public bool is_Filtered { get; set; }
        public int? page_Number { get; set; }
        public string Search_Incident_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_Client_Ref_No { get; set; }
        public string Search_Client_ID_No { get; set; }
        public string Search_ID_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        [Display(Name = "Assigned To")]
        public SelectList Allocated_Social_Worker_List
        {
            get
            {
                var socialWorkerModel = new SocialWorkerModel();
                var listOfSocialWorkers = socialWorkerModel.GetListOfSocialWorkers(false, false);

                var socialWorkersList = (from c in listOfSocialWorkers
                                         select new SelectListItem()
                                         {
                                             Text = string.Format("{0} {1}", c.apl_User.First_Name, c.apl_User.Last_Name),
                                             Value = c.Social_Worker_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = c.Social_Worker_Id.Equals(Selected_Allocated_Social_Worker_Id)
                                         }).ToList();

                var selectList = new SelectList(socialWorkersList, "Value", "Text", Selected_Allocated_Social_Worker_Id);

                return selectList;
            }
        }
        [Display(Name = "Transfer To")]
        public SelectList Transfer_To_Social_Worker_List
        {
            get
            {
                var socialWorkerModel = new SocialWorkerModel();
                var listOfSocialWorkers = socialWorkerModel.GetListOfSocialWorkers(false, false);

                var socialWorkersList = (from c in listOfSocialWorkers
                                         select new SelectListItem()
                                         {
                                             Text = string.Format("{0} {1}", c.apl_User.First_Name, c.apl_User.Last_Name),
                                             Value = c.Social_Worker_Id.ToString(CultureInfo.InvariantCulture),
                                             Selected = c.Social_Worker_Id.Equals(Selected_Transferred_Social_Worker_id)
                                         }).ToList();

                var selectList = new SelectList(socialWorkersList, "Value", "Text", Selected_Transferred_Social_Worker_id);

                return selectList;
            }
        }
        public int Selected_Allocated_Social_Worker_Id { get; set; }
        public int Selected_Transferred_Social_Worker_id { get; set; }
        public List<CPR_Incident> Allocated_Incident_List { get; set; }
        public string Selected_Cases_To_Transfer { get; set; }
    }
}
