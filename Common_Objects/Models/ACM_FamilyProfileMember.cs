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
    
    public partial class ACM_FamilyProfileMember
    {
        public int FamilyProfileMember_Id { get; set; }
        public Nullable<int> FamilyProfile_Id { get; set; }
        public Nullable<int> Person_Id { get; set; }
    
        public virtual ACM_FamilyProfile ACM_FamilyProfile { get; set; }
    }
}