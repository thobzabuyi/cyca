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
    
    public partial class ACM_AbuseDetails
    {
        public int AbuseDetails_Id { get; set; }
        public Nullable<int> Reportable_Incident { get; set; }
        public Nullable<System.DateTime> DateOfAbuse { get; set; }
        public string PlaceOfAbuse { get; set; }
        public Nullable<int> TypeOfInjury { get; set; }
        public string BriefExplanationOfIncident { get; set; }
        public string BriefExplanationOfMedicalIntervention { get; set; }
        public string AllegedAbuserFirstName { get; set; }
        public string AllegedAbuserSurname { get; set; }
        public string AllegedAbuserIDNumber { get; set; }
        public string AllegedAbuserDateOfBirth { get; set; }
        public Nullable<int> AllegedAbuserEstimatedAge { get; set; }
        public string AllegedAbuserGender { get; set; }
        public int CaseWorklist_Id { get; set; }
    
        public virtual ACM_CaseWorkList ACM_CaseWorkList { get; set; }
    }
}
