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
    
    public partial class Eye_Color
    {
        public Eye_Color()
        {
            this.CYCA_Admissions_ExtraMuralActivity = new HashSet<CYCA_Admissions_ExtraMuralActivity>();
        }
    
        public int Eye_Color_Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<CYCA_Admissions_ExtraMuralActivity> CYCA_Admissions_ExtraMuralActivity { get; set; }
    }
}