using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;

namespace Common_Objects.ViewModels
{
    public class ACMHomeViewModel
    {

        public string newCaseSearch { get; set; }

        public string currentCaseSearch { get; set; }

        public List<Intake_Assessment> NewCases { get; set; }
        public List<ACM_CaseWorkList> CurrentCases { get; set; }

    }
}
