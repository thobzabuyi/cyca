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
    
    public partial class AdoptionSupportingDocument
    {
        public int DocumentsId { get; set; }
        public Nullable<int> DocumentsLabel { get; set; }
        public string DocumentsFileName { get; set; }
        public string DocumentsPath { get; set; }
        public string DocumentsDescription { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> DocumentsCheck { get; set; }
    }
}
