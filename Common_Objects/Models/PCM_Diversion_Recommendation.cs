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
    
    public partial class PCM_Diversion_Recommendation
    {
        public int PCM_Diversion_Recomm_Id { get; set; }
        public Nullable<int> PCM_Recommendation_Id { get; set; }
        public Nullable<int> Service_Provider_Id { get; set; }
        public Nullable<int> Recommendation_Programmes_Id { get; set; }
        public Nullable<int> Level_Id { get; set; }
        public Nullable<int> Order_Id { get; set; }
        public Nullable<int> Personal_Details_Id { get; set; }
        public Nullable<int> Type_Of_Center_Id { get; set; }
        public Nullable<int> Type_Of_Placement_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
        public Nullable<bool> Withdrawal_Status { get; set; }
    
        public virtual PCM_Recommendation PCM_Recommendation { get; set; }
    }
}
