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
    
    public partial class Profiling_Answer
    {
        public int Profiling_Answer_Id { get; set; }
        public int Questionnaire_Question_Id { get; set; }
        public int Questionnaire_Question_Column_Id { get; set; }
        public Nullable<int> Household_Member_Number { get; set; }
        public string Answer_Value { get; set; }
        public int Profiling_Instance_Id { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
    
        public virtual Questionnaire_Question NISIS_Questionnaire_Question { get; set; }
        public virtual Profiling_Instance NISIS_Profiling_Instance { get; set; }
        public virtual Questionnaire_Question_Column NISIS_Questionnaire_Question_Column { get; set; }
    }
}
