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
    
    public partial class PCM_Diversion
    {
        public PCM_Diversion()
        {
            this.PCM_Diversion_Outcomes = new HashSet<PCM_Diversion_Outcomes>();
        }
    
        public int Diversion_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Source_Referral_Id { get; set; }
        public Nullable<int> Province_Id { get; set; }
        public Nullable<int> Service_Provider_Category { get; set; }
        public Nullable<int> Services_Provider_Id { get; set; }
        public Nullable<int> Programme_Category_Id { get; set; }
        public Nullable<int> Programme_id { get; set; }
        public Nullable<System.DateTime> Programme_Start_Date { get; set; }
        public Nullable<System.DateTime> Programme_End_Date { get; set; }
        public Nullable<int> Court_Type_Id { get; set; }
        public Nullable<int> No_Modules { get; set; }
        public Nullable<int> Programme_Level_Id { get; set; }
        public Nullable<int> Programme_AgeGroup_Id { get; set; }
        public Nullable<int> PCM_Preliminary_Id { get; set; }
        public Nullable<int> Formal_Courtcome_Id { get; set; }
        public Nullable<int> Childrens_Court_Outcome_Id { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    
        public virtual PCM_D_ServicesProvider PCM_D_ServicesProvider { get; set; }
        public virtual ICollection<PCM_Diversion_Outcomes> PCM_Diversion_Outcomes { get; set; }
    }
}
