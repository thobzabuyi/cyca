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
    
    public partial class CPR_OnlineNotification_RequestFeedback
    {
        public int Feedback_Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ReferenceNumber { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<bool> isCompleted { get; set; }
    }
}
