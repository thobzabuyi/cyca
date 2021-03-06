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
    
    public partial class School
    {
        public School()
        {
            this.Person_Education_Items = new HashSet<Person_Education>();
            this.Addresses = new HashSet<Address>();
        }
    
        public int School_Id { get; set; }
        public Nullable<int> School_Type_Id { get; set; }
        public string School_Name { get; set; }
        public string Contact_Person { get; set; }
        public string Telephone_Number { get; set; }
        public string Cellphone_Number { get; set; }
        public string Fax_Number { get; set; }
        public string Email_Address { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
    
        public virtual School_Type School_Type { get; set; }
        public virtual ICollection<Person_Education> Person_Education_Items { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
