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
    
    public partial class ADOPT_OrgansationResponsible
    {
        public int Adopt_Responsible_Organisation { get; set; }
        public Nullable<int> Organization_Id_Child { get; set; }
        public Nullable<int> Organization_Id_Parent { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Adopt_Case_Id { get; set; }
        public Nullable<int> Organization_Id_SocialWorker { get; set; }
        public Nullable<int> Child_Social_Worker_Id { get; set; }
        public Nullable<int> Parent_Social_Worker_Id { get; set; }
    
        public virtual ADOPT_Case_Details ADOPT_Case_Details { get; set; }
        public virtual Organization apl_Organization { get; set; }
        public virtual Organization apl_Organization1 { get; set; }
    }
}