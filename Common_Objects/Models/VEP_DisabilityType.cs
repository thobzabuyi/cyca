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
    
    public partial class VEP_DisabilityType
    {
        public int Disability_Type_Id { get; set; }
        public string Disability_Type { get; set; }
        public int DisabilityID { get; set; }
    
        public virtual Disability apl_Disability { get; set; }
    }
}
