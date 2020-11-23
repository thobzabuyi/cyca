using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
  public class ACMSummaryOfDevelopmentAreaModel
    {
        public int DevelopmentArea_Id { get; set; }
        public string DevelopmentAreaName { get; set; }
        public string DesiredOutcome { get; set; }
        public string WhatWillbeDone { get; set; }
        public string WhoWillDoThis { get; set; }
        public Nullable<System.DateTime>  ByWhen { get; set; }
        public int CaseWorklist_Id { get; set; }

    }
}
