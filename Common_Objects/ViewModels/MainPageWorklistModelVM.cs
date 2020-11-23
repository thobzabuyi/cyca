using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;

namespace Common_Objects.ViewModels
{
    public class MainPageWorklistModelVM
    {
        public string newCaseSearch { get; set; }

        public string currentCaseSearch { get; set; }

        public string Search_Intake_Ref_No { get; set; }
        public string Search_PCM_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Identification_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<AdoptionWorkListVM> CurrentCases { get; set; }
        
        public List<AdoptCaseGridMain> Clients_Case_List { get; set; }

        public List<AdoptionWorkListVM> NewCasez { get; set; }


        public List<Intake_Assessment> NewCases { get; set; }
        //public List<AdoptionWorkListVM> CurrentCases { get; set; }
        public List<AdoptionWorkload> Adoptionlist { get; set; }
public List<RACAPWorkListVM> CurrentCases_RACAP { get; set; }

        public List<RACAPWorkListVM> Clients_Case_List_RACAP { get; set; }
        public List<RACAPWorkListVM> NewCasezz { get; set; }

    }
}
