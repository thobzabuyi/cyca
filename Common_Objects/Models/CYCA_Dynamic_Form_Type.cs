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
    
    public partial class CYCA_Dynamic_Form_Type
    {
        public CYCA_Dynamic_Form_Type()
        {
            this.CYCA_Dynamic_Form = new HashSet<CYCA_Dynamic_Form>();
        }
    
        public int Dynamic_Form_Type_Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<CYCA_Dynamic_Form> CYCA_Dynamic_Form { get; set; }
    }
}
