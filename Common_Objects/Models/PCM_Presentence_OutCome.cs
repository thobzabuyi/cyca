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
    
    public partial class PCM_Presentence_OutCome
    {
        public int Sentence_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Suspended_Postponed_Sentence_Id { get; set; }
        public Nullable<System.DateTime> Court_Date { get; set; }
        public string Reason_for_Remand { get; set; }
        public Nullable<System.DateTime> NextCourtDate { get; set; }
        public string Court_Outcome { get; set; }
        public Nullable<bool> Community_Based { get; set; }
        public Nullable<int> Community_Based_Options_Id { get; set; }
        public Nullable<int> Restorective_Justice_Option_Id { get; set; }
        public Nullable<int> Programme_Type_Id { get; set; }
        public Nullable<int> Programme_Id { get; set; }
        public Nullable<bool> Fine_or_Alternatives_To_Fine { get; set; }
        public string Fine_Alternatives_Fine_Comments { get; set; }
        public Nullable<bool> Commital_Treatment_Centre { get; set; }
        public string Center_Name { get; set; }
        public Nullable<System.DateTime> Period_Commital_Treatment_Centre_From { get; set; }
        public Nullable<System.DateTime> Period_Commital_Treatment_Centre_To { get; set; }
        public Nullable<bool> Compulsory_esidence_CYCC { get; set; }
        public string Center_Name_CYCC { get; set; }
        public Nullable<System.DateTime> Compulsory_esidence_CYCC_From { get; set; }
        public Nullable<System.DateTime> Compulsory_esidence_CYCC_To { get; set; }
        public Nullable<bool> Imprisoment { get; set; }
        public Nullable<int> Imprisoment_Id { get; set; }
        public Nullable<System.DateTime> Imprisomen_From { get; set; }
        public Nullable<System.DateTime> Imprisomen_To { get; set; }
        public Nullable<int> Department_Id { get; set; }
        public Nullable<int> PCM_Case_Status_Id { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    }
}
