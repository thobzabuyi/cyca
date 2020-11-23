//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDIIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Service_Office
    {
        public Service_Office()
        {
            this.Employees = new HashSet<Employee>();
        }
    
        public int Service_Office_Id { get; set; }
        public Nullable<int> Area_Id { get; set; }
        public string Description { get; set; }
        public string Office_Size { get; set; }
        public Nullable<bool> Is_Rental_Office { get; set; }
        public Nullable<decimal> Monthly_Rent { get; set; }
        public Nullable<decimal> Vat { get; set; }
        public Nullable<decimal> Escalation_Rate { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public Nullable<bool> Is_Active { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual Area Area { get; set; }
    }
}