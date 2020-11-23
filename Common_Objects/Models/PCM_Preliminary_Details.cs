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
    
    public partial class PCM_Preliminary_Details
    {
        public int PCM_Preliminary_Id { get; set; }
        public Nullable<int> PCM_Case_Id { get; set; }
        public Nullable<int> Client_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public string PreInquiryConducted { get; set; }
        public string ReasonPreInquiryConducted { get; set; }
        public Nullable<System.DateTime> PCM_Preliminary_Date { get; set; }
        public Nullable<int> PCM_Preliminary_Status_Id { get; set; }
        public string PCM_Outcome_Reason { get; set; }
        public Nullable<int> PCM_Offence_Id { get; set; }
        public Nullable<int> PCM_Recommendation_Id { get; set; }
        public Nullable<int> Placement_Pre_Status_Id { get; set; }
        public Nullable<int> Placement_Pre_Recommended_Id { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    
        public virtual PCM_Case_Details PCM_Case_Details { get; set; }
        public virtual PCM_Offence_Details PCM_Offence_Details { get; set; }
    }
}
