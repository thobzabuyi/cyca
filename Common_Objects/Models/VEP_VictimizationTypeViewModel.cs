using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common_Objects.Models;

namespace VEP_Module.Models
{
    public class VEP_VictimizationTypeViewModel
    {
        public IList<VEP_VictimizationType> AvailableVictimizationType { get; set; }
        public IList<VEP_VictimizationType> SelectedVictimizationType { get; set; }
        public PostedVictimizationType PostedVictimizationType { get; set; }
    }
}