using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEPIncidentInformationViewModel
    {
        public int IncidentID { get; set; }
        public int Caseid { get; set; }
        public string Victimisation_Type { get; set; }
        public string PlaceofIncident { get; set; }
        public string TypeofSettlement { get; set; }
        public DateTime DateofIncident { get; set; }
        public DateTime DateofReporting { get; set; }
        public string DoyouknowtheallegedPerpetrator { get; set; }
        public string Perpetratorrelatedtoyou { get; set; }
        public string immediatecommunity { get; set; }
        public string ReportedtothePolice { get; set; }
    }
}
