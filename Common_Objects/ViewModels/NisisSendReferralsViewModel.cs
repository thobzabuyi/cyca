﻿using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class NisisSendReferralsViewModel
    {
        public bool isSelected { get; set; }
        public NISIS_Profiling_Instance_Referral referralItem { get; set; }
    }
}
