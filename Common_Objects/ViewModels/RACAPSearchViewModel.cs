using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class RACAPSearchViewModel
    {
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_Client_Ref_No { get; set; }
        public string Search_Client_ID_No { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Person> Person_List { get; set; }
        public int Selected_Person_Id { get; set; }
    }

    public class RACAPCaseListViewModel
    {
        public Address PhysicalAddress { get; set; }
        public Address PostalAddress { get; set; }
        public string Search_Intake_Ref_No { get; set; }
        public string Search_PCM_Ref_No { get; set; }
        public string Search_First_Name { get; set; }
        public string Search_Last_Name { get; set; }
        public string Search_ID_Number { get; set; }
        public string Search_ID_Number_Second_Parent { get; set; }
        public string Search_Date_Of_Birth { get; set; }
        public List<Client> Client_List { get; set; }
        public int Selected_Client_Id { get; set; }
        public int ChildOrParent { get; set; }
        public List<RACAPCaseGridMain> Clients_Case_List { get; set; }
        //public List<RACAPWorkListVM> NewCasezz { get; set; }
        //public List<RACAPWorkListVM> CurrentCases_RACAP { get; set; }
        public List<RACAPCaseGridMain> Clients_Case_List_RACAP { get; set; }


    }
    public partial class RACAPSecondParent
    {
        public string Address_Line_1Phy { get; set; }
        public string Address_Line_2Phy { get; set; }
        public string Postal_CodePhy { get; set; }
        

        public int Selected_Province_Id { get; set; }
        public int Selected_Municipality_Id { get; set; }
        public int Selected_Local_Municipality_Id { get; set; }
        public int Selected_Province_IdPhy { get; set; }
        public int Selected_Municipality_IdPhy { get; set; }
        public int Selected_Local_Municipality_IdPhy { get; set; }
        public Address PhysicalAddress { get; set; }
        public Address PostalAddress { get; set; }
        public int ClientId { get; set; }
        [Display(Name = "First Name")]

        public string FirstName { get; set; }
        [Display(Name = "Last Name")]

        public string LastName { get; set; }
        public string KnownAs { get; set; }
        [Display(Name = "Identity Number")]
        public string IDNumber { get; set; }
        [Display(Name = "Identity Type")]
        public string Identification_Type_Id_ParentTwo { get; set; }
        public int? RACAP_Prospective_Parent_Id { get; set; }
        public int? RACAP_Adoptive_Child_Id { get; set; }

        public int? Age { get; set; }
        [Display(Name = "Language")]
        public int? Language_Id_ParentTwo { get; set; }
        public int? Gender_Id { get; set; }
        public int? Population_Group_Id { get; set; }
        public int? Religion_Id { get; set; }
        public int? Marital_Status_Id { get; set; }
        public int? Nationality_Id { get; set; }
        public string Email_Address2P { get; set; }
        public string Mobile_Phone_Number { get; set; }


        [Display(Name = "Estimated Age")]
        public bool Is_Estimated_Age { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date_Of_Birth_ParentTwo { get; set; }
        public int AssessmentCount { get; set; }
        public int CaseCount { get; set; }
        public DateTime IntakeAssessmentDate { get; set; }
        //public int ClientId { get; set; }
        public int IntakeAssessmentId { get; set; }
        //public DateTime IntakeAssessmentDate { get; set; }

        public string RACAPRecordStatusDescription { get; set; }
        public string RecordStatusDescription { get; set; }

        public int? CaseId { get; set; }
        public int? Problem_Sub_Category_Id { get; set; }
        public DateTime? CaseDate { get; set; }
        public string PCMReferenceNumber { get; set; }
        public string DetensionPlace { get; set; }
        public RACAPPersonViewModel RACAPPersonViewModel { get; set; }

    }

    public partial class ResultsOfParentsInCPR
    {
        public string ParentOne { get; set; }
        public string ParentTwo { get; set; }
    }

}
