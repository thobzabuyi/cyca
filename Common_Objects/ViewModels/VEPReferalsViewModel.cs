using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEPReferalsViewModel
    {
        public int Referalsid { get; set; }
        public int Caseid { get; set; }
        public string FromDepartment { get; set; }
        public string ToDepartment { get; set; }
        public string Notes { get; set; }
        public int? Createdby { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByString { get; set; }
        public DateTime? CreateDate { get; set; }
        public int Statusid { get; set; }


    }
}
