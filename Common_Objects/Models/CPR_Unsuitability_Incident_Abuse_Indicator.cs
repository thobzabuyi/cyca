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
    
    public partial class CPR_Unsuitability_Incident_Abuse_Indicator
    {
        public int CPR_Unsuitability_Incident_Abuse_Indicator_Id { get; set; }
        public int CPR_Unsuitability_Id { get; set; }
        public int Abuse_Indicator_Id { get; set; }
        public System.DateTime CreatedTimeStamp { get; set; }
        public int Created_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
    
        public virtual Abuse_Indicator apl_Abuse_Indicator { get; set; }
    }
}
