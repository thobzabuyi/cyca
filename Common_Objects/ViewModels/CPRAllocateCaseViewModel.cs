using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Common_Objects.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CPRAllocateCaseViewModel
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

        [Display(Name = "Social Worker")]
        public SelectList Social_Worker_List
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
                                      Selected = c.Social_Worker_Id.Equals(Selected_Social_Worker_Id)
                                  }).ToList();

                var selectList = new SelectList(socialWorkersList, "Value", "Text", Selected_Social_Worker_Id);

                return selectList;
            }
        }

        public int Selected_Social_Worker_Id { get; set; }

        public List<CPR_Incident> Unallocated_Incident_List { get; set; }
        public List<CPR_Incident> Allocated_Incident_List { get; set; }
        public int Selected_Incident_Id { get; set; }
        public string SelectedCasesToAllocate { get; set; }
        public string SelectedCasesToDeallocate { get; set; }
    }
}
