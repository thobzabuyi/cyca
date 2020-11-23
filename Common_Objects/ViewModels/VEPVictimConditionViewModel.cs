using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
   public class VEPVictimConditionViewModel
    {
        public int VictimsConditionsID { get; set; }
        public int? Caseid { get; set; }
        public string Conditions { get; set; }
        public DateTime DateCaptured { get; set; }
       

    }
}
