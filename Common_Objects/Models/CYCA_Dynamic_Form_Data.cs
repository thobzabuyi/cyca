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
    
    public partial class CYCA_Dynamic_Form_Data
    {
        public int Dynamic_Form_Data_Id { get; set; }
        public int Dynamic_Form_Id { get; set; }
        public int Client_Id { get; set; }
        public int User_Id { get; set; }
        public string Data { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int Venue_Id { get; set; }
    
        public virtual User apl_User { get; set; }
        public virtual CYCA_Dynamic_Form CYCA_Dynamic_Form { get; set; }
        public virtual Client int_Client { get; set; }
    }
}
