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
    
    public partial class ACM_ChildrenInAlternativeCare
    {
        public int ChildrenInAlternativeCare_Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Gender { get; set; }
        public string ChildID { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public int Client_Id { get; set; }
        public string AlternativeCaregiverAddres { get; set; }
        public string MagisterialDistrict { get; set; }
        public string CourtReferenceNumber { get; set; }
        public string Organization { get; set; }
        public string Reference_Number { get; set; }
        public int ExtensionOfAlternativeCareOrder_Id { get; set; }
    }
}