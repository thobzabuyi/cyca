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
    
    public partial class Module_Action
    {
        public Module_Action()
        {
            this.Menu_Items = new HashSet<Menu_Item>();
            this.Roles = new HashSet<Role>();
        }
    
        public int Module_Action_Id { get; set; }
        public int Module_Controller_Id { get; set; }
        public string Module_Action_Name { get; set; }
    
        public virtual ICollection<Menu_Item> Menu_Items { get; set; }
        public virtual Module_Controller Module_Controller { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
