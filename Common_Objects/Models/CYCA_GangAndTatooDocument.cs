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
    
    public partial class CYCA_GangAndTatooDocument
    {
        public int Document_Id { get; set; }
        public string DocAppExt { get; set; }
        public string DocumentType { get; set; }
        public byte[] DataDocument { get; set; }
        public Nullable<int> Admission_Id { get; set; }
        public Nullable<int> AdmissionDocTypeID { get; set; }
        public Nullable<System.DateTime> DateSaved { get; set; }
        public string TimeSaved { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public Nullable<int> Gang_Membership_Id { get; set; }
        public Nullable<int> Tatoo_Id { get; set; }
        public string Document_Name { get; set; }
    
        public virtual apl_Cyca_Admission_DocumentType apl_Cyca_Admission_DocumentType { get; set; }
        public virtual CYCA_Admissions_AdmissionDetails CYCA_Admissions_AdmissionDetails { get; set; }
    }
}
