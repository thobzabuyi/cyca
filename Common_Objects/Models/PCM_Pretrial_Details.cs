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
    
    public partial class PCM_Pretrial_Details
    {
        public int PCM_Pretrial_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> PCM_Case_Id { get; set; }
        public string Educational_Summary { get; set; }
        public string Offence_Sammary { get; set; }
        public string Victims_Sammary { get; set; }
        public string PCM_Pretrial_Date { get; set; }
        public Nullable<int> PCM_Court_Outcome_Id { get; set; }
        public string PCM_Commemts { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    }
}
