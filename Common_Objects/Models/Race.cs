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
    
    public partial class Race
    {
        public Race()
        {
            this.Employees = new HashSet<Employee>();
            this.apl_Social_Worker = new HashSet<Social_Worker>();
        }
    
        public int Race_Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Definition { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Social_Worker> apl_Social_Worker { get; set; }
    }
}
