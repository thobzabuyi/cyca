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
    
    public partial class NISIS_Programme
    {
        public NISIS_Programme()
        {
            this.Responsible_Programme_Sites = new HashSet<NISIS_Site>();
            this.NISIS_Site1 = new HashSet<NISIS_Site>();
        }
    
        public int NISIS_Programme_Id { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<NISIS_Site> Responsible_Programme_Sites { get; set; }
        public virtual ICollection<NISIS_Site> NISIS_Site1 { get; set; }
    }
}