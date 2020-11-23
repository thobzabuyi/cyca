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
    
    public partial class Medical_Detail
    {
        public int Medical_Detail_Id { get; set; }
        public int Incident_Id { get; set; }
        public bool Is_Form9_Completed { get; set; }
        public bool Is_J88_Completed { get; set; }
        public string Practitioner_Name { get; set; }
        public string Practitioner_Contact_Number { get; set; }
        public Nullable<int> Treatment_Type_Id { get; set; }
        public Nullable<int> Treatment_Given_By_Id { get; set; }
        public Nullable<int> Treatment_Place_Id { get; set; }
        public Nullable<System.DateTime> Treatment_Date { get; set; }
        public string Treatment_Details { get; set; }
        public bool Is_Medical_Followup { get; set; }
    
        public virtual CPR_Incident CPR_Incident { get; set; }
        public virtual Treatment_Type Treatment_Type { get; set; }
        public virtual Treatment_Given_By Treatment_Given_By { get; set; }
        public virtual Treatment_Place Treatment_Place { get; set; }
    }
}