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
    
    public partial class CPR_Correspondence
    {
        public int CPR_Correnspondence_Id { get; set; }
        public int CPR_Correspondence_Letter_Id { get; set; }
        public string Letter_Sent { get; set; }
        public System.DateTime Date_Sent { get; set; }
        public int Sent_To_Person_Id { get; set; }
        public int Sent_By_User_Id { get; set; }
    
        public virtual User apl_User { get; set; }
        public virtual CPR_Correspondence_Letter CPR_Correspondence_Letter { get; set; }
        public virtual Person int_Person { get; set; }
    }
}
