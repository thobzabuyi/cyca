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
    
    public partial class Offence_Category
    {
        public Offence_Category()
        {
            this.PCM_Offence_Details = new HashSet<PCM_Offence_Details>();
            this.PCM_Previous_Involvement_Details = new HashSet<PCM_Previous_Involvement_Details>();
        }
    
        public int Offence_Category_Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
        public string OffenseCode { get; set; }
    
        public virtual ICollection<PCM_Offence_Details> PCM_Offence_Details { get; set; }
        public virtual ICollection<PCM_Previous_Involvement_Details> PCM_Previous_Involvement_Details { get; set; }
    }
}