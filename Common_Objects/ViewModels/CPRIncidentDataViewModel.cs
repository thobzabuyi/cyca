using System.Collections.Generic;
using Common_Objects.Models;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class CPRIncidentDataViewModel
    {
        public Client Client { get; set; }
        public Person Person { get; set; }
        public CPR_Incident Incident { get; set; }
        public Address Incident_Location_Address { get; set; }
        public Medical_Detail Incident_Medical_Detail { get; set; }
        public CPR_First_Reporter CPR_First_Reporter { get; set; }
        public List<Alleged_Offender> Alleged_Offenders { get; set; }
        public List<CPR_Informant> CPR_Informants { get; set; }
        public CPRFirstReporterSearchViewModel SearchPersonFirstReporter { get; set; }
        public PersonDetailViewModel FirstReporterCreatePerson { get; set; }
        public CPR_SAPS_Detail SAPS_Detail { get; set; }
        public CPR_Childrens_Court_Detail Childrens_Court_Detail { get; set; }
        public Incident_Monitoring_Item Incident_Monitoring { get; set; }
        public int Selected_Abuse_Type_Id { get; set; }
        //public IList<Abuse_Indicator> SelectedAbuseIndicatorType { get; set; }
        //public Posted_IncidentAbuseIndicatorType PostedIncidentAbuseIndicatorType { get; set; }

        public class Posted_IncidentAbuseIndicatorType
        {
            public int[] IncidentAbuseIndicatorTypeIDs { get; set; }

            public IEnumerable<SelectListItem> ListOfIncidentAbuseIndicatorTypeIDs { get; set; }
        }


    }
}
