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
    
    public partial class NISIS_Service_Category
    {
        public NISIS_Service_Category()
        {
            this.NISIS_Service = new HashSet<NISIS_Service>();
        }
    
        public int NISIS_Service_Category_Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<NISIS_Service> NISIS_Service { get; set; }
    }
}
