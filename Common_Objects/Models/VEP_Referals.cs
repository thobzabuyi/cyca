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
    
    public partial class VEP_Referals
    {
        public int ReferalsId { get; set; }
        public int CaseId { get; set; }
        public int FromDepartment { get; set; }
        public int ToDepartment { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> StatusId { get; set; }
    
        public virtual VEP_Status VEP_Status { get; set; }
    }
}