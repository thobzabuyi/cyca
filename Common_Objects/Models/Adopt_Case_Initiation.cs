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
    
    public partial class Adopt_Case_Initiation
    {
        public int Case_Initate_Id { get; set; }
        public Nullable<int> Client_Module_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Client_Id { get; set; }
        public Nullable<int> Client_Adoptive_Parent_Id { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    
        public virtual Intake_Assessment int_Intake_Assessment { get; set; }
        public virtual int_Client_Module_Registration int_Client_Module_Registration { get; set; }
    }
}
