using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Common_Objects.ViewModels
{
    public class AdoptionWorkListVM
    {
        public int? Manager { get; set; }
        public string User_Surname { get; set; }
        public int Adopt_CaseWoklist_Id { get; set; }
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

        public virtual AdoptionWorkload ADOPT_Worklist { get; set; }

        public List<AdoptionWorkload> Adoptionlist { get; set; }

        public List<AdoptionNewWorkListVM> newWorklist { get; set; }

        public virtual ICollection<ADOPT_Case_WorkList> AdoptionWorkList { get; set; }
    }

    public class AdoptionNewWorkListVM
    {

        public int Adopt_CaseWoklist_Id { get; set; }
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

        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }

        public virtual Intake_Assessment int_Intake_Assessment { get; set; }
        public virtual ICollection<apl_Adoption_Record_Status> apl_Adoption_Record_Status { get; set; }

        public virtual AdoptionWorkload ADOPT_Worklist { get; set; }

        public List<AdoptionWorkload> Adoptionlist { get; set; }

        public List<AdoptionWorkListVM> newWorklist { get; set; }

        public virtual ICollection<ADOPT_Case_WorkList> AdoptionWorkList { get; set; }
    }


}
