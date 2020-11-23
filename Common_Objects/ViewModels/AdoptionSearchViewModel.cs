using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class AdoptionSearchViewModel
    {
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_Client_Ref_No { get; set; }
        public string Search_Client_ID_No { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Person> Person_List { get; set; }
        public int Selected_Person_Id { get; set; }
    }

    public class AdoptionCaseListViewModel
    {
        public string Search_Intake_Ref_No { get; set; }
        public string Search_PCM_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Identification_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Client> Client_List { get; set; }
        public int Selected_Client_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Client_Id { get; set; }
        public List<AdoptCaseGridMain> Clients_Case_List { get; set; }
    }
}
