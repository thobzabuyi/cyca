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
    
    public partial class VEP_IncidentInformation
    {
        public int IncidentID { get; set; }
        public Nullable<int> Caseid { get; set; }
        public string Incident_Details { get; set; }
        public string Victimisation_Type { get; set; }
        public string PlaceofIncident { get; set; }
        public string TypeofSettlement { get; set; }
        public Nullable<System.DateTime> DateofIncident { get; set; }
        public Nullable<System.DateTime> DateofReporting { get; set; }
        public string DoyouknowtheallegedPerpetrator { get; set; }
        public string Perpetratorrelatedtoyou { get; set; }
        public string immediatecommunity { get; set; }
        public string ReportedtothePolice { get; set; }
        public string PoliceCaseNumber { get; set; }
        public string PoliceOBnumber { get; set; }
    }
}
