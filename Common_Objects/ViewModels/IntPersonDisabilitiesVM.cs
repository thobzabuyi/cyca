using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class IntPersonDisabilitiesVM
    {

        public int Person_Id { get; set; }
        public int? DisabilityId { get; set; }

        public string DisabilityDescription { get; set; }

    }
}
