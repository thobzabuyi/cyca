using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEP_VictimizatimTypeListVM
    {
        public int Id { get; set; }
        public int Case_Id { get; set; }
        public int VicTypeId { get; set; }
        [Display(Name = "Victimaztion Type")]
        public string VictimizationType { get; set; }
        public bool IsChecked { get; set; }
    }
}
