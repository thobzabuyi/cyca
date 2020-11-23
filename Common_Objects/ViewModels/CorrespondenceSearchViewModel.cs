using Common_Objects.Models;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class CorrespondenceSearchViewModel
    {
        public bool is_Filtered { get; set; }
        public int? pageNumber { get; set; }
        public string Search_Intake_Ref_No { get; set; }
        public string Search_CPR_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Person> Person_List { get; set; }
        public int Selected_Person_Id { get; set; }

    }
}
