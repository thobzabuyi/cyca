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
    
    public partial class CYCA_Team_Allocation
    {
        public int Team_Allocation_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<int> Reporting_User { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public Nullable<int> Venue_Id { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime Start_Time { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public Nullable<System.DateTime> End_Time { get; set; }
    
        public virtual User apl_User { get; set; }
        public virtual apl_Cyca_Venue apl_Cyca_Venue { get; set; }
    }
}