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
    
    public partial class CYCA_TotalFemalesDischargedPerFacility_Result
    {
        public int Facility_Id { get; set; }
        public int District_Id { get; set; }
        public int Province_Id { get; set; }
        public string MONTH { get; set; }
        public string Gender { get; set; }
        public string FacilityName { get; set; }
        public Nullable<int> Total { get; set; }
    }
}