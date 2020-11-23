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
    
    public partial class apl_Cyca_Venue
    {
        public apl_Cyca_Venue()
        {
            this.CYCA_Admissions_ReAdmissionDetails = new HashSet<CYCA_Admissions_ReAdmissionDetails>();
            this.CYCA_Team_Allocation = new HashSet<CYCA_Team_Allocation>();
            this.CYCA_Child_Movement = new HashSet<CYCA_Child_Movement>();
            this.CYCA_Admissions_AdmissionDetails = new HashSet<CYCA_Admissions_AdmissionDetails>();
            this.CYCA_ReAdmissionDetails = new HashSet<CYCA_ReAdmissionDetails>();
        }
    
        public int Venue_Id { get; set; }
        public string VenueName { get; set; }
        public Nullable<int> Section_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual apl_Cyca_Facility apl_Cyca_Facility { get; set; }
        public virtual ICollection<CYCA_Admissions_ReAdmissionDetails> CYCA_Admissions_ReAdmissionDetails { get; set; }
        public virtual ICollection<CYCA_Team_Allocation> CYCA_Team_Allocation { get; set; }
        public virtual ICollection<CYCA_Child_Movement> CYCA_Child_Movement { get; set; }
        public virtual ICollection<CYCA_Admissions_AdmissionDetails> CYCA_Admissions_AdmissionDetails { get; set; }
        public virtual ICollection<CYCA_ReAdmissionDetails> CYCA_ReAdmissionDetails { get; set; }
    }
}
