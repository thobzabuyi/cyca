using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class ServiceOfficeViewModel
    {
        public int Service_Office_Id { get; set; }
        public string Description { get; set; }
        public int Local_Municipality_Id { get; set; }
        public string Office_Size { get; set; }
        public bool Is_Rental_Office { get; set; }
        public float Monthly_Rent { get; set; }
        public float Vat { get; set; }
        public float Escalation_Rate { get; set; }
        public DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public DateTime Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }

        public List<LocalMunicipality> LocalMunicipality { get; set; }
    }
}
