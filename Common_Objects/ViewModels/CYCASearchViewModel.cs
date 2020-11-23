using Common_Objects.Models;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class CYCASearchViewModel
    {
        public int? pageNumber { get; set; }
        public bool showOnlyMyRecords { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_Client_Ref_No { get; set; }
        public string Search_Client_ID_No { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Person> Person_List { get; set; }
        public int Selected_Person_Id { get; set; }

        public List<CycaClientGridMain> Clients_Assessments_List { get; set; }
        public List<InboxGridItem> Inbox_List { get; set; }
        public List<NationalStatsGrid> NationalStatsList { get; set; }
        public List<NationalStatsGrid> NationalGenderStatsList { get; set; }
    }
}
