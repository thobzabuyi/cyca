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
    
    public partial class PCM_Referral_SocilaWorker
    {
        public int Referrals_SocilaWorker_Id { get; set; }
        public Nullable<int> Referrals_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Type_Referral_Id { get; set; }
        public Nullable<bool> Referral_Child_To_Social_Worker { get; set; }
        public Nullable<System.DateTime> Programme_Period_From { get; set; }
        public Nullable<System.DateTime> Programme_Period { get; set; }
        public string Child_Progress { get; set; }
        public Nullable<int> How_Often { get; set; }
        public string Copy_To { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    }
}
