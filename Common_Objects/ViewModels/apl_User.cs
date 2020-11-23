using Common_Objects.Models;
using System;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class apl_User
    {
        public int User_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Initials { get; set; }
        public string Email_Address { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> Date_Last_Login { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public virtual ICollection<CYCA_Team_Allocation> CYCA_Team_Allocation { get; set; }
    }
}