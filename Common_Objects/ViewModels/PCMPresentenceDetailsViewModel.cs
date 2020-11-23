using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Common_Objects.ViewModels
{
    public class PCMPresentenceDetailsViewModel
    {
        #region Presentence Sammary

        public int Presentence_Summary_Id { get; set; }
        [Display(Name = "Was Child Assessed")]
        public string IsAssessed { get; set; }

        public int Intake_Assessment_Id { get; set; }
        [Display(Name = "Education Summary")]
        public string Education_Sammary { get; set; }

        [Display(Name = "Offense Summary")]
        public string Offence_Sammary { get; set; }
        [Display(Name = "Victim Summary")]
        public string Victim_Sammary { get; set; }

        public int Created_By { get; set; }

        public DateTime Date_Created { get; set; }

        public int Modified_By { get; set; }

        public DateTime Date_Modified { get; set; }

        #endregion

        #region Presentence Details
        public int Presentence_Id { get; set; }

        [Display(Name = "Referal Court")]
        public int? Court_id { get; set; }
        [Display(Name = "Date Recieved")]
        public DateTime? Date_Request_Received { get; set; }
        [Display(Name = "Court Appearance Date")]
        public DateTime? Court_Appearance_Date { get; set; }
        [Display(Name = "Date Report Submited to Court")]
        public DateTime? Date_Report_Submitted_To_Court { get; set; }
        [Display(Name = "Reason for not Submission")]
        public string Reasons_For_Non_Submission { get; set; }

        [Display(Name = "Descussion")]
        public string Sentencing_Options { get; set; }

        [Display(Name = "Community Based Options")]
        public int? Community_Based_Options_Id { get; set; }
        [Display(Name = "Restorective Justice Option")]
        public int? Restorective_Justice_Option_Id { get; set; }
        [Display(Name = "Programme Type")]
        public int? Programme_Type_Id { get; set; }
        [Display(Name = "Programme")]
        public int? Programme_Id { get; set; }
        [Display(Name = "Fine or Alternatives To Fine")]
        public bool Fine_or_Alternatives_To_Fine { get; set; }
        [Display(Name = "Fine Alternatives Fine Comments")]
        public string Fine_Alternatives_Fine_Comments { get; set; }
        [Display(Name = "Suspended Postponed Sentence")]
        public int? Suspended_Postponed_Sentence_Id { get; set; }
        [Display(Name = "Commital Treatment Centre")]
        public bool Commital_Treatment_Centre { get; set; }
        [Display(Name = "Treatment Center Name")]
        public string Center_Name { get; set; }

        [Display(Name = "CYCC Center Name")]
        public string CYCCCenter_Name { get; set; }
        [Display(Name = "Period Commital Treatment Centre From")]
        public DateTime? Period_Commital_Treatment_Centre_From { get; set; }
        [Display(Name = "Period Commital Treatment Centre To")]
        public DateTime? Period_Commital_Treatment_Centre_To { get; set; }
        [Display(Name = " Compulsory Residence CYCC")]
        public bool  Compulsory_esidence_CYCC { get; set; }
        [Display(Name = "Compulsory Residence CYCC From")]
        public DateTime? Compulsory_esidence_CYCC_From { get; set; }
        [Display(Name = "Compulsory Residence CYCC To")]
        public DateTime? Compulsory_esidence_CYCC_To { get; set; }

        [Display(Name = "Imprisoment")]
        public bool Imprisoment { get; set; }

        [Display(Name = "Imprisoment Type")]
        public int? Imprisoment_Id { get; set; }
        [Display(Name = "Imprisomen From")]
        public DateTime? Imprisomen_From { get; set; }
        [Display(Name = "Imprisomen To")]
        public DateTime? Imprisomen_To { get; set; }
        [Display(Name = "Department")]
        public int? Department_Id { get; set; }

        [Display(Name = "Province")]
        public int Province_Id { get; set; }

        [Display(Name = "District")]
        public int District_Id { get; set; }
        



        public string SelectCommunityBasedOptionsDetails { get; set; }
        public virtual ICollection<PCMCommunityBasedOptionsLookup> CommunityBasedOptions_List { get; set; }

        public string SelectSupendedPostponedSentenceDetails { get; set; }
        public virtual ICollection<PCMSupendedPostponedSentenceLookup> SupendedPostponedSentence_List { get; set; }


        public string SelectCaseStatusDetails { get; set; }
        public virtual ICollection<PCMCaseStatusLookup> CaseStatus_List { get; set; }

        public string SelectRestorectiveJusticeDetails { get; set; }
        public virtual ICollection<PCMRestorectiveJusticeLookup> RestorectiveJustice_List { get; set; }

        public string SelectProgrammeTypeDetails { get; set; }
        public virtual ICollection<PCMProgrammeTypeLookup> ProgrammeType_List { get; set; }

        public string SelectProgrammeDetails { get; set; }
        public virtual ICollection<PCMProgrammeLookup> Programme_List { get; set; }

        public string SelectImprisomentDetails { get; set; }
        public virtual ICollection<PCMImprisomentLookup> Imprisoment_List { get; set; }

        public string SelectDepartmentDetails { get; set; }
        public virtual ICollection<PCMDepartmentLookup> Department_List { get; set; }

        public virtual ICollection<CourtLookup> Court_List { get; set; }

        public virtual ICollection<ProvinceLookupPCM> Province_List { get; set; }
        public virtual ICollection<DistrictLookupPCM> District_List { get; set; }


        #endregion

        #region Presentence Outcome
        public int Sentence_Id { get; set; }
        [Display(Name = "Court Date")]
        public DateTime? Court_Date { get; set; }

        [Display(Name = "Reason for Remand")]
        public string Reason_for_Remand { get; set; }
        [Display(Name = "Next Court Date")]
        public DateTime? NextCourtDate { get; set; }

        [Display(Name = "Descussion of Court Outcome")]
        public string Court_Outcome { get; set; }

        [Display(Name = "Case Status")]
        public int? PCM_Case_Status_Id { get; set; }

        
        #endregion

        #region Dropdowns

        public class CommunityBasedOptionsPCM
        {
            public int? SelectCommunityBasedOptionsDetails { get; set; }
            public IEnumerable<PCMCommunityBasedOptionsLookup> CommunityBasedOptions_List { get; set; }
        }

        public class PCMCommunityBasedOptionsLookup
        {
            public int? Community_Based_Options_Id { get; set; }
            public string Description { get; set; }

        }


        public class SupendedPostponedSentencePCM
        {
            public int? SelectSupendedPostponedSentenceDetails { get; set; }
            public IEnumerable<PCMSupendedPostponedSentenceLookup> SupendedPostponedSentence_List { get; set; }
        }

        public class PCMSupendedPostponedSentenceLookup
        {
            public int? Supended_Postponed_Sentence_Id { get; set; }
            public string Description { get; set; }

        }

        public class CaseStatusPCM
        {
            public int? SelectCaseStatusDetails { get; set; }
            public IEnumerable<PCMCaseStatusLookup> CaseStatus_List { get; set; }
        }

        public class PCMCaseStatusLookup
        {
            public int? PCM_Case_Status_Id { get; set; }
            public string Description { get; set; }


     
        }


        public class RestorectiveJusticePCM
        {
            public int? SelectRestorectiveJusticeDetails { get; set; }
            public IEnumerable<PCMRestorectiveJusticeLookup> RestorectiveJustice_List { get; set; }
        }

        public class PCMRestorectiveJusticeLookup
        {
            public int? Restorective_Justice_Option_Id { get; set; }
            public string Description { get; set; }




        }

        public class ProgrammeTypePCM
        {
            public int? SelectProgrammeTypeDetails { get; set; }
            public IEnumerable<PCMProgrammeTypeLookup> ProgrammeType_List { get; set; }
        }

        public class PCMProgrammeTypeLookup
        {
            public int? Programme_Type_Id { get; set; }
            public string Description { get; set; }
            
        }

        public class ProgrammePCM
        {
            public int? SelectProgrammeDetails { get; set; }
            public IEnumerable<PCMProgrammeLookup> Programme_List { get; set; }
        }

        public class PCMProgrammeLookup
        {
            public int? Programme_Id { get; set; }
            public string Description { get; set; }

        }

        public class ImprisomentPCM
        {
            public int? SelectImprisomentDetails { get; set; }
            public IEnumerable<PCMImprisomentLookup> Imprisoment_List { get; set; }
        }

        public class PCMImprisomentLookup
        {
            public int? Imprisoment_Id { get; set; }
            public string Description { get; set; }

        }


        public class DepartmentPCM
        {
            public int? SelectDepartmentDetails { get; set; }
            public IEnumerable<PCMDepartmentLookup> Department_List { get; set; }
        }

        public class PCMDepartmentLookup
        {
            public int? Department_Id { get; set; }
            public string Description { get; set; }

        }


        #endregion

    }
}
