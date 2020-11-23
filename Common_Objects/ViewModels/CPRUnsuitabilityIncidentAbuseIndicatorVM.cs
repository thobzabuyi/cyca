using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common_Objects.ViewModels.CPRUnsuitabilityEditDataModel;

namespace Common_Objects.ViewModels
{
    public class CPRUnsuitabilityIncidentAbuseIndicatorVM
    {
        public int CPR_Unsuitability_Incident_Abuse_Indicator_Id { get; set; }
        public int CPR_Unsuitability_Id { get; set; }
        public int Abuse_Indicator_Id { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public int Created_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public int Modified_By { get; set; }

        public IEnumerable<Abuse_Indicator> AvailableAbuseIndicatorType { get; set; }
        public IList<Abuse_Indicator> SelectedAbuseIndicatorType { get; set; }
        public Posted_AbuseIndicatorType PostedAbuseIndicatorType { get; set; }
    }
}
