using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class RACAPSpecialNeedsVM
    {
        public int Racap_CaseId {get;set;}

        public int? SpecialNeedId { get; set; }
        public string SpecialNeedDescription { get; set; }
    }
}
