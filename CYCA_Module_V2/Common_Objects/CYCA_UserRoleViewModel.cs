using Common_Objects.Models;
using Common_Objects.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CYCA_Module_V2.Common_Objects
{
    public class CYCA_UserRoleViewModel
    {
    }
    public class CYCA_CareWorkerViewModel
    {
        public List<CYCAChildAllocationViewModel> children { get; set; }
        public List<CYCAChildAllocationViewModel> childrenTransferByMe { get; set; }
        public List<CYCAChildAllocationViewModel> childrenTransferToMe { get; set; }
        public int InboxCount { get; set; }
        public int TransferCount { get; set; }
        [Display(Name ="Decline Reason")]
        [Required(ErrorMessage ="This field is required")]
        public string Decline_Reason { get; set; }
        public int Transfer_Id { get; set; }
        public CYCAChildAllocationViewModel Childmodel { get; set; }
    }
    public class CYCA_CenterManagerViewModel
    {
        public List<CYCAChildAllocationViewModel> children { get; set; }
        public List<CYCAChildAllocationViewModel> childrenTransferByMe { get; set; }
        public List<CYCAChildAllocationViewModel> childrenTransferToMe { get; set; }
        public int TransferCount { get; set; }
        [Display(Name = "Decline Reason")]
        [Required(ErrorMessage = "This field is required")]
        public string Decline_Reason { get; set; }
        public int Transfer_Id { get; set; }
    }
    public class CYCA_CenterManagerRightViewModel
    {
        public List<TeamLeader> TeamLeaders { get; set; }
    }
    public class CYCA_TeamLeaderViewModel
    {
        public List<CYCAChildAllocationViewModel> children { get; set; }
        public List<CYCAChildAllocationViewModel> childrenTransferByMe { get; set; }
        public List<CYCAChildAllocationViewModel> childrenTransferToMe { get; set; }
        public int TransferCount { get; set; }
        [Display(Name = "Decline Reason")]
        [Required(ErrorMessage = "This field is required")]
        public string Decline_Reason { get; set; }
        public int Transfer_Id { get; set; }
    }
    public class CYCA_TeamLeaderRightViewModel
    {
        public List<CareWorker> CareWorkers { get; set; }

    }
    public class TeamLeader
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int FacilityId { get; set; }
        public string Summary { get; set; }
        public string Desciption { get; set; }
        public string img { get; set; }
        public List<CYCAChildAllocationViewModel> children { get; set; }
    }
    public class CareWorker
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int FacilityId { get; set; }
        public string Summary { get; set; }
        public string Desciption { get; set; }
        public List<CYCAChildAllocationViewModel> children { get; set; }
    }

    #region  FACILITY BED SPACE
    public class ProvincialCoordinatorRightViewModel
    {
        public long Request_Id { get; set; }
        public string Sent_By { get; set; }
        public string Created_By { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        [Display(Name = "Subject")]
        public string Request_Subject { get; set; }
        public DateTime? Date_Recieved { get; set; }
        public string Time_Recieved { get; set; }
        public DateTime? Date_Created { get; set; }

        public int? Days_Lapsed { get; set; }
        public int Hours_Lapsed { get; set; }
        public int? Hours { get; set; }
        public int? Outcome_Id { get; set; }
        [DisplayName("Respond")]
        public string outcomeDescription { get; set; }



        public int? TotalFemaleBedSpace { get; set; }
        public int? TotalMalesBedSpace { get; set; }
        public int? AvailableMaleSpace { get; set; }
        public int? AvailableFemaleSpace { get; set; }


        [Display(Name = "Comments")]
        public string Request_Comments { get; set; }

        [Display(Name = "Facility")]
        public int? Facility_Id { get; set; }
        [Display(Name = "Facility")]
        public string SelectFacility { get; set; }
        public string RequestOpenClose { get; set; }
        [Display(Name = "Status")]
        public int? Request_Status_Id { get; set; }
        public string Request_Status_Comments { get; set; }

        [Display(Name = "Bed Request Status")]
        public string selectBedRequestStatus { get; set; }
        public string selectPropationOfficer { get; set; }
        [Display(Name = "Probation Officer Name")]
        public string selectProbationOfficerName { get; set; }
        [Display(Name = "Probation Officer Surname")]
        public string selectProbationOfficerSurname { get; set; }
        [Display(Name = "Probation Officer Tel")]
        public string selectProbationOfficerTel { get; set; }
        [Display(Name = "Probation Officer Email")]
        public string selectProbationOfficerEmail { get; set; }
        [Display(Name = "Court")]
        public string courtName { get; set; }
        [Display(Name = "Province")]
        public string selectProvince { get; set; }
        [Display(Name = "Child Name")]
        public string selectClientName { get; set; }
        [Display(Name = "Child Gender")]
        public string selectClientGender { get; set; }
        [Display(Name = "Offence Details")]
        public string selectOffenceDetails { get; set; }
        [Display(Name = "Request Status")]
        public string selectRequestStatus { get; set; }
        [Display(Name = "Admission Type")]
        public string selectAdmissionType { get; set; }
        [Display(Name = "Facility Name")]
        public string selectFacilityName { get; set; }
        [Display(Name = "Facility contact No")]
        public string selectFaciltyTel { get; set; }
        [Display(Name = "Facility Manager Email")]
        public string selectFacilityManagerEmail { get; set; }
        [Display(Name = "Reply Date")]
        public string Date_Closed { get; set; }
        [Display(Name = "Reply Time")]
        public string Time_Replied { get; set; }


        public int Province_Id { get; set; }
    }

    #endregion

}