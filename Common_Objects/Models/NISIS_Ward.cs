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
    
    public partial class NISIS_Ward
    {
        public NISIS_Ward()
        {
            this.NISIS_Sites = new HashSet<NISIS_Site>();
        }
    
        public int NISIS_Ward_Id { get; set; }
        public int Local_Municipality_Id { get; set; }
        public string Ward_Code { get; set; }
        public string Ward_Number { get; set; }
        public string Description { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Last_Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
    
        public virtual ICollection<NISIS_Site> NISIS_Sites { get; set; }
        public virtual Local_Municipality Local_Municipality { get; set; }
    }
}
