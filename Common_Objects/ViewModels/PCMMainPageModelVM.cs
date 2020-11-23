using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMMainPageModelVM
    {

        public string newCaseSearch { get; set; }

        public string currentCaseSearch { get; set; }


        public List<AdoptionWorkListVM> CurrentCases { get; set; }
        public PCMCaseDetailsViewModel PCMAssemenVM { get; set; }
        public List<PCMCaseDetailsViewModel> PCMCaseDetailsViewModel { get; set; }
        public List<PCMCaseDetailsViewModel> PCMpesronlistV { get; set; }
        public List<PCMCaseDetailsViewModel> PCMpesronlistRecom { get; set; }





    }
}
