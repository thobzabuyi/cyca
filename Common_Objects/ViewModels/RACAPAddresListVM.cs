using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class RACAPAddresListVM
    {
        public int Address_Id { get; set; }
        public int Person_Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string TownS { get; set; }
        public int? Town_Id { get; set; }
        public string PostalCode { get; set; }

        public string ProvinceS { get; set; }
        public string ChildLocationS { get; set; }
        public string RecordStatusS { get; set; }
        public int? Age { get; set; }
        public string GenderS { get; set; }
        public string PopulationGroupS { get; set; }
        public string SpecialNeedsS { get; set; }
        public string ReasonForAdopS { get; set; }
        public string SixtyDaysPeriod { get; set; }
        public DateTime? DateRegistered { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string WithinSixtyDaysPeriod { get; set; }
        public string ThreeYearPeriod { get; set; }
        public string WithinThreeYearPeriod { get; set; }
        public string MaritalStatusS { get; set; }
        public string FacilitationOrgS { get; set; }
        public string ChildPreferencesS { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
