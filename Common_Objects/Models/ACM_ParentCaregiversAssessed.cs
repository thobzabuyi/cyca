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
    
    public partial class ACM_ParentCaregiversAssessed
    {
        public int Id { get; set; }
        public int Sasat_Id { get; set; }
        public int Person_Id { get; set; }
        public Nullable<int> Relationship_Type_Id { get; set; }
    
        public virtual Person int_Person { get; set; }
        public virtual Relationship_Type apl_Relationship_Type { get; set; }
        public virtual ACM_SouthAfricanSafetyAssessmentTool ACM_SouthAfricanSafetyAssessmentTool { get; set; }
    }
}
