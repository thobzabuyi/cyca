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
    
    public partial class NISIS_Listing_Method
    {
        public NISIS_Listing_Method()
        {
            this.NISIS_Sites = new HashSet<NISIS_Site>();
            this.NISIS_Listings = new HashSet<NISIS_Listing>();
        }
    
        public int NISIS_Listing_Method_Id { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<NISIS_Site> NISIS_Sites { get; set; }
        public virtual ICollection<NISIS_Listing> NISIS_Listings { get; set; }
    }
}
