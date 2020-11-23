using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;

namespace Common_Objects.ViewModels
{
    public class CPRUnsuitabilityListViewModel
    {
        public List<CPR_Unsuitability> Unsuitability_List { get; set; }
        public string Search_Surname { get; set; }
        public string Search_Name { get; set; }
        public string Search_RecNumber { get; set; }
        public bool Search_Ruiling { get; set; }
    }
}
