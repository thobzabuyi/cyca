using Common_Objects.Models;
using System;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class PCMSocialWorkerWorkListVM
    {
        

        public int PCMCaseWoklist_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Allocated_By { get; set; }
        public int? Allocate_To { get; set; }
        public DateTime? Date_Allocated { get; set; }
        public string Reference_Number { get; set; }
        public int? Adopt_Record_Status_Id { get; set; }
        public string RecordStatusDescription { get; set; }
        public DateTime? Date_Accepted { get; set; }
        public DateTime? AssessmentDate { get; set; }
        public int? Accepted_By { get; set; }
        public int? Case_Manager_Id { get; set; }

        public string ProblemSubCategoryDescription { get; set; }
        public int Problem_Sub_Category_Id { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }

        public int gender { get; set; }

        public virtual Intake_Assessment int_Intake_Assessment { get; set; }
        public virtual ICollection<apl_Adoption_Record_Status> apl_Adoption_Record_Status { get; set; }

        public virtual ICollection<Problem_Sub_Category> Problem_Sub_Category { get; set; }

        public virtual ICollection<PCM_WorkList> PCMnWorkList { get; set; }
        public DateTime? Assessment_Date { get; set; }
    
        public int Assesment_Register_Id { get; set; }

        public int Probation_Officer_Id { get; set; }

        public string POName { get; set; }


    }

    public class PCMMainPageWorklistModelVM
    { 
        public int SelectedTab { get; set; }
    public string PCMnewCaseSearch { get; set; }

        public string PCMcurrentCaseSearch { get; set; }

        public string Search_Intake_Ref_No { get; set; }
        public string Search_PCM_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Identification_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<PCMSocialWorkerWorkListVM> PCMCurrentCases { get; set; }

        public List<PCMCaseGridMain> PCMClients_Case_List { get; set; }

        public List<PCMSocialWorkerWorkListVM> PCMNewCasez { get; set; }

        public List<PCMSocialWorkerWorkListVM> PCMallassessment { get; set; }

        public List<PCMSocialWorkerEndpointCasesVM> PCMEndPointAllocatedCasez { get; set; }

    }


    public class PCMCaseListViewModel
    {
        public string Search_Intake_Ref_No { get; set; }
        public string Search_PCM_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Identification_Number { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Client> Client_List { get; set; }
        public int Selected_Client_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public int? Client_Id { get; set; }
        public List<PCMCaseGridMain> PCMClients_Case_List { get; set; }

        public List<PCMCaseGridMain> PCMallassessment { get; set; }
    }

    public class PCMSocialWorkerEndpointCasesVM
    {


        public int End_Point_POD_Id { get; set; }
        public int? CaseInformationID { get; set; }
        public int? Person_Id { get; set; }

        public int NotificacationId { get; set; }
        public int? POAllocate_To { get; set; }

        public string PONmae_To { get; set; }
        public DateTime? Date_Recieved { get; set; }
        public string Reference_Number { get; set; }
        public int? Endpoint_Record_Status_Id { get; set; }
        public string EndpointRecordStatusDescription { get; set; }
        public DateTime? Date_Accepted { get; set; }
        public DateTime? AssessmentDate { get; set; }
        public string FirstNameE { get; set; }
        public string LastNameE { get; set; }
        public string IDNumberE { get; set; }

        public int genderE { get; set; }

        public int? Probation_Officer_Id { get; set; }

        public string POName { get; set; }
        public int? Allocated_By { get; set; }
        public string Allocated_ByName { get; set; }

    }
}
