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
    
    public partial class apl_Cyca_Transfer_Status
    {
        public apl_Cyca_Transfer_Status()
        {
            this.CYCA_Child_Transfer = new HashSet<CYCA_Child_Transfer>();
        }
    
        public int Tansfer_Status_Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<CYCA_Child_Transfer> CYCA_Child_Transfer { get; set; }
    }
}
