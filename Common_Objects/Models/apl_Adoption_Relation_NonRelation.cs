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
    
    public partial class apl_Adoption_Relation_NonRelation
    {
        public apl_Adoption_Relation_NonRelation()
        {
            this.ADOPT_Case_Details = new HashSet<ADOPT_Case_Details>();
        }
    
        public int NonRelation_Realtion_Id { get; set; }
        public string Description { get; set; }
        public string source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<ADOPT_Case_Details> ADOPT_Case_Details { get; set; }
    }
}
