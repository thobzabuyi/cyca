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
    
    public partial class Incident_Monitoring_Item
    {
        public Incident_Monitoring_Item()
        {
            this.Incident_Monitoring_ChildrensCourt_Extension_Items = new HashSet<Incident_Monitoring_ChildrensCourt_Extension>();
            this.Incident_Monitoring_CriminalCourt_Extension_Items = new HashSet<Incident_Monitoring_CriminalCourt_Extension>();
            this.Incident_Monitoring_Form36_Extension_Items = new HashSet<Incident_Monitoring_Form36_Extension>();
            this.Incident_Monitoring_Normal_Extension_Items = new HashSet<Incident_Monitoring_Normal_Extension>();
        }
    
        public int Incident_Monitoring_Id { get; set; }
        public int Incident_Id { get; set; }
        public bool Is_Normal_Monitoring_Active { get; set; }
        public bool Is_Form36_Monitoring_Active { get; set; }
        public bool Is_Childrens_Court_Monitoring_Active { get; set; }
        public bool Is_Criminal_Court_Monitoring_Active { get; set; }
        public Nullable<System.DateTime> Normal_21Days_Date { get; set; }
        public Nullable<System.DateTime> Normal_30Days_Date { get; set; }
        public Nullable<System.DateTime> Normal_60Days_Date { get; set; }
        public Nullable<System.DateTime> Form36_48Hours_Date { get; set; }
        public Nullable<System.DateTime> Form36_Weekly_Date { get; set; }
        public Nullable<System.DateTime> Form36_Monthly_Date { get; set; }
        public Nullable<System.DateTime> Childrens_Court_3Months_Date { get; set; }
        public Nullable<System.DateTime> Childrens_Court_6Months_Date { get; set; }
        public Nullable<System.DateTime> Criminal_Court_1Year_Date { get; set; }
        public Nullable<System.DateTime> Criminal_Court_2Year_Date { get; set; }
        public bool Is_Normal_Monitoring_Switched_Off { get; set; }
        public bool Is_Form36_Monitoring_Switched_Off { get; set; }
        public bool Is_Childrens_Court_Monitoring_Switched_Off { get; set; }
        public bool Is_Criminal_Court_Monitoring_Switched_Off { get; set; }
        public Nullable<System.DateTime> Normal_Monitoring_Off_Date { get; set; }
        public Nullable<System.DateTime> Form36_Monitoring_Off_Date { get; set; }
        public Nullable<System.DateTime> Childrens_Court_Monitoring_Off_Date { get; set; }
        public Nullable<System.DateTime> Criminal_Court_Monitoring_Off_Date { get; set; }
        public string Normal_Monitoring_Off_Reason { get; set; }
        public string Form36_Monitoring_Off_Reason { get; set; }
        public string Childrens_Court_Monitoring_Off_Reason { get; set; }
        public string Criminal_Court_Monitoring_Off_Reason { get; set; }
    
        public virtual CPR_Incident CPR_Incident { get; set; }
        public virtual ICollection<Incident_Monitoring_ChildrensCourt_Extension> Incident_Monitoring_ChildrensCourt_Extension_Items { get; set; }
        public virtual ICollection<Incident_Monitoring_CriminalCourt_Extension> Incident_Monitoring_CriminalCourt_Extension_Items { get; set; }
        public virtual ICollection<Incident_Monitoring_Form36_Extension> Incident_Monitoring_Form36_Extension_Items { get; set; }
        public virtual ICollection<Incident_Monitoring_Normal_Extension> Incident_Monitoring_Normal_Extension_Items { get; set; }
    }
}