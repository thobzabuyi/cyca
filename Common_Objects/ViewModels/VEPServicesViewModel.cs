using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEPServicesViewModel
    {
        public int Serviceid { get; set; }
        public int? Caseid { get; set; }

        public int clientID { get; set; }
        public int? ServiceTypeid { get; set; }
        public string ServiceNotes { get; set; }
        public DateTime? DateCreated { get; set; }

        public int isActive { get; set; }
    }
}
