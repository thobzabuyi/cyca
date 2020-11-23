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
    
    public partial class RACAP_Prospective_Parent
    {
        public RACAP_Prospective_Parent()
        {
            this.RACAP_Matches = new HashSet<RACAP_Matches>();
        }
    
        public int RACAP_Prospective_Parent_Id { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> Client_Module_Id { get; set; }
        public Nullable<int> RACAP_Case_Id { get; set; }
        public Nullable<int> Organization_Id { get; set; }
        public Nullable<int> Special_Needs_Id { get; set; }
        public Nullable<int> Ethnic_Cultural_Group_Id { get; set; }
        public Nullable<int> Eye_Color_Id { get; set; }
        public Nullable<int> Body_Structure_Id { get; set; }
        public Nullable<int> Skin_Color_Id { get; set; }
        public Nullable<int> Religion_Id { get; set; }
        public Nullable<int> Language_Id { get; set; }
        public Nullable<int> Gender_Id { get; set; }
        public Nullable<int> Race_Id { get; set; }
        public Nullable<int> Age_From { get; set; }
        public Nullable<int> Age_To { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
        public Nullable<int> Population_Group_Id { get; set; }
        public Nullable<int> Disability_Id { get; set; }
        public Nullable<int> Race_Id_Option2 { get; set; }
    
        public virtual Intake_Assessment int_Intake_Assessment { get; set; }
        public virtual ICollection<RACAP_Matches> RACAP_Matches { get; set; }
        public virtual RACAP_Case_Details RACAP_Case_Details { get; set; }
    }
}
