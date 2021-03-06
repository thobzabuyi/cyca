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
    
    public partial class CYCA_Admissions_GangMembership
    {
        public int Gang_Membership_Id { get; set; }
        public Nullable<int> Gang_Membership_Type_Id { get; set; }
        public Nullable<int> Admission_Id { get; set; }
        public bool Is_Member { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Date_Captured { get; set; }
        public Nullable<int> ReAdmission_Id { get; set; }
        public string Membership_Rank { get; set; }
        public Nullable<int> Document_Type_Id { get; set; }
        public string OtherDocTypeDescription { get; set; }
        public string OtherGangMemberDescription { get; set; }
    
        public virtual apl_Cyca_Gang_Membership_Type apl_Cyca_Gang_Membership_Type { get; set; }
        public virtual CYCA_Admissions_AdmissionDetails CYCA_Admissions_AdmissionDetails { get; set; }
        public virtual CYCA_ReAdmissionDetails CYCA_ReAdmissionDetails { get; set; }
    }
}
