//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common_Objects.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SAPS_Rank
    {
        public SAPS_Rank()
        {
            this.SAPS_Officials = new HashSet<SAPS_Official>();
        }
    
        public int SAPS_Rank_Id { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<SAPS_Official> SAPS_Officials { get; set; }
    }
}
