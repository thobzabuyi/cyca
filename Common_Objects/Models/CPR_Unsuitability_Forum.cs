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
    
    public partial class CPR_Unsuitability_Forum
    {
        public int Forum_Id { get; set; }
        public string Forum_Name { get; set; }
        public string Forum_Address { get; set; }
        public int Unsuitability_Id { get; set; }
    
        public virtual CPR_Unsuitability CPR_Unsuitability { get; set; }
    }
}