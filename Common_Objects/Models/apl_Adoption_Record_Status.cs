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
    
    public partial class apl_Adoption_Record_Status
    {
        public apl_Adoption_Record_Status()
        {
            this.ADOPT_Case_WorkList = new HashSet<ADOPT_Case_WorkList>();
        }
    
        public int Adopt_Record_Status_Id { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<ADOPT_Case_WorkList> ADOPT_Case_WorkList { get; set; }
    }
}