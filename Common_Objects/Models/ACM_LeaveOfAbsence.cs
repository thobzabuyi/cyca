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
    
    public partial class ACM_LeaveOfAbsence
    {
        public int LeaveOfAbsence_Id { get; set; }
        public int Client_id { get; set; }
        public int TypeOPlacement { get; set; }
        public Nullable<System.DateTime> ToGoOnHolidayLeaveFrom { get; set; }
        public Nullable<System.DateTime> ToGoOnHolidayLeaveTo { get; set; }
        public string ResidingAt { get; set; }
        public Nullable<System.DateTime> OnThisDate { get; set; }
        public string Level { get; set; }
        public int CaseWorklist_Id { get; set; }
        public string CustodyOf { get; set; }
        public string CareOf { get; set; }
    
        public virtual ACM_CaseWorkList ACM_CaseWorkList { get; set; }
    }
}
