using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public partial class VEP_Victimization_Type
    {
        public VEP_Victimization_Type()
        {
            this.VEP_Case = new HashSet<VEP_Cases>();
        }

        public int Id { get; set; }
        public string VictimizationType { get; set; }

        public virtual ICollection<VEP_Cases> VEP_Case { get; set; }
    }
}
