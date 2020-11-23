using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMSearchViewModel
    {
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_Client_Ref_No { get; set; }
        public string Search_Client_ID_No { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Person> Person_List { get; set; }
        public int Selected_Person_Id { get; set; }
        public List<Client> Client_List { get; set; }
        public List<PCMCaseGridMain> Clients_Case_List { get; set; }
    }


}
