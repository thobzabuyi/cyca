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
    
    public partial class ACM_IDPMeetingPersonPresence
    {
        public int IDPMeetingPersonPresence_Id { get; set; }
        public int Person_Id { get; set; }
        public int IndividualDevelopmentPlan_Id { get; set; }
        public Nullable<System.DateTime> DateOfAssessmentMeeting { get; set; }
        public Nullable<int> AttendendedMeeting { get; set; }
    
        public virtual ACM_YesNoOption ACM_YesNoOption { get; set; }
        public virtual Person int_Person { get; set; }
    }
}
