using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEPInboxVM
    {
      
        public int? Intake_Assessment_Id { get; set; }
        public int Client_Id { get; set; }
        public DateTime? Assessment_Date { get; set; }
        public int? Assessed_By_Id { get; set; }
        public int? Case_Manager_Supervisor_Id { get; set; }
        public int Case_Manager_Id { get; set; }
        public string Preliminary_Assessment { get; set; }
        public string Presenting_Problem { get; set; }
        public int? Problem_Sub_Category_Id { get; set; }
        public bool Is_Closed { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? Treatment_Date { get; set; }
        public Nullable<int> Intervention_Id { get; set; }
        public bool Is_Referred_For_Assessment { get; set; }
        public bool Is_Referred_To_Other_Service_Provider { get; set; }
        public string Case_Background { get; set; }
        public string Supervisor_Comments { get; set; }
        public string Social_Worker_Comments { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public DateTime? Date_Last_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public bool Is_Priority_Intervention { get; set; }
        public string Case_Allocation_Comments { get; set; }
        public DateTime? Date_Allocated { get; set; }
        public int? Allocated_TO { get; set; }
        public int? Allocated_By { get; set; }
        public DateTime? Date_Due { get; set; }
        public string VEpRecordStatusDescription { get; set; }



        public int Person_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Known_As { get; set; }
        public int? Identification_Type_Id { get; set; }
        public string Identification_Number { get; set; }
        public bool Is_Piva_Validated { get; set; }
        public string Piva_Transaction_Id { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public int? Age { get; set; }
        public bool Is_Estimated_Age { get; set; }
        public int? Language_Id { get; set; }
        public int? Gender_Id { get; set; }
        public string Description { get; set; }
        public int? Marital_Status_Id { get; set; }
       
       
        public string Reference_Number { get; set; }

        public int? Status_Id { get; set; }
        public string Case_Status { get; set; }
        public int? VEP_Record_Status { get; set; }
        public DateTime? Date_Acknowledge { get; set; }
        public DateTime? Date_Accepted { get; set; }
        public int Accepted_By { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public string VEPProblemSubCategoryDescription { get; set; }


        public List<VEPInboxVM> NewCasez { get; set; }

        public List<VEPInboxVM> CurrentCases { get; set; }

        public List<AdoptCaseGridMain> Clients_Case_List { get; set; }
    }
}
