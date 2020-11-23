using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class AddressViewModel
    {
        public int Address_id {get;set;}
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public int ProvId { get; set; }
        public int DistId { get; set; }
        public int LoMunId { get; set; }
        public int TownId { get; set; }
    }
}
