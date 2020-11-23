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
    
    public partial class CYCA_Admissions_ExtraMuralActivity
    {
        public int Extra_Mural_Activity_Id { get; set; }
        public string Weight { get; set; }
        public string Hobby_Id { get; set; }
        public string Activity_Id { get; set; }
        public Nullable<int> Eye_Color_Id { get; set; }
        public Nullable<int> Hair_Color_Id { get; set; }
        public Nullable<int> Physical_Build_Id { get; set; }
        public Nullable<int> Admission_Id { get; set; }
        public string Description { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
    
        public virtual apl_Cyca_Physical_Build apl_Cyca_Physical_Build { get; set; }
        public virtual Eye_Color apl_Eye_Color { get; set; }
        public virtual Hair_Color apl_Hair_Color { get; set; }
        public virtual CYCA_Admissions_AdmissionDetails CYCA_Admissions_AdmissionDetails { get; set; }
    }
}
