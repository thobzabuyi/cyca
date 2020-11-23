using Common_Objects.Models;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class NisisParticipantSearchViewModel
    {
        public bool Is_Filtered { get; set; }
        public int? Page_Number { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public string Search_ID_Number { get; set; }
        public List<Person> Person_List { get; set; }
        public int Selected_Person_Id { get; set; }
    }
}
