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
    
    public partial class RACAP_Removal
    {
        public int Removal_Id { get; set; }
        public Nullable<int> Removal_Type_Id { get; set; }
        public Nullable<int> RACAP_Case_Id { get; set; }
        public string Removal_Comments { get; set; }
        public Nullable<System.DateTime> Date_Removed { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
        public Nullable<int> Modified_By { get; set; }
    
        public virtual RACAP_Case_Details RACAP_Case_Details { get; set; }
    }
}