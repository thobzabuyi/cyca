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
    
    public partial class PCM_Health_Details
    {
        public int Health_Details_Id { get; set; }
        public int Health_Status_Id { get; set; }
        public string Injuries { get; set; }
        public string Medication { get; set; }
        public string Allergies { get; set; }
        public Nullable<System.DateTime> Medical_Appointments { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    }
}
