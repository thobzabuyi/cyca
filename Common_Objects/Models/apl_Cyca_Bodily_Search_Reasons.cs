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
    
    public partial class apl_Cyca_Bodily_Search_Reasons
    {
        public apl_Cyca_Bodily_Search_Reasons()
        {
            this.CYCA_Admissions_BodilySearch = new HashSet<CYCA_Admissions_BodilySearch>();
            this.CYCA_BodilySearch = new HashSet<CYCA_BodilySearch>();
        }
    
        public int Search_Reason_Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<CYCA_Admissions_BodilySearch> CYCA_Admissions_BodilySearch { get; set; }
        public virtual ICollection<CYCA_BodilySearch> CYCA_BodilySearch { get; set; }
    }
}
