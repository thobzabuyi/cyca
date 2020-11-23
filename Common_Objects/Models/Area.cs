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
    
    public partial class Area
    {
        public Area()
        {
            this.Service_Offices = new HashSet<Service_Office>();
            this.Municipalities = new HashSet<Municipality>();
        }
    
        public int Area_Id { get; set; }
        public int District_Id { get; set; }
        public string Description { get; set; }
    
        public virtual District District { get; set; }
        public virtual ICollection<Service_Office> Service_Offices { get; set; }
        public virtual ICollection<Municipality> Municipalities { get; set; }
    }
}
