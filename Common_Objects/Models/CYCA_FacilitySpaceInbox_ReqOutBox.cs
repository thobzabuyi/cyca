//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common_Objects.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CYCA_FacilitySpaceInbox_ReqOutBox
    {
        public long IncomingReqID { get; set; }
        public Nullable<int> SenderId { get; set; }
        public Nullable<int> PcmChildID { get; set; }
        public Nullable<int> OffenseId { get; set; }
        public string ReqSubject { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public string CourtName { get; set; }
        public Nullable<System.DateTime> DateRecieved { get; set; }
        public string TimeRecieved { get; set; }
        public Nullable<int> RequestedFemale { get; set; }
        public Nullable<int> RequestedMale { get; set; }
        public string ReqComments { get; set; }
        public Nullable<int> FacilityID { get; set; }
        public Nullable<int> NumberOfBedsDeclined { get; set; }
        public Nullable<int> NumberOfBedsApproved { get; set; }
        public Nullable<int> ApprovedFemale { get; set; }
        public Nullable<int> ApprovedMale { get; set; }
        public string ApprovedComments { get; set; }
        public Nullable<System.DateTime> ReplyDate { get; set; }
        public string ReplyTime { get; set; }
        public Nullable<int> RequestStatus { get; set; }
        public string RequestStatusComments { get; set; }
        public string RequestOpenClose { get; set; }
        public Nullable<System.DateTime> DateClosed { get; set; }
        public Nullable<int> StaffIDAttendedReq { get; set; }
        public Nullable<int> Intake_Assessment_Id { get; set; }
        public Nullable<int> NumberOfBedsRequest { get; set; }
        public Nullable<int> AdmissionTypeId { get; set; }
    }
}