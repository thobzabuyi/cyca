﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class RACAPReportVM
    {
        public int Report_Id { get; set; }
        public string Province { get; set; }
        public string ChildLocation { get; set; }
        public string RecordStatus { get; set; }

        public int? Age { get; set; }
        public string Gender { get; set; }
        public string PopulationGroup { get; set; }
        public string ReasonForAdoption { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateRegistered { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SixtyDaysPeriod { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]

        public DateTime WithinSixtyDaysPeriod { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]

        public DateTime ThreeYearPeriodLapsed { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]

        public DateTime ThreeYearPeriodWithin { get; set; }

        public string SpecialNeeds { get; set; }
        public string ChildPreferences { get; set; }

        public string MaritalStatus { get; set; }

        public string TypeOfAdoption { get; set; }

        public string FacilitatingOrganisation { get; set; }
        public int TotalChildren { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; }

        public List<RACAPReportVM> RetrieveFirstReport{ get; set; }

    }
}
