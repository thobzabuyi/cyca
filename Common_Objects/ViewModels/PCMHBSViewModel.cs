using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMHBSViewModel
    {
        public int HomeBasedSupervision_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public Nullable<int> Conditions_Id { get; set; }
        public string Visitation_Period { get; set; }
        public Nullable<int> Number_of_Visit { get; set; }
        public Nullable<System.DateTime> Placement_Date { get; set; }
        public Nullable<int> HBS_Supervisor_Id { get; set; }
        public string Placement_Confirmed { get; set; }

        public virtual ICollection<HBSConditionTypeLookup> HBSCondition_Type { get; set; }
        public string Conditions { get; set; }

        public virtual ICollection<HBSsupervisorTypeLookup> HBSsupervisor_Type { get; set; }
        public string HBS_Supervisor { get; set; }


        public int HB_Visitaion_Outcome_Id { get; set; }
        public string Conatact_Number { get; set; }
        public string Process_Notes { get; set; }
        public string Visitaion_Register { get; set; }
        public Nullable<int> Compliance_Id { get; set; }

        public int HB_CourtOutcome_Id { get; set; }
        public string Remand { get; set; }
        public DateTime? Next_Court_Date { get; set; }
        public string Reason_Remand { get; set; }
        public string Court_Outcome { get; set; }
        public Nullable<int> HB_Case_Status_Id { get; set; }

        public virtual ICollection<ComplianceTypeLookup> Compliance_Type { get; set; }
        public string Compliance { get; set; }

        public virtual ICollection<CaseStatusTypeLookup> CaseStatus_Type { get; set; }
        public string HB_Case_Status { get; set; }

        public int Condition_Id { get; set; }
        public string School_Attendance { get; set; }
        public string Family_Time { get; set; }
        public string Good_Behaviour { get; set; }
        public string Reporting { get; set; }
        public string Supervision { get; set; }

        public int Court_Type_Id { get; set; }
        public string Court_Type { get; set; }


    }

    public class HBSConditionType
    {
        public int? selectedHBSCondition { get; set; }
        public IEnumerable<HBSConditionTypeLookup> HBSCondition_Type_List { get; set; }
    }

    public class HBSConditionTypeLookup
    {
        public int Conditions_Id { get; set; }
        public string Conditions { get; set; }
    }

    public class HBSsupervisorType
    {
        public int? selectedHBSsupersor { get; set; }
        public IEnumerable<HBSsupervisorTypeLookup> HBSsupervisor_Type_List { get; set; }
    }

    public class HBSsupervisorTypeLookup
    {
        public int HBS_Supervisor_Id { get; set; }
        public string HBS_Supervisor { get; set; }
    }


    public class ComplianceType
    {
        public int? selectedCompliance { get; set; }
        public IEnumerable<ComplianceTypeLookup> Compliance_Type_List { get; set; }
    }

    public class ComplianceTypeLookup
    {
        public int Compliance_Id { get; set; }
        public string Compliance { get; set; }
    }


    public class CaseStatusType
    {
        public int? selectedCaseStatus { get; set; }
        public IEnumerable<CaseStatusTypeLookup> CaseStatus_List { get; set; }
    }

    public class CaseStatusTypeLookup
    {
        public int HB_Case_Status_Id { get; set; }
        public string HB_Case_Status { get; set; }
    }
}
