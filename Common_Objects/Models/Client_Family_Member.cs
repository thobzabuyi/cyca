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
    
    public partial class Client_Family_Member
    {
        public int Client_Family_Member_Id { get; set; }
        public int Client_Id { get; set; }
        public int Person_Id { get; set; }
        public Nullable<int> Relationship_Type_Id { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Person Person { get; set; }
        public virtual Relationship_Type Relationship_Type { get; set; }
    }
}
