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
    
    public partial class ACM_Interview_Type
    {
        public ACM_Interview_Type()
        {
            this.ACM_InterviewProcessNote = new HashSet<ACM_InterviewProcessNote>();
        }
    
        public int Interview_Type_Id { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<ACM_InterviewProcessNote> ACM_InterviewProcessNote { get; set; }
    }
}
