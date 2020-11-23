using Common_Objects.Models;
using System;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class RACAPWorkListVM
    {

        public int RACAP_CaseWoklist_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Allocated_By { get; set; }
        public int? Allocate_To { get; set; }
        public int? Allocate_To_2 { get; set; }
        public DateTime? Date_Allocated { get; set; }
        public string Reference_Number { get; set; }
        public int? RACAP_Record_Status_Id { get; set; }

        public int? ClientId { get; set; }
        public string RecordStatusDescription { get; set; }
        public int? Problem_Sub_Category_Id { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public string Search_Intake_Ref_No { get; set; }
        public string Search_PCM_Ref_No { get; set; }
        public string ProblemSubCategoryDescription { get; set; }
        public DateTime? Date_Accepted { get; set; }
        public DateTime? AssessmentDate { get; set; }
        public int? Accepted_By { get; set; }
        public int? Case_Manager_Id { get; set; }

        public int PersonId { get; set; }
        public int AssessmentCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string IDNumber { get; set; }


        public List<RACAPCaseGridMain> Clients_Case_List { get; set; }
        //public List<RACAPWorkListVM> NewCasezz { get; set; }
        //public List<RACAPWorkListVM> CurrentCases_RACAP { get; set; }
        public List<RACAPCaseGridMain> Clients_Case_List_RACAP { get; set; }
        public virtual Intake_Assessment int_Intake_Assessment { get; set; }
        public virtual ICollection<apl_RACAP_Record_Status> apl_RACAP_Record_Status { get; set; }

        public virtual RACAPWorkload RACAP_Worklist { get; set; }

        public List<RACAPWorkload> RACAPlist { get; set; }


        public virtual ICollection<RACAP_Case_WorkList> RACAPWorkList { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }


}
