using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMPretrailViewModel
    {
        public int? Intake_Assessment_Id { get; set; }

        public string Preliminary_Assessment { get; set; }
        public string Presenting_Problem { get; set; }
        public DateTime? Assessment_Date { get; set; }

        //public int? Intake_Assessment_Id { get; set; }

        public int PCM_Pretrial_Id { get; set; }
        public int? PCM_Case_Id { get; set; }
        public string Educational_Summary { get; set; }
        public string Offence_Sammary { get; set; }
        public string Victims_Sammary { get; set; }
        public int? Client_Id { get; set; }
        public int? PCM_Preliminary_Id { get; set; }
        public string PCM_Pretrial_Date { get; set; }
        public int?  PCM_Court_Outcome_Id { get; set; }
        public string selectedOutcome { get; set; }
        public int? PCM_Offence_Id { get; set; }
        public int? PCM_Recommendation_Id { get; set; }
        public string PCM_Commemts { get; set; }
        public int Created_By { get; set; }
        public DateTime? Date_Created { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Date_Modified { get; set; }
        public virtual ICollection<OutcomeTypeLookup> Outcome_Type { get; set; }

        //recommendation
        //public int? PCM_Recommendation_Id { get; set; }
        //public string Recommendation { get; set; }
        //public virtual ICollection<PCMPretrailViewModel> rec { get; set; }
    }

    public class OutcomeType
    {
        public int? selectedOutcome { get; set; }
        public IEnumerable<OutcomeTypeLookup> Outcome_Type_List { get; set; }
    }

    public class OutcomeTypeLookup
    {
        public int PCM_Court_Outcome_Id { get; set; }
        public string PCM_Court_Outcome { get; set; }
    }
}
