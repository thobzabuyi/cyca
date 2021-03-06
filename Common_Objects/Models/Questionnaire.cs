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
    
    public partial class Questionnaire
    {
        public Questionnaire()
        {
            this.Questionnaire_Sections = new HashSet<Questionnaire_Section>();
        }
    
        public int Questionnaire_Id { get; set; }
        public int Profiling_Tool_Id { get; set; }
        public string Description { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
    
        public virtual Profiling_Tool Profiling_Tool { get; set; }
        public virtual ICollection<Questionnaire_Section> Questionnaire_Sections { get; set; }
    }
}
