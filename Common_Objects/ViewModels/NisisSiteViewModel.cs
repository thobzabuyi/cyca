using Common_Objects.Models;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class NisisSiteViewModel
    {
        public NISIS_Site nisisSite { get; set; }
        public List<NisisSiteEAGridMain> siteEAItems { get; set; }
        public bool Site_QA_Approved { get; set; }
    }
}
