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
    
    public partial class PCM_Assessment_Register
    {
        public PCM_Assessment_Register()
        {
            this.PCM_Recommendation = new HashSet<PCM_Recommendation>();
        }
    
        public int Assesment_Register_Id { get; set; }
        public Nullable<int> PCM_Case_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Probation_Officer_Id { get; set; }
        public Nullable<int> Assessed_By { get; set; }
        public Nullable<System.DateTime> Assessment_Date { get; set; }
        public Nullable<System.DateTime> Assessment_Time { get; set; }
        public Nullable<int> Form_Of_Notification_Id { get; set; }
        public Nullable<int> Town_Id { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    
        public virtual ICollection<PCM_Recommendation> PCM_Recommendation { get; set; }
    }
}
